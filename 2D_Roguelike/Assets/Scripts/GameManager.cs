using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int playerFoodPoints = 100;
    [HideInInspector]
    public bool playerTurn = true;

    private BoardManager boardScript;
    private int level = 3;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    private void InitGame()
    {
        boardScript.SetupScene(level);
    }

    public void GameOver()
    {
        enabled = false;
    }
}