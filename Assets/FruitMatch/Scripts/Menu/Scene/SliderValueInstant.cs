using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//hate unity for this.  Have to do it...
public class SliderValueInstant : MonoBehaviour
{
    [SerializeField] private List<SliderSwitcher> sliderSwitchers;
   public void Init()
   {
       Initialize();
       Invoke(nameof(Initialize), 0.01f);
   }


   private void Initialize()
   {
       foreach (SliderSwitcher sliderSwitcher in sliderSwitchers) sliderSwitcher.Changed();
       foreach (SliderSwitcher sliderSwitcher in sliderSwitchers) sliderSwitcher.Changed();
   }
}
