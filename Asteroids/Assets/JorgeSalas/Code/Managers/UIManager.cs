using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton

    public static UIManager Instance { get; private set; }
    #endregion

    #region Fields

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    #endregion

    #region Awake
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    
    #region Methods

    public void UpdateLivesText(int lives)
    {
        if (livesText != null) livesText.text = lives.ToString();
    }

    public void UpdateScoreText(int score)
    {
        if (scoreText != null) scoreText.text = score.ToString();
    }
    #endregion
}
