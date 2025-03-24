using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PrimeiraPessoaController : MonoBehaviour
    {
           // Referência ao CharacterController para movimentação
    public CharacterController controller;

    // Referência à câmera para controle da visão
    public Transform playerCamera;

    [Header("Movimentação")]
    public float walkSpeed = 4f;   // Velocidade ao caminhar
    public float runSpeed = 8f;    // Velocidade ao correr
    public float crouchSpeed = 2f; // Velocidade ao agachar
    public float gravity = -9.81f; // Intensidade da gravidade aplicada ao jogador

    [Header("Pulo")]
    public float jumpHeight = 1.5f; // Altura do pulo
    private Vector3 velocity;       // Armazena a velocidade do jogador
    private bool isGrounded;        // Verifica se o jogador está no chão

    [Header("Agachamento")]
    public float crouchHeight = 0.5f; // Altura do CharacterController ao agachar
    private float originalHeight;      // Altura original do CharacterController
    private bool isCrouching = false;  // Verifica se o jogador está agachado

    [Header("Sensibilidade do Mouse")]
    public float mouseSensitivity = 2f; // Sensibilidade da câmera ao mover o mouse
    private float xRotation = 0f;       // Ângulo de rotação da câmera

    [Header("Sons de Passos")]
    public AudioSource footstepAudioSource; // Fonte de áudio para os passos
    public AudioClip[] walkSounds; // Lista de sons ao caminhar
    public AudioClip[] runSounds;  // Lista de sons ao correr
    public float walkStepInterval = 0.5f; // Tempo entre passos ao caminhar
    public float runStepInterval = 0.3f;  // Tempo entre passos ao correr
    private float stepTimer = 0f;         // Contador de tempo para os passos

    void Start()
    {
        // Trava o cursor no centro da tela para evitar que saia da janela
        Cursor.lockState = CursorLockMode.Locked;

        // Guarda a altura original do CharacterController para referência ao agachar
        originalHeight = controller.height;
    }

    void Update()
    {
        HandleMovement();   // Controla a movimentação do jogador
        HandleJumping();    // Controla o pulo do jogador
        HandleCrouching();  // Controla o agachamento
        HandleMouseLook();  // Controla a movimentação da câmera (mira)
        HandleFootsteps();  // Reproduz sons de passos conforme o jogador se move
    }

    /// <summary>
    /// Controla a movimentação do jogador com WASD e Shift para correr.
    /// </summary>
    void HandleMovement()
    {
        // Verifica se o jogador está tocando o chão
        isGrounded = controller.isGrounded;

        // Captura entrada de movimento nos eixos X (horizontal) e Z (vertical)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Verifica se o jogador está correndo (segurando Shift e não agachado)
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && !isCrouching;

        // Define a velocidade com base no estado do jogador (andando, correndo ou agachado)
        float speed = isRunning ? runSpeed : (isCrouching ? crouchSpeed : walkSpeed);

        // Calcula o movimento final
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Move o jogador usando o CharacterController
        controller.Move(move * speed * Time.deltaTime);

        // Aplicando gravidade ao jogador
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Mantém o jogador no chão, evitando que continue caindo infinitamente
        }

        velocity.y += gravity * Time.deltaTime; // Aplica a gravidade continuamente
        controller.Move(velocity * Time.deltaTime); // Move o jogador com a gravidade aplicada
    }

    /// <summary>
    /// Permite que o jogador pule ao pressionar a barra de espaço, caso esteja no chão.
    /// </summary>
    void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Aplica a força do pulo com base na equação da gravidade
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    /// <summary>
    /// Controla o agachamento ao pressionar a tecla Ctrl.
    /// </summary>
    void HandleCrouching()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // Alterna entre agachado e em pé
            isCrouching = !isCrouching;

            // Ajusta a altura do CharacterController
            controller.height = isCrouching ? crouchHeight : originalHeight;
        }
    }

    /// <summary>
    /// Controla a rotação da câmera para simular a mira do jogador.
    /// </summary>
    void HandleMouseLook()
    {
        // Captura o movimento do mouse nos eixos X e Y
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Controla a rotação da câmera no eixo vertical (limitando entre -90° e 90°)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Aplica a rotação à câmera
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotaciona o corpo do jogador horizontalmente
        transform.Rotate(Vector3.up * mouseX);
    }

    /// <summary>
    /// Controla a reprodução dos sons de passos ao andar ou correr.
    /// </summary>
    void HandleFootsteps()
    {
        // Se o jogador não estiver no chão ou estiver parado, não toca som
        if (!isGrounded || controller.velocity.magnitude < 0.1f) return;

        // Verifica se o jogador está correndo ou andando
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && !isCrouching;
        float stepInterval = isRunning ? runStepInterval : walkStepInterval;

        // Atualiza o temporizador dos passos
        stepTimer += Time.deltaTime;
        if (stepTimer >= stepInterval)
        {
            stepTimer = 0f; // Reseta o temporizador
            PlayFootstepSound(isRunning); // Toca um som de passo
        }
    }

    /// <summary>
    /// Toca um som de passo aleatório ao andar ou correr.
    /// </summary>
    void PlayFootstepSound(bool isRunning)
    {
        // Seleciona a lista correta de sons (andar ou correr)
        AudioClip[] sounds = isRunning ? runSounds : walkSounds;

        // Se houver sons disponíveis, toca um aleatoriamente
        if (sounds.Length > 0)
        {
            footstepAudioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
        }
    }
        
    }
}
