using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class UIManagerScript : MonoBehaviour {

    //Loads the deadline entry list
    public void LoadDeadLines()
    {
        SceneManager.LoadScene("Deadlines");
    }

    //Loads main menu on laptop screen
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //Hides and shows buttons
    public void ToggleButtonGroup(GameObject buttonGroup)
    {
        buttonGroup.SetActive(!buttonGroup.activeSelf);
    }

    //Enables buttons
    public void EnableButtonGroup(GameObject buttonGroup)
    {
        buttonGroup.SetActive(true);
        SceneManager.LoadScene("Deadlines");
    }

    //Disables buttons
    public void DisableButtonGroup(GameObject buttonGroup)
    {
        buttonGroup.SetActive(false);
        Debug.Log("Disabling group: " + buttonGroup.name);
    }

    //Sorts deadline entry list by subteam instead of by dates
    public void SortByTeam(bool sortByTeam)
    {
        PersistentObject.persitentObject.sortByTeam = sortByTeam;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
