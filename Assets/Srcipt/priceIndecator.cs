using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class priceIndecator : MonoBehaviour
{
    public GameObject snapWith;
    public bool isPriceIndecator = true;

    float maxY;
    float minY;

    float maxX;
    float minX;

    private void Start()
    {
        maxY = snapWith.transform.position.y + snapWith.GetComponent<RectTransform>().rect.height;
        minY = snapWith.transform.position.y;

        maxX = snapWith.transform.position.x + snapWith.GetComponent<RectTransform>().rect.width;
        minX = snapWith.transform.position.x;
    }

    private void LateUpdate()
    {
        Vector3 mousePosition = Input.mousePosition;

        if (isPriceIndecator && mousePosition.y <= maxY && mousePosition.y >= minY)
            transform.position = new Vector3(transform.position.x, mousePosition.y, 0);
        else if (!isPriceIndecator && mousePosition.x <= maxX && mousePosition.x >= minX)
            transform.position = new Vector3(mousePosition.x, transform.position.y, 0);
    }
}
