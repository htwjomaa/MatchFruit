using System.Collections;
using FruitMatch.Scripts.Core;
using UnityEngine;

public sealed class FadeController : MonoBehaviour
{
    public Animator panelAnim;
    public Animator gameInfoAnim;
    public Animator audioSettingsAnim;
    public Animator exitButtonLeft;
    private void Awake()
    {
        this.gameObject.SetActive(true);
    }

    public void OKButton()
    {
        Rl.GameManager.PlayAudio(Rl.soundStrings.OkButtonSound, Random.Range(0,4), Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
        if (panelAnim != null && gameInfoAnim != null)
        {
            panelAnim.SetBool("Out", true);
            gameInfoAnim.SetBool("Out", true);
            StartCoroutine(GameStartCo());
        }
    }

    public void GameOver()
    {
        panelAnim.SetBool("Out", false);
        panelAnim.SetBool("Game Over", true);
    }

    public void FadeOutAudioSettings()
    {
        audioSettingsAnim.SetBool("FadeIn", false);
        audioSettingsAnim.SetBool("FadeOut", true);
        exitButtonLeft.SetBool("FadeIn", false);
        exitButtonLeft.SetBool("FadeOut", true);
    }

    public void FadeInAudioSettings()
    {
        audioSettingsAnim.SetBool("FadeIn", true);
        audioSettingsAnim.SetBool("FadeOut", false);
        exitButtonLeft.SetBool("FadeIn", true);
        exitButtonLeft.SetBool("FadeOut", false);
    }

    IEnumerator GameStartCo()
    {
        yield return new WaitForSeconds(1.75f);
        Rl.board.currentState = GameState.move;
    }
}