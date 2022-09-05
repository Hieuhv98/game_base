using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxRotation : MonoBehaviour
{
    [SerializeField] GameObject model;
    private void Start()
    {
        var flareTransfrom = model.GetComponent<RectTransform>();
        flareTransfrom
            .DORotate(new Vector3(0, 0, 180), 1f, RotateMode.Fast)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
    }
}
