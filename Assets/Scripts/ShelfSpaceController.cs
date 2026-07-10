using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShelfSpaceController : MonoBehaviour
{
    public StockInfo info;
    public List<StockObject> objectsOnShelf;
    [Header("Shelf Space Position List")]
    [SerializeField] private List<Transform> fruitSmallPositions;
    [SerializeField] private List<Transform> bottlePositions;
    [SerializeField] private List<Transform> boxPositions;
    [SerializeField] private List<Transform> fruitLargePositions;
    [SerializeField] private List<Transform> canPositions;
    [Space]
    [Header("Shelf Space Price Label Settings")]
    [SerializeField] private TMP_Text priceLabel;

    public void PlaceStock(StockObject objectToPlace)
    {
        bool preventPlacement = true;

        if (objectsOnShelf.Count == 0)
        {
            info = objectToPlace.stockInfo;
            preventPlacement = false;
        }
        else if (info.stockName == objectToPlace.stockInfo.stockName)
        {
            preventPlacement = false;

            switch (info.stockType)
            {
                case StockType.FruitSmall:
                    if (objectsOnShelf.Count >= fruitSmallPositions.Count)
                    {
                        preventPlacement = true;
                    }
                    break;
                case StockType.Bottle:
                    if (objectsOnShelf.Count >= bottlePositions.Count)
                    {
                        preventPlacement = true;
                    }
                    break;
                case StockType.Box:
                    if (objectsOnShelf.Count >= boxPositions.Count)
                    {
                        preventPlacement = true;
                    }
                    break;
                case StockType.FruitLarge:
                    if (objectsOnShelf.Count >= fruitLargePositions.Count)
                    {
                        preventPlacement = true;
                    }
                    break;
                case StockType.Can:
                    if (objectsOnShelf.Count >= canPositions.Count)
                    {
                        preventPlacement = true;
                    }
                    break;
            }
        }

        if (!preventPlacement)
        {
            objectToPlace.transform.SetParent(transform);
            objectToPlace.MakePlaced();

            switch (info.stockType)
            {
                case StockType.FruitSmall:
                    objectToPlace.transform.SetParent(fruitSmallPositions[objectsOnShelf.Count]);
                    break;
                case StockType.Bottle:
                    objectToPlace.transform.SetParent(bottlePositions[objectsOnShelf.Count]);
                    break;
                case StockType.Box:
                    objectToPlace.transform.SetParent(boxPositions[objectsOnShelf.Count]);
                    break;
                case StockType.FruitLarge:
                    objectToPlace.transform.SetParent(fruitLargePositions[objectsOnShelf.Count]);
                    break;
                case StockType.Can:
                    objectToPlace.transform.SetParent(canPositions[objectsOnShelf.Count]);
                    break;
            }
            objectsOnShelf.Add(objectToPlace);

            priceLabel.color = Color.black;
            priceLabel.text = objectsOnShelf[0].stockInfo.stockName + "\n$" + objectsOnShelf[0].stockInfo.stockPrice;
        }

    }
    public StockObject GetStock()
    {
        StockObject stockObjectToReturn = null;

        if (objectsOnShelf.Count > 0)
        {
            stockObjectToReturn = objectsOnShelf[objectsOnShelf.Count - 1];
            objectsOnShelf.RemoveAt(objectsOnShelf.Count - 1);
        }

        if (objectsOnShelf.Count == 0)
        {
            priceLabel.color = Color.red;
            priceLabel.text = "Sold Out";
        }

        return stockObjectToReturn;
    }
}
