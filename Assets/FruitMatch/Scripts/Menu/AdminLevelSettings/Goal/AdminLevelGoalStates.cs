using System;
using UnityEngine;
public class AdminLevelGoalStates : MonoBehaviour
{
    [SerializeField] private ObjectiveNumber objectiveNumber;
    private void Awake() => AdminLevelSettingsGoalConfig.OnObjectiveSelect += ResetTab;

    private void OnDestroy()
    {
        AdminLevelSettingsGoalConfig.OnObjectiveSelect -= ResetTab;
    }

    public void ChangeObjectiveNumberState(bool playNoSound)
    {
        AdminLevelSettingsGoalConfig.SelectedObjective = this;
        AdminLevelSettingsGoalConfig.IsObjectiveSetting = false;
        Rl.adminLevelSettingsGoalConfig.ChangeObjectiveNumberState(playNoSound, objectiveNumber);
 
    }
    public void ChangeObjectiveSettingNumberState(bool playNoSound)
    {
       // Rl.adminLevelSettingsGoalConfig.ChangeObjectiveNumberState(objectiveNumber);
       AdminLevelSettingsGoalConfig.IsObjectiveSetting = true;
        AdminLevelSettingsGoalConfig.SelectedObjective = this;
        
        Rl.adminLevelSettingsGoalConfig.ChangeObjectiveSettingState(objectiveNumber);
    }
    private void ResetTab() => Rl.adminLevelSettingsGoalConfig.ResetTabs(this);

    public void ChangeObjectiveNumberStateNoSound() => ChangeObjectiveNumberState(true);

    public void ClickThisButtonFromAnotherObject()
    {
        //Doing that because objects might not be active and this works
        Invoke(nameof(ChangeObjectiveNumberStateNoSound), 0.01f);
       Invoke(nameof(ChangeObjectiveNumberStateNoSound), 0.015f);
       Invoke(nameof(ChangeObjectiveNumberStateNoSound), 0.02f);
    }
}