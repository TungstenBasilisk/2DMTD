using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    public GameObject regularTowerPrefab;
    public GameObject waterTowerPrefab;
    public bool isWaterPlot;
    public int towerCost = 10;
    public int waterTowerCost = 15;

    private Renderer rend;
    private Color startColor;
    private bool isOccupied;
    private GameObject placedTower;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        isOccupied = false;
    }

    void OnMouseDown()
    {
        if (isOccupied)
        {
            // Open upgrade UI for the placed tower
            placedTower.GetComponent<Tower>().OpenUpgradeUI();
            return;
        }

        if (isWaterPlot)
        {
            if (!ResourceManager.instance.CanAfford(PartType.Standard, waterTowerCost))
            {
                return;
            }

            ResourceManager.instance.SpendParts(PartType.Standard, waterTowerCost);
            placedTower = Instantiate(waterTowerPrefab, transform.position, transform.rotation);
        }
        else
        {
            if (!ResourceManager.instance.CanAfford(PartType.Standard, towerCost))
            {
                return;
            }

            ResourceManager.instance.SpendParts(PartType.Standard, towerCost);
            placedTower = Instantiate(regularTowerPrefab, transform.position, transform.rotation);
        }

        isOccupied = true;
    }

    void OnMouseEnter()
    {
        rend.material.color = Color.green;
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}