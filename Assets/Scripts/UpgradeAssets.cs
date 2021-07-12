using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeAssets : MonoBehaviour
{

    [Header("Refund Text")]
    public Text refundText;

    [Header("Price Text")]
    public Text upgradePriceOne;
    public Text upgradePriceTwo;

    [Header("Image GameObject")]
    public Image upgradeImageOne;
    public Image upgradeImageTwo;

    [Header("Upgrade Description")]
    public Text upgradeDescOne;
    public Text upgradeDescTwo;

    [Header("Pog Shooter Images")]
    public Sprite pogShooterImageOneOne;
    public Sprite pogShooterImageOneTwo;
    public Sprite pogShooterImageTwoOne;
    public Sprite pogShooterImageTwoTwo;

    [Header("Jroll Images")]
    public Sprite jrollImageOneOne;
    public Sprite jrollImageOneTwo;
    public Sprite jrollImageTwoOne;
    public Sprite jrollImageTwoTwo;

    [Header("Juuls Images")]
    public Sprite juulsImageOneOne;
    public Sprite juulsImageOneTwo;
    public Sprite juulsImageTwoOne;
    public Sprite juulsImageTwoTwo;

    private GameObject currentTower;

    private GameObject upgradeOneGameObject;
    private GameObject upgradeTwoGameObject;

    void Awake()
    {
        upgradeOneGameObject = GameObject.Find("Upgrade 1");
        upgradeTwoGameObject = GameObject.Find("Upgrade 2");

        upgradeDescOne = upgradeOneGameObject.transform.GetChild(0).GetComponent<Text>();
        upgradeImageOne = upgradeOneGameObject.transform.GetChild(1).GetComponent<Image>();
        upgradePriceOne = upgradeOneGameObject.transform.GetChild(2).GetComponent<Text>();

        upgradeDescTwo = upgradeTwoGameObject.transform.GetChild(0).GetComponent<Text>();
        upgradeImageTwo = upgradeTwoGameObject.transform.GetChild(1).GetComponent<Image>();
        upgradePriceTwo = upgradeTwoGameObject.transform.GetChild(2).GetComponent<Text>();
    }

    public void UpdateImagesAndText(GameObject tower, int treeOneLevel, int treeTwoLevel, int upgradeOnePrice, int upgradeTwoPrice, int refund)
    {
        currentTower = tower;
        string towerType = currentTower.GetComponent<Tower>().towerType;

        refundText.text = refund.ToString();
        upgradeOneGameObject.transform.GetChild(3).gameObject.SetActive(true);
        upgradeTwoGameObject.transform.GetChild(3).gameObject.SetActive(true);

        switch (towerType)
        {
            case "Pog Shooter":
                PogShooterAssets(treeOneLevel, treeTwoLevel, upgradeOnePrice, upgradeTwoPrice);
                break;
            case "Jroll":
                JrollAssets(treeOneLevel, treeTwoLevel, upgradeOnePrice, upgradeTwoPrice);
                break;
            case "Juuls":
                JuulsAssets(treeOneLevel, treeTwoLevel, upgradeOnePrice, upgradeTwoPrice);
                break;
            case "Coopa Troopa":
                CoopaAssets(treeOneLevel, treeTwoLevel, upgradeOnePrice, upgradeTwoPrice);
                break;
            case "Super Fran":
                FranAssets(treeOneLevel, treeTwoLevel, upgradeOnePrice, upgradeTwoPrice);
                break;
            case "Tad Rock":
                RockAssets(treeOneLevel, treeTwoLevel, upgradeOnePrice, upgradeTwoPrice);
                break;
            default:
                break;
        }

        ChangeColorOfText(GameObject.Find("GameUI").GetComponent<Player>().money, upgradeOnePrice, upgradeTwoPrice);

        if(upgradeOnePrice <= 0)
        {
            ZeroPriceOne();
        }
        if (upgradeTwoPrice <= 0)
        {
            ZeroPriceTwo();
        }
    }

    private void ChangeColorOfText(int money, int priceOne, int priceTwo)
    {
        if (money < priceOne)
        {
            upgradePriceOne.color = Color.red;
        }
        else
        {
            upgradePriceOne.color = Color.green;
        }
        if (money < priceTwo)
        {
            upgradePriceTwo.color = Color.red;
        }
        else
        {
            upgradePriceTwo.color = Color.green;
        }
    }
    
    private void PogShooterAssets(int treeOneLevel, int treeTwoLevel, int upgradeOnePrice, int upgradeTwoPrice)
    {
        if (treeOneLevel == 1)
        {
            upgradeDescOne.text = "Longer\nRange";
            upgradeImageOne.sprite = pogShooterImageOneOne;
            upgradePriceOne.text = upgradeOnePrice.ToString();
        }
        else
        {
            upgradeDescOne.text = "Double\nDamage";
            upgradeImageOne.sprite = pogShooterImageOneTwo;
            upgradePriceOne.text = upgradeOnePrice.ToString();
        }
        if (treeTwoLevel == 1)
        {
            upgradeDescTwo.text = "Faster\nPogs";
            upgradeImageTwo.sprite = pogShooterImageTwoOne;
            upgradePriceTwo.text = upgradeTwoPrice.ToString();
        }
        else
        {
            upgradeDescTwo.text = "Spread\nShot";
            upgradeImageTwo.sprite = pogShooterImageTwoTwo;
            upgradePriceTwo.text = upgradeTwoPrice.ToString();
        }
    }

    private void JrollAssets(int treeOneLevel, int treeTwoLevel, int upgradeOnePrice, int upgradeTwoPrice)
    {
        if (treeOneLevel == 1)
        {
            upgradeDescOne.text = "Faster\nJroll Spikes";
            upgradeImageOne.sprite = jrollImageOneOne;
            upgradePriceOne.text = upgradeOnePrice.ToString();
        }
        else if (treeOneLevel == 2)
        {
            upgradeDescOne.text = "Unlock Rapid\nSpikes Ability";
            upgradeImageOne.sprite = jrollImageOneTwo;
            upgradePriceOne.text = upgradeOnePrice.ToString();
        }
        else
        {
            upgradeDescOne.text = "Activate\nRapid Spikes";
            upgradeImageOne.sprite = jrollImageOneTwo;
            AbilityButtonAsset();
        }
        if (treeTwoLevel == 1)
        {
            upgradeDescTwo.text = "Fire\nSpikes";
            upgradeImageTwo.sprite = jrollImageTwoOne;
            upgradePriceTwo.text = upgradeTwoPrice.ToString();
        }
        else
        {
            upgradeDescTwo.text = "Jroll\nSpike Stacks";
            upgradeImageTwo.sprite = jrollImageTwoTwo;
            upgradePriceTwo.text = upgradeTwoPrice.ToString();
        }
    }
    private void JuulsAssets(int treeOneLevel, int treeTwoLevel, int upgradeOnePrice, int upgradeTwoPrice)
    {
        if (treeOneLevel == 1)
        {
            upgradeDescOne.text = "Burning Ice";
            upgradeImageOne.sprite = jrollImageOneOne;
            upgradePriceOne.text = upgradeOnePrice.ToString();
        }
        else if (treeOneLevel == 2)
        {
            upgradeDescOne.text = "Unlock\nBlizzard Ability";
            upgradeImageOne.sprite = jrollImageOneTwo;
            upgradePriceOne.text = upgradeOnePrice.ToString();
        }
        else
        {
            upgradeDescOne.text = "Activate\nBlizzard";
            upgradeImageOne.sprite = jrollImageOneTwo;
            AbilityButtonAsset();
        }
        if (treeTwoLevel == 1)
        {
            upgradeDescTwo.text = "Double Slow Duration";
            upgradeImageTwo.sprite = jrollImageTwoOne;
            upgradePriceTwo.text = upgradeTwoPrice.ToString();
        }
        else
        {
            upgradeDescTwo.text = "Ice Rush";
            upgradeImageTwo.sprite = jrollImageTwoTwo;
            upgradePriceTwo.text = upgradeTwoPrice.ToString();
        }
    }
    private void CoopaAssets(int treeOneLevel, int treeTwoLevel, int upgradeOnePrice, int upgradeTwoPrice)
    {
        if (treeOneLevel == 1)
        {
            upgradeDescOne.text = "Longer Flames\nDuration";
            upgradeImageOne.sprite = jrollImageOneOne;
            upgradePriceOne.text = upgradeOnePrice.ToString();
        }
        else
        {
            upgradeDescOne.text = "Even Longer\nFlame Duration";
            upgradeImageOne.sprite = jrollImageOneTwo;
            upgradePriceOne.text = upgradeOnePrice.ToString();
        }
        if (treeTwoLevel == 1)
        {
            upgradeDescTwo.text = "Reduce Cooldown\nand Fire Effectiveness";
            upgradeImageTwo.sprite = jrollImageTwoOne;
            upgradePriceTwo.text = upgradeTwoPrice.ToString();
        }
        else
        {
            upgradeDescTwo.text = "Increase Burn Duration";
            upgradeImageTwo.sprite = jrollImageTwoTwo;
            upgradePriceTwo.text = upgradeTwoPrice.ToString();
        }
    }
    private void FranAssets(int treeOneLevel, int treeTwoLevel, int upgradeOnePrice, int upgradeTwoPrice)
    {
        if (treeOneLevel == 1)
        {
            upgradeDescOne.text = "Faster Fire Rate";
            upgradeImageOne.sprite = jrollImageOneOne;
            upgradePriceOne.text = upgradeOnePrice.ToString();
        }
        else
        {
            upgradeDescOne.text = "Spread Shot";
            upgradeImageOne.sprite = jrollImageOneTwo;
            upgradePriceOne.text = upgradeOnePrice.ToString();
        }
        if (treeTwoLevel == 1)
        {
            upgradeDescTwo.text = "Increase Range";
            upgradeImageTwo.sprite = jrollImageTwoOne;
            upgradePriceTwo.text = upgradeTwoPrice.ToString();
        }
        else
        {
            upgradeDescTwo.text = "Double Damage";
            upgradeImageTwo.sprite = jrollImageTwoTwo;
            upgradePriceTwo.text = upgradeTwoPrice.ToString();
        }
    }
    private void RockAssets(int treeOneLevel, int treeTwoLevel, int upgradeOnePrice, int upgradeTwoPrice)
    {
        if (treeOneLevel == 1)
        {
            upgradeDescOne.text = "Towers Gain\nDouble Money";
            upgradeImageOne.sprite = jrollImageOneOne;
            upgradePriceOne.text = upgradeOnePrice.ToString();
        }
        else
        {
            upgradeDescOne.text = "Towers Can Shoot\nAny Enemy Type";
            upgradeImageOne.sprite = jrollImageOneTwo;
            upgradePriceOne.text = upgradeOnePrice.ToString();
        }
        if (treeTwoLevel == 1)
        {
            upgradeDescTwo.text = "Passive Rock Income";
            upgradeImageTwo.sprite = jrollImageTwoOne;
            upgradePriceTwo.text = upgradeTwoPrice.ToString();
        }
        else
        {
            upgradeDescTwo.text = "Stacks of Rock";
            upgradeImageTwo.sprite = jrollImageTwoTwo;
            upgradePriceTwo.text = upgradeTwoPrice.ToString();
        }
    }

    void AbilityButtonAsset()
    {
        if (currentTower.GetComponent<Tower>().secondsUntilCooldownDone != 0)
        {
            upgradePriceOne.text = currentTower.GetComponent<Tower>().secondsUntilCooldownDone.ToString();
        }
        else
        {
            upgradePriceOne.text = null;
        }

        upgradeOneGameObject.transform.GetChild(3).gameObject.SetActive(false);
    }

    void ZeroPriceOne()
    {
        upgradePriceOne.text = null;
        upgradeOneGameObject.transform.GetChild(3).gameObject.SetActive(false);
    }

    void ZeroPriceTwo()
    {
        upgradePriceTwo.text = null;
        upgradeTwoGameObject.transform.GetChild(3).gameObject.SetActive(false);
    }
}
