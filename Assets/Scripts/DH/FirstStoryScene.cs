using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstStoryScene : MonoBehaviour
{

    [SerializeField] private GameObject[] errors;
    [SerializeField] private GameObject BlackOut;
    [SerializeField] private AudioSource Audio;


    private void Awake()
    {
        if(PlayerPrefs.HasKey("FirstStory"))
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            PlayerPrefs.SetInt("FirstStory", 1);
            SoundManager.instance.StopBGM();
        }
    }
    public void Sugang()
    {
        StartCoroutine("Ani1");
    }

    public void PlayError() =>
        SoundManager.instance.playSFX(SoundManager.SFX.WindowError);

    IEnumerator Ani1()
    {
        SoundManager.instance.playSFX(SoundManager.SFX.WindowError);
        errors[0].SetActive(true);
        yield return new WaitForSeconds(1f);
        SoundManager.instance.playSFX(SoundManager.SFX.WindowError);
        errors[1].SetActive(true);
        yield return new WaitForSeconds(0.1f);
        SoundManager.instance.playSFX(SoundManager.SFX.WindowError);
        errors[2].SetActive(true);
        yield return new WaitForSeconds(0.1f);
        SoundManager.instance.playSFX(SoundManager.SFX.WindowError);
        errors[3].SetActive(true);
        yield return new WaitForSeconds(0.1f);
        SoundManager.instance.playSFX(SoundManager.SFX.WindowError);
        errors[4].SetActive(true);
        yield return new WaitForSeconds(0.1f);
        SoundManager.instance.playSFX(SoundManager.SFX.WindowError);
        errors[5].SetActive(true);
        yield return new WaitForSeconds(1f);
        SoundManager.instance.playSFX(SoundManager.SFX.Fade);
        BlackOut.SetActive(true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
}
