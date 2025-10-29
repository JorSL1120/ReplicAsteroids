using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroids : MonoBehaviour
{
    #region Fields

    public int size = 1;
    public float minSpeed = 0.5f;
    public float maxSpeed = 1.5f;
    public float fragmentExtraSpeed = 1.5f;
    
    private float currentSpeed;
    private Vector2 moveDirection;
    
    private float screenWidth;
    private float screenHeight;
    private float margin = 0.5f;
    
    private const float SUBSIDIARY_RATIO_MIN = 0.05f;
    private const float SUBSIDIARY_RATIO_MAX = 0.1f;
    #endregion

    #region Start_OnEnable_Update

    private void Start()
    {
        screenHeight = Camera.main.orthographicSize;
        screenWidth = screenHeight * Camera.main.aspect;
    }

    void OnEnable()
    {
        ApplyInitialMovement();
    }

    private void Update()
    {
        transform.Translate(moveDirection * Time.deltaTime, Space.World);
        CheckAndTeleport();
    }
    #endregion

    #region Methods

    public void Hit()
    {
        Fragment();
        GameEvents.AsteroidDestroyed(GetScoreValue());
        AsteroidsPoolManager.Instance.ReturnAsteroid(gameObject, size);
    }

    private void ApplyInitialMovement()
    {
        currentSpeed = Random.Range(minSpeed, maxSpeed);
        float slowComponent = currentSpeed * Random.Range(SUBSIDIARY_RATIO_MIN, SUBSIDIARY_RATIO_MAX);
        if (Random.value < 0.5f) slowComponent *= -1;
        float fastComponent = currentSpeed;
        if (transform.position.y > 0) fastComponent *= -1;
        moveDirection = new Vector2(slowComponent, fastComponent).normalized;
        moveDirection *= currentSpeed;
    }

    private void Fragment()
    {
        Vector2 inheritedDirection = moveDirection;
        
        if (size == 1)
            SpawnFragments(2, 2, inheritedDirection);
        else if (size == 2)
            SpawnFragments(2, 3, inheritedDirection);
    }

    private void SpawnFragments(int count, int nextSize, Vector2 baseDirection)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject fragment = AsteroidsPoolManager.Instance.GetAsteroids(nextSize);
            fragment.transform.position = transform.position;
            
            Asteroids fragmentScript = fragment.GetComponent<Asteroids>();
            if (fragmentScript != null)
            {
                fragmentScript.moveDirection = baseDirection;
                Vector2 randomOffset = Random.insideUnitCircle.normalized * fragmentExtraSpeed;
                fragmentScript.moveDirection = (fragmentScript.moveDirection + randomOffset).normalized;
                fragmentScript.currentSpeed = Random.Range(fragmentScript.minSpeed, fragmentScript.maxSpeed);
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
    
    private void CheckAndTeleport()
    {
        Vector3 newPosition = transform.position;
        
        if (newPosition.x > screenWidth + margin)
            newPosition.x = -screenWidth - margin;
        else if (newPosition.x < -screenWidth - margin)
            newPosition.x = screenWidth + margin;
        
        if (newPosition.y > screenHeight + margin)
            newPosition.y = -screenHeight - margin;
        else if (newPosition.y < -screenHeight - margin)
            newPosition.y = screenHeight + margin;

        transform.position = newPosition;
    }
    #endregion
}
