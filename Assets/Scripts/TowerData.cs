using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerData
{
    public string type;
    public int price;
    public float[] position;
    public int[] upgrade;

    public TowerData(Tower tower)
    {
        type = tower.towerType;
        price = tower.price;

        position = new float[2];
        upgrade = new int[2];

        position[0] = tower.transform.position.x;
        position[1] = tower.transform.position.y;

        upgrade[0] = tower.GetUpgradeInstance().GetUpgradeLevel(1);
        upgrade[1] = tower.GetUpgradeInstance().GetUpgradeLevel(2);
    }
}
