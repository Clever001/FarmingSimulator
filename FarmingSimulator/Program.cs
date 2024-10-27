using FarmingClasses.Config;
using FarmingClasses.Logger;
using FarmingClasses.Serialization;
using FarmingSimulator;
using Spectre.Console;

// Config init
var config = ConfigInfo.UnserializeConfig();
if (config is null) {
    AnsiConsole.MarkupLine("[red]Не был проинициализирован конфигурационный файл.[/]");
    return;
}

// Logging specification
var logger = new Logger();

StreamLogOutput? fileLog;
using FileStream outStream = new(config.LogFilePath, FileMode.Create, FileAccess.Write);
if (config.TypeLog) {
    fileLog = new StreamLogOutput(outStream, 5120);
    logger.LogEvent += fileLog.WriteLog;
}
else fileLog = null;

#if DEBUG
    using var consoleLog = new ConsoleLogOutput();
    logger.LogEvent += consoleLog.WriteLog;
#endif

// serialization specification
ISavesController? savesCont = null;
if (config.SerializationFormat == "json") { savesCont = new JsonSaver("game_saves"); }
else if (config.SerializationFormat == "xml") { savesCont = new XmlSaver("game_saves"); }
else {
    AnsiConsole.MarkupLineInterpolated($"[red]Сериализация при помощи вормата \"{config.SerializationFormat}\" не предусмотрена. [/]");
    AnsiConsole.MarkupLine("Просьба изменить формат сериализации на [yellow]\"json\"[/] или [yellow]\"xml\"[/]");
    return;
}

try {
    logger.Log("Игра была запущена");

    var gameBuilder = new GRArgs(logger, savesCont);
    gameBuilder.WriteStartInformation();
    gameBuilder.InitAdditionalGameInformation();

    var gameRenderer = new GameRenderer(gameBuilder, logger);
    await gameRenderer.MainCycle();
} finally {
    fileLog?.Dispose();
}