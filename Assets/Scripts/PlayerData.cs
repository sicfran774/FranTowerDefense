using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string level;
    public int round;
    public int health;
    public int money;
    public bool sandboxMode;

    public PlayerData(Player player)
    {
        level = player.level;
        round = player.round;
        health = player.health;
        money = player.money;
    }
}
