using UnityEngine;

public class Menu : MonoBehaviour
{

    public void LoadNewGame()
    {
        SceneLoader.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
