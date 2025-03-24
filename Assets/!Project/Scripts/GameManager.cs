using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public GameObject crosshair;
    public bool endGame;
    public void ExitGame()
    {
        // Sai do jogo no build
        Application.Quit();
    }
}