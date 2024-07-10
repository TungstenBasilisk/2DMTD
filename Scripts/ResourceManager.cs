using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    public int waterParts = 0;
    public int fireParts = 0;
    public int electricityParts = 0;
    public int standardParts = 100;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddParts(PartType partType, int amount)
    {
        switch (partType)
        {
            case PartType.Water:
                waterParts += amount;
                break;
            case PartType.Fire:
                fireParts += amount;
                break;
            case PartType.Electricity:
                electricityParts += amount;
                break;
            case PartType.Standard:
                standardParts += amount;
                break;
        }
    }

    public bool CanAfford(PartType partType, int amount)
    {
        switch (partType)
        {
            case PartType.Water:
                return waterParts >= amount;
            case PartType.Fire:
                return fireParts >= amount;
            case PartType.Electricity:
                return electricityParts >= amount;
            case PartType.Standard:
                return standardParts >= amount;
            default:
                return false;
        }
    }

    public void SpendParts(PartType partType, int amount)
    {
        if (CanAfford(partType, amount))
        {
            switch (partType)
            {
                case PartType.Water:
                    waterParts -= amount;
                    break;
                case PartType.Fire:
                    fireParts -= amount;
                    break;
                case PartType.Electricity:
                    electricityParts -= amount;
                    break;
                case PartType.Standard:
                    standardParts -= amount;
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Not enough parts to spend!");
        }
    }
}

public enum PartType
{
    None,
    Water,
    Fire,
    Electricity,
    Standard
}
