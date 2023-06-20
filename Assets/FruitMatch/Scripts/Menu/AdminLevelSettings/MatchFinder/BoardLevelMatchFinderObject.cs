using UnityEngine;

public class BoardLevelMatchFinderObject : MonoBehaviour
{
  [SerializeField] public Row Row;
  [SerializeField] public Diagonal Diagonal;
  [SerializeField] public Pattern Pattern;
  
  [SerializeField] public IsMatchStyle IsMatchStyle;
  public void ClickNextStyle() => Rl.AdminLevelSettingsMatchFinder.ClickNext(IsMatchStyle, ref Row, ref Diagonal, ref Pattern );
}