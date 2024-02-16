using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    /// <summary>
    /// Crate types
    /// 1 = Fuel
    /// 2 = Live
    /// 3 = Points
    /// 4 = Secret key
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hero")
        {
            if (gameObject.tag == "CrateFuel1" || gameObject.tag == "CrateFuel2" || gameObject.tag == "CrateFuel3")
            {
                if (FindObjectOfType<HeroScript>().GetHealth() >= 100f)
                    return;
                FindObjectOfType<HeroScript>().GetCrateBenefit(1);
                gameObject.SetActive(false);
            }
            if (gameObject.tag == "CrateLive1" || gameObject.tag == "CrateLive2" || gameObject.tag == "CrateLive3")
            {
                FindObjectOfType<HeroScript>().GetCrateBenefit(2);
                gameObject.SetActive(false);
            }
            if (gameObject.tag == "CratePoint1" || gameObject.tag == "CratePoint2" || gameObject.tag == "CratePoint3")
            {
                FindObjectOfType<HeroScript>().GetCrateBenefit(3);
                gameObject.SetActive(false);
            }
            if (gameObject.tag == "CrateSecretKey1" || gameObject.tag == "CrateSecretKey2" || gameObject.tag == "CrateSecretKey3")
            {
                FindObjectOfType<HeroScript>().GetCrateBenefit(4);
                gameObject.SetActive(false);
            }
        }
    }
}
