using UnityEngine;

public class PatternScript : MonoBehaviour
{
  [SerializeField] public bool isActive = false;
  [SerializeField] public string patternName = "3Matches";

  public void InstantiateGameObject(string patternName)
  {
    if (patternName == "3Matches")
    {
      this.name = "3Matches";
      // var f = InstantiateGameObject();
    }
  }
}
