using UnityEngine;

namespace Soor.RopeGenerator.Editor
{
    [CreateAssetMenu(fileName = "NewRopeData", menuName = "Rope/Rope Data")]
    public class RopeDataScriptableObject : ScriptableObject
    {
        public RopeData ropeData;
    }
}
