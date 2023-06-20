using UnityEngine;
using Random = UnityEngine.Random;

public sealed class  AdminLevelSettingsMatchFinderAcceptSwitch : MonoBehaviour
{
    [SerializeField] private byte SwitchValue;
    [SerializeField] private IsMatchStyle isMatchStyle;

    private void Awake()
     {  //has to be on Awake because other subscribe on Start
         AdminLevelSettingsMatchFinder.matchFinderLoadTrigger+= GetSwitchValue;
         AdminLevelSettingsMatchFinder.matchFinderLoadTriggerDebug += ClickOnSwitchNoAddValue;
     }

     public void GetSwitchValue()
     {
         Phase phase = Phase.NotInP2;

         switch (isMatchStyle)
         {
             case IsMatchStyle.IsRow:
                 phase = Rl.saveClipBoard.RowPhase;
                 break;
             case IsMatchStyle.IsDiagonal: 
                 phase = Rl.saveClipBoard.DiagonalPhase;
                 break;
             case IsMatchStyle.IsPattern1:
                 phase = Rl.saveClipBoard.Pattern1Phase;
                 break;
             case IsMatchStyle.IsPattern2:
                 phase = Rl.saveClipBoard.Pattern2Phase;
                 break;
         }
         switch (phase)
         {
             case Phase.NotInP2:
                 SwitchValue = 0;
                 break;
             case Phase.OnlyInP2:
                 SwitchValue = 1;
                 break;
             case Phase.InP1AndP2:
                 SwitchValue = 2;
                 break;
         }

         GenericSettingsFunctions.ActivateGameObject(transform, SwitchValue);
     }

     public void ClickOnSwitchNoAddValue() => ClickOnSwitch(false);

    public void ClickOnSwitch(bool addValue)
    {
        
        Phase phase = Phase.NotInP2;

        if (addValue)
        {
            switch (SwitchValue)
            {
                case 2:
                    phase = Phase.NotInP2;
                    break;
                case 0:
                    phase = Phase.OnlyInP2;
                    break;
                case 1:
                    phase = Phase.InP1AndP2;
                    break;
            }
            switch (isMatchStyle)
            {
                case IsMatchStyle.IsRow:
                    Rl.saveClipBoard.RowPhase = phase;
                    break;
                case IsMatchStyle.IsDiagonal: 
                    Rl.saveClipBoard.DiagonalPhase = phase;
                    break;
                case IsMatchStyle.IsPattern1:
                    Rl.saveClipBoard.Pattern1Phase = phase;
                    break;
                case IsMatchStyle.IsPattern2:
                    Rl.saveClipBoard.Pattern2Phase = phase;
                    break;
            }
            Rl.GameManager.PlayAudio(Rl.soundStrings.AcceptSwitchSound, Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
            SwitchValue++;
        }
        else
        {
            switch (isMatchStyle)
            {
                case IsMatchStyle.IsRow:
                    phase =  Rl.saveClipBoard.RowPhase;
                    break;
                case IsMatchStyle.IsDiagonal: 
                     phase = Rl.saveClipBoard.DiagonalPhase;
                    break;
                case IsMatchStyle.IsPattern1:
                    phase = Rl.saveClipBoard.Pattern1Phase ;
                    break;
                case IsMatchStyle.IsPattern2:
                  phase =   Rl.saveClipBoard.Pattern2Phase ;
                    break;
            }
            switch (phase)
            {
                case Phase.NotInP2:
                    SwitchValue = 0;
                    break;
                case Phase.OnlyInP2:
                    SwitchValue = 1;
                    break;
                case Phase.InP1AndP2:
                    SwitchValue = 2;
                    break;
            }
        }
            
     
        if (SwitchValue > transform.childCount - 1)
            SwitchValue = 0;
        
        GenericSettingsFunctions.ActivateGameObject(transform, SwitchValue);
    }
}