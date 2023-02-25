using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGController : MonoBehaviour
{
    [SerializeField] RectTransform _canvasRect;
    [SerializeField] Image[] bgImages;
    float screenHalfXSize;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(Screen.width, Screen.height, true);
        GameManager.Instance.Input.UpdateAction -= BGMove;
        GameManager.Instance.Input.UpdateAction += BGMove;

        SetBGScale();
    }

    void SetBGScale()
    {
        if ((float)Screen.width / Screen.height >= 1920f / 1080)
            _canvasRect.sizeDelta = new Vector2(Screen.width, Screen.height);
        foreach (Image i in bgImages)
            i.rectTransform.sizeDelta = new Vector2(3000, 1080);

        screenHalfXSize = -Camera.main.ScreenToWorldPoint(Vector3.zero).x;


        for (int i = 0; i < bgImages.Length; i++)
            bgImages[i].rectTransform.anchoredPosition = new Vector2(i * bgImages[i].rectTransform.sizeDelta.x, 0);
    }

    int idx = 1;
    void BGMove()
    {
        if (bgImages[idx].transform.position.x <= Camera.main.transform.position.x - screenHalfXSize)
        {
            idx = (idx + 1) % 2;
            bgImages[idx].rectTransform.anchoredPosition += new Vector2(2 * bgImages[idx].rectTransform.sizeDelta.x, 0);
        }
    }

    public void Unsubscribe()
    {
        GameManager.Instance.Input.UpdateAction -= BGMove;
    }
}

