using System;
using StarterAssets;
using UnityEngine;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    public ItemData requiredItemId; // ID da chave necessária para abrir a porta
    public GameObject interactionTrigger; // Área de interação da porta
    public InteractionIcon interactionIcon; // Ícone de interação
    public Canvas canvas; // Canvas para exibição do ícone
    public Transform icon; // Ícone de interação

    public GameObject winUI; // UI de vitória
    
    private void Update()
    {
        // Atualiza a posição do ícone de interação na tela
        if (canvas.gameObject.activeSelf)
        {
            icon.position = Camera.main.WorldToScreenPoint(transform.position);
        }
    }

    public void Interact()
    {
        var inventory = CoreGame.Instance.inventoryManager;

        // Verifica se o jogador tem a chave
        if (inventory.HasItem(requiredItemId.id))
        {
            UnlockDoor();
        }
        else
        {
            // Mensagem de erro caso a chave não esteja no inventário
            CoreGame.Instance.subtitleManager.ShowSubtitle("Não é possível abrir. Falta algo.");
        }
    }

    private void UnlockDoor()
    {
        // Remove a chave do inventário
        CoreGame.Instance.inventoryManager.RemoveItem(requiredItemId);

        // Ativa a UI de vitória
        if (winUI != null)
        {
            winUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
            CoreGame.Instance.gameManager.crosshair.SetActive(false);
            CoreGame.Instance.gameManager.endGame = true;
        }
    }

    public void Ready()
    {
        interactionIcon.Ready();
    }

    public void NotReady()
    {
        interactionIcon.NotReady();
    }
    
    private void OnInteractButton(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Interact();
    }
}