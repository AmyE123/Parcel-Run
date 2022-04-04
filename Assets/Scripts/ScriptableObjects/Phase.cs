using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GamePhase", menuName = "ScriptableObjects/GamePhase")]
public class Phase : ScriptableObject
{
    public enum Transport
    {
        Foot,
        Bike,
        Moped
    }

    public enum SpawnPoint
    {
        North,
        East,
        South,
        West
    }

    [System.Serializable]
    public class EnemyDrop
    {
        [HideInInspector]
        public string name;

        public int packageRequirement;
        public int numberToDrop;
        public SpawnPoint point;
        public GameObject prefabToSpawn;

        public void Validate()
        {
            if (prefabToSpawn == null)
                name = $"{numberToDrop} x ??? will spawn after {packageRequirement} deliveries (spawn: {point})";
            else
                name = $"{numberToDrop} x {prefabToSpawn.name} will spawn after {packageRequirement} deliveries (spawn: {point})";
        }
    }

    [System.Serializable]
    public class GameBalance
    {
        [Header("Police")]
        [Tooltip("How close can the player get before they're noticed?")]
        public float policeDetectRadius = 10;
        [Tooltip("How far can the player be seen for until they lose sight?")]
        public float policeVisionRange = 20;
        public float policeTopSpeed = 5;
        public float policeAcceleration = 15;
        [Tooltip("How close do they need to be to the player before diving?")]
        public float policeDiveDistance = 3;
        [Tooltip("How fast do they move while diving?")]
        public float policeDiveSpeed = 8;
        [Tooltip("How long will they wait on the floor after diving?")]
        public float policeDiveRecoveryTime = 1;

        [Header("Soldier")]
        [Tooltip("How close can the player get before they're noticed?")]
        public float soliderDetectRadius = 12;
        [Tooltip("How far can the player be seen for until they lose sight?")]
        public float soliderVisionRange = 20;
        public float soliderTopSpeed = 6;
        public float soliderAcceleration = 15;
        [Tooltip("How close do they need to be to the player before shooting?")]
        public float soliderShootDistance = 8;
        [Tooltip("How long do they spend lining up their shot?")]
        public float soldierAimTime = 1.5f;
        [Tooltip("How long will they wait after lining up the shot before shooting?")]
        public float soldierLockTime = 0.5f;
        [Tooltip("How long will they wait after shooting before moving again?")]
        public float soldierRecoilTime = 0.5f;
    }

    // [Header("Enemy Squad Information")]
    // public GameObject enemyPrefab;

    // [Tooltip("The number of enemies you want the phase to begin with, leave at 0 if you want them to spawn over time")]
    // public int minEnemyCount;

    // [Tooltip("The maximum number of enemies you want the phase to have as it progresses")]
    // public int maxEnemyCount;

    public EnemyDrop[] enemyDrops;
    public GameBalance balanceInfo;

    [Header("Gameplay Phase Information")]
    // There is a minimum and a maximum time here so we can randomly generate a time between this range for each parcel in this phase.
    // [Tooltip("The maximum time a player has to deliver each parcel")]
    // public float maxParcelDeliveryTime;

    // [Tooltip("The minimum time a player has to deliver each parcel")]
    // public float minParcelDeliveryTime;

    // [Tooltip("The time in seconds before another enemy (of the above type) joins the chase during this phase")]
    // [Range(1, 60)] public float timeBeforeNextPopulation;

    [Tooltip("How many parcels a player needs to deliver to get onto the next phase")]
    [Range(1, 20)] public int deliveryCountToPass;

    [Tooltip("The transport the player should use during this phase")]
    public Transport playerTransport;

    public GameObject cutscenePrefab;

    // Called when something changes in the editor
    void OnValidate()
    {
        foreach (var info in enemyDrops)
            info.Validate();
    }
}
