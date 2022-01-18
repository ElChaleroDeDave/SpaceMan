using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState//un enumerado public llamado gameState, Donde esta los estados Menu InGame GameOver
{
    menu,
    inGame,
    gameOver
}

public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.menu;

    public static GameManager sharedInstance;//un static para el GameManager con SharedInstance

    private PlayerController controller;//player controller a controller (privato)

    public int collectedObject = 0;

     void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }        
    }
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit") && currentGameState != GameState.inGame)//boton del submit para el StartGame
        {
            StartGame();
        }
    }


    public void StartGame()//un void publico para el startGame
    {
        SetGameState(GameState.inGame);//SetGame(Cambiar el modo de juego) a InGame(GameState.InGame)

    }

    public void GameOver()//un void publico para el GameOver
    {
        SetGameState(GameState.gameOver);
    }

    public void BackToMenu()//un void publico para el BackToMenu
    {
        SetGameState(GameState.menu);
    }

    private void SetGameState(GameState newGameState)//un void publico para el SetGameState(Cambiar el modo de juego)
    {
        if (newGameState == GameState.menu)//si(if) new game state =(es) menu
        {
            //todo: menu
            MenuManager.sharedInstance.HideGameOver();
            MenuManager.sharedInstance.HideScore();
            MenuManager.sharedInstance.ShowMainMenu();
        }
        else if (newGameState == GameState.inGame)//si(if) new game state =(es) InGame
        {
            LevelManager.sharedInstance.RemoveAllLevelBlocks();
            LevelManager.sharedInstance.GenerateInitialBlocks();
            controller.StartGame();//controller.startgame
            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.ShowScore();
            MenuManager.sharedInstance.HideGameOver();
        }
        else if(newGameState == GameState.gameOver)//si(if) new game state =(es) GameOver
        {
            MenuManager.sharedInstance.ShowGameOver();
            MenuManager.sharedInstance.HideScore();
        }

        this.currentGameState = newGameState;
    }

       public void collectObject(Collectable collectable)
    {
        collectedObject += collectable.value;
    }
    

}
