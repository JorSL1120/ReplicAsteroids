using UnityEngine;

public class ShipRotation : MonoBehaviour
{
    #region Fields

    public float rotationStep = 22.5f;
    #endregion
    
    #region Methods

    public void ApplyRotation(int direction)
    {
        transform.Rotate(0, 0, direction * rotationStep);
    }
    #endregion
}
