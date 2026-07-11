using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("Money Display Setting")]
    [SerializeField] private TMP_Text moneyText;
    [Header("Update Price UI Settings")]
    public GameObject updatePriceUI;
    [SerializeField] private TMP_Text priceText, modifiedPriceText;
    [SerializeField] private TMP_InputField _priceInputField;
    private StockInfo activeStockInfo;
    [Header("Shop Menu UI Settings")]
    public GameObject shopUI;
    [SerializeField] private TMP_Text shopMoneyText;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            if (updatePriceUI.activeSelf)
            {
                UpdateModifiedPrice();
            }
        }

        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            if (!updatePriceUI.activeSelf)
            {
                ShowShopUI();
            }
        }
    }

    public void ShowUpdatePriceUI(StockInfo stockInfo)
    {
        updatePriceUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        priceText.text = "$" + stockInfo.stockPrice.ToString("F2");
        modifiedPriceText.text = "$" + stockInfo.modifiedStockPrice.ToString("F2");

        activeStockInfo = stockInfo;
    }
    public void HideUpdatePriceUI()
    {
        updatePriceUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowShopUI()
    {
        Cursor.lockState = CursorLockMode.None;
        shopUI.SetActive(true);
    }
    public void HideShopUI()
    {
        Cursor.lockState = CursorLockMode.Locked;
        shopUI.SetActive(false);
    }

    public void UpdateModifiedPrice()
    {
        if (string.IsNullOrEmpty(_priceInputField.text))
        {
            return;
        }
        activeStockInfo.modifiedStockPrice = float.Parse(_priceInputField.text);

        modifiedPriceText.text = "$" + activeStockInfo.modifiedStockPrice.ToString("F2");
        StockInfoController.Instance.UpdatePrice(activeStockInfo.stockName, activeStockInfo.modifiedStockPrice);

        _priceInputField.text = "";
        HideUpdatePriceUI();
    }
    public void UpdateMoneyDisplay(float currentMoney)
    {
        moneyText.text = "<color=#00FFFF>Current money:</color>\n" + "<color=green>$</color>" + $"<color=yellow>{currentMoney.ToString("F2")}</color>";
        shopMoneyText.text = "<color=green>$</color>" + $"<color=yellow>{currentMoney.ToString("F2")}</color>";
    }
}
