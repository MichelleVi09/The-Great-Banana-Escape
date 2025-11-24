using UnityEngine;

public class ControlsPanelToggle : MonoBehaviour
{
    [SerializeField] private GameObject controlsPanel;

    void Start()
    {
        if (controlsPanel != null)
            controlsPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) ||
            Input.GetKeyDown(KeyCode.RightControl))
        {
            TogglePanel();
        }
    }

    public void TogglePanel()
    {
        if (controlsPanel == null) return;
        controlsPanel.SetActive(!controlsPanel.activeSelf);
    }
}

