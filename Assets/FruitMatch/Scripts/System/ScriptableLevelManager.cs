using System;
using System.IO;
using FruitMatch.Scripts.Level;
using FruitMatch.Scripts.TargetScripts.TargetSystem;
using UnityEngine;
using UnityEditor;
namespace FruitMatch.Scripts.System
{
    public static class ScriptableLevelManager
    {
        #if UNITY_EDITOR
        public static void CreateFileLevel(int level, LevelData _levelData)
        {
            var path = "Assets/SweetSugar/Resources/Levels/";

            if (Resources.Load("Levels/Level_" + level))
            {
                SaveLevel(path, level, _levelData);
            }
            else
            {
                string fileName = "Level_" + level;
                var newLevelData = ScriptableObjectUtility.CreateAsset<LevelContainer>(path, fileName);
                newLevelData.SetData(_levelData.DeepCopy(level));
                EditorUtility.SetDirty(newLevelData);
                AssetDatabase.SaveAssets();
            }
        }
        public static void SaveLevel(string path, int level, LevelData _levelData)
        {
            var levelScriptable = Resources.Load("Levels/Level_" + level) as LevelContainer;
            if (levelScriptable != null)
            {
                levelScriptable.SetData(_levelData.DeepCopy(level));
                EditorUtility.SetDirty(levelScriptable);
            }

            AssetDatabase.SaveAssets();
        }
        #endif

        public static LevelData LoadLevel(int level)
        {
            var levelScriptable = Resources.Load("Levels/Level_" + level) as LevelContainer;
            LevelData levelData;
            if(levelScriptable)
            {
                levelData = levelScriptable.levelData.DeepCopy(level);
            }
            else
            {
                var levelScriptables = Resources.Load("Levels/LevelScriptable") as LevelScriptable;
                var ld = levelScriptables.levels.TryGetElement(level - 1, null);
                levelData = ld.DeepCopy(level);
            }

            return levelData;
        }
    }
}