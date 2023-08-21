using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAnim : MonoBehaviour
{
    public Wall wall;

    public void HideImage()
    {
        wall?.HideImage();
    }

    public void HitEnd()
    {
        Destroy(gameObject);
    }
}
