using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

/// <summary>
/// Gerencia o inventário do jogo, armazenando itens em tempo de execução.
/// Este inventário não possui interface gráfica e trabalha apenas com dados internos.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    // Lista onde os itens do inventário serão armazenados
    private List<ItemData> inventory = new List<ItemData>();

    // Instância única do InventoryManager para acesso global (Singleton)
    public static InventoryManager Instance { get; private set; }

    /// <summary>
    /// Configuração do Singleton para garantir que exista apenas um InventoryManager ativo.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Define esta instância como a única
        }
        else
        {
            Destroy(gameObject); // Se já existir um InventoryManager, destrói este para evitar duplicação
        }
    }
    
    /// <summary>
    /// Adiciona um item ao inventário.
    /// </summary>
    /// <param name="item">O item a ser adicionado.</param>
    public void AddItem(ItemData item)
    {
        if (item == null) return; // Evita adicionar itens nulos

        inventory.Add(item); // Adiciona o item à lista
        Debug.Log($"Item adicionado: {item.itemName}"); // Exibe uma mensagem no console para depuração
    }

    /// <summary>
    /// Remove um item do inventário, se ele estiver presente.
    /// </summary>
    /// <param name="item">O item a ser removido.</param>
    public void RemoveItem(ItemData item)
    {
        if (inventory.Contains(item)) // Verifica se o item existe no inventário
        {
            inventory.Remove(item); // Remove o item da lista
            Debug.Log($"Item removido: {item.itemName}"); // Mensagem para depuração
        }
    }

    /// <summary>
    /// Verifica se um item com determinado ID está no inventário.
    /// </summary>
    /// <param name="itemId">O ID do item a ser verificado.</param>
    /// <returns>Retorna verdadeiro se o item estiver no inventário, falso caso contrário.</returns>
    public bool HasItem(int itemId)
    {
        return inventory.Any(i => i.id == itemId); // Retorna verdadeiro se encontrar um item com o ID correspondente
    }

    /// <summary>
    /// Retorna uma cópia da lista de todos os itens do inventário.
    /// </summary>
    /// <returns>Lista de itens do inventário.</returns>
    public List<ItemData> GetAllItems()
    {
        return new List<ItemData>(inventory); // Retorna uma cópia para evitar alterações externas
    }
    
}