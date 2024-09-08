using FarmingClasses.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmingClassesTests;

public class PlayerTests {
    [Fact]
    public void TestPlayer() {
        Player player = new("Viktor");
        int curMoney = player.Capital;


        Assert.Equal("Viktor", player.Name);
        Assert.True(curMoney >= 0);

        player.AddMoney(100);
        curMoney += 100;

        Assert.Equal(curMoney, player.Capital);

        Assert.True(player.PayMoney(150));
        curMoney -= 150;
        Assert.Equal(curMoney, player.Capital);

        Assert.False(player.PayMoney(150));
        Assert.Equal(curMoney, player.Capital);
    }
}

