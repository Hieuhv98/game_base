using Lance.Common;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Gamee_Hiukka.Common;
using Spine.Unity;

public static partial class Util
{
    public static float DistanceStick(float sizeStickX) => sizeStickX - Constant.DEAULT_STICK_A + Constant.DELTAL_STICK_HEAD;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sizeScrollbar"></param>
    /// <returns></returns>
    public static float DistanceScrollMove(float sizeScrollbar) => sizeScrollbar - Constant.DELTA_MOVEMENT_STICK_HEAD;
    public static float DistanceScrollDrag(float sizeScrollbar) => sizeScrollbar - Constant.DELTA_DYNAMIC_STICK_HEAD2;
    public static List<GameObject> GetAllObectsByTag(string tagName)
    {
        return GameObject.FindGameObjectsWithTag(tagName).ToList();
    }

    public static void Shuffle<T>(this IList<T> source)
    {
        var n = source.Count;
        while (n > 1)
        {
            n--;
            var k = Rand.ThreadSafe.Next(n + 1);
            var value = source[k];
            source[k] = source[n];
            source[n] = value;
        }
    }

    public static GameObject CoppyGameObject(GameObject taget) 
    {
        var avatar = new GameObject("avatar");
        avatar.transform.parent = taget.transform.parent;
        avatar.transform.localPosition = taget.transform.localPosition;
        return avatar;
    }

    public static void ShowDelay(GameObject obj, float time = 1f) 
    {
        obj.SetActive(false);
        DOTween.Sequence().SetDelay(time).OnComplete(() =>
        {
            obj.SetActive(true);
        });
    }

    public static string[] CutName(string name, char x)
    {
        return name.Split(x);
    }
    public static  float ConvertVersion(string version)
    {
        float verConvert = 0.1f;

        if (float.TryParse(version, out verConvert)) return verConvert;

        return verConvert;
    }
    public static void UpdateSkin(SkeletonGraphic ske, string skin) 
    {
        ske.Skeleton.SetSkin(skin);
        ske.Skeleton.SetSlotsToSetupPose();
        ske.AnimationState.Apply(ske.Skeleton);
    }

    public static void UpdateSkin(SkeletonAnimation ske, string skin)
    {
        ske.Skeleton.SetSkin(skin);
        ske.Skeleton.SetSlotsToSetupPose();
        ske.AnimationState.Apply(ske.Skeleton);
    }

    public static void UpdateSkinCurrent(SkeletonGraphic ske) 
    {
        var skin = SkinResources.Instance.GetSkinCurrent().SkinName;
        ske.Skeleton.SetSkin(skin);
        ske.Skeleton.SetSlotsToSetupPose();
        ske.AnimationState.Apply(ske.Skeleton);
    }
}
