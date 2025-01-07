using UnityEditor;
using UnityEngine;

namespace Soor.RopeGenerator.Editor
{
    public class RopeEditorWindow : EditorWindow
    {
        /// <summary>
        /// The parent object of the rope. Used to move the entire rope.
        /// </summary>
        private GameObject _ropeParent;

        /// <summary>
        /// The ScriptableObject of the rope's data.
        /// </summary>
        private RopeDataScriptableObject _ropeDataScriptableObject = null;

        /// <summary>
        /// The data of the rope.
        /// </summary>
        private RopeData _ropeData = new RopeData();

        /// <summary>
        /// Path to the rope data ScriptableObject when you are using the published version of the pacakge.
        /// </summary>
        private const string RopeDataPathWhenReleased =
            "Packages/ir.soor.rope-generator/ScriptableObjects/DefaultRopeData.asset";

        /// <summary>
        /// Path to the rope data ScriptableObject during development.
        /// </summary>
        private const string RopeDataPathInDevelopment = "Assets/ir.soor.rope-generator/ScriptableObjects/DefaultRopeData.asset";

        /// <summary>
        /// Loads the default RopeDataScriptableObject.
        /// </summary>
        public void LoadDefaultRopeDataScriptableObject()
        {
            // Load the ScriptableObject from the package path or the project path if it doesn't exist in the package.
            _ropeDataScriptableObject =
                AssetDatabase.LoadAssetAtPath<RopeDataScriptableObject>(RopeDataPathWhenReleased)
                ?? AssetDatabase.LoadAssetAtPath<RopeDataScriptableObject>(RopeDataPathInDevelopment);
            if (!DefaultRopeDataScriptableObjectSuccessfullyLoaded()) return;
            SerializeSettingRopeDataFromScriptableObject();
        }

        /// <summary>
        /// Checks if DefaultRopeDataScriptableObject loadded successfully or not.
        /// </summary>
        /// <returns>If DefaultRopeDataScriptableObject loadded successfully or not.</returns>
        private bool DefaultRopeDataScriptableObjectSuccessfullyLoaded()
        {
            if (_ropeDataScriptableObject) return true;
            Debug.unityLogger.LogError("Loading Default RopeDataSecriptableObject", "It seems there is no RopeDataSecriptableObject. | Check this package content.");
            return false;
        }

        /// <summary>
        /// Creates a new RopeDataScriptableObject asset file and saves it in the project.
        /// </summary>
        private void CreateNewRopeDataScriptableObject()
        {
            var path = GetScriptableObjectSavePath();
            if (string.IsNullOrEmpty(path)) return;
            var relativePath = Utility.ConvertAbsolutePathToRelatedToProjectPath(path);
            SaveDefaultRopeDataScriptableObjectAtPath(relativePath);
        }


        /// <summary>
        /// Creates and saves a new RopeDataScriptableObject with default values at the specified file path.
        /// </summary>
        /// <param name="path">The path to save the file to.</param>
        private void SaveDefaultRopeDataScriptableObjectAtPath(string path)
        {
            AssetDatabase.CreateAsset(CreateInstance<RopeDataScriptableObject>(), path);
            _ropeDataScriptableObject = AssetDatabase.LoadAssetAtPath<RopeDataScriptableObject>(path);
        }

        /// <summary>
        /// Shows the SaveFile panel and returns the path that the user selected to save the ScriptableObject asset to.
        /// </summary>
        /// <returns>The path to save the asset to (as a string).</returns>
        private string GetScriptableObjectSavePath()
        {
            var path = EditorUtility.SaveFilePanel(
                "Create a rope data ScriptableObject",
                Application.dataPath, name + ".asset",
                "asset");
            return path;
        }

