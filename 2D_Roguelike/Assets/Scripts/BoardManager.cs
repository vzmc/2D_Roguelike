using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    /// <summary>
    /// 各種カウント用クラスを作ります
    /// </summary>
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;                                     // ステージの縦マス
    public int rows = 8;                                        // ステージの横マス
    public Count wallCount = new Count(5, 9);                   // 壁の出現範囲
    public Count foodCount = new Count(1, 5);                   // アイテムの出現範囲
    public GameObject exit;                                     // 出口GameObject
    public GameObject[] floorTiles;                             // フロアタイルの配列
    public GameObject[] wallTiles;                              // 壁の配列
    public GameObject[] foodTiles;                              // アイテムの配列
    public GameObject[] enemyTiles;                             // 敵キャラの配列
    public GameObject[] outerWallTiles;                         // 外壁の配列

    private Transform boardHolder;                              // オブジェクトの配置できる範囲を保存する変数
    private List<Vector3> gridPositions = new List<Vector3>();  // オブジェクトを配置できる範囲を示すList

    /// <summary>
    /// gridPositionsのListを初期化
    /// </summary>
    private void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    /// <summary>
    /// Boardを作る
    /// </summary>
    private void BoardSetup()
    {
        // Boardというオブジェクトを作成し、transform情報をboardHolderに保存
        boardHolder = new GameObject("Board").transform;

        // x = -1〜8をループします。
        for (int x = -1; x < columns + 1; x++)
        {
            // y = -1〜8をループします。
            for (int y = -1; y < rows + 1; y++)
            {
                // ランダムで床をで選択します。
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                // 左端or右端or最低部or最上部の時＝外壁を作る時
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    // floorTileの時と同じように外壁をランダムで選択し、上書きする
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                // 床or外壁を生成し、instance変数に格納
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                // 生成したinstanceをBoardオブジェクトの子オブジェクトとする
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    /// <summary>
    /// ランダムの位置を取得する
    /// </summary>
    /// <returns>ランダムの位置</returns>
    private Vector3 RandomPosition()
    {
        // 6×6 = 36からランダムで１つを決めます。
        int randomIndex = Random.Range(0, gridPositions.Count);

        Vector3 randomPosition = gridPositions[randomIndex];

        // randomPositionにIndexを代入できたら、randomIndexは削除します。
        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    /// <summary>
    /// ランダムの位置でアイテムをランダムの種類で生成する
    /// </summary>
    /// <param name="tileArray"></param>
    /// <param name="minimum"></param>
    /// <param name="maximum"></param>
    private void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        // 最低値から最大値+1のランダム回数分だけループします。
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            // gridPositionから位置情報を１つ取得
            Vector3 randomPosition = RandomPosition();

            // 引数tileArrayからランダムで1つ選択
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            // ランダムで決定した種類・位置でオブジェクトを生成
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    /// <summary>
    /// BoardをSetupする
    /// </summary>
    /// <param name="level"></param>
    public void SetupScene(int level)
    {
        // 床と外壁を配置します。
        BoardSetup();

        // 敵キャラ、内壁、アイテムを配置できる位置を決定します。
        InitialiseList();

        // 敵キャラ、内壁、アイテムをランダム配置で配置します。
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        int enemyCount = (int)Mathf.Log(level, 2f);

        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        // Exitを7, 7の位置に配置する。
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}