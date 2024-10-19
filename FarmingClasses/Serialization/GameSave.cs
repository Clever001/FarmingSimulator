using FarmingClasses.Builders;
using FarmingClasses.Collections;
using FarmingClasses.Other;
using FarmingClasses.Plants;
using System;
using System.Collections.Generic;

namespace FarmingClasses.Serialization;
public class GameSave {
    public FarmingClasses.Other.Calendar Calendar { get; init; }
    public List<AutoMiner> AutoMiners { get; init; }
    public Garden<Plant> Garden { get; init; }
    public Inventory<Plant> Inventory { get; init; }
    public Shop Shop { get; init; }
    public Player Player { get; init; }

    public GameSave(Calendar calendar, List<AutoMiner> autoMiners, Garden<Plant> garden, Inventory<Plant> inventory, Shop shop, Player player) {
        ArgumentNullException.ThrowIfNull(autoMiners, nameof(autoMiners));
        ArgumentNullException.ThrowIfNull(garden, nameof(garden));
        ArgumentNullException.ThrowIfNull(inventory, nameof(inventory));
        ArgumentNullException.ThrowIfNull(calendar, nameof(calendar));
        ArgumentNullException.ThrowIfNull(shop, nameof(shop));
        ArgumentNullException.ThrowIfNull(player, nameof(player));

        Calendar = calendar;
        AutoMiners = autoMiners;
        Garden = garden;
        Inventory = inventory;
        Shop = shop;
        Player = player;
    }
}
