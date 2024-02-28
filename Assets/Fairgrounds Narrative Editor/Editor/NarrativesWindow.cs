using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

// Base class for all windows that display Narrative information.
public class NarrativesWindow : EditorWindow
{
    [SerializeField]
    protected VisualTreeAsset uxml;

    // Nested interface that can be either a single Narrative or a group of Narratives.
    protected interface INarrativeOrGroup
    {
        public string name
        {
            get;
        }

        public bool populated
        {
            get;
        }
    }

    // Nested class that represents a narrative.
    protected class Narrative : INarrativeOrGroup
    {
        public string name
        {
            get;
        }

        public bool populated
        {
            get;
        }

        public Narrative(string name, bool populated = false)
        {
            this.name = name;
            this.populated = populated;
        }
    }

    // Nested class that represents a group of narratives.
    protected class NarrativeGroup : INarrativeOrGroup
    {
        public string name
        {
            get;
        }

        public bool populated
        {
            get
            {
                var anyNarrativePopulated = false;
                foreach (Narrative narrative in narratives)
                {
                    anyNarrativePopulated = anyNarrativePopulated || narrative.populated;
                }
                return anyNarrativePopulated;
            }
        }

        public readonly IReadOnlyList<Narrative> narratives;

        public NarrativeGroup(string name, IReadOnlyList<Narrative> narratives)
        {
            this.name = name;
            this.narratives = narratives;
        }
    }

    // Data about narratives in our solar system.
    protected static readonly List<NarrativeGroup> narrativeGroups = new List<NarrativeGroup>
    {
        new NarrativeGroup("Narrative Events", new List<Narrative>
        {
            //TODO: create FOREACH loop that grabs all the cells from a csv in one column and creates a Narrative for each one
                //this will net like the declared planet variables below.

            new Narrative("Mars"),
            new Narrative("Venus"),
            new Narrative("Earth", true),
            new Narrative("Mars")
        }),
        //uncomment below to add another group. Another group would provide a second, multicolumn list hidden with an arrow
            //imagining this will mostly be useful in the future when there are multiple narratives in one scene to navigate between
        /* new NarrativeGroup("Outer Narratives", new List<Narrative>
        {
            new Narrative("Jupiter"),
            new Narrative("Saturn"),
            new Narrative("Uranus"),
            new Narrative("Neptune")
        }) */
    };

    // Expresses narrative data as a list of the narratives themselves. Needed for ListView and MultiColumnListView.
    protected static List<Narrative> narratives
    {
        get
        {
            var retVal = new List<Narrative>(8);
            foreach (var group in narrativeGroups)
            {
                retVal.AddRange(group.narratives);
            }
            return retVal;
        }
    }

    // Expresses narrative data as a list of TreeViewItemData objects. Needed for TreeView and MultiColumnTreeView.
    protected static IList<TreeViewItemData<INarrativeOrGroup>> treeRoots
    {
        get
        {
            int id = 0;
            var roots = new List<TreeViewItemData<INarrativeOrGroup>>(narrativeGroups.Count);
            foreach (var group in narrativeGroups)
            {
                var narrativesInGroup = new List<TreeViewItemData<INarrativeOrGroup>>(group.narratives.Count);
                foreach (var narrative in group.narratives)
                {
                    narrativesInGroup.Add(new TreeViewItemData<INarrativeOrGroup>(id++, narrative));
                }

                roots.Add(new TreeViewItemData<INarrativeOrGroup>(id++, group, narrativesInGroup));
            }
            return roots;
        }
    }
}