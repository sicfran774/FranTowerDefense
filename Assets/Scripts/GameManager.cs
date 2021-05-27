using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private GameObject spawner;
    private GameObject upgradeUI;
    private GameObject gameUI;
    private GameObject pauseMenu;

    void Awake()
    {
        round = 0;
        roundInProgress = false;
        startRound = false;
        paused = false;

        upgradeUI = GameObject.Find("Buttons");
        spawner = GameObject.FindGameObjectWithTag("Spawner");
        gameUI = GameObject.Find("GameUI");
        pauseMenu = GameObject.Find("PauseMenu");

        Time.timeScale = 1;

        //Start game with upgrade UI disabled and not showing
        upgradeUI.GetComponent<CanvasGroup>().interactable = false;
        upgradeUI.GetComponent<CanvasGroup>().alpha = 0;

        //Same as above but tower info
        gameUI.transform.GetChild(1).gameObject.SetActive(false);

        //Hide pause UI
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (!GameObject.FindGameObjectWithTag("Enemy") && roundInProgress) //Occurs only once when last enemy disappears
        {
            RewardEndOfRound();
            ClearAllProjectiles();

            startRoundButton.interactable = true;
            roundInProgress = false;
        }
    }

    public void StartRound()
    {
        if (!roundInProgress)
        {
            roundInProgress = true;
            startRoundButton.interactable = false;
            round++;
            startRound = true;
        }
    }

    public void RewardEndOfRound()
    {
        int bonus = (int)(round * spawner.GetComponent<SpawnManager>().numEnemiesSpawned * roundBonusMultiplier);
        gameUI.GetComponent<Player>().money += bonus;
        Debug.Log("Rewarded " + bonus + " money");
    }

    public string[] LoadLevelData()
    {
        string json = File.ReadAllText(Application.dataPath + "/Data/levelData.json");
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);
        return levelData.twistedStones;
    }

    public void Pause()
    {
        if (!paused)
        {
            Time.timeScale = 0;
            paused = true;

            ShowPauseMenu();
            //DisableAllButtons();
            DisableAllTowers();
        }
        else
        {
            Time.timeScale = 1;
            paused = false;

            HidePauseMenu();
            //EnableAllButtons();
            EnableAllTowers();
        }
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

    private void ClearAllProjectiles()
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

    private class LevelData
    {
        public string[] twistedStones;
    }

    public void PlayPopNoise(GameObject enemy)
    {
        pop.Play();
    }

    public void PlayWhishNoise()
    {
        whish.Play();
    }
}


