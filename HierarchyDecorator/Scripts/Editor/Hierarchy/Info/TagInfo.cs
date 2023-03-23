using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace HierarchyDecorator
{
    public class TagInfo : HierarchyInfo
    {
        protected override void DrawInfo(Rect rect, GameObject instance, Settings settings)
        {
            if (rect.x < (LabelRect.x + LabelRect.width))
            {
                return;
            }

            EditorGUI.LabelField (rect, instance.tag, Style.SmallDropdown);

            if (settings.Global.clickToSelectTag)
            {
                Event e = Event.current;
                bool hasClicked = rect.Contains (e.mousePosition) && e.type == EventType.MouseDown;

                if (!hasClicked)
                {
                    return;
                }

                GenericMenu menu = new GenericMenu ();

                foreach (string tag in InternalEditorUtility.tags)
                {
                    menu.AddItem(new GUIContent(tag), instance.CompareTag(tag), () => SetTag(instance, tag));
                }

                menu.ShowAsContext ();
                e.Use ();
            }
        }

        private void SetTag(GameObject instance, string tag)
        {
            Undo.RecordObject(instance, "Tag Updated");
            instance.tag = tag;
            Selection.SetActiveObjectWithContext(null, null);
        }

        protected override int GetGridCount()
        {
            return 3;
        }

        protected override bool DrawerIsEnabled(Settings settings, GameObject instance)
        {
            if (settings.Styles.HasStyle (instance.name) && !settings.Styles.displayTags)
            {
                return false;
            }

            return settings.Global.showTags;
        }
    }
}