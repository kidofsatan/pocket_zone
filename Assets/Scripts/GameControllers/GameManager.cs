using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private EnemySpawner enemySpawner;
    public GameObject deadPanel;

    void Awake()
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

    private void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        enemySpawner.SpawnEnemies();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (enemySpawner != null)
        {
            enemySpawner.SpawnEnemies();
        }
    }

    public void GameOver()
    {
        PauseGame();
        deadPanel.SetActive(true);
        AudioManager.Instance.PauseAll();
        ResetGame();
    }

    //метод перезагрузки уровня
    public void ReloadLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        enemySpawner.SpawnEnemies();
        AudioManager.Instance.PlayMusic();
        deadPanel.SetActive(false);
        ResumeGame();
    }

    //метод для сброса сохраненных данных
    public void ResetGame()
    {
        Debug.Log("Сброс данных игрока");
        SaveSystem.ResetAllData();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Debug.Log("Игра на паузе");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Игра продолжается");
    }
}
