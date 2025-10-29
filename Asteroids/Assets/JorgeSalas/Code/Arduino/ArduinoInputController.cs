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

// guardo el codigo de arduiino por si acaso

/*
 
 const int fireButton = 3;
const int jostickX = A0;
const int left = 480;
const int right = 520;

void setup() {
  Serial.begin(9600);
  pinMode(fireButton, INPUT_PULLUP);
  Serial.println("control arduino iniciado");
}

void loop() {
  int joyX = analogRead(jostickX);
  bool fireButtonPressed = (digitalRead(fireButton) == LOW);
  String command = "";

  if (joyX < left) {
    command += "L";
  } 
  else if (joyX > right) {
    command += "R";
  }

  if (fireButtonPressed) {
    command += "F";
  }

  if (command.length() > 0) {
    Serial.println(command); 
    delay(50); 
  } else {
    delay(10);
  }
}
 
 */
