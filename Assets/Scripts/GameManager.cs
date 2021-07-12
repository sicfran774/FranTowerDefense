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
    public float soundEffectsVolume;
    public float musicVolume;
    private bool enemyMenuOpen;

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
    private GameObject enemyMenu;
    private GameObject victoryMenu;
    private GameObject settingsMenu;

    public string sceneName;
    public bool mouseHeld;

    void Awake()
    {
        SaveManager.SaveLevelData(new LevelData());

        sceneName = SceneManager.GetActiveScene().name;

        round = 0;
        if (sceneName != "Sandbox")
            roundInProgress = false;
        else
            roundInProgress = true;
        startRound = false;
        paused = false;
        enemyMenuOpen = true;
        mouseHeld = true;

        upgradeUI = GameObject.Find("Buttons");
        upgradeManager = GameObject.Find("Upgrade Manager").GetComponent<UpgradeManager>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnManager>();
        gameUI = GameObject.Find("GameUI");
        pauseMenu = GameObject.Find("PauseMenu");
        enemyMenu = GameObject.Find("EnemyMenu");
        victoryMenu = GameObject.Find("VictoryMenu");
        settingsMenu = GameObject.Find("SettingsMenu");

        Time.timeScale = 1f;
         
        //Start game with upgrade UI disabled and not showing
        upgradeUI.GetComponent<CanvasGroup>().interactable = false;
        upgradeUI.GetComponent<CanvasGroup>().alpha = 0;

        //Same as above but tower info
        gameUI.transform.GetChild(1).gameObject.SetActive(false);

        //Hide pause UI
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        if (victoryMenu != null) victoryMenu.SetActive(false);

        LoadPlayerSaveData();
        LoadTowerSaveData();
        LoadVolumeConfig();

        if (sceneName != "Sandbox")
        {
            enemyMenu.SetActive(false);
        }
    }

    void Update()
    {
        if(sceneName != "Sandbox" && gameUI.GetComponent<Player>().health < 1 && Time.timeScale != 0f)
        {
            Time.timeScale = 0f;
            StatsData data = SaveManager.LoadStats();
            data.currency += round;
            SaveManager.SaveStats(data);
            victoryMenu.SetActive(true);
            victoryMenu.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "Wow, you lost. You have been given " + round + " FranBux!";
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

    public void Victory(int health)
    {
        StatsData data = SaveManager.LoadStats();
        victoryMenu.SetActive(true);

        int reward;
        if (health < 200)
        {
            reward = round + 25;

        }
        else
        {
            reward = 100;
        }
        data.currency += reward;

        SaveManager.SaveStats(data);
        Time.timeScale = 0f;
        victoryMenu.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "You have beaten " + sceneName + "! You have been granted " + reward + " FranBux.";
    }

    public void EndGame()
    {
        SceneManager.LoadScene("MainMenu");
        SaveManager.ClearData();
    }

    public void RestartGame()
    {
        SaveManager.ClearData();
        SceneManager.LoadScene(sceneName);
    }

    public string[] LoadLevelData()
    {
        LevelData levelData = SaveManager.LoadLevelData();

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
        if (sceneName == "Sandbox")
        {
            SaveChoice("yes");
        }
        else
        {
            ShowSaveMenu();
        }
    }
    public void EnemyMenu()
    {
        if (!enemyMenuOpen)
        {
            enemyMenuOpen = true;
            GameObject.Find("EnemyMenu").GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.25f);
        }
        else
        {
            enemyMenuOpen = false;
            GameObject.Find("EnemyMenu").GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        }
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
        
        if(GameObject.FindGameObjectWithTag("SelectedTower") != null)
        {
            GameObject.FindGameObjectWithTag("SelectedTower").tag = "Tower";
        }

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

    public void SettingsMenu(string str)
    {
        if (str == "open")
        {
            settingsMenu.SetActive(true);
            pauseMenu.SetActive(false);
            settingsMenu.transform.GetChild(1).GetChild(0).GetChild(2).GetComponent<Text>().text = "Sound Effects Volume " + Mathf.Round(soundEffectsVolume * 100) + "%";
            settingsMenu.transform.GetChild(1).GetChild(1).GetChild(2).GetComponent<Text>().text = "Music Volume\n" + Mathf.Round(musicVolume * 100) + "%";
        }
        else
        {
            pauseMenu.SetActive(true);
            settingsMenu.SetActive(false);
        }
    }
    public void MouseDown()
    {
        mouseHeld = true;
    }

    public void MouseUp()
    {
        mouseHeld = false;
    }

    public void IncreaseSoundEffectsVolume()
    {
        StartCoroutine(VolumeSFXChange(true));
    }
    public void DecreaseSoundEffectsVolume()
    {
        StartCoroutine(VolumeSFXChange(false));
    }
    public void IncreaseMusicVolume()
    {
        StartCoroutine(VolumeMusicChange(true));
    }
    public void DecreaseMusicVolume()
    {
        StartCoroutine(VolumeMusicChange(false));
    }

    IEnumerator VolumeSFXChange(bool increase)
    {
        if (increase)
        {
            while (soundEffectsVolume <= 0.99f)
            {
                IncreaseVolume("SoundEffects", ref soundEffectsVolume, "Sound Effects Volume ");
                yield return new WaitForSecondsRealtime(0.03f);
                if (!mouseHeld)
                {
                    break;
                }
            }
        }
        else
        {
            while (soundEffectsVolume >= 0.01f)
            {
                DecreaseVolume("SoundEffects", ref soundEffectsVolume, "Sound Effects Volume ");
                yield return new WaitForSecondsRealtime(0.03f);
                if (!mouseHeld)
                {
                    break;
                }
            }
        }
        StatsData stats = SaveManager.LoadStats();
        stats.sfxVolume = soundEffectsVolume;
        SaveManager.SaveStats(stats);
    }
    IEnumerator VolumeMusicChange(bool increase)
    {
        if (increase)
        {
            while (musicVolume <= 0.99f)
            {
                IncreaseVolume("Music", ref musicVolume, "Music Volume\n");
                yield return new WaitForSecondsRealtime(0.03f);
                if (!mouseHeld)
                {
                    break;
                }
            }
        }
        else
        {
            while (musicVolume >= 0.01f)
            {
                DecreaseVolume("Music", ref musicVolume, "Music Volume\n");
                yield return new WaitForSecondsRealtime(0.03f);
                if (!mouseHeld)
                {
                    break;
                }
            }
        }
        StatsData stats = SaveManager.LoadStats();
        stats.musicVolume = musicVolume;
        SaveManager.SaveStats(stats);
    }

    private void IncreaseVolume(string type, ref float volume, string str)
    {
        volume += 0.01f;
        GameObject.Find(type).transform.GetChild(2).GetComponent<Text>().text = str + Mathf.Round((volume * 100)) + "%";
    }

    private void DecreaseVolume(string type, ref float volume, string str)
    {
        volume -= 0.01f;
        GameObject.Find(type).transform.GetChild(2).GetComponent<Text>().text = str + Mathf.Round((volume * 100)) + "%";
    }
    private void LoadVolumeConfig()
    {
        StatsData stats = SaveManager.LoadStats();
        soundEffectsVolume = stats.sfxVolume;
        musicVolume = stats.musicVolume;
    }

    public void PlayPopNoise()
    {
        pop.volume = soundEffectsVolume;
        pop.pitch = Random.Range(0.8f, 1.2f);
        pop.Play();
    }

    public void PlayWhishNoise()
    {
        whish.volume = soundEffectsVolume;
        whish.Play();
    }
}