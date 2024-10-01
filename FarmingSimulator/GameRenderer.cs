using FarmingClasses.Builders;
using FarmingClasses.Collections;
using FarmingClasses.Other;
using FarmingClasses.Plants;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;

namespace FarmingSimulator;
internal class GameRenderer {
    private Player _player;
    private FarmingClasses.Other.Calendar _calendar;
    private List<AutoMiner> _autoMiners;
    private Garden<Plant> _garden;
    private Inventory _inventory;
    private Shop _shop;
    private AutoMinerBuilder _autoMinerBuilder;
    private VegetableBuilder _vegetableBuilder;
    private FruitBuilder _fruitBuilder;

    public GameRenderer(GRArgs args) {
        ArgumentNullException.ThrowIfNull(args.Player, nameof(args.Player));
        ArgumentNullException.ThrowIfNull(args.Calendar, nameof(args.Calendar));
        ArgumentNullException.ThrowIfNull(args.AutoMiners, nameof(args.AutoMiners));
        ArgumentNullException.ThrowIfNull(args.Garden, nameof(args.Garden));
        ArgumentNullException.ThrowIfNull(args.Inventory, nameof(args.Inventory));
        ArgumentNullException.ThrowIfNull(args.Shop, nameof(args.Shop));
        ArgumentNullException.ThrowIfNull(args.AutoMinerBuilder, nameof(args.AutoMinerBuilder));
        ArgumentNullException.ThrowIfNull(args.VegetableBuilder, nameof(args.VegetableBuilder));
        ArgumentNullException.ThrowIfNull(args.FruitBuilder, nameof(args.FruitBuilder));

        _player = args.Player;
        _calendar = args.Calendar;
        _autoMiners = args.AutoMiners;
        _garden = args.Garden;
        _inventory = args.Inventory;
        _shop = args.Shop;
        _autoMinerBuilder = args.AutoMinerBuilder;
        _vegetableBuilder = args.VegetableBuilder;
        _fruitBuilder = args.FruitBuilder;
    }

