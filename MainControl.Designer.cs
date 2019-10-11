namespace PacketAnalyzer
{
    partial class MainControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.PacketList = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.DumpBox = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.filterButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.characterButton = new System.Windows.Forms.Button();
            this.pcapButton = new System.Windows.Forms.Button();
            this.EnabledBox = new System.Windows.Forms.CheckBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ipcTypesList = new System.Windows.Forms.ListView();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // PacketList
            // 
            this.PacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PacketList.HideSelection = false;
            this.PacketList.Location = new System.Drawing.Point(0, 0);
            this.PacketList.Name = "PacketList";
            this.PacketList.Size = new System.Drawing.Size(344, 763);
            this.PacketList.TabIndex = 0;
            this.PacketList.UseCompatibleStateImageBehavior = false;
            this.PacketList.View = System.Windows.Forms.View.Details;
            this.PacketList.SelectedIndexChanged += new System.EventHandler(this.PacketList_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(344, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(6);
            this.panel1.Size = new System.Drawing.Size(732, 763);
            this.panel1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 48);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(720, 709);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.DumpBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(712, 677);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Dump";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // DumpBox
            // 
            this.DumpBox.BackColor = System.Drawing.SystemColors.Window;
            this.DumpBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DumpBox.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DumpBox.Location = new System.Drawing.Point(3, 3);
            this.DumpBox.Multiline = true;
            this.DumpBox.Name = "DumpBox";
            this.DumpBox.ReadOnly = true;
            this.DumpBox.Size = new System.Drawing.Size(706, 671);
            this.DumpBox.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.panel3);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(712, 677);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Filter";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ipcTypesList);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(306, 630);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IPC Types";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.filterButton);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(3, 633);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(706, 41);
            this.panel3.TabIndex = 1;
            // 
            // filterButton
            // 
            this.filterButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.filterButton.Location = new System.Drawing.Point(577, 0);
            this.filterButton.Name = "filterButton";
            this.filterButton.Size = new System.Drawing.Size(129, 41);
            this.filterButton.TabIndex = 4;
            this.filterButton.Text = "Filter";
            this.filterButton.UseVisualStyleBackColor = true;
            this.filterButton.Click += new System.EventHandler(this.FilterButton_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.characterButton);
            this.panel2.Controls.Add(this.pcapButton);
            this.panel2.Controls.Add(this.EnabledBox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(6, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(720, 42);
            this.panel2.TabIndex = 2;
            // 
            // characterButton
            // 
            this.characterButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.characterButton.Enabled = false;
            this.characterButton.Location = new System.Drawing.Point(425, 0);
            this.characterButton.Name = "characterButton";
            this.characterButton.Size = new System.Drawing.Size(159, 42);
            this.characterButton.TabIndex = 3;
            this.characterButton.Text = "Load Character";
            this.characterButton.UseVisualStyleBackColor = true;
            this.characterButton.Click += new System.EventHandler(this.CharacterButton_Click);
            // 
            // pcapButton
            // 
            this.pcapButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.pcapButton.Location = new System.Drawing.Point(584, 0);
            this.pcapButton.Name = "pcapButton";
            this.pcapButton.Size = new System.Drawing.Size(136, 42);
            this.pcapButton.TabIndex = 2;
            this.pcapButton.Text = "Load .pcap";
            this.pcapButton.UseVisualStyleBackColor = true;
            this.pcapButton.Click += new System.EventHandler(this.Button1_Click);
            // 
            // EnabledBox
            // 
            this.EnabledBox.AutoSize = true;
            this.EnabledBox.Checked = true;
            this.EnabledBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnabledBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.EnabledBox.Location = new System.Drawing.Point(0, 0);
            this.EnabledBox.Name = "EnabledBox";
            this.EnabledBox.Size = new System.Drawing.Size(97, 42);
            this.EnabledBox.TabIndex = 1;
            this.EnabledBox.Text = "Enabled";
            this.EnabledBox.UseVisualStyleBackColor = true;
            this.EnabledBox.CheckedChanged += new System.EventHandler(this.EnabledBox_CheckedChanged);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Pcap File|*.pcap";
            // 
            // ipcTypesList
            // 
            this.ipcTypesList.CheckBoxes = true;
            this.ipcTypesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ipcTypesList.HideSelection = false;
            this.ipcTypesList.Location = new System.Drawing.Point(3, 24);
            this.ipcTypesList.Name = "ipcTypesList";
            this.ipcTypesList.Size = new System.Drawing.Size(300, 603);
            this.ipcTypesList.TabIndex = 0;
            this.ipcTypesList.UseCompatibleStateImageBehavior = false;
            this.ipcTypesList.View = System.Windows.Forms.View.List;
            // 
            // MainControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PacketList);
            this.Controls.Add(this.panel1);
            this.Name = "MainControl";
            this.Size = new System.Drawing.Size(1076, 763);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView PacketList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox DumpBox;
        private System.Windows.Forms.CheckBox EnabledBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button pcapButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button filterButton;
        public System.Windows.Forms.Button characterButton;
        private System.Windows.Forms.ListView ipcTypesList;
    }
}
