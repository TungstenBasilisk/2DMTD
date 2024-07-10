using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int health = 1;
    public PartType partTypeOnDeath;
    public int partAmount = 1;

    private Transform target;
    private int waypointIndex = 0;

    public delegate void EnemyDestroyed();
    public event EnemyDestroyed OnDestroyed;

    private GameManager gameManager;
    private EnemySpawner enemySpawner;

    void Start()
    {
        target = Waypoints.points[0];
        gameManager = FindObjectOfType<GameManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();

        OnDestroyed += HandleOnDestroyed;
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - transform.position;
        transform.Translate(speed * Time.deltaTime * dir.normalized, Space.World);

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length - 1)
        {
            Die();
            return;
        }

        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

    public void TakeDamage(int damage)
    {
        if (health != 0)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        if (partTypeOnDeath != PartType.None)
        {
            ResourceManager.instance.AddParts(partTypeOnDeath, partAmount);
        }

        OnDestroyed?.Invoke();

        Destroy(gameObject);
    }

    void HandleOnDestroyed()
    {
        if (gameManager.currentState != GameManager.GameState.GameOver)
        {
            enemySpawner.EnemyDefeated(gameObject);
        }
    }
}