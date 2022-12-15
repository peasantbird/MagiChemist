using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TipWindowManager : MonoBehaviour
{
    public TextMeshProUGUI tipText;
    public RectTransform tipWindow;
    public float width;
    public float height;

    public static Action<string, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;
    // Start is called before the first frame update
    void Start()
    {
        hideTip();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        OnMouseHover += showTip;
        OnMouseLoseFocus += hideTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= showTip;
        OnMouseLoseFocus -= hideTip;
    }

    private void showTip(string tip, Vector2 mousePos)
    {
        tipText.text = tip;
        tipWindow.sizeDelta = new Vector2(width,height);//max width of 200
        tipWindow.gameObject.SetActive(true);
        //float xOffset = (tipWindow.sizeDelta.x / 5 > 10 ? 10 : tipWindow.sizeDelta.x / 5) * direction.x;
        //float yOffset = (tipWindow.sizeDelta.y / 5 > 10 ? 10 : tipWindow.sizeDelta.y / 5) * direction.y;
        tipWindow.transform.position = new Vector2(mousePos.x, 60+height/2);
    }

    private void hideTip()
    {
        // tipText.text = default;
        tipWindow.gameObject.SetActive(false);
    }
}
