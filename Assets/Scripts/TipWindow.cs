using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TipWindow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tipToShow;
    public string title;
    public float timeToWait;
    //public Vector2 appearDirection;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(startTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        TipWindowManager.OnMouseLoseFocus();
    }

    private void showMessage()
    {

        TipWindowManager.OnMouseHover(title + "\n\n" +tipToShow, Input.mousePosition);
    }

    private IEnumerator startTimer()
    {
        yield return new WaitForSeconds(timeToWait);
        showMessage();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
