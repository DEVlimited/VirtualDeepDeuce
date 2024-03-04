using UnityEditor;
using UnityEngine.UIElements;

public class NarrativesMultiColumnListView : NarrativesWindow
{
    [MenuItem("Narratives/Multicolumn List")]
    static void Summon()
    {
        GetWindow<NarrativesMultiColumnListView>("Multicolumn Narrative List");
    }

    void CreateGUI()
    {
        uxml.CloneTree(rootVisualElement);
        var listView = rootVisualElement.Q<MultiColumnListView>();

        // Set MultiColumnListView.itemsSource to populate the data in the list.
        listView.itemsSource = narratives;

        // For each column, set Column.makeCell to initialize each cell in the column.
        // You can index the columns array with names or numerical indices.
        listView.columns["event"].makeCell = () => new Label();
        listView.columns["start"].makeCell = () => new Label();

        // For each column, set Column.bindCell to bind an initialized cell to a data item.
        listView.columns["event"].bindCell = (VisualElement element, int index) =>
            (element as Label).text = "event number will go here";
        listView.columns["start"].bindCell = (VisualElement element, int index) =>
            (element as Label).text = "start time for event number will show here";
    }
}
