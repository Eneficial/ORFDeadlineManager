using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementResizeManager : MonoBehaviour {

    //This entire class is to resize deadline manager elements so it works on any display
    public RectTransform tParent;
    void OnRectTransformDimensionsChange()
    {
        Resize();
    }

    public void Resize()
    {
        if (tParent == null)
        {
            Debug.LogError("Parent not found");
            return;
        }
        if (GetComponent<RectTransform>().sizeDelta.y > tParent.sizeDelta.y)
        {
            float diff = Mathf.Abs(GetComponent<RectTransform>().sizeDelta.y - tParent.sizeDelta.y);
            tParent.sizeDelta = new Vector2(tParent.sizeDelta.x, GetComponent<RectTransform>().sizeDelta.y);
            tParent.transform.localPosition -= new Vector3(0,diff + GameObject.FindGameObjectWithTag("MainCamera").GetComponent<XMLDeadlineDisplay>().cellMargin);
        }
    }

}
