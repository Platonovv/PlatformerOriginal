using System;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.InGameMenuWindow
{
    public class InGameMenuWindow : AnimatedWindow
    {
        private float _defaultTimeScale;
        protected override void Start()
        {
            base.Start();

            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }

        public void OnExit()
        {
            SceneManager.LoadScene("MainMenu");
            
            var session = GameSession.Instance;
            Destroy(session.gameObject);
        }
        
        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale;
        }
    }
}