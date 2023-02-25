using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [Header("Interact")]
    [SerializeField] Player _player;
    [SerializeField] BGController _bgController;

    /// <summary>
    /// 장애물 생성 위치 - 0상, 1중, 2하
    /// </summary>
    [Header("Wall Generate")]
    [SerializeField] Transform[] genPos;
    /// <summary>
    /// 장애물 프리팹
    /// </summary>
    [SerializeField] Wall wallPrefab;
    /// <summary>
    /// World Space Canvas
    /// </summary>
    [SerializeField] RectTransform canvasRect;
    /// <summary>
    /// 비활성화된 장애물 관리
    /// </summary>
    Pool<Wall> _wallPool;
    /// <summary>
    /// 현재 활성화된 장애물들
    /// </summary>
    List<Wall> _walls = new List<Wall>();

    /// <summary>
    /// 
    /// </summary>
    [Header("Point Generate")]
    [SerializeField] Point pointPrefab;
    Pool<Point> _pointPool;
    List<Point> _points = new List<Point>();

    [Header("UI")]
    [SerializeField] GameObject gameOverUI;
    [SerializeField] Slider pointSlider;

    float screenHalfXSize;
    float limitXPos;

    float playerToGenDistance;

    int resent2x1 = -1;

    private void Start()
    {
        int bgm = PlayerPrefs.GetInt("bgmId", 0);
        PlayerPrefs.SetInt("bgmId",(bgm + 1) % 2);
        SoundManager.instance.playBGM((SoundManager.BGM)(1 + bgm));

        _wallPool = new Pool<Wall>(wallPrefab);
        _pointPool = new Pool<Point>(pointPrefab);

        Vector3 screenVector = Camera.main.ScreenToWorldPoint(Vector3.zero);
        screenHalfXSize = -screenVector.x;

        playerToGenDistance = screenHalfXSize - _player.transform.position.x + 2;

        CalcPivot();

        GameManager.Instance.Input.UpdateAction -= Clear;
        GameManager.Instance.Input.UpdateAction += Clear;
    }

    void CalcPivot()
    {
        Vector3 cameraVector = Camera.main.ScreenToWorldPoint(Vector3.zero);
        float screenHeight = -2 * cameraVector.y;

        genPos[0].position = new Vector3(genPos[0].position.x, 0.2629f * screenHeight, 0);
        genPos[1].position = new Vector3(genPos[1].position.x, -0.0277f * screenHeight, 0);
        genPos[2].position = new Vector3(genPos[2].position.x, -0.3185f * screenHeight, 0);
    }

    public List<int> GenerateWall()
    {
        List<int> emptyList = new List<int>();
        int rand = Random.Range(0, 100);
        int count = Random.Range(0, 3);
        
        if(resent2x1 >= 0)
        {
            resent2x1 = -1;

            if (rand <= 20)
            {
                for (int i = 0; i < 3; i++)
                    emptyList.Add(i);
                return emptyList;
            }
            else
                return Generate1x1();
        }
        else
        {
            if (rand <= 5)
            {
                for (int i = 0; i < 3; i++)
                    emptyList.Add(i);
                return emptyList;
            }
            else if (rand <= 40)
                return Generate1x1();
            else if (rand <= 75)
                return Generate2x1();
            else
            {
                int empty = Random.Range(0, 3);

                //가운데가 빈칸, 위 아래에 1x1 생성
                for (int i = 2; i >= 0; i--)
                    if (i != empty)
                        Generate1x1(i);

                emptyList.Add(empty);
                return emptyList;
            }
        }
    }

    /// <summary>
    /// 1칸짜리
    /// </summary>
    /// <param name="pos">-1일시 무작위, 0 ~ 2</param>
    List<int> Generate1x1(int pos = -1)
    {
        List<int> emptyList = new List<int>();

        if (pos < 0)
            pos = Random.Range(0, 3);

        Wall wall = _wallPool.Get(canvasRect);
        wall.gameObject.name = "1x1";
        wall.transform.position = genPos[pos].transform.position;
        wall.Init(pos);
        wall.transform.SetAsLastSibling();
        wall.gameObject.SetActive(true);
        _walls.Add(wall);

        for (int i = 0; i < 3; i++)
            if (i != pos)
                emptyList.Add(i);

        return emptyList;
    }

    /// <summary>
    /// 가로로 2칸
    /// </summary>
    List<int> Generate2x1()
    {
        List<int> emptyList = new List<int>();

        int pos = Random.Range(0, 3);

        Generate1x1(pos);
        Wall wall = _wallPool.Get(canvasRect);
        wall.gameObject.name = "1x1";
        wall.transform.position = genPos[pos].transform.position + new Vector3(Player.unitDistance, 0, 0);
        wall.Init(pos);
        wall.transform.SetAsLastSibling();
        wall.gameObject.SetActive(true);
        _walls.Add(wall);

        resent2x1 = pos;

        for(int i = 0;i < 3;i++)
            if(i != pos)
                emptyList.Add(i);

        return emptyList;
    }

    void Clear()
    {
        pointSlider.value = _player.Point / 100f;
        for (int i = 0; i < genPos.Length; i++)
            genPos[i].position = new Vector3(_player.transform.position.x + playerToGenDistance, genPos[i].position.y, 0);

        limitXPos = genPos[0].position.x - screenHalfXSize * 2 - 8;
        for (int i = 0; i < _walls.Count; i++)
        {
            if (_walls[i].transform.position.x <= limitXPos + _walls[i].transform.lossyScale.x)
            {
                _wallPool.Release(_walls[i]);
                _walls.RemoveAt(i);
            }
        }
        for(int i = 0;i < _points.Count; i++)
        {
            if (_points[i].transform.position.x <= limitXPos + _points[i].transform.lossyScale.x)
            {
                _pointPool.Release(_points[i]);
                _points.RemoveAt(i);
            }
        }
    }

    public void GeneratePoint(List<int> emptyList)
    {
        int pos = emptyList[Random.Range(0, emptyList.Count)];

        Point point = _pointPool.Get(canvasRect);
        point.transform.position = genPos[pos].position;
        point.transform.SetAsLastSibling();
        point.gameObject.SetActive(true);
        _points.Add(point);
    }

    public void GameOver()
    {
        SoundManager.instance.StopBGM();
        SoundManager.instance.playSFX(SoundManager.SFX.GameOver);
        pointSlider.value = 0;
        Time.timeScale = 0f;
        GameManager.Instance.Input.UpdateAction -= Clear;
        _bgController.Unsubscribe();
        gameOverUI.SetActive(true);
    }

    public void GoToEnding()
    {
        _bgController.Unsubscribe();
        GameManager.Instance.Input.UpdateAction -= Clear;
        SceneManager.LoadScene(4);
    }

    public void RetryBtn()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(3);
        SoundManager.instance.playSFX(SoundManager.SFX.Btn);
    }

    public void ExitBtn()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}