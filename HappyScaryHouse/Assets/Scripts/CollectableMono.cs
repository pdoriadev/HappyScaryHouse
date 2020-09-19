using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class CollectableMono : MonoBehaviour
{
    [SerializeField]
    private CollectableSO Data = default;
    private SpriteRenderer SR;


    private void Awake()
    {
        SR = GetComponent<SpriteRenderer>();
        SR.sprite = Data.sprite;
    } 
    private void Start()
    {
        Data.set.AddUncollected(this);
    }

    public void OnCollect()
    {
        Data.set.AddCollected(this);
        gameObject.SetActive(false);
    }
}
