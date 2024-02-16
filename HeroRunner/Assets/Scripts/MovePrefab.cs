using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class MovePrefab : MonoBehaviour
{
    public GameObject prefabToMove, sensorToActivate;
    
    private float deltaDX;

    void Start()
    {
        deltaDX = 76.596565f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hero")
        {
#if DEVELOPMENT
            Debug.Log($"Hero collied with {prefabToMove.name} and {sensorToActivate.name}");
            Debug.Log($"Find from parent Object: "+prefabToMove.transform.Find("FuelCan"));
#endif
            gameObject.SetActive(false);
            prefabToMove.transform.position = new Vector3(prefabToMove.transform.position.x + deltaDX,
                prefabToMove.transform.position.y, prefabToMove.transform.position.z);
            sensorToActivate.SetActive(true);
        }
    }
}
