using UnityEngine;
public class FruitUITab : MonoBehaviour
{   
    [SerializeField] private TabGroup tabGroup;
    private void Start()
    {
        tabGroup = GetComponent<TabGroup>();
        SetFruitSprites();
    }
    public void SetFruitSprites()
    {
        int counter = 0;
        for (var index = 0; index < tabGroup.tabButtons.Count; index++)
        {
     
            if (tabGroup.tabButtons[index].GetComponent<FruitUI>() !=null)
            {
                string s = tabGroup.tabButtons[index].GetComponent<FruitUI>().fruitType.ToString().TrimEnd();
                
                tabGroup.TabInfoTextList[counter] =  s;
                counter++;
            }
        }
    }
}