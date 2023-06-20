using UnityEngine;
using UnityEngine.UI;

public class FruitUI : MonoBehaviour
{
     public FruitType fruitType;
     private void Start() => gameObject.GetComponent<Image>().sprite = Rl.world.GetGoalSprite(fruitType);
     public void ChangeFruitType()
     {
          SwitchButtonFruitsManager.FruitType = GetComponentInChildren<FruitUI>().fruitType;
          SwitchButtonFruitsManager.InvokeLoadEvent();
     }
}
