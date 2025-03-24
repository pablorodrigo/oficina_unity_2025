using System;
using UnityEngine;

public class CandleInteractable : MonoBehaviour, IInteractable
{
    public Transform icon; // Ícone de interação
    public Canvas canvas; // Canvas para exibição do ícone
    public InteractionIcon interactionIcon; // Ícone de interação visual
    public InteractionAreaController interactionArea; // Controlador da área de interação
    public GameObject candleLight; // Luz da vela
    public int candleID; // ID único da vela

    private bool _isReady = false; // Se a interação está pronta
    private bool _isLit = false; // Se a vela já está acesa

    private void Start()
    {
        // Ativa o listener para interação quando o objeto fica ativo
        //InputManager.Instance.Controls.Player.Interact.started += OnInteractButton;
        candleLight.SetActive(false); // Começa apagada
    }

    private void OnDisable()
    {
        // Remove o listener para evitar chamadas duplicadas
        //InputManager.Instance.Controls.Player.Interact.started -= OnInteractButton;
    }

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
        
        if (CandlePuzzleManager.Instance.IsPuzzleCompleted())
        {
            Debug.Log("O puzzle já foi resolvido! Você não pode mais interagir.");
            return;
        }
        
        if (_isReady && interactionArea.IsPlayerInside()) // Só permite interação se estiver pronto
        {
            if (!CandlePuzzleManager.Instance.HasRequiredItem())
            {
                Debug.Log("Você precisa de " + CandlePuzzleManager.Instance.requiredItem.itemName + " para interagir!");
                CoreGame.Instance.subtitleManager.ShowSubtitle("Eu preciso encontrar um fosforo");
                return;
            }

            if (_isLit)
            {
                ApagarVela();
            }
            else
            {
                AcenderVela();
            }
        }
    }
    
    private void AcenderVela()
    {
        _isLit = true;
        candleLight.SetActive(true);
        CandlePuzzleManager.Instance.AddCandle(this);
    }

    private void ApagarVela()
    {
        _isLit = false;
        candleLight.SetActive(false);
        CandlePuzzleManager.Instance.RemoveCandle(this);
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
    
    public void ForceReset()
    {
        _isLit = false;
        candleLight.SetActive(false);
    }
    
    
    public bool IsLit()
    {
        return _isLit;
    }

    /*private void OnInteractButton(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // Apenas acende a vela se estiver pronto para interagir
        Interact();
    }*/
}
