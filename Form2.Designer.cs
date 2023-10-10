namespace LinkElement
{
    partial class Form2
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
            this.chkbox_door = new System.Windows.Forms.CheckBox();
            this.chkbox_fur = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn_arch = new System.Windows.Forms.Button();
            this.btn_strc = new System.Windows.Forms.Button();
            this.btn_elec = new System.Windows.Forms.Button();
            this.btn_mech = new System.Windows.Forms.Button();
            this.btn_plum = new System.Windows.Forms.Button();
            this.btn_infra = new System.Windows.Forms.Button();
            this.chkbox_model = new System.Windows.Forms.CheckBox();
            this.chkbox_analytical = new System.Windows.Forms.CheckBox();
            this.chkbox_anno = new System.Windows.Forms.CheckBox();
            this.dataGridView_categories = new System.Windows.Forms.DataGridView();
            this.btn_clr = new System.Windows.Forms.Button();
            this.search_txbox = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.lstbox_items = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_categories)).BeginInit();
            this.SuspendLayout();
            // 
            // chkbox_door
            // 
            this.chkbox_door.AutoSize = true;
            this.chkbox_door.Location = new System.Drawing.Point(8, 714);
            this.chkbox_door.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkbox_door.Name = "chkbox_door";
            this.chkbox_door.Size = new System.Drawing.Size(91, 19);
            this.chkbox_door.TabIndex = 3;
            this.chkbox_door.Text = "Lock Doors";
            this.chkbox_door.UseVisualStyleBackColor = true;
            this.chkbox_door.Visible = false;
            this.chkbox_door.CheckedChanged += new System.EventHandler(this.chkbox_door_CheckedChanged);
            // 
            // chkbox_fur
            // 
            this.chkbox_fur.AutoSize = true;
            this.chkbox_fur.Location = new System.Drawing.Point(103, 714);
            this.chkbox_fur.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkbox_fur.Name = "chkbox_fur";
            this.chkbox_fur.Size = new System.Drawing.Size(103, 19);
            this.chkbox_fur.TabIndex = 4;
            this.chkbox_fur.Text = "Lock furniture";
            this.chkbox_fur.UseVisualStyleBackColor = true;
            this.chkbox_fur.Visible = false;
            this.chkbox_fur.CheckedChanged += new System.EventHandler(this.chkbox_fur_CheckedChanged);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Tai Le", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(103, 102);
            this.button3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(45, 35);
            this.button3.TabIndex = 7;
            this.button3.Text = "All";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(211, 73);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(261, 635);
            this.dataGridView1.TabIndex = 11;
            this.dataGridView1.Visible = false;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // btn_arch
            // 
            this.btn_arch.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_arch.Location = new System.Drawing.Point(8, 64);
            this.btn_arch.Name = "btn_arch";
            this.btn_arch.Size = new System.Drawing.Size(35, 35);
            this.btn_arch.TabIndex = 12;
            this.btn_arch.Text = "A";
            this.btn_arch.UseVisualStyleBackColor = true;
            this.btn_arch.Click += new System.EventHandler(this.btn_arch_Click);
            // 
            // btn_strc
            // 
            this.btn_strc.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_strc.Location = new System.Drawing.Point(41, 64);
            this.btn_strc.Name = "btn_strc";
            this.btn_strc.Size = new System.Drawing.Size(35, 35);
            this.btn_strc.TabIndex = 13;
            this.btn_strc.Text = "S";
            this.btn_strc.UseVisualStyleBackColor = true;
            this.btn_strc.Click += new System.EventHandler(this.btn_strc_Click);
            // 
            // btn_elec
            // 
            this.btn_elec.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_elec.Location = new System.Drawing.Point(72, 64);
            this.btn_elec.Name = "btn_elec";
            this.btn_elec.Size = new System.Drawing.Size(35, 35);
            this.btn_elec.TabIndex = 14;
            this.btn_elec.Text = "E";
            this.btn_elec.UseVisualStyleBackColor = true;
            this.btn_elec.Click += new System.EventHandler(this.btn_elec_Click);
            // 
            // btn_mech
            // 
            this.btn_mech.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_mech.Location = new System.Drawing.Point(103, 64);
            this.btn_mech.Name = "btn_mech";
            this.btn_mech.Size = new System.Drawing.Size(35, 35);
            this.btn_mech.TabIndex = 15;
            this.btn_mech.Text = "M";
            this.btn_mech.UseVisualStyleBackColor = true;
            this.btn_mech.Click += new System.EventHandler(this.btn_mech_Click);
            // 
            // btn_plum
            // 
            this.btn_plum.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_plum.Location = new System.Drawing.Point(134, 64);
            this.btn_plum.Name = "btn_plum";
            this.btn_plum.Size = new System.Drawing.Size(35, 35);
            this.btn_plum.TabIndex = 16;
            this.btn_plum.Text = "P";
            this.btn_plum.UseVisualStyleBackColor = true;
            this.btn_plum.Click += new System.EventHandler(this.btn_plum_Click);
            // 
            // btn_infra
            // 
            this.btn_infra.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_infra.Location = new System.Drawing.Point(167, 64);
            this.btn_infra.Name = "btn_infra";
            this.btn_infra.Size = new System.Drawing.Size(35, 35);
            this.btn_infra.TabIndex = 17;
            this.btn_infra.Text = "I";
            this.btn_infra.UseVisualStyleBackColor = true;
            this.btn_infra.Click += new System.EventHandler(this.btn_infra_Click);
            // 
            // chkbox_model
            // 
            this.chkbox_model.AutoSize = true;
            this.chkbox_model.Font = new System.Drawing.Font("Calibri", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkbox_model.Location = new System.Drawing.Point(8, 6);
            this.chkbox_model.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkbox_model.Name = "chkbox_model";
            this.chkbox_model.Size = new System.Drawing.Size(64, 19);
            this.chkbox_model.TabIndex = 18;
            this.chkbox_model.Text = "Model";
            this.chkbox_model.UseVisualStyleBackColor = true;
            // 
            // chkbox_analytical
            // 
            this.chkbox_analytical.AutoSize = true;
            this.chkbox_analytical.Font = new System.Drawing.Font("Calibri", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkbox_analytical.Location = new System.Drawing.Point(8, 45);
            this.chkbox_analytical.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkbox_analytical.Name = "chkbox_analytical";
            this.chkbox_analytical.Size = new System.Drawing.Size(81, 19);
            this.chkbox_analytical.TabIndex = 19;
            this.chkbox_analytical.Text = "Analytical";
            this.chkbox_analytical.UseVisualStyleBackColor = true;
            // 
            // chkbox_anno
            // 
            this.chkbox_anno.AutoSize = true;
            this.chkbox_anno.Font = new System.Drawing.Font("Calibri", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkbox_anno.Location = new System.Drawing.Point(8, 25);
            this.chkbox_anno.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.chkbox_anno.Name = "chkbox_anno";
            this.chkbox_anno.Size = new System.Drawing.Size(91, 19);
            this.chkbox_anno.TabIndex = 20;
            this.chkbox_anno.Text = "Annotation";
            this.chkbox_anno.UseVisualStyleBackColor = true;
            // 
            // dataGridView_categories
            // 
            this.dataGridView_categories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_categories.Location = new System.Drawing.Point(478, 177);
            this.dataGridView_categories.Name = "dataGridView_categories";
            this.dataGridView_categories.RowHeadersWidth = 51;
            this.dataGridView_categories.RowTemplate.Height = 24;
            this.dataGridView_categories.Size = new System.Drawing.Size(174, 531);
            this.dataGridView_categories.TabIndex = 21;
            this.dataGridView_categories.Visible = false;
            this.dataGridView_categories.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_categories_CellContentClick);
            this.dataGridView_categories.CurrentCellChanged += new System.EventHandler(this.dataGridView_categories_CurrentCellChanged);
            this.dataGridView_categories.SelectionChanged += new System.EventHandler(this.dataGridView_categories_SelectionChanged);
            this.dataGridView_categories.Click += new System.EventHandler(this.dataGridView_categories_Click);
            // 
            // btn_clr
            // 
            this.btn_clr.Font = new System.Drawing.Font("Microsoft Tai Le", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_clr.Location = new System.Drawing.Point(157, 102);
            this.btn_clr.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_clr.Name = "btn_clr";
            this.btn_clr.Size = new System.Drawing.Size(45, 35);
            this.btn_clr.TabIndex = 22;
            this.btn_clr.Text = "Clr";
            this.btn_clr.UseVisualStyleBackColor = true;
            this.btn_clr.Click += new System.EventHandler(this.btn_clr_Click);
            // 
            // search_txbox
            // 
            this.search_txbox.Location = new System.Drawing.Point(8, 145);
            this.search_txbox.Name = "search_txbox";
            this.search_txbox.Size = new System.Drawing.Size(120, 20);
            this.search_txbox.TabIndex = 23;
            this.search_txbox.Visible = false;
            this.search_txbox.TextChanged += new System.EventHandler(this.search_txbox_TextChanged);
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(142, 142);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(60, 25);
            this.btn_search.TabIndex = 24;
            this.btn_search.Text = "Search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Visible = false;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // lstbox_items
            // 
            this.lstbox_items.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstbox_items.FormattingEnabled = true;
            this.lstbox_items.ItemHeight = 16;
            this.lstbox_items.Items.AddRange(new object[] {
            "Areas",
            "Audio Visual Devices",
            "Caseworks",
            "Ceilings",
            "Columns",
            "Curtain Panels",
            "Curtain Systems",
            "Details Items",
            "Doors",
            "Electrical Equipments",
            "Electrical Fixtures",
            "Entourages",
            "Fire Protections",
            "Food Service Equipments",
            "Furniture Systems",
            "Generic Models",
            "Hardscapes",
            "Lighting Fixtures",
            "Lines",
            "Mass",
            "Mechanical Equipments",
            "Medical Equipments",
            "Parkings",
            "Parts",
            "Plantings",
            "Plumbing Fixtures",
            "Railings",
            "Ramps",
            "Raster Images",
            "Roads",
            "Roofs",
            "Shaft Openings",
            "Signages",
            "Sites",
            "Speciality Equipments",
            "Stairs",
            "Structural Beam Systems",
            "Structural Columns",
            "Structural Connections",
            "Structural Fundations",
            "Structural Framings",
            "Structural Rebars",
            "Structural Rebar Couplers",
            "Structural Stiffeners",
            "Temporary Structures",
            "Topography",
            "Vertical Circulations",
            "Windows",
            "Floors",
            "Walls",
            "Rooms",
            "Furnitures",
            "Curtain Wall Mullions"});
            this.lstbox_items.Location = new System.Drawing.Point(8, 139);
            this.lstbox_items.Name = "lstbox_items";
            this.lstbox_items.Size = new System.Drawing.Size(194, 564);
            this.lstbox_items.TabIndex = 25;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(207, 753);
            this.Controls.Add(this.lstbox_items);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.search_txbox);
            this.Controls.Add(this.btn_clr);
            this.Controls.Add(this.dataGridView_categories);
            this.Controls.Add(this.chkbox_anno);
            this.Controls.Add(this.chkbox_analytical);
            this.Controls.Add(this.chkbox_model);
            this.Controls.Add(this.btn_infra);
            this.Controls.Add(this.btn_plum);
            this.Controls.Add(this.btn_mech);
            this.Controls.Add(this.btn_elec);
            this.Controls.Add(this.btn_strc);
            this.Controls.Add(this.btn_arch);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.chkbox_door);
            this.Controls.Add(this.chkbox_fur);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.Name = "Form2";
            this.ShowIcon = false;
            this.Text = "Disable Selection";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_categories)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkbox_door;
        private System.Windows.Forms.CheckBox chkbox_fur;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_arch;
        private System.Windows.Forms.Button btn_strc;
        private System.Windows.Forms.Button btn_elec;
        private System.Windows.Forms.Button btn_mech;
        private System.Windows.Forms.Button btn_plum;
        private System.Windows.Forms.Button btn_infra;
        private System.Windows.Forms.CheckBox chkbox_model;
        private System.Windows.Forms.CheckBox chkbox_analytical;
        private System.Windows.Forms.CheckBox chkbox_anno;
        private System.Windows.Forms.DataGridView dataGridView_categories;
        private System.Windows.Forms.Button btn_clr;
        private System.Windows.Forms.TextBox search_txbox;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.ListBox lstbox_items;
    }
}