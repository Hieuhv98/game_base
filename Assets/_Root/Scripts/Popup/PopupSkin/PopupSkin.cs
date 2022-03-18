using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Worldreaver.UniUI;
using TMPro;
using Gamee_Hiukka.Control;
using RescueFish.Controller;
using Spine.Unity;
using UniRx;
using System.Linq;
using Gamee_Hiukka.Other;
using Gamee_Hiukka.Common;

namespace Gamee_Hiukka.UI 
{
    public class PopupSkin : PopupBase
    {
        [SerializeField] private Transform contentBroad;
        [SerializeField] private SkinItem skinItemPrefab;
        [SerializeField] private GameObject groupSkinItemPrefab;
        [SerializeField] private SkeletonGraphic skePlayer;
        [SerializeField] private CoinDisplay coinDisplay;

        private Action _actionClose;
        private bool inittialized = false;
        private List<SkinItem> listSkinItems;
        public void Initialize(Action actionClose) 
        {
            _actionClose = actionClose;

            Util.UpdateSkinCurrent(skePlayer);
            
            if (!inittialized)
            {
                InstanceSkinItem();
                coinDisplay.ActionClose += UpdateDisplay;
                inittialized = true;
            }
        }

        private void InstanceSkinItem() 
        {
            contentBroad.Clear();

            var listItemData = SkinResources.Instance.GetAllSkin();
            listSkinItems = new List<SkinItem>();

            int bar = listItemData.Count / 3;
            int index = listItemData.Count - bar * 3;

            if (index != 0) bar = bar + 1;

            var listSkinInSkin = new List<SkinItem>();

            for(int i = 0; i < bar; i ++)
            {
                listSkinInSkin = Instantiate(groupSkinItemPrefab, contentBroad).GetComponentsInChildren<SkinItem>().ToList();
                for(int j = 0; j< listSkinInSkin.Count; j++) 
                {
                    var value = i * 3 + j;
                    if (value < listItemData.Count)
                    {
                        var item = listSkinInSkin[j];
                        listSkinItems.Add(item);
                        item.actionUpdated += UpdateSkin;
                        item.Init(listItemData[value]);
                        item.UpdateDisplay();
                    }
                    else listSkinInSkin[j].gameObject.SetActive(false);
                }

            }

/*            foreach (var itemData in listItemData) 
            {
                var item = Instantiate(skinItemPrefab, contentBroad);
                listSkinItems.Add(item);
                item.actionUpdated += UpdateSkin;
                item.Init(itemData);
                item.UpdateDisplay();
            }*/
        }

        void UpdateSkin(SkinItem item)
        {
            Util.UpdateSkin(skePlayer, item.SkinData.SkinName);

            foreach (var skinItem in listSkinItems) 
            {
                skinItem.UpdateDisplay();
            }
        }

        void UpdateDisplay()
        {
            foreach (var skinItem in listSkinItems) 
            {
                skinItem.UpdateDisplay();
            }
        }
        public void Hide() 
        {
            Close();
            _actionClose?.Invoke();
        }
    }
}

