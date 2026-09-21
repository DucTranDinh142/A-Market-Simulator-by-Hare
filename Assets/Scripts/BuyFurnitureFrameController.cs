using TMPro;
using UnityEngine;

public class BuyFurnitureFrameController : MonoBehaviour
{
    [SerializeField] private FurnitureController _furniture;
    [SerializeField] private TMP_Text _priceText;

    private void Start()
    {
        _priceText.text = "Price: " + "<color=green>$</color>" + $"<color=yellow>{_furniture.price.ToString("F2")}</color>";
    }
    public void BuyFurniture()
    {
        if (StoreController.Instance.CheckMoneyAvailable(_furniture.price))
        {
            StoreController.Instance.RemoveMoney(_furniture.price);

            Instantiate(_furniture, StoreController.Instance.furnitureSpawnPoint.position, Quaternion.identity);

        }
    }
}
