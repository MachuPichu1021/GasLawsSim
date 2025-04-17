using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Point : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltipPrefab;
    private GameObject tooltipReference;

    private DataPoint data;
    public DataPoint Data { set => data = value; }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        tooltipReference = Instantiate(tooltipPrefab, transform.parent);
        tooltipReference.GetComponentInChildren<TMP_Text>().text = $"V: {data.Volume:0.00} mL\nT: {data.Temperature:0.00} K\nP: {data.Pressure:0.00} kPa";
    }

    public void OnPointerMove(PointerEventData pointerEventData)
    {
        tooltipReference.transform.position = new Vector2(Input.mousePosition.x + 175, Input.mousePosition.y - 100);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Destroy(tooltipReference);
    }
}
