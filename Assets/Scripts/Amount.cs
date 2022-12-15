using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Amount : MonoBehaviour
{
    private TextMeshProUGUI tmPro;
    // Start is called before the first frame update
    void Start()
    {
        tmPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.GetComponentInChildren<Item>() == null)
        {
            tmPro.text = "";
        }
        else {
            tmPro.text = transform.parent.GetComponentInChildren<Item>().amount+"";
        }
    }
}
