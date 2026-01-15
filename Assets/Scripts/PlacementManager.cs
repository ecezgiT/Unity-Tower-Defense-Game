using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance { get; private set; }

    [Header("Tower Prefabs")]
    public GameObject ArrowTowerPrefab;
    public GameObject CannonTowerPrefab;
    public GameObject FrostTowerPrefab;

    private GameObject selectedTowerPrefab;
    private int selectedTowerCost;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void SelectArrowTower()
    {
        SelectTower(ArrowTowerPrefab, 50);
    }
    public void SelectCannonTower()
    {
        SelectTower(CannonTowerPrefab, 75);
    }
    public void SelectFrostTower()
    {
        SelectTower(FrostTowerPrefab, 70);
    }
    public void SelectTower(GameObject towerPrefab, int cost)
    {
        selectedTowerPrefab = towerPrefab;
        selectedTowerCost = cost;
        LogManager.Instance.WriteLog($"Kullanıcı, {towerPrefab.GetComponent<TowerBase>().towerName} inşa etmek için seçti (Maliyet: {cost}).");
    }
    public void PlaceTower(TowerSpot spot)
    {
        if (selectedTowerPrefab == null)
        {
            LogManager.Instance.WriteLog("Kule inşa edilemedi: Lütfen önce bir kule tipi seçin.");
            return;
        }

        if (GameManager.Instance.GetCurrentGold() < selectedTowerCost)
        {
            LogManager.Instance.WriteLog($"Kule inşa edilemedi: Yetersiz Altın. Gereken: {selectedTowerCost}, Mevcut: {GameManager.Instance.GetCurrentGold()}.");
            return;
        }

        spot.PlaceTower(selectedTowerPrefab);

        GameManager.Instance.SubtractGold(selectedTowerCost);

        if (LogManager.Instance != null)
        {
            LogManager.Instance.WriteLog($"Kullanıcı, ({spot.transform.position.x:F0}, {spot.transform.position.y:F0}) konumuna '{selectedTowerPrefab.GetComponent<TowerBase>().towerName}' inşa etti. Kalan Para: {GameManager.Instance.GetCurrentGold()}.");
        }

        selectedTowerPrefab = null;
    }
}
