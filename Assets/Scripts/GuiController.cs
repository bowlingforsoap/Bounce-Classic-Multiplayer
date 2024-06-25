using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GuiController : MonoBehaviour
{
    [SerializeField] private UIDocument _networkOverlay;
    [SerializeField] private UIDocument _gameOverUi;

    private void Start()
    {
        ShowNetworkOverlay();
    }

    public void ShowNetworkOverlay()
    {
        SetGuiEnabled(_networkOverlay, true);
    }
    
    public void ShowGameOverScreen()
    {
        SetGuiEnabled(_gameOverUi, true);
    }

    public void HideGameOverScreen()
    {
        SetGuiEnabled(_gameOverUi, false);
    }

    private void SetGuiEnabled(UIDocument uiDocument, bool isEnabled)
    {
        uiDocument.enabled = isEnabled;
    }
}
