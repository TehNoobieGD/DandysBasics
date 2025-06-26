using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public Slider mouseSlider;
    public Slider volumeSlider;

    [SerializeField] TextMeshProUGUI fullscreenText;
    [SerializeField] TextMeshProUGUI captionsText;
    [SerializeField] TextMeshProUGUI volumePercentText;
    [SerializeField] TextMeshProUGUI mousePercentText;

    [HideInInspector] public bool fullscreen;
    [HideInInspector] public bool captions;
    [HideInInspector] public float mouseSensitivity;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameObject.activeSelf)
            gameObject.SetActive(false);
    }
    public void InitializeSettings()
    {
        SaveLoad.LoadSettings();

        if (!SaveLoad.settingsLoaded) // default settings
        {
            mouseSensitivity = 0.5f;
            AudioListener.volume = 1.0f;
            fullscreen = true;
            captions = true;
        }


        UpdateFullscreen();
        UpdateCaptions();
        mouseSlider.value = mouseSensitivity;
        volumeSlider.value = AudioListener.volume;
        volumePercentText.text = Mathf.RoundToInt(volumeSlider.value * 100) + "%";
        mousePercentText.text = Mathf.RoundToInt(mouseSlider.value * 100) + "%";
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        SaveLoad.SaveSettings();
    }
    public void Button_Fullscreen()
    {
        fullscreen = !fullscreen;
        UpdateFullscreen();
    }
    void UpdateFullscreen()
    {
        Screen.fullScreen = fullscreen;
        string info = "On";

        if (fullscreen == false)
            info = "Off";

        ChangeText(fullscreenText, info);
    }
    public void Button_Captions()
    {
        captions = !captions;
        UpdateCaptions();
    }
    void UpdateCaptions()
    {
        string info = "On";

        if (captions == false)
            info = "Off";

        ChangeText(captionsText, info);
    }
    public void Slider_Volume()
    {
        AudioListener.volume = volumeSlider.value;
        volumePercentText.text = Mathf.RoundToInt(volumeSlider.value * 100) + "%";
    }
    public void Slider_MouseSensitivity()
    {
        mouseSensitivity = mouseSlider.value;
        mousePercentText.text = Mathf.RoundToInt(mouseSlider.value * 100) + "%";
    }
    void ChangeText(TextMeshProUGUI meshPro, string info)
    {
        meshPro.text = info;
    }
}
