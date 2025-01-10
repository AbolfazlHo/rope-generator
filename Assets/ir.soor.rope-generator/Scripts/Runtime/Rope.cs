using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Soor.RopeGenerator
{
    public class Rope
    {
        /// <summary>
        /// The parent object of the rope. Used to move the entire rope.
        /// </summary>
        private GameObject _ropeParent;
        
        /// <summary>
        /// The data of the rope.
        /// </summary>
        private RopeData _ropeData = new RopeData();

        /// <summary>
        /// Initializes a new instance of the Rope class with the specified parameters.
        /// </summary>
        /// <param name="ropeLength">The length of the rope in Unity units.</param>
        /// <param name="segmentsDistance">The distance between each pair of adjacent rope segments in Unity units.</param>
        /// <param name="ropeThickness">The thickness of the rope.</param>
        /// <param name="segmentPrefab">The prefab GameObject used to create each individual rope segment.</param>
        /// <param name="ropeParent">The parent GameObject of the rope; moving it moves the entire rope.</param>
        /// <param name="freezeFirstRopeSegment">Determines whether or not the position of the first rope segment is fixed.</param>
        /// <param name="freezeLastRopeSegment">Determines whether or not the position of the last rope segment is fixed.</param>
        /// <param name="setMaterialWhenSpawning">Determines whether or not a material is set to each rope segment when it is spawned. Defaults to false.</param>
        /// <param name="segmentsMaterial">The material to set to all rope segments if setMaterialWhenSpawning is true. Defaults to null.</param>
        public Rope(float ropeLength, float segmentsDistance, float ropeThickness, GameObject segmentPrefab, GameObject ropeParent,
            bool freezeFirstRopeSegment, bool freezeLastRopeSegment, bool setMaterialWhenSpawning = false, Material segmentsMaterial = null)
        {
            _ropeData.ropeLength = ropeLength;
            _ropeData.segmentsDistance = segmentsDistance;
            _ropeData.ropeThickness = ropeThickness;
            _ropeData.segmentPrefab = segmentPrefab;
            _ropeData.setMaterialWhenSpawning = setMaterialWhenSpawning;
            _ropeData.segmentsMaterial = segmentsMaterial;
            _ropeData.freezeFirstRopeSegment = freezeFirstRopeSegment;
            _ropeData.freezeLastRopeSegment = freezeLastRopeSegment;
            
            _ropeParent = ropeParent;
        }

        /// <summary>
        /// Initializes a new instance of the Rope class.
        /// </summary>
        /// <param name="ropeData">The data of the rope.</param>
        /// <param name="ropeParent">The parent GameObject of the rope; moving it moves the entire rope.</param>
        public Rope(RopeData ropeData, GameObject ropeParent = null)
        {
            _ropeData = ropeData;
            _ropeParent = ropeParent;
        }

        /// <summary>
        /// Instantiates a chain of connected rope segments based on the values of the Rope object's fields.
        /// </summary>
        public void SpawnRope()
        {
            if (!_ropeParent) InstantiateRopeParent();
            var ropeSegmentCount = (int) (_ropeData.ropeLength / _ropeData.segmentsDistance);
            SpawnAllRopeSegments(ropeSegmentCount);
            PrepareLastRopeSegment();
        }
        
        /// <summary>
        /// Spawns all the rope segments in the chain.
        /// </summary>
        /// <param name="segmentsCount">The total number of rope segments to spawn.</param>
        private void SpawnAllRopeSegments(int segmentsCount)
        {
            for (var i = 0; i < segmentsCount; i++)
            {
                var newRopeSegment = SpawnRopeSegment(i);
                if (i == 0) PrepareFirstRopeSegment(newRopeSegment);
                else ConnectSegment(newRopeSegment);
            }
        }
        
        /// <summary>
        /// Instantiates the parent object for the rope segments.
        /// </summary>
        private void InstantiateRopeParent()
        {
            _ropeParent = new GameObject();
            _ropeParent.transform.position = Vector3.zero;
#if UNITY_EDITOR
            _ropeParent.transform.position = SceneView.lastActiveSceneView.pivot;
#else
            if (Camera.main != null)
                _ropeParent.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
#endif
        }

        /// <summary>
        /// Spawns a rope segment at the specified index in the chain.
        /// </summary>
        /// <param name="segmentIndex">The zero-based index of the rope segment in the chain.</param>
        /// <returns>The spawned rope segment GameObject.</returns>
        private GameObject SpawnRopeSegment(int segmentIndex)
        {
            var position = _ropeParent.transform.position;
            var newRopeSegment = Object.Instantiate(_ropeData.segmentPrefab,
                new Vector3(position.x, position.y + _ropeData.segmentsDistance * (segmentIndex + 1), position.z),
                Quaternion.identity, _ropeParent.transform);
            ModifySegmentTransform(newRopeSegment);
            newRopeSegment.name = _ropeParent.transform.childCount.ToString();
            ModifySegmentMaterial(newRopeSegment);
            return newRopeSegment;
        }
        
        /// <summary>
        /// Modifies the transform of a rope segment.
        /// </summary>
        /// <param name="ropeSegment">The rope segment GameObject.</param>
        private void ModifySegmentTransform(GameObject ropeSegment)
        {
            ropeSegment.transform.localScale = new Vector3(_ropeData.ropeThickness, _ropeData.ropeThickness/2, _ropeData.ropeThickness);
            ropeSegment.transform.eulerAngles = new Vector3(180, 0, 0);
        }
        
        /// <summary>
        /// Modifies the material of a rope segment.
        /// </summary>
        /// <param name="ropeSegment">The rope segment GameObject.</param>
        private void ModifySegmentMaterial(GameObject ropeSegment)
        {
            if (!_ropeData.setMaterialWhenSpawning || _ropeData.segmentsMaterial == null) return;
            ropeSegment.transform.GetChild(0).GetComponent<Renderer>().material = _ropeData.segmentsMaterial;
            
        }
        
        /// <summary>
        /// Prepares the first rope segment by removing its CharacterJoint component and optionally freezing its position.
        /// </summary>
        /// <param name="firstRopeSegment">The first rope segment GameObject.</param>
        private void PrepareFirstRopeSegment(GameObject firstRopeSegment)
        {
            Object.DestroyImmediate(firstRopeSegment.GetComponent<CharacterJoint>());
            if (!_ropeData.freezeFirstRopeSegment) return;
            FreezeRigidbodyPosition(firstRopeSegment.GetComponent<Rigidbody>());
        }
        
        /// <summary>
        /// Connects a rope segment to the previous segment in the chain.
        /// </summary>
        /// <param name="segment">The rope segment to connect.</param>
        private void ConnectSegment(GameObject segment)
        {
            segment.GetComponent<CharacterJoint>().connectedBody = _ropeParent.transform
                .Find((_ropeParent.transform.childCount - 1).ToString()).GetComponent<Rigidbody>();
        }
        
        /// <summary>
        /// Prepares the last rope segment by optionally freezing its position.
        /// </summary>
        private void PrepareLastRopeSegment()
        {
            if (!_ropeData.freezeLastRopeSegment) return;
            var lastRopeSegment = _ropeParent.transform.Find(_ropeParent.transform.childCount.ToString());
            FreezeRigidbodyPosition(lastRopeSegment.GetComponent<Rigidbody>());
        }

        /// <summary>
        /// Prevents the target Rigidbody from moving by freezing its position.
        /// </summary>
        /// <param name="rigidbody">The target Rigidbody which its positiion will be freezed.</param>
        private void FreezeRigidbodyPosition(Rigidbody rigidbody)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezePosition;
        }
        
    }
}