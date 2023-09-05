using TMPro;
using UnityEngine;

public class AdminLevelSettingsSequenceGoal : MonoBehaviour
{   
    [SerializeField] private GoalNumber goalNumber;
    [SerializeField] private bool isFruit;
    
    private TextMeshProUGUI _enumText; 
    private TextMeshProUGUI _sibling;

    private void Start() => AdminLevelSettingsMatchFinder.matchFinderLoadTrigger += MatchSpriteToEntry;
    private void OnDestroy() => AdminLevelSettingsMatchFinder.matchFinderLoadTrigger -= MatchSpriteToEntry;

    public void ClickNextFruitOrColor() => Rl.AdminLevelSettingsMatchFinder.ClickNextFruitOrColor(goalNumber, isFruit, ref _enumText, ref _sibling );
    public void LoadSameFruitOrColor() => Rl.AdminLevelSettingsMatchFinder.LoadSame(goalNumber, isFruit, ref _enumText, ref _sibling );
    
    private void Awake()
    {
        _enumText = GetComponent<TextMeshProUGUI>();
        _sibling = GetSibling();
    }
    
    private  TextMeshProUGUI GetSibling()
    {
        int siblingIndex = transform.GetSiblingIndex();
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if(i != siblingIndex &&  transform.parent.GetChild(i).GetComponent<TextMeshProUGUI>() != null)
                return transform.parent.GetChild(i).GetComponent<TextMeshProUGUI>();
        }
        return null;
    }
    private void MatchSpriteToEntry()
    {
        Rl.AdminLevelSettingsMatchFinder.LoadCurrentGoalSetting(goalNumber, isFruit, ref _enumText, ref _sibling);
    }
}