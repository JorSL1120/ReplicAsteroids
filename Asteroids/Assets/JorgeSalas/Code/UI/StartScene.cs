using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public void ToGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitScene()
    {
        Application.Quit();
    }
}
