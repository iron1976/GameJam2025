using UnityEngine;
using TMPro;

public class EconomyScoreManager : MonoBehaviour
{
    [Header("Score Settings")]
    public float currentScore = 0f;
    public float passiveIncomeAmount = 1f;
    public float passiveIncomeInterval = 1f; // Time in seconds between passive income
    private float passiveIncomeTimer = 0f;


    [Header("UI References")]
    public TextMeshProUGUI scoreText;


    [Header("Enemy Score Drops")]
    public float basicEnemyScore = 5f;
    public float mediumEnemyScore = 10f;
    public float bossEnemyScore = 25f;

    private static EconomyScoreManager instance;
    public static EconomyScoreManager Instance{
        get {return instance;}
    }

    private void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreDisplay();
    }

    private void Update()
    {
        // Handle passive income
        passiveIncomeTimer += Time.deltaTime;
        if (passiveIncomeTimer >= passiveIncomeInterval)
        {
            AddScore(passiveIncomeAmount);
            passiveIncomeTimer = 0f;
        }
    }

    public void UpdateScoreDisplay(){
        if (scoreText != null)
        {
            scoreText.text = $"${currentScore:F0}";
        }
    }

    public bool SpendScore(float amount)
    {
        if (currentScore >= amount)
        {
            currentScore -= amount;
            UpdateScoreDisplay();
            return true;
        }
        return false;
    }

    public void AddScore(float amount){
        currentScore += amount;
        UpdateScoreDisplay();
    }

    // Methods for different enemy types
    public void OnBasicEnemyDefeated(Vector3 position)
    {
        AddScore(basicEnemyScore);
        // You can add particle effects or floating text here
    }

    public void OnMediumEnemyDefeated(Vector3 position)
    {
        AddScore(mediumEnemyScore);
    }

    public void OnHardEnemyDefeated(Vector3 position)
    {
        AddScore(bossEnemyScore);
    }

    public void OnTowerProduction(Vector3 position){

    }
}
