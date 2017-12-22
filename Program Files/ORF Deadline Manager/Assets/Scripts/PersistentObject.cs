using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentObject : MonoBehaviour {

    static public PersistentObject persitentObject;
    public bool sortByTeam = false;
   
    //Saves sortByTeam when deadlines are created, deleted, or sorted differently. 
    void Start()
    {
        persitentObject = (persitentObject == null) ? this : persitentObject;
    }
}
