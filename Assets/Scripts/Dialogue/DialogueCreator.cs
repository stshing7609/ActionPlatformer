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
    public Transform dialogueHolder;

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

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            StartCoroutine(LoadDataFromJSONWeb(dialogueDataFileName));
        }
        else
            LoadDataFromJSON(dialogueDataFileName);
    }

    public void InitDialogue(int id, bool win)
    {
        DialogueData data = allDialogueData[id];

        // only allow one instance of the dialogue box
        GameObject instance;
        if (dialogueHolder.childCount > 0)
            instance = dialogueHolder.GetChild(0).gameObject;
        else
            instance = Instantiate(dialogueBox, dialogueHolder);

        instance.GetComponent<DialogueRunner>().Init(data, win);
    }

    public void InitDialogue(int id)
    {
        InitDialogue(id, false);
    }

    private void LoadDataFromJSON(string fileName)
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string dataAsJson;

        if (File.Exists(filePath))
        {
            dataAsJson = File.ReadAllText(filePath);

            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            AllDialogueData loadedData = JsonUtility.FromJson<AllDialogueData>(dataAsJson);

            // Retrieve the allChapterData property of loadedData
            allDialogueData = loadedData.allDialogueData;
        }
        else
            Debug.Log("Cannot find file called " + filePath);
    }

    //private void LoadDataFromJSON(string fileName)
    //{
    //    TextAsset file = Resources.Load<TextAsset>(fileName);
    //    string dataAsJson = file.text;

    //    AllDialogueData loadedData = JsonUtility.FromJson<AllDialogueData>(dataAsJson);

    //    // Retrieve the allChapterData property of loadedData
    //    allDialogueData = loadedData.allDialogueData;
    //}

    IEnumerator LoadDataFromJSONWeb(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        string dataAsJson;

        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();
            dataAsJson = www.downloadHandler.text;
        }
        else
            dataAsJson = File.ReadAllText(filePath);

        // Pass the json to JsonUtility, and tell it to create a GameData object from it
        AllDialogueData loadedData = JsonUtility.FromJson<AllDialogueData>(dataAsJson);

        // Retrieve the allChapterData property of loadedData
        allDialogueData = loadedData.allDialogueData;
    }
}
