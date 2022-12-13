using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    private TextMeshProUGUI tmPro;
    public bool debug;
    // Start is called before the first frame update
    void Start()
    {
        // tmPro = transform.Find("Canvas").transform.Find("Text").transform.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            tmPro.text = "(" + transform.position.x + "," + transform.position.y + ")";
        }
    }

    public void ShowText(string s) {
        tmPro = transform.Find("Canvas").transform.Find("Text").transform.GetComponent<TextMeshProUGUI>();
        tmPro.text = s;
    }
}
