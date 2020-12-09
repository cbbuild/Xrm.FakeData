namespace CbBuild.Xrm.FakeData.Views
{
    partial class RulesTreeView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tvRules = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tvRules, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(553, 544);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // tvRules
            // 
            this.tvRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvRules.Location = new System.Drawing.Point(3, 26);
            this.tvRules.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.tvRules.Name = "tvRules";
            this.tvRules.Size = new System.Drawing.Size(547, 515);
            this.tvRules.TabIndex = 12;
            this.tvRules.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvRules_KeyDown);
            this.tvRules.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tvRules_KeyPress);
            this.tvRules.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tvRules_KeyUp);
            // 
            // RulesTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "RulesTreeView";
            this.Size = new System.Drawing.Size(553, 544);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView tvRules;
    }
}
