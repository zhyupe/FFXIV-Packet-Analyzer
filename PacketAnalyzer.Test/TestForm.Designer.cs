namespace PacketAnalyzer.Test
{
    partial class TestForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.DumpBox = new System.Windows.Forms.TextBox();
            this.PacketList = new System.Windows.Forms.ListView();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.DumpBox);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(862, 0);
            this.panel1.MaximumSize = new System.Drawing.Size(732, 65535);
            this.panel1.MinimumSize = new System.Drawing.Size(732, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(6);
            this.panel1.Size = new System.Drawing.Size(732, 666);
            this.panel1.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(6, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(720, 40);
            this.button1.TabIndex = 1;
            this.button1.Text = "Select";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // DumpBox
            // 
            this.DumpBox.BackColor = System.Drawing.SystemColors.Control;
            this.DumpBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DumpBox.Location = new System.Drawing.Point(6, 46);
            this.DumpBox.Multiline = true;
            this.DumpBox.Name = "DumpBox";
            this.DumpBox.Size = new System.Drawing.Size(720, 614);
            this.DumpBox.TabIndex = 0;
            this.DumpBox.Text = "#ADDRESS: 00 01 02 03 04 05 06 07   08 09 0A 0B 0C 0D 0E 0F : 0123456789ABCDEF\r\n=" +
    "=============================================================================";
            // 
            // PacketList
            // 
            this.PacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PacketList.HideSelection = false;
            this.PacketList.Location = new System.Drawing.Point(0, 0);
            this.PacketList.Name = "PacketList";
            this.PacketList.Size = new System.Drawing.Size(862, 666);
            this.PacketList.TabIndex = 3;
            this.PacketList.UseCompatibleStateImageBehavior = false;
            this.PacketList.View = System.Windows.Forms.View.Details;
            this.PacketList.SelectedIndexChanged += new System.EventHandler(this.PacketList_SelectedIndexChanged);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1594, 666);
            this.Controls.Add(this.PacketList);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox DumpBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView PacketList;
    }
}