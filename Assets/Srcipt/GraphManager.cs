using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
public class GraphManager : MonoBehaviour
{
    public CSVReader Data;
    public GraphRenderer graph;
    public List<MovingAvergeRenderer> movingAverages;

    [Header("Ma")]
    public InputField maInput;
    public FlexibleColorPicker maColorPicker;
    public GameObject maFeb;
    public Transform parent;

    [Header("Customize")]
    public int upperMarginPercent;
    public int dowerMarginPercent;
    public int rightMarginPercent;
    public float candleWickWidthPercen = 0.08f;
    public float candleMarginPercen = 0.13f;

    public float getCandleMargin { get { return candleWidth * candleMarginPercen; } }

    static float XW_max;
    static float YW_max;
    static float XW_min;
    static float YW_min;

    static float XV_max;
    static float YV_max;
    static float XV_min;
    static float YV_min;

    static float SX;
    static float SY;

    public static float candleWidth;

    public float getCandleWidth { get { return candleWidth; } }
    public float getCandleWick { get { return candleWidth * candleWickWidthPercen; } }

    int min;
    int max;
    
    [Header("Zoom In Zoom out")]
    public float scroolSpeedPer = 0.1f;
    public float zoomLimit = 0.1f;
    int scrollSpeed = 1;

    private void Start()
    {
        UpdateGraphManger();
    }

    public void UpdateGraphManger()
    {
        scrollSpeed = (int)(Data.endRow * scroolSpeedPer);
        updateGeometry();
    }

    //change on inspector
    private void OnValidate()
    {
        updateGeometry();
    }

    //change on CSV
    public void updateGeometry()
    {
        updateConfing();
        graph.UpdateGraph();
        foreach (MovingAvergeRenderer item in movingAverages)
        {
            item.updateLine();
        }
    }

    void updateConfing()
    {
        RectTransform rectTransform = this.GetComponent<RectTransform>();

        //XW
        XV_max = rectTransform.rect.width;
        YV_max = rectTransform.rect.height;
        XV_min = 0;
        YV_min = 0;

        candleWidth = XV_max / (Data.DATA.Length + Mathf.RoundToInt(findPercentage(rightMarginPercent, Data.DATA.Length)));
        min = Data.minValue();
        max = Data.maxValue();
        min -= (int)findPercentage(dowerMarginPercent, max);
        max += (int)findPercentage(upperMarginPercent, max);

        //XV
        XW_max = candleWidth * (Data.DATA.Length + 1);
        YW_max = max;
        XW_min = 0;
        YW_min = min;

        //SX SY
        SX = (XV_max - XV_min) / (XW_max - XW_min);
        SY = (YV_max - YV_min) / (YW_max - YW_min);
    }

    public static float findPercentage(float percent, float max)
    {
        return (percent * max) / 100;
    }

    public static int convertCoordinateYToViewportY(int posY)
    {
        return Mathf.RoundToInt(YV_min + (posY - YW_min) * SY);
    }

    public static int convertCoordinateXToViewportX(int posX)
    {
        return Mathf.RoundToInt(XV_min + (posX - XW_min) * SX);
    }

    public void createMaLine()
    {
        GameObject ma = Instantiate(maFeb, Vector3.zero, Quaternion.identity) as GameObject;
        ma.GetComponent<MovingAvergeRenderer>()._manager = this;
        ma.GetComponent<MovingAvergeRenderer>().color = maColorPicker.color;
        ma.GetComponent<MovingAvergeRenderer>().Data = Data;
        ma.GetComponent<MovingAvergeRenderer>().timeFrame = int.Parse(maInput.text);
        ma.transform.SetParent(parent, false);

        movingAverages.Add(ma.GetComponent<MovingAvergeRenderer>());
    }

    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && Data.startRow <= Data.endRow - (Data.endRow * zoomLimit))
        {
            Data.startRow += scrollSpeed;
            Data.Update();
            updateGeometry();
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Data.startRow -= scrollSpeed;
            Data.Update();
            updateGeometry();
        }
    }
}
