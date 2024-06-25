using UnityEngine;
using UnityEngine.UIElements;

public class GuiController : MonoBehaviour
{
    [SerializeField] private UIDocument _gameOverUi;

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
