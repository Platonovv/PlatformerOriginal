﻿using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components.LevelMenagement
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        { 
            var session = GameSession.Instance;
            session.LoadLastSave();
            
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        
    }
}
