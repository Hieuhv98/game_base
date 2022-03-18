using Gamee_Hiukka.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gamee_Hiukka.Control 
{
    [RequireComponent(typeof(Image))]
    public class BGController : MonoBehaviour
    {
        [SerializeField] List<Sprite> imgBgs;

        Image backGround;

        public void Awake()
        {
            backGround = GetComponent<Image>();
        }
        public void UpdateBG()
        {
            if (imgBgs.Count == 0) return;
            var i = Random.Range(0, imgBgs.Count - 1);

            backGround.sprite = imgBgs[i];
        }
    }
}

