using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using ComponentFactory.Krypton.Toolkit;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid;
using JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.CustomColumns;

namespace KryptonOutlookGrid.SandBox
{
    public class DataGridViewSetup
    {
        private const int mGRIDCONFIG_VERSION = 1;
        private enum SandBoxGridColumn
        {
            ColumnCustomerID = 0,
            ColumnCustomerName = 1,
            ColumnAddress = 2,
            ColumnCity = 3,
            ColumnCountry = 4,
            ColumnOrderDate = 5,
            ColumnProduct = 6,
            ColumnPrice = 7,
            SatisfactionColumn = 8
        }

        /// <summary>
        /// Use this function if you do not add your columns at design time.
        /// </summary>
        /// <param name="colType"></param>
        /// <returns></returns>
        private DataGridViewColumn SetupColumn(SandBoxGridColumn colType)
        {
            DataGridViewColumn column = null;
            switch (colType)
            {
                case SandBoxGridColumn.ColumnCustomerID:
                    column = new KryptonDataGridViewTextBoxColumn();
                    column.HeaderText = "Customer ID";
                    column.Name = "ColumnCustomerID";
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
                    column.Width = 79;
                    return column;
                case SandBoxGridColumn.ColumnCustomerName:
                    column = new KryptonDataGridViewTreeTextColumn();// KryptonDataGridViewTextBoxColumn();
                    column.HeaderText = "Name";
                    column.Name = "ColumnCustomerName";
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
                    column.Width = 79;
                    return column;
                case SandBoxGridColumn.ColumnAddress:
                    column = new KryptonDataGridViewTextBoxColumn();
                    column.HeaderText = "Address";
                    column.Name = "ColumnAddress";
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
                    column.Width = 79;
                    return column;
                case SandBoxGridColumn.ColumnCity:
                    column = new KryptonDataGridViewTextBoxColumn();
                    column.HeaderText = "City";
                    column.Name = "ColumnCity";
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
                    column.Width = 79;
                    return column;
                case SandBoxGridColumn.ColumnCountry:
                    column = new KryptonDataGridViewTextAndImageColumn();
                    column.HeaderText = "Country";
                    column.Name = "ColumnCountry";
                    column.Resizable = System.Windows.Forms.DataGridViewTriState.True;
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
                    column.Width = 78;
                    return column;
                case SandBoxGridColumn.ColumnOrderDate:
                    column = new KryptonDataGridViewDateTimePickerColumn();
                    ((KryptonDataGridViewDateTimePickerColumn)column).CalendarTodayDate = DateTime.Now;
                    ((KryptonDataGridViewDateTimePickerColumn)column).Checked = false;
                    ((KryptonDataGridViewDateTimePickerColumn)column).Format = System.Windows.Forms.DateTimePickerFormat.Short;
                    column.HeaderText = "Order Date";
                    column.Name = "ColumnOrderDate";
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
                    column.Width = 79;
                    return column;
                case SandBoxGridColumn.ColumnProduct:
                    column = new KryptonDataGridViewTextBoxColumn();
                    column.HeaderText = "Product";
                    column.Name = "ColumnProduct";
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
                    column.Width = 79;
                    return column;
                case SandBoxGridColumn.ColumnPrice:
                    column = new KryptonDataGridViewTextBoxColumn();
                    System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
                    dataGridViewCellStyle1.Format = "C2";
                    dataGridViewCellStyle1.NullValue = null;
                    column.DefaultCellStyle = dataGridViewCellStyle1;
                    column.HeaderText = "Price";
                    column.Name = "ColumnPrice";
                    column.Resizable = System.Windows.Forms.DataGridViewTriState.True;
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
                    column.Width = 79;
                    return column;
                case SandBoxGridColumn.SatisfactionColumn:
                    column = new KryptonDataGridViewPercentageColumn();
                    System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
                    dataGridViewCellStyle2.Format = "0%";
                    column.DefaultCellStyle = dataGridViewCellStyle2;
                    column.HeaderText = "Satisfaction";
                    column.Name = "SatisfactionColumn";
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
                    return column;
                default:
                    throw new Exception("Unknown Column Type !! TODO imprive that !");
            }
        }

