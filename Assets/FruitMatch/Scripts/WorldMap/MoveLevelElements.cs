using UnityEngine;
public class MoveLevelElements : MonoBehaviour
{
   public static GameObject selectedGameObject;
   public bool startMoving = false;
   private float _timer = 0.125f;
   private void Update()
   {
      if(_timer > -2) _timer -= Time.deltaTime;
      
      if (Input.GetKey(KeyCode.Mouse0) && _timer < 0)
      {
         Ray ray = Rl.Cam.ScreenPointToRay(Input.mousePosition);
         RaycastHit hit;
         if (Physics.Raycast(ray, out hit, Mathf.Infinity))
         {
            startMoving = !startMoving;
            selectedGameObject = hit.collider.gameObject;
         }
      
         _timer = 0.125f;
      }
      if (startMoving == false) selectedGameObject = null;
    
      if (selectedGameObject == null) return;
      Vector3 newPos = Rl.Cam.ScreenToWorldPoint(Input.mousePosition);
      selectedGameObject.transform.position = new Vector3(newPos.x, newPos.y, selectedGameObject.transform.position.z);
   }
   private void OnDestroy() => selectedGameObject = null;
}
