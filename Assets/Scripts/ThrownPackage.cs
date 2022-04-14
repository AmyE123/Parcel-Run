using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ThrownPackage : MonoBehaviour
{
    [SerializeField]
    private float _throwSpeed = 1;

    private DeliveryHouse _destination;

    // Start is called before the first frame update
    public void Throw(Vector3 start, DeliveryHouse destination)
    {
        _destination = destination;
        transform.position = start;

        float ran () => Random.Range(0, 360);

        transform.rotation = Quaternion.Euler(ran(), ran(), ran());
        Vector3 finalRotation = new Vector3(ran(), ran(), ran());

        float dist = Vector3.Distance(start, destination.DoorPosition);
        float time = dist / _throwSpeed;
        
        transform.DOJump(_destination.DoorPosition, 0.4f, 1, time).SetEase(Ease.Linear);
        transform.DORotate(finalRotation, time).SetEase(Ease.Linear).OnComplete(OnThrowComplete);
    }

    private void OnThrowComplete()
    {
        FindObjectOfType<DeliveryManager>().DeliveryMade();
        _destination.ItemDelivered();
        Destroy(gameObject);
    }
}
