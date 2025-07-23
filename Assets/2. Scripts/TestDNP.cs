using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDNP : MonoBehaviour
{
    public DamageNumber dn;
    public RectTransform rect;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dn.SpawnGUI(rect, Vector2.zero);
        }
    }
}