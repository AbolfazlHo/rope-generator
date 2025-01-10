using UnityEditor;
using UnityEngine;

namespace Soor.RopeGenerator
{
    public static class Utility
    {
        /// <summary>
        /// Converts an absolute path to a path relative to the project folder.
        /// </summary>
        public static string ConvertAbsolutePathToRelatedToProjectPath(string absolutePath)
        {
            return absolutePath.Remove(0, ProjectPath().Length);
        }

        /// <summary>
        /// Returns the path to the project folder.
        /// </summary>
        public static string ProjectPath()
        {
            return Application.dataPath.Remove(Application.dataPath.Length - 6, 6);
        }

#if UNITY_EDITOR
        ///<summary>
        /// Adds a visual separator and label to separate the target section from the latest section in the editor window.
        ///</summary>
        ///<param name="sectionName">The name of the target section to be separated.</param>
        public static void SeparateEditorWindowSection(string sectionName)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField(sectionName);
            EditorGUILayout.Space();
        }
#endif
        
    }
}