        public void SetupDataGridView(JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.KryptonOutlookGrid Grid, bool RestoreIfPossible)
        {
            if (File.Exists(Application.StartupPath + "/grid.xml") & RestoreIfPossible)
            {
                try
                {
                    LoadConfigFromFile(Application.StartupPath + "/grid.xml", Grid);
                }
                catch (Exception ex)
                {
#if (DEBUG)
                    Console.WriteLine("Error when retrieving configuration : " + ex.Message);
#endif
                    Grid.ClearEverything();
                    LoadDefaultConfiguration(Grid);
                }
            }
            else
            {
                LoadDefaultConfiguration(Grid);
            }
        }

        private void LoadConfigFromFile(string file, JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.KryptonOutlookGrid Grid)
        {
            if (string.IsNullOrEmpty(file))
                throw new Exception("Grid config file is missing !");

            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            //Upgrade if necessary the config file
            CheckAndUpgradeConfigFile(doc);
            Grid.ClearEverything();
            Grid.GroupBox.Visible = CommonHelper.StringToBool(doc.SelectSingleNode("OutlookGrid/GroupBox").InnerText);
            Grid.HideColumnOnGrouping = CommonHelper.StringToBool(doc.SelectSingleNode("OutlookGrid/HideColumnOnGrouping").InnerText);

            //initialize
            DataGridViewColumn[] columnsToAdd = new DataGridViewColumn[doc.SelectNodes("//Column").Count];
            OutlookGridColumn[] OutlookColumnsToAdd = new OutlookGridColumn[columnsToAdd.Length];
            SortedList<int, int> hash = new SortedList<int, int>();// (DisplayIndex , Index)

            int i = 0;
            IOutlookGridGroup group;
            XmlNode node2;

            foreach (XmlNode node in doc.SelectSingleNode("OutlookGrid/Columns").ChildNodes)
            {
                //Create the columns and restore the saved properties
                //As the OutlookGrid receives constructed DataGridViewColumns, only the parent application can recreate them (dgvcolumn not serializable)
                columnsToAdd[i] = SetupColumn((SandBoxGridColumn)Enum.Parse(typeof(SandBoxGridColumn), node["Name"].InnerText));
                columnsToAdd[i].Width = int.Parse(node["Width"].InnerText);
                columnsToAdd[i].Visible = CommonHelper.StringToBool(node["Visible"].InnerText);
                hash.Add(int.Parse(node["DisplayIndex"].InnerText), i);
                //Reinit the group if it has been set previously
                group = null;
                if (!node["GroupingType"].IsEmpty && node["GroupingType"].HasChildNodes)
                {
                    node2 = node["GroupingType"];
                    group = (IOutlookGridGroup)Activator.CreateInstance(Type.GetType(TypeConverter.ProcessType(node2["Name"].InnerText), true)); //GetOutlookGridGroup(node2("Name").InnerText)
                    group.OneItemText = node2["OneItemText"].InnerText;
                    group.XXXItemsText = node2["XXXItemsText"].InnerText;
                    group.SortBySummaryCount = CommonHelper.StringToBool(node2["SortBySummaryCount"].InnerText);
                    if (!string.IsNullOrEmpty(node2["ItemsComparer"].InnerText))
                    {
                        Object comparer = Activator.CreateInstance(Type.GetType(TypeConverter.ProcessType(node2["ItemsComparer"].InnerText), true));
                        group.ItemsComparer = (IComparer)comparer;
                    }


                    if ((node2["Name"].InnerText == "OutlookGridDateTimeGroup"))
                    {
                        ((OutlookGridDateTimeGroup)group).Interval = (OutlookGridDateTimeGroup.DateInterval)Enum.Parse(typeof(OutlookGridDateTimeGroup.DateInterval), node2["GroupDateInterval"].InnerText);
                    }
                }

                OutlookColumnsToAdd[i] = new OutlookGridColumn(columnsToAdd[i], group, (SortOrder)Enum.Parse(typeof(SortOrder), node["SortDirection"].InnerText), int.Parse(node["GroupIndex"].InnerText), int.Parse(node["SortIndex"].InnerText));

                i += 1;
            }
            //Add first the DataGridViewColumns
            Grid.Columns.AddRange(columnsToAdd);
            //Add then the outlookgrid columns
            Grid.AddRangeInternalColumns(OutlookColumnsToAdd);

            //We need to loop through the columns in the order of the display order, starting at zero; otherwise the columns will fall out of order as the loop progresses.
            foreach (KeyValuePair<int, int> kvp in hash)
            {
                columnsToAdd[kvp.Value].DisplayIndex = kvp.Key;
            }
        }


