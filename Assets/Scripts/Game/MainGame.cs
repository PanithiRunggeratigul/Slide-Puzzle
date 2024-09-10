using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SlidePuzzle
    {
        public class MainGame : MonoBehaviour
        {
            [SerializeField] private PuzzleMaker PuzzleMaker;

            [SerializeField] private string sceneName;

            [SerializeField] private GameObject winText;

            [SerializeField] private Button pauseButton;

            [SerializeField] private GameObject pausePanel;

            public bool isPause { get; private set; }

            void Start()
            {
                PuzzleMaker.CreatePuzzle();
                winText.SetActive(false);
                pausePanel.SetActive(false);
                isPause = false;
            }

            public void RestartGame()
            {
                winText.SetActive(false);

                PuzzleMaker.RestartPuzzle();

                isPause = !isPause;
                pausePanel.SetActive(isPause);
            }

            public void ResetGame()
            {
                winText.SetActive(false);

                PuzzleMaker.RemovePuzzle();

                PuzzleMaker.CreatePuzzle();

                isPause = !isPause;
                pausePanel.SetActive(isPause);
            }

            public void BackHome()
            {
                winText.SetActive(false);

                PuzzleMaker.RemovePuzzle();

                isPause = !isPause;
                pausePanel.SetActive(isPause);

                SceneManager.LoadScene(sceneName);

            }

            public void WinGame()
            {
                winText.SetActive(true);
            }

            public void PauseGame()
            {
                isPause = !isPause;
                pausePanel.SetActive(isPause);
            }
        }
    }