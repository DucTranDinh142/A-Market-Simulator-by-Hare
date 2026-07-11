using UnityEngine;
using UnityEngine.InputSystem;

public class StoreController : MonoBehaviour
{
    public static StoreController Instance;

    public float currentMoney = 1000f; // Example starting money
    public Transform stockSpawnPoint; // Point where the stock will be spawned

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UIController.Instance.UpdateMoneyDisplay(currentMoney);
    }
    private void Update()
    {
        if (Keyboard.current.numpadPlusKey.wasPressedThisFrame) AddMoney(10000);
        if (Keyboard.current.numpadMinusKey.wasPressedThisFrame) RemoveMoney(25000);
    }

    public void AddMoney(float amountToAdd)
    {
        currentMoney += amountToAdd;
        UIController.Instance.UpdateMoneyDisplay(currentMoney);
    }

    public void RemoveMoney(float amountToRemove)
    {
        if (!CheckMoneyAvailable(amountToRemove)) return; 
        currentMoney -= amountToRemove;
        UIController.Instance.UpdateMoneyDisplay(currentMoney);
    }
    public bool CheckMoneyAvailable(float amountToCheck)
    {
        bool hasEnough = false;
        if (currentMoney >= amountToCheck)
        {
            hasEnough = true;
        }
        return hasEnough;
    }
}
