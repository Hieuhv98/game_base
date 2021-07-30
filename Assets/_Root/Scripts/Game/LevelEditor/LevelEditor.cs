#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
using System.IO;
using System;
using System.Linq;
using Game_Base.Data;
using Game_Base.Control;

namespace Game_Base.Editor 
{
    public class LevelEditor : EditorWindow
    {
        string FolderPath => Path.Combine(Application.dataPath, "_Root/Prefabs/Game");
        static GameObject _selectedPrefab;
        static Vector2 _scrollPosition;

        private Camera cam;

        [UnityEditor.MenuItem("Window/LevelEditor %&o")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LevelEditor));
        }

        void OnEnable()
        {
            SceneView.duringSceneGui += DuringSceneGui;
        }

        void OnDisable()
        {
            SceneView.duringSceneGui -= DuringSceneGui;
        }

        void DuringSceneGui(SceneView sceneView)
        {
            sceneView.Repaint();
            CustomOnSceneGUI(sceneView);
            EditorUtility.SetDirty(sceneView);
        }

        void CustomOnSceneGUI(SceneView sceneView)
        {
            if (Application.isPlaying)
            {
                return;
            }
            NodesControl();

            var stageHandle = StageUtility.GetCurrentStageHandle();
            var level = stageHandle.FindComponentOfType<LevelMap>();
            if (!level) return;

            PlaceObject(level);

        }

        void OnGUI()
        {
            if (Application.isPlaying) return;

            EditorGUILayout.Space();
            var prefabUtility = PrefabStageUtility.GetCurrentPrefabStage();
            var level = prefabUtility?.prefabContentsRoot.GetComponent<LevelMap>();
            if (level)
            {
                var levelAsset = AssetDatabase.LoadAssetAtPath<LevelMap>(prefabUtility.prefabAssetPath);

                EditorGUILayout.ObjectField("Level_", levelAsset, typeof(LevelMap), true);

                EditorGUILayout.BeginVertical("HelpBox");
                GUILayout.Label("Select prefab => Shift + Right click: Place object");
                GUILayout.Label("Select Nodes => Control + Right click => Choose Options");
                GUILayout.Space(20);
                if (GUILayout.Button("DESELECT OBJECT", GUILayout.MinHeight(40)))
                {
                    _selectedPrefab = null;
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space();

                DrawPrefabSections();

                EditorGUILayout.Space();

                GUI.color = Color.green;
                if (GUILayout.Button("PLAY", GUILayout.Height(60)))
                {
                    //LevelResources.Instance.debugMap = levelAsset;

                    Config.IsTest = true;
                    Config.LevelTest = levelAsset;
                    EditorSceneManager.OpenScene("Assets/_Root/Scenes/Loading.unity");
                    EditorApplication.isPlaying = true;
                }
                GUI.color = Color.white;
            }
            else
            {
                EditorGUILayout.BeginHorizontal("HelpBox");
                GUILayout.Label("Open a level prefab first!");
                EditorGUILayout.EndHorizontal();
            }
        }
        static LevelEditor() { EditorApplication.playModeStateChanged += ModeChanged; }

        private static void ModeChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.ExitingPlayMode)
            {
                Config.IsTest = false;
                Config.LevelTest = null;
            }
        }
        void DrawPrefabSections()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            if (!string.IsNullOrEmpty(FolderPath))
            {
                var directoryInfo = new DirectoryInfo(FolderPath);
                if (directoryInfo.Exists)
                {
                    var directories = directoryInfo.GetDirectories();
                    foreach (var directory in directories)
                    {
                        DrawPrefabSelector(directory);
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }

        void DrawPrefabSelector(DirectoryInfo directory)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Label(directory.Name, EditorStyles.boldLabel);
            var fileInfos = directory.GetFiles();
            var prefabs = new List<GameObject>();
            foreach (var fileInfo in fileInfos)
            {
                var s = fileInfo.FullName.IndexOf("Assets", StringComparison.Ordinal);
                var path = fileInfo.FullName.Substring(s);
                var prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                if (prefab != null)
                {
                    prefabs.Add(prefab);
                }
            }

            const float buttonSize = 100;
            const float buttonSpace = 4;
            var column = Mathf.FloorToInt((EditorGUIUtility.currentViewWidth - 10) / (buttonSize + buttonSpace));
            var row = Mathf.CeilToInt(prefabs.Count * 1.0f / column);
            for (var i = 0; i < row; i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (var j = 0; j < column; j++)
                {
                    var index = i * column + j;
                    if (index >= prefabs.Count)
                    {
                        break;
                    }

                    EditorGUILayout.BeginVertical();
                    GUI.color = _selectedPrefab == prefabs[index] ? Color.cyan : Color.white;
                    if (GUILayout.Button(AssetPreview.GetAssetPreview(prefabs[index].gameObject), GUILayout.Width(100),
                        GUILayout.Height(100)))
                    {
                        _selectedPrefab = prefabs[index];
                    }

                    GUILayout.Label(prefabs[index].name);
                    EditorGUILayout.EndVertical();
                }

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }


        void PlaceObject(LevelMap level)
        {
            cam = SceneView.currentDrawingSceneView.camera;
            if (!MouseInView) return;
            if (_selectedPrefab == null) return;

            var mouseCast = RaycastPoint(level, EventMousePoint);
            var ec = Event.current;
            var hsize = HandleUtility.GetHandleSize(mouseCast) * 0.4f;
            Handles.color = new Color(1, 0, 0, 0.5f);
            Handles.DrawSolidDisc(mouseCast, Vector3.forward, hsize * 0.5f);

            /*if (ec.type == EventType.MouseDown)
            {
                Debug.LogError($"mouseDown_{ec.type == EventType.MouseDown}, button1_{ec.button == 1}, control_{ec.control}");
            }*/

            if (ec.type == EventType.MouseDown && ec.button == 1 && ec.shift)
            {
                var newGo = PrefabUtility.InstantiatePrefab(_selectedPrefab) as GameObject;
                StageUtility.PlaceGameObjectInCurrentStage(newGo);

                newGo.transform.parent = level.transform;
                newGo.transform.position = new Vector3(Mathf.RoundToInt(mouseCast.x), mouseCast.y, 0);
                Selection.activeObject = newGo;
                EditorUtility.SetDirty(level.gameObject);

                Undo.RegisterCreatedObjectUndo(newGo, "place object");
            }
        }

        void NodesControl()
        {
            var ev = Event.current;
            var stageHandle = StageUtility.GetCurrentStageHandle();

            var allNodes = stageHandle.FindComponentsOfType<Node>();
            foreach (var node in allNodes)
            {
                node.gameObject.SetIcon(ShapeIcon.DiamondOrange);
            }

            var nodes = Selection.gameObjects.Where(i => i.GetComponent<Node>()).ToList();

            foreach (var node in nodes)
            {
                node.gameObject.SetIcon(ShapeIcon.DiamondRed);
            }

            if (ev.control && ev.type == EventType.MouseDown && ev.button == 1)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Link 2 Node %&a"), false, LinkTwoNode);
                menu.AddItem(new GUIContent("Unlink 2 Node Only %&s"), false, UnlinkTwoNodeOnly);
                menu.AddItem(new GUIContent("Unlink All With Nodes %&d"), false, UnlinkAllWithNodes);
                menu.AddItem(new GUIContent("Cancel %&z"), false, ResetSelection);

                menu.ShowAsContext();
            }
            else if (ev.control && ev.alt && ev.keyCode == KeyCode.A)
            {
                LinkTwoNode();
            }
            else if (ev.control && ev.alt && ev.keyCode == KeyCode.S)
            {
                UnlinkTwoNodeOnly();
            }
            else if (ev.control && ev.alt && ev.keyCode == KeyCode.D)
            {
                UnlinkAllWithNodes();
            }
            else if (ev.control && ev.alt && ev.keyCode == KeyCode.Z)
            {
                ResetSelection();
            }

        }

        private void ResetSelection()
        {
            var nodes = Selection.gameObjects.Where(i => i.GetComponent<Node>()).ToList();
            foreach (var node in nodes)
            {
                node.gameObject.SetIcon(ShapeIcon.DiamondOrange);
            }
            Selection.activeGameObject = null;
        }

        private void UnlinkTwoNodeOnly()
        {
            NodeUtility.UnlinkTwoNode();
            ResetSelection();
        }

        private void UnlinkAllWithNodes()
        {
            /*nodeutility.unlink();
            resetselection();*/
        }

        private void LinkTwoNode()
        {
            NodeUtility.LinkNodes();
            ResetSelection();
        }

        bool MouseInView
        {
            get
            {
                try
                {
                    return mouseOverWindow != null && mouseOverWindow.titleContent != null &&
                           string.Equals("Scene", mouseOverWindow.titleContent.text);
                }
                catch
                {
                    return false;
                }
            }
        }

        Vector2 EventMousePoint
        {
            get
            {
                var v = Event.current.mousePosition;
                v.y = Screen.height - v.y - 60f;
                return v;
            }
        }

        Vector3 RaycastPoint(LevelMap level, Vector2 screenPoint, float dist = 10)
        {
            var r = cam.ScreenPointToRay(screenPoint);
            if (!RayCast(level, r, out var v))
            {
                v = r.origin + r.direction.normalized * dist;
            }
            return v;
        }

        bool RayCast(LevelMap level, Ray r, out Vector3 hitPoint)
        {
            hitPoint = Vector3.zero;

            RaycastHit hit;
            if (level.gameObject.scene.GetPhysicsScene().Raycast(r.origin, r.direction, out hit))
            {
                hitPoint = hit.point;
                return true;
            }

            return false;
        }

    }
#endif
}
