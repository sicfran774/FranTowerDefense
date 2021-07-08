using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int towerCount;
    public int round;
    public bool roundInProgress;
    public bool startRound;
    public float roundBonusMultiplier;
    public bool paused;

    [Space(20)]

    public Button startRoundButton;

    [Space(20)]

    public AudioSource pop;
    public AudioSource whish;

    public GameObject PogShooter;
    public GameObject CoopaTroopa;
    public GameObject Juuls;
    public GameObject TadRock;
    public GameObject Jroll;
    public GameObject SuperFran;

    private SpawnManager spawner;
    private UpgradeManager upgradeManager;
    private GameObject upgradeUI;
    private GameObject gameUI;
    private GameObject pauseMenu;

    private string sceneName;

    void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;

        round = 0;
        roundInProgress = false;
        startRound = false;
        paused = false;

        upgradeUI = GameObject.Find("Buttons");
        upgradeManager = GameObject.Find("Upgrade Manager").GetComponent<UpgradeManager>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnManager>();
        gameUI = GameObject.Find("GameUI");
        pauseMenu = GameObject.Find("PauseMenu");

        Time.timeScale = 1f;
         
        //Start game with upgrade UI disabled and not showing
        upgradeUI.GetComponent<CanvasGroup>().interactable = false;
        upgradeUI.GetComponent<CanvasGroup>().alpha = 0;

        //Same as above but tower info
        gameUI.transform.GetChild(1).gameObject.SetActive(false);

        //Hide pause UI
        pauseMenu.SetActive(false);

        LoadPlayerSaveData();
        LoadTowerSaveData();
    }

    void Update()
    {
        if(gameUI.GetComponent<Player>().health < 1)
        {
            //EndGame();
        }
    }

    public void StartRound()
    {
        if (!roundInProgress)
        {
            SavePlayerData();
            SaveTowerData();
            roundInProgress = true;
            startRoundButton.interactable = false;
            round++;
            startRound = true;
        }
    }

    public void RewardEndOfRound()
    {
        int bonus = (int)(round * roundBonusMultiplier * 100); 
        gameUI.GetComponent<Player>().money += bonus;
        Debug.Log("Rewarded " + bonus + " money");
    }

    public string[] LoadLevelData()
    {
        string json = File.ReadAllText(Application.dataPath + "/Resources/levelData.json");
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);

        switch (sceneName)
        {
            case "TwistedStones":
                return levelData.twistedStones;
            default:
                return null;
        }
    }

    public void Pause()
    {
        if (!paused)
        {
            Time.timeScale = 0;
            paused = true;

            ShowPauseMenu();
            HideSaveMenu();
            DisableAllTowers();
        }
        else
        {
            Time.timeScale = 1;
            paused = false;

            HidePauseMenu();
            EnableAllTowers();
        }
    }
    public void QuitGame()
    {
        ShowSaveMenu();
    }

    public void SaveChoice(string choice)
    {
        if (choice == "yes")
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            HideSaveMenu();
        }
    }

    public void SavePlayerData()
    {
        SaveManager.SavePlayer(gameUI.GetComponent<Player>());
        Debug.Log("Saved.");
    }
    public void SaveTowerData()
    {
        TowerData[] towers = new TowerData[300];
        int count = 0;
        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower"))
        {
            towers[count] = new TowerData(tower.GetComponent<Tower>());
            count++;
        }
        SaveManager.SaveTowers(towers);
    }

    private void LoadPlayerSaveData()
    {
        PlayerData playerData = SaveManager.LoadPlayer();
        if(playerData == null)
        {
            Player player = gameUI.GetComponent<Player>();
            playerData = new PlayerData(player);
        }
        
        gameUI.GetComponent<Player>().money = playerData.money;
        gameUI.GetComponent<Player>().health = playerData.health;
        spawner.GoToRound(playerData.round);
    }

    private void LoadTowerSaveData()
    {
        TowerData[] towerData = SaveManager.LoadTowers();
        if (towerData == null)
        {
            return;
        }

        foreach (TowerData data in towerData)
        {
            if(data == null)
            {
                continue;
            }

            GameObject tower = InstantiateTower(data.type);

            tower.GetComponent<PlaceTower>().placedTower = true;
            tower.transform.position = new Vector2(data.position[0], data.position[1]);

            Tower t = tower.GetComponent<Tower>();
            upgradeManager.currentTower = tower;
            upgradeManager.upgrade = t.GetUpgradeInstance();
            t.GetUpgradeInstance().SetUpgradeLevel(1, data.upgrade[0]);
            upgradeManager.UnlockUpgradeForSpecificTower(data.type, 1, data.upgrade[0] - 1);
            t.GetUpgradeInstance().SetUpgradeLevel(2, data.upgrade[1]);
            upgradeManager.UnlockUpgradeForSpecificTower(data.type, 2, data.upgrade[1] - 1);
            t.price = data.price;

            t.transform.GetChild(1).gameObject.SetActive(false);

            tower.tag = "Tower";
        }
    }

    private GameObject InstantiateTower(string type)
    {
        GameObject tower;
        switch (type)
        {
            case "Pog Shooter":
                tower = Instantiate(PogShooter);
                break;
            case "Coopa Troopa":
                tower = Instantiate(CoopaTroopa);
                break;
            case "Juuls":
                tower = Instantiate(Juuls);
                break;
            case "Tad Rock":
                tower = Instantiate(TadRock);
                break;
            case "Jroll":
                tower = Instantiate(Jroll);
                break;
            case "Super Fran":
                tower = Instantiate(SuperFran);
                break;
            default:
                tower = Instantiate(PogShooter);
                break;
        }
        return tower;
    }

    private void DisableAllButtons()
    {
        upgradeUI.GetComponent<CanvasGroup>().interactable = false;
        gameUI.GetComponent<CanvasGroup>().interactable = false;
    }

    private void EnableAllButtons()
    {
        upgradeUI.GetComponent<CanvasGroup>().interactable = true;
        gameUI.GetComponent<CanvasGroup>().interactable = true;
    }

    private void DisableAllTowers()
    {
        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower"))
        {
            tower.GetComponent<PlaceTower>().enabled = false;
            tower.GetComponent<Tower>().enabled = false;
        }
    }

    private void EnableAllTowers()
    {
        foreach (GameObject tower in GameObject.FindGameObjectsWithTag("Tower"))
        {
            tower.GetComponent<PlaceTower>().enabled = true;
            tower.GetComponent<Tower>().enabled = true;
        }
    }

    public void ClearAllProjectiles()
    {
        foreach (GameObject projectile in GameObject.FindGameObjectsWithTag("Projectile"))
        {
            Destroy(projectile);
        }
    }

    private void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    private void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    private void ShowSaveMenu()
    {
        pauseMenu.transform.GetChild(1).gameObject.SetActive(false);
        pauseMenu.transform.GetChild(2).gameObject.SetActive(true);
    }

    private void HideSaveMenu()
    {
        pauseMenu.transform.GetChild(1).gameObject.SetActive(true);
        pauseMenu.transform.GetChild(2).gameObject.SetActive(false);
    }

    private class LevelData
    {
        public string[] twistedStones;
    }

    public void PlayPopNoise()
    {
        pop.Play();
    }

    public void PlayWhishNoise()
    {
        whish.Play();
    }
}