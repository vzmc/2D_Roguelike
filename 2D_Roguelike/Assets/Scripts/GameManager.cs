using UnityEngine;
using System.Collections;
using System.Collections.Generic; //Listを使う時に宣言
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public float turnDelay = 0.1f;      // Enemyの動作時間
    public int playerFoodPoints = 100;
    public float levelStartDelay = 2f;

    [HideInInspector]
    public bool playerTurn = true;

    private BoardManager boardScript;
    private int level = 0;
    private Text levelText;
    private GameObject levelImage;
    private bool doingSetup;            //levelImageの表示等で活用

    private List<Enemy> enemies;        // EnemyのList
    private bool enemiesMoving;         // Enemyのターン中True

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        enemies = new List<Enemy>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        level++;
        InitGame();
    }

    private void Update()
    {
        if (playerTurn || enemiesMoving || doingSetup)
        {            return;        }

        StartCoroutine(MoveEnemies());
    }

    private IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        //if(enemies.Count == 0)
        //{
        //    yield return new WaitForSeconds(turnDelay);
        //}

        //Enemyの数だけEnemyスクリプトのMoveEnemyを実行
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playerTurn = true;
        enemiesMoving = false;
    }

    public void InitGame()
    {
        doingSetup = true;

        //LevelImageオブジェクト・LevelTextオブジェクトの取得
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day" + level;             //最新のレベルに更新
        levelImage.SetActive(true);                 //LebelImageをアクティブにし表示
        Invoke("HideLevelImage", levelStartDelay);  //2秒後にHideLevelImageメソッド呼び出し

        enemies.Clear();
        playerTurn = true;
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);    //LevelImage非アクティブ化
        doingSetup = false;             //プレイヤーが動けるようになる
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        //LevelImage非アクティブ化
        levelText.text = "After " + level + " days, you starved.";

        //プレイヤーが動けるようになる
        levelImage.SetActive(true);
        enabled = false;
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

}