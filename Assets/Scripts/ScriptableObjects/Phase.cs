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

    [Header("Enemy Squad Information")]
    public GameObject enemyPrefab;

    public int maxEnemyCount;

    [Header("Gameplay Phase Information")]
    // There is a minimum and a maximum time here so we can randomly generate a time between this range for each parcel in this phase.
    [Tooltip("The maximum time a player has to deliver each parcel")]
    public float maxParcelDeliveryTime;

    [Tooltip("The minimum time a player has to deliver each parcel")]
    public float minParcelDeliveryTime;

    [Tooltip("How many parcels a player needs to deliver to get onto the next phase")]
    [Range(1, 20)] public int deliveryCountToPass;

    [Tooltip("The transport the player should use during this phase")]
    public Transport playerTransport;
}
