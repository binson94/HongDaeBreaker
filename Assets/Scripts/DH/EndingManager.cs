using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{

    [SerializeField] private string[] text;
    [SerializeField] private Text[] endingText;

    private void Start()
    {
        endingText[0].text = "";
        endingText[1].text = "";
        endingText[2].text = "";
        endingText[3].text = "";

        StartCoroutine("TextMachine");
    }

    IEnumerator TextMachine()
    {
        for(int j = 0; j < 4; j++)
        {
            for (int i = 0; i < text[j].Length; i++)
            {
                yield return new WaitForSeconds(0.1f);
                if(j==2)
                {
                    yield return new WaitForSeconds(0.2f);
                }
                endingText[j].text += text[j][i];
            }
        }
    }
}
