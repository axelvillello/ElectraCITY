//Name: Tutorial System
//Description: Defines the logic of the tutorial
//Author: Axel Ello

using System.Collections;
using System.Numerics;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject tutorialCursor;
    private bool isBobbing;
    public GameObject tutGenerator;
    public int currentDialogue = 0;
    public int dialogueCounter = 0;

    void Start()
    {
        isBobbing = false;

        tutorialCursor.SetActive(false);

        tutGenerator = FindClosestGenerator(tutorialCursor.transform.position);

    }

    void Update()
    {
        if (dialogueCounter != currentDialogue)  //Linearly manipulates objects based on progress through dialogue
        {
            switch (dialogueCounter)
            {
                case 5:
                    MoveCursor(GameObject.Find("B-Wire"));
                    tutorialCursor.SetActive(true);
                    break;
                case 8:
                    MoveCursor(GameObject.Find("R-Wire"));
                    break;
                case 10:
                    MoveCursor(GameObject.Find("S-Wire"));
                    break;
                case 12:
                    tutorialCursor.SetActive(false);
                    break;
                case 13:
                    MoveCursor(GameObject.Find("B-Wire"));
                    tutorialCursor.SetActive(true);
                    break;
                case 14:
                    tutorialCursor.transform.position = tutGenerator.transform.position + new UnityEngine.Vector3(0, 200, 0);
                    break;
                case 15:
                    tutorialCursor.SetActive(false);
                    break;
                case 26:
                    tutorialCursor.transform.position = tutGenerator.transform.position + new UnityEngine.Vector3(0, 200, 0);
                    tutorialCursor.SetActive(true);
                    break;
                case 27:
                    MoveCursor(GameObject.Find("R-Wire"));
                    break;
                case 30:
                    tutorialCursor.SetActive(false);
                    break;
                default:
                    break;
            }

            currentDialogue = dialogueCounter;
        }

        if (tutorialCursor.activeSelf == true && !isBobbing)
            {
                StartCoroutine(IconBounceDelay());
            }
    }

    private void MoveCursor(GameObject target)
    { 
        float width = target.GetComponent<RectTransform>().rect.width;
        tutorialCursor.transform.position = target.transform.position + new UnityEngine.Vector3(width, 200, 0);
    }

    GameObject FindClosestGenerator(UnityEngine.Vector3 currentPosition)
    {
        GameObject[] generators = GameObject.FindGameObjectsWithTag("Generator");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject generator in generators)
        {
            float dist = UnityEngine.Vector3.Distance(currentPosition, generator.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = generator;
            }
        }

            closest.GetComponent<Generators>().setTutorialObjectStatus(true);

        return closest;
}


    private IEnumerator IconBounceDelay()
    {
        isBobbing = true;

        tutorialCursor.transform.position = tutorialCursor.transform.position + new UnityEngine.Vector3(0, 20, 0);
        yield return new WaitForSeconds(0.5f);

        tutorialCursor.transform.position = tutorialCursor.transform.position - new UnityEngine.Vector3(0, 20, 0);
        yield return new WaitForSeconds(0.5f);

        isBobbing = false;
    }

}
