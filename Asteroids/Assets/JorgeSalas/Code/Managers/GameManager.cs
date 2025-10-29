using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance { get; private set; }
    #endregion

    #region Fields

    [Header("Initial Values")]
    public int initialLives = 4;
    public int initialBigAsteroids = 10;
    
    [Header("Ship")]
    public GameObject shipPrefab; 
    
    private float safeRadius = 1.5f;
    private GameObject playerInstance; 
    
    private int currentLives;
    private int currentScore;
    private bool isGameActive = false;
    private bool isRespawning = false;
    #endregion

    #region Properties

    public bool IsGameActive
    {
        get
        {
            return isGameActive;
        }
    }
    #endregion

    #region Awake_Start_OnEnable_OnDisable

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (AsteroidsPoolManager.Instance == null || UIManager.Instance == null) return;
        StartGame();
    }
    
    void OnEnable()
    {
        GameEvents.OnAsteroidDestroyed += AddScore;
        GameEvents.OnPlayerHit += LoseLife;
    }

    void OnDisable()
    {
        GameEvents.OnAsteroidDestroyed -= AddScore;
        GameEvents.OnPlayerHit -= LoseLife;
    }
    #endregion

    #region Methods

    public void StartGame()
    {
        currentLives = initialLives;
        currentScore = 0;
        isGameActive = true;
        
        UIManager.Instance.UpdateLivesText(currentLives);
        UIManager.Instance.UpdateScoreText(currentScore);
        
        SpawnPlayer();
        GenerateInitialWave(initialBigAsteroids);
        GameEvents.GameStart();
    }

    private void EndGame()
    {
        isGameActive = false;
        if (playerInstance != null) playerInstance.SetActive(false);
        GameEvents.GameEnd();
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UIManager.Instance.UpdateScoreText(currentScore);
    }
    
    public void LoseLife()
    {
        if (!isGameActive || isRespawning) return;
        currentLives--;
        UIManager.Instance.UpdateLivesText(currentLives);
        if (playerInstance != null) playerInstance.SetActive(false);
        if (currentLives > 0)
        {
            StartCoroutine(SafeSpawnCheck());
            isRespawning = true;
        }
        else
            EndGame();
    }

    private void SpawnPlayer()
    {
        isRespawning = false;
        if (shipPrefab != null)
        {
            if (playerInstance == null)
            {
                playerInstance = Instantiate(shipPrefab, Vector3.zero, Quaternion.identity);
                Rigidbody2D rb = playerInstance.GetComponent<Rigidbody2D>();
                if(rb != null) rb.linearVelocity = Vector2.zero;
            }
            else
            {
                playerInstance.transform.position = Vector3.zero;
                playerInstance.transform.rotation = Quaternion.identity;
                playerInstance.SetActive(true);
            }
        }
    }
    
    private void SetRandomSpawnPosition(GameObject asteroid)
    {
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;
        float margin = 1.5f; 
        int side = Random.Range(0, 4); 
        Vector3 spawnPosition = Vector3.zero;
        
        if (side == 0)
            spawnPosition = new Vector3(Random.Range(-cameraWidth, cameraWidth), cameraHeight + margin, 0);
        else if (side == 1)
            spawnPosition = new Vector3(Random.Range(-cameraWidth, cameraWidth), -cameraHeight - margin, 0);
        else if (side == 2)
            spawnPosition = new Vector3(-cameraWidth - margin, Random.Range(-cameraHeight, cameraHeight), 0);
        else
            spawnPosition = new Vector3(cameraWidth + margin, Random.Range(-cameraHeight, cameraHeight), 0);
        
        asteroid.transform.position = spawnPosition;
    }

    private void GenerateInitialWave(int count)
    {
        int sizeBig = 1;
    
        for (int i = 0; i < count; i++)
        {
            GameObject asteroid = AsteroidsPoolManager.Instance.GetAsteroids(sizeBig);
            if (asteroid != null)
            {
                SetRandomSpawnPosition(asteroid); 
            }
        }
    }
    
    private bool IsAreaOccupied()
    {
        Collider2D hit = Physics2D.OverlapCircle(Vector3.zero, safeRadius);
        if (hit != null && hit.TryGetComponent<Asteroids>(out _)) return true;
        return false;
    }
    #endregion

    #region Coroutines

    private IEnumerator SafeSpawnCheck()
    {
        yield return new WaitForSeconds(1);

        while (IsAreaOccupied())
            yield return null;
        
        SpawnPlayer();
    }

    #endregion
}
