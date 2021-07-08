using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private PlayerData data;

    void Awake()
    {
        data = SaveManager.LoadPlayer();
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
    }

    public void StartGame()
    {
        if (data != null)
        {
            transform.GetChild(3).gameObject.SetActive(true);
        }
        else
        {
            SaveManager.ClearData();
            transform.GetChild(4).gameObject.SetActive(true);
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
            transform.GetChild(3).gameObject.SetActive(false);
            transform.GetChild(4).gameObject.SetActive(true);
        }
    }

    public void Cancel(string transform)
    {
        GameObject.Find(transform).SetActive(false);
    }
}