    public async Task MainCycle() {
        _inventory.Add(_vegetableBuilder.GetPotato(), 3);
        for (int i = 0; i != 10; ++i) _garden.Add(_vegetableBuilder.GetPotato(_calendar.CurDay));
        //for (int i = 0; i != 3; ++i) _garden.Add(_vegetableBuilder.GetPotato(_calendar.CurDay));
        //for (int i = 0; i != 4; ++i) _garden.Add(_vegetableBuilder.GetCabbage(_calendar.CurDay));
        var sortTask = _garden.SortAsync();

        while (!_player.IsBankrupt) {
            // ----- Печать календаря ----- //
            AnsiConsole.Write(new Rule("Новый день").Centered());
            var cal = new Spectre.Console.Calendar(_calendar.Year, _calendar.Month);
            cal.HeaderStyle(Style.Parse("cyan bold"));
            cal.AddCalendarEvent(_calendar.Year, _calendar.Month, _calendar.Day);
            cal.HighlightStyle(Style.Parse("green bold"));
            AnsiConsole.Write(cal.Centered());

            var today = _calendar.CurDay;
            while (_calendar.CurDay.Equals(today)) {
                // ----- Выбор действия ----- //
                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<PlayerAction>()
                        .Title("Какое [green]действие[/] выполнить?")
                        .PageSize(5)
                        .MoreChoicesText("[grey](Двигайтесь вверх или вниз, чтобы увидеть больше вариантов)[/]")
                        .AddChoices(new[] {
                        PlayerAction.ViewGarden, PlayerAction.HarvestCrops, PlayerAction.Plant,
                        PlayerAction.ViewShop, PlayerAction.ViewInventory, PlayerAction.SwitchDay,
                        PlayerAction.ViewInfo, PlayerAction.ExitGame
                        })
                        .UseConverter(x => x.GetType()
                                            .GetMember(x.ToString())
                                            .First()
                                            .GetCustomAttributes(false)
                                            .OfType<DisplayAttribute>()
                                            .LastOrDefault()!
                                            .GetName()!
                        ));
                await sortTask;
                switch (selection) {
                    case PlayerAction.ViewGarden:
                        ViewGarden();break;
                    case PlayerAction.HarvestCrops:
                        HarvestCrops();break;
                    case PlayerAction.Plant:
                        Plant();break;
                    case PlayerAction.ViewShop:
                        ViewShop();break;
                    case PlayerAction.ViewInventory:
                        ViewInventory();break;
                    case PlayerAction.SwitchDay:
                        SwitchDay();break;
                    case PlayerAction.ViewInfo:
                        ViewInfo();break;
                    default:
                        AnsiConsole.MarkupLine("[#6BE400]Выход из игры.[/]");
                        await Task.Delay(1000);
                        return;
                }
            }
        }
    }

    public void ViewGarden() {
        AnsiConsole.Write(new Rule("Вывод содержимого огорода").Centered());
        AnsiConsole.MarkupLineInterpolated($"Количество культур в огороде: [green]{_garden.Count}[/].");
        if (_garden.Count > 0) {
            var kvpairs = from culture in _garden
                          group culture by culture.Name into g
                          select new KeyValuePair<string, int>(g.Key, g.Count());

            var table = new Table {
                Border = TableBorder.Minimal,
                Width = 50
            };
            AnsiConsole.Live(table).Start(ctx => {
                table.AddColumn(new TableColumn("Название").Centered());
                table.AddColumn(new TableColumn("Количество").Centered());
                ctx.Refresh();
                Thread.Sleep(400);
                foreach (var kvp in kvpairs) {
                    table.AddRow(kvp.Key, kvp.Value.ToString());
                    ctx.Refresh();
                    Thread.Sleep(400);
                }
            });

        }
    }

    public void HarvestCrops() { 

        AnsiConsole.Write(new Rule("Собираем урожай").Centered());

        List<Plant> gainedPlants = new();
        _garden.RemoveIf(plant => plant.IsCollectable(_calendar.CurDay), gainedPlants.Add);

        if (gainedPlants.Count == 0) {
            AnsiConsole.MarkupLine("На данный момент ни одно растение не созрело.");
            return;
        }

        int minersCanHarvest = 0;
        foreach (AutoMiner miner in _autoMiners) { minersCanHarvest += miner.CanCollect; }

        int countOfWaitSteps = Math.Max(0, gainedPlants.Count - minersCanHarvest);

        AnsiConsole.Progress()
            .Start(ctx => {
                var task1 = ctx.AddTask("[green]Собираем урожай[/]", maxValue: countOfWaitSteps);
                while (!ctx.IsFinished) {
                    Thread.Sleep(1000);
                    task1.Increment(1.0);
                }
            });

        var kvpairs = (from plant in gainedPlants
                       group plant by plant.Name into g
                       select new KeyValuePair<Plant, int>(PlantBuilder.GetPlant(g.Key), g.Count())).ToArray();

        AnsiConsole.MarkupLine("В следующей таблице представлены растения, которые были собраны из огорода.");

        var tableOfCollected = new Table();

        tableOfCollected.AddColumn(new TableColumn("Растение").Centered());
        tableOfCollected.AddColumn(new TableColumn("Количество").Centered());

        foreach (var kvp in kvpairs) {
            tableOfCollected.AddRow(kvp.Key.Name, kvp.Value.ToString());
        }

        tableOfCollected.Border = TableBorder.Minimal;
        AnsiConsole.Write(tableOfCollected.Centered());

        AnsiConsole.MarkupLine("Некоторые растения дали больше урожая, чем ожидалось.");

        var rnd = new Random();
        var tableOfGained = new Table();
        tableOfGained.AddColumn(new TableColumn("Растение").Centered());
        tableOfGained.AddColumn(new TableColumn("Количество").Centered());

        foreach (var kvp in kvpairs) {
            int cnt = (int)(kvp.Value * (1.0 + rnd.NextDouble() / 2));
            _inventory.Add(kvp.Key, cnt);
            tableOfGained.AddRow(kvp.Key.Name, cnt.ToString());
        }

        AnsiConsole.Write(tableOfGained.Centered());
    }

    public void Plant() {
        AnsiConsole.Write(new Rule("Посадка растения").Centered());

        string plantName;
        do {
            ViewInventory(printRule: false, printMiners: false);
            var items = _inventory.GetSortedKeys().Select(x => x.Name);

            if (!items.Any()) {
                AnsiConsole.MarkupLine("Нету больше растений, которые можно посадить.");
                return;
            }

            plantName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Какие растения вы хотите посадить?")
                .PageSize(5)
                .MoreChoicesText("[grey](Двигайтесь вверх или вниз, чтобы увидеть больше вариантов)[/]")
                .AddChoices(items.Append("Ничего не садить")));
            if (plantName == "Ничего не садить") continue;
            
            Plant plant = PlantBuilder.GetPlant(plantName);
            int maxCnt = _inventory[plant];
            int cnt = AnsiConsole.Prompt(
                new TextPrompt<int>("Сколько всего растений нужно посадить?")
                    .Validate(n => {
                        if (n <= 0) return Spectre.Console.ValidationResult.Error("Нужно число большее нуля");
                        else if (n > maxCnt) return Spectre.Console.ValidationResult.Error("Столько растений нет в инвентаре");
                        else return Spectre.Console.ValidationResult.Success();
                    }));

            _garden.AddRange(PlantBuilder.GetRangeOfPlants(plant, cnt, _calendar.CurDay));
            _inventory.Remove(plant, cnt);
        } while (plantName != "Ничего не садить");
        _garden.Sort();
    } 

    public void ViewShop() {
        AnsiConsole.Write(new Rule("Магазин").Centered());
        

        string selectionName;
        do {
            AnsiConsole.MarkupLineInterpolated($"На данный момент у вас имеется {_player.Capital} денежных единиц.");

            var tableOfCosts = new Table();
            tableOfCosts.AddColumn(new TableColumn("Название").Centered());
            tableOfCosts.AddColumn(new TableColumn("Цена").Centered());

            foreach (var kvp in _shop) {
                tableOfCosts.AddRow(kvp.Key.Name, kvp.Value.ToString());
            }

            AnsiConsole.Write(tableOfCosts.Centered());

            selectionName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Что вы хотите приобрести?")
                .PageSize(10)
                .MoreChoicesText("[grey](Двигайтесь вверх или вниз, чтобы увидеть больше вариантов)[/]")
                .AddChoices(_shop.GetGoods().Select(x => x.Name).Concat(["Ничего не покупать"])));
            if (selectionName == "Ничего не покупать") continue;

            var good = GoodBuilder.GetGood(selectionName);
            if (_shop.Buy(good, _player)) {
                if (good is Plant pl) _inventory.Add(pl);
                else if (good is AutoMiner am) _autoMiners.Add(am);
                else throw new ArgumentNullException(nameof(good));
                AnsiConsole.MarkupLineInterpolated($"Товар \"{good.Name}\" был добавлен в инвентарь. Остаточный баланс: {_player.Capital}");
            }
            else {
                AnsiConsole.MarkupLineInterpolated($"У вас недостаточно средств для покупки товара.");
            }
        } while (selectionName != "Ничего не покупать");
    }

    public void ViewInventory(bool printRule = true, bool printMiners = true) {
        if (printRule) AnsiConsole.Write(new Rule("Вывод инвентаря").Centered());
        var sortedInventory = _inventory.GetSortedInventory();
        if (sortedInventory.Any()) {
            AnsiConsole.MarkupLine("Ваш инвентарь:");
            var table = new Table();
            table.AddColumn("Предмет");
            table.AddColumn("Количество");
            foreach (var kvp in sortedInventory) { table.AddRow(kvp.Key.Name, kvp.Value.ToString()); }
            AnsiConsole.Write(table.Centered());
        }
        else AnsiConsole.MarkupLine("У вас нет предметов в инвентаре.");
        

        if (printMiners && _autoMiners.Count != 0) {
            AnsiConsole.MarkupLine("Ваши работники:");
            var table = new Table();
            table.AddColumn("Название");
            table.AddColumn("Количество");
            var kvpairs = from miner in _autoMiners
                          group miner by miner.Name into g
                          select new KeyValuePair<string, int>(g.Key, g.Count());
            foreach (var kvp in kvpairs) { table.AddRow(kvp.Key, kvp.Value.ToString()); }
            AnsiConsole.Write(table.Centered());
        } else if (printMiners) {
            AnsiConsole.MarkupLine("У вас нет работников в саду.");
        }
    }

    public void ViewInfo() {
        AnsiConsole.Write(new Rule("Просмотр информации о растениях и работниках").Centered());
        AnsiConsole.MarkupLine("Информация о растениях");
        throw new NotImplementedException("Просмотр информации еще не доступен.");
#warning Доработай просмотр справочной информации.
    }

    public void SwitchDay() {
        AnsiConsole.Write(new Rule("Переключение времени").Centered());
        string periodSelection = AnsiConsole.Prompt(
            new TextPrompt<string>("Вы хотите промотать некоторое количество дней или месяцев?")
            .AddChoices(["Дни", "Месяцы", "Не проматывать время"])
            .DefaultValue("Дни"));
        if (periodSelection == "Не проматывать время") return;

        int cnt = AnsiConsole.Prompt(
            new TextPrompt<int>("Укажите число")
            .Validate(num =>  num > 0 ? Spectre.Console.ValidationResult.Success() : Spectre.Console.ValidationResult.Error("Слишком мало.")));

        if (periodSelection == "Дни") _calendar.AddDays(cnt);
        else _calendar.AddMonths(cnt);
    }
}

internal enum PlayerAction {
    [Display(Name = "Посмотреть содержимое огорода")]
    ViewGarden,
    [Display(Name = "Собрать урожай")]
    HarvestCrops,
    [Display(Name = "Посадить растения")]
    Plant,
    [Display(Name = "Посмотреть магазин")]
    ViewShop,
    [Display(Name = "Посмотреть инвентарь")]
    ViewInventory,
    [Display(Name = "Промотать время")]
    SwitchDay,
    [Display(Name = "Показать информацию о растениях и работниках")]
    ViewInfo,
    [Display(Name = "Закончить игру")]
    ExitGame
}