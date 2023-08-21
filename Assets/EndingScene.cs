using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScene : MonoBehaviour
{
    [SerializeField] GameObject endingPanel;

    private void Start()
    {
        SoundManager.instance.StopBGM();
        SoundManager.instance.playSFX(SoundManager.SFX.HongikCrash);

        Invoke("PlayEnding", 1f);
    }

    void PlayEnding() => SoundManager.instance.playBGMOneShot(SoundManager.BGM.Ending);

    public void ShowEndingPanel()
    {
        endingPanel.SetActive(true);
    }

    public void TitleBtn()
    {
        SceneManager.LoadScene(1);
    }
}
