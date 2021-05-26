using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("Pog Shooter Upgrades")]
    public float pogRangeUpgrade = 2f;
    public float pogFireRateUpgrade = 0.70f;

    [Header("Jroll Upgrades")]
    public float jrollFireRateUpgrade = 0.5f;
    public int jrollPerStack = 3;

    [Header("Upgraded Ammo")]
    public GameObject pogShooterDoubleDamageProjectile;
    public GameObject jrollBurnSpike;

    [Header("Other")]
    public float sellMultiplier;

    [Header("Game Object Reference")]
    public GameObject upgradeAssets;
    public Text sellAmount;

    private GameObject upgradeUI;
    private GameObject currentTower;
    private Upgrade upgrade;
    private GameObject gameUI;

    private string towerType;
    private int refund;

    void Awake()
    {
        upgradeUI = GameObject.Find("Buttons");
        gameUI = GameObject.Find("GameUI");
        refund = 0;
    }

    void Update()
    {
        currentTower = GameObject.FindGameObjectWithTag("SelectedTower");

        if (currentTower != null && currentTower.GetComponent<PlaceTower>().placedTower)
        {
            towerType = currentTower.GetComponent<Tower>().towerType;

            SetUpgrades(currentTower.GetComponent<Tower>().GetUpgradeInstance());
            ShowUpgradeUI();
            CalculateRefund();

            //Send specific tower and upgrade info to UpgradeAssets to update images and text
            int treeOneLevel = upgrade.GetUpgradeLevel(1);
            int treeTwoLevel = upgrade.GetUpgradeLevel(2);
            int priceOne = GetUpgradePrice(1, treeOneLevel);
            int priceTwo = GetUpgradePrice(2, treeTwoLevel);

            upgradeAssets.GetComponent<UpgradeAssets>().UpdateImagesAndText(towerType, upgrade.GetUpgradeLevel(1), upgrade.GetUpgradeLevel(2), priceOne, priceTwo, refund);
            ToggleUpgradeButton(treeOneLevel, treeTwoLevel);
        }
        else
        {
            HideUpgradeUI();
        }
    }

    private void SetUpgrades(Upgrade upgrade)
    {
        this.upgrade = upgrade;
    }

    private int GetUpgradePrice(int tree, int upgradeLevel)
    {
        if (tree == 1)
        {
            if (upgradeLevel == 1)
            {
                return currentTower.GetComponent<Tower>().upgrade11Price;
            }
            else if (upgradeLevel == 2)
            {
                return currentTower.GetComponent<Tower>().upgrade12Price;
            }
            else
            {
                return 0;
            }
        }
        else if (tree == 2)
        {
            if (upgradeLevel == 1)
            {
                return currentTower.GetComponent<Tower>().upgrade21Price;
            }
            else if (upgradeLevel == 2)
            {
                return currentTower.GetComponent<Tower>().upgrade22Price;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }

    public void UpgradeButton(int tree)
    {
        int upgradeLevel = upgrade.GetUpgradeLevel(tree); //The "1" is the specific tree, in this case, the first tree of upgrades
        int price = GetUpgradePrice(tree, upgradeLevel);

        if (AffordUpgrade(price))
        {
            SubtractMoney(price);
            currentTower.GetComponent<Tower>().price += price; //This adds the upgrade price to the total value of the tower; specifically for the CalculateRefund() function
            UnlockUpgradeForSpecificTower(towerType, tree, upgradeLevel);
        }
    }

    public void UnlockUpgradeForSpecificTower(string towerType, int tree, int upgradeLevel)
    {
        switch (towerType)
        {
            case "Pog Shooter":
                PogShooterUpgrades(tree, upgradeLevel);
                break;
            case "Jroll":
                JrollUpgrades(tree, upgradeLevel);
                break;
            default:
                break;
        }
    }

    public void SellTower()
    {
        gameUI.GetComponent<Player>().money += refund;
        Destroy(currentTower);
    }

    private bool AffordUpgrade(int price)
    {
        if (gameUI.GetComponent<Player>().money - price >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void CalculateRefund()
    {
        refund = (int)(currentTower.GetComponent<Tower>().price * sellMultiplier);
    }

    void ToggleUpgradeButton(int treeOneLevel, int treeTwoLevel)
    {
        if(treeOneLevel > 2) 
        {
            upgradeUI.transform.GetChild(0).GetComponent<Button>().interactable = false;
        }
        else
        {
            upgradeUI.transform.GetChild(0).GetComponent<Button>().interactable = true;
        }
        if(treeTwoLevel > 2)
        {
            upgradeUI.transform.GetChild(1).GetComponent<Button>().interactable = false;
        }
        else
        {
            upgradeUI.transform.GetChild(1).GetComponent<Button>().interactable = true;
        }
    }

    void ShowUpgradeUI()
    {
        upgradeUI.GetComponent<CanvasGroup>().interactable = true;
        upgradeUI.GetComponent<CanvasGroup>().alpha = 1;
    }

    void HideUpgradeUI()
    {
        upgradeUI.GetComponent<CanvasGroup>().interactable = false;
        upgradeUI.GetComponent<CanvasGroup>().alpha = 0;
    }

    /* Upgrade Methods */

    void PogShooterUpgrades(int tree, int upgradeLevel)
    {
        if(tree == 1 && upgradeLevel == 1)
        {
            UpgradeRange(pogRangeUpgrade);
            Debug.Log("Pog Shooter upgrade 1-1");
        }
        if(tree == 1 && upgradeLevel == 2)
        {
            UpgradeProjectile(pogShooterDoubleDamageProjectile);
            Debug.Log("Pog Shooter upgrade 1-2");
        }
        if(tree == 2 && upgradeLevel == 1)
        {
            UpgradeFireRate(pogFireRateUpgrade);
            Debug.Log("Pog Shooter upgrade 2-1");
        }
        if(tree == 2 && upgradeLevel == 2)
        {
            currentTower.GetComponent<SpreadShot>().activated = true;
            Debug.Log("Pog Shooter upgrade 2-2");
        }

        upgrade.IncrementUpgradeLevel(tree);
    }

    void JrollUpgrades(int tree, int upgradeLevel)
    {
        if (tree == 1 && upgradeLevel == 1)
        {
            UpgradeFireRate(jrollFireRateUpgrade);
            Debug.Log("Jroll upgrade 1-1");
        }
        if (tree == 1 && upgradeLevel == 2)
        {
            currentTower.GetComponent<JrollHandler>().activatedStack = true;
            Debug.Log("Jroll upgrade 1-2");
        }
        if (tree == 2 && upgradeLevel == 1)
        {
            
            Debug.Log("Jroll upgrade 2-1");
        }
        if (tree == 2 && upgradeLevel == 2)
        {
            UpgradeProjectile(jrollBurnSpike);
            Debug.Log("Jroll upgrade 2-2");
        }

        upgrade.IncrementUpgradeLevel(tree);
    }

    /* */

    void SubtractMoney(int price)
    {
        gameUI.GetComponent<Player>().money -= price;
    }

    void UpgradeRange(float range)
    {
        currentTower.GetComponent<Tower>().SetRange(range);
    }

    void UpgradeFireRate(float fireRate)
    {
        currentTower.GetComponent<Tower>().SetFireRate(fireRate);
    }

    void UpgradeProjectile(GameObject projectile)
    {
        currentTower.GetComponent<Tower>().SetProjectile(projectile);
    }
}
