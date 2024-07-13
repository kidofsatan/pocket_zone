using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float currentHealth;
    [SerializeField] private float playerHealthMax;
    public Slider healthSlider;
    public Transform healthBarCanvas;
    
    public Transform playerTransform;

    void Start()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            currentHealth = data.health;
            transform.position = new Vector3(data.positionX, data.positionY, data.positionZ);
        }
        else
        {
            currentHealth = playerHealthMax;
        }
        healthSlider.maxValue = playerHealthMax;
        UpdateHealthUI();
    }

    private void Update()
    {
        // позиция слайдера здоровья
        Vector3 healthBarPosition = transform.position + new Vector3(0, 0, 0);
        healthBarCanvas.position = healthBarPosition;
        healthBarCanvas.rotation = Quaternion.identity;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            PlayerDies();
        }
        UpdateHealthUI();
    }

    // метод для лечения игрока
    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > playerHealthMax)
        {
            currentHealth = playerHealthMax;
        }
        UpdateHealthUI();
    }

    // метод обновления слайдера
    private void UpdateHealthUI()
    {
        healthSlider.value = currentHealth;
    }

    void PlayerDies()
    {
        GameManager.instance.GameOver();
        
    }

    void OnApplicationQuit()
    {
        SaveSystem.SavePlayer(this);
    }
}