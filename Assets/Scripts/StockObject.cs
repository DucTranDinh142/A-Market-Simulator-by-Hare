using UnityEngine;

public class StockObject : MonoBehaviour
{
    public StockInfo stockInfo;
    [Header("Stock Object Dev Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    public Rigidbody _stockRigidbody { get; private set; }
    public bool _isPlaced { get; private set; }
    private Collider _stockCollider;


    void Awake()
    {
        _stockRigidbody = GetComponent<Rigidbody>();
        _stockCollider = GetComponent<Collider>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stockInfo = StockInfoController.Instance.GetStockInfoByName(stockInfo.stockName);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlaced)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition,
                Vector3.zero,
                _moveSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation,
                Quaternion.identity,
                _moveSpeed * Time.deltaTime);
        }
    }

    public void Pickup()
    {
        _stockRigidbody.isKinematic = true;

        _isPlaced = true;
        if (transform.localPosition == Vector3.zero && transform.localRotation == Quaternion.identity)
        {
            _isPlaced = false;
        }
        _stockCollider.enabled = false;
    }
    public void MakePlaced()
    {
        _stockRigidbody.isKinematic = true;

        _isPlaced = true;
        _stockCollider.enabled = false;
    }
    public void Release()
    {
        _stockRigidbody.isKinematic = false;

        _isPlaced = false;
        _stockCollider.enabled = true;
    }

    public void PlaceInBox() 
    {
        _stockRigidbody.isKinematic = true;
        _stockCollider.enabled = false;
    }
}
