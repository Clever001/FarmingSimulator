﻿using FarmingClasses.Builders;
using FarmingClasses.Collections;
using FarmingClasses.Logger;
using FarmingClasses.Other;
using FarmingClasses.Plants;
using FarmingClasses.Serialization;
using Spectre.Console;

namespace FarmingSimulator;
internal sealed class GRArgs {
    private Logger _logger;
    public Player? Player { get; private set; } = null;
    public FarmingClasses.Other.Calendar? Calendar { get; private set; } = null;
    public List<AutoMiner>? AutoMiners { get; private set; } = null;
    public Garden<Plant>? Garden { get; private set; } = null;
    public Inventory<Plant>? Inventory { get; private set; } = null;
    public Shop? Shop { get; private set; } = null;
    public VegetableBuilder? VegetableBuilder { get; private set; } = null;
    public FruitBuilder? FruitBuilder { get; private set; } = null;
    public ISavesController SavesController { get; private set; }

    public GRArgs(Logger logger, ISavesController savesController) {
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));
        ArgumentNullException.ThrowIfNull(savesController);

        _logger = logger;
        SavesController = savesController;
        _logger.Log("Был создан объект типа GameRendererArguments");
    }

    public void WriteStartInformation() {
        AnsiConsole.Write(new FigletText("Farming Simulator").Centered().Color(Color.Orange1));

        AnsiConsole.Write(new Rule("Some start information").Centered());

        AnsiConsole.MarkupLine("Добро пожаловать в игру [#6BE400]\"Farming Simulator\"[/]!");
        AnsiConsole.MarkupLine("В этой консольной игре вы сможете управлять фермером. У вас будет свой огород и даже свои работники на нем!");
        AnsiConsole.MarkupLine("Перед началом игры вам потребуется заполнить [#6BE400]имя игрока[/].");
        _logger.Log("Была записана стартовая информация о игре");
    }

    public void InitAdditionalGameInformation() {
        AnsiConsole.Write(new Rule("Запуск игры").Centered());

        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
              .Title("Загрузить сохранение игры или начать новую игру?")
              .AddChoices(["Начать новую игру", "Загрузить сохранение"]));

        if (selection == "Начать новую игру" || !LoadSave()) {
            CreatePlayer();
        } else {
            _logger.Log("Конец инициализации игрока.");
            return;
        }

        AnsiConsole.Status()
            .Start("Загрузка...", ctx => {
                Thread.Sleep(1000);
                Calendar = new();
                AnsiConsole.MarkupLine("Календарь создан.");

                Thread.Sleep(1000);
                VegetableBuilder = new();
                FruitBuilder = new();
                AnsiConsole.MarkupLine("Билдеры созданы.");

                Thread.Sleep(1000);
                AutoMiners = new();
                Garden = new();
                Inventory = new();
                Shop = new(GoodBuilder.GetAll());
                AnsiConsole.MarkupLine("Контейнеры созданы.");

                AnsiConsole.MarkupLine("Инициализация завершена.");
            });
        _logger.Log($"Были проинициализированы все классы, необходимые для игры");
    }

    public void CreatePlayer() {
        string name = AnsiConsole.Prompt(new TextPrompt<string>("Введите [bold]имя игрока[/]:"));
        Player = new(name);
        _logger.Log($"Был создан игрок с именем {Player.Name}");
    }

    public bool LoadSave() {
        _logger.Log("Начата загрузка информации о игроке с диска");
        if (SavesController.LoadSaves()) {
            _logger.Log("Успешно обработан файл c сохранениями.");
            AnsiConsole.MarkupLine("[green]Файл с сохранениями загружен успешно.[/]");
        } else {
            _logger.Log("Не удалось загрузить сохранение с диска");
            AnsiConsole.MarkupLine("[red]Не удалось загрузить файл с сохранениями. Придется создать нового игрока.[/]");
            return false;
        }

        if (SavesController.Count == 0 ) {
            _logger.Log("В файле не оказалось сохранений");
            AnsiConsole.MarkupLine("[red]В файле не оказалось сохранений. Придется создать нового игрока.[/]");
            return false;
        }
        var playersTable = new Table().Centered();
        playersTable.AddColumn(new TableColumn("Имя игрока").Centered());
        foreach (string player in SavesController.GetPlayers()) {
            playersTable.AddRow(player);
        }
        AnsiConsole.Write(new Rule("Вывод сохранений").Centered());
        AnsiConsole.Write(playersTable);

        _logger.Log("Игрок выбирает сохранение");

        string selectionName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
              .Title("Какое сохранение загрузить?")
              .AddChoices(SavesController.GetPlayers()));

        _logger.Log($"Игрок выбирал сохранение с именем {selectionName}");

        //string selectedPlayer = SavesController.GetPlayers().First(p => p.Equals(selectionName));
        GameSave save = SavesController[selectionName];

        this.Player = save.Player;
        this.Calendar = save.Calendar;
        this.AutoMiners = save.AutoMiners;
        this.Garden = new Garden<Plant>(save.Garden ?? throw new ArgumentNullException(nameof(save.Garden)));
        this.Inventory = new Inventory<Plant>(save.Inventory ?? throw new ArgumentNullException(nameof(save.Garden)));
        this.Shop = new Shop(save.Shop ?? throw new ArgumentNullException(nameof(save.Garden)));
        this.VegetableBuilder = new();
        this.FruitBuilder = new();

        AnsiConsole.MarkupLine("[green]Сохранение успешно загружено.[/]");
        return true;
    }
}
