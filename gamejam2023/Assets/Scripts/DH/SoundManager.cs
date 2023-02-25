using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public enum BGM
    {
        Title, Game1, Game2, Ending,
    }

    public enum SFX
    { 
        WindowError, Btn, News, ItemGet, Crash, GameOver, HongikCrash, NewsShort, Fade,
    }

    [SerializeField] AudioMixer mixer;
    [SerializeField] private AudioSource bgSound;
    [SerializeField] private AudioSource sfxSound;
    [SerializeField] private AudioClip[] bgClips;
    [SerializeField] private AudioClip[] sfxClips;

    static public SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public void playBGM(BGM bgmIdx)
    {
        bgSound.loop = true;
        if (bgSound.clip == bgClips[(int)bgmIdx] && bgSound.isPlaying)
            return;

        bgSound.clip = bgClips[(int)bgmIdx];
        bgSound.Play();
    }


    public void playBGMOneShot(BGM bgmIdx)
    {
        bgSound.loop = false;
        bgSound.clip = bgClips[(int)bgmIdx];
        bgSound.Play();
    }

    public void StopBGM()
    {
        bgSound.Stop();
    }

    public void playSFX(SFX sfxIdx)
    {
        sfxSound.PlayOneShot(sfxClips[(int)sfxIdx]);
    }

    public void SetBGM(float value)
    {
        mixer.SetFloat("BGM", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("BGM", value);
    }

    public void SetSFX(float value)
    {
        mixer.SetFloat("SFX", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFX", value);
    }
}