        private void CheckAndUpgradeConfigFile(XmlDocument doc)
        {
            int versionGrid = 0;
            int.TryParse(doc.SelectSingleNode("OutlookGrid").Attributes[0].Value, out versionGrid);

            while (versionGrid < mGRIDCONFIG_VERSION)
            {
                UpgradeGridConfigToVX(versionGrid + 1);
                versionGrid += 1;
            }
        }

        private void UpgradeGridConfigToVX(int version)
        {
            //Do changes according to version
        }

        private void LoadDefaultConfiguration(JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.KryptonOutlookGrid Grid)
        {
            Grid.GroupBox.Visible = true;
            Grid.HideColumnOnGrouping = false;

            DataGridViewColumn[] columnsToAdd = new DataGridViewColumn[9];
            columnsToAdd[0] = SetupColumn(SandBoxGridColumn.ColumnCustomerID);
            columnsToAdd[1] = SetupColumn(SandBoxGridColumn.ColumnCustomerName);
            columnsToAdd[2] = SetupColumn(SandBoxGridColumn.ColumnAddress);
            columnsToAdd[3] = SetupColumn(SandBoxGridColumn.ColumnCity);
            columnsToAdd[4] = SetupColumn(SandBoxGridColumn.ColumnCountry);
            columnsToAdd[5] = SetupColumn(SandBoxGridColumn.ColumnOrderDate);
            columnsToAdd[6] = SetupColumn(SandBoxGridColumn.ColumnProduct);
            columnsToAdd[7] = SetupColumn(SandBoxGridColumn.ColumnPrice);
            columnsToAdd[8] = SetupColumn(SandBoxGridColumn.SatisfactionColumn);
            Grid.Columns.AddRange(columnsToAdd);

            //Define the columns for a possible grouping
            Grid.AddInternalColumn(columnsToAdd[0], new OutlookGridDefaultGroup(null), SortOrder.None, -1, -1);
            Grid.AddInternalColumn(columnsToAdd[1], new OutlookGridAlphabeticGroup(null), SortOrder.None, -1, -1);
            Grid.AddInternalColumn(columnsToAdd[2], new OutlookGridDefaultGroup(null), SortOrder.None, -1, -1);
            Grid.AddInternalColumn(columnsToAdd[3], new OutlookGridDefaultGroup(null), SortOrder.None, -1, -1);
            Grid.AddInternalColumn(columnsToAdd[4], new OutlookGridDefaultGroup(null), SortOrder.None, -1, -1);
            Grid.AddInternalColumn(columnsToAdd[5], new OutlookGridDateTimeGroup(null), SortOrder.None, -1, -1);
            Grid.AddInternalColumn(columnsToAdd[6], new OutlookGridDefaultGroup(null) { OneItemText = "1 product", XXXItemsText = " products" }, SortOrder.None, -1, -1);
            Grid.AddInternalColumn(columnsToAdd[7], new OutlookGridDefaultGroup(null), SortOrder.None, -1, -1);
            Grid.AddInternalColumn(columnsToAdd[8], new OutlookGridDefaultGroup(null), SortOrder.None, -1, -1);
        }
    }
}

public class TypeConverter
{
    public static string ProcessType(string FullQualifiedName)
    {
        //Translate types here to accomodate code changes, namespaces and version
        //Select Case FullQualifiedName
        //    Case "JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.OutlookGridAlphabeticGroup, JDHSoftware.Krypton.Toolkit, Version=1.2.0.0, Culture=neutral, PublicKeyToken=e12f297423986ef5",
        //        "JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.OutlookGridAlphabeticGroup, JDHSoftware.Krypton.Toolkit, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null"
        //        'Change with new version or namespace or both !
        //        FullQualifiedName = "TestMe, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null"
        //        Exit Select
        //End Select
        return FullQualifiedName;
    }
}
