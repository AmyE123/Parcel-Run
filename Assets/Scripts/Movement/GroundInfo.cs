using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementInfo
{
    [System.Serializable]
    public class GroundInfo
    {
        [Range(0f, 90f)] public float maxSlopeAngle = 25f;
        [Range(0f, 100f)] public float maxSnapSpeed = 100f;
	    [Min(0f)] public float probeDistance = 1f;

        [HideInInspector] public int stepsSinceLastGrounded;
        [HideInInspector] public float minGroundDotProduct;

        [HideInInspector] public Vector3 contactNormal;
        [HideInInspector] public int groundContactCount;

        [HideInInspector] public Vector3 wallNormal;
        [HideInInspector] public int wallContactCount;

    }
}