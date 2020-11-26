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
            this.tvRules = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // tvRules
            // 
            this.tvRules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvRules.Location = new System.Drawing.Point(0, 0);
            this.tvRules.Name = "tvRules";
            this.tvRules.Size = new System.Drawing.Size(553, 544);
            this.tvRules.TabIndex = 12;
            // 
            // RulesTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvRules);
            this.Name = "RulesTreeView";
            this.Size = new System.Drawing.Size(553, 544);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvRules;
    }
}
