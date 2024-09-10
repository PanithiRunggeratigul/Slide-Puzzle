using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlidePuzzle
{
    public class HomeManager : MonoBehaviour
    {
        [SerializeField] private string sceneName;

        public void PlayGame()
        {
            SceneManager.LoadScene(sceneName);
        }

        public void QuitGame()
        {
            Application.Quit();
            // EditorApplication.ExitPlaymode();
        }
    }
}
