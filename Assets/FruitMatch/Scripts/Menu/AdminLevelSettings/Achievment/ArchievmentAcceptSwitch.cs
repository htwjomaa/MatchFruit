using UnityEngine;
using Random = UnityEngine.Random;
public class ArchievmentAcceptSwitch : MonoBehaviour
{
    [SerializeField] private bool noStar;
    [SerializeField] private string noStarString;
    [SerializeField] private byte SwitchValue;
    [SerializeField] private Star Star;
    private void Start()
     {
         if (!noStar) ArchievmentButtons.NeededForNextLevel += GetSwitchValue;
         else ArchievmentButtons.NeededForNextLevel += GetSwitchValueNoStar;
     }
    private void OnDestroy()
     {
         ArchievmentButtons.NeededForNextLevel -= GetSwitchValue;
         ArchievmentButtons.NeededForNextLevel -= GetSwitchValueNoStar;
     }
    public void GetSwitchValueNoStar()
     {
         switch (noStarString)
         {
             case "NoEmpty":
                 SwitchValue = Rl.saveClipBoard.TrophyNoEmpty ? (byte)1 : (byte)0;
                 GenericSettingsFunctions.ActivateGameObject(gameObject, SwitchValue);
                 ToggleTrophyNoEmpty(true);
                 ToggleTrophyNoEmpty(true);
                 break;
             case "PointsOrMoves":
                 SwitchValue = Rl.saveClipBoard.TrophyNumberAreMovesOrTime ? (byte)1 : (byte)0;
                 GenericSettingsFunctions.ActivateGameObject(gameObject, SwitchValue);
                 ToggleTrophyNumbersAreMovesOrTime(true);
                 ToggleTrophyNumbersAreMovesOrTime(true);
                 break;
         }
     }
         
     public void GetSwitchValue()
    {
        switch (Star)
        {
            case Star.Star1:
                SwitchValue = Rl.saveClipBoard.Star1NeededNextLevel ? (byte)1 : (byte)0;
                break;
            case Star.Star2:
                SwitchValue = Rl.saveClipBoard.Star2NeededNextLevel ? (byte)1 : (byte)0;
                break;
            case Star.Star3:
                SwitchValue = Rl.saveClipBoard.Star3NeededNextLevel ? (byte)1 : (byte)0;
                break;
            case Star.Trophy:
                SwitchValue = Rl.saveClipBoard.TrophyNeededNextLevel ? (byte)1 : (byte)0;
                break;
        }

        GenericSettingsFunctions.ActivateGameObject(gameObject, SwitchValue);
        ToggleNextLevel(true);
        ToggleNextLevel(true);
    }

   
    public void ToggleNextLevel(bool playNoSound)
    {
        GenericSettingsFunctions.ActivateGameObject(gameObject, SwitchValue);
        switch (Star)
        {
            case Star.Star1:
                Rl.saveClipBoard.Star1NeededNextLevel = !Rl.saveClipBoard.Star1NeededNextLevel;
                break;
            case Star.Star2: 
                Rl.saveClipBoard.Star2NeededNextLevel = !Rl.saveClipBoard.Star2NeededNextLevel;
                break;
            case Star.Star3: 
                Rl.saveClipBoard.Star3NeededNextLevel  = !Rl.saveClipBoard.Star3NeededNextLevel;
                break;
            case Star.Trophy:
                Rl.saveClipBoard.TrophyNeededNextLevel = !Rl.saveClipBoard.TrophyNeededNextLevel;
                break;
        }
        SwitchValue++;
        if (SwitchValue > transform.childCount - 1)
            SwitchValue = 0;
        
       if(!playNoSound) Rl.GameManager.PlayAudio(Rl.soundStrings.AcceptSwitchSound, Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    }
    public void ToggleTrophyNoEmpty(bool playNoSound)
    {
        GenericSettingsFunctions.ActivateGameObject(gameObject, SwitchValue);
        if(SwitchValue == 0)   Rl.saveClipBoard.TrophyNoEmpty =true;
          else if(SwitchValue == 1) Rl.saveClipBoard.TrophyNoEmpty = false;
        SwitchValue++;
        ArchievmentButtons.InvokeChangeTrophyEvent();
        if (SwitchValue > transform.childCount - 1) SwitchValue = 0;
        if(!playNoSound) Rl.GameManager.PlayAudio(Rl.soundStrings.AcceptSwitchSound, Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    }
    public void ToggleTrophyNumbersAreMovesOrTime(bool playNoSound)
    {
        GenericSettingsFunctions.ActivateGameObject(gameObject, SwitchValue);
        if(SwitchValue == 0)    Rl.saveClipBoard.TrophyNumberAreMovesOrTime  =true;
        else if(SwitchValue == 1)  Rl.saveClipBoard.TrophyNumberAreMovesOrTime  = false;
        SwitchValue++;
        ArchievmentButtons.InvokeChangeTrophyEvent();
        if (SwitchValue > transform.childCount - 1) SwitchValue = 0;
        if(!playNoSound) Rl.GameManager.PlayAudio(Rl.soundStrings.AcceptSwitchSound, Random.Range(0,5), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    }
}
