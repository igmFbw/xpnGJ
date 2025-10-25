using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class setUp : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    private void OnEnable()
    {
        Time.timeScale = 0;
        if (PlayerPrefs.HasKey("bgm"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("bgm");
        }
        if (PlayerPrefs.HasKey("sound"))
        {
            soundSlider.value = PlayerPrefs.GetFloat("sound");
        }
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetFloat("bgm", musicSlider.value);
        PlayerPrefs.SetFloat("sound", soundSlider.value);
    }
    public void setSoundVolume()
    {
        audioMixer.SetFloat("sound", Mathf.Log10(soundSlider.value) * 25);
    }
    public void setMusicVolume()
    {
        audioMixer.SetFloat("music", Mathf.Log10(musicSlider.value) * 25);
    }
    public void returnMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
