using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SwitchButton : MonoBehaviour
{
    [SerializeField] private GameObject SwitchButtonOn;
    [SerializeField] private GameObject SwitchBackgroundOn;

    
    [SerializeField] private GameObject SwitchButtonOff;
    [SerializeField] private GameObject SwitchBackgroundOff;
    
    // Start is called before the first frame update

    private void Start() => SetSwitches();

    public void SetSwitches()
    {
        if(Rl.settings.ColorBlind){ SwitchOn(true);}
        else SwitchOff(true);
        
        if(!Rl.world.UnlockDepensOnStars){ SwitchOnUnlockAllLevels(true);}
        else SwitchOffUnlockAllLevels(true);
        
        if(Rl.settings.KeepAdminAccess){SwitchOnKeepAdminAccessEnabled(true);}
        else SwitchOffKeepAdminAccessEnabled(true);
        
    }

    public void SwitchOff(bool playNoSound)
    {
        Rl.settings.ColorBlind = false;
        SwitchThings(false, playNoSound);
    }
    
    public void SwitchOn(bool playNoSound)
    {
        Rl.settings.ColorBlind = true;
        SwitchThings(true, playNoSound);
    }
    

    public void SwitchOffUnlockAllLevels(bool playNoSound)
    {
        Rl.splashMenu.LockLevelsAgain();
        SwitchThings(false, playNoSound);
    }
    
    public void SwitchOnUnlockAllLevels(bool playNoSound)
    {
        Rl.splashMenu.UnlockAllLevels();
        SwitchThings(true, playNoSound);
    }

    
    public void SwitchOffAdminAccess(bool playNoSound)
    {
        
        Rl.settingsPanelManager.ActivateOrDeactiveAdminButtons(false);
        SwitchThings(false, playNoSound);
        Rl.splashMenu.SplashSettingsTabGroup.tabButtons.First().GetComponent<TabButton>().ClickThisButtonFromOtherScript();
        SwitchOffKeepAdminAccessEnabled(true);
    }

    

    public void SwitchOffKeepAdminAccessEnabled(bool playNoSound)
    {
    
        Rl.settings.KeepAdminAccess = false;
        SwitchThings(false, playNoSound);
        Rl.saveFileManagerInMenu.SaveGame();
    }
    
    public void SwitchOnKeepAdminAccessEnabled(bool playNoSound)
    {
        Rl.settings.KeepAdminAccess = true;
        SwitchThings(true, playNoSound);
        Rl.saveFileManagerInMenu.SaveGame();
    }
    
    private void SwitchThings(bool on, bool playNoSound)
    {
       
        if (!playNoSound)
        {
         if(on)   Rl.GameManager.PlayAudio(Rl.soundStrings.SwitchButtonSound, Random.Range(2,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
         if(!on)  Rl.GameManager.PlayAudio(Rl.soundStrings.SwitchButtonSound, Random.Range(0,2), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        } 
        SwitchBackgroundOn.SetActive(on);
        SwitchButtonOn.SetActive(on);
        
        SwitchButtonOff.SetActive(!on);
        SwitchBackgroundOff.SetActive(!on);
    }
    
}