using System.Collections.Generic;
using UnityEngine;

public class FurnitureController : MonoBehaviour
{
    [SerializeField] private GameObject mainObject, blueprintObject;
    private Collider collider;

    public float price;

    public Transform standPoint;

    public List<ShelfSpaceController> shelves;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<Collider>();
        if(shelves.Count > 0)
        {
            StoreController.Instance.shelvingCases.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MakePlaceable()
    {
        mainObject.SetActive(false);
        blueprintObject.SetActive(true);
        collider.enabled = false;
    }
    public void PlaceFurniture()
    {
        mainObject.SetActive(true);
        blueprintObject.SetActive(false);
        collider.enabled = true;
    }
}
