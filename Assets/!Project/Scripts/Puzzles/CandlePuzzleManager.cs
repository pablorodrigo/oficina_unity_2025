using System.Collections.Generic;
using UnityEngine;

// Gerencia o puzzle das velas, onde o jogador deve acender as velas em uma ordem específica
public class CandlePuzzleManager : MonoBehaviour
{
    // Item necessário para interagir com as velas (ex: fósforo, isqueiro)
    public ItemData requiredItem;

    // Sequência correta de IDs das velas que devem ser acesas
    public List<int> correctSequence = new List<int> { 1, 2, 3 };

    // Sequência atual de velas acesas pelo jogador
    private List<int> playerSequence = new List<int>();

    // Lista de velas atualmente acesas pelo jogador
    private List<CandleInteractable> litCandles = new List<CandleInteractable>();

    // Objeto (ex: chave) que será ativado após o puzzle ser resolvido
    public GameObject keyObject;

    // Lista de todas as velas que fazem parte do puzzle (atribuídas via Inspector)
    public List<CandleInteractable> candles;

    // Instância singleton para fácil acesso em outros scripts
    public static CandlePuzzleManager Instance;

    // Indica se o puzzle já foi resolvido
    private bool puzzleCompleted = false;

    // Inicializa a instância e desativa o objeto-chave
    private void Awake()
    {
        Instance = this;
        keyObject.SetActive(false);
    }

    // Garante que o puzzle comece em estado reiniciado (nenhuma vela acesa)
    private void Start()
    {
        ResetPuzzle();
    }

    // Verifica se o jogador possui o item necessário no inventário
    public bool HasRequiredItem()
    {
        return CoreGame.Instance.inventoryManager.HasItem(requiredItem.id);
    }

    // Adiciona uma vela à sequência acesa pelo jogador
    public void AddCandle(CandleInteractable candle)
    {
        // Se o puzzle já foi resolvido, não permite mais interações
        if (puzzleCompleted) return;

        // Só adiciona se ainda não estiver acesa
        if (!litCandles.Contains(candle))
        {
            litCandles.Add(candle);
            playerSequence.Add(candle.candleID);
        }

        // Se o número de velas acesas for igual ao da sequência correta, validar o resultado
        if (litCandles.Count == correctSequence.Count)
        {
            ValidateSequence();
        }
    }

    // Remove uma vela da lista de acesas (caso o jogador apague manualmente, por exemplo)
    public void RemoveCandle(CandleInteractable candle)
    {
        if (puzzleCompleted) return;

        if (litCandles.Contains(candle))
        {
            litCandles.Remove(candle);
            playerSequence.Remove(candle.candleID);
        }
    }

    // Verifica se a sequência feita pelo jogador é a mesma da sequência correta
    private void ValidateSequence()
    {
        // Se o número de velas não for igual, reseta o puzzle
        if (playerSequence.Count != correctSequence.Count)
        {
            ResetPuzzle();
            return;
        }

        // Compara cada posição da sequência do jogador com a sequência correta
        for (int i = 0; i < correctSequence.Count; i++)
        {
            if (playerSequence[i] != correctSequence[i])
            {
                ResetPuzzle();
                return;
            }
        }

        // Se todas baterem, o puzzle é considerado resolvido
        CompletePuzzle();
    }

    // Reseta o estado do puzzle, apagando velas e limpando sequências
    private void ResetPuzzle()
    {
        playerSequence.Clear();
        litCandles.Clear();

        // Chama o método de reset individual de cada vela
        foreach (var candle in candles)
        {
            candle.ForceReset();
        }
    }

    // Marca o puzzle como concluído e ativa o objeto-chave (ex: faz uma chave aparecer)
    private void CompletePuzzle()
    {
        puzzleCompleted = true;
        keyObject.SetActive(true);
        Debug.Log("Puzzle concluído! A chave apareceu.");
    }

    // Permite que outros scripts verifiquem se o puzzle já foi resolvido
    public bool IsPuzzleCompleted()
    {
        return puzzleCompleted;
    }
}
