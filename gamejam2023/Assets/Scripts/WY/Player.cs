using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 현재 점수
    /// </summary>
    [HideInInspector]
    public int Point { get; set; } = 30;

    /// <summary>
    /// 
    /// </summary>
    [Header("Interact")]
    [SerializeField] GameScene _gameScene;
    [SerializeField] GameObject pointHighlight;
    [SerializeField] Slider clearSlider;

    /// <summary>
    /// 진동하는 구체
    /// </summary>
    [Header("Movement")]
    [SerializeField] GameObject circle;
    /// <summary>
    /// 구체 전진 속도
    /// </summary>
    float moveSpeed;
    /// <summary>
    /// 구체 진동수
    /// </summary>
    float anglePivot;
    /// <summary>
    /// 구체 진폭
    /// </summary>
    float amplitudePivot;
    /// <summary>
    /// 현재 각도, 진동 결정
    /// </summary>
    float angle;

    /// <summary>
    /// 구체 위치 기준점 : 0하, 1중, 2상
    /// </summary>
    float[] heightPivot = new float[3];
    /// <summary>
    /// 구체 위치 index : 0하, 1중, 2상
    /// </summary>
    int height = 1;

    bool startWinAnim = false;
    bool cameraHold = false;

    public static float unitDistance = 390 * 0.00925f;


    void Start()
    {
        GameManager.Instance.Input.FixedUpdateAction -= PlayerMove;
        GameManager.Instance.Input.FixedUpdateAction += PlayerMove;

        GameManager.Instance.Input.UpdateAction -= GetMoveInput;
        GameManager.Instance.Input.UpdateAction += GetMoveInput;
        CalcPivot();
    }

    /// <summary>
    /// 구체 위치 기준점 계산
    /// </summary>
    private void CalcPivot()
    {
        float screenHeight = -2 * Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        heightPivot[0] = -0.35f * screenHeight;
        heightPivot[1] = -0.05f * screenHeight;
        heightPivot[2] = 0.25f * screenHeight;
    }

    #region Update
    /// <summary>
    /// 벽 생성을 위해 이동한 거리
    /// </summary>
    float wallDistance = 0;
    /// <summary>
    /// 점수 생성을 위해 이동한 거리
    /// </summary>
    int pointDistance = 0;
    /// <summary>
    /// 플레이어 이동, GameManager.Instance.Input.UpdateAction에 속함
    /// </summary>
    void PlayerMove()
    {
        //구체 이동 속도 : 4 ~ 15
        moveSpeed = 6f + (Point / 100f) * 9f + (winCount == null ? 0 : 5f);
        //구체 진폭 : 0.1 ~ 1
        if (!startWinAnim) amplitudePivot = 0.1f + (Point / 100f) * 0.9f;
        //구체 진동수 : 1 ~ 40
        anglePivot = 1 + (Point / 100f) * 39f;

        //각도 계산
        angle += anglePivot * Time.deltaTime;

        //구체 진동 위치 계산
        circle.transform.localPosition = new Vector3(0, amplitudePivot * Mathf.Sin(angle), 0);
        //구체 전진 및 상중하 위치 조절
        transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y * 0.6f + heightPivot[height] * 0.4f, 0);
        //카메라 위치 설정
        if(!cameraHold) Camera.main.transform.position = new Vector3(transform.position.x + 7, 0, -10);

        //벽, 포인트 생성
        if(!startWinAnim)
            wallDistance += moveSpeed * Time.deltaTime;
        if (wallDistance >= 2 * unitDistance)
        {
            List<int> emptyList = _gameScene?.GenerateWall();
            wallDistance = 0;
            pointDistance++;

            if (pointDistance >= 5)
            {
                _gameScene?.GeneratePoint(emptyList);
                pointDistance= 0;
            }
        }
    }

    /// <summary>
    /// 입력 받음, GameManager.Instance.Input.FixedUpdateAction에 속함
    /// </summary>
    void GetMoveInput()
    {
        if (!Input.anyKey)
            height = 1;
        if(Input.GetMouseButton(0))
        {
            if (Input.mousePosition.x < Screen.width / 2f)
                height = 0;
            else
                height = 2;
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
    #endregion Update

    /// <summary>
    /// 장애물과 충돌 시 호출
    /// </summary>
    public void GetDamage()
    {
        Point = Mathf.Max(0, Point - 40);
        SoundManager.instance.playSFX(SoundManager.SFX.Crash);

        if (Point <= 0)
        {
            Unsubscribe();
            _gameScene?.GameOver();
        }
    }

    /// <summary>
    /// 점수 얻을 시 호출
    /// </summary>
    public void GetPoint()
    {
        Point = Mathf.Min(100, Point + 8);

        if (Point >= 100 && winCount == null)
            winCount = StartCoroutine(WinCounter());
    }

    #region GameClear

    float timer;

    Coroutine winCount = null;
    /// <summary>
    /// 버티기 돌입 : 10초 버티면 승리
    /// </summary>
    IEnumerator WinCounter()
    {
        pointHighlight.SetActive(true);
        timer = 0;
        clearSlider.value = 0;
        clearSlider.gameObject.SetActive(true);

        while(timer <= 10)
        {
            if (Point < 100)
            {
                pointHighlight.SetActive(false);
                clearSlider.gameObject.SetActive(false);
                winCount = null;
                yield break;
            }

            timer += 0.1f;
            clearSlider.value = timer / 10;
            yield return new WaitForSeconds(0.1f);
        }

        Win();
    }

    /// <summary>
    /// 버티기 성공 : 승리
    /// </summary>
    void Win()
    {
        startWinAnim = true;
        amplitudePivot = 2f;
        GameManager.Instance.Input.UpdateAction -= GetMoveInput;
        height = 1;
        circle.GetComponent<Collider2D>().enabled = false;

        Invoke("WinAnimation", 2f);
    }

    void WinAnimation()
    {
        cameraHold = true;
        Invoke("WinNextScene", 2f);
    }

    void WinNextScene()
    {
        Unsubscribe();
        _gameScene.GoToEnding();
    }

    /// <summary>
    /// Action 구독 취소
    /// </summary>
    public void Unsubscribe()
    {
        GameManager.Instance.Input.FixedUpdateAction -= PlayerMove;
        GameManager.Instance.Input.UpdateAction -= GetMoveInput;
    }
    #endregion GameClear
}
