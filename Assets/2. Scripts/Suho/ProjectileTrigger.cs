using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrigger : MonoBehaviour
{
    public IDamageable target;
    public event Action OnTriggerTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() == target)
        {
            OnTriggerTarget?.Invoke();

            OnTriggerTarget = null;
        }
    }
}