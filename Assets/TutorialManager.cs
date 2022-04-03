using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject Door;
    public GameManager GameManager;

    void Update()
    {
        if (GameManager.DeliveriesTotal >= 1)
        {
            Door.SetActive(false);
        }
    }
}
