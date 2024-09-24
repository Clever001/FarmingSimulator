using Spectre.Console;


var table = new Table().Centered();

AnsiConsole.Live(table)
    .Start(ctx => {
        table.AddColumn("Foo");
        ctx.Refresh();
        Thread.Sleep(1000);

        table.AddColumn("Bar");
        ctx.Refresh();
        Thread.Sleep(1000);
    });