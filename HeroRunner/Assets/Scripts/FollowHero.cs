using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHero : MonoBehaviour
{
    public GameObject heroCopy;

    // Update is called once per frame
    void Update()
    {
        if (heroCopy.transform.position.x > transform.position.x)
        {
            transform.position = new Vector3(heroCopy.transform.position.x, transform.position.y, transform.position.z);
        }
    }
}
