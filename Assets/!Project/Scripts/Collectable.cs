using System;
using UnityEngine;

public class Collectable : MonoBehaviour, IInteractable
{
    public Transform icon; // Ícone de interação
    public Canvas canvas; // Canvas para exibição do ícone
    public GameObject objectToDestroy; // Objeto que será destruído ao coletar
    public InteractionIcon interactionIcon; // Ícone de interação visual
    public InteractionAreaController interactionArea; // Referência para o controlador da área de interação
    public ItemData item; // Referência ao item a ser coletado
    public int amount = 1; // Quantidade do item (opcional, mas pode ser usado no futuro)
    private bool _isReady = false; // Verifica se o item está pronto para interação

    private void Update()
    {
        // Atualiza a posição do ícone de interação na tela
        if (canvas.gameObject.activeSelf)
        {
            icon.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        }
    }

    public void Interact()
    {
        // Só permite interação se estiver pronto e dentro da área
        if (_isReady && interactionArea.IsPlayerInside()) 
        {
            Debug.Log($"Interagiu com {item.itemName}");
            ConfirmCollect();
        }
    }

    public void Ready()
    {
        _isReady = true; // Define que o objeto está pronto para interação
        interactionIcon.Ready();
    }

    public void NotReady()
    {
        _isReady = false; // Define que o objeto não está pronto para interação
        interactionIcon.NotReady();
    }

    private void ConfirmCollect()
    {
        if (item != null)
        {
            CoreGame.Instance.inventoryManager.AddItem(item);
            Debug.Log($"Item coletado: {item.itemName}");
            CoreGame.Instance.subtitleManager.ShowSubtitle($"Coletou {item.itemName}");
        }

        NotReady(); // Remove a indicação de interação

        // Remove o objeto do jogo após a coleta
        if (objectToDestroy)
        {
            Destroy(objectToDestroy);
        }
    }
    
    /// <summary>
    /// Obtém o objeto raiz do objeto atual (útil para destruir o objeto inteiro)
    /// </summary>
    private static GameObject GetRootParentObject(GameObject obj)
    {
        var currentTransform = obj.transform;
        while (currentTransform.parent != null)
        {
            currentTransform = currentTransform.parent;
        }
        return currentTransform.gameObject;
    }
}
