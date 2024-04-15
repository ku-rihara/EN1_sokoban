using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NewBehaviourScript : MonoBehaviour
{
    int[] map;
    void PrintArray() {
        string debugText = "";
        for (int i = 0; i < map.Length; i++) { 
            debugText += map[i].ToString()+",";
        }
        Debug.Log(debugText);
    }
    int GetPlayerIndex() { 
        for(int i = 0;i < map.Length;i++)
        {
            if (map[i] == 1)
            {
                return i;
            }
        }
        return -1;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="number">//動かす数字</param>
    /// <param name="moveFrom">//動かす元のインデックス</param>
    /// <param name="moveTo">//動かすインデックス</param>
    /// <returns></returns>
    bool MoveNumber(int number,int moveFrom,int moveTo) {
        if (moveTo < 0||moveTo >= map.Length){
            return false;
        }
        if (map[moveTo] == 2)
        {
            //どの方向へ移動するかを算出
            int velocity = moveTo - moveFrom;
            //プレイヤーの移動先から、さらに先へ2(箱)を移動させる
        }
        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;
    }
    // Start is called before the first frame update
    void Start()
    {
        map = new int[] { 0, 0, 0, 1, 0, 2, 0, 0, 0 };
        PrintArray();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //1.をここから記述
            //見つからなかった時の為に-1で初期化する
            int playerIndex = GetPlayerIndex();

            /*playerIndex+1のインデックスの物と交換するので、
             playerIndex-1よりさらに小さいインデックスの時
            のみ交換処理を行う*/
            MoveNumber(1, playerIndex, playerIndex + 1);
            PrintArray();
        }
    }


}

