using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{

    [SerializeField] private string volumeParameter;
    [SerializeField] private Slider audioSlider;
    [SerializeField] private AudioMixer mixer;

    private const float _multipliar = 20f;
    private float _volumeValue;

    void Awake()
    {
        audioSlider.onValueChanged.AddListener(HandleSliderValueChanged);
    }

    private void Start()
    {
        _volumeValue = PlayerPrefs.GetFloat(volumeParameter, Mathf.Log10(audioSlider.value) * _multipliar);
        audioSlider.value = Mathf.Pow(10f, _volumeValue / _multipliar);
    }

    private void HandleSliderValueChanged(float value)
    {
        var volumeValue = Mathf.Log10(value) * _multipliar;
        mixer.SetFloat(volumeParameter, volumeValue);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, _volumeValue);
    }

    
}

