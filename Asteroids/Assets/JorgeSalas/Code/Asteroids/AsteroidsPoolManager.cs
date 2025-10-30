using System.Collections.Generic;
using UnityEngine;

public class AsteroidsPoolManager : MonoBehaviour
{
    #region Singleton

    public static AsteroidsPoolManager Instance { get; private set; }
    #endregion

    #region Fields

    [System.Serializable]
    public class AsteroidsPool
    {
        public int size;
        public GameObject prefab;
        public int initialSize = 5;
        public Queue<GameObject> pool;
    }
    
    [Header("Cinfiguration Pool")]
    public AsteroidsPool[] asteroidPools;

    public int ActiveAsteroidsCount { get; private set; } = 0;
    
    public AudioClip asteroidSound;
    public AudioSource asteroidSoundSource;
    #endregion

    #region Awake

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePools();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Methods
    
    private void InitializePools()
    {
        foreach (var poolConfig in  asteroidPools)
        {
            poolConfig.pool = new Queue<GameObject>();
            for (int i = 0; i < poolConfig.initialSize; i++)
            {
                GameObject asteroid = Instantiate(poolConfig.prefab, transform);
                asteroid.SetActive(false);
                poolConfig.pool.Enqueue(asteroid);
            }
        }
    }

    public GameObject GetAsteroids(int size)
    {
        AsteroidsPool poolConfig = System.Array.Find(asteroidPools, p => p.size == size);

        if (poolConfig != null && poolConfig.pool.Count > 0)
        {
            GameObject asteroid = poolConfig.pool.Dequeue();
            asteroid.SetActive(true);
            ActiveAsteroidsCount++;
            return asteroid;
        }
        
        if (poolConfig != null)
        {
            GameObject newAsteroid = Instantiate(poolConfig.prefab, transform);
            newAsteroid.SetActive(true);
            ActiveAsteroidsCount++;
            return newAsteroid;
        }
        return null;
    }
    
    public void ReturnAsteroid(GameObject asteroid, int size)
    {
        AsteroidsPool poolConfig = System.Array.Find(asteroidPools, p => p.size == size);
        if (poolConfig != null)
        {
            asteroid.SetActive(false);
            asteroidSoundSource.PlayOneShot(asteroidSound);
            poolConfig.pool.Enqueue(asteroid);
            if (ActiveAsteroidsCount > 0) ActiveAsteroidsCount--;
        }
    }
    #endregion
}
