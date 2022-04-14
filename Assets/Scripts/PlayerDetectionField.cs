using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionField : MonoBehaviour
{
    [SerializeField]
    private Enemy _enemy;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            _enemy.PlayerSeen(other.GetComponent<Player>());
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            _enemy.PlayerLeft(other.GetComponent<Player>());
    }
}
