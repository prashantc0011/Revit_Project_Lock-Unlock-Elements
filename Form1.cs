using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinkElement
{
    public partial class Form1 : System.Windows.Forms.Form 
    {
        ExternalCommandData commandData;
        public Form1(ExternalCommandData commandData_)
        {
            InitializeComponent();
            commandData = commandData_;
        }

        private void btn_submit_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogchkbox = MessageBox.Show("Do you want to Lock/Unlock the Elements?", "Select", MessageBoxButtons.YesNo);
                if (dialogchkbox == DialogResult.Yes)
                {
                    if (chkbox_door.Checked == true || chkbox_door.Checked == false)
                    {
                        Lock_door();
                    }
                    if (chkbox_fur.Checked == true || chkbox_fur.Checked == false)
                    {
                        Lock_furniture();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error No 1 : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Lock_door()
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
                    if (chkbox_door.Checked)
                    {
                        doorIds.Add(door.Id);
                        doorIds.Clear();
                        door.CanBeLocked();
                        door.Pinned = true;
                    }
                    else
                    {
                        door.Pinned = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error No 2 : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                foreach (Element furniture in furnitures)
                {
                    if (chkbox_fur.Checked)
                    {
                        furnitureIds.Remove(furniture.Id);
                        furnitureIds.Clear();
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
               
            }
        }

        private void chkbox_sel_all_cat_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbox_sel_all_cat.Checked == true)
            {
                chkbox_clear_all.Checked = false;
                bool selectAllCategories = chkbox_sel_all_cat.Checked;
                for (int i = 0; i < checkedListBox_Cat.Items.Count; i++)
                {
                    checkedListBox_Cat.SetItemChecked(i, selectAllCategories);
                }
            }
            else if (chkbox_sel_all_cat.Checked == false)
            {
                bool unselectAll = chkbox_sel_all_cat.Checked;

                for (int i = 0; i < checkedListBox_Cat.Items.Count; i++)
                {
                    checkedListBox_Cat.SetItemChecked(i, false);
                }
            }
        }

        private void chkbox_sel_all_fam_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbox_sel_all_fam.Checked == true)
            {
                chkbox_clear_all.Checked = false;
                bool selectAllFamlies = chkbox_sel_all_fam.Checked;
                for (int i = 0; i < checkedListBox_Famlies.Items.Count; i++)
                {
                    checkedListBox_Famlies.SetItemChecked(i, selectAllFamlies);
                }
            }
            else if (chkbox_sel_all_fam.Checked == false)
            {
                bool selectAllFamlies = chkbox_sel_all_fam.Checked;
                for (int i = 0; i < checkedListBox_Famlies.Items.Count; i++)
                {
                    checkedListBox_Famlies.SetItemChecked(i, false);
                }
            }
        }

        private void chkbox_sel_all_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbox_sel_all.Checked == true)
            {
                chkbox_sel_all.Enabled = false;
                chkbox_sel_all_cat.Checked = true;
                chkbox_sel_all_fam.Checked = true;
                chkbox_clear_all.Checked = false;

                bool selectAllCategories = chkbox_sel_all.Checked;
                bool selectAllFamlies = chkbox_sel_all.Checked;

                for (int i = 0; i < checkedListBox_Cat.Items.Count; i++)
                {
                    checkedListBox_Cat.SetItemChecked(i, selectAllCategories);
                }

                for (int i = 0; i < checkedListBox_Famlies.Items.Count; i++)
                {
                    checkedListBox_Famlies.SetItemChecked(i, selectAllFamlies);
                }
            }
        }

        private void chkbox_clear_all_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbox_clear_all.Checked == true)
            {
                chkbox_sel_all.Enabled = true;
                chkbox_sel_all.Checked = false;
                chkbox_sel_all_cat.Checked = false;
                chkbox_sel_all_fam.Checked = false;

                for (int i = 0; i < checkedListBox_Cat.Items.Count; i++)
                {
                    checkedListBox_Cat.SetItemChecked(i, false);
                }
                for (int i = 0; i < checkedListBox_Famlies.Items.Count; i++)
                {
                    checkedListBox_Famlies.SetItemChecked(i, false);
                }
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
