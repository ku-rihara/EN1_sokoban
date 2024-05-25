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

    int[,] map;//���x���f�U�C���p�̔z��
    GameObject[,] field;//�Q�[���Ǘ��p�̔z��


    // Start is called before the first frame update
    void Start()
    {

        Screen.SetResolution(1280, 720, false);

        map = new int[,] {//3x5�̃T�C�Y
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
            //�v���C���[�̈ړ�
            PlayerMove();

        //�N���A������
        if (IsCleard())
        {
            clearText.SetActive(true);
        }
    }

//�֐�******************************************************************************************************
    Vector2Int GetPlayerIndex()//�v���C���[��index
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                //null�`�F�b�N
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
    /// �C���f�b�N�X�̈ړ�
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

        for (int i = 0; i < 10; i++)
        {
            Particle instance = (Particle)Instantiate(particlePrefab,
                                                   new Vector3Int(GetPlayerIndex().x, 0, field.GetLength(0) - GetPlayerIndex().y),
                                                    Quaternion.identity);

        }


        //�^�O�֘A�̏���
        if (field[moveTo.y, moveTo.x] != null)//null�`�F�b�N
        {
            if (field[moveTo.y, moveTo.x].tag == "Stop") { return false; }//Stop�^�O�͓����Ȃ�

            else if (field[moveTo.y, moveTo.x].tag == tag)//Box�^�O
            {
                {//Box�̈ʒu�𓮂���
                    Vector2Int velocity = moveTo - moveFrom;
                    bool success = MoveNumber(tag, moveTo, moveTo + velocity);
                    if (!success) { return false; }
                }
            }
        }
      
        //�񎟌��z��ɑΉ�
        Vector3 moveToPosition = new Vector3(
          moveTo.x, 0, field.GetLength(0) - moveTo.y);
        field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];

     
        field[moveFrom.y, moveFrom.x] = null;

        return true;
    }
    /// <summary>
    /// �v���C���[�̈ړ�
    /// </summary>
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
            MoveNumber("Box", playerIndex, new Vector2Int(playerIndex.x, playerIndex.y - 1));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber("Box", playerIndex, new Vector2Int(playerIndex.x, playerIndex.y + 1));
        }
    }
  
    //�N���A����
    bool IsCleard()
    {
        //Vector2Int�^�̉ϒ��z��̍쐬
        List<Vector2Int> goals = new List<Vector2Int>();
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                //�i�[�ꏊ���ۂ�
                if (map[y, x] == 3)
                {
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }
        //�v�f����goals.Coust�Ŏ擾
        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box")
            {
                return false;
            }
        }
        //�����B���o�Ȃ����
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
                    //�}�b�v���ɂ�����̂�j������
                    Destroy(field[y, x]);
                    field[y, x] = null;
                }
            }
            Start();//������
        }
    }

}

