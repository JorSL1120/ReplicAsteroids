using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroids : MonoBehaviour
{
    #region Fields

    public int size = 1;
    public float minSpeed = 1;
    public float maxSpeed = 3;
    public float fragmentExtraSpeed = 1.5f;
    
    private Rigidbody2D rb;
    #endregion

    #region Awake_OnEnable

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        ApplyInitialVelocity();
    }
    #endregion

    #region Methods

    public void Hit()
    {
        Fragment();
        GameEvents.AsteroidDestroyed(GetScoreValue());
        AsteroidsPoolManager.Instance.ReturnAsteroid(gameObject, size);
    }

    private void ApplyInitialVelocity()
    {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
        float randomSpeed = UnityEngine.Random.Range(minSpeed, maxSpeed);
        rb.linearVelocity = randomDirection * randomSpeed;
    }

    private void Fragment()
    {
        if (size == 1)
            SpawnFragments(2, 2);
        else if (size == 2)
            SpawnFragments(2, 3);
    }

    private void SpawnFragments(int count, int nextSize)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject fragment = AsteroidsPoolManager.Instance.GetAsteroids(nextSize);
            fragment.transform.position = transform.position;
            Rigidbody2D fragmentRb = fragment.GetComponent<Rigidbody2D>();
            if (fragmentRb != null)
            {
                Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
                fragmentRb.linearVelocity = randomDirection * (maxSpeed + fragmentExtraSpeed);
            }
        }
    }

    private int GetScoreValue()
    {
        if (size == 1) return 20;
        if (size == 2) return 50;
        if (size == 3) return 100;
        return 0;
    }
    #endregion
}
