using UnityEngine;
using UnityEngine.UI;

public class InteractionIcon : MonoBehaviour
{
    [SerializeField] Image icon;
    public Sprite readyIcon;
    public Sprite notReadyIcon;
        
    private void Start()
    {
        icon.sprite = notReadyIcon;
    }

    public void Ready()
    {
        icon.sprite = readyIcon;
    }

    public void NotReady()
    {
        icon.sprite = notReadyIcon;
    }
}
