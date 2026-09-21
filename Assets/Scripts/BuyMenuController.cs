using UnityEngine;
using UnityEngine.UI;

public class BuyMenuController : MonoBehaviour
{
    public GameObject stocksPanel, furnituresPanel;
    public Button stockButton, furnitureButton;

    private void Start()
    {
        stockButton.interactable = false;
        furnitureButton.interactable = true;
    }
    public void OpenStocksPanel()
    {
        stocksPanel.SetActive(true);
        stockButton.interactable = false;

        furnituresPanel.SetActive(false);
        furnitureButton.interactable = true;
    }
    public void OpenFurnituresPanel()
    {
        stocksPanel.SetActive(false);
        stockButton.interactable = true;

        furnituresPanel.SetActive(true);
        furnitureButton.interactable = false;
    }
}
