using System.Collections.Generic;
using UnityEngine;
public class WorldLineCurvePoints : MonoBehaviour
{
    public WorldLine parentWorldLine;
    public float timer = 0.075f;
    private float _cachedTimer = 0f;
    private bool _toggleButton = true;
    
    private void Awake()
    {
        transform.hasChanged = false;
        _cachedTimer = timer;
      //  GetComponent<Button> ().onClick.AddListener ( delegate { SubscribeToMove(); });
    }

    void Update()
    {
        if (timer > -2f) timer -= Time.deltaTime;
        if (transform.hasChanged && timer < 0)
        {
            timer = _cachedTimer;
            transform.hasChanged = false;
            parentWorldLine.ControlPoints[transform.GetSiblingIndex()] = transform.position;
            Rl.worldMap.ReDrawLine(parentWorldLine, transform, transform.parent.GetSiblingIndex() );
        }
    }

    
   
    private void SubscribeToMove()
    {
    //    if (_toggleButton) MoveLevelElements.selectedGameObject = gameObject;
      //  else MoveLevelElements.selectedGameObject = null;
     //  _toggleButton = !_toggleButton;
    }
}