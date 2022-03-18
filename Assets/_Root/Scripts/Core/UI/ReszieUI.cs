using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReszieUI : MonoBehaviour
{
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private Camera cam;

    private float aspect = 0;
    public void Awake()
    {
        if (cam.aspect > 0.6f)
        {
            canvasScaler.matchWidthOrHeight = 1f;
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 0f;
        }
    }

    public void Update()
    {
        if(aspect != cam.aspect) 
        {
            aspect = cam.aspect;

            if (cam.aspect > 0.6f)
            {
                canvasScaler.matchWidthOrHeight = 1f;
            }
            else
            {
                canvasScaler.matchWidthOrHeight = 0f;
            }
        }
    }
}
