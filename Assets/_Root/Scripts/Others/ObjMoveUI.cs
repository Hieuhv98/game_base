using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gamee_Hiukka.Common;
using Gamee_Hiukka.Data;
using UnityEngine;

namespace Game_Base.Control
{
    public class ObjMoveUI : MonoBehaviour
    {
        [SerializeField] private EMoveType moveType;
        [SerializeField] private float distane = 500;
        [SerializeField] private float time = .5f;
        [SerializeField] private Ease ease = Ease.InBack;

        private Vector3 positionDefaut = Vector3.zero;
        private RectTransform thisRectTransform;
        private void Awake()
        {
            //positionDefaut = this.transform.localPosition;
            thisRectTransform = this.GetComponent<RectTransform>();
            positionDefaut = thisRectTransform.anchoredPosition;
        }

        public void Move(Action actionCompleted = null)
        {
            thisRectTransform.DOKill();
            GameData.IsUIMoving = true;
            switch (moveType)
            {
                case EMoveType.MOVE_UP:
                    //this.transform.DOLocalMoveY(positionDefaut.y + distane, time).SetEase(ease);
                    thisRectTransform.DOAnchorPosY(positionDefaut.y + distane, time).SetEase(ease).OnComplete(() => 
                    {
                        GameData.IsUIMoving = false;
                        actionCompleted?.Invoke();
                    });
                    break;
                case EMoveType.MOVE_DOWN:
                    //this.transform.DOLocalMoveY(positionDefaut.y - distane, time).SetEase(ease);
                    thisRectTransform.DOAnchorPosY(positionDefaut.y - distane, time).SetEase(ease).OnComplete(() =>
                    {
                        GameData.IsUIMoving = false;
                        actionCompleted?.Invoke();
                    });
                    break;
                case EMoveType.MOVE_RIGHT:
                    //this.transform.DOLocalMoveX(positionDefaut.x + distane, time).SetEase(ease);
                    thisRectTransform.DOAnchorPosX(positionDefaut.x + distane, time).SetEase(ease).OnComplete(() =>
                    {
                        GameData.IsUIMoving = false;
                        actionCompleted?.Invoke();
                    });
                    break;
                case EMoveType.MOVE_LEFT:
                    //this.transform.DOLocalMoveX(positionDefaut.x - distane, time).SetEase(ease);
                    thisRectTransform.DOAnchorPosX(positionDefaut.x - distane, time).SetEase(ease).OnComplete(() =>
                    {
                        GameData.IsUIMoving = false;
                        actionCompleted?.Invoke();
                    });
                    break;
            }
        }

        public void Defaut()
        {
            if (thisRectTransform == null) return;
            GameData.IsUIMoving = false;
            thisRectTransform.anchoredPosition = positionDefaut;
        }
        public void MoveBack()
        {
            thisRectTransform.DOKill();
            thisRectTransform.DOAnchorPos(positionDefaut, .25f).SetEase(ease);
        }
    }
}

