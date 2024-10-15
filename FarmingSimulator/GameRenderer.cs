using FarmingClasses.Builders;
using FarmingClasses.Collections;
using FarmingClasses.Logger;
using FarmingClasses.Other;
using FarmingClasses.Plants;
using FarmingClasses.Serialization;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;

namespace FarmingSimulator;
internal sealed class GameRenderer {
    private Logger _logger;
    private Player _player;
    private FarmingClasses.Other.Calendar _calendar;
    private List<AutoMiner> _autoMiners;
    private Garden<Plant> _garden;
    private Inventory _inventory;
    private Shop _shop;
    private VegetableBuilder _vegetableBuilder;
    private FruitBuilder _fruitBuilder;
    private SavesController _savesController;

    public GameRenderer(GRArgs args, Logger logger) {
        ArgumentNullException.ThrowIfNull(args.Player, nameof(args.Player));
        ArgumentNullException.ThrowIfNull(args.Calendar, nameof(args.Calendar));
        ArgumentNullException.ThrowIfNull(args.AutoMiners, nameof(args.AutoMiners));
        ArgumentNullException.ThrowIfNull(args.Garden, nameof(args.Garden));
        ArgumentNullException.ThrowIfNull(args.Inventory, nameof(args.Inventory));
        ArgumentNullException.ThrowIfNull(args.Shop, nameof(args.Shop));
        ArgumentNullException.ThrowIfNull(args.VegetableBuilder, nameof(args.VegetableBuilder));
        ArgumentNullException.ThrowIfNull(args.FruitBuilder, nameof(args.FruitBuilder));
        ArgumentNullException.ThrowIfNull(args.SavesController, nameof(args.SavesController));

        _player = args.Player;
        _calendar = args.Calendar;
        _autoMiners = args.AutoMiners;
        _garden = args.Garden;
        _inventory = args.Inventory;
        _shop = args.Shop;
        _vegetableBuilder = args.VegetableBuilder;
        _fruitBuilder = args.FruitBuilder;
        _savesController = args.SavesController;
        _logger = logger;
        _logger.Log("Был проинициализирован объект GameRenderer");
    }

