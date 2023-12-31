﻿using System.Collections;
using FruitMatch.Scripts.Core;
using FruitMatch.Scripts.GUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FruitMatch.Scripts.Level
{
    /// <summary>
    /// restart level helper
    /// </summary>
    public class RestartLevel : MonoBehaviour
    {

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(WaitForLoad(scene));
        }

        IEnumerator WaitForLoad(Scene scene)
        {
            yield return new WaitUntil(()=>LevelManager.THIS != null);
            if(scene.name == "game")
            {
                Debug.Log("restart");

                GUIUtils.THIS.StartGame();
                Destroy(gameObject);
            }
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        void OnDisable()
        {
            Debug.Log("OnDisable");
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
