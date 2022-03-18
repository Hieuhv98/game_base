using Gamee_Hiukka.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Gamee_Hiukka.Component 
{
    public class CheckVisionComponent : MonoBehaviour
    {
        [SerializeField]float distane = 10f;
        [SerializeField]LayerMask layerMark;
        
        /// <summary>
        /// check target is visible in vision
        /// </summary>
        /// <param name="isRight"> true is right derection, false is left derection</param>
        /// <returns></returns>
        /// 

        public GameObject CheckVision(float dis, string tag = null) 
        {
            RaycastHit2D hit;
            var direction = this.transform.right;
            
            hit = Physics2D.Raycast(this.transform.position + this.transform.up * .5f, direction, dis, layerMark.value);
            Debug.DrawRay(transform.position + this.transform.up * .5f, direction * distane, Color.green);

            if (hit.collider != null)
            {
                if(hit.collider.CompareTag(tag)) return hit.collider.gameObject;
            }

            return null;

        }
        public void Stop() { }

        private void OnDrawGizmos()
        {
            
        }
    }
}

