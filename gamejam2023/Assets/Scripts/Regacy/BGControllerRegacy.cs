//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BGController : MonoBehaviour
//{
//    /// <summary>
//    /// 배경 스프라이트들
//    /// </summary>
//    [Header("BG Sprites")]
//    [SerializeField] Transform[] backgrounds;
//    /// <summary>
//    /// 배경 이동 속도
//    /// </summary>
//    [SerializeField] float speed;
//    /// <summary>
//    /// 스프라이드 가로 대 세로 비율
//    /// </summary>
//    [SerializeField] float bgRatio;
//    readonly float xSize = 0.03337609f;
//    readonly float ySize = 0.09278057f;

//    /// <summary>
//    /// 왼쪽 한계, 이 수치보다 작아지면 오른쪽 끝으로 이동
//    /// </summary>
//    float limitXPos;

//    /// <summary>
//    /// 배경 스프라이트 scale Vector
//    /// </summary>
//    Vector3 bgScale;

//    void Start()
//    {
//        SetBGScale();

//        GameManager.Instance.Input.FixedUpdateAction -= BGMove;
//        GameManager.Instance.Input.FixedUpdateAction += BGMove;
//    }

//    void SetBGScale()
//    {
//        //좌하단 좌표, 배경 스케일 계산용
//        Vector3 cameraPivotVector = Camera.main.ScreenToWorldPoint(Vector3.zero);
//        //배경 스프라이트 크기 계산
//        bgScale = new Vector3(bgRatio * Mathf.Abs(2 * cameraPivotVector.y) * xSize, Mathf.Abs(2 * cameraPivotVector.y) * ySize, 0);
//        limitXPos = -cameraPivotVector.x + bgScale.x / xSize / 2f;
//        float bgPos = limitXPos = cameraPivotVector.x - bgScale.x / xSize / 2f;

//        for (int i = 0; i < backgrounds.Length; i++)
//        {
//            backgrounds[i].localScale = bgScale;
//            bgPos += bgScale.x / xSize;
//            backgrounds[i].position = new Vector3(bgPos, 0, 0);
//        }
//    }

//    void BGMove()
//    {
//        foreach (Transform t in backgrounds)
//        {
//            if (t.position.x <= Camera.main.transform.position.x + limitXPos)
//                //제일 뒤로 이동
//                t.position += new Vector3(backgrounds.Length * bgScale.x / xSize, 0, 0);
//        }
//    }
//}
