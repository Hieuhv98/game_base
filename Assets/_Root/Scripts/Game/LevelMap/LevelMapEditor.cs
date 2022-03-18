#if UNITY_EDITOR
using Gamee_Hiukka.Control;
using Gamee_Hiukka.Data;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Gamee_Hiukka.Editor
{
    [CustomEditor(typeof(LevelMap))]
    public class LevelMapEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!serializedObject.isEditingMultipleObjects)
            {
                var level = (LevelMap) target;
                
                if (AssetDatabase.Contains(target))
                {
                    if (GUILayout.Button("Play"))
                    {
                        EditorSceneManager.OpenScene("Assets/_Root/Scenes/Loading.unity");
                        Config.IsTest = true;
                        Config.LevelTest = level;
                        EditorApplication.isPlaying = true;
                        //todo
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("Select a prefab level to play!");
                }
            }
        }

        static LevelMapEditor() { EditorApplication.playModeStateChanged += ModeChanged; }

        private static void ModeChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.ExitingPlayMode)
            {
                Config.IsTest = false;
                Config.LevelTest = null;
            }
        }
    }
}
#endif

