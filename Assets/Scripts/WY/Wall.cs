using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wall : MonoBehaviour
{
    public bool IsAttatched;
    [SerializeField] Image bgImage;
    [SerializeField] Image mainImage;
    [SerializeField] HitAnim hitAnim;

    public void Init(int y)
    {
        IsAttatched = false;
        bgImage.sprite = GameManager.Instance.SpriteHandle.LoadSprite($"WallBG{y}");
        mainImage.sprite = GameManager.Instance.SpriteHandle.LoadSprite($"Wall{Random.Range(1, 6)}");
        mainImage.gameObject.SetActive(true);
        bgImage.gameObject.SetActive(true);
    }

    public void PlayDestroy()
    {
        HitAnim hit = Instantiate(hitAnim);
        hit.transform.SetParent(mainImage.transform);
        hit.transform.position = mainImage.transform.position;
        hit.transform.localScale = Vector3.one;
        hit.transform.SetParent(transform);
        hit.wall = this;
    }

    public void HideImage()
    {
        mainImage.gameObject.SetActive(false);
        bgImage.gameObject.SetActive(false);
    }
}
