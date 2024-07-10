using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float range = 5f;

    public int maxUpgradeLevel = 3;
    public int currentLevel = 1;
    public int[] upgradeCosts;
    public float[] upgradeFireRates;
    public float[] upgradeRanges;

    private float fireCooldown = 0f;
    private Transform target;

    private Dictionary<PartType, int> bulletUpgradeParts = new Dictionary<PartType, int>();
    private Dictionary<PartType, int> fireRateUpgradeParts = new Dictionary<PartType, int>();

    private const int maxPartsPerSlot = 10;

    void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f); // Update target every 0.5 seconds
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }

        if (fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = 1f / fireRate;
        }

        fireCooldown -= Time.deltaTime;
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        if (bulletGO.TryGetComponent<Bullet>(out var bullet))
        {
            bullet.Seek(target);
            ApplyBulletUpgrades(bullet);
        }
    }

    void ApplyBulletUpgrades(Bullet bullet)
    {
        // Apply upgrades to the bullet based on the parts in bulletUpgradeParts dictionary
        foreach (var part in bulletUpgradeParts)
        {
            switch (part.Key)
            {
                case PartType.Water:
                    bullet.damage += part.Value;
                    break;
                case PartType.Fire:
                    bullet.damage += part.Value;
                    break;
                case PartType.Electricity:
                    // Implement specific logic for electric part
                    break;
                case PartType.Standard:
                    bullet.damage += part.Value;
                    break;
            }
        }
    }

    public void OpenUpgradeUI()
    {
        UpgradeUI ui = UpgradeUI.instance;
        if (ui != null)
        {
            ui.SetCurrentTower(this);
        }
        else
        {
            Debug.LogWarning("UpgradeUI is not assigned.");
        }
    }

    public void AddPartToBulletUpgrade(PartType partType, int amount)
    {
        if (bulletUpgradeParts.ContainsKey(partType))
        {
            if (bulletUpgradeParts[partType] + amount <= maxPartsPerSlot)
            {
                bulletUpgradeParts[partType] += amount;
                ResourceManager.instance.SpendParts(partType, amount);
            }
            else
            {
                Debug.LogWarning("Maximum parts per slot reached!");
            }
        }
        else if (bulletUpgradeParts.Count == 0)
        {
            bulletUpgradeParts[partType] = amount;
            ResourceManager.instance.SpendParts(partType, amount);
        }
        else
        {
            Debug.LogWarning("Only one type of part can be added to this slot!");
        }
    }

    public void AddPartToFireRateUpgrade(PartType partType, int amount)
    {
        if (fireRateUpgradeParts.ContainsKey(partType))
        {
            if (fireRateUpgradeParts[partType] + amount <= maxPartsPerSlot)
            {
                fireRateUpgradeParts[partType] += amount;
                ResourceManager.instance.SpendParts(partType, amount);
            }
            else
            {
                Debug.LogWarning("Maximum parts per slot reached!");
            }
        }
        else if (fireRateUpgradeParts.Count == 0)
        {
            fireRateUpgradeParts[partType] = amount;
            ResourceManager.instance.SpendParts(partType, amount);
        }
        else
        {
            Debug.LogWarning("Only one type of part can be added to this slot!");
        }
    }

    public void RemovePartFromBulletUpgrade()
    {
        foreach (var part in bulletUpgradeParts)
        {
            ResourceManager.instance.AddParts(part.Key, part.Value);
        }
        bulletUpgradeParts.Clear();
    }

    public void RemovePartFromFireRateUpgrade()
    {
        foreach (var part in fireRateUpgradeParts)
        {
            ResourceManager.instance.AddParts(part.Key, part.Value);
        }
        fireRateUpgradeParts.Clear();
    }

    public float GetBulletDamage()
    {
        float damage = 1; // base damage
        foreach (var part in bulletUpgradeParts)
        {
            switch (part.Key)
            {
                case PartType.Water:
                    damage += part.Value;
                    break;
                case PartType.Fire:
                    damage += part.Value;
                    break;
                case PartType.Electricity:
                    // Add specific logic for electric part
                    break;
                case PartType.Standard:
                    damage += part.Value;
                    break;
            }
        }
        return damage;
    }
}