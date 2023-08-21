using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class TTPlayerCollider : MonoBehaviour
{
    [SerializeField] TTPlayer _player;
    [SerializeField] TutorialManager tutorialManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Wall wall = collision.transform.parent.GetComponent<Wall>();
            if (wall != null && wall.IsAttatched == false)
            {
                if(tutorialManager.canDamage)
                {
                    _player.GetDamage();
                    if(tutorialManager.oneShot)
                    {
                        _player.Point = 0;
                    }
                }
                else
                {
                    tutorialManager.pass3 = false;
                }
                wall.IsAttatched = true;
            }
        }
        else if (collision.tag == "Point")
        {
            _player.GetPoint();
            collision.gameObject.SetActive(false);
        }
    }
}

