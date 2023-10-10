using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using Parameter = Autodesk.Revit.DB.Parameter;
using View = Autodesk.Revit.DB.View;

namespace LinkElement
{
    public partial class HistoryMaintain : System.Windows.Forms.Form
    {
        ExternalCommandData commandData;
        private object ElementChangeType;

        private bool checkboxState = false;
        public HistoryMaintain(ExternalCommandData commandData_)
        {
            InitializeComponent();
            commandData = commandData_;
        }

        private void btn_cncl_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /*
        private void btn_ok_Click(object sender, EventArgs e)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc_ = uiDoc.Document;

            Document doc = commandData.Application.ActiveUIDocument.Document;

            string value = Idtxtbox.Text.ToString();
            int IdValue = int.Parse(value);

            ElementId elementIdToCheck = new ElementId(IdValue);

            Element element = doc.GetElement(elementIdToCheck);

            if (element != null)
            {
                
                var modifications = new FilteredElementCollector(doc)
                    .OfClass(typeof(Revision))
                    .Cast<Revision>()
                    .Where(r => r.Issued(elementIdToCheck).Any())
                .OrderByDescending(r => r.Issued);
                
                if (modifications != null)
                {
                    var userName = doc.GetElement(modifications.GetChangeUserId());

                    var lastModification = modifications.First();
                    var user = doc.GetElement(lastModification.UserId);

                    MessageBox.Show("Element Ownership",
                        $"The element was last modified by {user.Name} on {lastModification.Issued.ToLongDateString()} at {lastModification.Issued.ToLongTimeString()}");
                }
                else
                {
                    MessageBox.Show("Element Ownership", "No ownership information found for the element.");
                } 
            } 
            else
            {
                MessageBox.Show("Element Ownership", "Element not found.");
            }
        }
        */
        
        /*
        public void FindOwnershipDateForElement(Document doc,  ElementId elementId)
        {
            
              var modificationInfo = GetLastModificationInfo(doc, elementId);

            if (modificationInfo != null)
            {
                MessageBox.Show("Element Ownership", $"The element was last modified by {modificationInfo.User.Name} on {modificationInfo.Time}");
            }
            else
            {
                MessageBox.Show("Element Ownership", "No ownership information found for the element.");
            }

        }
        */
        /*
        public ElementModificationInfo GetLastModificationInfo(Document doc, ElementId elementId)
        {
            var history = doc.GetEntityChanges(new EntityChangeTypeFilter(ElementChangeType.Modify));

            var lastModification = history
                .Where(change => change.GetAddedElementIds().Contains(elementId))
                .OrderByDescending(change => change.GetChangeTime())
                .FirstOrDefault();

            if (lastModification != null)
            {
                var changeTime = lastModification.GetChangeTime();
                var user = doc.GetElement(lastModification.GetChangeUserId());

                return new ElementModificationInfo
                {
                    Time = changeTime,
                    User = user
                };
            }

            return null; // No modifications found.
        }

        */
        public class ElementModificationInfo
        {
            public DateTime Time { get; set; }
            public Element User { get; set; }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void btn_disable_Click(object sender, EventArgs e)
        {
            if (checkBoxDoor.Checked)
            {
                Lock_Element(BuiltInCategory.OST_Doors);

               // UIApplication uiApp = commandData.Application;
               // UIDocument uiDoc = uiApp.ActiveUIDocument;
               // Document doc = uiDoc.Document;

               // FilteredElementCollector collector = new FilteredElementCollector(doc);
               // collector.OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_Doors);

               // using (Transaction tx = new Transaction(doc, "Disable All Doors"))
               // {
               //     tx.Start();

               //   /*  foreach (Element door in collector)
               //     {
               //             // Set the custom "Lock Status" parameter for the door
               //             //Parameter hiddenParam = door.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED);
               //             //if (hiddenParam != null)
               //             //{
               //             //    hiddenParam.Set(1); // You can customize this value as needed
               //             //}
               //         View specificView = doc.ActiveView; // Use the desired view
               //         specificView.SetElementOverrides(door.Id, new OverrideGraphicSettings().SetSurfaceTransparency(1));

               //     }*/
               //      foreach (FamilyInstance door in collector)
               //      {
               //          // Disable the door by changing its parameter
               //          Parameter isLockedParam = door.get_Parameter(BuiltInParameter.DOOR_NUMBER);
               //          if (isLockedParam != null && isLockedParam.IsReadOnly == false)
               //          {
               //             if (door.Pinned == true)
               //             {
               //                 continue;
               //             }
               //             View specificView = doc.ActiveView;
               //             isLockedParam.Element.Pinned = true;
               //            // door.Category.set_Visible(specificView, false);
               //             door.Pinned = true;                            
               //          }
               //      }
               //     tx.Commit();
               // }
               // checkboxState = checkBoxDoor.Checked;
               //// Properties.Settings.Default.CheckboxStatus = checkBox.Checked;
               // Properties.Settings.Default.Save();
            }
            if (checkBoxfur.Checked)
            {
                Lock_Element(BuiltInCategory.OST_Furniture);

            }
            if (checkBoxcw.Checked)
            {
                Lock_Element(BuiltInCategory.OST_Casework);
            }
            if (checkBoxstructcol.Checked)
            {
                Lock_Element(BuiltInCategory.OST_StructuralColumns);
            }
        }
        public void Lock_Element(BuiltInCategory elecat)
        {
            try
            {
                UIApplication uiApp = commandData.Application;
                UIDocument uiDoc = uiApp.ActiveUIDocument;
                Document doc = uiDoc.Document;

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance)).OfCategory(elecat);

