namespace LinkElement
{
    partial class Form4
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form4));
            this.lbl_msg = new System.Windows.Forms.Label();
            this.btn_abort = new System.Windows.Forms.Button();
            this.btn_Proceed = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_msg
            // 
            this.lbl_msg.AutoSize = true;
            this.lbl_msg.Location = new System.Drawing.Point(104, 49);
            this.lbl_msg.Name = "lbl_msg";
            this.lbl_msg.Size = new System.Drawing.Size(337, 32);
            this.lbl_msg.TabIndex = 0;
            this.lbl_msg.Text = "Ensure \'select pinned elements\' under the modify option\r\nshould be unchecked whil" +
    "e using Lock Element Plugin.\r\n";
            this.lbl_msg.Click += new System.EventHandler(this.lbl_msg_Click);
            // 
            // btn_abort
            // 
            this.btn_abort.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_abort.BackColor = System.Drawing.SystemColors.Control;
            this.btn_abort.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_abort.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btn_abort.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.btn_abort.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_abort.Location = new System.Drawing.Point(226, 125);
            this.btn_abort.Name = "btn_abort";
            this.btn_abort.Size = new System.Drawing.Size(85, 35);
            this.btn_abort.TabIndex = 1;
            this.btn_abort.Text = "Abort";
            this.btn_abort.UseVisualStyleBackColor = false;
            this.btn_abort.Click += new System.EventHandler(this.btn_abort_Click);
            // 
            // btn_Proceed
            // 
            this.btn_Proceed.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_Proceed.BackColor = System.Drawing.SystemColors.Control;
            this.btn_Proceed.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_Proceed.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btn_Proceed.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.btn_Proceed.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Proceed.Location = new System.Drawing.Point(331, 125);
            this.btn_Proceed.Name = "btn_Proceed";
            this.btn_Proceed.Size = new System.Drawing.Size(85, 35);
            this.btn_Proceed.TabIndex = 2;
            this.btn_Proceed.Text = "Proceed";
            this.btn_Proceed.UseVisualStyleBackColor = false;
            this.btn_Proceed.Click += new System.EventHandler(this.btn_Proceed_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(86, 72);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(550, 181);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn_Proceed);
            this.Controls.Add(this.btn_abort);
            this.Controls.Add(this.lbl_msg);
            this.ForeColor = System.Drawing.SystemColors.InfoText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form4";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lock Element(s)";
            this.Load += new System.EventHandler(this.Form4_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_msg;
        private System.Windows.Forms.Button btn_abort;
        private System.Windows.Forms.Button btn_Proceed;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}