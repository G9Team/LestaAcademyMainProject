using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{

    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string MusicPref = "MusicPref";
    private int firstPlayInt;
    [SerializeField] private Slider musicSlider;
    private float musicFloat;
    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioSource soundEffectsAudio;
    [SerializeField] private AudioClip buttonSound;

    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0)
        {
            musicFloat = 0.5f;
            musicSlider.value = musicFloat;
            PlayerPrefs.SetFloat(MusicPref, musicFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else
        {
            musicFloat = PlayerPrefs.GetFloat(MusicPref);
            musicSlider.value = musicFloat;
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(MusicPref, musicSlider.value);
    }

    void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        musicAudio.volume = musicSlider.value;
    }

    public void ButtonSound()
    {
        musicAudio.PlayOneShot(buttonSound);
    }

}

