using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace UI
{
    [DefaultExecutionOrder(-1)]
    public class SceneLoader : MonoBehaviour, ISceneLoader
    {
        public static SceneLoader Instance { get; private set; }

        [Header("씬 이름 설정")]
        [SerializeField] private string lobbySceneName   = "LobbyScene";
        [SerializeField] private string dungeonSceneName = "DungeonScene";
        [SerializeField] private string combatSceneName  = "CombatScene";

        private Scene _dungeonScene;
        private Scene _combatScene;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Load()
        {
            LoadLobbyToDungeonAsync().Forget();
        }

        private async UniTaskVoid LoadLobbyToDungeonAsync()
        {
            await SceneManager
                .LoadSceneAsync(dungeonSceneName, LoadSceneMode.Additive)
                .ToUniTask();
            _dungeonScene = SceneManager.GetSceneByName(dungeonSceneName);

            await SceneManager
                .LoadSceneAsync(combatSceneName, LoadSceneMode.Additive)
                .ToUniTask();
            _combatScene = SceneManager.GetSceneByName(combatSceneName);

            await SceneManager
                .UnloadSceneAsync(lobbySceneName)
                .ToUniTask();

            SetSceneActive(_combatScene, false);
            SetSceneActive(_dungeonScene, true);

            SceneManager.SetActiveScene(_dungeonScene);
        }

        public void EnterCombat()
        {
            SetSceneActive(_dungeonScene, false);
            SetSceneActive(_combatScene, true);
            SceneManager.SetActiveScene(_combatScene);
        }

        public void ExitCombat(bool isWin)
        {
            SetSceneActive(_combatScene, false);
            SetSceneActive(_dungeonScene, true);
            SceneManager.SetActiveScene(_dungeonScene);

            if (!isWin)
            {
                SceneManager.LoadScene(lobbySceneName);
            }
        }

        private void SetSceneActive(Scene scene, bool isActive)
        {
            foreach (var go in scene.GetRootGameObjects())
                go.SetActive(isActive);
        }
    }
}
