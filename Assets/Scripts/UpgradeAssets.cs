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
    [Space(10)]

    [Header("Image GameObject")]
    public Image upgradeImageOne;
    public Image upgradeImageTwo;

    [Header("Upgrade Description")]
    public Text upgradeDescOne;
    public Text upgradeDescTwo;
    [Space(10)]

    [Header("Pog Shooter Images")]
    public Sprite pogShooterImageOneOne;
    public Sprite pogShooterImageOneTwo;
    public Sprite pogShooterImageTwoOne;
    public Sprite pogShooterImageTwoTwo;
    [Space(10)]

    [Header("Pog Shooter Images")]
    public Image jrollerangImageOneOne;
    
    void Awake()
    {
        GameObject upgradeOneGameObject = GameObject.Find("Upgrade 1");
        GameObject upgradeTwoGameObject = GameObject.Find("Upgrade 2");

        upgradeDescOne = upgradeOneGameObject.transform.GetChild(0).GetComponent<Text>();
        upgradeImageOne = upgradeOneGameObject.transform.GetChild(1).GetComponent<Image>();
        upgradePriceOne = upgradeOneGameObject.transform.GetChild(2).GetComponent<Text>();

        upgradeDescTwo = upgradeTwoGameObject.transform.GetChild(0).GetComponent<Text>();
        upgradeImageTwo = upgradeTwoGameObject.transform.GetChild(1).GetComponent<Image>();
        upgradePriceTwo = upgradeTwoGameObject.transform.GetChild(2).GetComponent<Text>();
    }

    public void UpdateImagesAndText(string towerType, int treeOneLevel, int treeTwoLevel, int upgradeOnePrice, int upgradeTwoPrice, int refund)
    {
        refundText.text = refund.ToString();

        switch (towerType)
        {
            case "Pog Shooter":
                PogShooterAssets(treeOneLevel, treeTwoLevel, upgradeOnePrice, upgradeTwoPrice);
                break;
            case "Jroll":
                JrollAssets(treeOneLevel, treeTwoLevel, upgradeOnePrice, upgradeTwoPrice);
                break;
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
        else if (treeOneLevel == 2)
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
        else if (treeTwoLevel == 2)
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
            upgradeImageOne.sprite = pogShooterImageOneOne;
            upgradePriceOne.text = upgradeOnePrice.ToString();
        }
        else if (treeOneLevel == 2)
        {
            upgradeDescOne.text = "Jroll\nSpike Stacks";
            upgradeImageOne.sprite = pogShooterImageOneTwo;
            upgradePriceOne.text = upgradeOnePrice.ToString();
        }
        if (treeTwoLevel == 1)
        {
            upgradeDescTwo.text = "";
            upgradeImageTwo.sprite = pogShooterImageTwoOne;
            upgradePriceTwo.text = upgradeTwoPrice.ToString();
        }
        else if (treeTwoLevel == 2)
        {
            upgradeDescTwo.text = "";
            upgradeImageTwo.sprite = pogShooterImageTwoTwo;
            upgradePriceTwo.text = upgradeTwoPrice.ToString();
        }
    }
}
