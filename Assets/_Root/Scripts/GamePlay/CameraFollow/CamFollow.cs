using System;
using UnityEngine;
using UnityEngine.UI;

public class CamFollow : MonoBehaviour
{
    [SerializeField] GameObject objectToFollow /*,group*/;
    [SerializeField] bool beginFollow;
    [SerializeField] GameObject popup;
    public float speed = 2.0f;
    public float camSize = 2.5f;

    public GameObject ObjectToFollow {set => objectToFollow = value; get => objectToFollow;}
    public bool BeginFollow { set => beginFollow = value; get => beginFollow; }
    public GameObject Popup {set => popup = value; get => popup; }

    private Camera _myCam;

    Vector3 position;
    float interpolation;
    Vector3 popupScaleDefaut = Vector3.one;
    Vector3 camPositionDefaut = Vector3.zero;
    float camSizeDefaut = 8;

    private void Start()
    {
        _myCam = GetComponent<Camera>();
        camSizeDefaut = _myCam.orthographicSize;
        camPositionDefaut = _myCam.transform.position;

    }

    void Update()
    {
        if (beginFollow)
        {
            //if (Vector3.Distance(transform.position, objectToFollow.transform.position) == 10)
            //{
            //    beginFollow = false;
            //}
            //else
            //{
            interpolation = speed * Time.deltaTime;
            _myCam.orthographicSize = Mathf.Lerp(_myCam.orthographicSize, camSize, interpolation);
            //var tempColor = BG.color;
            //tempColor.a = Mathf.Lerp(tempColor.a, 1, interpolation);
            //BG.color = tempColor;

            var scale = popup.transform.localScale;
            scale.x = scale.y = Mathf.Lerp(scale.x, 1, interpolation * 3);
            popup.transform.localScale = scale;

            position = this.transform.position;
            if (!(objectToFollow is null))
            {
                try
                {
                    position.y = Mathf.Lerp(this.transform.position.y, objectToFollow.transform.position.y + 0.5f, interpolation);
                    position.x = Mathf.Lerp(this.transform.position.x, objectToFollow.transform.position.x, interpolation);
                }
                catch (Exception e)
                {
                    //
                    position = this.transform.position;
                }
            }

            this.transform.position = position;
            // Debug.LogError(Vector2.Distance(transform.position, objectToFollow.transform.position));
            // if (GameManager.instance.bouderCoinFly.activeSelf || GameManager.instance.gPanelWin.sprite == GameManager.instance.loseSp)
            //     return;
            // if (popup.transform.localScale.x <= 1.1f)
            // {
            //     GameManager.instance.bouderCoinFly.SetActive(true);
            // }
        }
    }

    public void StartRoom(GameObject objFollow, GameObject popup)
    {
        this.popup = popup;
        beginFollow = true;
        objectToFollow = objFollow;
        popupScaleDefaut = popup.transform.localScale;
    }
    public void Defaut() 
    {
        beginFollow = false;
        objectToFollow = null;

        if(popup != null) 
        {
            this._myCam.transform.position = camPositionDefaut;
            this._myCam.orthographicSize = camSizeDefaut;
            popup.transform.localScale = popupScaleDefaut;
            popup = null;
        }

    }
}