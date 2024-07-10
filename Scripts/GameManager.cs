using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { Idle, Active, Paused, GameOver }
    public GameState currentState;

    private EnemySpawner enemySpawner;
    private int remainingEnemies;

    void Start()
    {
        currentState = GameState.Idle;
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    public void StartNextRound()
    {
        if (currentState == GameState.Idle || currentState == GameState.Paused)
        {
            currentState = GameState.Active;
            enemySpawner.StartSpawning();
        }
    }

    public void PauseGame()
    {
        if (currentState == GameState.Active)
        {
            currentState = GameState.Paused;
            Time.timeScale = 0;
        }
        else if (currentState == GameState.Paused)
        {
            currentState = GameState.Active;
            Time.timeScale = 1;
        }
    }

    public void GameOver()
    {
        currentState = GameState.GameOver;
        enemySpawner.StopSpawning();
        Time.timeScale = 1;
    }

    public void EnemyDefeated()
    {
        remainingEnemies--;

        if (remainingEnemies <= 0)
        {
            if (enemySpawner.HasMoreRounds())
            {
                currentState = GameState.Idle;
            }
            else
            {
                GameOver();
            }
        }
    }

    public void SetRemainingEnemies(int count)
    {
        remainingEnemies = count;
    }

    public void IncrementRemainingEnemies()
    {
        remainingEnemies++;
    }
}