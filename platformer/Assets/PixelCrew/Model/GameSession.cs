
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PixelCrew.Components.LevelManegement;
using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Models;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour

    {
        [SerializeField] private int _levelIndex;
        [SerializeField] private PlayerData _data;
        [SerializeField] private string _defaultCheckPoint;
        
        
        public static GameSession Instance { get; private set ;  }
        
        
        
        public PlayerData Data => _data;
        private PlayerData _save;
        
        private readonly CompositeDisposables _trash = new CompositeDisposables();
        public QuickInventoryModel QuickInventory { get; private set; }
        public BigInventoryModel BigInventory { get; private set; }
        public PerksModel PerksModel { get; private set; }
        public StatsModel StatsModel { get; private set; }


        private readonly List<string> _checkpoints = new List<string>();
        
        private void Awake()
        {
            //level_start
            //level_index
            var existsSession = GetExistsSession();
            if (existsSession != null)
            {
                existsSession.StartSession(_defaultCheckPoint, _levelIndex);
                Destroy(gameObject);
            }
            else
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
                Instance = this;
                StartSession(_defaultCheckPoint, _levelIndex);
            }
        }


        
        private void StartSession(string defaultCheckPoint, int levelIndex)
        {
            SetChecked(defaultCheckPoint);
            TrackSessionStart(levelIndex);
            LoadHUD();
            SpawnHero();
        }

        private void TrackSessionStart(int levelIndex)
        {
            var eventParams = new Dictionary<string, object>
            {
                {"level_index", levelIndex}
            };
            AnalyticsEvent.Custom("level_start", eventParams);
        }

        private void SpawnHero()
        {
            var checkpoints = FindObjectsOfType<CheckPointComponent>();
            var lastCheckPoint = _checkpoints.Last();
            foreach (var checkPoint in checkpoints)
            {
                if (checkPoint.Id == lastCheckPoint)
                {
                    checkPoint.SpawnHero();
                    break;
                }
            }
        }


        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_data);
            _trash.Retain(QuickInventory);
            
            BigInventory = new BigInventoryModel(_data);
            _trash.Retain(BigInventory);

            PerksModel = new PerksModel(_data);
            _trash.Retain(PerksModel);
            
            StatsModel = new StatsModel(_data);
            _trash.Retain(StatsModel);

            _data.Hp.Value = (int) StatsModel.GetValue(StatId.Hp);
        }

        private void LoadHUD()
        {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
            LoadScreenControls();
        }

        [Conditional("USE_ONSCREEN_CONTROLS")]
        private void LoadScreenControls()
        {
            SceneManager.LoadScene("Controls", LoadSceneMode.Additive);
        }
        
        private GameSession GetExistsSession()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                    return gameSession;
            }

            return null;
            
        }

        public void Save()
        {
            _save = _data.Clone();
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();
            
            _trash.Dispose();
            InitModels();
        }

        public bool IsChecked(string id)
        {
            return _checkpoints.Contains(id);
        }
        
        public void SetChecked(string id)
        {
            if (!_checkpoints.Contains(id))
            {
                Save();
                _checkpoints.Add(id); 
            }
               
        }
        
        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
            _trash.Dispose();
        }

        private readonly List<string> _removedItems = new List<string>();

        public bool RestoreState(string id)
        {
            return _removedItems.Contains(id);
        }

        public void StoreState(string id)
        {
            if (!_removedItems.Contains(id))
                _removedItems.Add(id);
        }
    }
}