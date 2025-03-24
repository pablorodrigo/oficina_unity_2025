using UnityEngine;
using UnityEngine;

public class InteractionAreaController : MonoBehaviour
{
    public GameObject interactionAreaCanvas; // Canvas de interação

    private bool _isPlayerInside = false;

    private void Start()
    {
        interactionAreaCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = true;
            interactionAreaCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
            interactionAreaCanvas.SetActive(false);
        }
    }

    public bool IsPlayerInside()
    {
        return _isPlayerInside;
    }
}