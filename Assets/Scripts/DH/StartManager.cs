using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{

    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

    private void Start()
    {
        float bgm = PlayerPrefs.GetFloat("BGM", 1);
        float sfx = PlayerPrefs.GetFloat("SFX", 1);

        bgmSlider.value = bgm;
        sfxSlider.value = sfx;
        SetBGM(bgm);
        SetSFX(sfx);

        SoundManager.instance.playBGM(SoundManager.BGM.Title);
    }

    public void SetBGM(float value)
    {
        SoundManager.instance.SetBGM(value);
    }

    public void SetSFX(float value)
    {
        SoundManager.instance.SetSFX(value);
    }

    public void GameStart()
    {
        if(PlayerPrefs.HasKey("firstTest"))
        {
            SceneManager.LoadScene(3);
        }
        else
        {
            PlayerPrefs.SetInt("firstTest", 1);
            SceneManager.LoadScene(2);
        }
    }   

    public void GameEnd()
    {
        Application.Quit();
    }

    public void RetryTutorial()
    {
        SceneManager.LoadScene(2);
    }

    public void RetryIntro()
    {
        PlayerPrefs.DeleteKey("FirstStory");
        SceneManager.LoadScene(0);
    }

    public void BtnSFX() => SoundManager.instance.playSFX(SoundManager.SFX.Btn);
}
