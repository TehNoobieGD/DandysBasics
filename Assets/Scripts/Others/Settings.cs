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

    [HideInInspector] public bool fullscreen;
    [HideInInspector] public bool captions;
    [HideInInspector] public float mouseSensitivity;


    private void Start()
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
    }
    public void Slider_MouseSensitivity()
    {
        mouseSensitivity = mouseSlider.value;
    }
    void ChangeText(TextMeshProUGUI meshPro, string info)
    {
        meshPro.text = info;
    }
}
