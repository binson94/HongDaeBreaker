using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TTGameScene : MonoBehaviour
{
    [SerializeField] TTPlayer _player;
    [SerializeField] TTBGController _bgController;

    /// <summary>
    /// 장애물 생성 위치 - 0상, 1중, 2하
    /// </summary>
    [Header("Wall Generate")]
    [SerializeField] Transform[] genPos;
    /// <summary>
    /// 장애물 프리팹
    /// </summary>
    [SerializeField] Wall wallPrefab;
    [SerializeField] RectTransform canvasRect;
    /// <summary>
    /// 비활성화된 장애물 관리
    /// </summary>
    Pool<Wall> _wallPool;
    /// <summary>
    /// 현재 활성화된 장애물들
    /// </summary>
    List<Wall> _walls = new List<Wall>();

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
        _wallPool = new Pool<Wall>(wallPrefab);
        _pointPool = new Pool<Point>(pointPrefab);

        Vector3 screenVector = Camera.main.ScreenToWorldPoint(Vector3.zero);
        screenHalfXSize = -screenVector.x;

        playerToGenDistance = screenHalfXSize - _player.transform.position.x + 2;

        GameManager.Instance.Input.UpdateAction -= Clear;
        GameManager.Instance.Input.UpdateAction += Clear;
    }

    private void Update()
    {
        pointSlider.value = _player.Point / 100f;
    }

    public void GenerateWall()
    {
        int count = Random.Range(0, 3);

        switch (count)
        {
            case 1:
                bool is2x1 = Random.Range(0, 2) == 1;
                if (is2x1)
                    Generate2x1();
                else
                    Generate1x1(2);
                break;
            case 2:
                int empty;
                do
                    empty = Random.Range(0, 3);
                while (resent2x1 >= 0 && resent2x1 == empty);
                //가운데가 빈칸, 위 아래에 1x1 생성
                if (empty == 1)
                {
                    Generate1x1(0);
                    Generate1x1(2);
                }
                else
                {
                    Generate1x2(empty);
                }
                break;
        }
    }

    /// <summary>
    /// 1칸짜리
    /// </summary>
    /// <param name="pos">-1일시 무작위, 0 ~ 2</param>
    public void Generate1x1(int pos)
    {
        resent2x1 = -1;

        Wall wall = _wallPool.Get(canvasRect);
        wall.gameObject.name = "1x1";
        wall.transform.position = genPos[pos].transform.position;
        wall.Init(pos);
        wall.gameObject.SetActive(true);
        _walls.Add(wall);
    }

    /// <summary>
    /// 가로로 2칸
    /// </summary>
    void Generate2x1()
    {
        int pos = resent2x1 = Random.Range(0, 3);

        Wall wall = _wallPool.Get(canvasRect);
        wall.gameObject.name = "2x1";

        wall.transform.position = genPos[pos].transform.position;
        wall.Init(pos);
        //wall.transform.position = genPos[pos].transform.position + new Vector3(Player.unitDistance / 4f, 0, 0);
        wall.gameObject.SetActive(true);
        _walls.Add(wall);
    }

    /// <summary>
    /// 세로로 2칸
    /// </summary>
    /// <param name="empty">0아래, 2 위</param>
    void Generate1x2(int empty)
    {
        Wall wall = _wallPool.Get(canvasRect);
        int pos = empty == 0 ? 1 : 0;
        wall.gameObject.name = "1x2";

        wall.transform.position = genPos[pos].transform.position;
        wall.Init(pos);
        //wall.transform.position = new Vector3(genPos[pos].position.x, (genPos[pos].position.y + genPos[pos + 1].position.y) / 2f, 0);
        wall.gameObject.SetActive(true);
        _walls.Add(wall);
    }

    void Clear()
    {
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
        for (int i = 0; i < _points.Count; i++)
        {
            if (_points[i].transform.position.x <= limitXPos + _points[i].transform.lossyScale.x)
            {
                _pointPool.Release(_points[i]);
                _points.RemoveAt(i);
            }
        }
    }

    public void GeneratePoint(int pos)
    {
        Point point = _pointPool.Get(canvasRect);
        point.transform.position = genPos[pos].position;
        point.gameObject.SetActive(true);
        _points.Add(point);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        GameManager.Instance.Input.UpdateAction -= Clear;
        _bgController.Unsubscribe();
        gameOverUI.SetActive(true);
    }

    public void RetryBtn()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Unsubscribe()
    {
        GameManager.Instance.Input.UpdateAction -= Clear;
    }
}