using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UtilityFocus;

namespace InterfaceUI
{
    public class AdapterBetweenScenes : MonoBehaviour
    {
        [SerializeField] private AnimationsScene _animationsScene;
        [SerializeField] private Image _imageClickBlocking;
        [SerializeField] private Image _imageSceneTransition;

        private int _time = 1;

        private void Start()
        {
            LoadEntryScene();
        }

        public void LoadExitScene(int indexScene)
        {
            FocusObserver.IsTransitioning = true;
            StartCoroutine(StartExitScene(indexScene));
        }

        public void RestartLevel()
        {
            FocusObserver.IsTransitioning = true;
            StartCoroutine(StartRestartLevel());
        }

        private void LoadEntryScene()
        {
            StartCoroutine(StartEntryScene());
        }

        private IEnumerator StartRestartLevel()
        {
            _imageSceneTransition.gameObject.SetActive(true);
            _imageClickBlocking.gameObject.SetActive(true);
            _animationsScene.TriggerExitScene();

            yield return WaitForSecondsOrPause(_time);

            FocusObserver.IsTransitioning = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private IEnumerator StartEntryScene()
        {
            _imageSceneTransition.gameObject.SetActive(true);
            _animationsScene.TriggerEntryScene();

            yield return WaitForSecondsOrPause(_time);

            _imageClickBlocking.gameObject.SetActive(false);
            _imageSceneTransition.gameObject.SetActive(false);
        }

        private IEnumerator StartExitScene(int indexScene)
        {
            _imageSceneTransition.gameObject.SetActive(true);
            _imageClickBlocking.gameObject.SetActive(true);
            _animationsScene.TriggerExitScene();

            yield return WaitForSecondsOrPause(_time);

            FocusObserver.IsTransitioning = false;
            SceneManager.LoadScene(indexScene);
        }

        private IEnumerator WaitForSecondsOrPause(float delay)
        {
            float elapsed = 0;

            while (elapsed < delay)
            {
                if (FocusObserver.IsPause == false && FocusObserver.HasFocus)
                {
                    elapsed += Time.unscaledDeltaTime;
                }

                yield return null;
            }
        }
    }
}