using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] Player _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Wall wall = collision.transform.parent.GetComponent<Wall>();
            if(wall != null && wall.IsAttatched == false)
            {
                _player.GetDamage();
                wall.IsAttatched = true;
                wall.PlayDestroy();
            }
        }
        else if(collision.tag == "Point")
        {
            _player.GetPoint();
            SoundManager.instance.playSFX(SoundManager.SFX.ItemGet);
            collision.gameObject.SetActive(false);
        }
    }
}
