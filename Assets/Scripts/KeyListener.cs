using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyListener : MonoBehaviour
{
    private TextMeshProUGUI tmPro;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        tmPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //  if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Q)) {
        //if (transform.parent.GetComponentInChildren<Item>() !=null)
        //{
        //    if (playerController.selectedItem.itemType != transform.parent.GetComponentInChildren<Item>().itemType)
        //    {
        //        playerController.selectedItem.transform.parent.parent.Find("Filler").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        //    }

        //}
        //else {
        //    playerController.selectedItem.transform.parent.parent.Find("Filler").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        //}
        // }
        if (transform.parent.GetComponentInChildren<Item>() != playerController.selectedItem || !HasItem())
        {
            transform.parent.parent.Find("Filler").GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else {
            transform.parent.parent.Find("Filler").GetComponent<Image>().color = new Color32(184, 219, 255, 255);
        }


        if (Input.GetKeyDown(KeyCode.Alpha1)) {
        
            if (tmPro.text.Trim() == "1" && HasItem()) {
                UpdatePlayerSelection();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
          
            if (tmPro.text.Trim() == "2" && HasItem()) {
                UpdatePlayerSelection();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
          
            if (tmPro.text.Trim() == "3" && HasItem())
            {
                UpdatePlayerSelection();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
       
            if (tmPro.text.Trim() == "4" && HasItem())
            {
                UpdatePlayerSelection();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
     
            if (tmPro.text.Trim() == "5" && HasItem())
            {
                UpdatePlayerSelection();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
          
            if (tmPro.text.Trim() == "6" && HasItem())
            {
                UpdatePlayerSelection();
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
        
            if (tmPro.text.Trim() == "Q")
            {
                UpdatePlayerSelection();
            }
        }

    }

    private void UpdatePlayerSelection() {
        if (playerController.selectedItem == transform.parent.GetComponentInChildren<Item>())
        {
            playerController.selectedItem = null;
            playerController.RemoveRange();
        }
        else
        {
            playerController.selectedItem = transform.parent.GetComponentInChildren<Item>();

            playerController.spellRange = transform.parent.GetComponentInChildren<Item>().itemRange;
            playerController.RemoveRange();
            playerController.SpawnRange();
        }
    }

    private bool HasItem() {
        return transform.parent.GetComponentInChildren<Item>() != null;
    }
}
