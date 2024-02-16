using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FuelCan : MonoBehaviour
{
    public HeroScript heroScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hero")
        {
            if (heroScript.GetHealth() < 100f)
            {
                gameObject.SetActive(false);
                heroScript.IncreaseHealth();
            }
        }
    }
}
