using System.Collections.Generic;
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
}
