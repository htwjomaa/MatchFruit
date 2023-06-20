using UnityEngine;
public class CurrentCopySection : MonoBehaviour
{
   [SerializeField] private CopySection copySection;
    public void FadeInFieldBoard()
    {
        Rl.fadeControllerSplashMenu.FadeInFieldBoard(SplashMenu.PanelOnLeft);
        FieldState.CurrentSection = copySection;
        Debug.Log("CurrentSection :" + copySection);
    }
    
    public void FadeOutFieldBoard() => Rl.fadeControllerSplashMenu.FadeOutFieldBoard();
}
