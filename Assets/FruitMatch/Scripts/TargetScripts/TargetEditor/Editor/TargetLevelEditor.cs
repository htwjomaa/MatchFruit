using System;
using System.Collections.Generic;
using System.Linq;
using FruitMatch.Scripts.Level;
using FruitMatch.Scripts.TargetScripts.TargetSystem;
using Malee.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FruitMatch.Scripts.TargetScripts.TargetEditor.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(TargetLevel))]
    public class TargetLevelEditor : UnityEditor.Editor
    {
        //empty
    }
}