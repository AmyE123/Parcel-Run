using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementInfo
{
    [System.Serializable]
    public class MoveInfo
    {
        [Range(0f, 100f)] public float maxAcceleration = 10f;
        [Range(0f, 100f)] public float maxDecceleration = 10f;
        [Range(0f, 100f)] public float maxAirAcceleration = 1f;
        [Range(0f, 100f)] public float maxAirDecceleration = 1f;
        [Range(0f, 100f)] public float maxSpeed = 10f;

        [HideInInspector] public Vector3 velocity;
        [HideInInspector] public Vector3 desiredVelocity;
    }
}