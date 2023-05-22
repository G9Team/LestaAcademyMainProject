using UnityEngine;

public class Menu : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void LoadNewGame()
    {
        SceneLoader.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ButtonSound()
    {
        audioSource.PlayOneShot(audioClip);
    }
}
