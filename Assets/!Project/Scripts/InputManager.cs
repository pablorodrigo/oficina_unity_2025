using UnityEngine;
using UnityEngine.InputSystem;

// Classe responsável por gerenciar os inputs do jogador
public class InputManager : MonoBehaviour
{
    // Instância única do InputManager (Singleton)
    public static InputManager Instance;

    // Referência para o conjunto de controles definidos no Input System
    public InputSystemActions Controls;

    // Ação de movimento capturada do Input System
    InputAction _axis;

    // Método chamado quando o script é inicializado
    void Awake()
    {
        // Garante que apenas uma instância do InputManager exista na cena
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroi essa instância duplicada
            return;
        }
        Instance = this; // Define a instância atual como a única válida

        // Inicializa os controles
        Controls = new InputSystemActions();

        // Captura a ação de movimento definida nos controles
        _axis = Controls.Player.Move;

        // Ativa os controles para começar a receber input
        Controls.Enable();
    }

    // Ativa os controles quando o objeto é habilitado (por exemplo, ativado na cena)
    private void OnEnable()
    {
        Controls.Enable();
    }

    // Desativa os controles quando o objeto é desabilitado
    private void OnDisable()
    {
        Controls.Disable();
    }

    // Garante que os controles sejam desativados corretamente ao destruir o objeto
    private void OnDestroy()
    {
        Controls.Disable();
    }

    // Retorna o valor atual do input de movimento como um Vector3
    // Esse vetor pode ser usado, por exemplo, para mover o personagem
    public Vector3 GetAxis()
    {
        // Converte o Vector2 do input (X e Y do teclado ou joystick) para um Vector3 no plano XZ
        return new Vector3(_axis.ReadValue<Vector2>().x, 0, _axis.ReadValue<Vector2>().y);
    }
}