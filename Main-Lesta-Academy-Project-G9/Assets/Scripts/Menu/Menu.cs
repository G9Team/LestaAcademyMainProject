using UnityEngine;

public class Menu : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        this.transform.Find("Menu Panel/Continue").GetComponent<UnityEngine.UI.Button>().interactable =
            System.IO.File.Exists(System.IO.Path.Combine(Application.persistentDataPath, "save.json"));
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
