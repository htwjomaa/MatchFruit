using UnityEngine;

public sealed class PlayMenuBGMusic : MonoBehaviour
{
    void Start() => StartMusic();
    public void StartMusic()
    {
        if (Rl.GameManager.gameManagerAudioSource.clip != null) return;
        Rl.GameManager.PlayAudio(Random.Range(0, 2) == 0 ? Rl.soundStrings.MenuBackgroundMusic : Rl.soundStrings.MenuBackgroundMusic2, Random.Range(0, 4), false, Rl.settings.GetMusicVolume);
    }
}
