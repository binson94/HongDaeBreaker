//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BGController : MonoBehaviour
//{
//    /// <summary>
//    /// ��� ��������Ʈ��
//    /// </summary>
//    [Header("BG Sprites")]
//    [SerializeField] Transform[] backgrounds;
//    /// <summary>
//    /// ��� �̵� �ӵ�
//    /// </summary>
//    [SerializeField] float speed;
//    /// <summary>
//    /// �������̵� ���� �� ���� ����
//    /// </summary>
//    [SerializeField] float bgRatio;
//    readonly float xSize = 0.03337609f;
//    readonly float ySize = 0.09278057f;

//    /// <summary>
//    /// ���� �Ѱ�, �� ��ġ���� �۾����� ������ ������ �̵�
//    /// </summary>
//    float limitXPos;

//    /// <summary>
//    /// ��� ��������Ʈ scale Vector
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
//        //���ϴ� ��ǥ, ��� ������ ����
//        Vector3 cameraPivotVector = Camera.main.ScreenToWorldPoint(Vector3.zero);
//        //��� ��������Ʈ ũ�� ���
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
//                //���� �ڷ� �̵�
//                t.position += new Vector3(backgrounds.Length * bgScale.x / xSize, 0, 0);
//        }
//    }
//}
