using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    public GameObject triggerObject;
    public float resetTime;
    public Animator animator;
    private Coroutine _coroutine;
    private TriggerObject _triggerObject;
    private int _triggerCount;

    private void Start()
    {
        _triggerCount = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.gameObject.layer.Equals(LayerMask.NameToLayer("InteractiveObject")))
        {
            _triggerCount++;
            if (animator != null)
            {
                animator.SetTrigger("EnterTrigger");
                animator.ResetTrigger("ExitTrigger");
            }
            _triggerObject = triggerObject.GetComponentInChildren<TriggerObject>();
            _triggerObject.TriggerActive();           
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.gameObject.layer.Equals(LayerMask.NameToLayer("InteractiveObject")))
        {
            _triggerCount--;
            if (_triggerCount == 0)
            {
                _triggerObject.TriggerInactive(resetTime);
                _coroutine = StartCoroutine(ResetButton());
            }
        }
    }

    IEnumerator ResetButton()
    {
        yield return new WaitForSeconds(resetTime);
        if (animator != null)
        {
            animator.SetTrigger("ExitTrigger");
            animator.ResetTrigger("EnterTrigger");
        }
    }
}
