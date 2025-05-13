using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace LCPS.SlipForge
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameObject LoadingScreen;
        [SerializeField] private Image LoadingBar;
        [SerializeField] private Image LoadingBarBG;

        // Singleton pattern
        private static SceneLoader _instance = null;
        public static SceneLoader Instance { get { return _instance; } }

        private void Start()
        {
            // TODO: idk if it's possible or nessesary to have a singleton MonoBehaviour
            // this is primarily so SceneManager can call LoadScene without constructing a new SceneLoader
            // or having a serialized reference to one (which would require it to also be a MonoBehaviour and thus not static)
            _instance = _instance != null ? _instance : this;

            // Ensure the loading screen is hidden
            LoadingScreen.SetActive(false);
            // load the initial scenes
            SceneManager.Initialize();
        }

        public void LoadScenes(params string[] sceneNames)
        {
            StartCoroutine(LoadWithScreenAsync(sceneNames));
        }

        private IEnumerator LoadWithScreenAsync(params string[] sceneNames)
        {
            LoadingScreen.SetActive(true);
            List<AsyncOperation> loadTasks = new();

            foreach (string sceneName in sceneNames)
            {
                loadTasks.Add(UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive));
            }

            LoadingBar.fillAmount = 0;

            // while any of the tasks are not done, update the loading bar
            while (loadTasks.Any(task => !task.isDone))
            {
                // sum the progress of all the tasks and divide by the number of tasks to get the total progress
                float totalProgress = loadTasks.Sum(task => task.progress) / loadTasks.Count;
                LoadingBar.fillAmount = Mathf.Clamp01(totalProgress / 0.9f);

                yield return null;
            }

            StartCoroutine(FadeLoadScreen());
        }

        private IEnumerator FadeLoadScreen()
        {
            const float fadeDuration = 0.5f;
            float elapsedTime = 0;

            // hide the loading bar background so it doesn't become visible during the fade
            LoadingBarBG.gameObject.SetActive(false);

            // get the canvas group component of the loading screen
            CanvasGroup lsCanvas = LoadingScreen.GetComponent<CanvasGroup>();

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                lsCanvas.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
                yield return null;
            }

            // hide the loading screen before resetting alpha values
            LoadingScreen.SetActive(false);
            // re-enable the loading bar background for the next time it's shown
            LoadingBarBG.gameObject.SetActive(true);

            // reset alpha value
            lsCanvas.alpha = 1;
        }
    }
}
