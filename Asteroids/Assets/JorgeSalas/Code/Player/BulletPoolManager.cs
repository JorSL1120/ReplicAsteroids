using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    #region Fields
    
    public static BulletPoolManager Instance { get; private set; }
    
    [Header("Pool Configuration")]
    public GameObject bulletPrefab;
    public int poolSize = 20;
    
    private Queue<GameObject> bulletPool;
    #endregion

    #region Awake
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Methods
    
    private void InitializePool()
    {
        bulletPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            GameObject newBullet = Instantiate(bulletPrefab, transform);
            newBullet.SetActive(true);
            return newBullet;
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
    #endregion
}
