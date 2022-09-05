using DG.Tweening;
using Gamee_Hiukka.Data;
using System.Collections;
using System.Collections.Generic;
using Gamee_Hiukka.GameUI;
using TMPro;
using UnityEngine;
using System;

namespace Gamee_Hiukka.Other 
{
    public class CoinDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtCoin;
        private int _coinValue = 0;
        private bool _isUpdate = false;
        public Action ActionClose { set; get; }
        public Action ActionUpdate { set; get; }

        private void Start()
        {
            _coinValue = GameData.CoinCurrent;
            txtCoin.text = _coinValue.ToString();
        }
        private void Update()
        {
            if (_coinValue != GameData.CoinCurrent)
            {
                if (!_isUpdate)
                {
                    _isUpdate = true;
                    DOTween.To(() => _coinValue, x => _coinValue = x, GameData.CoinCurrent, 0.25f).OnUpdate(() =>
                    {
                        txtCoin.text = _coinValue.ToString();
                    });
                    DOTween.To(() => _coinValue, x => _coinValue = x, GameData.CoinCurrent, 0.25f).OnComplete(() =>
                    {
                        txtCoin.text = GameData.CoinCurrent.ToString();
                        _coinValue = GameData.CoinCurrent;
                        ActionUpdate?.Invoke();
                        _isUpdate = false;
                    });
                }
            }
        }

        public void ShowPopupShop()
        {
            GamePopup.Instance.ShowPopupShop(Hide);
        }

        void Hide()
        {
            ActionClose?.Invoke();
            GamePopup.Instance.Hide();
        }
    }
}

