using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeadlineCreation : MonoBehaviour
{
    //Toggles sub-menus of deadline manager
    public GameObject Panel;
    public GameObject Panel2;
    public GameObject Panel3;
    public GameObject Panel4;

    public void showhidePanel()
    {
           
            Panel.gameObject.SetActive(!Panel.gameObject.activeSelf);
    }

    public void showhideDeadlines()
    {
       
        Panel2.gameObject.SetActive(!Panel2.gameObject.activeSelf);
    }

    public void showhideAnnouncements()
    {
       
        Panel3.gameObject.SetActive(!Panel2.gameObject.activeSelf);
    }

    public void showhideEdit()
    {
        Panel4.gameObject.SetActive(!Panel2.gameObject.activeSelf);
    }
}
