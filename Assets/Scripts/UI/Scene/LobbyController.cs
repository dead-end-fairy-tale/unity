using UnityEngine;

namespace UI
{
    public class LobbyController : MonoBehaviour
    {
        private ISceneLoader _sceneLoader;

        private void Awake()
        {
            _sceneLoader = SceneLoader.Instance;
        }

        public void OnPlayButtonClicked()
        {
            _sceneLoader.Load();
        }
    }
}