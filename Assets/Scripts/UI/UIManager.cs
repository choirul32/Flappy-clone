using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public UIDocument uiDocument;
    private VisualElement mainMenu;
    private VisualElement pokedex;
    private VisualElement detail;
    Button toDetail;
    Button exitDetail;

    void Start()
    {
        var root = uiDocument.rootVisualElement;

        mainMenu = root.Q<VisualElement>("MainMenu");
        pokedex = root.Q<VisualElement>("Pokedex");
        detail = root.Q<VisualElement>("Detail");

        toDetail = pokedex.Q<Button>("button-apple");
        exitDetail = detail.Q<Button>("button-exit-detail");

        toDetail.clicked += ShowDetail;
        exitDetail.clicked += HideDetail;
    }

    void ShowDetail()
    {
        detail.AddToClassList("detailsheet--up");
    }

    void HideDetail()
    {
        detail.RemoveFromClassList("detailsheet--up");
    }

    void OnDisable()
    {
        toDetail.clicked -= ShowDetail;
        exitDetail.clicked -= HideDetail;
    }
}