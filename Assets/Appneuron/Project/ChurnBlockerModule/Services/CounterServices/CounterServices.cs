using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Appneuron.Project.ChurnBlockerModule.ChildModules.DataVisualizationModule.Services.CounterServices
{
    public class CounterServices : MonoBehaviour
    {
        [HideInInspector]
        public float levelBaseGameTimer;

        [HideInInspector]
        public float TimerForGeneralSession;

        [HideInInspector]
        public string SceneName;

        [HideInInspector]
        public DateTime levelBaseGameSessionStart;

        [HideInInspector]
        public DateTime gameSessionEveryLoginStart;



        void Start()
        {
            levelBaseGameTimer = 0;
            gameSessionEveryLoginStart = DateTime.Now;
            levelBaseGameSessionStart = DateTime.Now;
        }

        // Update is called once per frame
        void Update()
        {
            levelBaseGameTimer += Time.deltaTime;
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneName = scene.name;
            levelBaseGameTimer = 0;
            levelBaseGameSessionStart = DateTime.Now;
        }
    }
}
