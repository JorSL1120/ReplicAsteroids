using UnityEngine;
using System.IO.Ports;
using System;
using System.Collections;

public class SerialReader : MonoBehaviour
{
    public static event Action<string> OnCommandReceived;

    [Header("Configuración Serial")]
    public string portName = "COM3"; 
    public int baudRate = 9600;

    private SerialPort serialPort;
    private bool isRunning = false;

    void Awake()
    {
        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.ReadTimeout = 1;
            serialPort.Open();
            isRunning = true;
            Debug.Log("Conexión serial establecida en " + portName);
        }
        catch (Exception e)
        {
            Debug.LogError("Error al abrir el puerto serial " + portName + " con " + e.Message);
            isRunning = false;
        }
    }

    void Update()
    {
        if (!isRunning || serialPort == null || !serialPort.IsOpen) return;

        try
        {
            string data = serialPort.ReadLine();
            
            if (!string.IsNullOrEmpty(data))
            {
                string command = data.Trim();
                OnCommandReceived?.Invoke(command);
            }
        }
        catch (TimeoutException)
        {

        }
        catch (Exception e)
        {
            Debug.LogError("Error de lectura serial inesperado: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Puerto serial cerrado al salir de la aplicación.");
        }
    }
}
