using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamagerMono : MonoBehaviour
{
    [SerializeField]
    protected DamagerSO Data;
    public DamagerSO data => Data;
    public abstract void Attack();
}
