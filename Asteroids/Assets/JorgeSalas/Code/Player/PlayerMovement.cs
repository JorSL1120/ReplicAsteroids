using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Fields
    
    private ShipRotation shipRotation;
    private Weapon weapon;
    #endregion

    #region Start_Update
    
    void Start()
    {
        shipRotation = GetComponent<ShipRotation>();
        weapon = GetComponent<Weapon>();
        
        if (shipRotation == null) enabled = false;
    }

    void Update()
    {
        RotationInput();
        FireInput();
    }
    #endregion

    #region Methods

    private void RotationInput()
    {
        if(Input.GetKeyDown(KeyCode.A))
            shipRotation.ApplyRotation(1);
        else if (Input.GetKeyDown(KeyCode.D))
            shipRotation.ApplyRotation(-1);
    }

    private void FireInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) weapon.TryFire();
    }
    #endregion
}
