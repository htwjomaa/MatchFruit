/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLineCurveAdditonalPoint : MonoBehaviour
{
    public WorldLineCurvePoints parentPoint;
    public float timer = 0.075f;
    private float _cachedTimer = 0f;
    private bool _toggleButton = true;
    private void Awake()
    {
        transform.hasChanged = false;
        _cachedTimer = timer;
        //  GetComponent<Button> ().onClick.AddListener ( delegate { SubscribeToMove(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > -2f) timer -= Time.deltaTime;
        if (transform.hasChanged && timer < 0)
        {
            timer = _cachedTimer;
            transform.hasChanged = false;
            Debug.Log("Transform Changed");
            parentPoint.parentWorldLine.AdditionalPoints[transform.GetSiblingIndex()] = transform.position;
            Rl.worldMap.ReDrawLine(parentPoint.parentWorldLine, transform, transform.GetSiblingIndex() );
        }
    }
}
*/
