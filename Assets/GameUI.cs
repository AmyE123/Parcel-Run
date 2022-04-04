using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    private LoseScreen _loseScreen;

    private GameManager _gameManager;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _loseScreen.gameObject.SetActive(false);
    }

    public void TriggerLoseScreen()
    {
        _loseScreen.SetValues(_gameManager.DeliveriesTotal, 10);
        _loseScreen.gameObject.SetActive(true);
        _loseScreen.StartAllTweens();
    }
}
