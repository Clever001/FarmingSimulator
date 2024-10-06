using FarmingClasses.Logger;
using FarmingSimulator;

var logger = new Logger<string>();

using var fileLog = new FileLogOutput("my_log.txt", 1024);
logger.LogEvent += fileLog.WriteLog;

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