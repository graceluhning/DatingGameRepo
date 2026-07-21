using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    
    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(3);
    }

    public void GameWon()
    {
        SceneManager.LoadScene(4);
    }

    
    public void QuitButton()
    {
        Application.Quit();
    }
}
