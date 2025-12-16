using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FinalSceneDialogue : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject continueButton;

    [Header("Dialogue content")]
    [TextArea] public string[] dialogueLines;
    [SerializeField] private float wordSpeed = 0.05f;

    private int index = 0;
    // Start is called before the first frame update
    private void Start()
    {
        if (dialogueText != null)
            dialogueText.text = "";

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);
        index = 0;
        StartTypingCurrentLine();

    }

    private void StartTypingCurrentLine()
    {
        if (dialogueLines == null || dialogueLines.Length == 0 || dialogueText == null)
        {
            return;
        }
        StopAllCoroutines();
        StartCoroutine(Typing());
    }

    private IEnumerator Typing()
    {
        dialogueText.text = "";

        if (continueButton != null)
            continueButton.SetActive(false);

        foreach(char letter in dialogueLines[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        if (continueButton != null)
            continueButton.SetActive(true) ;
    }

    public void NextLine()
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            return;
        }
        if (index < dialogueLines.Length - 1)
        {
            index++;
            if (continueButton != null)
                continueButton.SetActive(false);

            StartTypingCurrentLine();
        }
        else
        {
            if (dialoguePanel != null)
                dialoguePanel.SetActive(false);
        }
    }
}
