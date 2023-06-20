using System;
using UnityEngine;

namespace FruitMatch.Scripts.MapScripts
{
    public class DemoButton : MonoBehaviour
    {

        public event EventHandler Click;

        public void OnMouseUpAsButton()
        {
            if (Click != null)
                Click(this, EventArgs.Empty);
        }

    }
}
