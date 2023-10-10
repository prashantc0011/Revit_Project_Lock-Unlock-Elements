using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Color = System.Drawing.Color;
using DataTable = System.Data.DataTable;
using Document = Autodesk.Revit.DB.Document;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using MessageBox = System.Windows.Forms.MessageBox;
using Parameter = Autodesk.Revit.DB.Parameter;
using View = Autodesk.Revit.DB.View;


namespace LinkElement
{
    public partial class Form3 : System.Windows.Forms.Form
    {
        ExternalCommandData commandData;
        string door_cat_Id = null;
        private List<string> selectedItems;
        int batchSize = 20;
        int btn_color = -1;
        int group_id = -1;
        int index = 0;
        bool isChecked;


        #region variable Declaration
        public DataTable dt = null;
        public bool Model = false;
        public bool Annotation = false;
        public bool machineid = false;
        public bool docname = false;
        public string userid = "";
        public string username = "";
        public static string errorLog = "";
        public static string strWorkPath = "";
        static Dictionary<int, string> _start_state = null;
        public static string RevitVersion = "";
        #endregion

        /*         
            Walls
            Rooms
            Curtain Wall Mullions
         */

        List<ElementId> all_mech_ele_Ids = new List<ElementId>();
        public Form3(ExternalCommandData commandData_)
        {
            InitializeComponent();
            commandData = commandData_;

            this.KeyDown += MyForm_KeyDown;

            lstbox_items.SelectionMode = SelectionMode.MultiExtended;
            lstbox_items.SelectedIndexChanged += lstbox_items_SelectedIndexChanged;
            selectedItems = new List<string>();

            this.Controls.Add(lstbox_items);
        }

