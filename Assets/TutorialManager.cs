using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup[] _grps;

    [SerializeField]
    private GameManager GameManager;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private float _fadeSpeed = 1;

    const int JUMP_POS = -24;
    const int DOUBLE_POS = -13;
    const int POSTBOX_POS = 0;
    const int POLICE_POS = 30;
    const int UNTIL_THEN_POS = 54;
    const int KEEP_DELIVER_POS = 64;
    const int FINAL_POS = 75;

    void Start()
    {
        foreach (CanvasGroup grp in _grps)
        {
            grp.interactable = grp.blocksRaycasts = false;
            grp.alpha = 0;
        }

        StartCoroutine(TutorialRoutine());
    }

    IEnumerator TutorialRoutine()
    {
        while (_player.transform.position.x < JUMP_POS)
            yield return new WaitForSeconds(0.1f);

        _grps[0].DOFade(1, _fadeSpeed);

        while (_player.transform.position.x < DOUBLE_POS)
            yield return new WaitForSeconds(0.1f);

        _grps[1].DOFade(1, _fadeSpeed);

        while (_player.transform.position.x < POSTBOX_POS)
            yield return new WaitForSeconds(0.1f);

        yield return new WaitForSeconds(0.5f);
        _grps[2].DOFade(1, _fadeSpeed);
        
        while (GameManager.DeliveriesTotal == 0)
            yield return new WaitForSeconds(0.1f);

        yield return new WaitForSeconds(1);
        _grps[3].DOFade(1, _fadeSpeed);

        while (_player.transform.position.x < POLICE_POS)
            yield return new WaitForSeconds(0.1f);

        yield return new WaitForSeconds(1.5f);
        _grps[4].DOFade(1, _fadeSpeed);

        yield return new WaitForSeconds(3);

        _grps[5].DOFade(1, _fadeSpeed);

        yield return new WaitForSeconds(3);

        _grps[6].DOFade(1, _fadeSpeed);

        while (_player.transform.position.x < UNTIL_THEN_POS)
            yield return new WaitForSeconds(0.1f);

        _grps[7].DOFade(1, _fadeSpeed);
        
        while (_player.transform.position.x < KEEP_DELIVER_POS)
            yield return new WaitForSeconds(0.1f);

        _grps[8].DOFade(1, _fadeSpeed);

        while (_player.transform.position.x < FINAL_POS)
            yield return new WaitForSeconds(0.1f);

        _player.DisablePlayer();
    }
}
