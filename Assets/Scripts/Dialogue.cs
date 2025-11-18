//Name: Dialogue System
//Description: Definiton for dialogue objects that populate text boxes. Intended for tutorial usage
//             and treats each line of dialogue as a "step" 

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public LinkedList<DialogueStep> dialogueList;
    private LinkedListNode<DialogueStep> currentNode;
    [SerializeField] private TextAsset jsonfile;
    [SerializeField] private Transform dialogueBox;
    [SerializeField] private GameObject clickableIndicator;
    [SerializeField] private GameObject blackWire;
    private TextMeshProUGUI messageContent;
    private bool isBobbing;
    private Tutorial tutorialSystem;
    private StaticValues staticValues;
    private GameObject tutorialBuilding;
    private List<GameObject> tutorialConnections = new List<GameObject>();

    void Start()
    {
        tutorialSystem = GameObject.Find("Tutorial").GetComponent<Tutorial>();
        staticValues = GameObject.FindGameObjectWithTag("StaticValues").GetComponent<StaticValues>();
        isBobbing = false;

        messageContent = dialogueBox.Find("TutorialMessage").GetComponent<TextMeshProUGUI>();

        dialogueList = new LinkedList<DialogueStep>();
        DialogueSteps readDialogueFile = JsonUtility.FromJson<DialogueSteps>(jsonfile.text);

        foreach (DialogueStep readLine in readDialogueFile.dialogue)
        {
            dialogueList.AddLast(readLine);
        }

        foreach (DialogueStep n in dialogueList)    //test if LinkedList was populated
        {
            Debug.Log(n.content);
        }

        currentNode = dialogueList.First;
        StartCoroutine(TypeText(currentNode.Value.content));
    }

    void Update()
    {
        CheckDialogueBehaviour();
        
        //Progress dialogue if mouse button is clicked or if the node flags progress to next dialogue step
        if (Input.GetMouseButtonDown(0) || (currentNode.Value.nextStepReady == true))
        {
            if (currentNode.Value.isClickable == true)
            {
                if (currentNode.Next != null)
                {
                    clickableIndicator.SetActive(false);
                    currentNode = currentNode.Next;
                    StopAllCoroutines();
                    StartCoroutine(TypeText(currentNode.Value.content));

                    isBobbing = false;

                    tutorialSystem.dialogueCounter += 1;
                }
                else
                {
                    Debug.Log("End of dialogue!");
                }
            }
        }

        //Applies an animation to the click icon
        if (clickableIndicator.activeSelf == true && !isBobbing)
            {
                StartCoroutine(IconBounceDelay());
            }
    }

    //Determines behaviour of dialogue at certain steps
    private void CheckDialogueBehaviour()
    {   
        switch (tutorialSystem.dialogueCounter)
        {
            case 13: //Progresses when user clicks the blue wire
                if (blackWire.GetComponent<WireAttach>().selected == false)
                {
                    currentNode.Value.isClickable = false;
                }
                else
                {
                    currentNode.Value.nextStepReady = true;
                    currentNode.Value.isClickable = true;
                }

                break;

            case 14: //Progresses when user connects a wire into the generator
                bool genSelected = false;

                for (int i = 0; i < tutorialSystem.tutGenerator.GetComponent<ConnectorGen>().Connectors.Length; i++)
                {
                    if (tutorialSystem.tutGenerator.GetComponent<ConnectorGen>().Connectors[i].GetComponent<WireConnection>().selected == true)
                    {
                        genSelected = true;
                    }
                }

                if (genSelected == false)
                {
                    currentNode.Value.isClickable = false;
                }
                else
                {
                    currentNode.Value.nextStepReady = true;
                    currentNode.Value.isClickable = true;
                }

                break;

            case 15: //Progresses when user connects a wire between the generator and a consumer
                bool genConnected = false;

                for (int i = 0; i < tutorialSystem.tutGenerator.GetComponent<ConnectorGen>().Connectors.Length; i++)
                {
                    if (tutorialSystem.tutGenerator.GetComponent<ConnectorGen>().Connectors[i].GetComponent<WireConnection>().otherConnector)
                    {
                        genConnected = true;
                        tutorialBuilding = tutorialSystem.tutGenerator.GetComponent<ConnectorGen>().Connectors[i].GetComponent<WireConnection>().otherConnector.GetComponent<WireConnection>().getParent();
                        tutorialConnections.Add(tutorialBuilding);
                    }
                }

                if (genConnected == false)
                {
                    currentNode.Value.isClickable = false;
                }
                else
                {
                    currentNode.Value.nextStepReady = true;
                    currentNode.Value.isClickable = true;
                }

                break;

            case 18: //Progresses when a wire is connected between a building and another
                bool buildingConnected = false;

                for (int i = 0; i < tutorialBuilding.GetComponentInParent<ConnectorGen>().Connectors.Length; i++)
                {
                    if (tutorialBuilding.GetComponentInParent<ConnectorGen>().Connectors[i].GetComponent<WireConnection>().otherConnector)
                    {
                        Transform[] parents = tutorialBuilding.GetComponentInParent<ConnectorGen>().Connectors[i].GetComponent<WireConnection>().otherConnector.GetComponent<WireConnection>().getParent().GetComponentsInParent<Transform>();

                        foreach (Transform p in parents)
                        {
                            if (p.CompareTag("Consumer"))
                            {
                                buildingConnected = true;
                            }
                        }
                        tutorialConnections.Add(tutorialBuilding.GetComponentInParent<ConnectorGen>().Connectors[i].GetComponent<WireConnection>().otherConnector.GetComponent<WireConnection>().getParent());
                    }
                }
                
                if (buildingConnected == false)
                {
                    currentNode.Value.isClickable = false;
                }
                else
                {
                    currentNode.Value.nextStepReady = true;
                    currentNode.Value.isClickable = true;
                }

                break;

            /*
            //Depreciated step that progresses when a powerline is created and a building has not received power

            case 21:
                
                bool unpoweredBuilding = true; //set to true for testing 

                PopulateTutorialConsumers(tutorialBuilding);

                if (tutorialConnections.Count == 0)
                    { Debug.Log("tutorialConnections empty!"); }

                foreach (GameObject c in tutorialConnections)
                {
                    var connGen = c.GetComponentInParent<ConnectorGen>();

                    if (connGen == null || connGen.Connectors == null)
                        continue;

                    foreach (var connector in connGen.Connectors)
                    {
                        var wireConnection = connector?.GetComponent<WireConnection>();
                        if (wireConnection == null || wireConnection.otherConnector == null) continue;

                        var otherCon = wireConnection?.otherConnector;
                        var otherWire = otherCon?.GetComponent<WireConnection>();
                        var parent = otherWire?.getParent();
                        var consumer = parent?.GetComponent<Consumers>();

                        if (consumer != null && consumer.isPowerOn() == false)
                        {
                            unpoweredBuilding = true;
                            Debug.Log("Building unpowered!");

                        }

                    }
                    
                }

                if (unpoweredBuilding == false)
                {
                    currentNode.Value.isClickable = false;
                }
                else
                {
                    currentNode.Value.nextStepReady = true;
                    currentNode.Value.isClickable = true;
                }

                break;
            */

            case 28: //End of current JSON file for dialogue, dialogue box becomes inactive in the scene
                dialogueBox.gameObject.SetActive(false);
                break;

            default:
                currentNode.Value.isClickable = true;
                break;
            }
        
    }

    //Unused function for reverting to a previous step of dialogue
    public void DialogueRetrace(int reSteps)
    {
        for (int i = 0; i < reSteps; i++)
        {
            currentNode = currentNode.Previous;
            tutorialSystem.dialogueCounter -= 1;
        }

        StartCoroutine(TypeText(currentNode.Value.content));
    }

    //Finds all consumer objects needed for the tutorial
    private void PopulateTutorialConsumers(GameObject building)
    {
        GameObject currentBuilding;

        for (int i = 0; i < building.GetComponentInParent<ConnectorGen>().Connectors.Length; i++)
        {
            if (building.GetComponentInParent<ConnectorGen>().Connectors[i].GetComponent<WireConnection>() && building.GetComponentInParent<ConnectorGen>().Connectors[i].GetComponent<WireConnection>().otherConnector)
            {
                if (building.GetComponentInParent<ConnectorGen>().Connectors[i].GetComponent<WireConnection>().otherConnector.GetComponent<WireConnection>().getParent() != null)
                {
                    currentBuilding = building.GetComponentInParent<ConnectorGen>().Connectors[i].GetComponent<WireConnection>().otherConnector.GetComponent<WireConnection>().getParent();

                    if (!tutorialConnections.Contains(currentBuilding))
                    {
                        tutorialConnections.Add(currentBuilding);
                        PopulateTutorialConsumers(currentBuilding);
                    }
                }
            }
        }

        return;
    }

    //Typewriter effect for dialogue messages
    private IEnumerator TypeText(string line)   
    {
        float delay = 0.1f / staticValues.textSpd; //Speed of text typewriting 

        messageContent.text = line;
        LayoutRebuilder.ForceRebuildLayoutImmediate(dialogueBox.GetComponent<RectTransform>());
        messageContent.text = "";
        
        foreach (char letter in line.ToCharArray())
        {
            messageContent.text += letter;
            yield return new WaitForSeconds(delay);
        }

        if (currentNode.Value.isClickable == true) //Displays click icon when dialogue step is fully "typed out"
            clickableIndicator.SetActive(true);

    }

    //A bobbing animation for the click icon
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
    public int id;
    public string content;
    public int retraceSteps = 0;     //If > 0, revert that many steps in the dialogue list
    public bool nextStepReady = false;     //Flags when to automatically traverse to dialogue
    public bool isClickable = false;      //Flags when a user can click to traverse dialogue 

}

[Serializable]
public class DialogueSteps
{
    public DialogueStep[] dialogue;
}
