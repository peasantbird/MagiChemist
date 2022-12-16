using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Message : MonoBehaviour
{
    // Start is called before the first frame update

    public float messageAppearTime;
    public Color battleMsgColor;
    public Color itemMessageColor;
    public Color eventMessageColor;
    public Color miscMessageColor;
    private string messageString;
    private TextMeshProUGUI tmPro;
    private bool messageDestroying;
    private List<Color> colorList;
    void Start()
    {
        messageDestroying = false;
        tmPro = GetComponent<TextMeshProUGUI>();
        colorList = new List<Color>();
        colorList.Add(battleMsgColor);//0
        colorList.Add(itemMessageColor);//1
        colorList.Add(eventMessageColor);//2
        colorList.Add(miscMessageColor);//3
    }

    // Update is called once per frame
    void Update()
    {
        if (!tmPro.text.Trim().Equals("") && !messageDestroying) {
           // StartCoroutine(DestroyMessage());
        }
    }

    public void PushMessage(string msg,int eventType) {
        tmPro.text +=  "<color=#"+ColorUtility.ToHtmlStringRGB(colorList[eventType])+ ">"+msg+"</color>"+"\n";
       // if (!messageDestroying) {
            //StopCoroutine(DestroyMessage());
            StartCoroutine(DestroyMessage());
       // }
    }

    IEnumerator DestroyMessage() {
      //  messageDestroying = true;
        yield return new WaitForSeconds(messageAppearTime);
        tmPro.text = tmPro.text.Remove(0,tmPro.text.IndexOf("\n")+1);
       // messageDestroying = false;
    }
}
