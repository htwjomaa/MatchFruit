using System.Collections;

namespace FruitMatch.Scripts.LinqConstructor.AnyFieldInspector.Editor
{
    public class AnyFieldEditor : UnityEditor.Editor
    {
        public void ShowList(IList list)
        {
            GuiList.Show(list);
        }
    }
}