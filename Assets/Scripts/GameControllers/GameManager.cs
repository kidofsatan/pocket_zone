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

    //����� ������������ ������
    public void ReloadLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        enemySpawner.SpawnEnemies();
        AudioManager.Instance.PlayMusic();
        deadPanel.SetActive(false);
        ResumeGame();
    }

    //����� ��� ������ ����������� ������
    public void ResetGame()
    {
        Debug.Log("����� ������ ������");
        SaveSystem.ResetAllData();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Debug.Log("���� �� �����");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Debug.Log("���� ������������");
    }
}
