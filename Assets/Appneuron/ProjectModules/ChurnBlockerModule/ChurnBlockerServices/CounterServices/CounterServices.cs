using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Appneuron.ProjectModules.ChurnBlockerModule.ChurnBlockerServices.CounterServices
{
    public class CounterServices : MonoBehaviour
    {
        [HideInInspector]
        public float LevelBaseGameTimer { get; set; }

        [HideInInspector]
        public float TimerForGeneralSession { get; set; }

        [HideInInspector]
        public string SceneName { get; set; }

        [HideInInspector]
        public DateTime LevelBaseGameSessionStart { get; set; }

        [HideInInspector]
        public DateTime GameSessionEveryLoginStart { get; set; }



        void Start()
        {
            LevelBaseGameTimer = 0;
            GameSessionEveryLoginStart = DateTime.Now;
            LevelBaseGameSessionStart = DateTime.Now;
        }

        // Update is called once per frame
        void Update()
        {
            LevelBaseGameTimer += Time.deltaTime;
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneName = scene.name;
            LevelBaseGameTimer = 0;
            LevelBaseGameSessionStart = DateTime.Now;
        }
    }
}
