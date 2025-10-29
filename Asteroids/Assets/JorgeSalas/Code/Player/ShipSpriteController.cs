using UnityEngine;

public class ShipSpriteController : MonoBehaviour
{
    [Header("Configuración de Sprites")]
    [Tooltip("Lista de los 16 sprites de la nave, del índice 0 al 15.")]
    public Sprite[] rotationSprites = new Sprite[16];

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer no encontrado en el objeto de la nave.");
            enabled = false;
        }
        // Inicializar con el sprite 0
        SetSpriteIndex(0); 
    }

    // ELIMINAMOS EL MÉTODO UPDATE() Y LA LÓGICA DE CÁLCULO DE ÁNGULO.

    public void SetSpriteIndex(int index)
    {
        // Verificación de seguridad
        if (index >= 0 && index < rotationSprites.Length)
        {
            spriteRenderer.sprite = rotationSprites[index];
        }
        else
        {
            // Esto no debería suceder gracias a la lógica de módulo en ShipRotation
            Debug.LogError("Índice de sprite fuera de rango: " + index);
        }
    }
}
