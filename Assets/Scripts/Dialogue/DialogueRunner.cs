using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueRunner : MonoBehaviour
{
    DialogueData dialogueData;
    
    public TextMeshProUGUI textMesh;            // the text for dialogue
    public GameObject arrow;

    private Queue<string> dialogueQueue;        // an array of string queues where we'll hold each speaker's dialogue sets
    private bool typing;                        // are we currently typing
    private string currSentence;                // store the current sentence
    bool triggerWin = false;

    public void Init(DialogueData data, bool win)
    {
        StopAllCoroutines();
        dialogueQueue = new Queue<string>();
        dialogueData = data;
        triggerWin = win;
        StartDialogue();
    }

    public void Init(DialogueData data)
    {
        Init(data, false);
    }

    void Update()
    {
        // check if the player clicks to advance dialogue
        if (Input.GetMouseButtonDown(0))
        {
           
            // if we're not typing, start typing a new sentence
            if (!typing)
                TypeNewSentence();
            // if we are typing, then the player wants to display the entire text, so instantly display the whole thing
            else
                DisplayWholeSentence();
        }
    }

    public void StartDialogue()
    {
        dialogueQueue.Clear();

        for (int i = 0; i < dialogueData.lines.Length; i++)
        {
            dialogueQueue.Enqueue(dialogueData.lines[i]);
        }

        TypeNewSentence();
    }

    // types out a new sentence
    void TypeNewSentence()
    {
        // if our queue is empty, the sequence is over
        if (IsLastSentence())
        {
            EndDialogue();
            return;
        }

        // enqueue text
        typing = true;                          // we are now typing
        currSentence = dialogueQueue.Dequeue();     // remove the first sentence in the queue and set it to our current sentence

        StopAllCoroutines();                    // stop all coroutines in case we force advance before any coroutines finish
        StartCoroutine(TypeText());             // start the text typing coroutine
    }

    // displays the whole sentence
    void DisplayWholeSentence()
    {
        StopAllCoroutines();                                        // stop all coroutines as the player force advanced past them
        typing = false;                                             // we're not typing out now
        textMesh.text = currSentence;    // reveal the entire sentence
        arrow.SetActive(true);

        StartCoroutine(Wait(2f));                                 // wait for 3 seconds before starting the next sentence
    }

    // Coroutine for typing out text letter by letter
    IEnumerator TypeText()
    {
        textMesh.text = "";              // set text of the current textArea in the pattern to empty
        arrow.SetActive(false);

        // reveal each character letter by letter, one per frame
        foreach (char letter in currSentence.ToCharArray())
        {
            textMesh.text += letter;
            yield return null;
        }

        // do some things after the coroutine finishes
        typing = false;     // we are not typing anymore
        arrow.SetActive(true);

        // if this is our last sentence, wait longer so the player can read
        if (IsLastSentence())
        {
            StartCoroutine(Wait(3));
        }
        // otherwise, wait 2 seconds
        else
        {
            StartCoroutine(Wait(2));
        }
    }

    // coroutine to delay dialogue advancing - param is the time in seconds that we want to wait for
    IEnumerator Wait(float timeInSeconds)
    {
        yield return new WaitForSeconds(timeInSeconds); // wait for real time seconds

        TypeNewSentence();                                      // start the next sentence
    }

    // End the dialogue
    void EndDialogue()
    {
        if (triggerWin)
            VictoryTracker.Instance.TriggerWin();
        Destroy(gameObject);
    }

    bool IsLastSentence()
    {
        // if we passed the check in the loop, then we reached our last sentence
        return dialogueQueue.Count == 0;
    }
}
