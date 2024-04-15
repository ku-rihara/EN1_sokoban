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
    /// <param name="number">//����������</param>
    /// <param name="moveFrom">//���������̃C���f�b�N�X</param>
    /// <param name="moveTo">//�������C���f�b�N�X</param>
    /// <returns></returns>
    bool MoveNumber(int number,int moveFrom,int moveTo) {
        if (moveTo < 0||moveTo >= map.Length){
            return false;
        }
        if (map[moveTo] == 2)
        {
            //�ǂ̕����ֈړ����邩���Z�o
            int velocity = moveTo - moveFrom;
            //�v���C���[�̈ړ��悩��A����ɐ��2(��)���ړ�������
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
            //1.����������L�q
            //������Ȃ��������ׂ̈�-1�ŏ���������
            int playerIndex = GetPlayerIndex();

            /*playerIndex+1�̃C���f�b�N�X�̕��ƌ�������̂ŁA
             playerIndex-1��肳��ɏ������C���f�b�N�X�̎�
            �̂݌����������s��*/
            MoveNumber(1, playerIndex, playerIndex + 1);
            PrintArray();
        }
    }


}

