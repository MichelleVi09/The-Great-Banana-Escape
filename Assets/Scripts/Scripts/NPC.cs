using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Controller;   // for WaypointPatrol

public class NPC : MonoBehaviour
{
    [Header("Dialogue")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;

    [TextArea] public string[] dialogue;              //1st dialogue for before quest
    [TextArea] public string[] completedDialogue;     //2nd dialogue for after quest
    [TextArea] public string[] postPatrolDialogue;    //3rd quest after walking player

    public GameObject contButton;
    public float wordSpeed = 0.05f;

    [Header("Instruction UI")]
    [SerializeField] private GameObject instructionPanel;
    private bool hasShownInstructionPanel = false;

    [Header("Player Proximity")]
    public bool playerIsClose;

    [Header("Quest Settings")]
    [SerializeField] private InventoryManager inventory;
    [SerializeField] private ItemClass seedItem;
    [SerializeField] private int seedsRequired = 10; //requires players to have at least 10 seeds

    [SerializeField] private ItemClass glassesItem; //drag glasses item 
    [SerializeField] private XRayController xrayController; //instance of xrayController

    [Header("Post-Quest Movement")]
    [SerializeField] private WaypointPatrol waypointPatrol;   //drag chicken's WaypointPatrol 

    private int index = 0;
    private string[] currentDialogue;
    private bool seedsTurnedIn = false; //keeps track of if seeds are turned in
    private bool patrolStarted = false; //keeps track of patrol of chicke
    private bool patrolCompleted = false; //keeps track if patrol is done
    private bool glassesGiven = false; //tracks if glasses were given

    void Start()
    {
        //takes 1st dialogue and assigns it to currentDialogue
        currentDialogue = dialogue;

        if (dialogueText != null)
            dialogueText.text = "";

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (instructionPanel != null)
            instructionPanel.SetActive(false);

        //chicken should NOT move until we tell it to
        if (waypointPatrol != null)
            waypointPatrol.enabled = false;
    }

    void Update()
    {
        //will only fire if player is close and if player presses E
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            HandleInteract();
        }

        //closes dialogue with q
        if (Input.GetKeyDown(KeyCode.Q) && dialoguePanel.activeInHierarchy)
        {
            RemoveText();
        }

        //detect when patrol finishes
        if (patrolStarted && !patrolCompleted && waypointPatrol != null && !waypointPatrol.enabled)
        {
            patrolCompleted = true;
        }
    }

    private void HandleInteract()
    {
        //only runs once, before seedsTurnedIn is true 
        if (!seedsTurnedIn && inventory != null && seedItem != null)
        {
            SlotClass seedSlot = inventory.Contains(seedItem);
            int currentSeeds = (seedSlot != null) ? seedSlot.GetQuantity() : 0;

            if (currentSeeds >= seedsRequired)
            {
                //remove required seeds
                for (int i = 0; i < seedsRequired; i++)
                {
                    inventory.Remove(seedItem);
                }

                //remove glasses 
                if (glassesItem != null)
                {
                    inventory.Remove(glassesItem);
                }

                //disables xRay
                if (xrayController != null)
                {
                    xrayController.DisableXRay();
                }

                //hides instruction panel
                if (instructionPanel != null)
                    instructionPanel.SetActive(false);

                seedsTurnedIn = true;
                index = 0;
                if (dialogueText != null)
                    dialogueText.text = "";
            }
        }

        //decides which dialogue set to use based on quest & patrol state
        if (!seedsTurnedIn)
        {
            //still in phase 1
            currentDialogue = dialogue;
        }
        else if (seedsTurnedIn && !patrolCompleted)
        {
            //phase 2: seeds are turned in, chicken hasn't finished walking yet
            currentDialogue = completedDialogue;
        }
        else
        {
            //phase 3: patrol finished? only show the post-patrol dialogue
            currentDialogue = postPatrolDialogue;
        }

        //open dialogue panel
        if (!dialoguePanel.activeInHierarchy)
        {
            dialoguePanel.SetActive(true);

            if (currentDialogue != null && currentDialogue.Length > 0)
            {
                StopAllCoroutines();
                StartCoroutine(Typing());
            }
            else
            {
                if (dialogueText != null)
                    dialogueText.text = "";
            }
        }
        else
        {
            if (currentDialogue != null &&
                currentDialogue.Length > 0 &&
                dialogueText.text == currentDialogue[index])
            {
                //goes to nextLine
                NextLine();
            }
        }
    }

    public void RemoveText()
    {
        if (dialogueText != null)
            dialogueText.text = "";

        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        if (currentDialogue == null || currentDialogue.Length == 0 || dialogueText == null)
            yield break;

        dialogueText.text = "";

        if (contButton != null) contButton.SetActive(false);

        foreach (char letter in currentDialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }

        if (contButton != null) contButton.SetActive(true);
    }

    public void NextLine()
    {
        if (currentDialogue == null || currentDialogue.Length == 0)
        {
            RemoveText();
            return;
        }

        if (index < currentDialogue.Length - 1)
        {
            index++;

            if (contButton != null) contButton.SetActive(false);
            StopAllCoroutines();
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            //finished the CURRENT dialogue
            if (!seedsTurnedIn && currentDialogue == dialogue)
            {
                //first dialogue finish, show instructionpanel 
                if (!hasShownInstructionPanel && instructionPanel != null) 
                { instructionPanel.SetActive(true);
                    hasShownInstructionPanel = true;
                }
                //give glasses 
                if (!glassesGiven && inventory != null && glassesItem !=null)
                {
                    inventory.Add(glassesItem, 1);
                    glassesGiven = true;

                    if (xrayController != null)
                        xrayController.UnlockXRay();

                }
            }
            else if (seedsTurnedIn && !patrolStarted && currentDialogue ==completedDialogue)
            {
                if (waypointPatrol != null)
                {
                    waypointPatrol.enabled = true;
                    patrolStarted = true; 
                }
            }



                RemoveText();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            RemoveText();
        }
    }
}



