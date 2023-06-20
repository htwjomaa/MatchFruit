using FruitMatch.Scripts.Core;
using FruitMatch.Scripts.MapScripts.StaticMap.Editor;
using FruitMatch.Scripts.System;
using FruitMatch.Scripts.Integrations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FruitMatch.Scripts.GUI
{
	/// <summary>
	/// GUI events for Facebook, Settings and main scene
	/// </summary>
	public class GUIEvents : MonoBehaviour {
		public GameObject loading;
		public void Settings () {
			
			Rl.GameManager.PlayLimitSound(Rl.soundStrings.Click,
				Random.Range(0, 5),  Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);

			MenuReference.THIS.Settings.gameObject.SetActive (true);

		}

		public void Play () {
			Rl.GameManager.PlayLimitSound(Rl.soundStrings.Click,
				Random.Range(0, 5),  Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);
			LeanTween.delayedCall(1, ()=>SceneManager.LoadScene(Resources.Load<MapSwitcher>("Scriptable/MapSwitcher").GetSceneName()));
		}

		public void Pause () {
			Rl.GameManager.PlayLimitSound(Rl.soundStrings.Click,
				Random.Range(0, 5),  Rl.settings.GetUISoundVolume, Rl.uiSounds.audioSource);

			if (LevelManager.THIS.gameStatus == GameState.Playing)
				GameObject.Find ("CanvasGlobal").transform.Find ("MenuPause").gameObject.SetActive (true);

		}

		public void FaceBookLogin () {
#if FACEBOOK

			FacebookManager.THIS.CallFBLogin ();
#endif
		}

		public void FaceBookLogout () {
#if FACEBOOK
			FacebookManager.THIS.CallFBLogout ();

#endif
		}

		public void Share () {
#if FACEBOOK

			FacebookManager.THIS.Share ();
#endif
		}

	}
}
