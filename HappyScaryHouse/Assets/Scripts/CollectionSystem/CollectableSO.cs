using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectableSO", menuName = "CollectableSystem/CollectableSO", order = 2)]
public class CollectableSO : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public CollectableSet set;

    // PD - 9/19/2020 
    // #TODO - additional collectable functionality
    

}
