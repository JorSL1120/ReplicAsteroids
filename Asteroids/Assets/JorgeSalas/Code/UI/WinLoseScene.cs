using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseScene : MonoBehaviour
{
    public void ToStartScene()
    {
        SceneManager.LoadScene("Start");
    }

    public void QuitScene()
    {
        Application.Quit();
    }
}
