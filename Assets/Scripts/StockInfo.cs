using UnityEngine;

[System.Serializable]
public class StockInfo
{
    public string stockName;
    public StockType stockType;
    public float stockPrice, modifiedStockPrice;

    public StockObject stockObject;
}