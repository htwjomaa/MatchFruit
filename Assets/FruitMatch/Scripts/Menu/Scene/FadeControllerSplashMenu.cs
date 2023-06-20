using UnityEngine;
public sealed class FadeControllerSplashMenu : MonoBehaviour
{
    // public Animator panelAnim;
    // public Animator gameInfoAnim;
    public Animator levelInfoPanelAnim;
    public Animator SplashSettingsMenu;
    public Animator AdminLevelSettingsAnimator;
    public Animator ExitButtonAnimator;
    public Animator PreviewBoardAnimator;
    public Animator FieldBoardAnimator;
    private void Awake() => gameObject.SetActive(true);
    public void FadeInFieldBoard(bool leftPanel) => FadeInPanel(leftPanel, FieldBoardAnimator);
    public void FadeOutFieldBoard() => FadeOutInfoPanel(FieldBoardAnimator);
    public void FadeInAdminSettingsPanel(bool leftPanel) => FadeInPanel(leftPanel, AdminLevelSettingsAnimator);
    public void FadeInLevelInfoPanel(bool leftPanel) => FadeInPanel(leftPanel, levelInfoPanelAnim);
    public void FadeInPreviewBoard(bool leftPanel) => FadeInPanel(leftPanel, PreviewBoardAnimator);
    public void FadeOutAdminSettingsPanel() => FadeOutInfoPanel(AdminLevelSettingsAnimator);
    public void FadeOutLevelInfoPanelPanel() => FadeOutInfoPanel(levelInfoPanelAnim);
    public void FadeOutPreviewBoard() => FadeOutInfoPanel(PreviewBoardAnimator);
    public void LevelInfoPanelSideMove(bool leftPanel) => PanelSideMove(leftPanel, levelInfoPanelAnim);
    public void AdminSettingsPanelSideMove(bool leftPanel) => PanelSideMove(leftPanel, AdminLevelSettingsAnimator);
    public void PreviewBoardSideMove(bool leftPanel) => PanelSideMove(leftPanel, PreviewBoardAnimator);
    
    private void FadeOutInfoPanel(Animator infoPanel)
    {
        infoPanel.SetBool("RightFadeIn", false);
        infoPanel.SetBool("LeftFadeIn", false);
        infoPanel.SetBool("FadeOut", true);
    }

    private void FadeInPanel(bool leftPanel, Animator infoPanel)
    {
        if (leftPanel)
        {
            infoPanel.SetBool("LeftFadeIn", true);
            infoPanel.SetBool("SideMove", false);
            infoPanel.SetBool("FadeOut", false);
        }
        else if (!leftPanel)
        {
            infoPanel.SetBool("RightFadeIn", true);
            infoPanel.SetBool("SideMove", false);
            infoPanel.SetBool("FadeOut", false);
        }
    }
    private void PanelSideMove(bool leftPanel, Animator panel)
    {
        if (leftPanel)
        {
            panel.SetBool("SideMove", true);
            panel.SetBool("LeftFadeIn", false);
            panel.SetBool("FadeOut", false);
        }
        else if (!leftPanel)
        {
            panel.SetBool("SideMove", true);
            panel.SetBool("RightFadeIn", false);
            panel.SetBool("FadeOut", false);
        }
    }
    
    public void FadeSplashSettingsMenu(bool fadeIn) => FadeOutSplashSettingsMenuAndExitButton(fadeIn, SplashSettingsMenu);
    public void FadeExitButton(bool fadeIn) => FadeOutSplashSettingsMenuAndExitButton(fadeIn, ExitButtonAnimator);

    private void FadeOutSplashSettingsMenuAndExitButton(bool fadeIn, Animator animator)
    {
        if (fadeIn)
        {
            animator.SetBool("FadeIn", true);
            animator.SetBool("FadeOut", false);
        }
        
        else
        {
            animator.SetBool("FadeIn", false);
            animator.SetBool("FadeOut", true);
        }
    }
}