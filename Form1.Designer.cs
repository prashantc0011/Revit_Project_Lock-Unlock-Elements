namespace LinkElement
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_submit = new System.Windows.Forms.Button();
            this.chkbox_door = new System.Windows.Forms.CheckBox();
            this.chkbox_fur = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.grpbox_action = new System.Windows.Forms.GroupBox();
            this.btn_close = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkedListBox_Famlies = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox_Cat = new System.Windows.Forms.CheckedListBox();
            this.chkbox_clear_all = new System.Windows.Forms.CheckBox();
            this.chkbox_sel_all = new System.Windows.Forms.CheckBox();
            this.chkbox_sel_all_fam = new System.Windows.Forms.CheckBox();
            this.chkbox_sel_all_cat = new System.Windows.Forms.CheckBox();
            this.grpbox_action.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_submit
            // 
            this.btn_submit.Location = new System.Drawing.Point(27, 19);
            this.btn_submit.Name = "btn_submit";
            this.btn_submit.Size = new System.Drawing.Size(80, 30);
            this.btn_submit.TabIndex = 0;
            this.btn_submit.Text = "Submit";
            this.btn_submit.UseVisualStyleBackColor = true;
            this.btn_submit.Click += new System.EventHandler(this.btn_submit_Click);
            // 
            // chkbox_door
            // 
            this.chkbox_door.AutoSize = true;
            this.chkbox_door.Location = new System.Drawing.Point(47, 29);
            this.chkbox_door.Name = "chkbox_door";
            this.chkbox_door.Size = new System.Drawing.Size(81, 17);
            this.chkbox_door.TabIndex = 1;
            this.chkbox_door.Text = "Lock Doors";
            this.chkbox_door.UseVisualStyleBackColor = true;
            // 
            // chkbox_fur
            // 
            this.chkbox_fur.AutoSize = true;
            this.chkbox_fur.Location = new System.Drawing.Point(166, 29);
            this.chkbox_fur.Name = "chkbox_fur";
            this.chkbox_fur.Size = new System.Drawing.Size(91, 17);
            this.chkbox_fur.TabIndex = 2;
            this.chkbox_fur.Text = "Lock furniture";
            this.chkbox_fur.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "1).";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(141, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "2).";
            // 
            // grpbox_action
            // 
            this.grpbox_action.Controls.Add(this.btn_close);
            this.grpbox_action.Controls.Add(this.btn_submit);
            this.grpbox_action.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpbox_action.Location = new System.Drawing.Point(590, 380);
            this.grpbox_action.Name = "grpbox_action";
            this.grpbox_action.Size = new System.Drawing.Size(200, 60);
            this.grpbox_action.TabIndex = 5;
            this.grpbox_action.TabStop = false;
            this.grpbox_action.Text = "Action";
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(113, 19);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(80, 30);
            this.btn_close.TabIndex = 1;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkbox_clear_all);
            this.groupBox1.Controls.Add(this.chkbox_sel_all);
            this.groupBox1.Controls.Add(this.chkbox_sel_all_fam);
            this.groupBox1.Controls.Add(this.chkbox_sel_all_cat);
            this.groupBox1.Controls.Add(this.checkedListBox_Famlies);
            this.groupBox1.Controls.Add(this.checkedListBox_Cat);
            this.groupBox1.Controls.Add(this.chkbox_door);
            this.groupBox1.Controls.Add(this.chkbox_fur);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(777, 362);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Categories/Families";
            // 
            // checkedListBox_Famlies
            // 
            this.checkedListBox_Famlies.FormattingEnabled = true;
            this.checkedListBox_Famlies.Items.AddRange(new object[] {
            "Door",
            "Furniture",
            "Wall",
            "Electrical",
            "Mechanical",
            "Plumbing",
            "Railling",
            "GrillDoor",
            "Furniture",
            "Wall",
            "Electrical",
            "Mechanical",
            "Plumbing",
            "Railling",
            "GrillDoor",
            "Furniture",
            "Wall",
            "Electrical",
            "Mechanical",
            "Plumbing",
            "Railling",
            "GrillDoor",
            "Furniture",
            "Wall",
            "Electrical",
            "Mechanical",
            "Plumbing",
            "Railling",
            "GrillDoor",
            "Furniture",
            "Wall",
            "Electrical",
            "Mechanical",
            "Plumbing",
            "Railling",
            "Grill"});
            this.checkedListBox_Famlies.Location = new System.Drawing.Point(422, 52);
            this.checkedListBox_Famlies.Name = "checkedListBox_Famlies";
            this.checkedListBox_Famlies.Size = new System.Drawing.Size(326, 229);
            this.checkedListBox_Famlies.TabIndex = 7;
            // 
            // checkedListBox_Cat
            // 
            this.checkedListBox_Cat.CheckOnClick = true;
            this.checkedListBox_Cat.FormattingEnabled = true;
            this.checkedListBox_Cat.Items.AddRange(new object[] {
            "Door",
            "Furniture",
            "Wall",
            "Electrical",
            "Mechanical",
            "Plumbing",
            "Railling",
            "GrillDoor",
            "Furniture",
            "Wall",
            "Electrical",
            "Mechanical",
            "Plumbing",
            "Railling",
            "GrillDoor",
            "Furniture",
            "Wall",
            "Electrical",
            "Mechanical",
            "Plumbing",
            "Railling",
            "GrillDoor",
            "Furniture",
            "Wall",
            "Electrical",
            "Mechanical",
            "Plumbing",
            "Railling",
            "GrillDoor",
            "Furniture",
            "Wall",
            "Electrical",
            "Mechanical",
            "Plumbing",
            "Railling",
            "Grill"});
            this.checkedListBox_Cat.Location = new System.Drawing.Point(24, 52);
            this.checkedListBox_Cat.Name = "checkedListBox_Cat";
            this.checkedListBox_Cat.Size = new System.Drawing.Size(340, 229);
            this.checkedListBox_Cat.TabIndex = 6;
            // 
            // chkbox_clear_all
            // 
            this.chkbox_clear_all.AutoSize = true;
            this.chkbox_clear_all.Location = new System.Drawing.Point(695, 330);
            this.chkbox_clear_all.Name = "chkbox_clear_all";
            this.chkbox_clear_all.Size = new System.Drawing.Size(64, 17);
            this.chkbox_clear_all.TabIndex = 13;
            this.chkbox_clear_all.Text = "Clear All";
            this.chkbox_clear_all.UseVisualStyleBackColor = true;
            this.chkbox_clear_all.CheckedChanged += new System.EventHandler(this.chkbox_clear_all_CheckedChanged);
            // 
            // chkbox_sel_all
            // 
            this.chkbox_sel_all.AutoSize = true;
            this.chkbox_sel_all.Location = new System.Drawing.Point(605, 330);
            this.chkbox_sel_all.Name = "chkbox_sel_all";
            this.chkbox_sel_all.Size = new System.Drawing.Size(70, 17);
            this.chkbox_sel_all.TabIndex = 12;
            this.chkbox_sel_all.Text = "Select All";
            this.chkbox_sel_all.UseVisualStyleBackColor = true;
            this.chkbox_sel_all.CheckedChanged += new System.EventHandler(this.chkbox_sel_all_CheckedChanged);
            // 
            // chkbox_sel_all_fam
            // 
            this.chkbox_sel_all_fam.AutoSize = true;
            this.chkbox_sel_all_fam.Location = new System.Drawing.Point(511, 330);
            this.chkbox_sel_all_fam.Name = "chkbox_sel_all_fam";
            this.chkbox_sel_all_fam.Size = new System.Drawing.Size(79, 17);
            this.chkbox_sel_all_fam.TabIndex = 11;
            this.chkbox_sel_all_fam.Text = "Select Fam";
            this.chkbox_sel_all_fam.UseVisualStyleBackColor = true;
            this.chkbox_sel_all_fam.CheckedChanged += new System.EventHandler(this.chkbox_sel_all_fam_CheckedChanged);
            // 
            // chkbox_sel_all_cat
            // 
            this.chkbox_sel_all_cat.AutoSize = true;
            this.chkbox_sel_all_cat.Location = new System.Drawing.Point(410, 330);
            this.chkbox_sel_all_cat.Name = "chkbox_sel_all_cat";
            this.chkbox_sel_all_cat.Size = new System.Drawing.Size(75, 17);
            this.chkbox_sel_all_cat.TabIndex = 10;
            this.chkbox_sel_all_cat.Text = "Select Cat";
            this.chkbox_sel_all_cat.UseVisualStyleBackColor = true;
            this.chkbox_sel_all_cat.CheckedChanged += new System.EventHandler(this.chkbox_sel_all_cat_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpbox_action);
            this.Name = "Form1";
            this.Text = "Disable Selection";
            this.grpbox_action.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_submit;
        private System.Windows.Forms.CheckBox chkbox_door;
        private System.Windows.Forms.CheckBox chkbox_fur;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grpbox_action;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkbox_clear_all;
        private System.Windows.Forms.CheckBox chkbox_sel_all;
        private System.Windows.Forms.CheckBox chkbox_sel_all_fam;
        private System.Windows.Forms.CheckBox chkbox_sel_all_cat;
        private System.Windows.Forms.CheckedListBox checkedListBox_Famlies;
        private System.Windows.Forms.CheckedListBox checkedListBox_Cat;
    }
}