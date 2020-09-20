using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class Pickup : MonoBehaviour
{
    [SerializeField]
    private GameObject PickupPrefab = default;
    [SerializeField]
    private Sprite Sprite = default;
    [SerializeField]
    private SpriteRenderer SR = default;

    private void Awake()
    {
        if (SR == null)
            Debug.LogError("Missing sprite renderer component");
        SR.sprite = Sprite;
    }
    public GameObject GetPickup()
    {
        OnPickup();
        return PickupPrefab;
    }
    private void OnPickup()
    {
        Debug.Log("Picked up " + PickupPrefab.name);
        gameObject.SetActive(false);
    }
}
