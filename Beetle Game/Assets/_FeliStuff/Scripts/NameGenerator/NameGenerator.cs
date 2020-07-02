using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NameGenerator : MonoBehaviour
{
    public TextAsset FirstNameList;
    public TextAsset LastNameList;
    private List<string> FirstNames;
    private List<string> LastNames;

    private void Start()
    {
        FirstNames = NamesInFile(FirstNameList);
        LastNames = NamesInFile(LastNameList);
    }

    private List<string> NamesInFile(TextAsset textFile)
    {
        //FirstNameList = Resources.Load("_FeliStuff/FirstNames") as TextAsset;
        //LastNameList = Resources.Load("_FeliStuff/LastNames") as TextAsset;
        List<string> nameList = new List<string>();
        string[] names = textFile.text.Split('\n');
        
        for (int i = 0; i < names.Length; i++)
        {
            nameList.Add(names[i]);
        }
        return nameList;
    }

    public string NewBugName()
    {
        string firstName = FirstNames[Random.Range(0, FirstNames.Count)];
        string lastName = LastNames[Random.Range(0, LastNames.Count)];
        return firstName + " " + lastName;
    }
}
