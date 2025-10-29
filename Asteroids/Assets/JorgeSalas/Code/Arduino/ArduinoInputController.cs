using UnityEngine;

public class ArduinoInputController : MonoBehaviour
{
    private ShipRotation shipRotation;
    private Weapon weapon;
    
    private bool inputEnabled = false;

    void Start()
    {
        shipRotation = GetComponent<ShipRotation>();
        weapon = GetComponent<Weapon>();
    }
    
    void OnEnable()
    {
        SerialReader.OnCommandReceived += HandleCommand;
        GameEvents.OnGameStart += EnableInput;
        GameEvents.OnGameEnd += DisableInput;
        if (GameManager.Instance != null && GameManager.Instance.IsGameActive) EnableInput();
    }

    void OnDisable()
    {
        SerialReader.OnCommandReceived -= HandleCommand;
        GameEvents.OnGameStart -= EnableInput;
        GameEvents.OnGameEnd -= DisableInput;
    }
    
    private void EnableInput()
    {
        inputEnabled = true;
    }
    
    private void DisableInput()
    {
        inputEnabled = false;
    }

    private void HandleCommand(string command)
    {
        if (!inputEnabled) return;
        string processedCommand = command.Trim().ToUpper();
        if (processedCommand.Contains("L"))
        {
            shipRotation.ApplyRotation(1); 
        }
        else if (processedCommand.Contains("R"))
        {
            shipRotation.ApplyRotation(-1);
        }
        
        if (processedCommand.Contains("F"))
        {
            weapon.TryFire();
        }
    }
}
