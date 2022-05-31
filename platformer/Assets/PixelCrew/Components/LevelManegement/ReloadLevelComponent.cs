using System.Diagnostics;
using PixelCrew.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Components.LevelManegement
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        { 
            var session = GameSession.Instance;
            session.LoadLastSave();
            
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            
            LoadScreenControls();
        }
        
        [Conditional("USE_ONSCREEN_CONTROLS")]
        private void LoadScreenControls()
        {
            SceneManager.LoadScene("Controls", LoadSceneMode.Additive);
        }
    }
}
