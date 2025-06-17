//Name: Dialogue System
//Description: Definiton for dialogue objects that populate text boxes. Intended for tutorial usage
//Author: Axel Ello

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Numerics;

public class Dialogue : MonoBehaviour
{
    public LinkedList<DialogueStep> dialogueList;
    private LinkedListNode<DialogueStep> currentNode;
    [SerializeField] private TextAsset jsonfile;
    [SerializeField] private Transform dialogueBox;
    [SerializeField] private GameObject clickableIndicator;
    private TextMeshProUGUI messageContent;
    private bool isBobbing;

    void Start()
    {
        isBobbing = false;

        messageContent = dialogueBox.Find("TutorialMessage").GetComponent<TextMeshProUGUI>();

        dialogueList = new LinkedList<DialogueStep>();
        DialogueSteps readDialogueFile = JsonUtility.FromJson<DialogueSteps>(jsonfile.text);

        foreach (DialogueStep readLine in readDialogueFile.dialogue)
        {
            dialogueList.AddLast(readLine);
        }

        foreach (DialogueStep n in dialogueList)    //test if LinkedList works
        {
            Debug.Log(n.content);
        }

        currentNode = dialogueList.First;
        StartCoroutine(TypeText(currentNode.Value.content));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentNode.Value.nextStepReady == true && currentNode.Value.isClickable == true)
            {
                if (currentNode.Next != null)
                {
                    clickableIndicator.SetActive(false);
                    currentNode = currentNode.Next;
                    StartCoroutine(TypeText(currentNode.Value.content));
                }
                else
                {
                    Debug.Log("End of dialogue!");
                }
            }
        }

        if (clickableIndicator.activeSelf == true && !isBobbing)
        {
            StartCoroutine(IconBounceDelay());
        }
    }

    private IEnumerator TypeText(string line)   //Typewriter effect for dialogue messages
    {
        float delay = 0.03f;
        messageContent.text = "";
        foreach (char letter in line.ToCharArray())
        {
            messageContent.text += letter;
            yield return new WaitForSeconds(delay);
        }

        currentNode.Value.nextStepReady = true;
        clickableIndicator.SetActive(true);

    }

    private IEnumerator IconBounceDelay()
    {
        isBobbing = true;

        clickableIndicator.transform.position = clickableIndicator.transform.position + new UnityEngine.Vector3(0, 10, 0);
        yield return new WaitForSeconds(0.5f);

        clickableIndicator.transform.position = clickableIndicator.transform.position - new UnityEngine.Vector3(0, 10, 0);
        yield return new WaitForSeconds(0.5f);

        isBobbing = false;
    }
}

[Serializable]
public class DialogueStep
{
    public string content;
    public int retraceSteps = 0;     //If > 0, revert that many steps in the dialogue list
    public bool nextStepReady = false;     //Flag for when to point traverse to the next dialogue step
    public bool isClickable = true;       //Flags when a user can click to traverse dialogue (may not be needed)

}

[Serializable]
public class DialogueSteps
{
    public DialogueStep[] dialogue;
}
