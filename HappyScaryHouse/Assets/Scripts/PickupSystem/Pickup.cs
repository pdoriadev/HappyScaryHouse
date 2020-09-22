using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class Pickup : MonoBehaviour
{
    [SerializeField]
    private GameObject ThingToPickupPrefab = default;
    [SerializeField]
    private Sprite Sprite = default;
    [SerializeField]
    private SpriteRenderer SR = default;
    [SerializeField]
    private PickupPoolSO Pool = default;
    public PickupPoolSO pool => Pool;
    private bool IsInScene = false;
    public bool isInScene => IsInScene;



    private void Awake()
    {
        IsInScene = true;
        if (SR == null)
            Debug.LogError("Missing sprite renderer component");
        SR.sprite = Sprite;
    }
    private void OnDestroy()
    {
        IsInScene = false;
    }
    public GameObject GetPickup()
    {
        OnPickup();
        return ThingToPickupPrefab;
    }
    private void OnPickup()
    {
        Debug.Log("Picked up " + ThingToPickupPrefab.name);
        pool.PoolAPickup(this);
        gameObject.SetActive(false);
    }
    
}
