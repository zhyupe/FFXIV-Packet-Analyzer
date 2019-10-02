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
            this.DumpBox = new System.Windows.Forms.TextBox();
            this.EnabledBox = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PacketList
            // 
            this.PacketList.Dock = System.Windows.Forms.DockStyle.Left;
            this.PacketList.HideSelection = false;
            this.PacketList.Location = new System.Drawing.Point(0, 0);
            this.PacketList.Name = "PacketList";
            this.PacketList.Size = new System.Drawing.Size(499, 763);
            this.PacketList.TabIndex = 0;
            this.PacketList.UseCompatibleStateImageBehavior = false;
            this.PacketList.View = System.Windows.Forms.View.Details;
            this.PacketList.SelectedIndexChanged += new System.EventHandler(this.PacketList_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.DumpBox);
            this.panel1.Controls.Add(this.EnabledBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(499, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(6);
            this.panel1.Size = new System.Drawing.Size(577, 763);
            this.panel1.TabIndex = 1;
            // 
            // DumpBox
            // 
            this.DumpBox.BackColor = System.Drawing.SystemColors.Control;
            this.DumpBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DumpBox.Location = new System.Drawing.Point(6, 28);
            this.DumpBox.Multiline = true;
            this.DumpBox.Name = "DumpBox";
            this.DumpBox.ReadOnly = true;
            this.DumpBox.Size = new System.Drawing.Size(565, 729);
            this.DumpBox.TabIndex = 0;
            // 
            // EnabledBox
            // 
            this.EnabledBox.AutoSize = true;
            this.EnabledBox.Checked = true;
            this.EnabledBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EnabledBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.EnabledBox.Location = new System.Drawing.Point(6, 6);
            this.EnabledBox.Name = "EnabledBox";
            this.EnabledBox.Size = new System.Drawing.Size(565, 22);
            this.EnabledBox.TabIndex = 1;
            this.EnabledBox.Text = "Enabled";
            this.EnabledBox.UseVisualStyleBackColor = true;
            // 
            // MainControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.PacketList);
            this.Name = "MainControl";
            this.Size = new System.Drawing.Size(1076, 763);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView PacketList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox DumpBox;
        private System.Windows.Forms.CheckBox EnabledBox;
    }
}
