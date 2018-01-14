using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateOptionsOnEnable : MonoBehaviour {

    public XMLDeadline xml;

	void OnEnable()
    {
        xml.UpdateEditDropdownValues();
    }

    public void UpdateEditScreenThing()
    {
        xml.LoadDeadlineEdit(GetComponent<Dropdown>().captionText.text);
    }
}