    public async Task MainCycle() {
        _logger.Log("Начало основного цикла программы");

        _inventory.Add(_vegetableBuilder.GetPotato(), 3);
        for (int i = 0; i != 10; ++i) _garden.Add(_vegetableBuilder.GetPotato(_calendar.CurDay));
        //for (int i = 0; i != 3; ++i) _garden.Add(_vegetableBuilder.GetPotato(_calendar.CurDay));
        //for (int i = 0; i != 4; ++i) _garden.Add(_vegetableBuilder.GetCabbage(_calendar.CurDay));
        var sortTask = _garden.SortAsync();

        while (!_player.IsBankrupt) {
            _logger.Log($"Начался новый день, а именно: {_calendar.CurDay}");

            // ----- Печать календаря ----- //
            AnsiConsole.Write(new Rule("Новый день").Centered());
            var cal = new Spectre.Console.Calendar(_calendar.Year, _calendar.Month);
            cal.HeaderStyle(Style.Parse("cyan bold"));
            cal.AddCalendarEvent(_calendar.Year, _calendar.Month, _calendar.Day);
            cal.HighlightStyle(Style.Parse("green bold"));
            AnsiConsole.Write(cal.Centered());

            var today = _calendar.CurDay;
            while (_calendar.CurDay.Equals(today)) {
                _logger.Log("Игрок начал выбирать игровое действие");
                // ----- Выбор действия ----- //
                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<PlayerAction>()
                        .Title("Какое [green]действие[/] выполнить?")
                        .PageSize(5)
                        .MoreChoicesText("[grey](Двигайтесь вверх или вниз, чтобы увидеть больше вариантов)[/]")
                        .AddChoices(new[] {
                        PlayerAction.ViewGarden, PlayerAction.HarvestCrops, PlayerAction.Plant,
                        PlayerAction.BuySmth, PlayerAction.SellSmth, PlayerAction.ViewInventory, 
                        PlayerAction.SwitchDay, PlayerAction.ViewInfo, PlayerAction.SaveGame, 
                        PlayerAction.ExitGame
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
                    case PlayerAction.BuySmth:
                        BuySmth();break;
                    case PlayerAction.SellSmth:
                        SellSmth();break;
                    case PlayerAction.ViewInventory:
                        ViewInventory();break;
                    case PlayerAction.SwitchDay:
                        SwitchDay();break;
                    case PlayerAction.ViewInfo:
                        ViewInfo();break;
                    case PlayerAction.SaveGame:
                        SaveGame();break;
                    default:
                        _logger.Log("Игрок выбрал завершить игру");
                        AnsiConsole.MarkupLine("[#6BE400]Выход из игры.[/]");
                        await Task.Delay(1000);
                        return;
                }
            }
        }
    }

    public void ViewGarden() {
        _logger.Log("Начало вывода содержимого сада");

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
        _logger.Log("Начало сбора урожая");

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

        _logger.Log("Игроку удалось собрать: ");
        foreach (var kvp in kvpairs) {
            int cnt = (int)(kvp.Value * (1.0 + rnd.NextDouble())) + 1;
            _logger.Log($"{kvp.Key.Name} в количестве {cnt} шт.");
            _inventory.Add(kvp.Key, cnt);
            tableOfGained.AddRow(kvp.Key.Name, cnt.ToString());
        }

        AnsiConsole.Write(tableOfGained.Centered());
    }

    public void Plant() {
        _logger.Log("Начало посадки растений");
        AnsiConsole.Write(new Rule("Посадка растения").Centered());

        string plantName;
        do {
            ViewInventory(printRule: false, printMiners: false, printCapital: false);
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
            _logger.Log($"Игрок решил посадить {plantName} в количестве {cnt} шт.");

            _garden.AddRange(PlantBuilder.GetRangeOfPlants(plant, cnt, _calendar.CurDay));
            _inventory.Remove(plant, cnt);
        } while (plantName != "Ничего не садить");
        _logger.Log("Начало сортировки содержимого огорода");
        _garden.Sort();
        _logger.Log("Содержимое огорода отсортировано");
    } 

    public void BuySmth() {
        _logger.Log("Игрок решил пойти в магазин");

        AnsiConsole.Write(new Rule("Магазин").Centered());

        string selectionName;
        do {
            _logger.Log("Вывод цен");
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
            if (selectionName == "Ничего не покупать") {
                _logger.Log("Выход из магазина");
                continue; 
            }

            var good = GoodBuilder.GetGood(selectionName);
            if (_shop.Buy(good, _player)) {
                if (good is Plant pl) _inventory.Add(pl);
                else if (good is AutoMiner am) _autoMiners.Add(am);
                else throw new ArgumentNullException(nameof(good));
                AnsiConsole.MarkupLineInterpolated($"Товар \"{good.Name}\" был добавлен в инвентарь. Остаточный баланс: {_player.Capital}");
                _logger.Log($"Товар \"{good.Name}\" был добавлен в инвентарь. Остаточный баланс: {_player.Capital}");
            }
            else {
                AnsiConsole.MarkupLineInterpolated($"У вас недостаточно средств для покупки товара.");
            }
        } while (selectionName != "Ничего не покупать");
    }

    public void SellSmth() {
        _logger.Log($"Игрок решил что-нибудь продать.");
        AnsiConsole.Write(new Rule("Продажа").Centered());
        var goods = _inventory.GetSortedInventory().Select(g => new KeyValuePair<string, int>(g.Key.Name, g.Value))
                    .Concat(_autoMiners.GroupBy(x => x.Name).Select(g => new KeyValuePair<string, int>(g.Key, g.Count())));
        string selectionName = string.Empty;
        do {
            if (!goods.Any()) {
                _logger.Log("У игрока нет товаров для продажи.");
                AnsiConsole.MarkupLine("[red]У вас нет товаров для продажи[/]");
                return;
            }
            AnsiConsole.MarkupLineInterpolated($"На данный момент у вас {_player.Capital} денежных едениц в инвентаре.");

            _logger.Log("Вывод цен");
            var tableOfCosts = new Table();
            tableOfCosts.AddColumn(new TableColumn("Название").Centered());
            tableOfCosts.AddColumn(new TableColumn("Цена").Centered());
            foreach (var kvp in _shop) {
                tableOfCosts.AddRow(kvp.Key.Name, (kvp.Value / 2).ToString());
            }
            AnsiConsole.Write(tableOfCosts.Centered());

            if (_inventory.Count == 0 && _autoMiners.Count == 0) {
                _logger.Log("У игрока нету товаров на продажу.");
                AnsiConsole.MarkupLine("У вас нету товаров, которые можно было бы продать.");
                selectionName = "Ничего не продавать";
            } else {
                AnsiConsole.MarkupLine("Вывод товаров, доступных к продаже.");
                ViewInventory(printRule: false, printMiners: true, printCapital: false);

                selectionName = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Что вы хотите приобрести?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Двигайтесь вверх или вниз, чтобы увидеть больше вариантов)[/]")
                        .AddChoices(goods.Select(g => g.Key).Concat(["Ничего не продавать"])));
                if (selectionName == "Ничего не продавать") {
                    _logger.Log("Человек решил ничего не продавать.");
                    continue;
                }
                int maxCnt = goods.First(g => g.Key.Equals(selectionName)).Value;
                int cnt = AnsiConsole.Prompt(
                    new TextPrompt<int>("В каком количестве вы хотите продать товар?")
                        .Validate(n => {
                            if (n <= 0) return Spectre.Console.ValidationResult.Error("Нужно число большее нуля");
                            else if (n > maxCnt) return Spectre.Console.ValidationResult.Error("Столько товаров нет в инвентаре");
                            else return Spectre.Console.ValidationResult.Success();
                        })
                        .DefaultValue(maxCnt));

                var good = GoodBuilder.GetGood(selectionName);
                if (_inventory.Remove(good, cnt) ||
                    _autoMiners.Remove(good as AutoMiner ?? throw new ArgumentOutOfRangeException("Товар не является типом AutoMiner."))) {
                    _logger.Log($"Игрок успешно купил {selectionName} в размере {cnt} шт.");
                    _player.AddMoney(_shop[good] / 2 * cnt);
                    AnsiConsole.MarkupLineInterpolated($"[#6BE400]Вы успешно купил {selectionName} в размере {cnt} шт.[/]");
                    AnsiConsole.MarkupLineInterpolated($"На данный момент у вас {_player.Capital} денежных едений в инвентаре.");
                }
                else throw new ArgumentOutOfRangeException("Не удалось определить тип товара.");
            }
        } while (!selectionName.Equals("Ничего не продавать"));
    }

    public void ViewInventory(bool printRule = true, bool printMiners = true, bool printCapital = true) {
        _logger.Log($"Вывод содержимого инвентаря, PrintRule: {printRule} PrintMiners: {printMiners}");
        if (printRule) AnsiConsole.Write(new Rule("Вывод инвентаря").Centered());
        if (printCapital) AnsiConsole.MarkupLineInterpolated($"На данный момент у вас {_player.Capital} денежных едениц в инвентаре.");
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
        _logger.Log("Вывод справочной информации");

        AnsiConsole.Write(new Rule("Просмотр информации о растениях и работниках").Centered());
        AnsiConsole.MarkupLine("Информация о растениях");
        var plants = PlantBuilder.GetAll();

        var plantsTable = new Table();
        plantsTable.AddColumn(new TableColumn("Растение").Centered());
        plantsTable.AddColumn(new TableColumn("Описание").LeftAligned());
        foreach (Plant plant in plants) { plantsTable.AddRow(plant.Name, plant.Description); }
        plantsTable.Border = TableBorder.Minimal;
        AnsiConsole.Write(plantsTable.Centered());

        AnsiConsole.MarkupLine("Информация о работниках");
        var miners = AutoMinerBuilder.GetAll();

        var minersTable = new Table();
        minersTable.AddColumn(new TableColumn("Работник").Centered());
        minersTable.AddColumn(new TableColumn("Описание").LeftAligned());
        foreach (AutoMiner miner in miners) { minersTable.AddRow(miner.Name, miner.Description); }
        minersTable.Border = TableBorder.Minimal;
        AnsiConsole.Write(minersTable.Centered());
    }

    public void SwitchDay() {
        _logger.Log("Смена дня игроком");

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

    public void SaveGame() {
        GameSave save = new(_calendar, _autoMiners, _garden, _inventory, _shop, _player);
        _savesController.SaveGame(_player.Name, save);
        AnsiConsole.MarkupLineInterpolated($"[green]Игра успешно сохранена.[/] Игрок: {_player.Name}.");
    }
}

internal enum PlayerAction {
    [Display(Name = "Посмотреть содержимое огорода")]
    ViewGarden,
    [Display(Name = "Собрать урожай")]
    HarvestCrops,
    [Display(Name = "Посадить растения")]
    Plant,
    [Display(Name = "Купить что-нибудь в магазине")]
    BuySmth,
    [Display(Name = "Продать что-нибудь в магазине")]
    SellSmth,
    [Display(Name = "Посмотреть инвентарь")]
    ViewInventory,
    [Display(Name = "Промотать время")]
    SwitchDay,
    [Display(Name = "Показать информацию о растениях и работниках")]
    ViewInfo,
    [Display(Name = "Сохранить игру")]
    SaveGame,
    [Display(Name = "Закончить игру")]
    ExitGame
}