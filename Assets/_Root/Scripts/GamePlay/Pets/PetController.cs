using Gamee_Hiukka.Common;
using Gamee_Hiukka.Data;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Gamee_Hiukka.Component;

namespace Gamee_Hiukka.Control 
{
    public class PetController : MonoBehaviour
    {
        [SerializeField] PetModel pet;
        private PlayerController player;
        private EPetState state = EPetState.PET_IDLE;
        string skinName = "defaut";
        Transform target = null;
        float distane = 0f;
        float distaneX = 0f;
        float distaneY = 0f;
        bool canMove = false;

        public bool IsMoveTeleport { get ; set; }
        public bool IsMoved { get; set; }

        public BoxCollider2D Collider => this.GetComponent<BoxCollider2D>();

        public void SetTarget(PlayerController player, Transform target)
        {
            this.player = player;
            this.transform.position = target.position;
            this.target = target;

            UpdateFlip();
        }
        public void Update()
        {
            if (player == null) return;
            if (target == null) return;

            distaneX = Mathf.Abs(this.transform.position.x - target.position.x);
            distaneY = Mathf.Abs(this.transform.position.y - target.position.y);
            distane = (this.transform.position - target.position).magnitude;

            if (state == EPetState.PET_RUN) return;
            MoveToTarget();
        }

        public void MoveToTarget()
        {
            if (distaneX > 0.25f || distaneY > 1f)
            {
                Run();
                var time = (distane / pet.Speed) < 0.4f ? .4f : distane / pet.Speed;
                this.transform.DOMove(target.position, time).SetEase(Ease.Linear).OnComplete(() => OnMoveCompleted());
            }
            //else this.transform.position = target.position;
        }

        void OnMoveCompleted() 
        {
            UpdateFlip();
            if (state != EPetState.PET_RUN) return;
            Idle();
            UpdateState();
        }

        void UpdateState() 
        {
            if (state == EPetState.PET_RUN)  return;
        }
        void MoveTeleport()
        {
            this.transform.position = player.transform.position;
            state = EPetState.PET_IDLE;
        }

        void UpdateFlip() 
        {
            var index = player.transform.localScale.x < 0 ? -1 : 1;
            this.transform.localScale = new Vector3(index , this.transform.localScale.y, this.transform.localScale.z);
        }

        public void UpdateModel(int id) 
        {
            pet.Skeletion.ClearState();
            pet.Skeletion.skeletonDataAsset = PetCollection.GetSkeletonAsset(id);
            pet.Skeletion.Initialize(true);
            //skinName = $"Level{PetDataController.Instance.GetLevel(id) + 1}";
            skinName = $"Level{GameData.PetLevel}";

            UpdateSkin();
        }

        void UpdateSkin() 
        {
            Util.UpdateSkin(pet.Skeletion, skinName);
        }

        public void Win() 
        {
            if (state == EPetState.PET_WIN) return;
            state = EPetState.PET_WIN;
            UpdateAnimation(pet.AnimWin, true);
        }

        public void Die() 
        {
            if (state == EPetState.PET_DIE) return;
            state = EPetState.PET_DIE;
            UpdateAnimation(pet.AnimDie, true); 

            this.gameObject.layer = LayerMask.NameToLayer(Constant.LAYER_DIE);
        }
        public void Run()
        {
            if (state == EPetState.PET_RUN) return;
            state = EPetState.PET_RUN;
            UpdateAnimation(pet.AnimRun, true); 
        }
        public void Idle() 
        {
            if (state == EPetState.PET_IDLE || state == EPetState.PET_DIE) return;
            state = EPetState.PET_IDLE;
            UpdateAnimation(pet.AnimIdle, true);
        }

        void UpdateAnimation(string animationName, bool loop = false) 
        {
            pet.Skeletion.AnimationState.SetAnimation(0, animationName, loop);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer.Equals(LayerMask.NameToLayer(Constant.LAYER_GROUND)))
            {
                canMove = true;
            }
            if (col.gameObject.layer.Equals(LayerMask.NameToLayer(Constant.LAYER_STONE)) ||
                col.gameObject.layer.Equals(LayerMask.NameToLayer(Constant.LAYER_ICE_BLOCK)))
            {
                canMove = true;
            }
        }
        private void OnTriggerStay2D(Collider2D target)
        {
            if (!canMove)
            {
                if (target.gameObject.layer.Equals(LayerMask.NameToLayer(Constant.LAYER_GROUND)) ||
                    target.gameObject.layer.Equals(LayerMask.NameToLayer(Constant.LAYER_STONE)) ||
                    target.gameObject.layer.Equals(LayerMask.NameToLayer(Constant.LAYER_ICE_BLOCK)))
                {
                    canMove = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D target)
        {
            if (target.gameObject.layer.Equals(LayerMask.NameToLayer(Constant.LAYER_GROUND)) ||
                target.gameObject.layer.Equals(LayerMask.NameToLayer(Constant.LAYER_STONE)) ||
                target.gameObject.layer.Equals(LayerMask.NameToLayer(Constant.LAYER_ICE_BLOCK)))
            {
                canMove = false;
            }
        }
    }
}

