using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class StockInfoController : MonoBehaviour
{
    [SerializeField] private List<StockInfo> bottleInfos, boxInfos, canInfos, fruitSmallInfos, fruitLargeInfos;

    private List<StockInfo> allStockInfos = new List<StockInfo>();

    public static StockInfoController Instance;

    private void Awake()
    {
        Instance = this;

        allStockInfos.AddRange(bottleInfos);
        allStockInfos.AddRange(boxInfos);
        allStockInfos.AddRange(canInfos);
        allStockInfos.AddRange(fruitSmallInfos);
        allStockInfos.AddRange(fruitLargeInfos);

        for (int i = 0; i < allStockInfos.Count; i++)
        {
            if (allStockInfos[i].modifiedStockPrice == 0)
            {
                allStockInfos[i].modifiedStockPrice = allStockInfos[i].stockPrice;
            }
        }
    }
    public StockInfo GetStockInfoByName(string stockName)
    {
        StockInfo infoToReturn = null;

        for (int i = 0;i < allStockInfos.Count; i++)
        {
            if (allStockInfos[i].stockName == stockName)
            {
                infoToReturn = allStockInfos[i];
                break;
            }
        }
        return infoToReturn;
    }

    public void UpdatePrice(string stockName, float newPrice)
    {
        for (int i = 0; i < allStockInfos.Count; i++)
        {
            if (allStockInfos[i].stockName == stockName)
            {
                allStockInfos[i].modifiedStockPrice = newPrice;
                break;
            }
        }

        List<ShelfSpaceController> shelves = new List<ShelfSpaceController>();
        
        shelves.AddRange(FindObjectsByType<ShelfSpaceController>(FindObjectsSortMode.None));

        foreach(ShelfSpaceController shelf in shelves)
        {
            if (shelf.info.stockName == stockName)
            {
                shelf.UpdatePriceLabel(newPrice);
            }
        }
    }
}
