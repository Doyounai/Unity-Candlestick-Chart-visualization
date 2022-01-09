using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*  =====================================================
*
*                       _oo0oo_
*                      o8888888o
*                      88" . "88
*                      (| -_- |)
*                      0\  =  /0
*                    ___/`---'\___
*                  .' \|     |// '.
*                 / \|||  :  |||// \
*                / _||||| -:- |||||- \
*               |   | \\  -  /// |   |
*               | \_|  ''\---/''  |_/ |
*               \  .-\__  '-'  ___/-. /
*             ___'. .'  /--.--\  `. .'___
*          ."" '<  `.___\_<|>_/___.' >' "".
*         | | :  `- \`.;`\ _ /`;.`/ - ` : | |
*         \  \ `_.   \_ __\ /__ _/   .-` /  /
*     =====`-.____`.___ \_____/___.-`___.-'=====
*                       `=---='
*
*     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
*
*               Buddha Bless:  "No Bugs"
*
*/

[System.Serializable]
public class candleData
{
    public string Date;
    public int Open;
    public int Close;
    public int Hightest;
    public int Lowest;

    public bool isGrowCandle
    {
        get
        {
            return (Close > Open) ? true : false;
        }
    }
}

[CreateAssetMenu(fileName = "new CSV", menuName = "CSV DATA")]
public class CSVReader : ScriptableObject
{
    public TextAsset CSV_DATA;
    public int startRow;
    public int endRow;
    public int currentPrice;
    public float fee;

    [Header("Data")]
    public candleData[] DATA;

    public void OnValidate()
    {
        Update();
    }

    public void Update()
    {
        checkError();
        loadData();
    }

    public void passTrun(int day)
    {
        endRow += day;
        checkError();
        loadData();
    }

    private void checkError()
    {
        string[] datas = CSV_DATA.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        int dataSize = datas.Length / 7 - 1;

        if (startRow < 1)
            startRow = 1;
        if (startRow >= dataSize + 1)
            startRow = dataSize;
        if (endRow > dataSize + 1)
            endRow = dataSize + 1;
        if (endRow <= 0)
            endRow = 2;
    }

    private void loadData()
    {
        string[] datas = CSV_DATA.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        //Debug.Log(datas[8]);

        int dataSize = endRow - startRow;
        DATA = new candleData[dataSize];

        for (int i = 0; i < dataSize; i++)
        {
            DATA[i] = new candleData();
            int index = (7 * (i + 1)) + ((startRow - 1) * 7);
            DATA[i].Date = datas[index];
            DATA[i].Open = convertStringFloatToInt(datas[index + 1]);
            DATA[i].Hightest = convertStringFloatToInt(datas[index + 2]);
            DATA[i].Lowest = convertStringFloatToInt(datas[index + 3]);
            DATA[i].Close = convertStringFloatToInt(datas[index + 4]);
            if (i == dataSize - 1)
                currentPrice = convertStringFloatToInt(datas[index + 4]);
        }
    }

    int convertStringFloatToInt(string text)
    {
        return (int)float.Parse(text);
    }

    public int minValue()
    {
        int min;

        //min = (DATA[0].isGrowCandle) ? DATA[0].Open : DATA[0].Close;
        min = DATA[0].Lowest;

        for (int i = 1; i < DATA.Length; i++)
        {
            candleData candle = DATA[i];
            //int temp = (candle.isGrowCandle) ? candle.Open : candle.Close;
            int temp = DATA[i].Lowest;
            min = (temp < min) ? temp : min;
        }

        return min;
    }

    public int maxValue()
    {
        int max;

        //max = (DATA[0].isGrowCandle) ? DATA[0].Close : DATA[0].Open;
        max = DATA[0].Hightest;

        for (int i = 0; i < DATA.Length; i++)
        {
            candleData candle = DATA[i];
            //int temp = (candle.isGrowCandle) ? candle.Close : candle.Open;
            int temp = DATA[i].Hightest;
            max = (temp > max) ? temp : max;
        }

        return max;
    }
}
