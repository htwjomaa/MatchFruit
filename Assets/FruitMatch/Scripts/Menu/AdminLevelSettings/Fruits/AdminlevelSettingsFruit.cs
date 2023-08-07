using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AdminlevelSettingsFruit : MonoBehaviour
{
    public void DeactivateObject(GameObject panel)
    {
        if (panel != null)
        {
            StartCoroutine(DeactiveObject_CO(panel, 0.03f));
        }
    }

    IEnumerator DeactiveObject_CO(GameObject panel, float waitForSec)
    {
        yield return new WaitForSeconds(waitForSec);
        panel.SetActive(false);
    }
}
