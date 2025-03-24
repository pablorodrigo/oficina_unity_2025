using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class SubtitleManager : MonoBehaviour
{
    public GameObject subtitlePanel; // Painel onde a legenda será exibida
    public TMP_Text subtitle; // Texto da legenda

    public float typingSpeed = 0.05f; // Velocidade da animação de digitação
    public float displayTime = 2f; // Tempo que a legenda ficará visível após digitar
    
    private Coroutine currentCoroutine; // Referência para a coroutine ativa

    /// <summary>
    /// Exibe um subtítulo na tela.
    /// </summary>
    /// <param name="text">Texto da legenda a ser exibida.</param>
    public void ShowSubtitle(string text)
    {
        // Limpa o texto e ativa o painel de legenda
        subtitle.text = "";
        subtitlePanel.SetActive(true);

        // Se houver uma coroutine ativa, interrompe antes de iniciar uma nova
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // Inicia a animação da legenda com o novo texto
        currentCoroutine = StartCoroutine(ShowCaptionLetter(text));
    }

    /// <summary>
    /// Anima a exibição do subtítulo, mostrando letra por letra.
    /// </summary>
    /// <param name="subtitle">Texto da legenda a ser exibida.</param>
    private IEnumerator ShowCaptionLetter(string subtitle)
    {
        this.subtitle.maxVisibleCharacters = 0; // Começa ocultando todas as letras
        this.subtitle.text = subtitle; // Define o texto completo da legenda
        
        // Anima a digitação letra por letra
        while (this.subtitle.maxVisibleCharacters < subtitle.Length)
        {
            yield return new WaitForSeconds(typingSpeed);
            this.subtitle.maxVisibleCharacters += 1;
        }

        // Aguarda o tempo de exibição antes de ocultar a legenda
        yield return new WaitForSeconds(displayTime);
        subtitlePanel.SetActive(false);
        currentCoroutine = null; // Reseta a referência da coroutine ativa
    }
}