using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour
{
    public string content;
    public string header; 
    private void OnMouseEnter()
    {
        TooltipSystem.Show(content, header);
    }
    private void OnMouseExit()
    {
        TooltipSystem.Hide();
    }
}
