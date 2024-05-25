using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject playerPrefab;//1
    public GameObject BoxPrefab;//2
    public GameObject goalPrefab;//3
    public GameObject StopPrefab;
    public GameObject clearText;
    public GameObject ResetText;
    public Particle particlePrefab;

    int[,] map;//レベルデザイン用の配列
    GameObject[,] field;//ゲーム管理用の配列


    // Start is called before the first frame update
    void Start()
    {

        Screen.SetResolution(1280, 720, false);

        map = new int[,] {//3x5のサイズ
        {4,4,4,4,4,4,4,4,4},
        {4,3,1,3,4,0,0,0,4},
        {4,0,2,0,4,0,0,0,4},
        {4,2,3,2,4,0,0,0,4},
        {4,0,0,0,0,0,0,0,4},
        {4,0,0,0,0,0,0,0,4},
        {4,4,4,4,4,4,4,4,4},
        };
        field = new GameObject[
            map.GetLength(0),
            map.GetLength(1)
            ];

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    field[y, x] = Instantiate(
                    playerPrefab,
                    new Vector3(x, 0, map.GetLength(0) - y),
                    Quaternion.identity
                    );
                }
                else if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                    BoxPrefab,
                  new Vector3(x, 0, map.GetLength(0) - y),
                    Quaternion.identity
                    );
                }
                else if (map[y, x] == 3)
                {
                    field[y, x] = Instantiate(
                    goalPrefab,
                     new Vector3(x, 0, map.GetLength(0) - y),
                    Quaternion.identity
                    );
                }
                else if (map[y, x] == 4)
                {
                    field[y, x] = Instantiate(
                    StopPrefab,
                     new Vector3(x, 0, map.GetLength(0) - y),
                    Quaternion.identity
                    );
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
       GameReset();
            //プレイヤーの移動
            PlayerMove();

        //クリアしたら
        if (IsCleard())
        {
            clearText.SetActive(true);
        }
    }

//関数******************************************************************************************************
    Vector2Int GetPlayerIndex()//プレイヤーのindex
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                //nullチェック
                if (field[y, x] == null) { continue; }

                if (field[y, x].tag == "Player")
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }
    /// <summary>
    /// インデックスの移動
    /// </summary>
    /// <param name="number">//動かす数字</param>
    /// <param name="moveFrom">//動かす元のインデックス</param>
    /// <param name="moveTo">//動かすインデックス</param>
    /// <returns></returns>
    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
       
        //二次元配列に対応
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }

        for (int i = 0; i < 10; i++)
        {
            Particle instance = (Particle)Instantiate(particlePrefab,
                                                   new Vector3Int(GetPlayerIndex().x, 0, field.GetLength(0) - GetPlayerIndex().y),
                                                    Quaternion.identity);

        }


        //タグ関連の処理
        if (field[moveTo.y, moveTo.x] != null)//nullチェック
        {
            if (field[moveTo.y, moveTo.x].tag == "Stop") { return false; }//Stopタグは動けない

            else if (field[moveTo.y, moveTo.x].tag == tag)//Boxタグ
            {
                {//Boxの位置を動かす
                    Vector2Int velocity = moveTo - moveFrom;
                    bool success = MoveNumber(tag, moveTo, moveTo + velocity);
                    if (!success) { return false; }
                }
            }
        }
      
        //二次元配列に対応
        Vector3 moveToPosition = new Vector3(
          moveTo.x, 0, field.GetLength(0) - moveTo.y);
        field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];

     
        field[moveFrom.y, moveFrom.x] = null;

        return true;
    }
    /// <summary>
    /// プレイヤーの移動
    /// </summary>
    void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            //1.をここから記述
            //見つからなかった時の為に-1で初期化する
            Vector2Int playerIndex = GetPlayerIndex();

            /*playerIndex+1のインデックスの物と交換するので、
             playerIndex-1よりさらに小さいインデックスの時
            のみ交換処理を行う*/
            MoveNumber("Box", playerIndex, new Vector2Int(playerIndex.x + 1, playerIndex.y));

        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber("Box", playerIndex, new Vector2Int(playerIndex.x - 1, playerIndex.y));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber("Box", playerIndex, new Vector2Int(playerIndex.x, playerIndex.y - 1));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber("Box", playerIndex, new Vector2Int(playerIndex.x, playerIndex.y + 1));
        }
    }
  
    //クリア判定
    bool IsCleard()
    {
        //Vector2Int型の可変長配列の作成
        List<Vector2Int> goals = new List<Vector2Int>();
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                //格納場所か否か
                if (map[y, x] == 3)
                {
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }
        //要素数はgoals.Coustで取得
        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box")
            {
                return false;
            }
        }
        //条件達成出なければ
        return true;
    }
    public void GameReset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    //マップ内にあるものを破棄する
                    Destroy(field[y, x]);
                    field[y, x] = null;
                }
            }
            Start();//初期化
        }
    }

}

