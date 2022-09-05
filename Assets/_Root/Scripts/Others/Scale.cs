using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Scale : MonoBehaviour
{
    public float scale = 1.15f;
    [SerializeField] bool isAuto = true;
    public void Start()
    {
        if (isAuto) PlayScale();
    }

    public void PlayScale()
    {
        transform.DOScale(transform.localScale * scale, 0.5f)
                 .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
