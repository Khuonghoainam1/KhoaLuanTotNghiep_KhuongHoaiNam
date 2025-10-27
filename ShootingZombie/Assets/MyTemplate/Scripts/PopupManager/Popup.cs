

using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Thanh.Core
{
    /// <summary>
    /// The base class of all the popups in the game.
    /// </summary>
    public class Popup : MonoBehaviour
    {
        public PopupID popupID;
        public bool isPause;
        public bool isShowBanner = true;
        public string openSound = "Popup";

        public UnityEvent onOpen;
        public UnityEvent onClose;
        public UnityEvent onCloseDone;

        public Animator animator { get; private set; }

        public float rootBaseSize = 1f;
        public bool isPopupGameWin;
        public Button btnClose;

        public bool skipClose { get; set; }

        public bool isClosing { get; set; }

        /// <summary>
        /// Unity's Awake method.
        /// </summary>
        public virtual void Start()
        {
            animator = GetComponent<Animator>();
        }

        protected virtual void OnDisable()
        {
            StopAllCoroutines();
            onOpen.RemoveAllListeners();
            onClose.RemoveAllListeners();
            onCloseDone.RemoveAllListeners();
        }

        public virtual void OnShow()
        {
            if (BaseScene.currentSceneName == SceneName.GameScene)
            {
                Time.timeScale = isPause ? 0f : 1f;
            }

            gameObject.SetActive(true);
            isClosing = false;
            onOpen.Invoke();
            if (animator != null)
            {
                animator.Play("Open");
            }

            if (btnClose != null)
            {
                btnClose.onClick.RemoveListener(Close);
                btnClose.onClick.AddListener(Close);
            }

            //AudioAssistant.PlaySound(openSound);
            //AudioManager.instance.Play("BtnClick");
            ManagerBase.isBlockPointer = true;
        }

        public virtual void OnShowDone()
        {

        }

        /// <summary>
        /// Closes the popup.
        /// </summary>
        public virtual void Close()
        {
            if (skipClose)
            {
                return;
            }

            if (isClosing)
            {
                return;
            }
            isClosing = true;

            try
            {
                if (animator != null)
                {
                    animator.Play("Close");
                    StartCoroutine(DestroyPopup());
                }
                else
                {
                    _DestroyPopup();
                }
            }
            catch { }
            ManagerBase.isBlockPointer = false;
           // AudioAssistant.PlaySound("BtnClick");
           AudioManager.instance.Play("BtnClick");
        }

        public void Hide()
        {
            if (animator != null)
            {
                animator.Play("Close");
            }
        }

        public virtual IEnumerator IEClose()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            Close();
        }

        /// <summary>
        /// Utility coroutine to automatically destroy the popup after its closing animation has finished.
        /// </summary>
        /// <returns>The coroutine.</returns>
        protected virtual IEnumerator DestroyPopup()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            _DestroyPopup();
        }

        void _DestroyPopup()
        {
            onClose.Invoke();
            gameObject.SetActive(false);
            onCloseDone?.Invoke();
            PopupManager.Instance.OnCloseDone(popupID);
            if (BaseScene.currentSceneName == SceneName.GameScene)
            {
                if (PopupManager.Instance.hasPopup() == false)
                {
                    Time.timeScale = 1f;
                }
            }
        }
    }
}
