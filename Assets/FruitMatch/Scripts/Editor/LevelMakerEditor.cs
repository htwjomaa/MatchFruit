using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FruitMatch.Scriptable.Rewards;
using FruitMatch.Scripts.Blocks;
using FruitMatch.Scripts.Core;
using FruitMatch.Scripts.GUI;
using FruitMatch.Scripts.GUI.Boost;
using FruitMatch.Scripts.Items;
using FruitMatch.Scripts.Level;
using FruitMatch.Scripts.Level.ItemsPerLevel.Editor;
using FruitMatch.Scripts.MapScripts.StaticMap.Editor;
using FruitMatch.Scripts.System;
using FruitMatch.Scripts.TargetScripts.TargetEditor;
using FruitMatch.Scripts.TargetScripts.TargetSystem;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace FruitMatch.Scripts.Editor
{
    [InitializeOnLoad]
    public class LevelMakerEditor : EditorWindow
    {
        //empty
    }
}