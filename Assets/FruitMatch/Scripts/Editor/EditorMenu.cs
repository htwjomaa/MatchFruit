using FruitMatch.Scripts.MapScripts.StaticMap.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace FruitMatch.Scripts.Editor
{
    [InitializeOnLoad]
    public static class EditorMenu
    {
        private const string MenuMapStatic = "FruitMatch/Scenes/Map switcher/Static map";
        private const string MenuMapDinamic = "FruitMatch/Scenes/Map switcher/Dinamic map";

    [MenuItem("FruitMatch/Scenes/Main scene")]
    public static void MainScene()
    {
        EditorSceneManager.OpenScene("Assets/FruitMatch/Scenes/main.unity");
    }
    
    [MenuItem(MenuMapStatic)]
    public static void MapSceneStatic()
    {
        SetStaticMap( true);
        GameScene();
    }
    
    [MenuItem(MenuMapDinamic)]
    public static void MapSceneDinamic()
    {
        SetStaticMap( false);
        GameScene();
    }
    
    public static void SetStaticMap(bool enabled) {
 
        Menu.SetChecked(MenuMapStatic, enabled);
        Menu.SetChecked(MenuMapDinamic, !enabled);
        var sc = Resources.Load<MapSwitcher>("Scriptable/MapSwitcher");
        sc.staticMap = enabled;
        EditorUtility.SetDirty(sc);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("FruitMatch/Scenes/Game scene")]
    public static void GameScene()
    {
        EditorSceneManager.OpenScene("Assets/FruitMatch/Scenes/"+Resources.Load<MapSwitcher>("Scriptable/MapSwitcher").GetSceneName()+".unity");
    }

    [MenuItem("FruitMatch/Settings/Debug settings")]
    public static void DebugSettings()
    {
        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/SweetSugar/Resources/Scriptable/DebugSettings.asset");
    }
    [MenuItem("FruitMatch/Settings/Pool settings")]
    public static void PoolSettings()
    {
        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/SweetSugar/Resources/Scriptable/PoolSettings.asset");
    }
    
 
    //     [MenuItem("FruitMatch/procc")]
//     public static void Start()
//     {
//         var targets = AssetDatabase.LoadAssetAtPath("Assets/FruitMatch/Resources/Levels/TargetEditorScriptable.asset", typeof(TargetEditorScriptable)) as TargetEditorScriptable;
//         var target = targets.targets.Where(i => i.name == "Ingredients").First();
//         var levelData = AssetDatabase.LoadAssetAtPath("Assets/FruitMatch/Resources/Levels/LevelScriptable.asset", typeof(LevelScriptable)) as LevelScriptable;
//         foreach (var level in levelData.levels)
//         {
//             if (level.target.name == "Ingredients")
//             {
//                 level.target = target.DeepCopy();
//                 Debug.Log(level.levelNum);
//             }
//         }
//     }   
    }
}