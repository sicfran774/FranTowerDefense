using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button sandboxMode;

    private PlayerData data;
    private StatsData stats;
    private Text currency;

    void Start()
    {
        Time.timeScale = 1f;
        sandboxMode.interactable = false;

        if (SaveManager.LoadStats() == null)
        {
            SaveManager.SaveStats(new StatsData(new Stats()));
            stats = SaveManager.LoadStats();
        }
        else
        {
            stats = SaveManager.LoadStats();
            CheckUnlockedLevels();
        }

        data = SaveManager.LoadPlayer();

        currency = transform.GetChild(0).GetChild(1).GetComponent<Text>();

        transform.GetChild(4).gameObject.SetActive(false);
        transform.GetChild(5).gameObject.SetActive(false);
        transform.GetChild(7).gameObject.SetActive(false);
        transform.GetChild(8).gameObject.SetActive(false);
        transform.GetChild(9).gameObject.SetActive(false);
    }
    void Update()
    {
        currency.text = stats.currency.ToString();
    }

    public void StartGame()
    {
        if (data != null)
        {
            transform.GetChild(4).gameObject.SetActive(true);
        }
        else
        {
            SaveManager.ClearData();
            transform.GetChild(5).gameObject.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void Continue(string choice)
    {
        if(choice == "yes")
        {
            LoadScene(data.level);
        }
        else if(choice == "no")
        {
            SaveManager.ClearData();
            data = null;
            transform.GetChild(4).gameObject.SetActive(false);
            transform.GetChild(5).gameObject.SetActive(true);
        }
    }

    public void UnlockLevel(int price)
    {
        if(stats.currency >= price)
        {
            stats.currency -= price;
            stats.sandboxMode = true;
            sandboxMode.interactable = true;
            sandboxMode.transform.GetChild(1).gameObject.SetActive(false);
            sandboxMode.transform.GetChild(2).gameObject.SetActive(false);
            SaveManager.SaveStats(stats);
        }
        else
        {
            StartCoroutine(HighlightRedText());
        }
    }

    public void Cancel(string transform)
    {
        GameObject.Find(transform).SetActive(false);
    }

    public void Settings()
    {
        transform.GetChild(7).gameObject.SetActive(true);
    }

    public void ClearGame()
    {
        transform.GetChild(8).gameObject.SetActive(true);
        transform.GetChild(7).gameObject.SetActive(false);
    }

    public void ClearGames(string choice)
    {
        if (choice == "yes")
        {
            SaveManager.ClearData();
            SaveManager.ClearSaveData();
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            transform.GetChild(7).gameObject.SetActive(true);
            Cancel("AreYouSure");
        }
    }
    
    public void About()
    {
        transform.GetChild(7).gameObject.SetActive(false);
        transform.GetChild(9).gameObject.SetActive(true);
    }

    private void CheckUnlockedLevels()
    {
        if (stats.sandboxMode)
        {
            sandboxMode.interactable = true;
            sandboxMode.transform.GetChild(1).gameObject.SetActive(false);
            sandboxMode.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    IEnumerator HighlightRedText()
    {
        currency.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        currency.color = Color.green;
    }
    void OnDestroy()
    {
        StopAllCoroutines();
    }
}
