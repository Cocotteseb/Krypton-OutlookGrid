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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonHeaderGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup();
            this.OutlookGrid1 = new JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.KryptonOutlookGrid();
            this.ColumnCustomerID = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.ColumnCustomerName = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.ColumnAddress = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.ColumnCity = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.ColumnCountry = new JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.CustomsColumns.KryptonDataGridViewTextAndImageColumn();
            this.ColumnOrderDate = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewDateTimePickerColumn();
            this.ColumnProduct = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.ColumnPrice = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.SatisfactionColumn = new JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.KryptonDataGridViewPercentageColumn();
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
            this.kryptonHeaderGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonHeaderGroup1.HeaderVisiblePrimary = false;
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
            // 
            // OutlookGrid1
            // 
            this.OutlookGrid1.AllowDrop = true;
            this.OutlookGrid1.AllowUserToAddRows = false;
            this.OutlookGrid1.AllowUserToResizeRows = false;
            this.OutlookGrid1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCustomerID,
            this.ColumnCustomerName,
            this.ColumnAddress,
            this.ColumnCity,
            this.ColumnCountry,
            this.ColumnOrderDate,
            this.ColumnProduct,
            this.ColumnPrice,
            this.SatisfactionColumn});
            this.OutlookGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutlookGrid1.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            this.OutlookGrid1.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.OutlookGrid1.GroupBox = this.KryptonOutlookGridGroupBox1;
            this.OutlookGrid1.HideOuterBorders = true;
            this.OutlookGrid1.Location = new System.Drawing.Point(0, 46);
            this.OutlookGrid1.Name = "OutlookGrid1";
            this.OutlookGrid1.PreviousSelectedGroupRow = -1;
            this.OutlookGrid1.RowHeadersVisible = false;
            this.OutlookGrid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.OutlookGrid1.Size = new System.Drawing.Size(620, 287);
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
            this.ColumnCountry.Image = null;
            this.ColumnCountry.Name = "ColumnCountry";
            this.ColumnCountry.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnCountry.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnCountry.Width = 78;
            // 
            // ColumnOrderDate
            // 
            this.ColumnOrderDate.CalendarTodayDate = new System.DateTime(2013, 5, 11, 0, 0, 0, 0);
            this.ColumnOrderDate.Checked = false;
            this.ColumnOrderDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
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
            dataGridViewCellStyle1.Format = "C2";
            dataGridViewCellStyle1.NullValue = null;
            this.ColumnPrice.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColumnPrice.HeaderText = "Price";
            this.ColumnPrice.Name = "ColumnPrice";
            this.ColumnPrice.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnPrice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            this.ColumnPrice.Width = 79;
            // 
            // SatisfactionColumn
            // 
            dataGridViewCellStyle2.Format = "0%";
            this.SatisfactionColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.SatisfactionColumn.HeaderText = "Satisfaction";
            this.SatisfactionColumn.Name = "SatisfactionColumn";
            this.SatisfactionColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
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
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn ColumnCustomerID;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn ColumnCustomerName;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn ColumnAddress;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn ColumnCity;
        private JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.CustomsColumns.KryptonDataGridViewTextAndImageColumn ColumnCountry;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewDateTimePickerColumn ColumnOrderDate;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn ColumnProduct;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn ColumnPrice;
        private JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.KryptonDataGridViewPercentageColumn SatisfactionColumn;

    }
}

