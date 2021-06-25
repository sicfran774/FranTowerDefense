using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeManager : MonoBehaviour
{
    [Header("Pog Shooter Upgrades")]
    public float pogRangeUpgrade = 2f;
    public float pogFireRateUpgrade = 0.70f;

    [Header("Jroll Upgrades")]
    public float jrollFireRateUpgrade = 0.5f;
    public int jrollPerStack = 3;

    [Header("Juuls Upgrades")]
    public float juulsFireRateUpgrade = 0.1f;

    [Header("Coopa Troopa Upgrades")]
    public float coopaLongerFlameDuration = 3f;
    public float coopaUpgradedCooldown = 1f;
    public int coopaTickAmountUpgrade = 5;
    public float coopaTickIntervalUpgrade = 0.5f;
    public float coopaLongestFlameDuration = 4f;
    
    [Header("Upgraded Ammo")]
    public GameObject pogShooterDoubleDamageProjectile;
    public GameObject jrollBurnSpike;
    public GameObject juulCubeLongerSlow;
    public GameObject juulCubeDamage;
    public GameObject juulCubeBoth;

    [Header("Other")]
    public float sellMultiplier;

    [Header("Game Object Reference")]
    public UpgradeAssets upgradeAssets;
    public Text sellAmount;

    private GameObject upgradeUI;
    private GameObject currentTower;
    private Upgrade upgrade;
    private GameObject gameUI;

    private string towerType;
    private int refund;
    private List<GameObject> towers;
    public bool enemiesAlreadyFrozen;

    void Awake()
    {
        upgradeUI = GameObject.Find("Buttons");
        gameUI = GameObject.Find("GameUI");
        refund = 0;

        towers = new List<GameObject>();
    }

    void Update()
    {
        currentTower = GameObject.FindGameObjectWithTag("SelectedTower");

        //Debug.Log(string.Join(" ", towers));

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

            upgradeAssets.UpdateImagesAndText(currentTower, upgrade.GetUpgradeLevel(1), upgrade.GetUpgradeLevel(2), priceOne, priceTwo, refund);
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
                return currentTower.GetComponent<Tower>().upgrade1_1Price;
            }
            else if (upgradeLevel == 2)
            {
                return currentTower.GetComponent<Tower>().upgrade1_2Price;
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
                return currentTower.GetComponent<Tower>().upgrade2_1Price;
            }
            else if (upgradeLevel == 2)
            {
                return currentTower.GetComponent<Tower>().upgrade2_2Price;
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

        if(upgradeLevel == 4)
        {
            ActivateAbility();
        }
        else if (AffordUpgrade(price))
        {
            SubtractMoney(price);
            currentTower.GetComponent<Tower>().price += price; //This adds the upgrade price to the total value of the tower; specifically for the CalculateRefund() function
            UnlockUpgradeForSpecificTower(towerType, tree, upgradeLevel);
        }
    }

    void ActivateAbility()
    {
        switch (towerType)
        {
            case "Jroll":
                Debug.Log("Activated jroll ability");
                StartCoroutine(currentTower.GetComponent<JrollHandler>().RapidSpikes());
                break;
            case "Juuls":
                Debug.Log("Activated juuls ablility");
                StartCoroutine(currentTower.GetComponent<JuulsHandler>().FreezeAllEnemies());
                break;
            default:
                break;
        }

        StartCoroutine(currentTower.GetComponent<Tower>().AbilityCooldown());
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
            case "Juuls":
                JuulsUpgrades(tree, upgradeLevel);
                break;
            case "Coopa Troopa":
                CoopaUpgrades(tree, upgradeLevel);
                break;
            default:
                break;
        }
    }

    public void SellTower()
    {
        gameUI.GetComponent<Player>().money += refund;
        currentTower.GetComponent<Tower>().StopAllCoroutines();
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
        if(treeOneLevel > 2 && treeOneLevel != 4 || currentTower.GetComponent<Tower>().abilityOnCooldown) 
        {
            upgradeUI.transform.GetChild(0).GetComponent<Button>().interactable = false;
        }
        else
        {
            upgradeUI.transform.GetChild(0).GetComponent<Button>().interactable = true;
        }
        if(treeTwoLevel > 2 && treeTwoLevel != 4)
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
            upgrade.IncrementUpgradeLevel(1);
            Debug.Log("Jroll upgrade 1-2");
        }
        if (tree == 2 && upgradeLevel == 1)
        {
            UpgradeProjectile(jrollBurnSpike);
            Debug.Log("Jroll upgrade 2-1");
        }
        if (tree == 2 && upgradeLevel == 2)
        {
            currentTower.GetComponent<JrollHandler>().SetJrollPerStack(jrollPerStack);
            currentTower.GetComponent<JrollHandler>().activatedStack = true;
            Debug.Log("Jroll upgrade 2-2");
        }

        upgrade.IncrementUpgradeLevel(tree);
    }

    void JuulsUpgrades(int tree, int upgradeLevel)
    {
        if (tree == 1 && upgradeLevel == 1)
        {
            EitherOrBothJuulCube(tree);
            Debug.Log("Juuls upgrade 1-1");
        }
        if (tree == 1 && upgradeLevel == 2)
        {
            upgrade.IncrementUpgradeLevel(1);
            Debug.Log("Juuls upgrade 1-2");
        }
        if (tree == 2 && upgradeLevel == 1)
        {
            EitherOrBothJuulCube(tree);
            Debug.Log("Juuls upgrade 2-1");
        }
        if (tree == 2 && upgradeLevel == 2)
        {
            UpgradeFireRate(juulsFireRateUpgrade);
            Debug.Log("Juuls upgrade 2-2");
        }

        upgrade.IncrementUpgradeLevel(tree);
    }

    void EitherOrBothJuulCube(int tree)
    {
        if (upgrade.GetUpgradeLevel(1) > 1 || upgrade.GetUpgradeLevel(2) > 1)
        {
            UpgradeProjectile(juulCubeBoth);
        }
        else if (tree == 1)
        {
            UpgradeProjectile(juulCubeDamage);
        }
        else
        {
            UpgradeProjectile(juulCubeLongerSlow);
        }
    }

    void CoopaUpgrades(int tree, int upgradeLevel)
    {
        if (tree == 1 && upgradeLevel == 1)
        {
            currentTower.GetComponent<CoopaHandler>().flameDuration = coopaLongerFlameDuration;
            Debug.Log("Coopa upgrade 1-1");
        }
        if (tree == 1 && upgradeLevel == 2)
        {
            currentTower.GetComponent<CoopaHandler>().flameDuration = coopaLongestFlameDuration;
            Debug.Log("Coopa upgrade 1-2");
        }
        if (tree == 2 && upgradeLevel == 1)
        {
            currentTower.GetComponent<CoopaHandler>().cooldownDuration = coopaUpgradedCooldown;
            currentTower.GetComponent<CoopaHandler>().tickInterval = coopaTickIntervalUpgrade;
            Debug.Log("Coopa upgrade 2-1");
        }
        if (tree == 2 && upgradeLevel == 2)
        {
            currentTower.GetComponent<CoopaHandler>().tickAmount = coopaTickAmountUpgrade;
            Debug.Log("Coopa upgrade 2-2");
        }

        upgrade.IncrementUpgradeLevel(tree);
    }

    /* */

    public void ActivateAllJrollAbilities()
    {
        towers.Clear();
        UpdateList();
        foreach (GameObject tower in towers)
        {
            if (!tower.GetComponent<Tower>().abilityOnCooldown)
            {
                StartCoroutine(tower.GetComponent<JrollHandler>().RapidSpikes());
                StartCoroutine(tower.GetComponent<Tower>().AbilityCooldown());
            }
        }
        //StartCoroutine(DisableButton(EventSystem.current.currentSelectedGameObject));
    }

    void UpdateList()
    {
        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower"))
        {
            if (tower.GetComponent<JrollHandler>() != null && tower.GetComponent<PlaceTower>().placedTower && tower.GetComponent<Tower>().GetUpgradeInstance().GetUpgradeLevel(1) == 4)
            {
                towers.Add(tower);
            }
        }
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

    public IEnumerator DisableButton(GameObject button = null)
    {
        if (button != null)
        {
            button.GetComponent<Button>().interactable = false;
            yield return new WaitForSeconds(0.5f);
            button.GetComponent<Button>().interactable = true;
        }
    }
}
