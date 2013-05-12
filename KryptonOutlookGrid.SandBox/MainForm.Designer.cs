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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.OutlookGrid1 = new JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.OutlookGrid();
            this.ColumnCustomerID = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.ColumnCustomerName = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.ColumnAddress = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.ColumnCity = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.ColumnCountry = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.ColumnOrderDate = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewDateTimePickerColumn();
            this.ColumnProduct = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.ColumnPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KryptonOutlookGridGroupBox1 = new JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.KryptonOutlookGridGroupBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OutlookGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(632, 58);
            this.panel1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // OutlookGrid1
            // 
            this.OutlookGrid1.AllowDrop = true;
            this.OutlookGrid1.AllowUserToAddRows = false;
            this.OutlookGrid1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.OutlookGrid1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCustomerID,
            this.ColumnCustomerName,
            this.ColumnAddress,
            this.ColumnCity,
            this.ColumnCountry,
            this.ColumnOrderDate,
            this.ColumnProduct,
            this.ColumnPrice});
            this.OutlookGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutlookGrid1.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            this.OutlookGrid1.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.OutlookGrid1.GroupBox = this.KryptonOutlookGridGroupBox1;
            this.OutlookGrid1.HideOuterBorders = true;
            this.OutlookGrid1.Location = new System.Drawing.Point(0, 104);
            this.OutlookGrid1.Name = "OutlookGrid1";
            this.OutlookGrid1.PreviousSelectedGroupRow = -1;
            this.OutlookGrid1.RowHeadersVisible = false;
            this.OutlookGrid1.Size = new System.Drawing.Size(632, 241);
            this.OutlookGrid1.TabIndex = 0;
            this.OutlookGrid1.Resize += new System.EventHandler(this.OutlookGrid1_Resize);
            // 
            // ColumnCustomerID
            // 
            this.ColumnCustomerID.HeaderText = "Customer ID";
            this.ColumnCustomerID.Name = "ColumnCustomerID";
            this.ColumnCustomerID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnCustomerID.Width = 79;
            // 
            // ColumnCustomerName
            // 
            this.ColumnCustomerName.HeaderText = "Name";
            this.ColumnCustomerName.Name = "ColumnCustomerName";
            this.ColumnCustomerName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnCustomerName.Width = 79;
            // 
            // ColumnAddress
            // 
            this.ColumnAddress.HeaderText = "Address";
            this.ColumnAddress.Name = "ColumnAddress";
            this.ColumnAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnAddress.Width = 79;
            // 
            // ColumnCity
            // 
            this.ColumnCity.HeaderText = "City";
            this.ColumnCity.Name = "ColumnCity";
            this.ColumnCity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnCity.Width = 79;
            // 
            // ColumnCountry
            // 
            this.ColumnCountry.HeaderText = "Country";
            this.ColumnCountry.Name = "ColumnCountry";
            this.ColumnCountry.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnCountry.Width = 78;
            // 
            // ColumnOrderDate
            // 
            this.ColumnOrderDate.CalendarTodayDate = new System.DateTime(2013, 5, 11, 0, 0, 0, 0);
            this.ColumnOrderDate.Checked = false;
            this.ColumnOrderDate.HeaderText = "Order Date";
            this.ColumnOrderDate.Name = "ColumnOrderDate";
            this.ColumnOrderDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnOrderDate.Width = 79;
            // 
            // ColumnProduct
            // 
            this.ColumnProduct.HeaderText = "Product";
            this.ColumnProduct.Name = "ColumnProduct";
            this.ColumnProduct.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnProduct.Width = 79;
            // 
            // ColumnPrice
            // 
            this.ColumnPrice.HeaderText = "Price";
            this.ColumnPrice.Name = "ColumnPrice";
            this.ColumnPrice.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // KryptonOutlookGridGroupBox1
            // 
            this.KryptonOutlookGridGroupBox1.AllowDrop = true;
            this.KryptonOutlookGridGroupBox1.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top;
            this.KryptonOutlookGridGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.KryptonOutlookGridGroupBox1.Location = new System.Drawing.Point(0, 58);
            this.KryptonOutlookGridGroupBox1.Name = "KryptonOutlookGridGroupBox1";
            this.KryptonOutlookGridGroupBox1.Size = new System.Drawing.Size(632, 46);
            this.KryptonOutlookGridGroupBox1.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 345);
            this.Controls.Add(this.OutlookGrid1);
            this.Controls.Add(this.KryptonOutlookGridGroupBox1);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OutlookGrid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.OutlookGrid OutlookGrid1;
        private JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.KryptonOutlookGridGroupBox KryptonOutlookGridGroupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn ColumnCustomerID;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn ColumnCustomerName;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn ColumnAddress;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn ColumnCity;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn ColumnCountry;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewDateTimePickerColumn ColumnOrderDate;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn ColumnProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPrice;

    }
}

