using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Soor.RopeGenerator.Editor
{
    [ExecuteInEditMode]
    public static class RopePhysicsSimulator
    {
        /// <summary>
        /// Determines whether or not RopePhysicsSimulator is registered.
        /// </summary>
        public static bool registered = false;

        /// <summary>
        /// Registers the RopePhysicsSimulator with the EditorApplication.update.
        /// </summary>
        public static void Register()
        {
            EditorApplication.update += Update;
            registered = true;
        }

        /// <summary>
        /// Starts the physics simulation by waking up all rigidbodies for rope segments and enabling physics simulation.
        /// </summary>
        public static void Activate()
        {
            var allRopeSegments = GetAllRopeSegmentRigidbodies();
            Physics.autoSimulation = false;
            WakeUpAllRopeSegmentRigidbodies(allRopeSegments);
            Physics.autoSimulation = true;
            Debug.unityLogger.Log("Physics simulation started.");
        }

        /// <summary>
        /// Stops the physics simulation by disabling physics simulation.
        /// </summary>
        public static void Deactivate()
        {
            Physics.autoSimulation = false;
            Debug.unityLogger.Log("Physics simulation stopped.");
        }

        /// <summary>
        /// Finds all rigidbodies in the scene that belong to objects tagged as "RopeSegment" and returns them as a list.
        /// </summary>
        /// <returns>A list of all RopeSegment Rigidbodies in the scene.</returns>
        private static List<Rigidbody> GetAllRopeSegmentRigidbodies()
        {
            var allRopeSegments = Object.FindObjectsOfType<Rigidbody>().ToList();
            allRopeSegments = allRopeSegments.Where(rb => rb.gameObject.CompareTag("RopeSegment")).ToList();
            return allRopeSegments;
        }

        /// <summary>
        /// Wakes up all rigidbodies passed in as a parameter.
        /// </summary>
        /// <param name="allRopeSegmentRigidbodies">A list of all RopeSegment Rigidbodies to wake up.</param>
        private static void WakeUpAllRopeSegmentRigidbodies(List<Rigidbody> allRopeSegmentRigidbodies)
        {
            foreach (Rigidbody body in allRopeSegmentRigidbodies)
            {
                body.WakeUp();
            }
        }

        /// <summary>
        /// is used to update the state of the physics simulation each frame.
        /// </summary>
        private static void Update()
        {
            if (!Physics.autoSimulation) return;
            Physics.autoSimulation = false;
            Physics.Simulate(Time.deltaTime);
            Physics.autoSimulation = true;
        }
    }
}