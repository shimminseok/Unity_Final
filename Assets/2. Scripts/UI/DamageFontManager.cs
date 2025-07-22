using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;


public enum DamageType
{
    Normal,
    Critical,
    Heal,
    Miss,
    Immune
}

public class DamageFontManager : SceneOnlySingleton<DamageFontManager>
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private DamageNumber normalDamageNumber;
    [SerializeField] private DamageNumber criticalDamageNumber;
    [SerializeField] private DamageNumber healDamageNumber;
    [SerializeField] private DamageNumber missDamageNumber;
    [SerializeField] private DamageNumber immuneDamageNumber;
    [SerializeField] private Camera mainCamera;

    private Dictionary<DamageType, DamageNumber> damageNumberMap = new();

    protected override void Awake()
    {
        base.Awake();
        damageNumberMap = new Dictionary<DamageType, DamageNumber>
        {
            { DamageType.Normal, normalDamageNumber },
            { DamageType.Critical, criticalDamageNumber },
            { DamageType.Heal, healDamageNumber },
            { DamageType.Miss, missDamageNumber },
            { DamageType.Immune, immuneDamageNumber }
        };
    }

    private void Start()
    {
        // mainCamera = Camera.main;
    }

    public void SetDamageNumber(IDamageable target, float damage, DamageType damageType)
    {
        Vector3 worldPos  = target.Collider.transform.position + new Vector3(0, 1.5f, 0);
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, // ✅ canvas 기준!
            screenPos,
            null,
            out Vector2 anchoredPos
        );
        if (!damageNumberMap.TryGetValue(damageType, out DamageNumber damageNumber))
        {
            Debug.LogWarning($"DamageNumber for type {damageType} is not defined.");
            return;
        }

        if (damageType == DamageType.Miss)
        {
            damageNumber.SpawnGUI(rectTransform, anchoredPos, "MISS");
        }
        else if (damageType == DamageType.Immune)
        {
            damageNumber.SpawnGUI(rectTransform, anchoredPos, "IMMUNE");
        }
        else
        {
            damageNumber.SpawnGUI(rectTransform, anchoredPos, damage);
        }
    }
}