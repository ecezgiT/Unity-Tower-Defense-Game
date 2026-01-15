using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    private bool isOccupied = false; 

    private void OnMouseDown()
    {
        if (!isOccupied)
        {
            PlacementManager.Instance.PlaceTower(this);
        }
        else
        {
            Debug.Log(gameObject.name + " dolu. Yükseltme menüsü açılacak.");

            return;
        }
    }

    public void PlaceTower(GameObject towerPrefab)
    {
        Instantiate(towerPrefab, transform.position, Quaternion.identity, null); 
        isOccupied = true;
    }
}