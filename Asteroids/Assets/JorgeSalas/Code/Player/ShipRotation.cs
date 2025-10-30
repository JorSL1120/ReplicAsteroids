using UnityEngine;

public class ShipRotation : MonoBehaviour
{
    #region Fields

    public float rotationStep = 22.5f;
    private int currentSpriteIndex = 0;
    private ShipSpriteController spriteController;
    #endregion

    #region Start

    void Start()
    {
        spriteController = GetComponent<ShipSpriteController>();
        UpdateRotatioonAndSprite();
    }
    #endregion
    
    #region Methods

    public void ApplyRotation(int direction)
    {
        int step;
        if (direction == 1)
            step = 1;
        else
            step = -1;

        currentSpriteIndex = (currentSpriteIndex + step + 16) % 16;
        UpdateRotatioonAndSprite();
    }
    
    private void UpdateRotatioonAndSprite()
    {
        float targetAngle = currentSpriteIndex * rotationStep;
        transform.localRotation = Quaternion.Euler(0, 0, targetAngle);
        if (spriteController != null) spriteController.SetSpriteIndex(currentSpriteIndex);
    }
    
    public void ResetToInitialSprite()
    {
        currentSpriteIndex = 0;
        UpdateRotatioonAndSprite();
    }
    #endregion
}
