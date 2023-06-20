using UnityEngine;
public class FruitMenu : MonoBehaviour
{
  [SerializeField] public GameObject FruitPanel;
  private void OnEnable() => FruitPanel.SetActive(true);
  private void OnDisable() => FruitPanel.SetActive(false);
}