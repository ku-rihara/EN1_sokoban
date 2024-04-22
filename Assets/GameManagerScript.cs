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
    int[,] map;//���x���f�U�C���p�̔z��
    GameObject[,] field;//�Q�[���Ǘ��p�̔z��
  
   
    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                //null�`�F�b�N
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
    /// <param name="number">//����������</param>
    /// <param name="moveFrom">//���������̃C���f�b�N�X</param>
    /// <param name="moveTo">//�������C���f�b�N�X</param>
    /// <returns></returns>
    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
         //�񎟌��z��ɑΉ�
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }

        else if (field[moveTo.y, moveTo.x] != null && field[moveTo.y,moveTo.x].tag==tag)
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(tag, moveTo, moveTo + velocity);
            if (!success) { return false; }
        }
        //�񎟌��z��ɑΉ�
        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }
    void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //1.����������L�q
            //������Ȃ��������ׂ̈�-1�ŏ���������
            Vector2Int playerIndex = GetPlayerIndex();

            /*playerIndex+1�̃C���f�b�N�X�̕��ƌ�������̂ŁA
             playerIndex-1��肳��ɏ������C���f�b�N�X�̎�
            �̂݌����������s��*/
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

    //�N���A����
    bool IsCleard()
    {
        //Vector2Int�^�̉ϒ��z��̍쐬
        List<Vector2Int>goals=new List<Vector2Int>();
        for(int y=0;y<map.GetLength(0);y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                //�i�[�ꏊ���ۂ�
                if (map[y,x] == 3)
                {
                    goals.Add(new Vector2Int(x,y));
                }
            }
        }
        //�v�f����goals.Coust�Ŏ擾
        for(int i=0;i<goals.Count;i++)
        {
            GameObject f = field[goals[i].y,goals[i].x];
            if(f==null||f.tag!="Box")
            {
                return false;
            }
        }
        //�����B���o�Ȃ����
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        map = new int[,] {//3x5�̃T�C�Y
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
        //�v���C���[�̈ړ�
        PlayerMove();
    }


}

