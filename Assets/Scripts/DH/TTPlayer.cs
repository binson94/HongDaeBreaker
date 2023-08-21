using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class TTPlayer : MonoBehaviour
{
    public int Point = 30;

    [SerializeField] TutorialManager tutorialManager;
    [SerializeField] TTGameScene _gameScene;
    [SerializeField] public GameObject pointHighlight;
    [SerializeField] public Slider clearSlider;
    [SerializeField] public GameObject clearSlider1;
    [SerializeField] GameObject circle;
    [SerializeField] float moveSpeed;
    [SerializeField] float anglePivot;
    [SerializeField] float amplitudePivot;
    float angle;

    float[] heightPivot = new float[3];
    int height = 1;

    float wallDistance = 0;
    float pointDistance = 0;

    Coroutine winCount = null;

    public static float unitDistance = 390 * 0.00925f;

    // Start is called before the first frame update
    public void TTStart()
    {
        GameManager.Instance.Input.FixedUpdateAction -= PlayerMove;
        GameManager.Instance.Input.FixedUpdateAction += PlayerMove;

        GameManager.Instance.Input.UpdateAction -= GetMoveInput;
        GameManager.Instance.Input.UpdateAction += GetMoveInput;
        CalcPivot();
    }


    private void CalcPivot()
    {
        float screenHeight = -2 * Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        heightPivot[0] = -0.35f * screenHeight;
        heightPivot[1] = -0.05f * screenHeight;
        heightPivot[2] = 0.25f * screenHeight;
    }

    void PlayerMove()
    {
        if(!tutorialManager.gameStop)
        {
            moveSpeed = 4f + (Point / 100f) * 11f;
            amplitudePivot = 0.1f + (Point / 100f) * 0.9f;
            anglePivot = 1 + (Point / 100f) * 39f;

            angle += anglePivot * Time.deltaTime;

            circle.transform.localPosition = new Vector3(0, amplitudePivot * Mathf.Sin(angle), 0);
            transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y * 0.6f + heightPivot[height] * 0.4f, 0);
            Camera.main.transform.position = new Vector3(transform.position.x + 7, 0, -10);

            wallDistance += moveSpeed * Time.deltaTime;
            pointDistance += moveSpeed * Time.deltaTime;
        }
    }

    void GetMoveInput()
    {
        
        if (!Input.anyKey)
            height = 1;
        if (Input.GetMouseButton(0))
        {
            if (Input.mousePosition.x < Screen.width / 2f)
            {
                if(tutorialManager.downCan)
                {
                    height = 0;
                }
            }
            else
            {
                if (tutorialManager.upCan)
                {
                    height = 2;
                }
            }
        }

#if UNITY_EDITOR
        if (!Input.anyKey)
            height = 1;
        else if (Input.GetKey(KeyCode.DownArrow))
            height = 0;
        else if (Input.GetKey(KeyCode.UpArrow))
            height = 2;
#endif
    }

    public void GetDamage()
    {
        Point = Mathf.Max(0, Point - 30);
    }

    public void GetPoint()
    {
        Point = Mathf.Min(100, Point + 8);

        if (Point >= 100 && winCount == null)
            winCount = StartCoroutine(WinCounter());
    }

    float timer;
    IEnumerator WinCounter()
    {
        pointHighlight.SetActive(true);
        timer = 0;
        clearSlider.value = 0;
        clearSlider1.SetActive(true);

        while (timer <= 10)
        {
            if (Point < 100)
            {
                pointHighlight.SetActive(false);
                clearSlider1.SetActive(false);
                winCount = null;
                yield break;
            }

            timer += 0.1f;
            clearSlider.value = timer / 10;
            yield return new WaitForSeconds(0.1f);
        }

        Win();
    }

    void Win()
    {
        Debug.Log("win");
        Unsubscribe();
        _gameScene?.GameOver();
    }

    public void Unsubscribe()
    {
        GameManager.Instance.Input.FixedUpdateAction -= PlayerMove;
        GameManager.Instance.Input.UpdateAction -= GetMoveInput;
    }
}

