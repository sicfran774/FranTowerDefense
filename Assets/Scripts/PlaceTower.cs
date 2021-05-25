using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlaceTower : MonoBehaviour
{
    public GameObject towerType;
    public bool placedTower;

    private int price, numObjects;
    public bool destroyable, overlapping, afford;

    private GameObject gameUI;
    private GameObject rangeIndicator;
    private Button pauseButton;

    private Vector2 worldPosition;
    private Color color;
    private Color colorRed;

    void Start()
    {
        placedTower = false;
        destroyable = false;
        overlapping = false;

        gameUI = GameObject.Find("GameUI");
        pauseButton = GameObject.Find("PauseButton").GetComponent<Button>();

        transform.GetChild(0).gameObject.SetActive(false);
        rangeIndicator = transform.GetChild(1).gameObject;

        color = rangeIndicator.GetComponent<SpriteRenderer>().color;
        colorRed = new Color(1f, 0f, 0f, 0.38f);

        this.tag = "SelectedTower";
        price = GetComponent<Tower>().price;
    }

    void Update()
    {
        if(numObjects == 0) //If there are no collisions occuring
        {
            rangeIndicator.GetComponent<SpriteRenderer>().color = color;
            overlapping = false;
            destroyable = false;
        }
        else if (destroyable)
        {
            rangeIndicator.GetComponent<SpriteRenderer>().color = Color.clear;
        }
        else if (!placedTower)
        {
            overlapping = true;
            rangeIndicator.GetComponent<SpriteRenderer>().color = colorRed;
        }

        if (!placedTower) //If the object hasn't been placed down
        {
            DisableButtons();
            FollowMouse();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (destroyable)
            {
                Destroy(gameObject);
                EnableButtons();
            }
            if (!overlapping && !placedTower && afford)
            {
                placedTower = true;
                transform.GetChild(0).gameObject.SetActive(true);

                SubtractMoney();
                EnableButtons();
            }
        }
        if(GameObject.Find("Player").GetComponent<Player>().money - price >= 0)
        {
            afford = true;
        }
    }
    public void ButtonPressed()
    {
        GameObject tower = Instantiate(towerType);
    }

    void FollowMouse()
    {
        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = worldPosition;
    }
    void DisableButtons()
    {
        gameUI.GetComponent<CanvasGroup>().interactable = false;
        pauseButton.interactable = false;
    }
    void EnableButtons()
    {
        gameUI.GetComponent<CanvasGroup>().interactable = true;
        pauseButton.interactable = true;
    }

    void SubtractMoney()
    {
        GameObject.Find("Player").GetComponent<Player>().money -= price;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        numObjects++;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.name == "Trash Bin")
        {
            destroyable = true;
        }
        else
        {
            destroyable = false;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        numObjects--;
    }
}
