﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnnouncementCreatorScript : MonoBehaviour
{
    public Text Announcement;
    public InputField AnnouncementInput;
    public Text AnnouncementDisplay;
    
    //Saves announcement to splashMessage.txt, is later parsed and displayed
    public void DisplayAnnouncement()
    {
        AnnouncementDisplay.text = Announcement.text;
        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(Application.dataPath + "/StreamingAssets/splashMessage.txt"))
        {
            try
            {
                writer.Write(Announcement.text);
                AnnouncementInput.text = "";
            }
            catch
            {
                Debug.LogWarning("Could not write message to splashMessage.txt!");
            }
            
        }
        
    }
}
