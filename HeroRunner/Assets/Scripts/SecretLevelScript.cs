using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretLevelScript : MonoBehaviour
{
    // Start is called before the first frame update

    private void Awake()
    {
        if (GameManager.Instance.GetMenuSoundBoolValue())
        {
            GameManager.Instance.PlayMenuMusic();
        }
    }

    void Start()
    {
        GameManager.Instance.UpdateMainCamera(GameObject.FindWithTag("MainCamera"));
    }
    
}
