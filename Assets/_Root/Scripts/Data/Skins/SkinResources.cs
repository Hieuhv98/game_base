using System.Collections;
using System.Collections.Generic;
using Gamee_Hiukka.Control;
using Gamee_Hiukka.Data;
using UnityEngine;
using Spine.Unity;
using UnityEditor;
using Spine;

[CreateAssetMenu(fileName = "SkinResources", menuName = "ScriptableObject/SkinResources")]
public class SkinResources : ScriptableObject
{
    public SkeletonDataAsset dataAsset;
    public List<SkinData> skins;

    private List<SkinData> skinDaily = new List<SkinData>();

    private const string path = "SkinResources";
    private static SkinResources _instance;
    public static SkinResources Instance => _instance ?? (_instance = Resources.Load<SkinResources>(path));

    public List<SkinData> GetAllSkin()
    {
        return skins;
    }

    public SkinData GetSkinCurrent()
    {
        string id = GameData.IDSkinCurrent;

        foreach (var skin in skins)
        {
            if (skin.ID.Equals(id)) return skin;
        }

        return null;
    }

    public List<SkinData> GetSkinDaily() 
    {
        if(skinDaily != null) skinDaily.Clear();

        foreach (var skin in skins) 
        {
            if (skin.IsDailyReward) skinDaily.Add(skin);
        }

        return skinDaily;
    }
    public SkinData GetSkinGiftCode()
    {
        foreach (var skin in skins)
        {
            if (skin.IsGiftCode) return skin;
        }
        return null;
    }


    public SkinData GetSkinByID(string id)
    {
        foreach (var skin in skins)
        {
            if (skin.ID.Equals(id)) return skin;
        }
        return null;
    }
    public SkinData GetSkinByName(string name)
    {
        foreach (var skin in skins)
        {
            if (skin.SkinName.Equals(name)) return skin;
        }
        return null;
    }

    public SkinData GetSkinDefaut()
    {
        skins[0].IsHas = true;
        SaveDataAsset();
        return skins[0];
    }

    public void SetIsHasSkin(string id)
    {
        foreach (var skin in skins)
        {
            if (skin.ID.Equals(id))
            {
                skin.IsHas = true;
            }
        }
        SaveDataAsset();
    }

    public void UnlockAllSkin() 
    {
        foreach(var skin in skins) 
        {
            skin.IsHas = true;
        }

        SaveDataAsset();
    }

    public void ResetID() 
    {
        foreach (var skin in skins)
        {
            skin.IsHas = false;
        }
    }

    public void SaveDataAsset() 
    {
        foreach(var skin in skins) 
        {
            DataController.SaveDataByID(skin.ID, skin.IsHas);
        }
    }

    public void LoadDataAsset() 
    {
        foreach (var skin in skins)
        {
            skin.IsHas = DataController.LoadDataByID(skin.ID);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SkinResources))]
    [CanEditMultipleObjects]
    public class SkinResourcesEditor : Editor
    {
        private SerializedProperty skins;

        void OnEnable()
        {
            skins = serializedObject.FindProperty("skins");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            if (skins.arraySize > 0)
            {
                for (int i = 0; i < skins.arraySize; i++)
                {
                    var id = skins.GetArrayElementAtIndex(i).FindPropertyRelative("id");

                    if (i == 0)
                    {
                        if (string.IsNullOrEmpty(id.stringValue)) { id.stringValue = System.Guid.NewGuid().ToString(); }
                    }
                    else 
                    {
                        for(int j = 0; j< i; j++) 
                        {
                            var coppy = skins.GetArrayElementAtIndex(j).FindPropertyRelative("id");
                            if(id.stringValue.Equals(coppy.stringValue)) 
                            {
                                id.stringValue = System.Guid.NewGuid().ToString();
                                break;
                            }
                        }
                        if (string.IsNullOrEmpty(id.stringValue))
                        {
                            id.stringValue = System.Guid.NewGuid().ToString();
                        }
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

#endif
}
