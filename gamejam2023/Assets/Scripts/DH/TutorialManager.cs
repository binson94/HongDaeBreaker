using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject story;
    [SerializeField] private GameObject BG;
    [SerializeField] private GameObject leftPanel;
    [SerializeField] private GameObject rightPanel;
    [SerializeField] private GameObject clickPanel;
    [SerializeField] private TTGameScene tTGameScene;
    [SerializeField] private GameObject max;
    [SerializeField] private TTPlayer tTPlayer;
    [SerializeField] private TTBGController tTBGController;
    [SerializeField] private Text[] text;
    [SerializeField] private GameObject[] textBox;
    public bool pass3;
    private bool textPass;
    public bool gameStop;
    public bool downCan;
    public bool upCan;
    public bool canDamage;
    public bool oneShot;

    int newsIdx = 0;
    [SerializeField] GameObject[] newspapers;

    private void Start()
    {
        downCan = false;
        upCan = false;
        textPass = false;
        canDamage = false;
        oneShot = false;
        tTPlayer.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        SoundManager.instance.playBGM(SoundManager.BGM.Title);

        NextNews();
    }

    public void NextNews()
    {
        if(newsIdx <= 2)
        {
            if (newsIdx <= 0)
                SoundManager.instance.playSFX(SoundManager.SFX.News);
            else
                SoundManager.instance.playSFX(SoundManager.SFX.NewsShort);
            newspapers[newsIdx++].SetActive(true);
        }
        else
        {
            foreach (GameObject g in newspapers)
                g.SetActive(false);
            StartTutorial();
        }    
    }

    public void StartTutorial()
    {
        BG.SetActive(false);
        story.SetActive(false);
        max.SetActive(true);
        tTBGController.TTStart();
        tTPlayer.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        tTPlayer.TTStart();

        StartCoroutine("TutorialStart");
    }

    IEnumerator TutorialStart()
    {
        // 1번째
        yield return new WaitForSeconds(1f);
        tTGameScene.Generate1x1(1);
        yield return new WaitForSeconds(1.6f);
        gameStop = true;
        rightPanel.SetActive(true);
        textBox[0].SetActive(true);
        clickPanel.SetActive(true);
        while (!textPass)
        {
            yield return new WaitForSeconds(0.1f);
        }
        downCan = true;
        rightPanel.SetActive(false);
        gameStop = false;
        while (!pass3)
        {
            pass3 = true;
            yield return new WaitForSeconds(3f);
            if (!pass3)
            {
                tTGameScene.Generate1x1(1);
                yield return new WaitForSeconds(1f);
            }
        }
        textBox[0].SetActive(false);
        pass3 = false;
        downCan = false;
        textPass = false;

        // 2번째
        yield return new WaitForSeconds(1f);
        tTGameScene.Generate1x1(1);
        yield return new WaitForSeconds(1.6f);
        gameStop = true;
        leftPanel.SetActive(true);
        textBox[1].SetActive(true);
        clickPanel.SetActive(true);
        while (!textPass)
        {
            yield return new WaitForSeconds(0.1f);
        }
        upCan = true;
        leftPanel.SetActive(false);
        gameStop = false;
        while (!pass3)
        {
            pass3 = true;
            yield return new WaitForSeconds(3f);
            if (!pass3)
            {
                tTGameScene.Generate1x1(1);
                yield return new WaitForSeconds(1f);
            }
        }
        textBox[1].SetActive(false);
        pass3 = false;
        upCan = false;
        textPass = false;

        //3번째
        yield return new WaitForSeconds(1f);
        tTGameScene.GeneratePoint(1);
        yield return new WaitForSeconds(1.6f);
        gameStop = true;
        textBox[2].SetActive(true);
        yield return new WaitForSeconds(2f);
        gameStop = false;
        textBox[2].SetActive(false);

        //4번째
        canDamage = true;
        yield return new WaitForSeconds(1f);
        tTGameScene.Generate1x1(1);
        yield return new WaitForSeconds(1.6f);
        gameStop = true;
        textBox[3].SetActive(true);
        yield return new WaitForSeconds(2f);
        gameStop = false;
        textBox[3].SetActive(false);

        //5번째
        yield return new WaitForSeconds(2f);
        textBox[4].SetActive(true);
        gameStop = true;
        tTPlayer.Point = 100;
        yield return new WaitForSeconds(2f);
        tTPlayer.GetPoint();
        downCan = true;
        upCan = true;
        textBox[4].SetActive(false);
        gameStop = false;
        oneShot = true;
        yield return new WaitForSeconds(6f);
        tTGameScene.Generate1x1(2);
        tTGameScene.Generate1x1(1);
        tTGameScene.Generate1x1(0);
        yield return new WaitForSeconds(1.75f);
        gameStop = true;
        textBox[5].SetActive(true);
        yield return new WaitForSeconds(2f);
        tTPlayer.Unsubscribe();
        tTBGController.Unsubscribe();
        tTGameScene.Unsubscribe();
        SceneManager.LoadScene(3);
    }

    public void textSkip()
    {
        textPass = true;
    }
    
}
