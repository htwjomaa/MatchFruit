using FruitMatch.Scripts.TargetScripts.TargetSystem;
using UnityEngine;

namespace FruitMatch.Scripts.GUI
{
    public class TargetHolder : MonoBehaviour
    {
        public static TargetContainer target;
        public static int level;
        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(this);
        }
    }
}
