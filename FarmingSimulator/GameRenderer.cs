using FarmingClasses.Builders;
using FarmingClasses.Collections;
using FarmingClasses.Other;
using FarmingClasses.Plants;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public void MainCycle() {
        _inventory.Add(_vegetableBuilder.GetPotato(), 3);
        for (int i = 0; i != 2; ++i) _garden.Add(_vegetableBuilder.GetCarrot(_calendar.CurDay));
        for (int i = 0; i != 3; ++i) _garden.Add(_vegetableBuilder.GetPotato(_calendar.CurDay));
        for (int i = 0; i != 4; ++i) _garden.Add(_vegetableBuilder.GetCabbage(_calendar.CurDay));
        _garden.Sort();

        while (!_player.IsBankrupt) {
            // ----- Печать календаря ----- //
            AnsiConsole.Write(new Rule("Новый день").Centered());
            var cal = new Spectre.Console.Calendar(_calendar.Year, _calendar.Month);
            cal.HeaderStyle(Style.Parse("cyan bold"));
            cal.AddCalendarEvent(_calendar.Year, _calendar.Month, _calendar.Day);
            cal.HighlightStyle(Style.Parse("green bold"));
            AnsiConsole.Write(cal);

            var today = _calendar.CurDay;
            while (_calendar.CurDay.Equals(today)) {
                // ----- Выбор действия ----- //
                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<PlayerAction>()
                        .Title("Какое [green]действие[/] выполнить?")
                        .PageSize(5)
                        .MoreChoicesText("[grey](Двигайтесь вверх или вниз, чтобы увидеть больше вариантов)[/]")
                        .AddChoices(new[] {
                        PlayerAction.ViewGarden, PlayerAction.HarvestCrops, PlayerAction.ViewShop,
                        PlayerAction.ViewInventory, PlayerAction.SwitchDay
                        })
                        .UseConverter(x => x.GetType()
                                            .GetMember(x.ToString())
                                            .First()
                                            .GetCustomAttributes(false)
                                            .OfType<DisplayAttribute>()
                                            .LastOrDefault()!
                                            .GetName()!
                        ));
                switch (selection) {
                    case PlayerAction.ViewGarden:
                        ViewGarden();
                        break;
                    case PlayerAction.HarvestCrops:
                        HarvestCrops();
                        break;
                    case PlayerAction.ViewShop:
                        ViewShop();
                        break;
                    case PlayerAction.ViewInventory:
                        ViewInventory();
                        break;
                    case PlayerAction.SwitchDay:
                        SwitchDay();
                        break;
                }
                break;
            }
            break;
        }
    }

    public void ViewGarden() {
        AnsiConsole.Write(new Rule("Вывод содержимого огорода").Centered());
        AnsiConsole.MarkupLineInterpolated($"Количество культур в огороде: [green]{_garden.Count}[/].");
        if (_garden.Count > 0) {
            var kvpairs = from culture in _garden
                          group culture by culture.Name into g
                          select new KeyValuePair<string, int>(g.Key, g.Count());

            var table = new Table();
            table.AddColumn(new TableColumn("Название").Centered());
            table.AddColumn(new TableColumn("Количество").Centered());

            foreach (var kvp in kvpairs) {
                table.AddRow(kvp.Key, kvp.Value.ToString());
            }

            table.Border = TableBorder.Minimal;

            table.Width = 50;

            AnsiConsole.Write(table);
        }
    }

    public void HarvestCrops() {

    }

    public void ViewShop() {

    }

    public void ViewInventory() {

    }

    public void SwitchDay() {

    }
}

internal enum PlayerAction {
    [Display(Name = "Посмотреть содержимое огорода")]
    ViewGarden,
    [Display(Name = "Собрать урожай")]
    HarvestCrops,
    [Display(Name = "Посмотреть магазин")]
    ViewShop,
    [Display(Name = "Посмотреть инвентарь")]
    ViewInventory,
    [Display(Name = "Промотать время")]
    SwitchDay
}