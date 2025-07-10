using UnityEngine;

namespace UI
{
    public class LobbyController : MonoBehaviour
    {
        [Header("Scene Settings")]
        [SerializeField] private string nextSceneName = "GameScene";

        private ISceneLoader _sceneLoader;

        private void Awake()
        {
            _sceneLoader = new SceneLoader();
        }

        public void OnPlayButtonClicked()
        {
            _sceneLoader.Load(nextSceneName);
        }
    }
}