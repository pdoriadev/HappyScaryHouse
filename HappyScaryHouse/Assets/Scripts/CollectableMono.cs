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
        Debug.Log("add me plz");
        Data.set.AddUncollected(this);
    }

    public void OnCollect()
    {
        Debug.Log("called");
        Data.set.AddCollected(this);
        gameObject.SetActive(false);
    }
}
