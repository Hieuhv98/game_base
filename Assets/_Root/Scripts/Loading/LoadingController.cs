using DG.Tweening;
using Gamee_Hiukka.Common;
using Gamee_Hiukka.Data;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gamee_Hiukka.Control 
{
    public class LoadingController : MonoBehaviour
    {
        public Launcher launcher;
        [SerializeField] private TextMeshProUGUI txtPercent;
        [SerializeField] private Image imgLoadingIndex;

        private float _timeDelayLoadScene = 0.5f;
        private float _percentValue = 0;
        private float _timeLoadingMin = 6f;
        private bool _isLoadComplete = false;
        private AsyncOperation _loadScene; 
        public void Awake() 
        {
            _isLoadComplete = false;
            txtPercent.text = string.Format(Constant.LOADING, _percentValue);

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
            ShowMenuGame();
        }

        private void LoadGameMenu()
        {
            _loadScene = SceneManager.LoadSceneAsync(1);
            _loadScene.allowSceneActivation = false;
        }

        private void ShowMenuGame() 
        {
            //_loadScene.allowSceneActivation = true;
            SceneManager.LoadScene(1);
        }
    }
}

