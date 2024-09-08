using FarmingClasses.Other;

Player player = new("Viktor");

player.AddMoney(27);
Console.WriteLine(player);

Calendar calendar = new();

calendar.AddMonths(1);
calendar.AddDays(3);
Console.WriteLine(calendar);
