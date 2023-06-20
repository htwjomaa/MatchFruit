using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioDatabase))]
public sealed class GUIAudioManager : Editor
{
    private string[] pitchLevels = {"zero", "one", "three", "four", "five"};
    private int pitchLevel = 3;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        AudioDatabase aDatabase = (AudioDatabase) target;
      
        GUILayout.Label("Testing Pitch", EditorStyles.boldLabel);
        GUILayout.Space(1);
        EditorGUILayout.BeginVertical();
        pitchLevel = GUILayout.Toolbar(pitchLevel,pitchLevels);
        EditorGUILayout.EndVertical();
        GUI.backgroundColor =  new Color(0.5f, 0.7f, 1f);
        if (GUILayout.Button("Play Audio")) aDatabase.TestAudio(pitchLevel);
        GUILayout.Space(3);
        
        GUI.backgroundColor =  new Color(0.85f, 0.7f, 1f);
        GUILayout.Label("Add and Change", EditorStyles.boldLabel);
        GUILayout.Space(1);
        if (GUILayout.Button("Add a Clip")) aDatabase.AddClip();
        GUI.backgroundColor =  new Color(0.88f, 0.88f, 0.88f);
        if (GUILayout.Button("Change the Audio Clip for entered Audio Name")) aDatabase.ChangeAudioClipForAudioName();
       
        GUILayout.Label("Find", EditorStyles.boldLabel);
        GUILayout.Space(1);
        if (GUILayout.Button("Find Audio Name with Line Input")) aDatabase.FindAudioNameWithInputLine();
        if (GUILayout.Button("Find Audio Clip with Clip Input")) aDatabase.FindAudioClipWithClipInput();
        GUILayout.Space(3);
 
        GUILayout.Label("Reset", EditorStyles.boldLabel);
        GUILayout.Space(1);
        if (GUILayout.Button("Reset Audio Settings with Clip Input")) aDatabase.ResetAudioSettingsClipInput();
        if (GUILayout.Button("Reset Audio Settings with Line Input")) aDatabase.ResetAudioSettingsLineInput();
        GUILayout.Space(3);
    
        GUILayout.Label("Delete", EditorStyles.boldLabel);
        GUILayout.Space(1);
        if (GUILayout.Button("Delete Audio Name Line Input")) aDatabase.DeleteAudioNameLineInput();
        if (GUILayout.Button("Delete Audio Settings Clip Input")) aDatabase.DeleteAudioSettingClipInput();
        
        
        
        GUILayout.Space(10);
        GUI.backgroundColor = new Color(0.35f, 0.35f, 0.35f);
        if (GUILayout.Button("Clear Audio Database")) aDatabase.ClearAll();
        
        GUILayout.Space(10);
        GUI.backgroundColor = new Color(0.35f, 0.75f, 0.35f);
        if (GUILayout.Button("Fix Wrong string")) aDatabase.FixWrongStringClipNames();
        GUI.backgroundColor = new Color(0.35f, 0.75f, 0.75f);
        if (GUILayout.Button("Debug Empty Audio Settings")) aDatabase.DebugEmptyAudioSettings();
        GUILayout.Space(1);
        if (GUILayout.Button("Delete String Line Input")) aDatabase.DelteStringLineInput();
    }
}