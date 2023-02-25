using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    /// <summary>
    /// ���� ����
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
    /// �����ϴ� ��ü
    /// </summary>
    [Header("Movement")]
    [SerializeField] GameObject circle;
    /// <summary>
    /// ��ü ���� �ӵ�
    /// </summary>
    float moveSpeed;
    /// <summary>
    /// ��ü ������
    /// </summary>
    float anglePivot;
    /// <summary>
    /// ��ü ����
    /// </summary>
    float amplitudePivot;
    /// <summary>
    /// ���� ����, ���� ����
    /// </summary>
    float angle;

    /// <summary>
    /// ��ü ��ġ ������ : 0��, 1��, 2��
    /// </summary>
    float[] heightPivot = new float[3];
    /// <summary>
    /// ��ü ��ġ index : 0��, 1��, 2��
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
    /// ��ü ��ġ ������ ���
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
    /// �� ������ ���� �̵��� �Ÿ�
    /// </summary>
    float wallDistance = 0;
    /// <summary>
    /// ���� ������ ���� �̵��� �Ÿ�
    /// </summary>
    int pointDistance = 0;
    /// <summary>
    /// �÷��̾� �̵�, GameManager.Instance.Input.UpdateAction�� ����
    /// </summary>
    void PlayerMove()
    {
        //��ü �̵� �ӵ� : 4 ~ 15
        moveSpeed = 6f + (Point / 100f) * 9f + (winCount == null ? 0 : 5f);
        //��ü ���� : 0.1 ~ 1
        if (!startWinAnim) amplitudePivot = 0.1f + (Point / 100f) * 0.9f;
        //��ü ������ : 1 ~ 40
        anglePivot = 1 + (Point / 100f) * 39f;

        //���� ���
        angle += anglePivot * Time.deltaTime;

        //��ü ���� ��ġ ���
        circle.transform.localPosition = new Vector3(0, amplitudePivot * Mathf.Sin(angle), 0);
        //��ü ���� �� ������ ��ġ ����
        transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y * 0.6f + heightPivot[height] * 0.4f, 0);
        //ī�޶� ��ġ ����
        if(!cameraHold) Camera.main.transform.position = new Vector3(transform.position.x + 7, 0, -10);

        //��, ����Ʈ ����
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
    /// �Է� ����, GameManager.Instance.Input.FixedUpdateAction�� ����
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
    /// ��ֹ��� �浹 �� ȣ��
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
    /// ���� ���� �� ȣ��
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
    /// ��Ƽ�� ���� : 10�� ��Ƽ�� �¸�
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
    /// ��Ƽ�� ���� : �¸�
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
    /// Action ���� ���
    /// </summary>
    public void Unsubscribe()
    {
        GameManager.Instance.Input.FixedUpdateAction -= PlayerMove;
        GameManager.Instance.Input.UpdateAction -= GetMoveInput;
    }
    #endregion GameClear
}
