using UnityEngine;

public class ShipSpriteController : MonoBehaviour
{
    [Header("Sprites List")]
    public Sprite[] rotationSprites = new Sprite[16];

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) enabled = false;
        SetSpriteIndex(0); 
    }


    public void SetSpriteIndex(int index)
    {

        if (index >= 0 && index < rotationSprites.Length)
            spriteRenderer.sprite = rotationSprites[index];
    }
}
