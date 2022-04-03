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
    

    public bool IsThisYourDoor(Transform obj) => _doorObject == obj;

    public void ItemDelivered()
    {
        transform.DOScale(_punchScale, 0.1f).SetEase(Ease.OutSine).OnComplete(() => 
        {
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutElastic);
        });
    }
}
