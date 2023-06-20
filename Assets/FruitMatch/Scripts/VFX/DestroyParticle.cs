using UnityEngine;

public sealed class DestroyParticle : MonoBehaviour {

    private float delayTimer = 1;
    void Update () {
        delayTimer -= Time.deltaTime;
        if(delayTimer<= 0){
            Destroy(this.gameObject);
        }
	}
}
