using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public HeroScript _heroScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hero")
        {
#if DEVELOPMENT
            Debug.Log($"Hero collied with {transform.name}");
#endif
            _heroScript.DecreaseHealth(1);
            
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hero")
        {
#if DEVELOPMENT
            Debug.Log($"Hero standing still on {other.gameObject.name}");
#endif
            _heroScript.DecreaseHealth(2);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hero")
        {
            _heroScript.DisableStayOnLaser();
        }
    }
}