        /// <summary>
        /// Sets the rope data fields based on the values in the RopeDataScriptableObject.
        /// </summary>
        private void GetRopeDataFromScriptabelObject()
        {
            _ropeData.customizeSegmentsDistance = _ropeDataScriptableObject.ropeData.customizeSegmentsDistance;
            _ropeData.segmentsDistance = _ropeDataScriptableObject.ropeData.segmentsDistance;
            _ropeData.ropeLength = _ropeDataScriptableObject.ropeData.ropeLength;
            _ropeData.ropeThickness = _ropeDataScriptableObject.ropeData.ropeThickness;
            _ropeData.setMaterialWhenSpawning = _ropeDataScriptableObject.ropeData.setMaterialWhenSpawning;
            _ropeData.segmentsMaterial = _ropeDataScriptableObject.ropeData.segmentsMaterial;
            _ropeData.freezeFirstRopeSegment = _ropeDataScriptableObject.ropeData.freezeFirstRopeSegment;
            _ropeData.freezeLastRopeSegment = _ropeDataScriptableObject.ropeData.freezeLastRopeSegment;
            _ropeData.segmentPrefab = _ropeDataScriptableObject.ropeData.segmentPrefab;
        }

        /// <summary>
        /// Serializes all Rope's data fields and displays them in the inspector.
        /// </summary>
        private void SerializeRopeData()
        {
            Utility.SeparateEditorWindowSection("Rope Data");
            
            // Allow the user to set the segment prefab for the rope.
            _ropeData.segmentPrefab =
                EditorGUILayout.ObjectField("Segment Prefab", _ropeData.segmentPrefab, typeof(GameObject)) as GameObject;

            // Allow the user to set the length and thickness of the rope.
            _ropeData.ropeLength = EditorGUILayout.FloatField("Rope Length", _ropeData.ropeLength);
            _ropeData.ropeThickness = EditorGUILayout.FloatField("Rope Thickness", _ropeData.ropeThickness);
            
            _ropeData.customizeSegmentsDistance = EditorGUILayout.Toggle("Customize Segments Distance", _ropeData.customizeSegmentsDistance);

            if (_ropeData.customizeSegmentsDistance)
            {
                // Show `segmentsDistance` in editor.
                _ropeData.segmentsDistance = EditorGUILayout.FloatField("Segments Distance", _ropeData.segmentsDistance);
            }
            else
            {
                // Set the segment distance to the same value as the rope thickness.
                _ropeData.segmentsDistance = _ropeData.ropeThickness;
            }
            
            _ropeData.setMaterialWhenSpawning = EditorGUILayout.Toggle("Set Custom Material when spawning",
                _ropeData.setMaterialWhenSpawning);

            if (_ropeData.setMaterialWhenSpawning)
            {
                _ropeData.segmentsMaterial =
                    EditorGUILayout.ObjectField("Segments Material", _ropeData.segmentsMaterial, typeof(Material)) as Material;
            }
            
            // Allow the user to freeze the first and last rope segments.
            _ropeData.freezeFirstRopeSegment =
                EditorGUILayout.Toggle("Freeze First RopeSegment", _ropeData.freezeFirstRopeSegment);
            _ropeData.freezeLastRopeSegment = EditorGUILayout.Toggle("Freeze Last RopeSegment", _ropeData.freezeLastRopeSegment);
        }

        /// <summary>
        /// Registers the RopePhysicsSimulator.
        /// </summary>
        private void RegisterRopePhysicsSimulator()
        {
            if (!RopePhysicsSimulator.registered) RopePhysicsSimulator.Register();
        }

        /// <summary>
        /// It serializes all inspector buttons.
        /// </summary>
        private void SerializeButtons()
        {
            if (GUILayout.Button("Spawn The Rope")) SpawnRope();
            if (GUILayout.Button("Save This Rope Data To The Current ScriptableObject"))
                SaveThisRopeDataToCurrentScriptableObject();
            Utility.SeparateEditorWindowSection("Physics Control");
            if (GUILayout.Button("Start Simulate Ropes Physics")) RopePhysicsSimulator.Activate();
            if (GUILayout.Button("Stop Simulate Ropes Physics")) RopePhysicsSimulator.Deactivate();
        }

