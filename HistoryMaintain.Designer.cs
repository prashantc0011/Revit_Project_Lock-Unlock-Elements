namespace LinkElement
{
    partial class HistoryMaintain
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
            this.Idtxtbox = new System.Windows.Forms.TextBox();
            this.lbl_Id = new System.Windows.Forms.Label();
            this.btn_cncl = new System.Windows.Forms.Button();
            this.btn_ok = new System.Windows.Forms.Button();
            this.checkBoxDoor = new System.Windows.Forms.CheckBox();
            this.btn_disable = new System.Windows.Forms.Button();
            this.btn_enable = new System.Windows.Forms.Button();
            this.checkBoxfur = new System.Windows.Forms.CheckBox();
            this.checkBoxcw = new System.Windows.Forms.CheckBox();
            this.checkBoxstructcol = new System.Windows.Forms.CheckBox();
            this.btn_unhid_ele = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Idtxtbox
            // 
            this.Idtxtbox.Location = new System.Drawing.Point(30, 68);
            this.Idtxtbox.Name = "Idtxtbox";
            this.Idtxtbox.Size = new System.Drawing.Size(150, 22);
            this.Idtxtbox.TabIndex = 0;
            // 
            // lbl_Id
            // 
            this.lbl_Id.AutoSize = true;
            this.lbl_Id.Location = new System.Drawing.Point(30, 46);
            this.lbl_Id.Name = "lbl_Id";
            this.lbl_Id.Size = new System.Drawing.Size(135, 16);
            this.lbl_Id.TabIndex = 1;
            this.lbl_Id.Text = "Search Element By Id";
            // 
            // btn_cncl
            // 
            this.btn_cncl.Location = new System.Drawing.Point(105, 110);
            this.btn_cncl.Name = "btn_cncl";
            this.btn_cncl.Size = new System.Drawing.Size(75, 25);
            this.btn_cncl.TabIndex = 2;
            this.btn_cncl.Text = "Cancel";
            this.btn_cncl.UseVisualStyleBackColor = true;
            this.btn_cncl.Click += new System.EventHandler(this.btn_cncl_Click);
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(12, 165);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(50, 25);
            this.btn_ok.TabIndex = 3;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // checkBoxDoor
            // 
            this.checkBoxDoor.AutoSize = true;
            this.checkBoxDoor.Location = new System.Drawing.Point(282, 12);
            this.checkBoxDoor.Name = "checkBoxDoor";
            this.checkBoxDoor.Size = new System.Drawing.Size(91, 20);
            this.checkBoxDoor.TabIndex = 4;
            this.checkBoxDoor.Text = "Lock Door";
            this.checkBoxDoor.UseVisualStyleBackColor = true;
            // 
            // btn_disable
            // 
            this.btn_disable.Location = new System.Drawing.Point(92, 165);
            this.btn_disable.Name = "btn_disable";
            this.btn_disable.Size = new System.Drawing.Size(75, 25);
            this.btn_disable.TabIndex = 5;
            this.btn_disable.Text = "Disable";
            this.btn_disable.UseVisualStyleBackColor = true;
            this.btn_disable.Click += new System.EventHandler(this.btn_disable_Click);
            // 
            // btn_enable
            // 
            this.btn_enable.Location = new System.Drawing.Point(186, 165);
            this.btn_enable.Name = "btn_enable";
            this.btn_enable.Size = new System.Drawing.Size(75, 25);
            this.btn_enable.TabIndex = 6;
            this.btn_enable.Text = "Enable";
            this.btn_enable.UseVisualStyleBackColor = true;
            this.btn_enable.Click += new System.EventHandler(this.btn_enable_Click);
            // 
            // checkBoxfur
            // 
            this.checkBoxfur.AutoSize = true;
            this.checkBoxfur.Location = new System.Drawing.Point(282, 38);
            this.checkBoxfur.Name = "checkBoxfur";
            this.checkBoxfur.Size = new System.Drawing.Size(80, 20);
            this.checkBoxfur.TabIndex = 7;
            this.checkBoxfur.Text = "Furniture";
            this.checkBoxfur.UseVisualStyleBackColor = true;
            // 
            // checkBoxcw
            // 
            this.checkBoxcw.AutoSize = true;
            this.checkBoxcw.Location = new System.Drawing.Point(282, 70);
            this.checkBoxcw.Name = "checkBoxcw";
            this.checkBoxcw.Size = new System.Drawing.Size(93, 20);
            this.checkBoxcw.TabIndex = 8;
            this.checkBoxcw.Text = "CaseWork";
            this.checkBoxcw.UseVisualStyleBackColor = true;
            // 
            // checkBoxstructcol
            // 
            this.checkBoxstructcol.AutoSize = true;
            this.checkBoxstructcol.Location = new System.Drawing.Point(282, 96);
            this.checkBoxstructcol.Name = "checkBoxstructcol";
            this.checkBoxstructcol.Size = new System.Drawing.Size(107, 20);
            this.checkBoxstructcol.TabIndex = 9;
            this.checkBoxstructcol.Text = "Structural Col";
            this.checkBoxstructcol.UseVisualStyleBackColor = true;
            // 
            // btn_unhid_ele
            // 
            this.btn_unhid_ele.Location = new System.Drawing.Point(12, 225);
            this.btn_unhid_ele.Name = "btn_unhid_ele";
            this.btn_unhid_ele.Size = new System.Drawing.Size(142, 28);
            this.btn_unhid_ele.TabIndex = 10;
            this.btn_unhid_ele.Text = "Unhide Element";
            this.btn_unhid_ele.UseVisualStyleBackColor = true;
            this.btn_unhid_ele.Click += new System.EventHandler(this.btn_unhid_ele_Click);
            // 
            // HistoryMaintain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(402, 576);
            this.Controls.Add(this.btn_unhid_ele);
            this.Controls.Add(this.checkBoxstructcol);
            this.Controls.Add(this.checkBoxcw);
            this.Controls.Add(this.checkBoxfur);
            this.Controls.Add(this.btn_enable);
            this.Controls.Add(this.btn_disable);
            this.Controls.Add(this.checkBoxDoor);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.btn_cncl);
            this.Controls.Add(this.lbl_Id);
            this.Controls.Add(this.Idtxtbox);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "HistoryMaintain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HistoryMaintain";
            this.Load += new System.EventHandler(this.HistoryMaintain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Idtxtbox;
        private System.Windows.Forms.Label lbl_Id;
        private System.Windows.Forms.Button btn_cncl;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.CheckBox checkBoxDoor;
        private System.Windows.Forms.Button btn_disable;
        private System.Windows.Forms.Button btn_enable;
        private System.Windows.Forms.CheckBox checkBoxfur;
        private System.Windows.Forms.CheckBox checkBoxcw;
        private System.Windows.Forms.CheckBox checkBoxstructcol;
        private System.Windows.Forms.Button btn_unhid_ele;
    }
}