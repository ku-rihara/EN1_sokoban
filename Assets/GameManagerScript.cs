using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject BoxPrefab;
    int[,] map;//レベルデザイン用の配列
    GameObject[,] field;//ゲーム管理用の配列
  
   
    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                //nullチェック
                if (field[y, x] == null){continue;}

                if (field[y, x].tag == "Player")
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }
    /// <summary>
    /// 
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

        else if (field[moveTo.y, moveTo.x] != null && field[moveTo.y,moveTo.x].tag==tag)
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(tag, moveTo, moveTo + velocity);
            if (!success) { return false; }
        }
        //二次元配列に対応
        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }
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
            MoveNumber("Box", playerIndex, new Vector2Int(playerIndex.x, playerIndex.y-1));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber("Box", playerIndex, new Vector2Int(playerIndex.x, playerIndex.y+1));
        }
    }

    //クリア判定
    bool IsCleard()
    {
        //Vector2Int型の可変長配列の作成
        List<Vector2Int>goals=new List<Vector2Int>();
        for(int y=0;y<map.GetLength(0);y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                //格納場所か否か
                if (map[y,x] == 3)
                {
                    goals.Add(new Vector2Int(x,y));
                }
            }
        }
        //要素数はgoals.Coustで取得
        for(int i=0;i<goals.Count;i++)
        {
            GameObject f = field[goals[i].y,goals[i].x];
            if(f==null||f.tag!="Box")
            {
                return false;
            }
        }
        //条件達成出なければ
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        map = new int[,] {//3x5のサイズ
        {0,0,0,0,0},
        {0,3,1,3,0},
        {0,0,2,0,0},
        {0,2,3,2,0},
        {0,0,0,0,0},
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
                    field[y,x] = Instantiate(
                    playerPrefab,
                    new Vector3(x, map.GetLength(0) - y, 0),
                    Quaternion.identity
                    );
                }
              else  if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                    BoxPrefab,
                    new Vector3(x, map.GetLength(0) - y, 0),
                    Quaternion.identity
                    );
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        //プレイヤーの移動
        PlayerMove();
    }


}

