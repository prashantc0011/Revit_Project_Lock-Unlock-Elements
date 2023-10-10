using Autodesk.Revit.DB.Structure;
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
    public partial class Form4 : System.Windows.Forms.Form
    {
        ExternalCommandData commandData;
        public Form4(ExternalCommandData commandData_)
        {
            InitializeComponent();
            commandData = commandData_;
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // lbl_msg.Text = "Ensure 'select pinned elements' under the modify option should be unchecked while using Lock Element Plugin.";
        }

        private void btn_abort_Click(object sender, EventArgs e)
        {
            this.Close();
            //Form4 frm_4 = new Form4(commandData);
            //frm_4.Close();
        }

        private void btn_Proceed_Click(object sender, EventArgs e)
        {
            //this.Close();

            //Form3 frm_3 = new Form3(commandData);
            //frm_3.Show();

            bool IsOpen = false;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Text == "Disable Selection")
                {
                    IsOpen = true;
                    this.Close();
                    frm.BringToFront();
                    break;
                }

            }
            if (IsOpen == false)
            {
                this.Close();
                Form3 frm_3 = new Form3(commandData);
                frm_3.Show();
            }


        }

        private void lbl_msg_Click(object sender, EventArgs e)
        {

        }
    }
}
