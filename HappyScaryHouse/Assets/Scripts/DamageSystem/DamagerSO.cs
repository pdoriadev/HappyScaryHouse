using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DamagerSO", menuName = "DamageableSystem/DamagerSO", order = 3)]
public class DamagerSO : ScriptableObject
{
    [SerializeField]
    private string DamagerName = default;
    public string damagerName => DamagerName;
    [SerializeField]
    private int Damage = default;
    public int damage => Damage;
    [SerializeField]
    private Sprite Sprite= default;
    public Sprite sprite => Sprite;
    [SerializeField]
    private FactionSO Faction = default;
    public FactionSO faction => Faction;
}
