using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code
{
    public class SceneReloader : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}