using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(LocalisedString))]
public class LocalisedStringDrawer : PropertyDrawer
{
   private bool dropDown;
   private float height;

   public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
   {
      if (dropDown)
      {
        return height += 35;
      }

      return 30;
      //base.GetPropertyHeight(property, label);
   }

   public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
   {
       EditorGUI.BeginProperty(position, label, property);
       position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
       position.width -= 34;
       position.height = 18;
       Rect valueRect = new Rect(position);
       valueRect.x += 15;
       valueRect.y -= 0;
       Rect foldButtonRect = new Rect(position);
       foldButtonRect.width = 15;

       dropDown = EditorGUI.Foldout(foldButtonRect, dropDown, "");
       position.x += 15;
       position.y -= 0;

       SerializedProperty key = property.FindPropertyRelative("key");
       key.stringValue = EditorGUI.TextField(position, key.stringValue);

       position.x += position.width + 2;
       position.width = 12;
       position.height = 12;

       Texture searchIcon = (Texture)Resources.Load("search");
       GUIContent searchContent = new GUIContent(searchIcon);
       GUIStyle guistyle = new GUIStyle();
       if (GUI.Button(position , searchContent , guistyle))
       {
           TextLocaliserSearchWindow.Open();
       }
       
       position.x += position.width +4;
       
       
       Texture storeIcon = (Texture)Resources.Load("store");
       GUIContent storeContent = new GUIContent(storeIcon);
       if (GUI.Button(position, storeContent, guistyle))
       {
           TextLocaliserEditWindow.Open(key.stringValue);
       }


       if (dropDown)
       {
           var value = LocalisationSystem.GetLocalisedValue(key.stringValue);
           GUIStyle style = GUI.skin.box;
           height = style.CalcHeight(new GUIContent(value), valueRect.width);

           valueRect.height = height;
           valueRect.y += 21;
           EditorGUI.LabelField(valueRect, value, EditorStyles.wordWrappedLabel);
       }

     EditorGUI.EndProperty();
   }
}
