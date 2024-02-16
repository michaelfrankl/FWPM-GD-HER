using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPrefab : MonoBehaviour
{
    public int PrefabToRandomize;

    public GameObject PrefabSensorToActivate;

    public RandomizerScript _randomizerScript;

    public GunScript _gunScript;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hero")
        {
#if DEVELOPMENT
            Debug.Log($"Randomized the Prefab{PrefabToRandomize}");
#endif
            gameObject.SetActive(false);
            _randomizerScript.RandomizePrefab(PrefabToRandomize);
            GameManager.Instance.UpdateWeaponStatus(false);
            PrefabSensorToActivate.SetActive(true);
            if (PrefabToRandomize == 3)
            {
                _randomizerScript.DisableAllCratesForSpecificPrefab(1);
                _gunScript.DisableBullet(1);
                _gunScript.ResetBulletCoordinates(1);
                GameManager.Instance.ResetBulletCount();
            }
            else if (PrefabToRandomize == 2)
            {
                _randomizerScript.DisableAllCratesForSpecificPrefab(3);
                _gunScript.DisableBullet(3);
                _gunScript.ResetBulletCoordinates(3);
                GameManager.Instance.ResetBulletCount();
            }
            else if (PrefabToRandomize == 1)
            {
                _randomizerScript.DisableAllCratesForSpecificPrefab(2);
                _gunScript.DisableBullet(2);
                _gunScript.ResetBulletCoordinates(2);
                GameManager.Instance.ResetBulletCount();
            }
        }
    }
}
