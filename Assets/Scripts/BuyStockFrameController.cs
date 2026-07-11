using System.Diagnostics;
using TMPro;
using UnityEngine;

public class BuyStockFrameController : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText, priceText, amountInBoxText, boxPriceText, buttonText;
    [SerializeField] private StockInfo info;
    [SerializeField] private StockBoxController boxToSpawn;
    private float boxPrice;
    private void Start()
    {
        UpdateFrameInfo();
    }
    public void UpdateFrameInfo()
    {
        info = StockInfoController.Instance.GetStockInfoByName(info.stockName);

        nameText.text = $"<color=#00FFFF>{info.stockName}</color>";
        priceText.text = "<color=green>$</color>" + $"<color=yellow>{info.stockPrice.ToString("F2")}</color>"+" each";

        int boxAmount = boxToSpawn.GetStockAmountInBox(info.stockType);
        amountInBoxText.text = $"<color=#FF00C9>{boxAmount.ToString()}</color>"+" per box";

        boxPrice = boxAmount * info.stockPrice;
        boxPriceText.text = "Box: "+ "<color=green>$</color>" + $"<color=yellow>{boxPrice.ToString("F2")}</color>";
        buttonText.text = "<color=red>PAY: </color>"+ $"<color=yellow>{boxPrice.ToString("F2")}</color>";
    }

    public void BuyBox()
    {
        if (StoreController.Instance.CheckMoneyAvailable(boxPrice))
        {
            StoreController.Instance.RemoveMoney(boxPrice);

            Instantiate(boxToSpawn, StoreController.Instance.stockSpawnPoint.position,Quaternion.identity).SetupBox(info);
        }
    }
}
