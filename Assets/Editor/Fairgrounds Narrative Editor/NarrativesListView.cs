using UnityEditor;
using UnityEngine.UIElements;

public class NarrativesListView : NarrativesWindow
{
    [MenuItem("Narratives/Standard List")]
    static void Summon()
    {
        GetWindow<NarrativesListView>("Standard Narrative List");
    }

    void CreateGUI()
    {
        uxml.CloneTree(rootVisualElement);
        var listView = rootVisualElement.Q<ListView>();

        // Set ListView.itemsSource to populate the data in the list.
        listView.itemsSource = narratives;

        // Set ListView.makeItem to initialize each entry in the list.
        listView.makeItem = () => new Label();

        // Set ListView.bindItem to bind an initialized entry to a data item.
        listView.bindItem = (VisualElement element, int index) =>
            (element as Label).text = narratives[index].name;
    }
}
