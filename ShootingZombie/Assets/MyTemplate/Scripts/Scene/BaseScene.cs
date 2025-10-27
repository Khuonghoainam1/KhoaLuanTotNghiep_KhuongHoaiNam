using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Yurowm.GameCore;

namespace Thanh.Core
{
    /// <summary>
    /// class cơ bản của tất cả các scene
    /// </summary>
    public class BaseScene : Singleton<BaseScene>
    {
        public static SceneName currentSceneName = SceneName.SplashScene;
        public static SceneName prevSceneName = SceneName.SplashScene;
        public Camera UICamera;
        public Camera GameCamera;
        public SceneName sceneName;
        public UnityEvent Reset;
        public Canvas sceneOverlayCanvas;
        public CanvasScaler UICanvas;

        public static Theme theme;

        protected virtual void Start()
        {
            Application.targetFrameRate = 60;

            PopupManager.Instance?.OnSetupCamera(UICamera);
            ItemFX.Instance?.OnSetupCamera(UICamera);
            TutorialManager.Instance?.OnSetupCamera(UICamera);

            currentSceneName = sceneName;
            Time.timeScale = 1f;

            if (UICamera.aspect >= 2.1f)
            {
                UICanvas.matchWidthOrHeight = 1;
            }
            else
            {
                UICanvas.matchWidthOrHeight = 0;
            }
        }

        public virtual void Init()
        {

        }

        private void OnDisable()
        {
            GameEvent.ClearDelegates();

            Reset?.Invoke();

            ILiveContentPoolable.KillEverything();

            if (PopupManager.Instance)
                PopupManager.Instance.DisableAllPopup();
        }

        public void CleanUp()
        {
            Reset?.Invoke();

            ILiveContentPoolable.KillEverything();

            Debug.Log("Clean up scene");
        }
    }
}