                using (Transaction tx = new Transaction(doc, "Disable All Element"))
                {
                    tx.Start();
                    foreach (FamilyInstance fur in collector)
                    {
                        Parameter isLockedParam = fur.get_Parameter(BuiltInParameter.DOOR_NUMBER);
                        if (isLockedParam != null && isLockedParam.IsReadOnly == false)
                        {
                            if (fur.Pinned == true)
                            {
                                continue;
                            }
                            View specificView = doc.ActiveView;
                            isLockedParam.Element.Pinned = true;
                            fur.Pinned = true;
                        }
                    }
                    tx.Commit();
                }
            }
            catch (Exception ex)
            { 
                
            }
        }

        private void HistoryMaintain_Load(object sender, EventArgs e)
        {
            checkBoxDoor.Checked = checkboxState;
           // checkBoxDoor.Checked = Properties.Settings.Default.CheckboxStatus;
        }

        private void btn_unhid_ele_Click(object sender, EventArgs e)
        {
            try
            {
                UIApplication uiApp = commandData.Application;
                UIDocument uidoc = uiApp.ActiveUIDocument;
                Document doc = uidoc.Document;

                Selection sel = uidoc.Selection;
                View activeView = doc.ActiveView;

                var ele_id = Idtxtbox.Text;

                //FilteredElementCollector collector = new FilteredElementCollector(doc);
                //collector.OfClass(typeof(FamilyInstance));

                //IEnumerable<ElementId> refCallouts = new FilteredElementCollector(doc)
                //.OfCategory(BuiltInCategory.OST_Viewers)
                //    .WhereElementIsNotElementType().Where(x => x.IsHidden(activeView))
                //.Select(x => x.Id)
                //.Cast<ElementId>();

                // FilteredElementCollector collector = new FilteredElementCollector(doc, activeView.Id)
                //.WhereElementIsNotElementType()
                //.Where(element => activeView.IsElementHidden(element.Id));

              
                Reference pickedRef = uidoc.Selection.PickObject(ObjectType.Element, "Select an element to isolate");
               // this.Hide();
                ICollection<ElementId> elementIds = uidoc.Selection.GetElementIds();
                elementIds.Clear();
                // Get the selected element
                // Element selectedElement = doc.GetElement(pickedRef.ElementId);

                // Isolate the selected element temporarily in the active view
                // activeView.IsolateElementTemporary(selectedElement.Id);

                // Disable the temporary hide/isolate mode
                // doc.ActiveView.DisableTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate);


                //  IList<Element> allElements = collector.ToElements();

                // IList<Element> hiddenElements = new List<Element>();
                /*
                foreach (Element element in collector)
                {
                   MessageBox.Show("Revit", element.ToString());
                }

                using (Transaction tr = new Transaction(doc, "Unhide"))
                {
                    tr.Start();
                    if (activeView.IsTemporaryHideIsolateActive())
                    {
                        TemporaryViewMode tempView = TemporaryViewMode.TemporaryHideIsolate;
                        activeView.DisableTemporaryViewMode(tempView);
                    }
                    tr.Commit();
                }
                */

                //FilteredElementCollector collector = new FilteredElementCollector(doc);
                //collector.OfClass(typeof(FamilyInstance));

                //// Use the WhereElementIsNotElementType method to exclude element types
                //ICollection<Element> allElements = collector.ToElements();

                //// Create a list to store hidden elements
                //List<Element> hiddenElements = new List<Element>();

                //// Iterate through all elements to find hidden elements
                //foreach (Element element in allElements)
                //{
                //    if (!element.IsHidden(doc.ActiveView))
                //    {
                //        hiddenElements.Add(element);
                //    }
                //}
            }
            catch (Exception ex) 
            { 
                
            }
        }

        private void btn_enable_Click(object sender, EventArgs e)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uidoc = uiApp.ActiveUIDocument;
            Document doc = uidoc.Document;

            Selection sel = uidoc.Selection;
            View activeView = doc.ActiveView;

            string element_id = Idtxtbox.Text;

            // Try to parse the text as an integer (ElementId)
            if (int.TryParse(Idtxtbox.Text, out int elementIdValue))
            {
                ElementId elementId = new ElementId(elementIdValue);

                Element element = doc.GetElement(elementId);

                // Unhide the element in the active view
               //  doc.ActiveView.UnhideElements(new List<ElementId> { elementId });
                doc.ActiveView.DisableTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate);

                //using (Transaction transaction = new Transaction(doc, "Unhide Element"))
                //{
                //    transaction.Start();

                //    doc.ActiveView.DisableTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate);

                //    transaction.Commit();
                //}
            }
            //Element selectedElement = doc.GetElement(element_id);

            // Disable the temporary hide/isolate mode
            //doc.ActiveView.DisableTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate);
        }
    }


    internal class HistoryTracker
    {
        internal void TrackElementChanges(Document doc, ElementId elementIdToCheck)
        {
            throw new NotImplementedException();
        }
    }

    internal class EntityChangeTypeFilter
    {
        private object modify;

        public EntityChangeTypeFilter(object modify)
        {
            this.modify = modify;
        }
    }
}
