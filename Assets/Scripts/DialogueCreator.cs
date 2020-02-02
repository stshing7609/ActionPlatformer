using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DialogueCreator : MonoBehaviour
{
    private const string dialogueDataFileName = "data_dialogues.json";

    DialogueData[] allDialogueData;

    public GameObject dialogueBox;              // dialogue box prefab

    // Start is called before the first frame update
    void Start()
    {
        InitDialogue();
    }

    void InitDialogue()
    {
        GameObject instance = Instantiate(dialogueBox, transform);
        instance.GetComponent<DialogueManager>().Init();
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
