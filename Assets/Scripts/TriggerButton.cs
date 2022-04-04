using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerButton : MonoBehaviour
{
    public GameObject Door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            Door.SetActive(false);
        }
    }
}
