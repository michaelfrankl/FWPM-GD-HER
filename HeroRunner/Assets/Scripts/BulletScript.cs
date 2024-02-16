using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public HeroScript _heroScript;
    public GunScript _gunScript;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Prefab1")
        {
            gameObject.SetActive(false);
            _heroScript.PlayBulletHitSound();
            _gunScript.ResetBulletCoordinates(1);
        }
        else if (other.gameObject.tag == "Prefab2")
        {
            gameObject.SetActive(false);
            _heroScript.PlayBulletHitSound();
            _gunScript.ResetBulletCoordinates(2);
        }
        else if (other.gameObject.tag == "Prefab3")
        {
            gameObject.SetActive(false);
            _heroScript.PlayBulletHitSound();
            _gunScript.ResetBulletCoordinates(3);
        }
#if DEVELOPMENT
            Debug.Log($"Bullet collied with Prefab {other.gameObject.tag}");
#endif

        if (other.gameObject.tag == "Hero")
        {
            if (gameObject.tag == "Prefab1_GunBullet")
                 _gunScript.DisableBullet(1);
            if (gameObject.tag == "Prefab2_GunBullet")
                _gunScript.DisableBullet(2);
            if (gameObject.tag == "Prefab3_GunBullet")
                _gunScript.DisableBullet(3); ;
            _heroScript.PlayBulletHitSound();
            _heroScript.DecreaseHealth(3);
#if DEVELOPMENT
            Debug.Log($"Bullet collied with Hero {other.gameObject.tag}");
#endif
        }
        else if (other.gameObject.tag == "Prefab1_FuelCan" || other.gameObject.tag == "Prefab2_FuelCan" ||
                 other.gameObject.tag == "Prefab3_FuelCan")
        {
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
            _heroScript.PlayFuelDestroySound();
#if DEVELOPMENT
            Debug.Log($"Bulled collied with FuelCan {other.gameObject.name}");
#endif
        }
        else if (other.gameObject.tag == "Sensor1" || other.gameObject.tag == "Sensor2" ||
                 other.gameObject.tag == "Sensor3")
#if DEVELOPMENT
            Debug.Log($"Bulled collied with Sensor:  {other.gameObject.tag}");
#endif
        
        if (other.gameObject.tag == "Prefab1_Laser")
        {
            gameObject.SetActive(false);
            _heroScript.PlayBulletHitSound();
            _gunScript.ResetBulletCoordinates(1);
        }
        else if (other.gameObject.tag == "Prefab2_Laser")
        {
            gameObject.SetActive(false);
            _heroScript.PlayBulletHitSound();
            _gunScript.ResetBulletCoordinates(2);
        }
        else if (other.gameObject.tag == "Prefab3_Laser")
        {
            gameObject.SetActive(false);
            _heroScript.PlayBulletHitSound();
            _gunScript.ResetBulletCoordinates(3);
        }
        else if (other.gameObject.tag == "CameraFollower")
        {
            gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "CrateFuel1" || other.gameObject.tag == "CrateFuel2" ||
            other.gameObject.tag == "CrateFuel3")
        {
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
            _heroScript.PlayBulletHitSound();
#if DEVELOPMENT
            Debug.Log($"Bullet hit FuelCrate: {other.gameObject.tag}");
#endif
        }

        if (other.gameObject.tag == "CrateLive1" || other.gameObject.tag == "CrateLive2" ||
            other.gameObject.tag == "CrateLive3")
        {
            gameObject.SetActive(false);
            _heroScript.PlayBulletHitSound();
            other.gameObject.SetActive(false);
#if DEVELOPMENT
            Debug.Log($"Bullet hit LiveCrate: {other.gameObject.tag}");
#endif
        }

        if (other.gameObject.tag == "CratePoint1" || other.gameObject.tag == "CratePoint2" || 
            other.gameObject.tag == "CratePoint3")
        {
            gameObject.SetActive(false);
            _heroScript.PlayBulletHitSound();
            other.gameObject.SetActive(false);
#if DEVELOPMENT
            Debug.Log($"Bullet hit PointCrate: {other.gameObject.tag}");
#endif
        }

        
        if (other.gameObject.tag == "CrateSecretKey1" || other.gameObject.tag == "CrateSecretKey2" || 
            other.gameObject.tag == "CrateSecretKey3")
        {
            gameObject.SetActive(false);
            _heroScript.PlayBulletHitSound();
            other.gameObject.SetActive(false);
#if DEVELOPMENT
            Debug.Log($"Bullet hit KeyCrate: {other.gameObject.tag}");
#endif
        }
    }
}
