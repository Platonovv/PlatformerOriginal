using PixelCrew.Model;
using PixelCrew.UI.LevelsLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components.LevelManegement
{
    public class ExitLevelComponent : MonoBehaviour

    {
        [SerializeField] private string _sceneName;
        
        public void Exit()
        {
            var session = GameSession.Instance;
            session.Save();
            var loader = FindObjectOfType<LevelLoader>();
            loader.LoadLevel(_sceneName);
        }
    }
}