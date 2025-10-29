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
    
    [HideInInspector]
    public Color currentAsteroidColor;
    
    private SpriteRenderer spriteRenderer;
    
    private float currentSpeed;
    private Vector2 moveDirection;
    
    private float screenWidth;
    private float screenHeight;
    private float margin = 0.5f;
    
    private const float SUBSIDIARY_RATIO_MIN = 0.05f;
    private const float SUBSIDIARY_RATIO_MAX = 0.1f;
    private const float SEPARATION_IMPULSE_MAGNITUDE = 0.2f;
    #endregion

    #region Awake_Start_OnEnable_Update

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        screenHeight = Camera.main.orthographicSize;
        screenWidth = screenHeight * Camera.main.aspect;
    }

    void OnEnable()
    {
        ApplyInitialMovement();
    }

    void Update()
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

        AsteroidColor();
    }

    private void AsteroidColor()
    {
        if (spriteRenderer != null && GameManager.Instance.asteroidColors.Length > 0)
        {
            Color[] availableColors = GameManager.Instance.asteroidColors;
            int randomColor = Random.Range(0, availableColors.Length);
            Color selectedColor = availableColors[randomColor];
            spriteRenderer.color = selectedColor;
            currentAsteroidColor = selectedColor;
        }
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
        Vector2 principalDirection = baseDirection.normalized;
        float parentSpeed = baseDirection.magnitude; 

        for (int i = 0; i < count; i++)
        {
            GameObject fragment = AsteroidsPoolManager.Instance.GetAsteroids(nextSize);
            fragment.transform.position = transform.position;
        
            Asteroids fragmentScript = fragment.GetComponent<Asteroids>();
            if (fragmentScript != null)
            {
                fragmentScript.currentSpeed = Random.Range(fragmentScript.minSpeed, fragmentScript.maxSpeed);
                Vector2 lateralDirection = new Vector2(-principalDirection.y, principalDirection.x);
                
                float impulseMagnitude = Random.Range(0.1f, 1.0f) * SEPARATION_IMPULSE_MAGNITUDE;
                
                if (Random.value < 0.5f) impulseMagnitude *= -1;
            
                Vector2 separationImpulse = lateralDirection * impulseMagnitude;
                Vector2 finalDirectionVector = (baseDirection + separationImpulse);
                fragmentScript.moveDirection = finalDirectionVector.normalized * fragmentScript.currentSpeed;
                
                fragmentScript.currentAsteroidColor = this.currentAsteroidColor;
                SpriteRenderer fragmentRenderer = fragment.GetComponent<SpriteRenderer>();
                if (fragmentRenderer != null) fragmentRenderer.color = this.currentAsteroidColor;
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
