using FarmingSimulator;
using Spectre.Console;

var gameBuilder = new GRArgs();
gameBuilder.WriteStartInformation();
gameBuilder.InitAdditionalGameInformation();

var gameRenderer = new GameRenderer(gameBuilder);
await gameRenderer.MainCycle();