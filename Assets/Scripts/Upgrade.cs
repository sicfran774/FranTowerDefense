using System.Collections;
using System.Collections.Generic;

public class Upgrade
{
    private int upgradeOneLevel;
    private int upgradeTwoLevel;

    public Upgrade()
    {
        upgradeOneLevel = 1;
        upgradeTwoLevel = 1;
    }

    public int GetUpgradeLevel(int tree)
    {
        if(tree == 1)
        {
            return upgradeOneLevel;
        }
        else if (tree == 2)
        {
            return upgradeTwoLevel;
        }
        else
        {
            return -1;
        }
    }

    public void IncrementUpgradeLevel(int upgradeTree)
    {
        if(upgradeTree == 1)
        {
            upgradeOneLevel++;
        }
        else if(upgradeTree == 2)
        {
            upgradeTwoLevel++;
        }
    }
}
