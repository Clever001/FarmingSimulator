using FarmingClasses.Builders;
using FarmingClasses.Collections;
using FarmingClasses.Logger;
using FarmingClasses.Other;
using FarmingClasses.Plants;
using Spectre.Console;

namespace FarmingSimulator;
internal sealed class GRArgs {
    private Logger<string> _logger;
    public Player? Player { get; private set; } = null;
    public FarmingClasses.Other.Calendar? Calendar { get; private set; } = null;
    public List<AutoMiner>? AutoMiners { get; private set; } = null;
    public Garden<Plant>? Garden { get; private set; } = null;
    public Inventory? Inventory { get; private set; } = null;
    public Shop? Shop { get; private set; } = null;
    public VegetableBuilder? VegetableBuilder { get; private set; } = null;
    public FruitBuilder? FruitBuilder { get; private set; } = null;

    public GRArgs(Logger<string> logger) {
        _logger = logger;
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

        string name = AnsiConsole.Prompt(new TextPrompt<string>("Введите [bold]имя игрока[/]:"));
        Player = new(name);
        _logger.Log($"Был создан игрок с именем {Player.Name}");

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
}
