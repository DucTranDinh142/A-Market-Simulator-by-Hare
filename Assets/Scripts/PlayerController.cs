using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Input Action References")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference lookAction;

    private CharacterController characterController;
    [Space]
    [Header("Camera Setup")]
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _minVerticalLookAngle = -80f;
    [SerializeField] private float _maxVerticalLookAngle = 75f;
    [Header("Crosshair")]
    [SerializeField] private Image _crosshair;
    [Space]
    [Header("Player Movement Settings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _lookSpeed;

    private float _horizontalRotation, _verticalRotation;
    private float _ySpeed;

    [Space]
    [Header("Layer Mask Settings")]
    [SerializeField] private LayerMask _whatIsStock;
    [SerializeField] private LayerMask _whatIsShelf;
    [SerializeField] private LayerMask _whatIsBox;
    [SerializeField] private LayerMask _whatIsBin;
    [SerializeField] private LayerMask _whatIsFurniture;
    [Header("Interaction Settings")]
    [SerializeField] private float _interactDistance;
    [SerializeField] private Transform _stockHoldPoint;
    [SerializeField] private Transform _boxHoldPoint;
    [SerializeField] private Transform _furnitureHoldPoint;
    [SerializeField] private float _throwForce;
    [SerializeField] private float _fastRestockHoldingTime;
    private float _placeStockTimer;
    private FurnitureController _heldFurniture;
    private StockObject _heldItem;
    private StockBoxController _heldBox;


    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (UIController.Instance.updatePriceUI != null || UIController.Instance.shopUI != null)
        {
            if (UIController.Instance.updatePriceUI.activeSelf || UIController.Instance.shopUI.activeSelf)
            {
                return;
            }
        }

        MovementActionHandle();

        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit visualHit, actualHit;
        visualHit = InteractableRayCastHit(ray);

        if (_heldItem == null && _heldBox == null && _heldFurniture == null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (Physics.Raycast(ray, out actualHit, _interactDistance, _whatIsStock))
                {
                    _heldItem = actualHit.collider.GetComponent<StockObject>();
                    _heldItem.transform.SetParent(_stockHoldPoint);
                    _heldItem.Pickup();
                    return;
                }
                if (Physics.Raycast(ray, out actualHit, _interactDistance, _whatIsBox))
                {
                    _heldBox = actualHit.collider.GetComponent<StockBoxController>();
                    _heldBox.transform.SetParent(_boxHoldPoint);
                    _heldBox.Pickup();
                    if (!_heldBox.opened)
                    {
                        _heldBox.OpenClose();
                    }
                    return;
                }
                if (Physics.Raycast(ray, out actualHit, _interactDistance, _whatIsShelf))
                {
                    actualHit.collider.GetComponent<ShelfSpaceController>().StartPriceUpdate();
                    return;
                }
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (Physics.Raycast(ray, out actualHit, _interactDistance, _whatIsShelf))
                {
                    _heldItem = actualHit.collider.GetComponent<ShelfSpaceController>().GetStock();
                    if (_heldItem != null)
                    {
                        _heldItem.transform.SetParent(_stockHoldPoint);
                        _heldItem.Pickup();
                    }
                }
            }

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (Physics.Raycast(ray, out actualHit, _interactDistance, _whatIsBox))
                {
                    actualHit.collider.GetComponent<StockBoxController>().OpenClose();
                    return;
                }
                if (Physics.Raycast(ray, out actualHit, _interactDistance, _whatIsFurniture))
                {
                    _heldFurniture = actualHit.transform.GetComponent<FurnitureController>();

                    _heldFurniture.transform.SetParent(_furnitureHoldPoint);
                    _heldFurniture.transform.localPosition = Vector3.zero;
                    _heldFurniture.transform.localRotation = Quaternion.identity;

                    _heldFurniture.MakePlaceable();
                }
            }

        }
        else if (_heldItem != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (Physics.Raycast(ray, out actualHit, _interactDistance, _whatIsShelf))
                {
                    actualHit.collider.GetComponent<ShelfSpaceController>().PlaceStock(_heldItem);
                    if (_heldItem._isPlaced)
                    {
                        _heldItem = null;
                    }
                }
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                _heldItem.Release();
                _heldItem._stockRigidbody.AddForce(_playerCamera.transform.forward * _throwForce, ForceMode.Impulse);
                _heldItem.transform.SetParent(null);
                _heldItem = null;
            }
        }
        else if (_heldBox != null)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                _heldBox.OpenClose();
                return;
            }
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (Physics.Raycast(ray, out actualHit, _interactDistance, _whatIsShelf))
                {
                    if (_heldBox.opened)
                    {
                        _heldBox.PlaceStockOnShelf(actualHit.collider.GetComponent<ShelfSpaceController>());
                        _placeStockTimer = _fastRestockHoldingTime;
                    }
                return;
                }
                if (Physics.Raycast(ray, out actualHit, _interactDistance, _whatIsBin))
                {
                    if (_heldBox.stocksInBox.Count <= 0)
                    {
                        Destroy(_heldBox.gameObject);
                    }
                }
            }
            if (Mouse.current.leftButton.isPressed)
            {
                _placeStockTimer -= Time.deltaTime;
                if (_placeStockTimer <= 0f)
                {
                    if (Physics.Raycast(ray, out actualHit, _interactDistance, _whatIsShelf))
                    {
                        if (_heldBox.opened)
                        {
                            _heldBox.PlaceStockOnShelf(actualHit.collider.GetComponent<ShelfSpaceController>());
                            _placeStockTimer = _fastRestockHoldingTime;
                        }
                    }
                }
            }
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                _heldBox.Release();
                _heldBox._boxRigidbody.AddForce(_playerCamera.transform.forward * _throwForce, ForceMode.Impulse);
                _heldBox.transform.SetParent(null);
                _heldBox = null;
                return;
            }
        }
        else if(_heldFurniture != null)
        {
            _heldFurniture.transform.position = new Vector3 (_furnitureHoldPoint.position.x,0f,_furnitureHoldPoint.position.z);
            _heldFurniture.transform.LookAt(new Vector3(transform.position.x,0f,transform.position.z));

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                _heldFurniture.transform.SetParent(null);
                _heldFurniture.PlaceFurniture();
                _heldFurniture = null;

            }
        }
    }

    private void MovementActionHandle()
    {
        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();

        _horizontalRotation += lookInput.x * _lookSpeed * Time.deltaTime;
        _verticalRotation -= lookInput.y * _lookSpeed * Time.deltaTime;
        _verticalRotation = Mathf.Clamp(_verticalRotation, _minVerticalLookAngle, _maxVerticalLookAngle);

        transform.rotation = Quaternion.Euler(0f, _horizontalRotation, 0f);
        _playerCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0f, 0f);

        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        //transform.position += new Vector3(moveInput.x, 0f, moveInput.y) * Time.deltaTime * moveSpeed;

        Vector3 verticalMovement = transform.forward * moveInput.y;
        Vector3 horizontalMovement = transform.right * moveInput.x;

        Vector3 moveAmount = verticalMovement + horizontalMovement;
        moveAmount = moveAmount.normalized;
        moveAmount *= _moveSpeed;

        if (characterController.isGrounded)
        {
            _ySpeed = 0f;

            if (jumpAction.action.WasPressedThisFrame())
                _ySpeed = _jumpForce;
        }
        _ySpeed += Physics.gravity.y * Time.deltaTime;

        moveAmount.y = _ySpeed;

        characterController.Move(moveAmount * Time.deltaTime);
    }

    private RaycastHit InteractableRayCastHit(Ray ray)
    {
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, _interactDistance, _whatIsStock | _whatIsShelf | _whatIsBox | _whatIsBin))
        {
            _crosshair.color = Color.green;
            _crosshair.transform.localScale = Vector3.one * 0.6f;
        }
        else
        {
            _crosshair.color = Color.white;
            _crosshair.transform.localScale = Vector3.one * 0.4f;
        }
        return hit;
    }
}
