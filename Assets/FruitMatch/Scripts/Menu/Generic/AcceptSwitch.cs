using System;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class AcceptSwitch : MonoBehaviour
{
    [SerializeField] private byte SwitchValue;
    [SerializeField] private Colors color;

    private void Awake()
     {  //has to be on Awake because other subscribe on Start
         SwitchButtonFruitsManager.LoadFruitEvent += GetSwitchValue;
     }

     private void OnDestroy()
     {
         SwitchButtonFruitsManager.LoadFruitEvent -= GetSwitchValue;
     }

     public void GetSwitchValue()
    {
        for (int i = 0; i < Rl.saveClipBoard.FruitClipboardParent.Length;i++)
        {
            if (SwitchButtonFruitsManager.GetFruitEntry(Rl.saveClipBoard.FruitClipboardParent[i]))
            {
                for (int j = 0; j < Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards.Length; j++)
                {
                    if (color == Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].FruitColor)
                    {
                        switch (Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].phase)
                        {
                            case Phase.NotInP2:
                                SwitchValue  = 0;
                                break;
                            case   Phase.OnlyInP2:
                                SwitchValue= 1;
                                break;
                            case Phase.InP1AndP2:
                                SwitchValue = 2;
                                break;
                        }
                    }
                }
            }
        }

        GenericSettingsFunctions.ActivateGameObject(transform, SwitchValue);
    }
     
    public void ClickOnSwitch(bool addValue)
    {
        Phase phase = Phase.NotInP2;
        if (addValue)
        {   for (int i = 0; i < Rl.saveClipBoard.FruitClipboardParent.Length;i++)
            {
                if (SwitchButtonFruitsManager.GetFruitEntry(Rl.saveClipBoard.FruitClipboardParent[i]))
                {
                    for (int j = 0; j < Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards.Length; j++)
                    {
                        if (color == Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].FruitColor)
                        {
                            switch (SwitchValue)
                            {
                                case 2:
                                    Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].phase =
                                        Phase.NotInP2;
                                    break;
                                case 0:
                                    Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].phase =
                                        Phase.OnlyInP2;
                                    break;
                                case 1:
                                    Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].phase =
                                        Phase.InP1AndP2;
                                    break;
                            }
                        }
                    }
                }
            }
            SwitchValue++;
        }

        else
        {
            for (int i = 0; i < Rl.saveClipBoard.FruitClipboardParent.Length;i++)
            {
                if (SwitchButtonFruitsManager.GetFruitEntry(Rl.saveClipBoard.FruitClipboardParent[i]))
                {
                    for (int j = 0; j < Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards.Length; j++)
                    {
                        if (color == Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].FruitColor)
                        {
                            switch (SwitchValue)
                            {
                                case 0:
                                    phase = Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].phase;
                                    break;
                                case 1:
                                    phase = Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].phase;
                                    break;
                                case 2:
                                    phase = Rl.saveClipBoard.FruitClipboardParent[i].fruitClipboards[j].phase;
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
                    }
                }
            }
        }
        
        if (SwitchValue > transform.childCount - 1)
            SwitchValue = 0;
        
        GenericSettingsFunctions.ActivateGameObject(transform, SwitchValue);
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i) !=null)     GenericSettingsFunctions.SmallJumpAnimation(transform.GetChild(i).transform);
        }
        Rl.GameManager.PlayAudio(Rl.soundStrings.AcceptSwitchSound, Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
    }
}