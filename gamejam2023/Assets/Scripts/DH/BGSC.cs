using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSC : MonoBehaviour
{
    public float speed;
    public Transform[] backgrounds;

    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].position += new Vector3(-speed, 0, 0) * Time.deltaTime;
            if (backgrounds[i].position.x < -40)
            {
                Vector3 nextPos = backgrounds[i].position;
                nextPos = new Vector3(nextPos.x + 99.9999972f, nextPos.y, nextPos.z);
                backgrounds[i].position = nextPos;
            }
        }
    }
}