using System;
using UnityEditor;
using UnityEngine;

namespace RescueFish.Controller 
{
    public class CustomSkinString : PropertyAttribute
    {
        private string[] items = new string[] { "default"};

        public int index = 0;
        public string[] Item 
        {
            set { items = value; }
            get { return items;}
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(CustomSkinString))]
    public class CustomStringDrawer : PropertyDrawer
    {
        CustomSkinString custom { get { return (CustomSkinString)attribute; } }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();

            if (SkinResources.Instance.dataAsset != null) 
            {
                var items = SkinResources.Instance.dataAsset.GetSkeletonData(false).Skins.Items;
                custom.Item = new string[items.Length];

                for (int i = 0; i < items.Length; i++)
                {
                    custom.Item[i] = items[i].Name;
                }

                if (string.IsNullOrEmpty(property.stringValue)) property.stringValue = custom.Item[0];
            }

            custom.index = Array.IndexOf(custom.Item, property.stringValue);
            custom.index = EditorGUI.Popup(position, label.text, custom.index, custom.Item);

            if (EditorGUI.EndChangeCheck())
            {
                property.stringValue = custom.Item[custom.index];
            }
        }
    }
#endif
}
