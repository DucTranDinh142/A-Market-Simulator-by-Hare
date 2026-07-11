using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StockBoxController : MonoBehaviour
{
    public StockInfo info;

    [Header("Box Space Position List")]
    [SerializeField] private List<Transform> fruitSmallPositions;
    [SerializeField] private List<Transform> bottlePositions;
    [SerializeField] private List<Transform> boxPositions;
    [SerializeField] private List<Transform> fruitLargePositions;
    [SerializeField] private List<Transform> canPositions;
    [Space]
    public List<StockObject> stocksInBox;

    public Rigidbody _boxRigidbody { get; private set; }
    public Collider _boxCollider { get; private set; }
    public bool _isHeld { get; private set; }
    [Header("Box Dev Settings")]
    [SerializeField] private float _moveSpeed = 5f;

    private Animator _boxAnimatorController;
    public bool opened { get; private set; }
    void Awake()
    {
        _boxRigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<Collider>();
        _boxAnimatorController = GetComponentInChildren<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        _boxAnimatorController.SetBool("Open", opened);

        if (_isHeld)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                Vector3.zero,
                _moveSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation,
                Quaternion.identity,
                _moveSpeed * Time.deltaTime);
        }
    }

    public void SetupBox(StockInfo stockInfo)
    {
        info = stockInfo;

        List<Transform> positions = new List<Transform>();

        switch(info.stockType)
        {
            case StockType.FruitSmall:
                positions.AddRange(fruitSmallPositions);
                break;
            case StockType.Bottle:
                positions.AddRange(bottlePositions);
                break;
            case StockType.Box:
                positions.AddRange(boxPositions);
                break;
            case StockType.FruitLarge:
                positions.AddRange(fruitLargePositions);
                break;
            case StockType.Can:
                positions.AddRange(canPositions);
                break;
        }

        if(stocksInBox.Count == 0)
        {
            for(int i = 0; i < positions.Count; i++)
            {
                StockObject stock = Instantiate(stockInfo.stockObject, positions[i]);
                stock.transform.localPosition = Vector3.zero;
                stock.transform.localRotation = Quaternion.identity;

                stocksInBox.Add(stock);
                stock.PlaceInBox();
            }
        }
    }

    public void Pickup()
    {
        _boxRigidbody.isKinematic = true;
        _boxCollider.enabled = false;
        _isHeld = true;
    }

    public void Release()
    {
        _boxRigidbody.isKinematic = false;
        _boxCollider.enabled = true;
        _isHeld = false;
    }
    public void OpenClose()
    {
        opened = !opened;
    }

    public void PlaceStockOnShelf(ShelfSpaceController shelf)
    {
        if(stocksInBox.Count > 0)
        {
            shelf.PlaceStock(stocksInBox[stocksInBox.Count-1]);

            if (stocksInBox[stocksInBox.Count-1]._isPlaced == true)
            {
                stocksInBox.RemoveAt(stocksInBox.Count - 1);
            }
        }
    }

    public int GetStockAmountInBox(StockType type)
    {
        int amount = 0;

        switch (type)
        {
            case StockType.FruitSmall:
               amount = fruitSmallPositions.Count;
                break;
            case StockType.Bottle:
                amount = bottlePositions.Count;
                break;
            case StockType.Box:
                amount = boxPositions.Count;
                break;
            case StockType.FruitLarge:
                amount = fruitLargePositions.Count;
                break;
            case StockType.Can:
                amount = canPositions.Count;
                break;
        }

        return amount;
    }
}
