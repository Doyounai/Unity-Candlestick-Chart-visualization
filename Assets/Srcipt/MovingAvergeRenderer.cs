using UnityEngine;
using UnityEngine.UI;

public class MovingAvergeRenderer : Graphic
{
    [Header("Properties")]
    public GraphManager _manager;
    public CSVReader Data;

    public int timeFrame = 2;
    public int[] movingAverage;

    [Header("Customize")]
    public float thickness = 10f;
    public Color testColor;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (timeFrame < 1)
            return;

        caculatemovingAverage();

        if (movingAverage.Length < 2)
            return;

        float angle = 0;

        for (int i = 0; i < movingAverage.Length; i++)
        {
            if (i < movingAverage.Length - 1)
            {
                float candleWidth = GraphManager.candleWidth;
                Vector2 me = new Vector2(i, GraphManager.convertCoordinateYToViewportY(movingAverage[i]));
                Vector2 target = new Vector2(i + 1, GraphManager.convertCoordinateYToViewportY(movingAverage[i + 1]));
                angle = getAngle(me, target) + 10f;
            }
            drawLine(i, vh, angle);
        }

        for (int i = 0; i < movingAverage.Length - 1; i++)
        {
            int index = i * 2;
            vh.AddTriangle(index + 0, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index + 0);
        }
    }

    private void drawLine(int i, VertexHelper vh, float angle)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;
        //vertex.color = (i % 2 == 0) ? color : testColor;

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(GraphManager.candleWidth * (i + timeFrame), GraphManager.convertCoordinateYToViewportY(movingAverage[i]));
        vh.AddVert(vertex);

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(GraphManager.candleWidth * (i + timeFrame), GraphManager.convertCoordinateYToViewportY(movingAverage[i]));
        vh.AddVert(vertex);
    }

    private void caculatemovingAverage()
    {
        if (timeFrame > Data.DATA.Length)
            return;

        movingAverage = new int[Data.DATA.Length - (timeFrame - 1)];
        int countMA = 0;

        for (int i = timeFrame - 1; i < Data.DATA.Length; i++)
        {
            int[] temp = new int[timeFrame];
            int countARR = 0;

            //create array
            for (int k = (i + 1) - timeFrame; k < i + 1; k++)
            {
                temp[countARR] = Data.DATA[k].Close;
                countARR++;
            }

            movingAverage[countMA] = average(temp);
            countMA++;
        }
    }

    private int average(int[] numbers)
    {
        int sum = 0;

        for (int i = 0; i < numbers.Length; i++)
        {
            //Debug.Log(numbers[i]);
            sum += numbers[i];
        }

        return Mathf.RoundToInt(sum / numbers.Length);
    }

    private float getAngle(Vector2 me, Vector2 target)
    {
        return (float)(Mathf.Atan2(target.y - me.y, target.x - me.x) * (180 / Mathf.PI));
    }

    public void updateLine()
    {
        UpdateGeometry();
    }

    private void Update()
    {
        if (_manager != null && Data != _manager.Data)
            Data = _manager.Data;
    }
}
