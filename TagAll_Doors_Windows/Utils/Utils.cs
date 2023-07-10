using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TagAll_Doors_Windows
{
    internal static class Utils
    {

        #region Ribbon
        internal static RibbonPanel CreateRibbonPanel(UIControlledApplication app, string tabName, string panelName)
        {
            RibbonPanel currentPanel = GetRibbonPanelByName(app, tabName, panelName);

            if (currentPanel == null)
                currentPanel = app.CreateRibbonPanel(tabName, panelName);

            return currentPanel;
        }        

        internal static RibbonPanel GetRibbonPanelByName(UIControlledApplication app, string tabName, string panelName)
        {
            foreach (RibbonPanel tmpPanel in app.GetRibbonPanels(tabName))
            {
                if (tmpPanel.Name == panelName)
                    return tmpPanel;
            }

            return null;
        }

        #endregion

        #region Tags

        internal static FamilySymbol GetTagByName(Document curDoc, string tagName)
        {
            // get all loaded door & window tags
            List<BuiltInCategory> m_colTags = new List<BuiltInCategory>();
            m_colTags.Add(BuiltInCategory.OST_DoorTags);
            m_colTags.Add(BuiltInCategory.OST_WindowTags);

            ElementMulticategoryFilter filter = new ElementMulticategoryFilter(m_colTags);

            IList<Element> tElements = new FilteredElementCollector(curDoc)
                .WherePasses(filter)
                .WhereElementIsElementType()
                .ToElements();

            foreach (Element elem in tElements)
            {
                if (elem.Name.Equals(tagName))
                    return elem as FamilySymbol;
            }

            return null;
        }

        internal static void TagAllUntaggedDoorsInView(Document curDoc, View flrPlan)
        {

            // get all doors in the view
            ICollection<ElementId> m_DoorIds = new FilteredElementCollector(curDoc, flrPlan.Id)
                .OfCategory(BuiltInCategory.OST_Doors)
                .WhereElementIsNotElementType()
                .ToElementIds();

            // filter out already tagged doors
            ICollection<ElementId> m_UntaggedDoorIds = new List<ElementId>();
            foreach (ElementId doorId in m_DoorIds)
            {
                // review Relationship Inverter page on The Building Coder

                // I don't think this code is right

                FamilyInstance door = curDoc.GetElement(doorId) as FamilyInstance;

                if (door != null && door.GetParameters("Tag").FirstOrDefault() == null)
                {
                    m_UntaggedDoorIds.Add(doorId);
                }
            }

            // set the door tag family & type
            FamilySymbol doorTag = Utils.GetTagByName(curDoc, "Door Tag-Type Comments : Type 1");

            // create a new tag for each untagged door
            foreach (ElementId doorId in m_UntaggedDoorIds)
            {
                FamilyInstance door = curDoc.GetElement(doorId) as FamilyInstance;
                
                if (door != null)
                {
                    // create a reference for the door
                    Reference doorRef = new Reference(door);

                    // get the door location
                    XYZ doorLoc = (door.Location as LocationPoint)?.Point;

                    // create a new tag at the door location
                    IndependentTag newTag = IndependentTag.Create(curDoc,
                        doorTag.Id,
                        flrPlan.Id,
                        doorRef,
                        false,
                        TagOrientation.AnyModelDirection,
                        doorLoc);

                    // assign tag family symbol
                    newTag.ChangeTypeId(doorTag.Id);
                }
            }            
        }       

        internal static void TagAllUntaggedWindowsInView(Document curDoc, View flrPlan)
        {
            throw new NotImplementedException();
        }

        internal static void MarkAllDoorsInView(Document curDoc, View flrPlan)
        {
            throw new NotImplementedException();
        }

        internal static void MarkAllWindowsInView(Document curDoc, View flrPlan)
        {
            throw new NotImplementedException();
        }

        internal static void TagAllDoorOpeningsInView(Document curDoc, View dimPlan)
        {
            throw new NotImplementedException();
        }

        internal static void TagAllWindowOpeningsInView(Document curDoc, View dimPlan)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Views

        internal static List<View> GetAllViews(Document curDoc)
        {
            {
                FilteredElementCollector m_colviews = new FilteredElementCollector(curDoc);
                m_colviews.OfCategory(BuiltInCategory.OST_Views);

                List<View> m_views = new List<View>();
                foreach (View x in m_colviews.ToElements())
                {
                    if (x.IsTemplate == false)

                        m_views.Add(x);
                }

                return m_views;
            }
        }

        internal static List<View> GetAllViewsByNameContains(Document curDoc, string viewName)
        {
            List<View> m_Views = Utils.GetAllViews(curDoc);

            List<View> m_ViewsToTag = new List<View>();

            foreach (View curView in m_Views)
            {
                if (curView.Name.Contains(viewName))
                    m_ViewsToTag.Add(curView);
            }

            return m_ViewsToTag;
        }

        #endregion

    }
}