        /// <summary>
        /// Saves the current rope data to the current RopeDataScriptableObject.
        /// </summary>
        private void SaveThisRopeDataToCurrentScriptableObject()
        {
            _ropeDataScriptableObject.ropeData.customizeSegmentsDistance = _ropeData.customizeSegmentsDistance; 
            _ropeDataScriptableObject.ropeData.segmentsDistance = _ropeData.segmentsDistance;
            _ropeDataScriptableObject.ropeData.ropeLength = _ropeData.ropeLength;
            _ropeDataScriptableObject.ropeData.ropeThickness = _ropeData.ropeThickness;
            _ropeDataScriptableObject.ropeData.setMaterialWhenSpawning = _ropeData.setMaterialWhenSpawning;
            _ropeDataScriptableObject.ropeData.segmentsMaterial = _ropeData.segmentsMaterial;
            _ropeDataScriptableObject.ropeData.freezeFirstRopeSegment = _ropeData.freezeFirstRopeSegment;
            _ropeDataScriptableObject.ropeData.freezeLastRopeSegment = _ropeData.freezeLastRopeSegment;
            _ropeDataScriptableObject.ropeData.segmentPrefab = _ropeData.segmentPrefab;
        }

        /// <summary>
        /// Creates a new rope instance and calls the `Spawn` method of it.
        /// </summary>
        private void SpawnRope()
        {
            // Create a new Rope instance with the current rope data and parent object.
            var newRope = new Rope(_ropeData, _ropeParent);

            // Spawn the rope.
            newRope.SpawnRope();
        }

        /// <summary>
        /// Serializes the buttons for loading and creating a new RopeDataScriptableObject asset.
        /// </summary>
        private void SerializeRopeDataInitializeButtons()
        {
            if (GUILayout.Button("Load Default RopeDataScriptableObject")) LoadDefaultRopeDataScriptableObject();
            if (GUILayout.Button("Create New RopeDataScriptableObject")) CreateNewRopeDataScriptableObject();
        }

        /// <summary>
        /// Displays the inspector window for the Rope Editor.
        /// </summary>
        [MenuItem("Tools/Rope Editor")]
        public static void ShowRopeEditorWindow()
        {
            // Get the RopeEditorWindow instance and set its title.
            EditorWindow ropeEditorWindow = GetWindow<RopeEditorWindow>();
            ropeEditorWindow.titleContent = new GUIContent("Rope Editor");
        }

        /// <summary>
        /// This method is called every frame to draw the GUI of the inspector.
        /// </summary>
        private void OnGUI()
        {
            EditorGUILayout.Space();
            SerializeWindowBasicFields();
            if (!RopeDataScriptableObjectHasSet()) return;
            SerializeSettingRopeDataFromScriptableObject();
            SerializeRopeData();
            RegisterRopePhysicsSimulator();
            SerializeButtons();
        }

        /// <summary>
        /// Serializes window's basic fields and displays them in the inspector.
        /// </summary>
        private void SerializeWindowBasicFields()
        {
            _ropeDataScriptableObject = EditorGUILayout.ObjectField("Rope Data ScriptableObject",
                _ropeDataScriptableObject, typeof(RopeDataScriptableObject)) as RopeDataScriptableObject;

            _ropeParent =
                EditorGUILayout.ObjectField("Rope Parent", _ropeParent, typeof(GameObject)) as GameObject;

        }

        /// <summary>
        /// Checks if the RopeDataScriptableObject has been set or not.
        /// </summary>
        /// <returns></returns>
        private bool RopeDataScriptableObjectHasSet()
        {
            if (_ropeDataScriptableObject) return true;
            SerializeRopeDataInitializeButtons();
            return false;
        }

        /// <summary>
        /// Serializes `Get Rope Data From ScriptableObject` button.
        /// </summary>
        private void SerializeSettingRopeDataFromScriptableObject()
        {
            if (GUILayout.Button("Get Rope Data From ScriptableObject")) GetRopeDataFromScriptabelObject();
        }
    }
}