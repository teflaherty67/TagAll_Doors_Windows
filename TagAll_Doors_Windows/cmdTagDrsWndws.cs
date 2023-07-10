#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace TagAll_Doors_Windows
{
    [Transaction(TransactionMode.Manual)]
    public class cmdTagDrsWndws : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document curDoc = uidoc.Document;

            // put any code needed for the form here

            // open form
            frmTagDrsWndws curForm = new frmTagDrsWndws()
            {
                Width = 350,
                Height = 200,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };

            curForm.ShowDialog();

            // create some lists to hold the views
            List<View> flrPlans = Utils.GetAllViewsByNameContains(curDoc, "Annotation");
            List<View> dimPlans = Utils.GetAllViewsByNameContains(curDoc, "Dimension");

            // get form data and do something

            // create a transaction
            using(Transaction trans = new Transaction(curDoc))
            {
                // start the transaction
                trans.Start("Tag all doors and windows");

                if (curForm.GetCheckBoxFlrs() == true)
                {
                    // loop through the annotation views
                    foreach (View flrPlan in flrPlans)
                    {
                        // tag all doors in view
                        Utils.TagAllUntaggedDoorsInView(curDoc, flrPlan);

                        // tag all windows in view
                        Utils.TagAllUntaggedWindowsInView(curDoc, flrPlan);
                    }
                }

                if (curForm.GetCheckBoxDouble()  == true)
                {
                    // loop through the annotation views
                    foreach (View flrPlan in flrPlans)
                    {
                        // mark all doors in view
                        Utils.MarkAllDoorsInView(curDoc, flrPlan);

                        // mark all windows in view
                        Utils.MarkAllWindowsInView(curDoc, flrPlan);
                    }
                }

                if (curForm.GetCheckBoxDims() == true)
                {
                    // loop through the dimension views
                    foreach (View dimPlan in dimPlans)
                    {
                        // tag all door rough opening sizes
                        Utils.TagAllDoorOpeningsInView(curDoc, dimPlan);

                        // tag all window rough opening sizes
                        Utils.TagAllWindowOpeningsInView(curDoc, dimPlan);
                    }
                }

                // commit the transaction
                trans.Commit();
            }

            return Result.Succeeded;
        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}
