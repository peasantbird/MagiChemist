using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EquipInfo : MonoBehaviour
{
    private TextMeshProUGUI tmPro;
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        tmPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.selectedItem == null)
        {
            tmPro.text = "";
        }
        else
        {
            tmPro.text = player.selectedItem.itemType.ToString();
        }
    }
}
