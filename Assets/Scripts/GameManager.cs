using UnityEngine;
using TMPro; 
using System.Collections.Generic; 
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

   
    [Header("Player Stats")]
    private int playerHealth = 100; 
    private int gold = 200;        
    private int currentWave = 0;   

    [Header("UI References")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI waveText;

    [Header("Tower Placement")]
    public TowerButton[] towerButtons; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
      UpdateUI();
        SpawnManager.Instance.StartNextWave();

       
        LogManager.Instance.WriteLog($"Simülasyon Başladı. Başlangıç Can: {playerHealth}, Para: {gold}.");
    }

    public void UpdateUI()
    {
        
        if (waveText != null)
        {
            int displayWave = currentWave;

            
            if (currentWave == 0)
            {
                displayWave = 1;
            }

            waveText.text = $"GECE AKINI: {displayWave} / 2";
        }

        if (healthText != null) healthText.text = $"RUH OZU: {GetCurrentHealth()}";
        if (goldText != null) goldText.text = $"ALTIN: {GetCurrentGold()}";

        if (towerButtons != null)
        {
            int currentGold = GetCurrentGold();
            foreach (TowerButton button in towerButtons)
            {
                if (button != null)
                {
                    button.CheckAvailability(currentGold);
                }
            }
        }
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateUI();
        LogManager.Instance.WriteLog($"Düşman öldü. Ödül +{amount}. Toplam Para: {gold}.");
    }

    
    public void SubtractGold(int amount)
    {
        gold -= amount;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        playerHealth -= amount;
        UpdateUI();
        LogManager.Instance.WriteLog($"Düşman üsse ulaştı. Oyuncu Canı: {playerHealth} (-{amount}).");

        if (playerHealth <= 0)
        {
            GameOver(false);
        }
    }

   
    public int StartNextWave()
{
   
    currentWave = SpawnManager.Instance.currentWaveIndex + 1; 

    UpdateUI(); 
    LogManager.Instance.WriteLog($"Dalga {currentWave} Başladı.");
    
    
    if (SpawnManager.Instance != null)
    {
        StartCoroutine(SpawnManager.Instance.SpawnWave()); 
    }
    
    return currentWave;
}

    public void GameOver(bool won)
    {
        if (won)
        {
            LogManager.Instance.WriteLog($"SON: Tüm dalgalar temizlendi. OYUN KAZANILDI! (Kalan Can: {playerHealth}, Toplam Para: {gold})");
        }
        else
        {
            LogManager.Instance.WriteLog("SON: Oyuncu Canı 0'a düştü. OYUN KAYBEDİLDİ!");
        }

        Time.timeScale = 0f;

        
        return;
    }

    public int GetCurrentGold() { return gold; }
    public int GetCurrentHealth() { return playerHealth; }
    public int GetCurrentWave() { return currentWave; }
}