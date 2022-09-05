using Gamee_Hiukka.Control;
using Gamee_Hiukka.Data;
using Gamee_Hiukka.Pattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxManager : Singleton<FxManager>
{
    [SerializeField] GameObject fxTouch;

    public GameObject CreateFxTouch(Vector3 pos, bool release = true)
    {
        GameObject obj = PoolManager.SpawnObject(fxTouch, pos, Quaternion.identity);
        if (release) StartCoroutine(IEReleaseFx(obj));
        return obj;
    }

    public GameObject CreateFx(GameObject fx, Vector3 pos, bool release = true)
    {
        if (fx == null) return null;
        GameObject obj = PoolManager.SpawnObject(fx, pos, fx.transform.rotation);
        if (release) StartCoroutine(IEReleaseFx(obj));
        return obj;
    }

    IEnumerator IEReleaseFx(GameObject fx, float time = 2)
    {
        yield return new WaitForSeconds(time);
        PoolManager.ReleaseObject(fx);
    }
    public void ReleaseObj(GameObject obj) 
    {
        PoolManager.ReleaseObject(obj);
    }
}
