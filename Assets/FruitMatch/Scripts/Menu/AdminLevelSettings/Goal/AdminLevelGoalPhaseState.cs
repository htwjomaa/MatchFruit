using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminLevelGoalPhaseState: MonoBehaviour
{
    [SerializeField]private PhaseNumber phaseNumber;

    private void Awake()
    {
        AdminLevelSettingsGoalConfig.OnPhaseSelect += ResetTab;
    }

    private void OnDestroy()
    {
        AdminLevelSettingsGoalConfig.OnPhaseSelect -= ResetTab;
    }

    public void ChangePhaseNumberState(bool playNoSound)
    {
        AdminLevelSettingsGoalConfig.SelectedPhase = this;
        Rl.adminLevelSettingsGoalConfig.ChangePhaseNumberState(playNoSound, phaseNumber);
    }
    
    private void ResetTab() => Rl.adminLevelSettingsGoalConfig.ResetPhaseTabs(this);
}