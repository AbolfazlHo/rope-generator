using System;
using UnityEngine;

namespace Soor.RopeGenerator
{
    [Serializable]
    public class RopeData
    {
        /// <summary>
        /// It's the prefab of the rope segment. Each rope is a chain of this GameObject.
        /// </summary>
        public GameObject segmentPrefab;

        /// <summary>
        /// The length of the rope.
        /// </summary>
        public float ropeLength = 1.0f;
        
        /// <summary>
        /// If it's true, the `segmentsDistance` property will show in editor.
        /// </summary>
        public bool customizeSegmentsDistance = false;
        
        /// <summary>
        /// The distance between the anchor points of the two successive `ropeSegment`.
        /// </summary>
        public float segmentsDistance = 0.1f;
        
        /// <summary>
        /// If it's true, the `segmentsMaterial` property will show in editor.
        /// </summary>
        public bool setMaterialWhenSpawning = false;
        
        /// <summary>
        /// The material of any segment of the rope.
        /// </summary>
        public Material segmentsMaterial;
        
        /// <summary>
        /// The thickness of the rope.
        /// </summary>
        public float ropeThickness = 0.1f;

        /// <summary>
        /// If It's true, the position of the rope's first segment will freeze.
        /// </summary>
        public bool freezeFirstRopeSegment;

        /// <summary>
        /// If It's true, the position of the rope's last segment will freeze.
        /// </summary>
        public bool freezeLastRopeSegment;
    }
}