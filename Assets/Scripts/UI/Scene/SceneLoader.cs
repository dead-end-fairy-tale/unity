using UnityEngine.SceneManagement;

namespace UI
{
    public class SceneLoader : ISceneLoader
    {
        public void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}