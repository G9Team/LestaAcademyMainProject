using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeUIScript : MonoBehaviour
{

    [SerializeField] GameObject _controllsPanel, _settingsPanel;
    private float initTimeScale;
    public void OnEnable()
    {
        initTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void OnMainMenuButton()
    {
        SceneLoader.LoadScene(1);
    }
    public void OnControlsButton()
    {
        _controllsPanel.SetActive(true);
    }
    public void OnSettingsButton()
    {
        _settingsPanel.SetActive(true);
    }
    public void OnPonyatnoButton()
    {
        _controllsPanel.SetActive(false);
        _settingsPanel.SetActive(false);
    }

    private void OnDisable() {
        Time.timeScale = initTimeScale;
    }
}
