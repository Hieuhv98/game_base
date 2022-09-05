using DG.Tweening;
using Gamee_Hiukka.Common;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.UI;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;
using UnityEngine.UI;

namespace Gamee_Hiukka.Control
{
    public class LoadingController : MonoBehaviour
    {
        [Header("Loading")]
        public Launcher launcher;
        [SerializeField] private TextMeshProUGUI txtPercent;
        [SerializeField] private Image imgLoadingIndex;
        [SerializeField] private float _timeDelayLoadScene = 0.5f;
        [SerializeField] private float _timeLoadingMin = 6f;

        [Header("Intro")]
        [SerializeField] private bool isShowIntro;
        [SerializeField] private GameObject loadingUI;
        [SerializeField] private GameObject introUI;
        [SerializeField] private Animator action;
        [SerializeField] private ButtonBase btnSkipIntro;
        [SerializeField] private float timeShowSkipButton = 1f;

        private float _percentValue = 0;
        private bool _isLoadComplete = false;
        private AsyncOperation _loadScene;
        bool isDoneIntro = false;
        public void Awake()
        {
            _isLoadComplete = false;
            txtPercent.text = string.Format(Constant.LOADING, _percentValue);
            Application.targetFrameRate = 60;

            isDoneIntro = false;
            introUI.SetActive(false);
            btnSkipIntro.onClick.RemoveListener(SkipIntro);
            btnSkipIntro.onClick.AddListener(SkipIntro);
            btnSkipIntro.gameObject.SetActive(false);

            Loading();
        }

        private void Loading()
        {
            DOTween.To(x => _percentValue = (int)x, 0, 99, _timeLoadingMin).SetEase(Ease.OutQuad).OnUpdate(() =>
            {
                txtPercent.text = string.Format(Constant.LOADING, _percentValue);
                imgLoadingIndex.fillAmount = _percentValue / 100;
            });

            DOTween.To(x => _percentValue = (int)x, 0, 99, _timeLoadingMin).OnComplete(() =>
            {
                _isLoadComplete = true;
            });

            StartCoroutine(LoadGameData());
            //LoadGameMenu();
        }

        private IEnumerator LoadGameData()
        {
            launcher.LoadData();
            yield return new WaitUntil(() => launcher.IsLoadDataComplete == true);
            launcher.Setting();
            //yield return new WaitUntil(() => _loadScene != null);
            StartCoroutine(WaitForLoadMenuScene());
        }

        private IEnumerator WaitForLoadMenuScene()
        {
            yield return new WaitUntil(() => _isLoadComplete == true);
            txtPercent.text = string.Format(Constant.LOADING, 100);
            imgLoadingIndex.fillAmount = 1;
            yield return new WaitForSeconds(_timeDelayLoadScene);
            //ShowMenuGame();
            if (isShowIntro && !GameData.IsShowIntro)
            {
                GameData.IsShowIntro = true;
                ShowIntro();
            }
            else ShowMenuGameDefaut();
        }

        private void LoadGameMenu()
        {
            if (Config.AutoStartGame || GameData.IsNewGame) _loadScene = SceneManager.LoadSceneAsync(2);
            else _loadScene = SceneManager.LoadSceneAsync(1);
            _loadScene.allowSceneActivation = false;
        }

        private void ShowIntro()
        {
            LoadGameMenu();
            loadingUI.SetActive(false);
            introUI.SetActive(true);
            string actionName = "Action_New";
            action.Play(actionName);
            StartCoroutine(WaitTime(action.GetAnimationLenght(actionName, 0.1f), () =>
            {
                if (isDoneIntro) return;
                DoneIntro();
            }));

            StartCoroutine(WaitTime(timeShowSkipButton, () => { btnSkipIntro.gameObject.SetActive(true); }));
        }

        IEnumerator WaitTime(float time, Action actionConpleted)
        {
            yield return new WaitForSeconds(time);
            actionConpleted?.Invoke();
        }

        public void SkipIntro()
        {
            if (isDoneIntro) return;
            MyAnalytic.LogEvent(MyAnalytic.SKIP_INTRO);
            DoneIntro();
        }
        public void DoneIntro()
        {
            isDoneIntro = true;
            action.Play("Done");
            introUI.gameObject.SetActive(false);
            ShowMenuGame();
        }

        private void ShowMenuGame()
        {
#if (!UNITY_EDITOR)
            GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
#endif

            _loadScene.allowSceneActivation = true;
            //SceneManager.LoadScene(1);
        }
        private void ShowMenuGameDefaut()
        {
#if (!UNITY_EDITOR)
            GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
#endif
            if (Config.AutoStartGame || GameData.IsNewGame)
            {
                SceneManager.LoadScene(2);
            }
            else SceneManager.LoadScene(1);
        }
    }
}

