using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Linq;
using Color = System.Drawing.Color;
using DataTable = System.Data.DataTable;
using Document = Autodesk.Revit.DB.Document;
using View = Autodesk.Revit.DB.View;

namespace LinkElement
{
    public partial class Form2 : System.Windows.Forms.Form
    {
        ExternalCommandData commandData;
        string door_cat_Id = null;
        private List<string> selectedItems;
        public Form2(ExternalCommandData commandData_)
        {
            InitializeComponent();
            commandData = commandData_;


            lstbox_items.SelectionMode = SelectionMode.MultiExtended;
            lstbox_items.SelectedIndexChanged += lstbox_items_SelectedIndexChanged;

            // Add items to the ListBox
            //lstbox_items.Items.Add("Doors");
            //lstbox_items.Items.Add("Rooms");
            //lstbox_items.Items.Add("Walls");

            selectedItems = new List<string>();

            this.Controls.Add(lstbox_items);
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            Show_Categories();
        }
        public void lock_only_door()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> doors = collector.ToElements();

                // Create a list to store the door element IDs
                List<ElementId> doorIds = new List<ElementId>();

                foreach (Element door in doors)
                {
                    doorIds.Add(door.Id);
                    door.CanBeLocked();
                    door.Pinned = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Only_Door : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_only_Wall()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
                    .Select((element, index) => new { Element = element, Index = index })
                    .GroupBy(x => x.Index / batchSize)
                    .Select(g => g.Select(x => x.Element));

                // Create a list to store the door element IDs
                List<ElementId> areaIds = new List<ElementId>();

                List<ElementId> all_ele_Ids = new List<ElementId>();

                foreach (IEnumerable<Element> batch in batches)
                {
                    // Process the elements in the current batch
                    foreach (Element element in batch)
                    {
                        if (element.Pinned == true)
                        {
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Walls : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            /*  try
              {
                  // Get the current Revit document
                  Document doc = commandData.Application.ActiveUIDocument.Document;

                  // Create a FilteredElementCollector to collect all the doors in the document
                  FilteredElementCollector collector = new FilteredElementCollector(doc);
                  collector.OfClass(typeof(Wall));

                  // Create a filter to only select the doors
                  // ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_CurtainGridsWall);

                  // Apply the filter to the collector
                  // collector.WherePasses(categoryFilter);

                  // Get the elements that pass the filter
                  IList<Element> walls = collector.ToElements();

                  // Create a list to store the door element IDs
                  List<ElementId> wallIds = new List<ElementId>();

                  foreach (Element wall in walls)
                  {
                      wallIds.Add(wall.Id);
                      wall.CanBeLocked();
                      wall.Pinned = true;
                      // wall.get_Parameter(BuiltInParameter.CURTAIN_WALL_PANEL_HOST_ID).Set(1);
                  }
              }
              catch (Exception ex)
              {
                  MessageBox.Show("Error Lock_Only_Walls : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
              */
        }
        public void Lock_Room()
        {
            /* try
             {
                 // Get the current Revit document
                 Document doc = commandData.Application.ActiveUIDocument.Document;

                 int batchSize = 20;
                 // Create a FilteredElementCollector to collect all the doors in the document
                 FilteredElementCollector collector = new FilteredElementCollector(doc);
                 collector.OfClass(typeof(FamilyInstance));

                 // Create a filter to only select the doors
                 ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Rooms);

                 // Apply the filter to the collector
                 collector.WherePasses(categoryFilter);

                 // Get the elements that pass the filter
                 IList<Element> areas = collector.ToElements();

                 // Create batches using LINQ
                 IEnumerable<IEnumerable<Element>> batches = areas
                     .Select((element, index) => new { Element = element, Index = index })
                     .GroupBy(x => x.Index / batchSize)
                     .Select(g => g.Select(x => x.Element));

                 // Create a list to store the door element IDs
                 List<ElementId> roomIds = new List<ElementId>();

                 List<ElementId> all_ele_Ids = new List<ElementId>();

                   foreach (IEnumerable<Element> batch in batches)
                   {
                       // Process the elements in the current batch
                       foreach (Element element in batch)
                       {
                           if (element.Pinned == true)
                           {
                               // element.Pinned = false;
                               continue;
                           }
                           all_ele_Ids.Add(element.Id);
                           element.CanBeLocked();
                           element.Pinned = true;
                       }
                   } 
             }
             catch (Exception ex)
             {
                 MessageBox.Show("Error Lock_Rooms : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
             } */

            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Get the active view
                View activeView = doc.ActiveView;

                Categories categories = doc.Settings.Categories;

                // Get the door category
                Category roomCategory = Category.GetCategory(doc, BuiltInCategory.OST_Rooms);

                string room_cat_Id = roomCategory.Id.ToString();

                // Create a list to store the door element IDs
                List<ElementId> roomIds = new List<ElementId>();

                foreach (Category category in categories)
                {
                    string cat_Id = category.Id.ToString();
                    if (cat_Id == room_cat_Id)
                    {
                        roomIds.Add(category.Id);
                        // roomIds.Add((ElementId)category.Id);
                    }
                }
                foreach (ElementId room in roomIds)
                {
                    doc.ActiveView.SetCategoryHidden(room, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Only_Room : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void furniture()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Furniture);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Furniture : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            /* try
             {
                 // Get the current Revit document
                 Document doc = commandData.Application.ActiveUIDocument.Document;

                 // Create a FilteredElementCollector to collect all the doors in the document
                 FilteredElementCollector collector = new FilteredElementCollector(doc);
                 collector.OfClass(typeof(FamilyInstance));

                 // Get the active view
                 View activeView = doc.ActiveView;

                 Categories categories = doc.Settings.Categories;

                 Category fur_Category = Category.GetCategory(doc, BuiltInCategory.OST_Furniture);

                 string fur_cat_Id = fur_Category.Id.ToString();

                 // Create a list to store the door element IDs
                 List<ElementId> furIds = new List<ElementId>();

                 foreach (Category category in categories)
                 {
                     string cat_Id = category.Id.ToString();
                     if (cat_Id == fur_cat_Id)
                     {
                         furIds.Add(category.Id);
                     }
                 }

                 foreach (ElementId categoryId in furIds)
                 {
                     doc.ActiveView.SetCategoryHidden(categoryId, true);
                 }
             }
             catch (Exception ex)
             {

             }*/
        }
        public void room()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Get the active view
                View activeView = doc.ActiveView;

                Categories categories = doc.Settings.Categories;

                // Get the door category
                Category roomCategory = Category.GetCategory(doc, BuiltInCategory.OST_Rooms);

                string room_cat_Id = roomCategory.Id.ToString();

                // Create a list to store the door element IDs
                List<ElementId> roomIds = new List<ElementId>();

                foreach (Category category in categories)
                {
                    string cat_Id = category.Id.ToString();
                    if (cat_Id == room_cat_Id)
                    {
                        roomIds.Add(category.Id);
                    }
                }
                foreach (ElementId room in roomIds)
                {
                    doc.ActiveView.SetCategoryHidden(room, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Room Function : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            /*  try
              {
                  // Get the current Revit document
                  Document doc = commandData.Application.ActiveUIDocument.Document;

                  int batchSize = 20;

                  // Create a FilteredElementCollector to collect all the doors in the document
                  FilteredElementCollector collector = new FilteredElementCollector(doc);
                  collector.OfClass(typeof(FamilyInstance));

                  // Get all other element categories in the document
                  Categories categories = doc.Settings.Categories;

                  //  Retrieve the filtered elements
                  List<Element> allElements = collector.ToElements().ToList();

                  // Create a list to store the door element IDs
                  List<ElementId> roomIds = new List<ElementId>();

                  // Create batches using LINQ
                  IEnumerable<IEnumerable<Element>> batches = allElements
                      .Select((element, index) => new { Element = element, Index = index })
                      .GroupBy(x => x.Index / batchSize)
                      .Select(g => g.Select(x => x.Element));

                  foreach (IEnumerable<Element> batch in batches)
                  {
                      // Process the elements in the current batch
                      foreach (Element element in batch)
                      {
                          // Check if the element is a room
                          if (element is FamilyInstance doorInstance && doorInstance.Symbol.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Rooms)
                          {
                              roomIds.Add(element.Id);
                              element.CanBeLocked();
                              element.Pinned = true;
                          }
                      }
                  }
              }
              catch (Exception ex)
              {
                  MessageBox.Show("Error Lock_Only_Room : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }  */
        }
        public void Lock_casework()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Casework);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
                    .Select((element, index) => new { Element = element, Index = index })
                    .GroupBy(x => x.Index / batchSize)
                    .Select(g => g.Select(x => x.Element));

                // Create a list to store the door element IDs
                List<ElementId> areaIds = new List<ElementId>();

                List<ElementId> all_ele_Ids = new List<ElementId>();

                foreach (IEnumerable<Element> batch in batches)
                {
                    // Process the elements in the current batch
                    foreach (Element element in batch)
                    {
                        if (element.Pinned == true)
                        {
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_CaseWorkIds : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Floors()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_FloorsProjection);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
                    .Select((element, index) => new { Element = element, Index = index })
                    .GroupBy(x => x.Index / batchSize)
                    .Select(g => g.Select(x => x.Element));

                // Create a list to store the door element IDs
                List<ElementId> areaIds = new List<ElementId>();

                List<ElementId> all_ele_Ids = new List<ElementId>();

                foreach (IEnumerable<Element> batch in batches)
                {
                    // Process the elements in the current batch
                    foreach (Element element in batch)
                    {
                        if (element.Pinned == true)
                        {
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Floor : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Curtain_Wall_Mullions()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_CurtainWallMullions);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
                    .Select((element, index) => new { Element = element, Index = index })
                    .GroupBy(x => x.Index / batchSize)
                    .Select(g => g.Select(x => x.Element));

                // Create a list to store the door element IDs
                List<ElementId> areaIds = new List<ElementId>();

                List<ElementId> all_ele_Ids = new List<ElementId>();

                foreach (IEnumerable<Element> batch in batches)
                {
                    // Process the elements in the current batch
                    foreach (Element element in batch)
                    {
                        if (element.Pinned == true)
                        {
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Floor : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Areas()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Areas);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
                    .Select((element, index) => new { Element = element, Index = index })
                    .GroupBy(x => x.Index / batchSize)
                    .Select(g => g.Select(x => x.Element));

                // Create a list to store the door element IDs
                List<ElementId> areaIds = new List<ElementId>();

                List<ElementId> all_ele_Ids = new List<ElementId>();

                foreach (IEnumerable<Element> batch in batches)
                {
                    // Process the elements in the current batch
                    foreach (Element element in batch)
                    {
                        if (element.Pinned == true)
                        {
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Caseworks : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Audio_Visual_Devices()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_AudioVisualDevices);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
                    .Select((element, index) => new { Element = element, Index = index })
                    .GroupBy(x => x.Index / batchSize)
                    .Select(g => g.Select(x => x.Element));

                // Create a list to store the door element IDs
                List<ElementId> areaIds = new List<ElementId>();

                List<ElementId> all_ele_Ids = new List<ElementId>();

                foreach (IEnumerable<Element> batch in batches)
                {
                    // Process the elements in the current batch
                    foreach (Element element in batch)
                    {
                        if (element.Pinned == true)
                        {
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Audio_Visual_Devices : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Ceilings()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Ceilings);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
                    .Select((element, index) => new { Element = element, Index = index })
                    .GroupBy(x => x.Index / batchSize)
                    .Select(g => g.Select(x => x.Element));

                // Create a list to store the door element IDs
                List<ElementId> areaIds = new List<ElementId>();

                List<ElementId> all_ele_Ids = new List<ElementId>();

                foreach (IEnumerable<Element> batch in batches)
                {
                    // Process the elements in the current batch
                    foreach (Element element in batch)
                    {
                        if (element.Pinned == true)
                        {
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ceiling : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Columns()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Columns);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
                    .Select((element, index) => new { Element = element, Index = index })
                    .GroupBy(x => x.Index / batchSize)
                    .Select(g => g.Select(x => x.Element));

                // Create a list to store the door element IDs
                List<ElementId> areaIds = new List<ElementId>();

                List<ElementId> all_ele_Ids = new List<ElementId>();

                foreach (IEnumerable<Element> batch in batches)
                {
                    // Process the elements in the current batch
                    foreach (Element element in batch)
                    {
                        if (element.Pinned == true)
                        {
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ceiling : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Elec_Equipments()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalEquipment);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
                    .Select((element, index) => new { Element = element, Index = index })
                    .GroupBy(x => x.Index / batchSize)
                    .Select(g => g.Select(x => x.Element));

                // Create a list to store the door element IDs
                List<ElementId> areaIds = new List<ElementId>();

                List<ElementId> all_ele_Ids = new List<ElementId>();

                foreach (IEnumerable<Element> batch in batches)
                {
                    // Process the elements in the current batch
                    foreach (Element element in batch)
                    {
                        if (element.Pinned == true)
                        {
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ele_Equipments : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Gen_Models()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_GenericModel);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> generic_models = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = generic_models
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ele_Equipments : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Cur_Panels()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_CurtainWallPanels);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
                    .Select((element, index) => new { Element = element, Index = index })
                    .GroupBy(x => x.Index / batchSize)
                    .Select(g => g.Select(x => x.Element));

                // Create a list to store the door element IDs
                List<ElementId> areaIds = new List<ElementId>();

                List<ElementId> all_ele_Ids = new List<ElementId>();

                foreach (IEnumerable<Element> batch in batches)
                {
                    // Process the elements in the current batch
                    foreach (Element element in batch)
                    {
                        if (element.Pinned == true)
                        {
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ele_Equipments : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Cur_Sys()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Curtain_Systems);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
                    .Select((element, index) => new { Element = element, Index = index })
                    .GroupBy(x => x.Index / batchSize)
                    .Select(g => g.Select(x => x.Element));

                // Create a list to store the door element IDs
                List<ElementId> areaIds = new List<ElementId>();

                List<ElementId> all_ele_Ids = new List<ElementId>();

                foreach (IEnumerable<Element> batch in batches)
                {
                    // Process the elements in the current batch
                    foreach (Element element in batch)
                    {
                        if (element.Pinned == true)
                        {
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Cur_Systems : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Detail_Items()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_RepeatingDetailLines);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> det_lines = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = det_lines
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_CaseWorkIds : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Spec_Equip()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_SpecialityEquipment);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ele_Equipments : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Elec_Fixtures()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalFixtures);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ele_Fixtures : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Entourages()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Entourage);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ele_Equipments : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Fire_Protections()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_FireProtection);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ele_Equipments : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Mechanical_Equipments()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_MechanicalEquipment);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ele_Equipments : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Fur_Systems()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_FurnitureSystems);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ele_Equipments : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Hardscapes()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Hardscape);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ele_Equipments : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Lighting_Fixtures()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_LightingFixtures);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ele_Equipments : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_lines()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Lines);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ele_Equipments : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Mass()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Mass);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ele_Equipments : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Medical_Equipments()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_MedicalEquipment);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ele_Equipments : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Parkings()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Parking);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ele_Equipments : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Parts()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Parts);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Parts : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Plantings()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Planting);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Plantings : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Plumbing_Fixtures()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_PlumbingFixtures);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Plumbing_Fixtures : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Railings()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Railings);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Railings : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Ramps()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Ramps);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Ramps : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Raster_Images()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_RasterImages);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Raster_Images : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Roads()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Roads);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Roads : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Roofs()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Roofs);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Roofs : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Struc_Columns()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Structural_Columns : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Shaft_Openings()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_ShaftOpening);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Shaft_Openings : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Signages()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Signage);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Signages : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Sites()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Site);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Sites : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Stairs()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Stairs);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Stairs : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Struc_Foundations()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFoundation);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Structural_Fundations : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Struc_Connections()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralConnectionHandler_Deprecated);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Structural_Connections : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Struc_Framings()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Structural_Framings: " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Struc_Stiffeners()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralStiffener);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Structural_Stiffener : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Rebars_Couplers()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Coupler);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Couplers : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Struc_Rebars()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Rebar);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Struc_Rebars : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Temp_Struc()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_TemporaryStructure);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Temp_Structures : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Topography()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Topography);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Topography : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Ver_Circulations()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_VerticalCirculation);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Vertical_Circulations : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_Windows()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 20;
                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> areas = collector.ToElements();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = areas
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
                            // element.Pinned = false;
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Lock_Windows : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void lstbox_items_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //selectedItems.Clear();
                foreach (var selectedItem in lstbox_items.SelectedItems)
                {
                    selectedItems.Add(selectedItem.ToString());
                }
                foreach (var item in selectedItems)
                {
                    string sel_item = item.ToString();
                    if (sel_item == "Areas")
                    {
                        Lock_Areas();
                    }
                    if (sel_item == "Audio Visual Devices")
                    {
                        Lock_Audio_Visual_Devices();
                    }
                    if (sel_item == "Caseworks")
                    {
                        Lock_casework();
                    }
                    if (sel_item == "Columns")
                    {
                        Lock_Columns();
                    }
                    if (sel_item == "Ceilings")
                    {
                        Lock_Ceilings();
                    }
                    if (sel_item == "Curtain Panels")
                    {
                        Lock_Cur_Panels();
                    }
                    if (sel_item == "Curtain Systems")
                    {
                        Lock_Cur_Sys();
                    }
                    if (sel_item == "Details Items")
                    {
                        Lock_Detail_Items();
                    }
                    if (sel_item == "Doors")
                    {
                        Lock_all_except_door();
                        //Lock_door();
                        //lock_only_door();
                    }
                    if (sel_item == "Electrical Equipments")
                    {
                        Lock_Elec_Equipments();
                    }
                    if (sel_item == "Electrical Fixtures")
                    {
                        Lock_Elec_Fixtures();
                    }
                    if (sel_item == "Entourages")
                    {
                        Lock_Entourages();
                    }
                    if (sel_item == "Fire Protections")
                    {
                        Lock_Fire_Protections();
                    }
                    if (sel_item == "Furniture Systems")
                    {
                        Lock_Fur_Systems();
                    }
                    if (sel_item == "Generic Models")
                    {
                        Lock_Gen_Models();
                    }
                    if (sel_item == "Hardscapes")
                    {
                        Lock_Hardscapes();
                    }
                    if (sel_item == "Lighting Fixtures")
                    {
                        Lock_Lighting_Fixtures();
                    }
                    if (sel_item == "Lines")
                    {
                        Lock_lines();
                    }
                    if (sel_item == "Mass")
                    {
                        Lock_Mass();
                    }
                    if (sel_item == "Mechanical Equipments")
                    {
                        Lock_Mechanical_Equipments();
                    }
                    if (sel_item == "Medical Equipments")
                    {
                        Lock_Medical_Equipments();
                    }
                    if (sel_item == "Parkings")
                    {
                        Lock_Parkings();
                    }
                    if (sel_item == "Parts")
                    {
                        Lock_Parts();
                    }
                    if (sel_item == "Plantings")
                    {
                        Lock_Plantings();
                    }
                    if (sel_item == "Plumbing Fixtures")
                    {
                        Lock_Plumbing_Fixtures();
                    }
                    if (sel_item == "Railings")
                    {
                        Lock_Railings();
                    }
                    if (sel_item == "Ramps")
                    {
                        Lock_Ramps();
                    }
                    if (sel_item == "Raster Images")
                    {
                        Lock_Raster_Images();
                    }
                    if (sel_item == "Roads")
                    {
                        Lock_Roads();
                    }
                    if (sel_item == "Roofs")
                    {
                        Lock_Roofs();
                    }
                    if (sel_item == "Shaft Openings")
                    {
                        Lock_Shaft_Openings();
                    }
                    if (sel_item == "Signages")
                    {
                        Lock_Signages();
                    }
                    if (sel_item == "Sites")
                    {
                        Lock_Sites();
                    }
                    if (sel_item == "Speciality Equipments")
                    {
                        Lock_Spec_Equip();
                    }
                    if (sel_item == "Stairs")
                    {
                        Lock_Stairs();
                    }
                    if (sel_item == "Structural Fundations")
                    {
                        Lock_Struc_Foundations();
                    }
                    if (sel_item == "Structural Columns")
                    {
                        Lock_Struc_Columns();
                    }
                    if (sel_item == "Structural Connections")
                    {
                        Lock_Struc_Connections();
                    }
                    if (sel_item == "Structural Framings")
                    {
                        Lock_Struc_Framings();
                    }
                    if (sel_item == "Structural Rebars")
                    {
                        Lock_Struc_Rebars();
                    }
                    if (sel_item == "Structural Rebar Couplers")
                    {
                        Lock_Rebars_Couplers();
                    }
                    if (sel_item == "Structural Stiffeners")
                    {
                        Lock_Struc_Stiffeners();
                    }
                    if (sel_item == "Temporary Structures")
                    {
                        Lock_Temp_Struc();
                    }
                    if (sel_item == "Topography")
                    {
                        Lock_Topography();
                    }
                    if (sel_item == "Vertical Circulations")
                    {
                        Lock_Ver_Circulations();
                    }
                    if (sel_item == "Windows")
                    {
                        Lock_Windows();
                    }
                    if (sel_item == "Floors")
                    {
                        Lock_Floors();
                    }


                    if (sel_item == "Furnitures")
                    {
                        //Enable_only_furniture();
                        furniture();
                    }
                    if (sel_item == "Walls")
                    {
                        Lock_only_Wall();
                        //Hide_Walls();
                    }
                    if (sel_item == "Rooms")
                    {
                        //room();
                        Lock_Room();
                    }
                    if (sel_item == "Curtain Wall Mullions")
                    {
                        MessageBox.Show("Currently Not Working !!");
                        // Lock_Curtain_Wall_Mullions();
                    }

                }
                /*  if (lstbox_items.Text.ToString() == "Door")
                  {
                      lock_only_door();
                      //Lock_door_2();
                      //Door();
                      // Lock_door();
                      //Lock_all_except_door();
                      //left_door();
                  }
                  else if (lstbox_items.Text.ToString() == "Furniture")
                  {
                      Lock_furniture();
                      //furniture();
                  }
                  else if (lstbox_items.Text.ToString() == "Room")
                  {
                      Lock_Room();
                  }
                  else if (lstbox_items.Text.ToString() == "Wall")
                  {
                      //Hide_Walls();
                      Lock_only_Wall();
                  }
                  */
            }
            catch (Exception ex)
            {

            }
        }




        public void Show_Categories()
        {
            try
            {
                UIApplication uiApp = commandData.Application;
                UIDocument uiDoc = uiApp.ActiveUIDocument;
                Document doc = uiDoc.Document;

                // Get the active view
                View activeView = uiDoc.ActiveView;

                // Get all other element categories in the document
                Categories categories = doc.Settings.Categories;

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Category_Id", typeof(string));
                dataTable.Columns.Add("Categories", typeof(string));

                foreach (Category category_detail in categories)
                {
                    dataTable.Rows.Add(category_detail.Id.ToString(), category_detail.Name.ToString());
                }
                dataGridView_categories.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Show Categories : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {

        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }
        private void dataGridView_categories_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView_categories.SelectedRows.ToString() == "Doors")
                {
                    Lock_door();
                }
                else if (dataGridView_categories.SelectedRows.ToString() == "Furniture")
                {
                    Lock_furniture();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void btn_arch_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                // ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);

                // Apply the filter to the collector
                // collector.WherePasses(categoryFilter);

                // Get the active view
                View activeView = doc.ActiveView;

                Categories categories = doc.Settings.Categories;

                // Get the door category
                Category Arch_Category = Category.GetCategory(doc, BuiltInCategory.OST_BridgeArches);

                string arch_cat_Id = Arch_Category.Id.ToString();
                // Get the elements that pass the filter
                //IList<Element> doors = collector.ToElements();

                // Create a list to store the door element IDs
                List<ElementId> archIds = new List<ElementId>();

                foreach (Category category in categories)
                {
                    string cat_Id = category.Id.ToString();
                    if (cat_Id == arch_cat_Id)
                    {
                        archIds.Add(category.Id);
                    }
                }
                if (archIds.Count > 0)
                {
                    if (btn_arch.BackColor != Color.RoyalBlue)
                    {
                        foreach (ElementId ArchitectureId in archIds)
                        {
                            doc.ActiveView.SetCategoryHidden(ArchitectureId, true);
                            btn_arch.BackColor = Color.RoyalBlue;
                        }
                    }
                    else
                    {
                        foreach (ElementId ArchitectureId in archIds)
                        {
                            doc.ActiveView.SetCategoryHidden(ArchitectureId, false);
                            btn_arch.BackColor = Color.White;
                        }
                    }
                }
                else
                {
                    btn_arch.BackColor = Color.White;
                    MessageBox.Show("No Element(s) found !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Architecture_Equipment : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            /* try
             {
                 // Get the current Revit document
                 Document doc = commandData.Application.ActiveUIDocument.Document;

                 // Create a FilteredElementCollector to collect all the doors in the document
                 FilteredElementCollector collector = new FilteredElementCollector(doc);
                 collector.OfClass(typeof(FamilyInstance));

                 // Create a filter to only select the doors
                 ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_BridgeArches);

                 // Apply the filter to the collector
                 collector.WherePasses(categoryFilter);

                 // Get the elements that pass the filter
                 IList<Element> architectures = collector.ToElements();

                 // Create a list to store the door element IDs
                 List<ElementId> archIds = new List<ElementId>();
                 if (architectures.Count > 0)
                 {
                     if (btn_arch.BackColor != Color.RoyalBlue)
                     {
                         foreach (Element arch in architectures)
                         {

                             archIds.Add(arch.Id);
                             archIds.Clear();
                             arch.CanBeLocked();
                             arch.Pinned = true;
                             btn_arch.BackColor = Color.RoyalBlue;
                         }
                     }
                     else
                     {
                         foreach (Element arch in architectures)
                         {
                             arch.Pinned = false;
                             btn_arch.BackColor = Color.White;
                         }
                     }
                 }
                 else
                 {
                     btn_arch.BackColor = Color.White;
                     MessageBox.Show("No Element(s) found !!");
                 }

             }
             catch (Exception ex)
             {
                 MessageBox.Show("Error Architecture : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
             */
        }
        private void btn_elec_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                // ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);

                // Apply the filter to the collector
                // collector.WherePasses(categoryFilter);

                // Get the active view
                View activeView = doc.ActiveView;

                Categories categories = doc.Settings.Categories;

                // Get the door category
                Category Elec_Category = Category.GetCategory(doc, BuiltInCategory.OST_ElectricalEquipment);

                string elec_cat_Id = Elec_Category.Id.ToString();
                // Get the elements that pass the filter
                //IList<Element> doors = collector.ToElements();

                // Create a list to store the door element IDs
                List<ElementId> elecIds = new List<ElementId>();

                foreach (Category category in categories)
                {
                    string cat_Id = category.Id.ToString();
                    if (cat_Id == elec_cat_Id)
                    {
                        elecIds.Add(category.Id);
                    }
                }
                if (elecIds.Count > 0)
                {
                    if (btn_arch.BackColor != Color.RoyalBlue)
                    {
                        foreach (ElementId Elec_EquipId in elecIds)
                        {
                            doc.ActiveView.SetCategoryHidden(Elec_EquipId, true);
                            btn_arch.BackColor = Color.RoyalBlue;
                        }
                    }
                    else
                    {
                        foreach (ElementId Elec_EquipId in elecIds)
                        {
                            doc.ActiveView.SetCategoryHidden(Elec_EquipId, false);
                            btn_arch.BackColor = Color.White;
                        }
                    }
                }
                else
                {
                    btn_arch.BackColor = Color.White;
                    MessageBox.Show("No Element(s) found !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Electrical_Equipment : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            /* try
             {
                 // Get the current Revit document
                 Document doc = commandData.Application.ActiveUIDocument.Document;

                 // Create a FilteredElementCollector to collect all the doors in the document
                 FilteredElementCollector collector = new FilteredElementCollector(doc);
                 collector.OfClass(typeof(FamilyInstance));

                 // Create a filter to only select the doors
                 ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_ElectricalEquipment);

                 // Apply the filter to the collector
                 collector.WherePasses(categoryFilter);

                 // Get the elements that pass the filter
                 IList<Element> electrical_equips = collector.ToElements();

                 // Create a list to store the door element IDs
                 List<ElementId> equipIds = new List<ElementId>();

                 if (electrical_equips.Count > 0)
                 {
                     if (btn_elec.BackColor != Color.RoyalBlue)
                     {
                         foreach (Element equip in electrical_equips)
                         {
                             equipIds.Add(equip.Id);
                             equipIds.Clear();
                             equip.CanBeLocked();
                             equip.Pinned = true;
                             btn_elec.BackColor = Color.RoyalBlue;
                         }
                     }
                     else
                     {
                         foreach (Element equip in electrical_equips)
                         {
                             equip.Pinned = false;
                             btn_elec.BackColor = Color.White;
                         }
                     }
                 }
                 else
                 {
                     MessageBox.Show("No Element(s) found !!");
                 }
             }
             catch (Exception ex)
             {
                 MessageBox.Show("Error Electrical_Equipment : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
             */
        }
        private void btn_strc_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralAnnotations);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> struct_equips = collector.ToElements();

                // Create a list to store the door element IDs
                List<ElementId> structIds = new List<ElementId>();

                if (struct_equips.Count > 0)
                {
                    if (btn_strc.BackColor != Color.RoyalBlue)
                    {
                        foreach (Element struc in struct_equips)
                        {

                            structIds.Add(struc.Id);
                            structIds.Clear();
                            struc.CanBeLocked();
                            struc.Pinned = true;
                            btn_strc.BackColor = Color.RoyalBlue;
                        }
                    }
                    else
                    {
                        foreach (Element struc in struct_equips)
                        {
                            struc.Pinned = false;
                            btn_strc.BackColor = Color.White;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No Element(s) found !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Structural_Equipment : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_mech_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(MechanicalEquipment));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_MechanicalEquipment);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> struct_equips = collector.ToElements();

                // Create a list to store the door element IDs
                List<ElementId> structIds = new List<ElementId>();

                if (struct_equips.Count > 0)
                {
                    if (btn_mech.BackColor != Color.RoyalBlue)
                    {
                        foreach (Element struc in struct_equips)
                        {
                            structIds.Add(struc.Id);
                            structIds.Clear();
                            struc.CanBeLocked();
                            struc.Pinned = true;
                            btn_mech.BackColor = Color.RoyalBlue;
                        }
                    }
                    else
                    {
                        foreach (Element struc in struct_equips)
                        {
                            struc.Pinned = false;
                            btn_mech.BackColor = Color.White;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No Element(s) found !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Mechanical_Equipment : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_plum_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                // ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);

                // Apply the filter to the collector
                // collector.WherePasses(categoryFilter);

                // Get the active view
                View activeView = doc.ActiveView;

                Categories categories = doc.Settings.Categories;

                // Get the door category
                Category Plumbing_Category = Category.GetCategory(doc, BuiltInCategory.OST_PlumbingFixtures);

                string plumb_cat_Id = Plumbing_Category.Id.ToString();
                // Get the elements that pass the filter
                //IList<Element> doors = collector.ToElements();

                // Create a list to store the door element IDs
                List<ElementId> plumbIds = new List<ElementId>();

                foreach (Category category in categories)
                {
                    string cat_Id = category.Id.ToString();
                    if (cat_Id == plumb_cat_Id)
                    {
                        plumbIds.Add(category.Id);
                    }
                }
                if (plumbIds.Count > 0)
                {
                    if (btn_plum.BackColor != Color.RoyalBlue)
                    {
                        foreach (ElementId PlumbId in plumbIds)
                        {
                            doc.ActiveView.SetCategoryHidden(PlumbId, true);
                            btn_plum.BackColor = Color.RoyalBlue;
                        }
                    }
                    else
                    {
                        foreach (ElementId PlumbId in plumbIds)
                        {
                            doc.ActiveView.SetCategoryHidden(PlumbId, false);
                            btn_plum.BackColor = Color.White;
                        }
                    }
                }
                else
                {
                    btn_plum.BackColor = Color.White;
                    MessageBox.Show("No Element(s) found !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error plumbing_Equipment : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            /* try
             {
                 // Get the current Revit document
                 Document doc = commandData.Application.ActiveUIDocument.Document;

                 // Create a FilteredElementCollector to collect all the doors in the document
                 FilteredElementCollector collector = new FilteredElementCollector(doc);
                 collector.OfClass(typeof(FamilyInstance));

                 // Create a filter to only select the doors
                 ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_PlumbingFixtures);

                 // Apply the filter to the collector
                 collector.WherePasses(categoryFilter);

                 // Get the elements that pass the filter
                 IList<Element> plum_equips = collector.ToElements();

                 // Create a list to store the door element IDs
                 List<ElementId> plumbIds = new List<ElementId>();

                 if (plum_equips.Count > 0)
                 {
                     if (btn_plum.BackColor != Color.RoyalBlue)
                     {
                         foreach (Element plumb in plum_equips)
                         {
                             plumbIds.Add(plumb.Id);
                             plumbIds.Clear();
                             plumb.CanBeLocked();
                             plumb.Pinned = true;
                             btn_plum.BackColor = Color.RoyalBlue;
                         }
                     }
                     else
                     {
                         foreach (Element plumb in plum_equips)
                         {
                             plumb.Pinned = false;
                             btn_plum.BackColor = Color.White;
                         }
                     }
                 }
                 else
                 {
                     MessageBox.Show("No Element(s) found !!");
                 }
             }
             catch (Exception ex)
             {
                 // MessageBox.Show("Error Plumbing_Equipment : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
             } */
        }
        private void btn_infra_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_InsulationLines);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> struct_equips = collector.ToElements();

                // Create a list to store the door element IDs
                List<ElementId> structIds = new List<ElementId>();

                if (struct_equips.Count > 0)
                {
                    if (btn_infra.BackColor != Color.RoyalBlue)
                    {
                        foreach (Element struc in struct_equips)
                        {
                            structIds.Add(struc.Id);
                            structIds.Clear();
                            struc.CanBeLocked();
                            struc.Pinned = true;
                            btn_infra.BackColor = Color.RoyalBlue;
                        }
                    }
                    else
                    {
                        foreach (Element struc in struct_equips)
                        {
                            struc.Pinned = false;
                            btn_infra.BackColor = Color.White;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No Element(s) found !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Infra_Equipment : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {

        }
        private void btn_clr_Click(object sender, EventArgs e)
        {

        }
        private void dataGridView_categories_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView_categories.SelectedRows.ToString() == "Doors")
                {
                    Lock_door();
                }
                else if (dataGridView_categories.SelectedRows.ToString() == "Furniture")
                {
                    Lock_furniture();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void dataGridView_categories_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView_categories.SelectedRows.ToString() == "Doors")
                {
                    Lock_door();
                }
                else if (dataGridView_categories.SelectedRows.ToString() == "Furniture")
                {
                    Lock_furniture();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void search_txbox_TextChanged(object sender, EventArgs e)
        {

        }
        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                if (search_txbox.Text != null)
                {
                    // Filter the ListBox items based on the search text
                    string searchText = search_txbox.Text.ToLower();

                    // Use LINQ to filter the items
                    var filteredItems = lstbox_items.Items.Cast<string>()
                        .Where(item => item.ToLower().Contains(searchText))
                        .ToArray();

                    // Update the ListBox with the filtered items
                    lstbox_items.BeginUpdate();
                    lstbox_items.Items.Clear();
                    lstbox_items.Items.AddRange(filteredItems);
                    lstbox_items.EndUpdate();
                }
                else if (search_txbox.Text == "" || search_txbox.Text == null)
                {
                    // Use LINQ to filter the items
                    var filteredItems = lstbox_items.Items.Cast<string>();

                    // Update the ListBox with the filtered items
                    lstbox_items.BeginUpdate();
                    lstbox_items.EndUpdate();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void dataGridView_categories_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void Lock_all_except_door()
        {
            try
            {
                UIApplication uiApp = commandData.Application;
                UIDocument uiDoc = uiApp.ActiveUIDocument;
                Document doc = uiDoc.Document;

                // Get the active view
                View activeView = uiDoc.ActiveView;

                // Get the door category
                Category doorCategory = Category.GetCategory(doc, BuiltInCategory.OST_Doors);

                // Create an override graphic settings object to hide doors
                OverrideGraphicSettings doorOverrideSettings = new OverrideGraphicSettings();
                //doorOverrideSettings.SetProjectionLineWeight(GraphicsStyleType.Projection, 0); // Set line weight to 0 to hide lines
                doorOverrideSettings.SetSurfaceTransparency(0); // Set transparency to 0 to hide surfaces

                // Create an override graphic settings object to unhide other categories
                OverrideGraphicSettings otherOverrideSettings = new OverrideGraphicSettings();
                //otherOverrideSettings.SetProjectionLineWeight(GraphicsStyleType.Projection, 1); // Set line weight to -1 to use the default value
                otherOverrideSettings.SetSurfaceTransparency(1); // Set transparency to -1 to use the default value

                // Apply the override graphic settings to the elements of the door category
                //using (Transaction trans = new Transaction(doc, "Hide Door Category Elements"))
                //{
                //    trans.Start();

                activeView.SetCategoryOverrides(doorCategory.Id, doorOverrideSettings);

                //    trans.Commit();
                //}

                // Get all other element categories in the document
                Categories categories = doc.Settings.Categories;

                // Iterate through each category
                foreach (Category category in categories)
                {
                    // Skip the door category
                    if (category == doorCategory)
                        continue;

                    // Skip categories that are not visible in the view
                    if (!category.get_Visible(activeView))
                        continue;

                    // Apply the override graphic settings to the elements of the category
                    //using (Transaction trans = new Transaction(doc, "Unhide Other Category Elements"))
                    //{
                    //    trans.Start();

                    // activeView.SetCategoryOverrides(category.Id, otherOverrideSettings);

                    //    trans.Commit();
                    //} 
                }

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                //// Create a filter to only select the doors
                //ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);

                //// Apply the filter to the collector
                //collector.WherePasses(categoryFilter);

                //  Retrieve the filtered elements
                List<Element> allElements = collector.ToElements().ToList();

                // Disable all elements except doors
                //using (Transaction trans = new Transaction(doc, "Disable Elements"))
                //{
                //    trans.Start();

                foreach (Element element in allElements)
                {
                    // Check if the element is a door
                    if (element is FamilyInstance doorInstance && doorInstance.Symbol.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Doors)
                    {
                        // Skip doors
                        continue;
                    }
                    // Disable the element
                    element.Pinned = true;
                    // element.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).IsReadOnly = true;
                }
                //    trans.Commit();
                //}

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Category Id", typeof(string));
                dataTable.Columns.Add("Category Name", typeof(string));

                foreach (Category category_detail in categories)
                {
                    dataTable.Rows.Add(category_detail.Id.ToString(), category_detail.Name.ToString());
                }
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error No 3 : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Door()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Get the active view
                View activeView = doc.ActiveView;

                Categories categories = doc.Settings.Categories;

                // Get the door category
                Category doorCategory = Category.GetCategory(doc, BuiltInCategory.OST_Doors);

                string door_cat_Id = doorCategory.Id.ToString();

                // Create a list to store the door element IDs
                List<ElementId> doorIds = new List<ElementId>();

                foreach (Category category in categories)
                {
                    string cat_Id = category.Id.ToString();
                    if (cat_Id == door_cat_Id)
                    {
                        doorIds.Add(category.Id);
                    }
                }
                foreach (ElementId door in doorIds)
                {
                    doc.ActiveView.SetCategoryHidden(door, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Door Function : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Hide_Walls()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Get the active view
                View activeView = doc.ActiveView;

                Categories categories = doc.Settings.Categories;

                // Get the door category
                Category wall_Category = Category.GetCategory(doc, BuiltInCategory.OST_Walls);

                string wall_cat_Id = wall_Category.Id.ToString();

                // Create a list to store the door element IDs
                List<ElementId> wallIds = new List<ElementId>();

                foreach (Category category in categories)
                {
                    string cat_Id = category.Id.ToString();
                    if (cat_Id == wall_cat_Id)
                    {
                        wallIds.Add(category.Id);
                    }
                }
                foreach (ElementId categoryId in wallIds)
                {
                    doc.ActiveView.SetCategoryHidden(categoryId, true);
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void disable_cat()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_BridgeArches);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> architectures = collector.ToElements();

                // Create a list to store the door element IDs
                List<ElementId> archIds = new List<ElementId>();
                if (architectures.Count > 0)
                {
                    if (btn_arch.BackColor != Color.RoyalBlue)
                    {
                        foreach (Element arch in architectures)
                        {
                            archIds.Add(arch.Id);
                            archIds.Clear();
                            arch.CanBeLocked();
                            arch.Pinned = true;
                            btn_arch.BackColor = Color.RoyalBlue;
                        }
                    }
                    else
                    {
                        foreach (Element arch in architectures)
                        {
                            arch.Pinned = false;
                            btn_arch.BackColor = Color.White;
                        }
                    }
                }
                else
                {
                    btn_arch.BackColor = Color.White;
                    MessageBox.Show("No Element(s) found !!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Architecture : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void left_door()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Get the active view
                View activeView = doc.ActiveView;

                Categories categories = doc.Settings.Categories;

                List<Element> allElements = collector.ToElements().ToList();

                // Get the door category
                Category doorCategory = Category.GetCategory(doc, BuiltInCategory.OST_Doors);

                string door_cat_Id = doorCategory.Id.ToString();

                // Create a list to store the door element IDs
                List<ElementId> doorIds = new List<ElementId>();

                List<ElementId> all_ele = new List<ElementId>();

                foreach (Category category in categories)
                {
                    string cat_Id = category.Id.ToString();
                    if (cat_Id == door_cat_Id)
                    {
                        doorIds.Add(category.Id);
                    }
                    else
                    {
                        all_ele.Add(category.Id);
                    }
                }
                foreach (ElementId cat_id in all_ele)
                {
                    doc.ActiveView.SetCategoryHidden(cat_id, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Door Function : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_door()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                int batchSize = 50;

                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Get all other element categories in the document
                Categories categories = doc.Settings.Categories;

                //  Retrieve the filtered elements
                List<Element> allElements = collector.ToElements().ToList();

                // Create a list to store the door element IDs
                List<ElementId> doorIds = new List<ElementId>();

                List<ElementId> all_ele_Ids = new List<ElementId>();

                // Create batches using LINQ
                IEnumerable<IEnumerable<Element>> batches = allElements
                    .Select((element, index) => new { Element = element, Index = index })
                    .GroupBy(x => x.Index / batchSize)
                    .Select(g => g.Select(x => x.Element));

                foreach (IEnumerable<Element> batch in batches)
                {
                    // Process the elements in the current batch
                    foreach (Element element in batch)
                    {
                        // Check if the element is a door
                        if (element is FamilyInstance doorInstance && doorInstance.Symbol.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Doors)
                        {
                            // Skip doors
                            doorIds.Add(element.Id);
                            continue;
                        }
                        // Get the element by its ElementId
                        //Element _elemnts = doc.GetElement(element.Id);

                        //doc.ActiveView.SetCategoryHidden(element.Id, true);
                        //element.get_Parameter(BuiltInParameter.VIS_GRAPHICS).Set(1);
                        // Disable the element

                        if (element.Pinned == true || element.Category.Name == "Curtain Wall Mullions")
                        {
                            continue;
                        }
                        all_ele_Ids.Add(element.Id);
                        element.CanBeLocked();
                        element.Pinned = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error No 1 : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Enable_only_furniture()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Get the active view
                View activeView = doc.ActiveView;

                int batchSize = 50;

                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Get all other element categories in the document
                Categories categories = doc.Settings.Categories;

                foreach (FilteredElementCollector category in categories)
                {
                    //  Retrieve the filtered elements
                    List<Element> allElements = category.ToElements().ToList();

                    // Create a list to store the door element IDs
                    List<ElementId> doorIds = new List<ElementId>();

                    List<ElementId> all_ele_Ids = new List<ElementId>();

                    // Create batches using LINQ
                    IEnumerable<IEnumerable<Element>> batches = allElements
                        .Select((element, index) => new { Element = element, Index = index })
                        .GroupBy(x => x.Index / batchSize)
                        .Select(g => g.Select(x => x.Element));

                    foreach (IEnumerable<Element> batch in batches)
                    {
                        // Process the elements in the current batch
                        foreach (Element element in batch)
                        {
                            // Check if the element is a door
                            if (element is FamilyInstance doorInstance && doorInstance.Symbol.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Furniture)
                            {
                                // Skip doors
                                doorIds.Add(element.Id);
                                continue;
                            }
                            // Disable the element
                            if (element.Pinned == true || element.Category.Name == "Curtain Wall Mullions")
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
                MessageBox.Show("Error No 1 : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_furniture()
        {
            try
            {
                // Get the current Revit document
                Document doc = commandData.Application.ActiveUIDocument.Document;

                // Create a FilteredElementCollector to collect all the doors in the document
                FilteredElementCollector collector = new FilteredElementCollector(doc);
                collector.OfClass(typeof(FamilyInstance));

                // Create a filter to only select the doors
                ElementCategoryFilter categoryFilter = new ElementCategoryFilter(BuiltInCategory.OST_Furniture);

                // Apply the filter to the collector
                collector.WherePasses(categoryFilter);

                // Get the elements that pass the filter
                IList<Element> furnitures = collector.ToElements();

                // Create a list to store the door element IDs
                List<ElementId> furnitureIds = new List<ElementId>();

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Fur Id", typeof(string));
                dataTable.Columns.Add("Fur Name", typeof(string));

                foreach (Element fur in furnitures)
                {
                    dataTable.Rows.Add(fur.Id.ToString(), fur.Name.ToString());
                }
                dataGridView1.DataSource = dataTable;

                foreach (Element furniture in furnitures)
                {
                    if (lstbox_items.Text.ToString() == "Furniture")
                    {
                        furnitureIds.Add(furniture.Id);
                        furniture.CanBeLocked();
                        furniture.Pinned = true;
                    }
                    else
                    {
                        furniture.Pinned = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Error No 2 : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void chkbox_fur_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbox_fur.Checked == true || chkbox_fur.Checked == false)
            {
                Lock_furniture();
            }
        }
        private void chkbox_door_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbox_door.Checked == true || chkbox_door.Checked == false)
            {
                Lock_door();
            }
        }
        private static List<DataTable> Batch(DataTable originalTable, int batchSize)
        {
            List<DataTable> dts = new List<DataTable>();
            DataTable dt = new DataTable();
            dt = originalTable.Clone();
            int j = 0;
            int k = 1;
            if (originalTable.Rows.Count <= batchSize)
            {
                dt.TableName = "Table_" + k;
                dt = originalTable.Copy();
                dts.Add(dt.Copy());
            }
            else
            {
                for (int i = 0; i < originalTable.Rows.Count; i++)
                {
                    dt.NewRow();
                    dt.ImportRow(originalTable.Rows[i]);
                    if ((i + 1) == originalTable.Rows.Count)
                    {
                        dt.TableName = "Table_" + k;
                        dts.Add(dt.Copy());
                        dt.Rows.Clear();
                        k++;
                    }
                    else if (++j == batchSize)
                    {
                        dt.TableName = "Table_" + k;
                        dts.Add(dt.Copy());
                        dt.Rows.Clear();
                        k++;
                        j = 0;
                    }
                }
            }
            return dts;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
