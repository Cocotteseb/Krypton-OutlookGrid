namespace KryptonOutlookGrid.SandBox
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.OutlookGridGroupCollection outlookGridGroupCollection1 = new JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.OutlookGridGroupCollection();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonHeaderGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup();
            this.buttonSpecHeaderGroup1 = new ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup();
            this.buttonSpecHeaderGroup2 = new ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup();
            this.buttonSpecHeaderGroup3 = new ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup();
            this.OutlookGrid1 = new JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.KryptonOutlookGrid();
            this.KryptonOutlookGridGroupBox1 = new JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.KryptonOutlookGridGroupBox();
            this.kryptonManager1 = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).BeginInit();
            this.kryptonHeaderGroup1.Panel.SuspendLayout();
            this.kryptonHeaderGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OutlookGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.kryptonHeaderGroup1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(632, 345);
            this.panel1.TabIndex = 2;
            // 
            // kryptonHeaderGroup1
            // 
            this.kryptonHeaderGroup1.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup[] {
            this.buttonSpecHeaderGroup1,
            this.buttonSpecHeaderGroup2,
            this.buttonSpecHeaderGroup3});
            this.kryptonHeaderGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonHeaderGroup1.HeaderVisibleSecondary = false;
            this.kryptonHeaderGroup1.Location = new System.Drawing.Point(5, 5);
            this.kryptonHeaderGroup1.Name = "kryptonHeaderGroup1";
            // 
            // kryptonHeaderGroup1.Panel
            // 
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.OutlookGrid1);
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.KryptonOutlookGridGroupBox1);
            this.kryptonHeaderGroup1.Size = new System.Drawing.Size(622, 335);
            this.kryptonHeaderGroup1.TabIndex = 2;
            this.kryptonHeaderGroup1.ValuesPrimary.Heading = "Options";
            this.kryptonHeaderGroup1.ValuesPrimary.Image = null;
            // 
            // buttonSpecHeaderGroup1
            // 
            this.buttonSpecHeaderGroup1.Text = "Load config";
            this.buttonSpecHeaderGroup1.UniqueName = "B803E19DF9944F1593A01EF944CD3DCB";
            this.buttonSpecHeaderGroup1.Click += new System.EventHandler(this.buttonSpecHeaderGroup1_Click);
            // 
            // buttonSpecHeaderGroup2
            // 
            this.buttonSpecHeaderGroup2.Text = "Save config";
            this.buttonSpecHeaderGroup2.UniqueName = "8759F4DBDA4544F62EA5239B1C0DEC24";
            this.buttonSpecHeaderGroup2.Click += new System.EventHandler(this.buttonSpecHeaderGroup2_Click);
            // 
            // buttonSpecHeaderGroup3
            // 
            this.buttonSpecHeaderGroup3.Text = "Toogle all nodes";
            this.buttonSpecHeaderGroup3.UniqueName = "52F142270C854D89D9886479CF81F2F8";
            this.buttonSpecHeaderGroup3.Click += new System.EventHandler(this.buttonSpecHeaderGroup3_Click);
            // 
            // OutlookGrid1
            // 
            this.OutlookGrid1.AllowDrop = true;
            this.OutlookGrid1.AllowUserToAddRows = false;
            this.OutlookGrid1.AllowUserToResizeRows = false;
            this.OutlookGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutlookGrid1.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            this.OutlookGrid1.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.OutlookGrid1.GroupBox = this.KryptonOutlookGridGroupBox1;
            this.OutlookGrid1.GroupCollection = outlookGridGroupCollection1;
            this.OutlookGrid1.HideOuterBorders = true;
            this.OutlookGrid1.Location = new System.Drawing.Point(0, 46);
            this.OutlookGrid1.Name = "OutlookGrid1";
            this.OutlookGrid1.PreviousSelectedGroupRow = -1;
            this.OutlookGrid1.RowHeadersVisible = false;
            this.OutlookGrid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.OutlookGrid1.ShowLines = false;
            this.OutlookGrid1.Size = new System.Drawing.Size(620, 257);
            this.OutlookGrid1.TabIndex = 0;
            this.OutlookGrid1.GroupImageClick += new System.EventHandler<JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.OutlookGridGroupImageEventArgs>(this.OutlookGrid1_GroupImageClick);
            this.OutlookGrid1.Resize += new System.EventHandler(this.OutlookGrid1_Resize);
            // 
            // KryptonOutlookGridGroupBox1
            // 
            this.KryptonOutlookGridGroupBox1.AllowDrop = true;
            this.KryptonOutlookGridGroupBox1.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom;
            this.KryptonOutlookGridGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.KryptonOutlookGridGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.KryptonOutlookGridGroupBox1.Name = "KryptonOutlookGridGroupBox1";
            this.KryptonOutlookGridGroupBox1.Size = new System.Drawing.Size(620, 46);
            this.KryptonOutlookGridGroupBox1.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 345);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Krypton OutlookGrid Sandbox";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panel1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).EndInit();
            this.kryptonHeaderGroup1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).EndInit();
            this.kryptonHeaderGroup1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OutlookGrid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.KryptonOutlookGrid OutlookGrid1;
        private JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.KryptonOutlookGridGroupBox KryptonOutlookGridGroupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel panel1;
        private ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroup1;
        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager1;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup buttonSpecHeaderGroup1;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup buttonSpecHeaderGroup2;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecHeaderGroup buttonSpecHeaderGroup3;

    }
}

