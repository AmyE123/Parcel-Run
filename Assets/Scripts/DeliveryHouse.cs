using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeliveryHouse : MonoBehaviour
{
    [SerializeField]
    private Transform _doorObject;

    [SerializeField]
    private Vector3 _punchScale = Vector3.one;
    

    public Vector3 DoorPosition => _doorObject.position;

    public AudioSource SFX;
    public AudioClip DeliverAudio;
    

    public bool IsThisYourDoor(Transform obj) => _doorObject == obj;

    public void Start()
    {
        SFX = GameObject.Find("SoundFX").GetComponent<AudioSource>();
    }

    public void ItemDelivered()
    {
        SFX.PlayOneShot(DeliverAudio);
        transform.DOScale(_punchScale, 0.1f).SetEase(Ease.OutSine).OnComplete(() => 
        {
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        });
    }
}
