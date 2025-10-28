using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Fields
    
    private ShipRotation shipRotation;
    #endregion

    #region Awake_Start_Update
    
    void Start()
    {
        shipRotation = GetComponent<ShipRotation>();
        
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
        if (Input.GetKeyDown(KeyCode.W)) Debug.Log("pum pum");
    }
    #endregion
}
