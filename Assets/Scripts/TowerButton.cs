using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    public int towerCost;
    private Button buttonComponent;

    void Start()
    {
        buttonComponent = GetComponent<Button>();
      
    }

    public void CheckAvailability(int currentGold)
    {
        bool canAfford = currentGold >= towerCost;

        if (buttonComponent != null)
        {
            buttonComponent.interactable = canAfford;
        }
    }
}