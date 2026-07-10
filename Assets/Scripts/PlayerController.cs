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
    [Header("Interaction Settings")]
    [SerializeField] private float _interactDistance;
    [SerializeField] private Transform _holdPoint;
    [SerializeField] private float _throwForce;
    private StockObject _heldItem;

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
        MovementActionHandle();

        Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit visualHit, actualHit;
        visualHit = InteractableRayCastHit(ray);

        if (_heldItem == null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (Physics.Raycast(ray, out actualHit, _interactDistance, _whatIsStock))
                {
                    _heldItem = actualHit.collider.GetComponent<StockObject>();
                    _heldItem.transform.SetParent(_holdPoint);
                    _heldItem.Pickup();
                }
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (Physics.Raycast(ray, out actualHit, _interactDistance, _whatIsShelf))
                {
                    _heldItem = actualHit.collider.GetComponent<ShelfSpaceController>().GetStock();
                    if (_heldItem != null)
                    {
                        _heldItem.transform.SetParent(_holdPoint);
                        _heldItem.Pickup();
                    }
                }
            }
        }
        else
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
        if (_heldItem == null)
        {
            if (Physics.Raycast(ray, out hit, _interactDistance, _whatIsStock))
            {
                _crosshair.color = Color.green;
                _crosshair.transform.localScale = Vector3.one * 0.6f;
            }
            else
            {
                _crosshair.color = Color.white;
                _crosshair.transform.localScale = Vector3.one * 0.4f;
            }
        }
        else
        {
            if (Physics.Raycast(ray, out hit, _interactDistance, _whatIsShelf))
            {
                _crosshair.color = Color.green;
                _crosshair.transform.localScale = Vector3.one * 0.6f;
            }
            else
            {
                _crosshair.color = Color.white;
                _crosshair.transform.localScale = Vector3.one * 0.4f;
            }
        }

        return hit;
    }
}
