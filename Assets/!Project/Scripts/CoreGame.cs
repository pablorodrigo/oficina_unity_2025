using UnityEngine;

public class CoreGame : MonoBehaviour
{
    public static CoreGame Instance;

    public GameManager gameManager;
    public InventoryManager inventoryManager;
    public SubtitleManager subtitleManager;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }


        Instance = this;
    }
    
    
}
