using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highscoreTxt;
    [SerializeField] Slider volumeSlider;
    [SerializeField] AudioMixerGroup mixer;

    public void HomeScreen()
    {
        SceneManager.LoadScene("Home");
    }

    public void Play()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void Start()
    {
        if(highscoreTxt)
        {
            highscoreTxt.text = "Highscore: " + PlayerPrefs.GetInt("Highscore");
        }
        if(volumeSlider)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
            mixer.audioMixer.SetFloat("Volume", volumeSlider.value);
        }
    }

    public void ChangeVolume()
    {
        mixer.audioMixer.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }
}
