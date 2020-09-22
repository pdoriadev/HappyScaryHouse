using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamagerMono : MonoBehaviour
{
    [SerializeField]
    protected DamagerSO DamagerSO;
    public DamagerSO damagerSO => DamagerSO;
    public abstract void Attack();
}