        private void MyForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                // Handle the Escape key press here based on your needs
                // For example, you can show a message to the user or ignore it
                MessageBox.Show("Escape key pressed, operation cannot be canceled.");
                // e.Handled = true; // Uncomment this line to prevent further processing of the Escape key press
            }
        }
        public void Disable_Elements(BuiltInCategory cat)
        {
            try
            {
                UIApplication uiApp = commandData.Application;
                UIDocument uiDoc = uiApp.ActiveUIDocument;
                Document doc = uiDoc.Document;

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance)).OfCategory(cat);

                using (Transaction tx = new Transaction(doc, "Disable All Elements"))
                {
                    tx.Start();

                    foreach (FamilyInstance ele in collector)
                    {
                        if (ele != null)
                        {

                            if (ele.Pinned == true)
                            {
                                continue;
                            }
                            ele.Pinned = true;
                        }
                    }
                    tx.Commit();
                }

                /*
                IList<Element> elements = collector.ToElements();
                if (elements.Count > 0)
                {
                    IEnumerable<IEnumerable<Element>> batches = elements
                        .Select((element, index) => new { Element = element, Index = index })
                        .GroupBy(x => x.Index / batchSize)
                        .Select(g => g.Select(x => x.Element));

                    List<ElementId> all_ele_Ids = new List<ElementId>();

                    foreach (IEnumerable<Element> batch in batches)
                    {
                        foreach (Element element in batch)
                        {
                            if (element.Pinned == true)
                            {
                                continue;
                            }
                            all_ele_Ids.Add(element.Id);
                            element.CanBeLocked();
                            element.Pinned = true;

                        }
                    }
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Disable_Elements : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Enable_Elements(BuiltInCategory cat)
        {
            try
            {
                UIApplication uiApp = commandData.Application;
                UIDocument uiDoc = uiApp.ActiveUIDocument;
                Document doc = uiDoc.Document;

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance)).OfCategory(cat);

                using (Transaction tx = new Transaction(doc, "Enable All Elements"))
                {
                    tx.Start();
                    foreach (FamilyInstance ele in collector)
                    {
                        if (ele != null)
                        {
                            if (ele.Pinned == false)
                            {
                                continue;
                            }
                            ele.Pinned = false;
                        }
                    }
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Enable_Elements : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Disable_CeilingsAndFloor(BuiltInCategory CatType)
        {
            try
            {
                UIApplication uiApp = commandData.Application;
                UIDocument uiDoc = uiApp.ActiveUIDocument;
                Document doc = uiDoc.Document;

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(CeilingAndFloor));

                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(CatType);

                collector.WherePasses(categoryFilter);

                using (Transaction tx = new Transaction(doc, "Disable All Elements"))
                {
                    tx.Start();

                    foreach (FamilyInstance ele in collector)
                    {
                        if (ele != null)
                        {
                            if (ele.Pinned == true)
                            {
                                continue;
                            }
                            ele.Pinned = true;
                        }
                    }
                    tx.Commit();
                }
                /* Document doc = commandData.Application.ActiveUIDocument.Document;

                 FilteredElementCollector collector = new FilteredElementCollector(doc);
                 collector.OfClass(typeof(CeilingAndFloor));

                 ElementCategoryFilter categoryFilter = new ElementCategoryFilter(CatType);

                 collector.WherePasses(categoryFilter);

                 IList<Element> elements = collector.ToElements();

                 IEnumerable<IEnumerable<Element>> batches = elements
                     .Select((element, index) => new { Element = element, Index = index })
                     .GroupBy(x => x.Index / batchSize)
                     .Select(g => g.Select(x => x.Element));

                 List<ElementId> all_ele_Ids = new List<ElementId>();

                 foreach (IEnumerable<Element> batch in batches)
                 {
                     foreach (Element element in batch)
                     {
                         if (element.Pinned == true)
                         {
                             continue;
                         }
                         all_ele_Ids.Add(element.Id);
                         element.CanBeLocked();
                         element.Pinned = true;
                     }
                 }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Disable_Elements : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Enable_CeilingsAndFloor(BuiltInCategory CatType)
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the Categories in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(CeilingAndFloor));

                // Create a filter to only select the single Category
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(CatType);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> elements = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = elements
                    .Select((element, index) => new { Element = element, Index = index })
                    .GroupBy(x => x.Index / batchSize)
                    .Select(g => g.Select(x => x.Element));

                List<ElementId> all_ele_Ids = new List<ElementId>();

                foreach (IEnumerable<Element> batch in batches)
                {
                    // Process the elements in the current batch
                    foreach (Element element in batch)
                    {
                        if (element.Pinned == false || element.GroupId.ToString() != group_id.ToString())
                        {
                            continue;
                        }
                        element.Pinned = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Enable_Elements : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Disable_Railings(BuiltInCategory CatRailings)
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(CeilingAndFloor));

                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(CatRailings);

                collector.WherePasses(categoryFilter);

                using (Transaction tx = new Transaction(doc, "Disable All Elements"))
                {
                    tx.Start();

                    foreach (FamilyInstance ele in collector)
                    {
                        if (ele != null)
                        {
                            if (ele.Pinned == true)
                            {
                                continue;
                            }
                            ele.Pinned = true;
                        }
                    }
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Disable_Elements : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Enable_Railings(BuiltInCategory CatRailings)
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the Categories in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(Railing));

                // Create a filter to only select the single Category
                //ElementCategoryFilter categoryFilter = new ElementCategoryFilter(CatRailings);

                // Apply the filter to the collector
                //collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> elements = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = elements
                    .Select((element, index) => new { Element = element, Index = index })
                    .GroupBy(x => x.Index / batchSize)
                    .Select(g => g.Select(x => x.Element));

                List<ElementId> all_ele_Ids = new List<ElementId>();

                foreach (IEnumerable<Element> batch in batches)
                {
                    // Process the elements in the current batch
                    foreach (Element element in batch)
                    {
                        if (element.Pinned == false || element.GroupId.ToString() != group_id.ToString())
                        {
                            continue;
                        }
                        element.Pinned = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Disable_Elements : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //Not in Use
        public void Dis_Ele_By_CheckBox_Model(BuiltInCategory _cat)
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance)).OfCategory(_cat).WhereElementIsNotElementType();

                using (Transaction tx = new Transaction(doc, "Disable All Elements"))
                {
                    tx.Start();

                    foreach (FamilyInstance ele in collector)
                    {
                        if (ele != null)
                        {

                            if (ele.Pinned == true)
                            {
                                continue;
                            }
                            ele.Pinned = true;
                        }
                    }
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Disable_Elements : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Disable_cad_import(BuiltInCategory cad_import)
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the Categories in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(ImportInstance)).WhereElementIsNotElementType();

                // Create a filter to only select the single Category
                // ElementCategoryFilter categoryFilter = new ElementCategoryFilter(cad_import);

                // Apply the filter to the collector
                // collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> elements = collector.ToElements();

                if (elements.Count > 0)
                {
                    // Create batches using LINQ
                    IEnumerable<IEnumerable<Element>> batches = elements
                        .Select((element, index) => new { Element = element, Index = index })
                        .GroupBy(x => x.Index / batchSize)
                        .Select(g => g.Select(x => x.Element));

                    List<ElementId> all_ele_Ids = new List<ElementId>();

                    foreach (IEnumerable<Element> batch in batches)
                    {
                        // Process the elements in the current batch
                        foreach (Element element in batch)
                        {
                            if (chk_cad_import.Checked)
                            {
                                if (element.Pinned == true)
                                {
                                    continue;
                                }
                                all_ele_Ids.Add(element.Id);
                                element.CanBeLocked();
                                element.Pinned = true;
                            }
                            else if (chk_cad_import.Checked == false)
                            {
                                if (element.Pinned == false)
                                {
                                    continue;
                                }
                                element.Pinned = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Disable_cad_import : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Enable_cad_import(BuiltInCategory cad_import)
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the Categories in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(ImportInstance)).WhereElementIsNotElementType();

                // Get the elements that pass the filter
                IList<Element> elements = collector.ToElements();

                if (elements.Count > 0)
                {
                    // Create batches using LINQ
                    IEnumerable<IEnumerable<Element>> batches = elements
                        .Select((element, index) => new { Element = element, Index = index })
                        .GroupBy(x => x.Index / batchSize)
                        .Select(g => g.Select(x => x.Element));

                    List<ElementId> all_ele_Ids = new List<ElementId>();

                    foreach (IEnumerable<Element> batch in batches)
                    {
                        // Process the elements in the current batch
                        foreach (Element element in batch)
                        {
                            if (element.Pinned == false)
                            {
                                continue;
                            }
                            element.Pinned = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Disable_cad_import : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Disable_Anno_Tags(BuiltInCategory Cat_anno)
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                FilteredElementCollector tagCollector = new FilteredElementCollector(doc);
                ICollection<Element> Element_Tags = tagCollector.OfCategory(Cat_anno).WhereElementIsNotElementType().ToElements();

                if (Element_Tags.Count > 0)
                {
                    IEnumerable<IEnumerable<Element>> batches = Element_Tags
                        .Select((element, index) => new { Element = element, Index = index })
                        .GroupBy(x => x.Index / batchSize)
                        .Select(g => g.Select(x => x.Element));

                    List<ElementId> all_ele_Ids = new List<ElementId>();

                    foreach (IEnumerable<Element> batch in batches)
                    {
                        // Process the elements in the current batch
                        foreach (Element element in batch)
                        {
                            if (element.Pinned == true || element.GroupId.ToString() != group_id.ToString() || element.Category.Name == "Curtain Wall Mullions")
                            {
                                continue;
                            }
                            all_ele_Ids.Add(element.Id);
                            element.CanBeLocked();
                            element.Pinned = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Disable_Elements : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Enable_Anno_Tags(BuiltInCategory Cat_anno)
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the Categories in the document
                FilteredElementCollector tagCollector = new FilteredElementCollector(doc);
                ICollection<Element> Element_Tags = tagCollector.OfCategory(Cat_anno).WhereElementIsNotElementType().ToElements();

                if (Element_Tags.Count > 0)
                {
                    IEnumerable<IEnumerable<Element>> batches = Element_Tags
                        .Select((element, index) => new { Element = element, Index = index })
                        .GroupBy(x => x.Index / batchSize)
                        .Select(g => g.Select(x => x.Element));

                    List<ElementId> all_ele_Ids = new List<ElementId>();

                    foreach (IEnumerable<Element> batch in batches)
                    {
                        // Process the elements in the current batch
                        foreach (Element element in batch)
                        {
                            if (element.Pinned == false || element.GroupId.ToString() != group_id.ToString() || element.Category.Name == "Curtain Wall Mullions")
                            {
                                continue;
                            }
                            element.Pinned = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Disable_Elements : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Not in Use
        public void Dis_Cat_Group(BuiltInCategory cate_)
        {
            /* try
             {
                 // Get the current Revit document
                 Document doc = commandData.Application.ActiveUIDocument.Document;

                 // Create a FilteredElementCollector to collect all the Categories in the document
                 FilteredElementCollector collector = new FilteredElementCollector(doc);
                 collector.OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType();

                 // Create a filter to only select the single Category
                 ElementCategoryFilter categoryFilter = new ElementCategoryFilter(cate_);

                 // Apply the filter to the collector
                 collector.WherePasses(categoryFilter);

                 // Get the elements that pass the filter
                 IList<Element> elements = collector.ToElements();

                 if (elements.Count > 0)
                 {
                     // Create batches using LINQ
                     IEnumerable<IEnumerable<Element>> batches = elements
                     .Select((element, index) => new { Element = element, Index = index })
                     .GroupBy(x => x.Index / batchSize)
                     .Select(g => g.Select(x => x.Element));

                     List<ElementId> all_ele_Ids = new List<ElementId>();

                     if (cate_.ToString() == "OST_MechanicalEquipment" && btn_mech.BackColor != Color.RoyalBlue)
                     {
                         foreach (IEnumerable<Element> batch in batches)
                         {
                             // Process the elements in the current batch
                             foreach (Element element in batch)
                             {
                                 if (element.Pinned == true)
                                 {
                                     continue;
                                 }
                                 all_ele_Ids.Add(element.Id);
                                 element.CanBeLocked();
                                 element.Pinned = true;
                             }
                         }
                         btn_mech.BackColor = Color.RoyalBlue;
                     }
                     else if (cate_.ToString() == "OST_StructuralAnnotations" && btn_strc.BackColor != Color.RoyalBlue)
                     {
                         foreach (IEnumerable<Element> batch in batches)
                         {
                             // Process the elements in the current batch
                             foreach (Element element in batch)
                             {
                                 if (element.Pinned == true)
                                 {
                                     continue;
                                 }
                                 all_ele_Ids.Add(element.Id);
                                 element.CanBeLocked();
                                 element.Pinned = true;
                             }
                         }
                         btn_strc.BackColor = Color.RoyalBlue;
                     }
                     else if (cate_.ToString() == "OST_ElectricalEquipment" && btn_elec.BackColor != Color.RoyalBlue)
                     {
                         foreach (IEnumerable<Element> batch in batches)
                         {
                             // Process the elements in the current batch
                             foreach (Element element in batch)
                             {
                                 if (element.Pinned == true)
                                 {
                                     continue;
                                 }
                                 all_ele_Ids.Add(element.Id);
                                 element.CanBeLocked();
                                 element.Pinned = true;
                             }
                         }
                         //btn_elec.BackColor = Color.RoyalBlue;
                     }
                     else if (cate_.ToString() == "OST_ElectricalFixtures" && btn_elec.BackColor != Color.RoyalBlue)
                     {
                         foreach (IEnumerable<Element> batch in batches)
                         {
                             // Process the elements in the current batch
                             foreach (Element element in batch)
                             {
                                 if (element.Pinned == true)
                                 {
                                     continue;
                                 }
                                 all_ele_Ids.Add(element.Id);
                                 element.CanBeLocked();
                                 element.Pinned = true;
                             }
                         }
                         btn_elec.BackColor = Color.RoyalBlue;
                     }
                     else if (cate_.ToString() == "OST_PipeConnections" && btn_plum.BackColor != Color.RoyalBlue)
                     {
                         foreach (IEnumerable<Element> batch in batches)
                         {
                             // Process the elements in the current batch
                             foreach (Element element in batch)
                             {
                                 if (element.Pinned == true)
                                 {
                                     continue;
                                 }
                                 all_ele_Ids.Add(element.Id);
                                 element.CanBeLocked();
                                 element.Pinned = true;
                             }
                         }
                         btn_plum.BackColor = Color.RoyalBlue;
                     }

                     else if (cate_.ToString() == "OST_MechanicalEquipment" && btn_mech.BackColor == Color.RoyalBlue)
                     {
                         foreach (IEnumerable<Element> batch in batches)
                         {
                             // Process the elements in the current batch
                             foreach (Element element in batch)
                             {
                                 if (element.Pinned == false || element.Id.ToString() == "3969714")
                                 {
                                     continue;
                                 }
                                 element.Pinned = false;
                             }
                         }
                         btn_mech.BackColor = Color.White;
                     }
                     else if (cate_.ToString() == "OST_StructuralAnnotations" && btn_strc.BackColor == Color.RoyalBlue)
                     {
                         foreach (IEnumerable<Element> batch in batches)
                         {
                             // Process the elements in the current batch
                             foreach (Element element in batch)
                             {
                                 if (element.Pinned == false || element.Id.ToString() == "3969714")
                                 {
                                     continue;
                                 }
                                 element.Pinned = false;
                             }
                         }
                         btn_strc.BackColor = Color.White;
                     }
                     else if (cate_.ToString() == "OST_ElectricalEquipment" && btn_elec.BackColor == Color.RoyalBlue)
                     {
                         foreach (IEnumerable<Element> batch in batches)
                         {
                             // Process the elements in the current batch
                             foreach (Element element in batch)
                             {
                                 if (element.Pinned == false || element.Id.ToString() == "3969714")
                                 {
                                     continue;
                                 }
                                 element.Pinned = false;
                             }
                         }
                         //btn_elec.BackColor = Color.White;
                     }
                     else if (cate_.ToString() == "OST_ElectricalFixtures" && btn_elec.BackColor == Color.RoyalBlue)
                     {
                         foreach (IEnumerable<Element> batch in batches)
                         {
                             // Process the elements in the current batch
                             foreach (Element element in batch)
                             {
                                 if (element.Pinned == false || element.Id.ToString() == "3969714")
                                 {
                                     continue;
                                 }
                                 element.Pinned = false;
                             }
                         }
                         btn_elec.BackColor = Color.White;
                     }
                     else if (cate_.ToString() == "OST_PipeConnections" && btn_plum.BackColor == Color.RoyalBlue)
                     {
                         foreach (IEnumerable<Element> batch in batches)
                         {
                             // Process the elements in the current batch
                             foreach (Element element in batch)
                             {
                                 if (element.Pinned == false || element.Id.ToString() == "3969714")
                                 {
                                     continue;
                                 }
                                 element.Pinned = false;
                             }
                         }
                         btn_plum.BackColor = Color.White;
                     }
                 }                
             }
             catch (Exception ex)
             {
                 MessageBox.Show("Error Disable_Elements : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
             } */
        }
        public void Dis_Ele_By_CheckBox_Analytical(BuiltInCategory _cat)
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the Categories in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType();

                // Create a filter to only select the single Category
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(_cat);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> elements = collector.ToElements();
                if (elements.Count > 0)
                {

                    // Create batches using LINQ
                    IEnumerable<IEnumerable<Element>> batches = elements
                        .Select((element, index) => new { Element = element, Index = index })
                        .GroupBy(x => x.Index / batchSize)
                        .Select(g => g.Select(x => x.Element));

                    List<ElementId> all_ele_Ids = new List<ElementId>();

                    foreach (IEnumerable<Element> batch in batches)
                    {
                        foreach (Element element in batch)
                        {
                            if (chkbox_analytical.Checked)
                            {
                                if (element.Pinned == true || element.Category.Name == "Curtain Wall Mullions")
                                {
                                    continue;
                                }
                                all_ele_Ids.Add(element.Id);
                                element.CanBeLocked();
                                element.Pinned = true;
                            }
                            else if (chkbox_analytical.Checked == false)
                            {
                                all_ele_Ids.Add(element.Id);
                                element.CanBeLocked();
                                element.Pinned = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Disable_Elements : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void lstbox_items_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void btn_arch_Click(object sender, EventArgs e)
        {
            if (btn_arch.BackColor != Color.RoyalBlue)
            {
                Disable_CeilingsAndFloor(BuiltInCategory.OST_Ceilings);
                Disable_CeilingsAndFloor(BuiltInCategory.OST_Floors);
                Disable_Railings(BuiltInCategory.OST_Railings);
                Disable_Elements(BuiltInCategory.OST_Areas);
                Disable_Elements(BuiltInCategory.OST_AudioVisualDevices);
                Disable_Elements(BuiltInCategory.OST_Casework);
                Disable_Elements(BuiltInCategory.OST_Columns);
                Disable_Elements(BuiltInCategory.OST_CurtainWallPanels);
                Disable_Elements(BuiltInCategory.OST_Curtain_Systems);
                Disable_Elements(BuiltInCategory.OST_DetailComponents);
                Disable_Elements(BuiltInCategory.OST_Doors);
                Disable_Elements(BuiltInCategory.OST_ElectricalEquipment);
                Disable_Elements(BuiltInCategory.OST_ElectricalFixtures);
                Disable_Elements(BuiltInCategory.OST_Entourage);
                Disable_Elements(BuiltInCategory.OST_FireProtection);
                Disable_Elements(BuiltInCategory.OST_FoodServiceEquipment);
                Disable_Elements(BuiltInCategory.OST_Furniture);
                Disable_Elements(BuiltInCategory.OST_FurnitureSystems);
                Disable_Elements(BuiltInCategory.OST_GenericModel);
                Disable_Elements(BuiltInCategory.OST_Hardscape);
                Disable_Elements(BuiltInCategory.OST_LightingFixtures);
                Disable_Elements(BuiltInCategory.OST_Lines);
                Disable_Elements(BuiltInCategory.OST_Mass);
                Disable_Elements(BuiltInCategory.OST_MechanicalEquipment);
                Disable_Elements(BuiltInCategory.OST_MedicalEquipment);
                Disable_Elements(BuiltInCategory.OST_Parking);
                Disable_Elements(BuiltInCategory.OST_Parts);
                Disable_Elements(BuiltInCategory.OST_Planting);
                Disable_Elements(BuiltInCategory.OST_PlumbingFixtures);
                Disable_Elements(BuiltInCategory.OST_Ramps);
                Disable_Elements(BuiltInCategory.OST_RasterImages);
                Disable_Elements(BuiltInCategory.OST_Roads);
                Disable_Elements(BuiltInCategory.OST_Roofs);
                Disable_Elements(BuiltInCategory.OST_ShaftOpening);
                Disable_Elements(BuiltInCategory.OST_Signage);
                Disable_Elements(BuiltInCategory.OST_Site);
                Disable_Elements(BuiltInCategory.OST_SpecialityEquipment);
                Disable_Elements(BuiltInCategory.OST_Stairs);
                Disable_Elements(BuiltInCategory.OST_StructuralFoundation);
                Disable_Elements(BuiltInCategory.OST_StructuralColumns);
                Disable_Elements(BuiltInCategory.OST_StructConnections);
                Disable_Elements(BuiltInCategory.OST_StructuralFraming);
                Disable_Elements(BuiltInCategory.OST_Rebar);
                Disable_Elements(BuiltInCategory.OST_Coupler);
                Disable_Elements(BuiltInCategory.OST_StructuralStiffener);
                Disable_Elements(BuiltInCategory.OST_TemporaryStructure);
                Disable_Elements(BuiltInCategory.OST_Topography);
                Disable_Elements(BuiltInCategory.OST_VerticalCirculation);
                Disable_Elements(BuiltInCategory.OST_Windows);

                btn_arch.BackColor = Color.RoyalBlue;
            }
            else if (btn_arch.BackColor == Color.RoyalBlue)
            {
                Enable_CeilingsAndFloor(BuiltInCategory.OST_Ceilings);
                Enable_CeilingsAndFloor(BuiltInCategory.OST_Floors);
                Enable_Railings(BuiltInCategory.OST_Railings);
                Enable_Elements(BuiltInCategory.OST_Areas);
                Enable_Elements(BuiltInCategory.OST_AudioVisualDevices);
                Enable_Elements(BuiltInCategory.OST_Casework);
                Enable_Elements(BuiltInCategory.OST_Columns);
                Enable_Elements(BuiltInCategory.OST_CurtainWallPanels);
                Enable_Elements(BuiltInCategory.OST_Curtain_Systems);
                Enable_Elements(BuiltInCategory.OST_DetailComponents);
                Enable_Elements(BuiltInCategory.OST_Doors);
                Enable_Elements(BuiltInCategory.OST_ElectricalEquipment);
                Enable_Elements(BuiltInCategory.OST_ElectricalFixtures);
                Enable_Elements(BuiltInCategory.OST_Entourage);
                Enable_Elements(BuiltInCategory.OST_FireProtection);
                Enable_Elements(BuiltInCategory.OST_FoodServiceEquipment);
                Enable_Elements(BuiltInCategory.OST_Furniture);
                Enable_Elements(BuiltInCategory.OST_FurnitureSystems);
                Enable_Elements(BuiltInCategory.OST_GenericModel);
                Enable_Elements(BuiltInCategory.OST_Hardscape);
                Enable_Elements(BuiltInCategory.OST_LightingFixtures);
                Enable_Elements(BuiltInCategory.OST_Lines);
                Enable_Elements(BuiltInCategory.OST_Mass);
                Enable_Elements(BuiltInCategory.OST_MechanicalEquipment);
                Enable_Elements(BuiltInCategory.OST_MedicalEquipment);
                Enable_Elements(BuiltInCategory.OST_Parking);
                Enable_Elements(BuiltInCategory.OST_Parts);
                Enable_Elements(BuiltInCategory.OST_Planting);
                Enable_Elements(BuiltInCategory.OST_PlumbingFixtures);
                Enable_Elements(BuiltInCategory.OST_Ramps);
                Enable_Elements(BuiltInCategory.OST_RasterImages);
                Enable_Elements(BuiltInCategory.OST_Roads);
                Enable_Elements(BuiltInCategory.OST_Roofs);
                Enable_Elements(BuiltInCategory.OST_ShaftOpening);
                Enable_Elements(BuiltInCategory.OST_Signage);
                Enable_Elements(BuiltInCategory.OST_Site);
                Enable_Elements(BuiltInCategory.OST_SpecialityEquipment);
                Enable_Elements(BuiltInCategory.OST_Stairs);
                Enable_Elements(BuiltInCategory.OST_StructuralFoundation);
                Enable_Elements(BuiltInCategory.OST_StructuralColumns);
                Enable_Elements(BuiltInCategory.OST_StructConnections);
                Enable_Elements(BuiltInCategory.OST_StructuralFraming);
                Enable_Elements(BuiltInCategory.OST_Rebar);
                Enable_Elements(BuiltInCategory.OST_Coupler);
                Enable_Elements(BuiltInCategory.OST_StructuralStiffener);
                Enable_Elements(BuiltInCategory.OST_TemporaryStructure);
                Enable_Elements(BuiltInCategory.OST_Topography);
                Enable_Elements(BuiltInCategory.OST_VerticalCirculation);
                Enable_Elements(BuiltInCategory.OST_Windows);

                btn_arch.BackColor = Color.White;
            }
            //Dis_Cat_Group(BuiltInCategory.OST_MEPSystemZone);
        }
        private void btn_mech_Click(object sender, EventArgs e)
        {
            if (btn_mech.BackColor != Color.RoyalBlue)
            {
                Disable_Elements(BuiltInCategory.OST_Areas);
                Disable_Elements(BuiltInCategory.OST_AudioVisualDevices);
                Disable_Elements(BuiltInCategory.OST_DetailComponents);
                Disable_Elements(BuiltInCategory.OST_DuctAccessory);
                Disable_Elements(BuiltInCategory.OST_DuctFitting);
                Disable_Elements(BuiltInCategory.OST_DuctInsulations);
                Disable_Elements(BuiltInCategory.OST_DuctLinings);
                Disable_Elements(BuiltInCategory.OST_PlaceHolderDucts);
                Disable_Elements(BuiltInCategory.OST_FireProtection);
                Disable_Elements(BuiltInCategory.OST_FlexDuctCurves);
                Disable_Elements(BuiltInCategory.OST_FoodServiceEquipment);
                Disable_Elements(BuiltInCategory.OST_GenericModel);
                Disable_Elements(BuiltInCategory.OST_HVAC_Zones);
                Disable_Elements(BuiltInCategory.OST_Mass);
                Disable_Elements(BuiltInCategory.OST_MechanicalEquipment);
                Disable_Elements(BuiltInCategory.OST_MedicalEquipment);
                Disable_Elements(BuiltInCategory.OST_FabricationDuctwork);
                Disable_Elements(BuiltInCategory.OST_FabricationHangers);
                Disable_Elements(BuiltInCategory.OST_Parts);
                Disable_Elements(BuiltInCategory.OST_RasterImages);
                Disable_Elements(BuiltInCategory.OST_Signage);
                Disable_Elements(BuiltInCategory.OST_MEPSpaces);
                Disable_Elements(BuiltInCategory.OST_TemporaryStructure);
                Disable_Elements(BuiltInCategory.OST_VerticalCirculation);

                btn_mech.BackColor = Color.RoyalBlue;
            }
            else if (btn_mech.BackColor == Color.RoyalBlue)
            {
                Enable_Elements(BuiltInCategory.OST_Areas);
                Enable_Elements(BuiltInCategory.OST_AudioVisualDevices);
                Enable_Elements(BuiltInCategory.OST_DetailComponents);
                Enable_Elements(BuiltInCategory.OST_DuctAccessory);
                Enable_Elements(BuiltInCategory.OST_DuctFitting);
                Enable_Elements(BuiltInCategory.OST_DuctInsulations);
                Enable_Elements(BuiltInCategory.OST_DuctLinings);
                Enable_Elements(BuiltInCategory.OST_PlaceHolderDucts);
                Enable_Elements(BuiltInCategory.OST_FireProtection);
                Enable_Elements(BuiltInCategory.OST_FlexDuctCurves);
                Enable_Elements(BuiltInCategory.OST_FoodServiceEquipment);
                Enable_Elements(BuiltInCategory.OST_GenericModel);
                Enable_Elements(BuiltInCategory.OST_HVAC_Zones);
                Enable_Elements(BuiltInCategory.OST_Mass);
                Enable_Elements(BuiltInCategory.OST_MechanicalEquipment);
                Enable_Elements(BuiltInCategory.OST_MedicalEquipment);
                Enable_Elements(BuiltInCategory.OST_FabricationDuctwork);
                Enable_Elements(BuiltInCategory.OST_FabricationHangers);
                Enable_Elements(BuiltInCategory.OST_Parts);
                Enable_Elements(BuiltInCategory.OST_RasterImages);
                Enable_Elements(BuiltInCategory.OST_Signage);
                Enable_Elements(BuiltInCategory.OST_MEPSpaces);
                Enable_Elements(BuiltInCategory.OST_TemporaryStructure);
                Enable_Elements(BuiltInCategory.OST_VerticalCirculation);


                btn_mech.BackColor = Color.White;
            }
        }
        private void btn_strc_Click(object sender, EventArgs e)
        {
            if (btn_strc.BackColor != Color.RoyalBlue)
            {
                Disable_Elements(BuiltInCategory.OST_AudioVisualDevices);
                Disable_Elements(BuiltInCategory.OST_Columns);
                Disable_Elements(BuiltInCategory.OST_DetailComponents);
                Disable_Elements(BuiltInCategory.OST_FireProtection);
                Disable_CeilingsAndFloor(BuiltInCategory.OST_Floors);
                Disable_Elements(BuiltInCategory.OST_FoodServiceEquipment);
                Disable_Elements(BuiltInCategory.OST_GenericModel);
                Disable_Elements(BuiltInCategory.OST_Mass);
                Disable_Elements(BuiltInCategory.OST_MedicalEquipment);
                Disable_Elements(BuiltInCategory.OST_Parts);
                Disable_Elements(BuiltInCategory.OST_RasterImages);
                Disable_Elements(BuiltInCategory.OST_Signage);
                Disable_Elements(BuiltInCategory.OST_ShaftOpening);
                Disable_Elements(BuiltInCategory.OST_Stairs);
                Disable_Elements(BuiltInCategory.OST_StructuralColumns);
                Disable_Elements(BuiltInCategory.OST_StructuralConnectionHandler_Deprecated);
                Disable_Elements(BuiltInCategory.OST_FabricAreas);
                Disable_Elements(BuiltInCategory.OST_FabricReinforcement);
                Disable_Elements(BuiltInCategory.OST_StructuralFoundation);
                Disable_Elements(BuiltInCategory.OST_StructuralFraming);
                Disable_Elements(BuiltInCategory.OST_Rebar);
                Disable_Elements(BuiltInCategory.OST_RebarCover);
                Disable_Elements(BuiltInCategory.OST_StructuralStiffener);
                Disable_Elements(BuiltInCategory.OST_StructuralTruss);
                Disable_Elements(BuiltInCategory.OST_TemporaryStructure);
                Disable_Elements(BuiltInCategory.OST_VerticalCirculation);

                btn_strc.BackColor = Color.RoyalBlue;
            }

            else if (btn_strc.BackColor == Color.RoyalBlue)
            {
                Enable_Elements(BuiltInCategory.OST_AudioVisualDevices);
                Enable_Elements(BuiltInCategory.OST_Columns);
                Enable_Elements(BuiltInCategory.OST_DetailComponents);
                Enable_Elements(BuiltInCategory.OST_FireProtection);
                Enable_CeilingsAndFloor(BuiltInCategory.OST_Floors);
                Enable_Elements(BuiltInCategory.OST_FoodServiceEquipment);
                Enable_Elements(BuiltInCategory.OST_GenericModel);
                Enable_Elements(BuiltInCategory.OST_Mass);
                Enable_Elements(BuiltInCategory.OST_MedicalEquipment);
                Enable_Elements(BuiltInCategory.OST_Parts);
                Enable_Elements(BuiltInCategory.OST_RasterImages);
                Enable_Elements(BuiltInCategory.OST_Signage);
                Enable_Elements(BuiltInCategory.OST_ShaftOpening);
                Enable_Elements(BuiltInCategory.OST_Stairs);
                Enable_Elements(BuiltInCategory.OST_StructuralColumns);
                Enable_Elements(BuiltInCategory.OST_StructuralConnectionHandler_Deprecated);
                Enable_Elements(BuiltInCategory.OST_FabricAreas);
                Enable_Elements(BuiltInCategory.OST_FabricReinforcement);
                Enable_Elements(BuiltInCategory.OST_StructuralFoundation);
                Enable_Elements(BuiltInCategory.OST_StructuralFraming);
                Enable_Elements(BuiltInCategory.OST_Rebar);
                Enable_Elements(BuiltInCategory.OST_RebarCover);
                Enable_Elements(BuiltInCategory.OST_StructuralStiffener);
                Enable_Elements(BuiltInCategory.OST_StructuralTruss);
                Enable_Elements(BuiltInCategory.OST_TemporaryStructure);
                Enable_Elements(BuiltInCategory.OST_VerticalCirculation);

                btn_strc.BackColor = Color.White;
            }
        }
        private void btn_elec_Click(object sender, EventArgs e)
        {
            if (btn_elec.BackColor != Color.RoyalBlue)
            {
                Disable_Elements(BuiltInCategory.OST_Areas);
                Disable_Elements(BuiltInCategory.OST_AudioVisualDevices);
                Disable_Elements(BuiltInCategory.OST_CableTrayFitting);
                Disable_Elements(BuiltInCategory.OST_CableTray);
                Disable_Elements(BuiltInCategory.OST_CommunicationDevices);
                Disable_Elements(BuiltInCategory.OST_ConduitFitting);
                Disable_Elements(BuiltInCategory.OST_Conduit);
                Disable_Elements(BuiltInCategory.OST_DataDevices);
                Disable_Elements(BuiltInCategory.OST_ElectricalEquipment);
                Disable_Elements(BuiltInCategory.OST_ElectricalFixtures);
                Disable_Elements(BuiltInCategory.OST_FireAlarmDevices);
                Disable_Elements(BuiltInCategory.OST_FireProtection);
                Disable_Elements(BuiltInCategory.OST_FoodServiceEquipment);
                Disable_Elements(BuiltInCategory.OST_GenericModel);
                Disable_Elements(BuiltInCategory.OST_LightingDevices);
                Disable_Elements(BuiltInCategory.OST_FoodServiceEquipment);
                Disable_Elements(BuiltInCategory.OST_LightingFixtures);
                Disable_Elements(BuiltInCategory.OST_MedicalEquipment);
                Disable_Elements(BuiltInCategory.OST_FabricationContainment);
                Disable_Elements(BuiltInCategory.OST_FabricationHangers);
                Disable_Elements(BuiltInCategory.OST_NurseCallDevices);
                Disable_Elements(BuiltInCategory.OST_Parts);
                Disable_Elements(BuiltInCategory.OST_RasterImages);
                Disable_Elements(BuiltInCategory.OST_SecurityDevices);
                Disable_Elements(BuiltInCategory.OST_Signage);
                Disable_Elements(BuiltInCategory.OST_MEPSpaces);
                Disable_Elements(BuiltInCategory.OST_TelephoneDevices);
                Disable_Elements(BuiltInCategory.OST_Wire);
                Disable_Elements(BuiltInCategory.OST_TemporaryStructure);
                Disable_Elements(BuiltInCategory.OST_VerticalCirculation);

                btn_elec.BackColor = Color.RoyalBlue;
            }
            else if (btn_elec.BackColor == Color.RoyalBlue)
            {
                Enable_Elements(BuiltInCategory.OST_Areas);
                Enable_Elements(BuiltInCategory.OST_AudioVisualDevices);
                Enable_Elements(BuiltInCategory.OST_CableTrayFitting);
                Enable_Elements(BuiltInCategory.OST_CableTray);
                Enable_Elements(BuiltInCategory.OST_CommunicationDevices);
                Enable_Elements(BuiltInCategory.OST_ConduitFitting);
                Enable_Elements(BuiltInCategory.OST_Conduit);
                Enable_Elements(BuiltInCategory.OST_DataDevices);
                Enable_Elements(BuiltInCategory.OST_ElectricalEquipment);
                Enable_Elements(BuiltInCategory.OST_ElectricalFixtures);
                Enable_Elements(BuiltInCategory.OST_FireAlarmDevices);
                Enable_Elements(BuiltInCategory.OST_FireProtection);
                Enable_Elements(BuiltInCategory.OST_FoodServiceEquipment);
                Enable_Elements(BuiltInCategory.OST_GenericModel);
                Enable_Elements(BuiltInCategory.OST_LightingDevices);
                Enable_Elements(BuiltInCategory.OST_FoodServiceEquipment);
                Enable_Elements(BuiltInCategory.OST_LightingFixtures);
                Enable_Elements(BuiltInCategory.OST_MedicalEquipment);
                Enable_Elements(BuiltInCategory.OST_FabricationContainment);
                Enable_Elements(BuiltInCategory.OST_FabricationHangers);
                Enable_Elements(BuiltInCategory.OST_NurseCallDevices);
                Enable_Elements(BuiltInCategory.OST_Parts);
                Enable_Elements(BuiltInCategory.OST_RasterImages);
                Enable_Elements(BuiltInCategory.OST_SecurityDevices);
                Enable_Elements(BuiltInCategory.OST_Signage);
                Enable_Elements(BuiltInCategory.OST_MEPSpaces);
                Enable_Elements(BuiltInCategory.OST_TelephoneDevices);
                Enable_Elements(BuiltInCategory.OST_Wire);
                Enable_Elements(BuiltInCategory.OST_TemporaryStructure);
                Enable_Elements(BuiltInCategory.OST_VerticalCirculation);

                btn_elec.BackColor = Color.White;
            }
        }
        private void btn_plum_Click(object sender, EventArgs e)
        {
            if (btn_plum.BackColor != Color.RoyalBlue)
            {
                Disable_Elements(BuiltInCategory.OST_Areas);
                Disable_Elements(BuiltInCategory.OST_AudioVisualDevices);
                Disable_Elements(BuiltInCategory.OST_DetailComponents);
                Disable_Elements(BuiltInCategory.OST_FireProtection);
                Disable_Elements(BuiltInCategory.OST_FlexPipeCurves);
                Disable_Elements(BuiltInCategory.OST_FoodServiceEquipment);
                Disable_Elements(BuiltInCategory.OST_GenericModel);
                Disable_Elements(BuiltInCategory.OST_Mass);
                Disable_Elements(BuiltInCategory.OST_MechanicalEquipment);
                Disable_Elements(BuiltInCategory.OST_MedicalEquipment);
                Disable_Elements(BuiltInCategory.OST_FabricationDuctwork);
                Disable_Elements(BuiltInCategory.OST_FabricationHangers);
                Disable_Elements(BuiltInCategory.OST_FabricationPipework);
                Disable_Elements(BuiltInCategory.OST_Parts);
                Disable_Elements(BuiltInCategory.OST_PipeAccessory);
                Disable_Elements(BuiltInCategory.OST_PipeFitting);
                Disable_Elements(BuiltInCategory.OST_PipeInsulations);
                Disable_Elements(BuiltInCategory.OST_PlaceHolderPipes);
                Disable_Elements(BuiltInCategory.OST_PipeSegments);
                Disable_Elements(BuiltInCategory.OST_PlumbingFixtures);
                Disable_Elements(BuiltInCategory.OST_RasterImages);
                Disable_Elements(BuiltInCategory.OST_Signage);
                Disable_Elements(BuiltInCategory.OST_MEPSpaces);
                Disable_Elements(BuiltInCategory.OST_Sprinklers);
                Disable_Elements(BuiltInCategory.OST_TemporaryStructure);
                Disable_Elements(BuiltInCategory.OST_VerticalCirculation);

                btn_plum.BackColor = Color.RoyalBlue;
            }
            else if (btn_plum.BackColor == Color.RoyalBlue)
            {
                Enable_Elements(BuiltInCategory.OST_Areas);
                Enable_Elements(BuiltInCategory.OST_AudioVisualDevices);
                Enable_Elements(BuiltInCategory.OST_DetailComponents);
                Enable_Elements(BuiltInCategory.OST_FireProtection);
                Enable_Elements(BuiltInCategory.OST_FlexPipeCurves);
                Enable_Elements(BuiltInCategory.OST_FoodServiceEquipment);
                Enable_Elements(BuiltInCategory.OST_GenericModel);
                Enable_Elements(BuiltInCategory.OST_Mass);
                Enable_Elements(BuiltInCategory.OST_MechanicalEquipment);
                Enable_Elements(BuiltInCategory.OST_MedicalEquipment);
                Enable_Elements(BuiltInCategory.OST_FabricationDuctwork);
                Enable_Elements(BuiltInCategory.OST_FabricationHangers);
                Enable_Elements(BuiltInCategory.OST_FabricationPipework);
                Enable_Elements(BuiltInCategory.OST_Parts);
                Enable_Elements(BuiltInCategory.OST_PipeAccessory);
                Enable_Elements(BuiltInCategory.OST_PipeFitting);
                Enable_Elements(BuiltInCategory.OST_PipeInsulations);
                Enable_Elements(BuiltInCategory.OST_PlaceHolderPipes);
                Enable_Elements(BuiltInCategory.OST_PipeSegments);
                Enable_Elements(BuiltInCategory.OST_PlumbingFixtures);
                Enable_Elements(BuiltInCategory.OST_RasterImages);
                Enable_Elements(BuiltInCategory.OST_Signage);
                Enable_Elements(BuiltInCategory.OST_MEPSpaces);
                Enable_Elements(BuiltInCategory.OST_Sprinklers);
                Enable_Elements(BuiltInCategory.OST_TemporaryStructure);
                Enable_Elements(BuiltInCategory.OST_VerticalCirculation);

                btn_plum.BackColor = Color.White;
            }
        }
        private void btn_infra_Click(object sender, EventArgs e)
        {
            if (btn_infra.BackColor != Color.RoyalBlue)
            {
                Disable_Elements(BuiltInCategory.OST_AbutmentFoundations);
                Disable_Elements(BuiltInCategory.OST_AudioVisualDevices);
                Disable_Elements(BuiltInCategory.OST_BridgeBearings);
                Disable_Elements(BuiltInCategory.OST_BridgeCables);
                Disable_Elements(BuiltInCategory.OST_AreaColorFill);
                Disable_Elements(BuiltInCategory.OST_AreaInteriorFill);
                Disable_Elements(BuiltInCategory.OST_BridgeDecks);
                Disable_Elements(BuiltInCategory.OST_BridgeFraming);
                Disable_Elements(BuiltInCategory.OST_DetailComponents);
                Disable_Elements(BuiltInCategory.OST_ExpansionJoints);
                Disable_Elements(BuiltInCategory.OST_FireProtection);
                Disable_Elements(BuiltInCategory.OST_FoodServiceEquipment);
                Disable_Elements(BuiltInCategory.OST_GenericModel);
                Disable_Elements(BuiltInCategory.OST_LightingFixtures);
                Disable_Elements(BuiltInCategory.OST_Lines);
                Disable_Elements(BuiltInCategory.OST_Mass);
                Disable_Elements(BuiltInCategory.OST_MedicalEquipment);
                Disable_Elements(BuiltInCategory.OST_Parts);
                Disable_Elements(BuiltInCategory.OST_BridgePiers);
                Disable_Railings(BuiltInCategory.OST_Railings);
                Disable_Elements(BuiltInCategory.OST_RasterImages);
                Disable_Elements(BuiltInCategory.OST_Signage);
                Disable_Elements(BuiltInCategory.OST_Site);
                Disable_Elements(BuiltInCategory.OST_AreaRein);
                Disable_Elements(BuiltInCategory.OST_StructuralColumns);
                Disable_Elements(BuiltInCategory.OST_StructConnections);
                Disable_Elements(BuiltInCategory.OST_FabricAreas);
                Disable_Elements(BuiltInCategory.OST_FabricReinforcement);
                Disable_Elements(BuiltInCategory.OST_StructuralFoundation);
                Disable_Elements(BuiltInCategory.OST_StructuralFraming);
                Disable_Elements(BuiltInCategory.OST_PathRein);
                Disable_Elements(BuiltInCategory.OST_Rebar);
                Disable_Elements(BuiltInCategory.OST_Coupler);
                Disable_Elements(BuiltInCategory.OST_StructuralStiffener);
                Disable_Elements(BuiltInCategory.OST_StructuralTendons);
                Disable_Elements(BuiltInCategory.OST_TemporaryStructure);
                Disable_Elements(BuiltInCategory.OST_VerticalCirculation);
                Disable_Elements(BuiltInCategory.OST_VibrationManagement);
                Disable_Elements(BuiltInCategory.OST_Windows);

                btn_infra.BackColor = Color.RoyalBlue;
            }
            else if (btn_infra.BackColor == Color.RoyalBlue)
            {
                Enable_Elements(BuiltInCategory.OST_AbutmentFoundations);
                Enable_Elements(BuiltInCategory.OST_AbutmentFoundations);
                Enable_Elements(BuiltInCategory.OST_AudioVisualDevices);
                Enable_Elements(BuiltInCategory.OST_BridgeBearings);
                Enable_Elements(BuiltInCategory.OST_BridgeCables);
                Enable_Elements(BuiltInCategory.OST_AreaColorFill);
                Enable_Elements(BuiltInCategory.OST_AreaInteriorFill);
                Enable_Elements(BuiltInCategory.OST_BridgeDecks);
                Enable_Elements(BuiltInCategory.OST_BridgeFraming);
                Enable_Elements(BuiltInCategory.OST_DetailComponents);
                Enable_Elements(BuiltInCategory.OST_ExpansionJoints);
                Enable_Elements(BuiltInCategory.OST_FireProtection);
                Enable_Elements(BuiltInCategory.OST_FoodServiceEquipment);
                Enable_Elements(BuiltInCategory.OST_GenericModel);
                Enable_Elements(BuiltInCategory.OST_LightingFixtures);
                Enable_Elements(BuiltInCategory.OST_Lines);
                Enable_Elements(BuiltInCategory.OST_Mass);
                Enable_Elements(BuiltInCategory.OST_MedicalEquipment);
                Enable_Elements(BuiltInCategory.OST_Parts);
                Enable_Elements(BuiltInCategory.OST_BridgePiers);
                Enable_Railings(BuiltInCategory.OST_Railings);
                Enable_Elements(BuiltInCategory.OST_RasterImages);
                Enable_Elements(BuiltInCategory.OST_Signage);
                Enable_Elements(BuiltInCategory.OST_Site);
                Enable_Elements(BuiltInCategory.OST_AreaRein);
                Enable_Elements(BuiltInCategory.OST_StructuralColumns);
                Enable_Elements(BuiltInCategory.OST_StructConnections);
                Enable_Elements(BuiltInCategory.OST_FabricAreas);
                Enable_Elements(BuiltInCategory.OST_FabricReinforcement);
                Enable_Elements(BuiltInCategory.OST_StructuralFoundation);
                Enable_Elements(BuiltInCategory.OST_StructuralFraming);
                Enable_Elements(BuiltInCategory.OST_PathRein);
                Enable_Elements(BuiltInCategory.OST_Rebar);
                Enable_Elements(BuiltInCategory.OST_Coupler);
                Enable_Elements(BuiltInCategory.OST_StructuralStiffener);
                Enable_Elements(BuiltInCategory.OST_StructuralTendons);
                Enable_Elements(BuiltInCategory.OST_TemporaryStructure);
                Enable_Elements(BuiltInCategory.OST_VerticalCirculation);
                Enable_Elements(BuiltInCategory.OST_VibrationManagement);
                Enable_Elements(BuiltInCategory.OST_Windows);

                btn_infra.BackColor = Color.White;
            }
        }
        private void chkbox_model_CheckedChanged(object sender, EventArgs e)
        {
            /*
            if (chkbox_model.Checked == true)
            {
                Disable_CeilingsAndFloor(BuiltInCategory.OST_Ceilings);
                Disable_CeilingsAndFloor(BuiltInCategory.OST_Floors);
                Disable_Railings(BuiltInCategory.OST_Railings);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CableTray);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Cameras);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Casework);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CaseworkHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CeilingOpening);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CeilingsCut);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CeilingsCutPattern);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CeilingsDefault);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CeilingsFinish1);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CeilingsFinish2);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CeilingsHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CeilingsInsulation);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CeilingsMembrane);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CeilingsProjection);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CeilingsStructure);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CeilingsSubstrate);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CeilingsSurfacePattern);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Columns);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_ColumnsHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CommunicationDevices);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Conduit);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Curtain_Systems);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CurtainGrids);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CurtainGridsCurtaSystem);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CurtainGridsRoof);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CurtainGridsSystem);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CurtainGridsWall);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CurtainWallMullions);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CurtainWallMullionsCut);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CurtainWallMullionsHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CurtainWallPanels);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_CurtainWallPanelsHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_DataDevices);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Doors);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_DoorsFrameMullionCut);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_DoorsFrameMullionProjection);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_DoorsGlassCut);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_DoorsGlassProjection);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_DoorsHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_DoorsOpeningCut);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_DoorsOpeningProjection);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_DoorsPanelCut);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_DuctAccessory);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_DuctCurves);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_DuctFitting);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_DuctLinings);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_DuctSystem);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_ElectricalCircuit);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_ElectricalEquipment);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_ElectricalEquipmentHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_ElectricalFixtures);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_ElectricalFixturesHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_ElectricalLoadClassifications);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Entourage);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Fixtures);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_FloorOpening);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Furniture);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_FurnitureHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_FurnitureSystems);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_FurnitureSystemsHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_GenericModel);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_GenericModelHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_LightingDevices);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_LightingFixtures);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_LightingFixturesHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_LightingFixtureSource);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_LinesHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Mass);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_MechanicalEquipment);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_MechanicalEquipmentHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Parking);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_ParkingHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_PartHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_PipeInsulations);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_PipeSegments);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_PipingSystem);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Planting);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_PlantingHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_PlumbingFixtures);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_PlumbingFixturesHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSupport);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystem);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemBaluster);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemBalusterHiddenLines_Deprecated);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemHandRail);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemHandRailBracket);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemHandRailBracketHiddenLines_Deprecated);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemHandRailHiddenLines_Deprecated);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemHardware);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemHiddenLines_Deprecated);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemPanel);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemPanelBracketHiddenLines_Deprecated);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemPanelHiddenLines_Deprecated);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemPost);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemPostHiddenLines_Deprecated);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemRail);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemRailHiddenLines_Deprecated);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemSegment);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemSegmentHiddenLines_Deprecated);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemTermination);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemTerminationHiddenLines_Deprecated);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemTopRail);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemTopRailHiddenLines_Deprecated);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemTransition);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingSystemTransitionHiddenLines_Deprecated);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingTermination);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingTopRail);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RailingTopRailAboveCut);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Ramps);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RampsHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Reveals);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Roads);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RoofOpening);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Roofs);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RoofsCut);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RoofsFinish1);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RoofsFinish2);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RoofsHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RoofsInsulation);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RoofsInteriorEdges);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RoofsMembrane);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RoofSoffit);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RoofsProjection);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RoofsStructure);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_RvtLinks);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_SecurityDevices);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_SpecialityEquipment);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_SpecialityEquipmentHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Sprinklers);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Stairs);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_StairsHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_StairsRailing);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_StairsRailingHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Topography);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_TopographyContours);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_TopographyHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_TopographyLink);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_TopographySurface);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Truss);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Windows);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_WindowsHiddenLines);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_Wire);
                Dis_Ele_By_CheckBox_Model(BuiltInCategory.OST_WireInsulations);

                this.Controls.Add(chkbox_model);

            }
            else if (chkbox_model.Checked == false)
            {
                Enable_CeilingsAndFloor(BuiltInCategory.OST_Ceilings);
                Enable_CeilingsAndFloor(BuiltInCategory.OST_Floors);
                Enable_Railings(BuiltInCategory.OST_Railings);
                Enable_Elements(BuiltInCategory.OST_CableTray);
                Enable_Elements(BuiltInCategory.OST_Cameras);
                Enable_Elements(BuiltInCategory.OST_Casework);
                Enable_Elements(BuiltInCategory.OST_CaseworkHiddenLines);
                Enable_Elements(BuiltInCategory.OST_CeilingOpening);
                Enable_Elements(BuiltInCategory.OST_CeilingsCut);
                Enable_Elements(BuiltInCategory.OST_CeilingsCutPattern);
                Enable_Elements(BuiltInCategory.OST_CeilingsDefault);
                Enable_Elements(BuiltInCategory.OST_CeilingsFinish1);
                Enable_Elements(BuiltInCategory.OST_CeilingsFinish2);
                Enable_Elements(BuiltInCategory.OST_CeilingsHiddenLines);
                Enable_Elements(BuiltInCategory.OST_CeilingsInsulation);
                Enable_Elements(BuiltInCategory.OST_CeilingsMembrane);
                Enable_Elements(BuiltInCategory.OST_CeilingsProjection);
                Enable_Elements(BuiltInCategory.OST_CeilingsStructure);
                Enable_Elements(BuiltInCategory.OST_CeilingsSubstrate);
                Enable_Elements(BuiltInCategory.OST_CeilingsSurfacePattern);
                Enable_Elements(BuiltInCategory.OST_Columns);
                Enable_Elements(BuiltInCategory.OST_ColumnsHiddenLines);
                Enable_Elements(BuiltInCategory.OST_CommunicationDevices);
                Enable_Elements(BuiltInCategory.OST_Conduit);
                Enable_Elements(BuiltInCategory.OST_Curtain_Systems);
                Enable_Elements(BuiltInCategory.OST_CurtainGrids);
                Enable_Elements(BuiltInCategory.OST_CurtainGridsCurtaSystem);
                Enable_Elements(BuiltInCategory.OST_CurtainGridsRoof);
                Enable_Elements(BuiltInCategory.OST_CurtainGridsSystem);
                Enable_Elements(BuiltInCategory.OST_CurtainGridsWall);
                Enable_Elements(BuiltInCategory.OST_CurtainWallMullions);
                Enable_Elements(BuiltInCategory.OST_CurtainWallMullionsCut);
                Enable_Elements(BuiltInCategory.OST_CurtainWallMullionsHiddenLines);
                Enable_Elements(BuiltInCategory.OST_CurtainWallPanels);
                Enable_Elements(BuiltInCategory.OST_CurtainWallPanelsHiddenLines);
                Enable_Elements(BuiltInCategory.OST_DataDevices);
                Enable_Elements(BuiltInCategory.OST_Doors);
                Enable_Elements(BuiltInCategory.OST_DoorsFrameMullionCut);
                Enable_Elements(BuiltInCategory.OST_DoorsFrameMullionProjection);
                Enable_Elements(BuiltInCategory.OST_DoorsGlassCut);
                Enable_Elements(BuiltInCategory.OST_DoorsGlassProjection);
                Enable_Elements(BuiltInCategory.OST_DoorsHiddenLines);
                Enable_Elements(BuiltInCategory.OST_DoorsOpeningCut);
                Enable_Elements(BuiltInCategory.OST_DoorsOpeningProjection);
                Enable_Elements(BuiltInCategory.OST_DoorsPanelCut);
                Enable_Elements(BuiltInCategory.OST_DuctAccessory);
                Enable_Elements(BuiltInCategory.OST_DuctCurves);
                Enable_Elements(BuiltInCategory.OST_DuctFitting);
                Enable_Elements(BuiltInCategory.OST_DuctLinings);
                Enable_Elements(BuiltInCategory.OST_DuctSystem);
                Enable_Elements(BuiltInCategory.OST_ElectricalCircuit);
                Enable_Elements(BuiltInCategory.OST_ElectricalEquipment);
                Enable_Elements(BuiltInCategory.OST_ElectricalEquipmentHiddenLines);
                Enable_Elements(BuiltInCategory.OST_ElectricalFixtures);
                Enable_Elements(BuiltInCategory.OST_ElectricalFixturesHiddenLines);
                Enable_Elements(BuiltInCategory.OST_ElectricalLoadClassifications);
                Enable_Elements(BuiltInCategory.OST_Entourage);
                Enable_Elements(BuiltInCategory.OST_Fixtures);
                Enable_Elements(BuiltInCategory.OST_FloorOpening);
                Enable_Elements(BuiltInCategory.OST_Furniture);
                Enable_Elements(BuiltInCategory.OST_FurnitureHiddenLines);
                Enable_Elements(BuiltInCategory.OST_FurnitureSystems);
                Enable_Elements(BuiltInCategory.OST_FurnitureSystemsHiddenLines);
                Enable_Elements(BuiltInCategory.OST_GenericModel);
                Enable_Elements(BuiltInCategory.OST_GenericModelHiddenLines);
                Enable_Elements(BuiltInCategory.OST_LightingDevices);
                Enable_Elements(BuiltInCategory.OST_LightingFixtures);
                Enable_Elements(BuiltInCategory.OST_LightingFixturesHiddenLines);
                Enable_Elements(BuiltInCategory.OST_LightingFixtureSource);
                Enable_Elements(BuiltInCategory.OST_LinesHiddenLines);
                Enable_Elements(BuiltInCategory.OST_Mass);
                Enable_Elements(BuiltInCategory.OST_MechanicalEquipment);
                Enable_Elements(BuiltInCategory.OST_MechanicalEquipmentHiddenLines);
                Enable_Elements(BuiltInCategory.OST_Parking);
                Enable_Elements(BuiltInCategory.OST_ParkingHiddenLines);
                Enable_Elements(BuiltInCategory.OST_PartHiddenLines);
                Enable_Elements(BuiltInCategory.OST_PipeInsulations);
                Enable_Elements(BuiltInCategory.OST_PipeSegments);
                Enable_Elements(BuiltInCategory.OST_PipingSystem);
                Enable_Elements(BuiltInCategory.OST_Planting);
                Enable_Elements(BuiltInCategory.OST_PlantingHiddenLines);
                Enable_Elements(BuiltInCategory.OST_PlumbingFixtures);
                Enable_Elements(BuiltInCategory.OST_PlumbingFixturesHiddenLines);
                Enable_Elements(BuiltInCategory.OST_RailingSupport);
                Enable_Elements(BuiltInCategory.OST_RailingSystem);
                Enable_Elements(BuiltInCategory.OST_RailingSystemBaluster);
                Enable_Elements(BuiltInCategory.OST_RailingSystemBalusterHiddenLines_Deprecated);
                Enable_Elements(BuiltInCategory.OST_RailingSystemHandRail);
                Enable_Elements(BuiltInCategory.OST_RailingSystemHandRailBracket);
                Enable_Elements(BuiltInCategory.OST_RailingSystemHandRailBracketHiddenLines_Deprecated);
                Enable_Elements(BuiltInCategory.OST_RailingSystemHandRailHiddenLines_Deprecated);
                Enable_Elements(BuiltInCategory.OST_RailingSystemHardware);
                Enable_Elements(BuiltInCategory.OST_RailingSystemHiddenLines_Deprecated);
                Enable_Elements(BuiltInCategory.OST_RailingSystemPanel);
                Enable_Elements(BuiltInCategory.OST_RailingSystemPanelBracketHiddenLines_Deprecated);
                Enable_Elements(BuiltInCategory.OST_RailingSystemPanelHiddenLines_Deprecated);
                Enable_Elements(BuiltInCategory.OST_RailingSystemPost);
                Enable_Elements(BuiltInCategory.OST_RailingSystemPostHiddenLines_Deprecated);
                Enable_Elements(BuiltInCategory.OST_RailingSystemRail);
                Enable_Elements(BuiltInCategory.OST_RailingSystemRailHiddenLines_Deprecated);
                Enable_Elements(BuiltInCategory.OST_RailingSystemSegment);
                Enable_Elements(BuiltInCategory.OST_RailingSystemSegmentHiddenLines_Deprecated);
                Enable_Elements(BuiltInCategory.OST_RailingSystemTermination);
                Enable_Elements(BuiltInCategory.OST_RailingSystemTerminationHiddenLines_Deprecated);
                Enable_Elements(BuiltInCategory.OST_RailingSystemTopRail);
                Enable_Elements(BuiltInCategory.OST_RailingSystemTopRailHiddenLines_Deprecated);
                Enable_Elements(BuiltInCategory.OST_RailingSystemTransition);
                Enable_Elements(BuiltInCategory.OST_RailingSystemTransitionHiddenLines_Deprecated);
                Enable_Elements(BuiltInCategory.OST_RailingTermination);
                Enable_Elements(BuiltInCategory.OST_RailingTopRail);
                Enable_Elements(BuiltInCategory.OST_RailingTopRailAboveCut);
                Enable_Elements(BuiltInCategory.OST_Ramps);
                Enable_Elements(BuiltInCategory.OST_RampsHiddenLines);
                Enable_Elements(BuiltInCategory.OST_Reveals);
                Enable_Elements(BuiltInCategory.OST_Roads);
                Enable_Elements(BuiltInCategory.OST_RoofOpening);
                Enable_Elements(BuiltInCategory.OST_Roofs);
                Enable_Elements(BuiltInCategory.OST_RoofsCut);
                Enable_Elements(BuiltInCategory.OST_RoofsFinish1);
                Enable_Elements(BuiltInCategory.OST_RoofsFinish2);
                Enable_Elements(BuiltInCategory.OST_RoofsHiddenLines);
                Enable_Elements(BuiltInCategory.OST_RoofsInsulation);
                Enable_Elements(BuiltInCategory.OST_RoofsInteriorEdges);
                Enable_Elements(BuiltInCategory.OST_RoofsMembrane);
                Enable_Elements(BuiltInCategory.OST_RoofSoffit);
                Enable_Elements(BuiltInCategory.OST_RoofsProjection);
                Enable_Elements(BuiltInCategory.OST_RoofsStructure);
                Enable_Elements(BuiltInCategory.OST_RvtLinks);
                Enable_Elements(BuiltInCategory.OST_SecurityDevices);
                Enable_Elements(BuiltInCategory.OST_SpecialityEquipment);
                Enable_Elements(BuiltInCategory.OST_SpecialityEquipmentHiddenLines);
                Enable_Elements(BuiltInCategory.OST_Sprinklers);
                Enable_Elements(BuiltInCategory.OST_Stairs);
                Enable_Elements(BuiltInCategory.OST_StairsHiddenLines);
                Enable_Elements(BuiltInCategory.OST_StairsRailing);
                Enable_Elements(BuiltInCategory.OST_StairsRailingHiddenLines);
                Enable_Elements(BuiltInCategory.OST_Topography);
                Enable_Elements(BuiltInCategory.OST_TopographyContours);
                Enable_Elements(BuiltInCategory.OST_TopographyHiddenLines);
                Enable_Elements(BuiltInCategory.OST_TopographyLink);
                Enable_Elements(BuiltInCategory.OST_TopographySurface);
                Enable_Elements(BuiltInCategory.OST_Truss);
                Enable_Elements(BuiltInCategory.OST_Windows);
                Enable_Elements(BuiltInCategory.OST_WindowsHiddenLines);
                Enable_Elements(BuiltInCategory.OST_Wire);
                Enable_Elements(BuiltInCategory.OST_WireInsulations);


            }

            */
        }
        private void chk_cad_import_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_cad_import.Checked == true)
            {
                Disable_cad_import(BuiltInCategory.OST_ImportObjectStyles);
            }
            else if (chk_cad_import.Checked == false)
            {
                Enable_cad_import(BuiltInCategory.OST_ImportObjectStyles);
            }
        }
        private void chkbox_anno_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbox_anno.Checked == true)
            {
                Disable_Anno_Tags(BuiltInCategory.OST_MassTags_Obsolete_IdInWrongRange);
                Disable_Anno_Tags(BuiltInCategory.OST_StickSymbols_Obsolete_IdInWrongRange);
                Disable_Anno_Tags(BuiltInCategory.OST_FoundationSlabAnalyticalTags);
                Disable_Anno_Tags(BuiltInCategory.OST_WallFoundationAnalyticalTags);
                Disable_Anno_Tags(BuiltInCategory.OST_IsolatedFoundationAnalyticalTags);
                Disable_Anno_Tags(BuiltInCategory.OST_WallAnalyticalTags);
                Disable_Anno_Tags(BuiltInCategory.OST_FloorAnalyticalTags);
                Disable_Anno_Tags(BuiltInCategory.OST_ColumnAnalyticalTags);
                Disable_Anno_Tags(BuiltInCategory.OST_BraceAnalyticalTags);
                Disable_Anno_Tags(BuiltInCategory.OST_BeamAnalyticalTags);
                Disable_Anno_Tags(BuiltInCategory.OST_CompassSectionFilled);
                Disable_Anno_Tags(BuiltInCategory.OST_SunsetText);
                Disable_Anno_Tags(BuiltInCategory.OST_CompassSection);
                Disable_Anno_Tags(BuiltInCategory.OST_SunriseText);
                Disable_Anno_Tags(BuiltInCategory.OST_StructuralTrussStickSymbols);
                Disable_Anno_Tags(BuiltInCategory.OST_StructConnectionProfilesTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StructConnectionHoleTags);
                Disable_Anno_Tags(BuiltInCategory.OST_CouplerTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StructConnectionWeldTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StructConnectionShearStudTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StructConnectionAnchorTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StructConnectionBoltTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StructConnectionPlateTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StructConnectionTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StructConnectionSymbol);
                Disable_Anno_Tags(BuiltInCategory.OST_FabricReinSpanSymbol);
                Disable_Anno_Tags(BuiltInCategory.OST_FabricAreaTags);
                Disable_Anno_Tags(BuiltInCategory.OST_FabricReinforcementTags);
                Disable_Anno_Tags(BuiltInCategory.OST_AreaReinTags);
                Disable_Anno_Tags(BuiltInCategory.OST_RebarTags);
                Disable_Anno_Tags(BuiltInCategory.OST_PathReinTags);
                Disable_Anno_Tags(BuiltInCategory.OST_PathReinSpanSymbol);
                Disable_Anno_Tags(BuiltInCategory.OST_AreaReinSpanSymbol);
                Disable_Anno_Tags(BuiltInCategory.OST_FabricationContainmentSymbology);
                Disable_Anno_Tags(BuiltInCategory.OST_FabricationContainmentTags);
                Disable_Anno_Tags(BuiltInCategory.OST_FabricationPipeworkSymbology);
                Disable_Anno_Tags(BuiltInCategory.OST_FabricationPipeworkTags);
                Disable_Anno_Tags(BuiltInCategory.OST_FabricationDuctworkSymbology);
                Disable_Anno_Tags(BuiltInCategory.OST_FabricationHangerTags);
                Disable_Anno_Tags(BuiltInCategory.OST_FabricationDuctworkTags);
                Disable_Anno_Tags(BuiltInCategory.OST_PipeInsulationsTags);
                Disable_Anno_Tags(BuiltInCategory.OST_DuctLiningsTags);
                Disable_Anno_Tags(BuiltInCategory.OST_DuctInsulationsTags);
                Disable_Anno_Tags(BuiltInCategory.OST_ConduitTags);
                Disable_Anno_Tags(BuiltInCategory.OST_CableTrayTags);
                Disable_Anno_Tags(BuiltInCategory.OST_ConduitFittingTags);
                Disable_Anno_Tags(BuiltInCategory.OST_CableTrayFittingTags);
                Disable_Anno_Tags(BuiltInCategory.OST_HVAC_Zones_InteriorFill_Visibility);
                Disable_Anno_Tags(BuiltInCategory.OST_HVAC_Zones_ColorFill);
                Disable_Anno_Tags(BuiltInCategory.OST_ZoneTags);
                Disable_Anno_Tags(BuiltInCategory.OST_HVAC_Zones_InteriorFill);
                Disable_Anno_Tags(BuiltInCategory.OST_SprinklerTags);
                Disable_Anno_Tags(BuiltInCategory.OST_LightingDeviceTags);
                Disable_Anno_Tags(BuiltInCategory.OST_FireAlarmDeviceTags);
                Disable_Anno_Tags(BuiltInCategory.OST_DataDeviceTags);
                Disable_Anno_Tags(BuiltInCategory.OST_CommunicationDeviceTags);
                Disable_Anno_Tags(BuiltInCategory.OST_SecurityDeviceTags);
                Disable_Anno_Tags(BuiltInCategory.OST_NurseCallDeviceTags);
                Disable_Anno_Tags(BuiltInCategory.OST_TelephoneDeviceTags);
                Disable_Anno_Tags(BuiltInCategory.OST_DuctFittingTags);
                Disable_Anno_Tags(BuiltInCategory.OST_PipeFittingTags);
                Disable_Anno_Tags(BuiltInCategory.OST_PipeColorFills);
                Disable_Anno_Tags(BuiltInCategory.OST_PipeColorFillLegends);
                Disable_Anno_Tags(BuiltInCategory.OST_WireTags);
                Disable_Anno_Tags(BuiltInCategory.OST_PipeAccessoryTags);
                Disable_Anno_Tags(BuiltInCategory.OST_FlexPipeTags);
                Disable_Anno_Tags(BuiltInCategory.OST_PipeTags);
                Disable_Anno_Tags(BuiltInCategory.OST_ElectricalVoltage);
                Disable_Anno_Tags(BuiltInCategory.OST_ElectricalCircuitTags);
                Disable_Anno_Tags(BuiltInCategory.OST_DuctAccessoryTags);
                Disable_Anno_Tags(BuiltInCategory.OST_DuctTerminalTags);
                Disable_Anno_Tags(BuiltInCategory.OST_DuctColorFills);
                Disable_Anno_Tags(BuiltInCategory.OST_FlexDuctTags);
                Disable_Anno_Tags(BuiltInCategory.OST_DuctTags);
                Disable_Anno_Tags(BuiltInCategory.OST_DuctColorFillLegends);
                Disable_Anno_Tags(BuiltInCategory.OST_BridgeBearingTags);
                Disable_Anno_Tags(BuiltInCategory.OST_BridgeGirderTags2021_Deprecated);
                Disable_Anno_Tags(BuiltInCategory.OST_BridgeFoundationTags);
                Disable_Anno_Tags(BuiltInCategory.OST_BridgeDeckTags);
                Disable_Anno_Tags(BuiltInCategory.OST_BridgeArchTags2021_Deprecated);
                Disable_Anno_Tags(BuiltInCategory.OST_BridgeCableTags);
                Disable_Anno_Tags(BuiltInCategory.OST_BridgeTowerTags);
                Disable_Anno_Tags(BuiltInCategory.OST_BridgePierTags);
                Disable_Anno_Tags(BuiltInCategory.OST_BridgeAbutmentTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StructConnectionSymbols);
                Disable_Anno_Tags(BuiltInCategory.OST_RevisionCloudTags);
                Disable_Anno_Tags(BuiltInCategory.OST_Revisions);
                Disable_Anno_Tags(BuiltInCategory.OST_RevisionClouds);
                Disable_Anno_Tags(BuiltInCategory.OST_ElevationMarks);
                Disable_Anno_Tags(BuiltInCategory.OST_InternalAreaLoadTags);
                Disable_Anno_Tags(BuiltInCategory.OST_InternalLineLoadTags);
                Disable_Anno_Tags(BuiltInCategory.OST_InternalPointLoadTags);
                Disable_Anno_Tags(BuiltInCategory.OST_AreaLoadTags);
                Disable_Anno_Tags(BuiltInCategory.OST_LineLoadTags);
                Disable_Anno_Tags(BuiltInCategory.OST_PointLoadTags);
                Disable_Anno_Tags(BuiltInCategory.OST_BeamSystemTags);
                Disable_Anno_Tags(BuiltInCategory.OST_FootingSpanDirectionSymbol);
                Disable_Anno_Tags(BuiltInCategory.OST_SpanDirectionSymbol);
                Disable_Anno_Tags(BuiltInCategory.OST_SpotSlopesSymbols);
                Disable_Anno_Tags(BuiltInCategory.OST_SpotCoordinateSymbols);
                Disable_Anno_Tags(BuiltInCategory.OST_SpotElevSymbols);
                Disable_Anno_Tags(BuiltInCategory.OST_StructuralConnectionHandlerTags_Deprecated);
                Disable_Anno_Tags(BuiltInCategory.OST_TrussTags);
                Disable_Anno_Tags(BuiltInCategory.OST_KeynoteTags);
                Disable_Anno_Tags(BuiltInCategory.OST_DetailComponentTags);
                Disable_Anno_Tags(BuiltInCategory.OST_MaterialTags);
                Disable_Anno_Tags(BuiltInCategory.OST_FloorTags);
                Disable_Anno_Tags(BuiltInCategory.OST_CurtaSystemTags);
                Disable_Anno_Tags(BuiltInCategory.OST_HostFinTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StairsTags);
                Disable_Anno_Tags(BuiltInCategory.OST_MultiCategoryTags);
                Disable_Anno_Tags(BuiltInCategory.OST_PlantingTags);
                Disable_Anno_Tags(BuiltInCategory.OST_AreaTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StructuralFoundationTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StructuralColumnTags);
                Disable_Anno_Tags(BuiltInCategory.OST_ParkingTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StructuralFramingTags);
                Disable_Anno_Tags(BuiltInCategory.OST_SpecialityEquipmentTags);
                Disable_Anno_Tags(BuiltInCategory.OST_GenericModelTags);
                Disable_Anno_Tags(BuiltInCategory.OST_CurtainWallPanelTags);
                Disable_Anno_Tags(BuiltInCategory.OST_WallTags);
                Disable_Anno_Tags(BuiltInCategory.OST_PlumbingFixtureTags);
                Disable_Anno_Tags(BuiltInCategory.OST_MechanicalEquipmentTags);
                Disable_Anno_Tags(BuiltInCategory.OST_LightingFixtureTags);
                Disable_Anno_Tags(BuiltInCategory.OST_FurnitureSystemTags);
                Disable_Anno_Tags(BuiltInCategory.OST_FurnitureTags);
                Disable_Anno_Tags(BuiltInCategory.OST_ElectricalFixtureTags);
                Disable_Anno_Tags(BuiltInCategory.OST_ElectricalEquipmentTags);
                Disable_Anno_Tags(BuiltInCategory.OST_CeilingTags);
                Disable_Anno_Tags(BuiltInCategory.OST_CaseworkTags);
                Disable_Anno_Tags(BuiltInCategory.OST_Tags);
                Disable_Anno_Tags(BuiltInCategory.OST_MEPSpaceColorFill);
                Disable_Anno_Tags(BuiltInCategory.OST_MEPSpaceInteriorFill);
                Disable_Anno_Tags(BuiltInCategory.OST_MEPSpaceInteriorFillVisibility);
                Disable_Anno_Tags(BuiltInCategory.OST_MassAreaFaceTags);
                Disable_Anno_Tags(BuiltInCategory.OST_MassTags);
                Disable_Anno_Tags(BuiltInCategory.OST_DividedSurface_PatternFill);
                Disable_Anno_Tags(BuiltInCategory.OST_RampsDownText);
                Disable_Anno_Tags(BuiltInCategory.OST_RampsUpText);
                Disable_Anno_Tags(BuiltInCategory.OST_StructuralStiffenerTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StructuralColumnStickSymbols);
                Disable_Anno_Tags(BuiltInCategory.OST_SitePropertyLineSegmentTags);
                Disable_Anno_Tags(BuiltInCategory.OST_SitePropertyTags);
                Disable_Anno_Tags(BuiltInCategory.OST_RiseDropSymbols);
                Disable_Anno_Tags(BuiltInCategory.OST_PipeHydronicSeparationSymbols);
                Disable_Anno_Tags(BuiltInCategory.OST_MechanicalEquipmentSetTags);
                Disable_Anno_Tags(BuiltInCategory.OST_AnalyticalPipeConnectionLineSymbol);
                Disable_Anno_Tags(BuiltInCategory.OST_DSR_DimStyleHeavyEndCategoryId);
                Disable_Anno_Tags(BuiltInCategory.OST_DSR_DimStyleHeavyEndCatId);
                Disable_Anno_Tags(BuiltInCategory.OST_DSR_DimStyleTickCategoryId);
                Disable_Anno_Tags(BuiltInCategory.OST_DSR_LineAndTextAttrFontId);
                Disable_Anno_Tags(BuiltInCategory.OST_DSR_LineAndTextAttrCategoryId);
                Disable_Anno_Tags(BuiltInCategory.OST_NodeAnalyticalTags);
                Disable_Anno_Tags(BuiltInCategory.OST_LinkAnalyticalTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StairsTriserTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StairsSupportTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StairsLandingTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StairsRunTags);
                Disable_Anno_Tags(BuiltInCategory.OST_RailingSystemTags);
                Disable_Anno_Tags(BuiltInCategory.OST_DimLockControlLeader);
                Disable_Anno_Tags(BuiltInCategory.OST_ReferencePoints_HiddenLines);
                Disable_Anno_Tags(BuiltInCategory.OST_ReferencePoints_Lines);
                Disable_Anno_Tags(BuiltInCategory.OST_ReferencePoints_Planes);
                Disable_Anno_Tags(BuiltInCategory.OST_ReferencePoints_Points);
                Disable_Anno_Tags(BuiltInCategory.OST_ColorFillSchema);
                Disable_Anno_Tags(BuiltInCategory.OST_RoomColorFill);
                Disable_Anno_Tags(BuiltInCategory.OST_ColorFillLegends);
                Disable_Anno_Tags(BuiltInCategory.OST_CalloutLeaderLine);
                Disable_Anno_Tags(BuiltInCategory.OST_CalloutBoundary);
                Disable_Anno_Tags(BuiltInCategory.OST_CalloutHeads);
                Disable_Anno_Tags(BuiltInCategory.OST_Callouts);
                Disable_Anno_Tags(BuiltInCategory.OST_Elev);
                Disable_Anno_Tags(BuiltInCategory.OST_MEPSpaceTags);
                Disable_Anno_Tags(BuiltInCategory.OST_RoomTags);
                Disable_Anno_Tags(BuiltInCategory.OST_DoorTags);
                Disable_Anno_Tags(BuiltInCategory.OST_WindowTags);
                Disable_Anno_Tags(BuiltInCategory.OST_SectionHeadWideLines);
                Disable_Anno_Tags(BuiltInCategory.OST_SectionHeadMediumLines);
                Disable_Anno_Tags(BuiltInCategory.OST_SectionHeadThinLines);
                Disable_Anno_Tags(BuiltInCategory.OST_SectionHeads);
                Disable_Anno_Tags(BuiltInCategory.OST_Sections);
                Disable_Anno_Tags(BuiltInCategory.OST_SectionBox);
                Disable_Anno_Tags(BuiltInCategory.OST_TextNotes);
                Disable_Anno_Tags(BuiltInCategory.OST_TitleBlockWideLines);
                Disable_Anno_Tags(BuiltInCategory.OST_TitleBlockMediumLines);
                Disable_Anno_Tags(BuiltInCategory.OST_TitleBlockThinLines);
                Disable_Anno_Tags(BuiltInCategory.OST_TitleBlocks);
                Disable_Anno_Tags(BuiltInCategory.OST_Views);
                Disable_Anno_Tags(BuiltInCategory.OST_PartTags);
                Disable_Anno_Tags(BuiltInCategory.OST_AssemblyTags);
                Disable_Anno_Tags(BuiltInCategory.OST_RoofTags);
                Disable_Anno_Tags(BuiltInCategory.OST_SpotSlopes);
                Disable_Anno_Tags(BuiltInCategory.OST_SpotCoordinates);
                Disable_Anno_Tags(BuiltInCategory.OST_SpotElevations);
                Disable_Anno_Tags(BuiltInCategory.OST_WeakDims);
                Disable_Anno_Tags(BuiltInCategory.OST_Dimensions);
                Disable_Anno_Tags(BuiltInCategory.OST_Grids);
                Disable_Anno_Tags(BuiltInCategory.OST_BrokenSectionLine);
                Disable_Anno_Tags(BuiltInCategory.OST_SectionLine);
                Disable_Anno_Tags(BuiltInCategory.OST_ReferenceViewerSymbol);
                Disable_Anno_Tags(BuiltInCategory.OST_ImportObjectStyles);
                Disable_Anno_Tags(BuiltInCategory.OST_MaskingRegion);
                Disable_Anno_Tags(BuiltInCategory.OST_Matchline);
                Disable_Anno_Tags(BuiltInCategory.OST_PlanRegion);
                Disable_Anno_Tags(BuiltInCategory.OST_FilledRegion);
                Disable_Anno_Tags(BuiltInCategory.OST_AreaInteriorFill);
                Disable_Anno_Tags(BuiltInCategory.OST_RoomInteriorFill);
                Disable_Anno_Tags(BuiltInCategory.OST_AreaColorFill);
                Disable_Anno_Tags(BuiltInCategory.OST_AreaInteriorFillVisibility);
                Disable_Anno_Tags(BuiltInCategory.OST_RoomInteriorFillVisibility);
                Disable_Anno_Tags(BuiltInCategory.OST_StairsRailingTags);
                Disable_Anno_Tags(BuiltInCategory.OST_StairsDownText);
                Disable_Anno_Tags(BuiltInCategory.OST_StairsUpText);
                Disable_Anno_Tags(BuiltInCategory.OST_IOSFabricReinSpanSymbolCtrl);
                Disable_Anno_Tags(BuiltInCategory.OST_GuideGrid);
                Disable_Anno_Tags(BuiltInCategory.OST_IOSRebarSystemSpanSymbolCtrl);
                Disable_Anno_Tags(BuiltInCategory.OST_IOSRoomTagToRoomLines);
                Disable_Anno_Tags(BuiltInCategory.OST_ReferenceLines);
            }
            else if (chkbox_anno.Checked == false)
            {
                Enable_Anno_Tags(BuiltInCategory.OST_MassTags_Obsolete_IdInWrongRange);
                Enable_Anno_Tags(BuiltInCategory.OST_StickSymbols_Obsolete_IdInWrongRange);
                Enable_Anno_Tags(BuiltInCategory.OST_FoundationSlabAnalyticalTags);
                Enable_Anno_Tags(BuiltInCategory.OST_WallFoundationAnalyticalTags);
                Enable_Anno_Tags(BuiltInCategory.OST_IsolatedFoundationAnalyticalTags);
                Enable_Anno_Tags(BuiltInCategory.OST_WallAnalyticalTags);
                Enable_Anno_Tags(BuiltInCategory.OST_FloorAnalyticalTags);
                Enable_Anno_Tags(BuiltInCategory.OST_ColumnAnalyticalTags);
                Enable_Anno_Tags(BuiltInCategory.OST_BraceAnalyticalTags);
                Enable_Anno_Tags(BuiltInCategory.OST_BeamAnalyticalTags);
                Enable_Anno_Tags(BuiltInCategory.OST_CompassSectionFilled);
                Enable_Anno_Tags(BuiltInCategory.OST_SunsetText);
                Enable_Anno_Tags(BuiltInCategory.OST_CompassSection);
                Enable_Anno_Tags(BuiltInCategory.OST_SunriseText);
                Enable_Anno_Tags(BuiltInCategory.OST_StructuralTrussStickSymbols);
                Enable_Anno_Tags(BuiltInCategory.OST_StructConnectionProfilesTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StructConnectionHoleTags);
                Enable_Anno_Tags(BuiltInCategory.OST_CouplerTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StructConnectionWeldTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StructConnectionShearStudTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StructConnectionAnchorTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StructConnectionBoltTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StructConnectionPlateTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StructConnectionTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StructConnectionSymbol);
                Enable_Anno_Tags(BuiltInCategory.OST_FabricReinSpanSymbol);
                Enable_Anno_Tags(BuiltInCategory.OST_FabricAreaTags);
                Enable_Anno_Tags(BuiltInCategory.OST_FabricReinforcementTags);
                Enable_Anno_Tags(BuiltInCategory.OST_AreaReinTags);
                Enable_Anno_Tags(BuiltInCategory.OST_RebarTags);
                Enable_Anno_Tags(BuiltInCategory.OST_PathReinTags);
                Enable_Anno_Tags(BuiltInCategory.OST_PathReinSpanSymbol);
                Enable_Anno_Tags(BuiltInCategory.OST_AreaReinSpanSymbol);
                Enable_Anno_Tags(BuiltInCategory.OST_FabricationContainmentSymbology);
                Enable_Anno_Tags(BuiltInCategory.OST_FabricationContainmentTags);
                Enable_Anno_Tags(BuiltInCategory.OST_FabricationPipeworkSymbology);
                Enable_Anno_Tags(BuiltInCategory.OST_FabricationPipeworkTags);
                Enable_Anno_Tags(BuiltInCategory.OST_FabricationDuctworkSymbology);
                Enable_Anno_Tags(BuiltInCategory.OST_FabricationHangerTags);
                Enable_Anno_Tags(BuiltInCategory.OST_FabricationDuctworkTags);
                Enable_Anno_Tags(BuiltInCategory.OST_PipeInsulationsTags);
                Enable_Anno_Tags(BuiltInCategory.OST_DuctLiningsTags);
                Enable_Anno_Tags(BuiltInCategory.OST_DuctInsulationsTags);
                Enable_Anno_Tags(BuiltInCategory.OST_ConduitTags);
                Enable_Anno_Tags(BuiltInCategory.OST_CableTrayTags);
                Enable_Anno_Tags(BuiltInCategory.OST_ConduitFittingTags);
                Enable_Anno_Tags(BuiltInCategory.OST_CableTrayFittingTags);
                Enable_Anno_Tags(BuiltInCategory.OST_HVAC_Zones_InteriorFill_Visibility);
                Enable_Anno_Tags(BuiltInCategory.OST_HVAC_Zones_ColorFill);
                Enable_Anno_Tags(BuiltInCategory.OST_ZoneTags);
                Enable_Anno_Tags(BuiltInCategory.OST_HVAC_Zones_InteriorFill);
                Enable_Anno_Tags(BuiltInCategory.OST_SprinklerTags);
                Enable_Anno_Tags(BuiltInCategory.OST_LightingDeviceTags);
                Enable_Anno_Tags(BuiltInCategory.OST_FireAlarmDeviceTags);
                Enable_Anno_Tags(BuiltInCategory.OST_DataDeviceTags);
                Enable_Anno_Tags(BuiltInCategory.OST_CommunicationDeviceTags);
                Enable_Anno_Tags(BuiltInCategory.OST_SecurityDeviceTags);
                Enable_Anno_Tags(BuiltInCategory.OST_NurseCallDeviceTags);
                Enable_Anno_Tags(BuiltInCategory.OST_TelephoneDeviceTags);
                Enable_Anno_Tags(BuiltInCategory.OST_DuctFittingTags);
                Enable_Anno_Tags(BuiltInCategory.OST_PipeFittingTags);
                Enable_Anno_Tags(BuiltInCategory.OST_PipeColorFills);
                Enable_Anno_Tags(BuiltInCategory.OST_PipeColorFillLegends);
                Enable_Anno_Tags(BuiltInCategory.OST_WireTags);
                Enable_Anno_Tags(BuiltInCategory.OST_PipeAccessoryTags);
                Enable_Anno_Tags(BuiltInCategory.OST_FlexPipeTags);
                Enable_Anno_Tags(BuiltInCategory.OST_PipeTags);
                Enable_Anno_Tags(BuiltInCategory.OST_ElectricalVoltage);
                Enable_Anno_Tags(BuiltInCategory.OST_ElectricalCircuitTags);
                Enable_Anno_Tags(BuiltInCategory.OST_DuctAccessoryTags);
                Enable_Anno_Tags(BuiltInCategory.OST_DuctTerminalTags);
                Enable_Anno_Tags(BuiltInCategory.OST_DuctColorFills);
                Enable_Anno_Tags(BuiltInCategory.OST_FlexDuctTags);
                Enable_Anno_Tags(BuiltInCategory.OST_DuctTags);
                Enable_Anno_Tags(BuiltInCategory.OST_DuctColorFillLegends);
                Enable_Anno_Tags(BuiltInCategory.OST_BridgeBearingTags);
                Enable_Anno_Tags(BuiltInCategory.OST_BridgeGirderTags2021_Deprecated);
                Enable_Anno_Tags(BuiltInCategory.OST_BridgeFoundationTags);
                Enable_Anno_Tags(BuiltInCategory.OST_BridgeDeckTags);
                Enable_Anno_Tags(BuiltInCategory.OST_BridgeArchTags2021_Deprecated);
                Enable_Anno_Tags(BuiltInCategory.OST_BridgeCableTags);
                Enable_Anno_Tags(BuiltInCategory.OST_BridgeTowerTags);
                Enable_Anno_Tags(BuiltInCategory.OST_BridgePierTags);
                Enable_Anno_Tags(BuiltInCategory.OST_BridgeAbutmentTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StructConnectionSymbols);
                Enable_Anno_Tags(BuiltInCategory.OST_RevisionCloudTags);
                Enable_Anno_Tags(BuiltInCategory.OST_Revisions);
                Enable_Anno_Tags(BuiltInCategory.OST_RevisionClouds);
                Enable_Anno_Tags(BuiltInCategory.OST_ElevationMarks);
                Enable_Anno_Tags(BuiltInCategory.OST_InternalAreaLoadTags);
                Enable_Anno_Tags(BuiltInCategory.OST_InternalLineLoadTags);
                Enable_Anno_Tags(BuiltInCategory.OST_InternalPointLoadTags);
                Enable_Anno_Tags(BuiltInCategory.OST_AreaLoadTags);
                Enable_Anno_Tags(BuiltInCategory.OST_LineLoadTags);
                Enable_Anno_Tags(BuiltInCategory.OST_PointLoadTags);
                Enable_Anno_Tags(BuiltInCategory.OST_BeamSystemTags);
                Enable_Anno_Tags(BuiltInCategory.OST_FootingSpanDirectionSymbol);
                Enable_Anno_Tags(BuiltInCategory.OST_SpanDirectionSymbol);
                Enable_Anno_Tags(BuiltInCategory.OST_SpotSlopesSymbols);
                Enable_Anno_Tags(BuiltInCategory.OST_SpotCoordinateSymbols);
                Enable_Anno_Tags(BuiltInCategory.OST_SpotElevSymbols);
                Enable_Anno_Tags(BuiltInCategory.OST_StructuralConnectionHandlerTags_Deprecated);
                Enable_Anno_Tags(BuiltInCategory.OST_TrussTags);
                Enable_Anno_Tags(BuiltInCategory.OST_KeynoteTags);
                Enable_Anno_Tags(BuiltInCategory.OST_DetailComponentTags);
                Enable_Anno_Tags(BuiltInCategory.OST_MaterialTags);
                Enable_Anno_Tags(BuiltInCategory.OST_FloorTags);
                Enable_Anno_Tags(BuiltInCategory.OST_CurtaSystemTags);
                Enable_Anno_Tags(BuiltInCategory.OST_HostFinTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StairsTags);
                Enable_Anno_Tags(BuiltInCategory.OST_MultiCategoryTags);
                Enable_Anno_Tags(BuiltInCategory.OST_PlantingTags);
                Enable_Anno_Tags(BuiltInCategory.OST_AreaTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StructuralFoundationTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StructuralColumnTags);
                Enable_Anno_Tags(BuiltInCategory.OST_ParkingTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StructuralFramingTags);
                Enable_Anno_Tags(BuiltInCategory.OST_SpecialityEquipmentTags);
                Enable_Anno_Tags(BuiltInCategory.OST_GenericModelTags);
                Enable_Anno_Tags(BuiltInCategory.OST_CurtainWallPanelTags);
                Enable_Anno_Tags(BuiltInCategory.OST_WallTags);
                Enable_Anno_Tags(BuiltInCategory.OST_PlumbingFixtureTags);
                Enable_Anno_Tags(BuiltInCategory.OST_MechanicalEquipmentTags);
                Enable_Anno_Tags(BuiltInCategory.OST_LightingFixtureTags);
                Enable_Anno_Tags(BuiltInCategory.OST_FurnitureSystemTags);
                Enable_Anno_Tags(BuiltInCategory.OST_FurnitureTags);
                Enable_Anno_Tags(BuiltInCategory.OST_ElectricalFixtureTags);
                Enable_Anno_Tags(BuiltInCategory.OST_ElectricalEquipmentTags);
                Enable_Anno_Tags(BuiltInCategory.OST_CeilingTags);
                Enable_Anno_Tags(BuiltInCategory.OST_CaseworkTags);
                Enable_Anno_Tags(BuiltInCategory.OST_Tags);
                Enable_Anno_Tags(BuiltInCategory.OST_MEPSpaceColorFill);
                Enable_Anno_Tags(BuiltInCategory.OST_MEPSpaceInteriorFill);
                Enable_Anno_Tags(BuiltInCategory.OST_MEPSpaceInteriorFillVisibility);
                Enable_Anno_Tags(BuiltInCategory.OST_MassAreaFaceTags);
                Enable_Anno_Tags(BuiltInCategory.OST_MassTags);
                Enable_Anno_Tags(BuiltInCategory.OST_DividedSurface_PatternFill);
                Enable_Anno_Tags(BuiltInCategory.OST_RampsDownText);
                Enable_Anno_Tags(BuiltInCategory.OST_RampsUpText);
                Enable_Anno_Tags(BuiltInCategory.OST_StructuralStiffenerTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StructuralColumnStickSymbols);
                Enable_Anno_Tags(BuiltInCategory.OST_SitePropertyLineSegmentTags);
                Enable_Anno_Tags(BuiltInCategory.OST_SitePropertyTags);
                Enable_Anno_Tags(BuiltInCategory.OST_RiseDropSymbols);
                Enable_Anno_Tags(BuiltInCategory.OST_PipeHydronicSeparationSymbols);
                Enable_Anno_Tags(BuiltInCategory.OST_MechanicalEquipmentSetTags);
                Enable_Anno_Tags(BuiltInCategory.OST_AnalyticalPipeConnectionLineSymbol);
                Enable_Anno_Tags(BuiltInCategory.OST_DSR_DimStyleHeavyEndCategoryId);
                Enable_Anno_Tags(BuiltInCategory.OST_DSR_DimStyleHeavyEndCatId);
                Enable_Anno_Tags(BuiltInCategory.OST_DSR_DimStyleTickCategoryId);
                Enable_Anno_Tags(BuiltInCategory.OST_DSR_LineAndTextAttrFontId);
                Enable_Anno_Tags(BuiltInCategory.OST_DSR_LineAndTextAttrCategoryId);
                Enable_Anno_Tags(BuiltInCategory.OST_NodeAnalyticalTags);
                Enable_Anno_Tags(BuiltInCategory.OST_LinkAnalyticalTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StairsTriserTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StairsSupportTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StairsLandingTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StairsRunTags);
                Enable_Anno_Tags(BuiltInCategory.OST_RailingSystemTags);
                Enable_Anno_Tags(BuiltInCategory.OST_DimLockControlLeader);
                Enable_Anno_Tags(BuiltInCategory.OST_ReferencePoints_HiddenLines);
                Enable_Anno_Tags(BuiltInCategory.OST_ReferencePoints_Lines);
                Enable_Anno_Tags(BuiltInCategory.OST_ReferencePoints_Planes);
                Enable_Anno_Tags(BuiltInCategory.OST_ReferencePoints_Points);
                Enable_Anno_Tags(BuiltInCategory.OST_ColorFillSchema);
                Enable_Anno_Tags(BuiltInCategory.OST_RoomColorFill);
                Enable_Anno_Tags(BuiltInCategory.OST_ColorFillLegends);
                Enable_Anno_Tags(BuiltInCategory.OST_CalloutLeaderLine);
                Enable_Anno_Tags(BuiltInCategory.OST_CalloutBoundary);
                Enable_Anno_Tags(BuiltInCategory.OST_CalloutHeads);
                Enable_Anno_Tags(BuiltInCategory.OST_Callouts);
                Enable_Anno_Tags(BuiltInCategory.OST_Elev);
                Enable_Anno_Tags(BuiltInCategory.OST_MEPSpaceTags);
                Enable_Anno_Tags(BuiltInCategory.OST_RoomTags);
                Enable_Anno_Tags(BuiltInCategory.OST_DoorTags);
                Enable_Anno_Tags(BuiltInCategory.OST_WindowTags);
                Enable_Anno_Tags(BuiltInCategory.OST_SectionHeadWideLines);
                Enable_Anno_Tags(BuiltInCategory.OST_SectionHeadMediumLines);
                Enable_Anno_Tags(BuiltInCategory.OST_SectionHeadThinLines);
                Enable_Anno_Tags(BuiltInCategory.OST_SectionHeads);
                Enable_Anno_Tags(BuiltInCategory.OST_Sections);
                Enable_Anno_Tags(BuiltInCategory.OST_SectionBox);
                Enable_Anno_Tags(BuiltInCategory.OST_TextNotes);
                Enable_Anno_Tags(BuiltInCategory.OST_TitleBlockWideLines);
                Enable_Anno_Tags(BuiltInCategory.OST_TitleBlockMediumLines);
                Enable_Anno_Tags(BuiltInCategory.OST_TitleBlockThinLines);
                Enable_Anno_Tags(BuiltInCategory.OST_TitleBlocks);
                Enable_Anno_Tags(BuiltInCategory.OST_Views);
                Enable_Anno_Tags(BuiltInCategory.OST_PartTags);
                Enable_Anno_Tags(BuiltInCategory.OST_AssemblyTags);
                Enable_Anno_Tags(BuiltInCategory.OST_RoofTags);
                Enable_Anno_Tags(BuiltInCategory.OST_SpotSlopes);
                Enable_Anno_Tags(BuiltInCategory.OST_SpotCoordinates);
                Enable_Anno_Tags(BuiltInCategory.OST_SpotElevations);
                Enable_Anno_Tags(BuiltInCategory.OST_WeakDims);
                Enable_Anno_Tags(BuiltInCategory.OST_Dimensions);
                Enable_Anno_Tags(BuiltInCategory.OST_Grids);
                Enable_Anno_Tags(BuiltInCategory.OST_BrokenSectionLine);
                Enable_Anno_Tags(BuiltInCategory.OST_SectionLine);
                Enable_Anno_Tags(BuiltInCategory.OST_ReferenceViewerSymbol);
                Enable_Anno_Tags(BuiltInCategory.OST_ImportObjectStyles);
                Enable_Anno_Tags(BuiltInCategory.OST_MaskingRegion);
                Enable_Anno_Tags(BuiltInCategory.OST_Matchline);
                Enable_Anno_Tags(BuiltInCategory.OST_PlanRegion);
                Enable_Anno_Tags(BuiltInCategory.OST_FilledRegion);
                Enable_Anno_Tags(BuiltInCategory.OST_AreaInteriorFill);
                Enable_Anno_Tags(BuiltInCategory.OST_RoomInteriorFill);
                Enable_Anno_Tags(BuiltInCategory.OST_AreaColorFill);
                Enable_Anno_Tags(BuiltInCategory.OST_AreaInteriorFillVisibility);
                Enable_Anno_Tags(BuiltInCategory.OST_RoomInteriorFillVisibility);
                Enable_Anno_Tags(BuiltInCategory.OST_StairsRailingTags);
                Enable_Anno_Tags(BuiltInCategory.OST_StairsDownText);
                Enable_Anno_Tags(BuiltInCategory.OST_StairsUpText);
                Enable_Anno_Tags(BuiltInCategory.OST_IOSFabricReinSpanSymbolCtrl);
                Enable_Anno_Tags(BuiltInCategory.OST_GuideGrid);
                Enable_Anno_Tags(BuiltInCategory.OST_IOSRebarSystemSpanSymbolCtrl);
                Enable_Anno_Tags(BuiltInCategory.OST_IOSRoomTagToRoomLines);
                Enable_Anno_Tags(BuiltInCategory.OST_ReferenceLines);
            }
        }
        private void chkbox_analytical_CheckedChanged(object sender, EventArgs e)
        {
            Dis_Ele_By_CheckBox_Analytical(BuiltInCategory.OST_AnalyticalNodes);
        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void checkedListBox_items_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (checkedListBox_items.Count <= e.Index)
            //{
            //    // Expand the list if needed
            //    for (int i = checkedListBox_items_ItemCheck.Count; i <= e.Index; i++)
            //    {
            //        checkedListBox_items.ControlAdded(false); // Initialize with unchecked
            //    }
            //}

            // Update the checkbox state in the list
          //  checkedListBox_items[e.Index] = (e.NewValue == CheckState.Checked);
        }
        private void checkedListBox_items_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }
        public void DisableCeilingsOnly()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the Categories in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the single Category
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_CeilingOpening);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> elements = collector.ToElements();
                if (elements.Count > 0)
                {
                    IEnumerable<IEnumerable<Element>> batches = elements
                        .Select((element, index) => new { Element = element, Index = index })
                        .GroupBy(x => x.Index / batchSize)
                        .Select(g => g.Select(x => x.Element));

                    List<ElementId> all_ele_Ids = new List<ElementId>();

                    foreach (IEnumerable<Element> batch in batches)
                    {
                        // Process the elements in the current batch
                        foreach (Element element in batch)
                        {
                            if (element.Pinned == true)
                            {
                                continue;
                            }
                            all_ele_Ids.Add(element.Id);
                            element.CanBeLocked();
                            element.Pinned = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Disable_Elements : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {

            if (button3.BackColor != Color.RoyalBlue)
            {
                /*
                chkbox_model.Checked = true;
                chkbox_anno.Checked = true;
                chkbox_analytical.Checked = true;
                chk_cad_import.Checked = true;
                */
                button3.BackColor = Color.RoyalBlue;
                btn_clr.BackColor = Color.White;

                for (int i = 0; checkedListBox_items.Items.Count > i; i++)
                {
                    checkedListBox_items.SetItemChecked(i, true);
                }

                // button1.PerformClick();

                /*
                btn_arch.PerformClick();
                btn_strc.PerformClick();
                btn_elec.PerformClick();
                btn_mech.PerformClick();
                btn_plum.PerformClick();
                btn_infra.PerformClick();
                
                chkbox_model_CheckedChanged(sender, e);
                chkbox_anno_CheckedChanged(sender, e);
                chkbox_analytical_CheckedChanged(sender, e);
                chk_cad_import_CheckedChanged(sender, e);
                */
            }
        }
        private void btn_clr_Click(object sender, EventArgs e)
        {
            if (btn_clr.BackColor != Color.RoyalBlue)
            {
                /*
                chkbox_model.Checked = false;
                chkbox_anno.Checked = false;
                chkbox_analytical.Checked = false;
                chk_cad_import.Checked = false;
                */

                btn_clr.BackColor = Color.RoyalBlue;
                button3.BackColor = Color.White;

                for (int i = 0; checkedListBox_items.Items.Count > i; i++)
                {
                    checkedListBox_items.SetItemChecked(i, true);
                }

                //  btn_enable.PerformClick();

                /*
                btn_arch.PerformClick();
                btn_strc.PerformClick();
                btn_elec.PerformClick();
                btn_mech.PerformClick();
                btn_plum.PerformClick();
                btn_infra.PerformClick();
                */
                for (int i = 0; checkedListBox_items.Items.Count > i; i++)
                {
                    checkedListBox_items.SetItemChecked(i, false);
                }
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var Item in checkedListBox_items.CheckedItems)
            {
                string sel_item = Item.ToString();
                if (sel_item == "Areas")
                {
                    Disable_Elements(BuiltInCategory.OST_Areas);
                }
                if (sel_item == "Audio Visual Devices")
                {
                    Disable_Elements(BuiltInCategory.OST_AudioVisualDevices);
                }
                if (sel_item == "Caseworks")
                {
                    Disable_Elements(BuiltInCategory.OST_Casework);
                }
                if (sel_item == "Columns")
                {
                    Disable_Elements(BuiltInCategory.OST_Columns);
                }
                if (sel_item == "Ceilings")
                {
                    //  DisableCeilingsOnly();
                    Disable_CeilingsAndFloor(BuiltInCategory.OST_Ceilings);
                }
                if (sel_item == "Curtain Panels")
                {
                    Disable_Elements(BuiltInCategory.OST_CurtainWallPanels);
                }
                if (sel_item == "Curtain Systems")
                {
                    Disable_Elements(BuiltInCategory.OST_Curtain_Systems);
                }
                if (sel_item == "Details Items")
                {
                    Disable_Elements(BuiltInCategory.OST_DetailComponents);
                }
                if (sel_item == "Doors")
                {
                    Disable_Elements(BuiltInCategory.OST_Doors);
                }
                if (sel_item == "Electrical Equipments")
                {
                    Disable_Elements(BuiltInCategory.OST_ElectricalEquipment);
                }
                if (sel_item == "Electrical Fixtures")
                {
                    Disable_Elements(BuiltInCategory.OST_ElectricalFixtures);
                }
                if (sel_item == "Entourages")
                {
                    Disable_Elements(BuiltInCategory.OST_Entourage);
                }
                if (sel_item == "Fire Protections")
                {
                    Disable_Elements(BuiltInCategory.OST_FireProtection);
                }
                if (sel_item == "Food Service Equipments")
                {
                    Disable_Elements(BuiltInCategory.OST_FoodServiceEquipment);
                }
                if (sel_item == "Furniture Systems")
                {
                    Disable_Elements(BuiltInCategory.OST_FurnitureSystems);
                }
                if (sel_item == "Generic Models")
                {
                    Disable_Elements(BuiltInCategory.OST_GenericModel);
                }
                if (sel_item == "Generic Annotations")
                {
                    Disable_Elements(BuiltInCategory.OST_GenericAnnotation);
                }
                if (sel_item == "Hardscapes")
                {
                    Disable_Elements(BuiltInCategory.OST_Hardscape);
                }
                if (sel_item == "Lighting Fixtures")
                {
                    Disable_Elements(BuiltInCategory.OST_LightingFixtures);
                }
                if (sel_item == "Lines")
                {
                    Disable_Elements(BuiltInCategory.OST_Lines);
                }
                if (sel_item == "Mass")
                {
                    Disable_Elements(BuiltInCategory.OST_Mass);
                }
                if (sel_item == "Mechanical Equipments")
                {
                    Disable_Elements(BuiltInCategory.OST_MechanicalEquipment);
                }
                if (sel_item == "Medical Equipments")
                {
                    Disable_Elements(BuiltInCategory.OST_MedicalEquipment);
                }
                if (sel_item == "Parkings")
                {
                    Disable_Elements(BuiltInCategory.OST_Parking);
                }
                if (sel_item == "Parts")
                {
                    Disable_Elements(BuiltInCategory.OST_Parts);
                }
                if (sel_item == "Plantings")
                {
                    Disable_Elements(BuiltInCategory.OST_Planting);
                }
                if (sel_item == "Plumbing Fixtures")
                {
                    Disable_Elements(BuiltInCategory.OST_PlumbingFixtures);
                }
                if (sel_item == "Railings")
                {
                    Disable_Railings(BuiltInCategory.OST_Railings);
                }
                if (sel_item == "Ramps")
                {
                    Disable_Elements(BuiltInCategory.OST_Ramps);
                }
                if (sel_item == "Raster Images")
                {
                    Disable_Elements(BuiltInCategory.OST_RasterImages);
                }
                if (sel_item == "Roads")
                {
                    Disable_Elements(BuiltInCategory.OST_Roads);
                }
                if (sel_item == "Roofs")
                {
                    Disable_Elements(BuiltInCategory.OST_Roofs);
                }
                if (sel_item == "Shaft Openings")
                {
                    Disable_Elements(BuiltInCategory.OST_ShaftOpening);
                }
                if (sel_item == "Signages")
                {
                    Disable_Elements(BuiltInCategory.OST_Signage);
                }
                if (sel_item == "Sites")
                {
                    Disable_Elements(BuiltInCategory.OST_Site);
                }
                if (sel_item == "Speciality Equipments")
                {
                    Disable_Elements(BuiltInCategory.OST_SpecialityEquipment);
                }
                if (sel_item == "Stairs")
                {
                    Disable_Elements(BuiltInCategory.OST_Stairs);
                }
                if (sel_item == "Structural Foundations")
                {
                    Disable_Elements(BuiltInCategory.OST_StructuralFoundation);
                }
                if (sel_item == "Structural Columns")
                {
                    Disable_Elements(BuiltInCategory.OST_StructuralColumns);
                }
                if (sel_item == "Structural Connections")
                {
                    Disable_Elements(BuiltInCategory.OST_StructConnections);
                }
                if (sel_item == "Structural Framings")
                {
                    Disable_Elements(BuiltInCategory.OST_StructuralFraming);
                }
                if (sel_item == "Structural Rebars")
                {
                    Disable_Elements(BuiltInCategory.OST_Rebar);
                }
                if (sel_item == "Structural Rebar Couplers")
                {
                    Disable_Elements(BuiltInCategory.OST_Coupler);
                }
                if (sel_item == "Structural Stiffeners")
                {
                    Disable_Elements(BuiltInCategory.OST_StructuralStiffener);
                }
                if (sel_item == "Temporary Structures")
                {
                    Disable_Elements(BuiltInCategory.OST_TemporaryStructure);
                }
                if (sel_item == "Topography")
                {
                    Disable_Elements(BuiltInCategory.OST_Topography);
                }
                if (sel_item == "Vertical Circulations")
                {
                    Disable_Elements(BuiltInCategory.OST_VerticalCirculation);
                }
                if (sel_item == "Windows")
                {
                    Disable_Elements(BuiltInCategory.OST_Windows);
                }
                if (sel_item == "Floors")
                {
                    Disable_CeilingsAndFloor(BuiltInCategory.OST_Floors);
                }
                if (sel_item == "Furnitures")
                {
                    Disable_Elements(BuiltInCategory.OST_Furniture);
                }
            }

            RegistryKey key = Registry.CurrentUser.CreateSubKey("LinkElement");
            key.SetValue("checkedListBox_items", checkedListBox_items.CheckedItems);
            key.Close();    
        }
        private void btn_enable_Click(object sender, EventArgs e)
        {
            foreach (var Item in checkedListBox_items.CheckedItems)
            {
                string sel_item = Item.ToString();
                if (sel_item == "Areas")
                {
                    Enable_Elements(BuiltInCategory.OST_Areas);
                }
                if (sel_item == "Audio Visual Devices")
                {
                    Enable_Elements(BuiltInCategory.OST_AudioVisualDevices);
                }
                if (sel_item == "Caseworks")
                {
                    Enable_Elements(BuiltInCategory.OST_Casework);
                }
                if (sel_item == "Columns")
                {
                    Enable_Elements(BuiltInCategory.OST_Columns);
                }
                if (sel_item == "Ceilings")
                {
                    Enable_CeilingsAndFloor(BuiltInCategory.OST_Ceilings);
                }
                if (sel_item == "Curtain Panels")
                {
                    Enable_Elements(BuiltInCategory.OST_CurtainWallPanels);
                }
                if (sel_item == "Curtain Systems")
                {
                    Enable_Elements(BuiltInCategory.OST_Curtain_Systems);
                }
                if (sel_item == "Details Items")
                {
                    Enable_Elements(BuiltInCategory.OST_DetailComponents);
                }
                if (sel_item == "Doors")
                {
                    Enable_Elements(BuiltInCategory.OST_Doors);
                }
                if (sel_item == "Electrical Equipments")
                {
                    Enable_Elements(BuiltInCategory.OST_ElectricalEquipment);
                }
                if (sel_item == "Electrical Fixtures")
                {
                    Enable_Elements(BuiltInCategory.OST_ElectricalFixtures);
                }
                if (sel_item == "Entourages")
                {
                    Enable_Elements(BuiltInCategory.OST_Entourage);
                }
                if (sel_item == "Fire Protections")
                {
                    Enable_Elements(BuiltInCategory.OST_FireProtection);
                }
                if (sel_item == "Furniture Systems")
                {
                    Enable_Elements(BuiltInCategory.OST_FurnitureSystems);
                }
                if (sel_item == "Generic Models")
                {
                    Enable_Elements(BuiltInCategory.OST_GenericModel);
                }
                if (sel_item == "Hardscapes")
                {
                    Enable_Elements(BuiltInCategory.OST_Hardscape);
                }
                if (sel_item == "Lighting Fixtures")
                {
                    Enable_Elements(BuiltInCategory.OST_LightingFixtures);
                }
                if (sel_item == "Lines")
                {
                    Enable_Elements(BuiltInCategory.OST_Lines);
                }
                if (sel_item == "Mass")
                {
                    Enable_Elements(BuiltInCategory.OST_Mass);
                }
                if (sel_item == "Mechanical Equipments")
                {
                    Enable_Elements(BuiltInCategory.OST_MechanicalEquipment);
                }
                if (sel_item == "Medical Equipments")
                {
                    Enable_Elements(BuiltInCategory.OST_MedicalEquipment);
                }
                if (sel_item == "Parkings")
                {
                    Enable_Elements(BuiltInCategory.OST_Parking);
                }
                if (sel_item == "Parts")
                {
                    Enable_Elements(BuiltInCategory.OST_Parts);
                }
                if (sel_item == "Plantings")
                {
                    Enable_Elements(BuiltInCategory.OST_Planting);
                }
                if (sel_item == "Plumbing Fixtures")
                {
                    Enable_Elements(BuiltInCategory.OST_PlumbingFixtures);
                }
                if (sel_item == "Railings")
                {
                    Enable_Railings(BuiltInCategory.OST_Railings);
                }
                if (sel_item == "Ramps")
                {
                    Enable_Elements(BuiltInCategory.OST_Ramps);
                }
                if (sel_item == "Raster Images")
                {
                    Enable_Elements(BuiltInCategory.OST_RasterImages);
                }
                if (sel_item == "Roads")
                {
                    Enable_Elements(BuiltInCategory.OST_Roads);
                }
                if (sel_item == "Roofs")
                {
                    Enable_Elements(BuiltInCategory.OST_Roofs);
                }
                if (sel_item == "Shaft Openings")
                {
                    Enable_Elements(BuiltInCategory.OST_ShaftOpening);
                }
                if (sel_item == "Signages")
                {
                    Enable_Elements(BuiltInCategory.OST_Signage);
                }
                if (sel_item == "Sites")
                {
                    Enable_Elements(BuiltInCategory.OST_Site);
                }
                if (sel_item == "Speciality Equipments")
                {
                    Enable_Elements(BuiltInCategory.OST_SpecialityEquipment);
                }
                if (sel_item == "Stairs")
                {
                    Enable_Elements(BuiltInCategory.OST_Stairs);
                }
                if (sel_item == "Structural Foundations")
                {
                    Enable_Elements(BuiltInCategory.OST_StructuralFoundation);
                }
                if (sel_item == "Structural Columns")
                {
                    Enable_Elements(BuiltInCategory.OST_StructuralColumns);
                }
                if (sel_item == "Structural Connections")
                {
                    Enable_Elements(BuiltInCategory.OST_StructConnections);
                }
                if (sel_item == "Structural Framings")
                {
                    Enable_Elements(BuiltInCategory.OST_StructuralFraming);
                }
                if (sel_item == "Structural Rebars")
                {
                    Enable_Elements(BuiltInCategory.OST_Rebar);
                }
                if (sel_item == "Structural Rebar Couplers")
                {
                    Enable_Elements(BuiltInCategory.OST_Coupler);
                }
                if (sel_item == "Structural Stiffeners")
                {
                    Enable_Elements(BuiltInCategory.OST_StructuralStiffener);
                }
                if (sel_item == "Temporary Structures")
                {
                    Enable_Elements(BuiltInCategory.OST_TemporaryStructure);
                }
                if (sel_item == "Topography")
                {
                    Enable_Elements(BuiltInCategory.OST_Topography);
                }
                if (sel_item == "Vertical Circulations")
                {
                    Enable_Elements(BuiltInCategory.OST_VerticalCirculation);
                }
                if (sel_item == "Windows")
                {
                    Enable_Elements(BuiltInCategory.OST_Windows);
                }
                if (sel_item == "Floors")
                {
                    Enable_CeilingsAndFloor(BuiltInCategory.OST_Floors);
                }
                if (sel_item == "Furnitures")
                {
                    Enable_Elements(BuiltInCategory.OST_Furniture);
                }
            }
        }
        private void btn_close_Click(object sender, EventArgs e)
        {
            //System.IO.File.WriteAllText(@"C:\Users\prashant.chaturvedi\Documents\AppFile.txt", this.chkbox_model.Checked.ToString());
            this.Close();
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                string value = System.IO.File.ReadAllText(@"C:\Users\prashant.chaturvedi\Documents\AppFile.txt");
                this.chkbox_model.Checked = bool.Parse(value);
                
                //RegistryKey key = Registry.CurrentUser.OpenSubKey("LinkElement");
                //if (key != null)
                //{
                //    object value = key.GetValue("checkedListBox_items");
                //    if (value != null && (bool)value)
                //    {
                //        //checkedListBox_items.CheckedItems = true;
                //    }
                //    key.Close();
                //}
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
            }
        }
        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                button1.PerformClick();
                // this.Close();
            }
        }

        public class StatusClass
        {
            public bool CheckBox_Model;
        }
        private StatusClass m_statusClass;
        public void FrmSaveCSV(StatusClass statusClass)
        {
            InitializeComponent();
            m_statusClass = statusClass;
        }
        private void FrmSaveCSV_Load(object sender, EventArgs e)
        {
            LoadStaus();
        }
        private void FrmSaveCSV_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveStatus();
        }
        private void LoadStaus()
        {
            this.chkbox_model.Checked = m_statusClass.CheckBox_Model;
        }
        private void SaveStatus()
        {
            m_statusClass.CheckBox_Model = this.chkbox_model.Checked;
        }
        public static string ElementDescription(Element e)
        {
            if (null == e)
            {
                return "<null>";
            }

            // For a wall, the element name equals the
            // wall type name, which is equivalent to the
            // family name ...

            FamilyInstance fi = e as FamilyInstance;

            string typeName = e.GetType().Name;

            string categoryName = (null == e.Category)
              ? string.Empty
              : e.Category.Name + " ";

            string familyName = (null == fi)
              ? string.Empty
              : fi.Symbol.Family.Name + " ";

            string symbolName = (null == fi
              || e.Name.Equals(fi.Symbol.Name))
                ? string.Empty
                : fi.Symbol.Name + " ";

            return string.Format("{0} {1}{2}{3}<{4} {5}>",
              typeName, categoryName, familyName,
              symbolName, e.Id.IntegerValue, e.Name);
        }
        public static string ElementDescription(Document doc, int element_id)
        {
            return ElementDescription(doc.GetElement(new ElementId(element_id)));
        }
        public async void asd(Document doc)
        {
            try
            {
                try
                {
                    GetWorkPath();
                }
                catch (Exception)
                {

                }
               // IEnumerable<Element> a = GetTrackedElements(doc);

                if (null == _start_state)
                {
                   // _start_state = GetSnapshot(a);
                }
                else
                {
                   // Dictionary<int, string> end_state = GetSnapshot(a);
                  //  await ReportDifferences(doc);
                   // _start_state = end_state;
                   // end_state = null;
                }
            }
            catch (Exception ex)
            {

                errorLog = "Error : " + RevitVersion + " - " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + " - ASD - " + ex.Message;
                Logger_();

            }
        }

        #region Write a log file into text file and remove recent file
        public static void Logger_()
        {
            try
            {
                string strFileName = "Log" + DateTime.Now.ToString("ddMMyyy") + ".xml";
                string strLogtxtFilePath = System.IO.Path.Combine(strWorkPath, strFileName);
                if (!Directory.Exists(strWorkPath))
                {
                    try
                    {
                        Directory.CreateDirectory(strWorkPath);

                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                #region delete old txt  files
                string[] files = Directory.GetFiles(strWorkPath, "*.txt");
                Array.Sort(files, StringComparer.InvariantCulture);

                if (files.Length > 5)
                {
                    try
                    {
                        int end = files.Length - 1;

                        for (int i = end; i >= 0; i--)
                        {
                            FileInfo fi = new FileInfo(files[i]);
                            if (fi.CreationTime < DateTime.Now.AddDays(-5))
                            {
                                fi.Delete();
                            }
                            string[] files2 = Directory.GetFiles(strWorkPath, "*.txt");
                            if (files2.Length <= 5)
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                #endregion

                #region write error log into text file
                if (File.Exists(strLogtxtFilePath))
                {
                    File.AppendAllText(strLogtxtFilePath, errorLog + Environment.NewLine);
                }
                else
                {
                    FileInfo fi = new FileInfo(strLogtxtFilePath);
                    using (StreamWriter sw = fi.CreateText())
                    {
                        sw.WriteLine(errorLog);
                    }
                }
                #endregion
            }
            catch (Exception)
            {

            }
        }
        #endregion
        public void InitializeDTable()
        {
            try
            {
                dt = new DataTable("row");
                dt.Columns.Add("Model", typeof(bool));
                dt.Columns.Add("Annotation", typeof(bool));
                dt.Columns.Add("Analytical", typeof(bool));
                dt.Columns.Add("CADImport", typeof(bool));
                dt.Columns.Add("Arch", typeof(ButtonState));
                dt.Columns.Add("Struct", typeof(ButtonState));
                dt.Columns.Add("Elec", typeof(ButtonState));
                dt.Columns.Add("Mech", typeof(ButtonState));
                dt.Columns.Add("Plum", typeof(ButtonState));
                dt.Columns.Add("Infra", typeof(ButtonState));
            }
            catch (Exception ex)
            {
                errorLog = "Error : " + RevitVersion + " - " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + " - InitializeDTable - " + ex.Message;
                Logger_();

            }
        }

        #region get network path
        public void GetWorkPath()
        {
            strWorkPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            strWorkPath = Directory.GetParent(strWorkPath).FullName;
            strWorkPath = Directory.GetParent(strWorkPath).FullName + "\\" + Dns.GetHostName();
        }
        #endregion

        #region save data into xml file and delete old file
        public void ReadWriteXML(DataTable dataTable)
        {
            try
            {
                string strFileName = "setting" + DateTime.Now.ToString("ddMMyyy") + ".xml";
                string strSettingsXmlFilePath = System.IO.Path.Combine(strWorkPath, strFileName);
                if (!Directory.Exists(strWorkPath))
                {
                    try
                    {
                        Directory.CreateDirectory(strWorkPath);
                    }
                    catch (Exception ex)
                    {
                        errorLog = "Error : " + RevitVersion + " - " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + " - ReadWriteXML1 - " + ex.Message;
                        Logger_();
                    }
                }
                string[] files = Directory.GetFiles(strWorkPath, "*.xml");
                Array.Sort(files, StringComparer.InvariantCulture);
                if (files.Length > 5)
                {
                    try
                    {
                        int end = files.Length - 1;
                        for (int i = end; i >= 0; i--)
                        {
                            FileInfo fi = new FileInfo(files[i]);
                            if (fi.CreationTime < DateTime.Now.AddDays(-5))
                            {
                                fi.Delete();
                            }
                            string[] files2 = Directory.GetFiles(strWorkPath, "*.xml");
                            if (files2.Length <= 5)
                            {
                                break;
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                        errorLog = "Error : " + RevitVersion + " - " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + " - ReadWriteXML2 - " + ex.Message;
                        Logger_();
                    }

                }
                if (File.Exists(strSettingsXmlFilePath))
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(strSettingsXmlFilePath);
                    if (dataTable != null)
                    {
                        DataTable dt1 = ds.Tables[0];
                        dataTable.Merge(dt1);
                        ds.Clear();
                        ds = new DataSet();
                        ds.Tables.Add(dataTable);
                    }
                    if (ds != null)
                    {
                        ds.WriteXml(strSettingsXmlFilePath);
                    }
                }
                else
                {
                    DataSet ds = new DataSet();
                    if (dataTable != null)
                    {
                        ds.Tables.Add(dataTable);
                    }
                    if (ds != null)
                    {
                        ds.WriteXml(strSettingsXmlFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                errorLog = "Error : " + RevitVersion + " - " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + " - ReadWriteXML - " + ex.Message;
                Logger_();

            }
        }
        #endregion

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dt.Rows.Count > 0)
            {
                ReadWriteXML(dt);
                dt = null;
            }

            //System.IO.File.AppendAllText(@"C:\Users\prashant.chaturvedi\Documents\AppFile.txt", this.chkbox_model.Checked.ToString());
           
            //Properties.Settings.Default.Setting = chkbox_model.Checked.ToString();
            //Properties.Settings.Default.Location = this.Location;
            //Properties.Settings.Default.Save();
        }
    }
}
