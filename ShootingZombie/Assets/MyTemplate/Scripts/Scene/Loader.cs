using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;
using MyGame;

namespace Thanh.Core
{
    public class Loader : Singleton<Loader>
    {
        public CanvasGroup splashBG;
        public GameObject objProgress;
        [SerializeField]
        private Slider slider;
        public TextMeshProUGUI txtLoading;
        Coroutine txtLoadingRoutine;
        public CanvasScaler canvasScaler;

        private void OnEnable()
        {
            slider.value = 0f;
        }

        public void FakeLoading()
        {
            objProgress.SetActive(true);

            slider.DOValue(0.8f, 3f).SetEase(Ease.Linear);

            txtLoadingRoutine = StartCoroutine(IE_TextLoading());
        }

        public void LoadScene(string sceneName)
        {
            this.gameObject.SetActive(true);

            splashBG.alpha = 1f;
            objProgress.SetActive(GameScene.currentSceneName == SceneName.SplashScene);

            GameScene.prevSceneName = GameScene.currentSceneName;

            StartCoroutine(RoutineLoadScene(sceneName));

        }

        WaitForSeconds txtLoadingDelay = WaitForSecondsCache.Get(0.2f);
        IEnumerator IE_TextLoading()
        {
            for (int i = 0; i < 1000; i++)
            {
                switch (i % 4)
                {
                    case 0:
                        {
                            txtLoading.text = "Loading";
                            break;
                        }
                    case 1:
                        {
                            txtLoading.text = "Loading.";
                            break;
                        }
                    case 2:
                        {
                            txtLoading.text = "Loading..";
                            break;
                        }
                    case 3:
                        {
                            txtLoading.text = "Loading...";
                            break;
                        }
                }

                yield return txtLoadingDelay;
            }
        }

        IEnumerator RoutineLoadScene(string sceneName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = true;

            while (!asyncOperation.isDone)
            {
                yield return null;
            }

            if (GameScene.prevSceneName == SceneName.SplashScene)
            {
                slider.DOValue(1f, 1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    if (txtLoadingRoutine != null)
                    {
                        StopCoroutine(txtLoadingRoutine);
                        txtLoadingRoutine = null;
                    }

                    splashBG.DOFade(0f, 0.5f).OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                    }).SetUpdate(true);
                }).SetUpdate(true);
            }
            else
            {
                splashBG.DOFade(0f, 1f).OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    if (User.Instance[ItemID.IsAutoPlay] == 1 && GameScene.prevSceneName == SceneName.GameScene)
                    {
                        //Invoke("StartGameDelay", 0.05f);
                        GameManager.Instance.StartGameDelay();
                    }
                }).SetUpdate(true);
            }
        }
    }
}
