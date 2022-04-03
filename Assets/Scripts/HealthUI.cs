using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _heartPrefab;

    [SerializeField]
    private List<HeartUI> _uiItems;

    [SerializeField]
    private float _timeBeforeReduce = 1;

    [SerializeField]
    private float _timePerHeartReduce = 0.5f;

    public void InitHealth(int totalHealth)
    {
        int numHearts = Mathf.CeilToInt(totalHealth / 4f);

        for (int i=0; i<numHearts; i++)
        {
            GameObject newObj = Instantiate(_heartPrefab, transform);
            HeartUI newUI = newObj.GetComponent<HeartUI>();

            int quarters = Mathf.Min(totalHealth - (4 * i), 4);
            newUI.SetInitialFillAmount(quarters);

            _uiItems.Add(newUI);
        }
    }

    public void HealthChanged(int newHealth)
    {
        for (int i=0; i<_uiItems.Count; i++)
        {
            int quarters = Mathf.Min(newHealth - (4 * i), 4);

            if (quarters < 4)
                _uiItems[i].TakeDamage(quarters);
        }

        StopAllCoroutines();
        StartCoroutine(ReduceHealthRoutine(newHealth));
    }

    IEnumerator ReduceHealthRoutine(int newHealth)
    {
        yield return new WaitForSeconds(_timeBeforeReduce);

        for (int i=_uiItems.Count-1; i>=0; i--)
        {
            int quarters = Mathf.Clamp(newHealth - (4 * i), 0, 4);

            HeartUI thisHeart = _uiItems[i];

            if (thisHeart == null)
            {
                _uiItems.RemoveAt(i);
                continue;
            }

            float reducePercent = thisHeart.GetReducePercent();
            
            if (reducePercent > 0)
            {
                thisHeart.AnimateHeartAway(reducePercent * _timePerHeartReduce);
                yield return new WaitForSeconds(reducePercent * _timePerHeartReduce);
            }
        }
    }
}
