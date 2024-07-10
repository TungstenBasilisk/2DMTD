using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI instance;

    public Tower currentTower;

    public Text fireRateText;
    public Text rangeText;
    public Text bulletDamageText;

    public Dropdown partTypeDropdown;
    public InputField amountInputField;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gameObject.SetActive(false); // Hide UI at start
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Close the Upgrade UI if clicking outside of it and not on a tower
        if (Input.GetMouseButtonDown(0) && currentTower != null)
        {
                CloseUpgradeUI();
        }
    }

    public void SetCurrentTower(Tower tower)
    {
        currentTower = tower;
        gameObject.SetActive(true); // Activate the UI panel
        UpdateUpgradeUIText();

        // Add a Debug.Log statement to confirm the tower is being set
        if (currentTower != null)
        {
            Debug.Log($"Current tower set to: {currentTower.gameObject.name}");
        }
        else
        {
            Debug.LogWarning("Current tower is null. Something went wrong.");
        }
    }

    // Intermediary methods for Unity's OnClick events
    public void AddPartToBulletUpgradeButton()
    {
        if (currentTower != null)
        {
            int partTypeIndex = partTypeDropdown.value - 1; // Subtract 1 to match the enum index
            if (partTypeIndex >= 0 && partTypeIndex < System.Enum.GetValues(typeof(PartType)).Length)
            {
                PartType partType = (PartType)partTypeIndex;
                if (int.TryParse(amountInputField.text, out int amount))
                {
                    AddPartToBulletUpgrade(partType, amount);
                }
                else
                {
                    Debug.LogWarning("Invalid amount entered.");
                }
            }
        }
    }

    public void AddPartToFireRateUpgradeButton()
    {
        if (currentTower != null)
        {
            int partTypeIndex = partTypeDropdown.value - 1; // Subtract 1 to match the enum index
            if (partTypeIndex >= 0 && partTypeIndex < System.Enum.GetValues(typeof(PartType)).Length)
            {
                PartType partType = (PartType)partTypeIndex;
                if (int.TryParse(amountInputField.text, out int amount))
                {
                    AddPartToFireRateUpgrade(partType, amount);
                }
                else
                {
                    Debug.LogWarning("Invalid amount entered.");
                }
            }
        }
    }

    // Original methods for adding parts
    private void AddPartToBulletUpgrade(PartType partType, int amount)
    {
        currentTower.AddPartToBulletUpgrade(partType, amount);
        UpdateUpgradeUIText();
    }

    private void AddPartToFireRateUpgrade(PartType partType, int amount)
    {
        currentTower.AddPartToFireRateUpgrade(partType, amount);
        UpdateUpgradeUIText();
    }

    public void RemovePartFromBulletUpgrade()
    {
        if (currentTower != null)
        {
            currentTower.RemovePartFromBulletUpgrade();
            UpdateUpgradeUIText();
        }
    }

    public void RemovePartFromFireRateUpgrade()
    {
        if (currentTower != null)
        {
            currentTower.RemovePartFromFireRateUpgrade();
            UpdateUpgradeUIText();
        }
    }

    public void CloseUpgradeUI()
    {
        currentTower = null;
        gameObject.SetActive(false); // Deactivate the UI panel
    }

    void UpdateUpgradeUIText()
    {
        if (currentTower != null)
        {
            fireRateText.text = "Fire Rate: " + currentTower.fireRate;
            rangeText.text = "Range: " + currentTower.range;
            bulletDamageText.text = "Bullet Damage: " + currentTower.GetBulletDamage();
        }
    }
}