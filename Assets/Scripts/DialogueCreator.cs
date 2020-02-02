using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DialogueCreator : MonoBehaviour
{
    private static DialogueCreator instance;
    public static DialogueCreator Instance { get => instance; }

    private const string dialogueDataFileName = "data_dialogues.json";

    DialogueData[] allDialogueData;

    public GameObject dialogueBox;              // dialogue box prefab

    private void Awake()
    {
        //Establish Singleton Pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadDataFromJSON(dialogueDataFileName);
    }

    public void InitDialogue(int id)
    {
        DialogueData data = allDialogueData[id];

        // only allow one instance of the dialogue box
        GameObject instance;
        if(transform.childCount > 0)
            instance = transform.GetChild(0).gameObject;
        else
            instance = Instantiate(dialogueBox, transform);

        instance.GetComponent<DialogueRunner>().Init(data);
    }

    private void LoadDataFromJSON(string fileName)
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string dataAsJson = File.ReadAllText(filePath);

        // Pass the json to JsonUtility, and tell it to create a GameData object from it
        AllDialogueData loadedData = JsonUtility.FromJson<AllDialogueData>(dataAsJson);

        // Retrieve the allChapterData property of loadedData
        allDialogueData = loadedData.allDialogueData;
    }
}
