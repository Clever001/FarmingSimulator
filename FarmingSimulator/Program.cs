using FarmingClasses.Config;
using FarmingClasses.Logger;
using FarmingSimulator;
using Spectre.Console;

var config = ConfigInfo.UnserializeConfig();
if (config is null) {
    AnsiConsole.MarkupLine("[red]Не был проинициализирован конфигурационный файл.[/]");
    return;
}

var logger = new Logger();

FileLogOutput? fileLog;
if (config.TypeLog) {
    fileLog = new FileLogOutput(config.LogFilePath, 1024);
    logger.LogEvent += fileLog.WriteLog;
}
else fileLog = null;

#if DEBUG
using var consoleLog = new ConsoleLogOutput();
logger.LogEvent += consoleLog.WriteLog;
#endif

logger.Log("Игра была запущена");

var gameBuilder = new GRArgs(logger);
gameBuilder.WriteStartInformation();
gameBuilder.InitAdditionalGameInformation();

var gameRenderer = new GameRenderer(gameBuilder, logger);
await gameRenderer.MainCycle();
fileLog?.Dispose();