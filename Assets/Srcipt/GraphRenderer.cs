using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphRenderer : Graphic
{
    [Header("Properties")]
    public GraphManager _Manger;
    public CSVReader Data;

    [Header("Customize")]
    public Color growColor;
    public Color downColor;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (Data.DATA.Length <= 0)
            return;

        //add vertex
        for (int i = 0; i < Data.DATA.Length; i++)
        {
            candleData candle = Data.DATA[i];
            drawGraph(candle, i, vh);
        }
    }

    private void drawGraph(candleData candle, int index, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = (candle.isGrowCandle)? growColor : downColor;
        //vertex.color = color;

        float candleStartPosition = _Manger.getCandleWidth;

        //0
        vertex.position = new Vector3(candleStartPosition * index, GraphManager.convertCoordinateYToViewportY(candle.Open));
        vh.AddVert(vertex);
        //1
        vertex.position = new Vector3(candleStartPosition * index, GraphManager.convertCoordinateYToViewportY(candle.Close));
        vh.AddVert(vertex);
        //2
        vertex.position = new Vector3(candleStartPosition * (index + 1) - _Manger.getCandleMargin, GraphManager.convertCoordinateYToViewportY(candle.Close));
        vh.AddVert(vertex);
        //3
        vertex.position = new Vector3(candleStartPosition * (index + 1) - _Manger.getCandleMargin, GraphManager.convertCoordinateYToViewportY(candle.Open));
        vh.AddVert(vertex);

        //candle Wick
        float candleWickStartPos = (candleStartPosition * index) + ((_Manger.getCandleWidth - _Manger.getCandleMargin) - _Manger.getCandleWick) / 2;

        //4 
        vertex.position = new Vector3(candleWickStartPos, GraphManager.convertCoordinateYToViewportY(candle.Hightest));
        vh.AddVert(vertex);
        //5
        vertex.position = new Vector3(candleWickStartPos, GraphManager.convertCoordinateYToViewportY(candle.Lowest));
        vh.AddVert(vertex);
        //6
        vertex.position = new Vector3(candleWickStartPos + _Manger.getCandleWick, GraphManager.convertCoordinateYToViewportY(candle.Lowest));
        vh.AddVert(vertex);
        //7
        vertex.position = new Vector3(candleWickStartPos + _Manger.getCandleWick, GraphManager.convertCoordinateYToViewportY(candle.Hightest));
        vh.AddVert(vertex);

        int i = index * 8;
        vh.AddTriangle(i + 0, i + 1, i + 2);
        vh.AddTriangle(i + 2, i + 3, i + 0);

        vh.AddTriangle(i + 4, i + 5, i + 6);
        vh.AddTriangle(i + 6, i + 7, i + 4);
    }

    public void UpdateGraph()
    {
        UpdateGeometry();
    }

    public void Update()
    {
        if (Data != _Manger.Data)
            Data = _Manger.Data;
    }
}
