using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

public class XMLDeadline : MonoBehaviour {

   public static XMLDeadline XML;

   public string titleToEdit;

   public InputField Title;
   public InputField Description;
   public InputField DueDate;
   public InputField Author;

   public InputField TitleEdit;
   public InputField DescriptionEdit;
   public InputField DueDateEdit;
   public InputField AuthorEdit;

   public Dropdown Team;
   public Dropdown DeadlineEdit;

   public Button SubmitButton;
   public Button SubmitEditsButton;
   public Button CompletedButton;
   
   public DeadlineDB deadlineDB;

   public GameObject ErrorObject;

    public void Start()
    {
        XML = this;
        LoadDeadline();
        LoadDeadlineEdit("title here");
    }


    //Saves information that's entered in textboxes on the "create a deadline" menu into DeadlineDB.xml
    public void SaveDeadline()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DeadlineDB));
        using (StreamWriter sw = new StreamWriter(Application.dataPath + "/StreamingAssets/XML/DeadlineDB.xml", false, Encoding.UTF8))
        {
            serializer.Serialize(sw, deadlineDB);
        }
      
        Title.text = "";
        Description.text = "";
        DueDate.text = "";
        Author.text = "";

    }

    //Loads deadlines from DeadlineDB.xml
    public void LoadDeadline() {
        XmlSerializer serializer = new XmlSerializer(typeof(DeadlineDB));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/XML/DeadlineDB.xml", FileMode.Open);
        deadlineDB = serializer.Deserialize(stream) as DeadlineDB;
        stream.Close();
    }

    //Deletes deadlines from the DeadlineDB.xml, refreshes screen upon deletion
    public void DeleteDeadline(GameObject entry) 
    {
        string title = "";
        foreach (Text t in entry.GetComponentsInChildren<Text>())
            if (t.name.ToLower() == "project")
            {
                title = t.text;
                break;
            }

        foreach (DeadLineEntry ded in deadlineDB.DeadLineList) 
        {
            
            if (title.ToLower() == ded.Title.ToLower()) 
            {
                deadlineDB.DeadLineList.Remove(ded);
                SaveDeadline();
                SceneManager.LoadScene("Deadlines");
                break;
            }
        }
 
    }

    //Update the edit deadline dropdown list
    public void UpdateEditDropdownValues()
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        foreach(DeadLineEntry entry in deadlineDB.DeadLineList)
        {
            options.Add(new Dropdown.OptionData(entry.Title));
        }
        DeadlineEdit.ClearOptions();
        DeadlineEdit.options = options;
        DeadlineEdit.RefreshShownValue();
    }

    //Load deadline information into input fields
    public void LoadDeadlineEdit(string title)
    {
        foreach (DeadLineEntry entry in deadlineDB.DeadLineList)
        {
            if (title == entry.Title)
            {
                titleToEdit = title;
                TitleEdit.text = title;
                DescriptionEdit.text = entry.Description;
                DueDateEdit.text = entry.DueDate;
                AuthorEdit.text = entry.Author;
            }
        }
    }

    public bool EditDeadline(string oldTitle, string title, string desc, string due, string author, string team)
    {
        //Save edited deadline, and overwrite old deadline
        foreach (DeadLineEntry entry in deadlineDB.DeadLineList)
        {
            TitleEdit.text = "";
            DescriptionEdit.text = "";
            DueDateEdit.text = "";
            AuthorEdit.text = "";


            if (oldTitle.ToLower() == entry.Title.ToLower())
            {
                DeadLineEntry e = new DeadLineEntry(title, desc, due, author, team);
                deadlineDB.DeadLineList.Add(e);
                deadlineDB.DeadLineList.Remove(entry);
                SaveDeadline();
                return true;
            }
        }
        return false;
    } 


    //A validation system to make sure there aren't a lot of incorrectly entered deadlines. Most common errors when creating deadlines are 
    //1. Entering the date in the wrong format 
    //2. Using a title that already exists
    //Preventative messures have been put in to stop the deadline from being created if there is an error in the entry
    public void ValidateInput()
    {
        bool gotError = false;
        string errorMessage = "";
        foreach (DeadLineEntry entry in deadlineDB.DeadLineList)
        {
            if (entry.Title.ToLower() == Title.text.ToLower())
            {
                gotError = true;
                errorMessage += "A deadline with this title does already exist!\n";
            }
        }
        DateTime date;
        Debug.Log(DueDate.text.Trim());
        if (!DateTime.TryParseExact(DueDate.text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            gotError = true;
            errorMessage += "Due date has to be in the format of: \"MM/dd/yyyy\"!";
        }

        if (gotError)
        {
            StartCoroutine(showWarning(errorMessage));
        }
        else
        {
            DeadLineEntry entry = new DeadLineEntry(Title.text, Description.text, DueDate.text, Author.text, Team.captionText.text);
            deadlineDB.DeadLineList.Add(entry);
            SaveDeadline();
            SceneManager.LoadScene("Deadlines");
        }
    }

    public void ValidateEditInput()
    {
        bool gotError = false;
        string errorMessage = "";
        if (TitleEdit.text.ToLower() != titleToEdit.ToLower())
            foreach (DeadLineEntry entry in deadlineDB.DeadLineList)
            {
                if (entry.Title.ToLower() == TitleEdit.text.ToLower())
                {
                    gotError = true;
                    errorMessage += "A deadline with this title does already exist!\n";
                }
            }
        DateTime date;

        if (!DateTime.TryParseExact(DueDateEdit.text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            gotError = true;
            errorMessage += "Due date has to be in the format of: \"MM/dd/yyyy\"!";
        }

        if (gotError)
        {
            StartCoroutine(showWarning(errorMessage));
        }
        else
        {
            if (!EditDeadline(titleToEdit, TitleEdit.text, DescriptionEdit.text, DueDateEdit.text, AuthorEdit.text, Team.captionText.text))
            {
                StartCoroutine(showWarning("No deadline has a matching title"));
            }
            else
            {
                SceneManager.LoadScene("Deadlines");
            }
        }
    }


    //Displays error message from ValidateInput()
    IEnumerator showWarning(string errorMessage)
    {
        ErrorObject.GetComponentInChildren<Text>().text = errorMessage;
        ErrorObject.SetActive(true);
        yield return new WaitForSeconds(5);
        ErrorObject.SetActive(false);
    }


    [System.Serializable]
    public class DeadLineEntry
    {
        public DeadLineEntry()
        {
            Title = "";
            Description = "";
            DueDate = "";
            Author = "";
            Team = "";
        }

        public DeadLineEntry(string title, string desc, string due, string author, string team)
        {
            Title = title;
            Description = desc;
            DueDate = due;
            Author = author;
            Team = team;
        }

        public string
        Author,
        Title,
        DueDate,
        Description,
        Team;
    }

   
    [System.Serializable]
    public class DeadlineDB  {
        public List<DeadLineEntry> DeadLineList = new List<DeadLineEntry>();
     
    }

}
