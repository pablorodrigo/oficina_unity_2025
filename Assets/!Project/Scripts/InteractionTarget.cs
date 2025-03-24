using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionTarget : MonoBehaviour
{
    // Define quais camadas podem ser interagidas
    public LayerMask interactionLayer;

    // Referência para a câmera principal
    public GameObject _mainCamera;

    // Referência para o objeto interagível atual
    IInteractable _interactable;

    // Declaração do raio para detectar objetos interagíveis
    private Ray _ray;
    private RaycastHit _hitInfo;

    // Inicializa o script
    private void Start()
    {
        // Obtém a referência da câmera principal caso não tenha sido atribuída manualmente
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        // Adiciona o evento para capturar o botão de interação do jogador
        InputManager.Instance.Controls.Player.Interact.started += OnInteractionButton;
    }
    
    private void OnDisable()
    {
        // Remove o listener para evitar chamadas duplicadas
        InputManager.Instance.Controls.Player.Interact.started -= OnInteractionButton;
    }

    // Atualiza a posição do InteractionTarget e verifica interações
    private void LateUpdate()
    {
        var mainCameraTransform = _mainCamera.transform;
        
        // Define a origem do raio na posição da câmera e sua direção para onde a câmera está olhando
        _ray.origin = mainCameraTransform.position;
        _ray.direction = mainCameraTransform.forward;

        // Exibe um raio vermelho no editor para debug, ajudando a visualizar a direção da interação
        Debug.DrawRay(_ray.origin, _ray.direction * 100f, Color.red);

        // Lança um raio para verificar se há um objeto interagível na frente do jogador
        if (Physics.Raycast(_ray, out _hitInfo, 100, interactionLayer))
        {
            // Move o InteractionTarget para o ponto onde o raio colidiu
            transform.position = _hitInfo.point;

            // Busca o componente interagível no próprio objeto ou em seus parentes/filhos
            _interactable = _hitInfo.collider.GetComponentInParent<IInteractable>();
            if (_interactable == null)
            {
                _interactable = _hitInfo.collider.GetComponentInChildren<IInteractable>();
            }

            // Se o objeto for interagível, chama o método para prepará-lo para interação
            if (_interactable != null)
            {
                _interactable.Ready();
            }
            else
            {
                CancelInteraction();
            }
        }
        else
        {
            transform.position = mainCameraTransform.position + mainCameraTransform.forward * 2f;
            CancelInteraction();
        }
    }

    // Método chamado quando o jogador pressiona o botão de interação
    private void OnInteractionButton(InputAction.CallbackContext value)
    {
        // Se não houver um objeto interagível, sai da função
        if (_interactable == null)
        {
            return;
        }

        // Se o botão de interação foi pressionado, chama o método de interação do objeto
        if (value.started)
        {
            _interactable.Interact();
            _interactable = null; // Limpa a referência após a interação
        }
    }
    
    private void CancelInteraction()
    {
        // Se nada for detectado, posiciona o InteractionTarget um pouco à frente da câmera
        // Cancela a interação caso nenhum objeto esteja na frente
        transform.position = _mainCamera.transform.position + _mainCamera.transform.forward * 2f;
        _interactable?.NotReady();
        _interactable = null;
    }
}
