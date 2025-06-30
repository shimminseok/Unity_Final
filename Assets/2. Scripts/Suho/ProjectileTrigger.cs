using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrigger : MonoBehaviour
{
    public Unit target;
    public Action OnTriggerTarget;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Unit>() == target)
        {
            OnTriggerTarget?.Invoke();
        }
    }
}
