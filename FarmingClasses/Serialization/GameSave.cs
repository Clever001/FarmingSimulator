using FarmingClasses.Builders;
using FarmingClasses.Collections;
using FarmingClasses.Other;
using FarmingClasses.Plants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace FarmingClasses.Serialization;
public class GameSave {
    public FarmingClasses.Other.Calendar Calendar { get; init; }
    public List<AutoMiner> AutoMiners { get; init; }
    public List<Plant> Garden { get; init; }
    public List<KeyValuePair<Plant, int>> Inventory { get; init; }
    public List<KeyValuePair<IBuyable, int>> Shop { get; init; }
    public Player Player { get; init; }

    [JsonConstructor]
    public GameSave(Calendar calendar, List<AutoMiner> autoMiners, List<Plant> garden, List<KeyValuePair<Plant, int>> inventory, List<KeyValuePair<IBuyable, int>> shop, Player player) {
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

    public GameSave(Calendar calendar, List<AutoMiner> autoMiners, Garden<Plant> garden, Inventory<Plant> inventory, Shop shop, Player player) {
        ArgumentNullException.ThrowIfNull(autoMiners, nameof(autoMiners));
        ArgumentNullException.ThrowIfNull(garden, nameof(garden));
        ArgumentNullException.ThrowIfNull(inventory, nameof(inventory));
        ArgumentNullException.ThrowIfNull(calendar, nameof(calendar));
        ArgumentNullException.ThrowIfNull(shop, nameof(shop));
        ArgumentNullException.ThrowIfNull(player, nameof(player));

        Calendar = calendar;
        AutoMiners = autoMiners;
        Garden = garden.ToList();
        Inventory = inventory.GetSortedInventory().ToList();
        Shop = shop.ToList();
        Player = player;
    }
}
