using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyObj : MonoBehaviour
{
    private float timer = 10;
 
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0) Destroy(gameObject);
    }
}
