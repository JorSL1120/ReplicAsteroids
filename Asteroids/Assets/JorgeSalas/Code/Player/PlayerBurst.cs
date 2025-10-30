using System.Collections;
using UnityEngine;

public class PlayerBurst : MonoBehaviour
{
    [Header("Burts Conf")]
    public Sprite[] burstsSprites = new Sprite[3];

    public float durationPerSprite = 0.05f;
    
    private SpriteRenderer spriteRenderer;
    private ShipRotation shipRotation;
    private ShipSpriteController  shipSpriteController;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        shipRotation = GetComponent<ShipRotation>();
        shipSpriteController = GetComponent<ShipSpriteController>();
    }

    void OnEnable()
    {
        GameEvents.OnPlayerHit += StartBurstAnimation;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerHit -= StartBurstAnimation;
    }

    private void StartBurstAnimation()
    {
        if (shipSpriteController != null) shipSpriteController.enabled = false;
        if (shipRotation != null) shipRotation.enabled = false;
        StartCoroutine(BurstSequence());
    }

    private IEnumerator BurstSequence()
    {
        if (burstsSprites.Length < 3) yield break;
        
        for (int i = 0; i < burstsSprites.Length; i++)
        {
            spriteRenderer.sprite = burstsSprites[i];
            yield return new WaitForSeconds(durationPerSprite);
        }
        
        gameObject.SetActive(false);
        if (shipRotation != null) shipRotation.enabled = true;
        if (shipSpriteController != null) shipSpriteController.enabled = true;
    }
}
