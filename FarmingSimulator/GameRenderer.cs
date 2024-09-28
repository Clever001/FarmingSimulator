using FarmingClasses.Builders;
using FarmingClasses.Collections;
using FarmingClasses.Other;
using FarmingClasses.Plants;
using Spectre.Console;
using System;
using System.Collections.Generic;
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
    private AutoMinerBuilder _autoMinerBuilder;
    private VegetableBuilder _vegetableBuilder;
    private FruitBuilder _fruitBuilder;

    public GameRenderer(GRArgs args) {
        ArgumentNullException.ThrowIfNull(args.Player, nameof(args.Player));
        ArgumentNullException.ThrowIfNull(args.Calendar, nameof(args.Calendar));
        ArgumentNullException.ThrowIfNull(args.AutoMiners, nameof(args.AutoMiners));
        ArgumentNullException.ThrowIfNull(args.Garden, nameof(args.Garden));
        ArgumentNullException.ThrowIfNull(args.Inventory, nameof(args.Inventory));
        ArgumentNullException.ThrowIfNull(args.AutoMinerBuilder, nameof(args.AutoMinerBuilder));
        ArgumentNullException.ThrowIfNull(args.VegetableBuilder, nameof(args.VegetableBuilder));
        ArgumentNullException.ThrowIfNull(args.FruitBuilder, nameof(args.FruitBuilder));

        _player = args.Player;
        _calendar = args.Calendar;
        _autoMiners = args.AutoMiners;
        _garden = args.Garden;
        _inventory = args.Inventory;
        _autoMinerBuilder = args.AutoMinerBuilder;
        _vegetableBuilder = args.VegetableBuilder;
        _fruitBuilder = args.FruitBuilder;
    }

    public void MainCycle() {
        while (!_player.IsBankrupt) {
            // ----- Печать календаря ----- //
            AnsiConsole.Write(new Rule("Новый день").Centered());
            var cal = new Spectre.Console.Calendar(_calendar.Year, _calendar.Month);
            cal.HeaderStyle(Style.Parse("cyan bold"));
            cal.AddCalendarEvent(_calendar.Year, _calendar.Month, _calendar.Day);
            cal.HighlightStyle(Style.Parse("green bold"));
            AnsiConsole.Write(cal);

            // ----- Выбор действия ----- //

        }
    }
}
