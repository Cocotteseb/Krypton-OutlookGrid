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
using JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.CustomsColumns;

namespace KryptonOutlookGrid.SandBox
{
    public class DataGridViewSetup
    {
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
                    column.HeaderText = "Customer ID";
                    column.Name = "ColumnCustomerID";
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
                    column.Width = 79;
                    return column;
                case SandBoxGridColumn.ColumnCustomerName:
                    column.HeaderText = "Name";
                    column.Name = "ColumnCustomerName";
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
                    column.Width = 79;
                    return column;
                case SandBoxGridColumn.ColumnAddress:
                    column.HeaderText = "Address";
                    column.Name = "ColumnAddress";
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
                    column.Width = 79;
                    return column;
                case SandBoxGridColumn.ColumnCity:
                    column.HeaderText = "City";
                    column.Name = "ColumnCity";
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
                    column.Width = 79;
                    return column;
                case SandBoxGridColumn.ColumnCountry:
                    column.HeaderText = "Country";
                    ((KryptonDataGridViewTextAndImageColumn)column).Image = null;
                    column.Name = "ColumnCountry";
                    column.Resizable = System.Windows.Forms.DataGridViewTriState.True;
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
                    column.Width = 78;
                    return column;
                case SandBoxGridColumn.ColumnOrderDate:
                    ((KryptonDataGridViewDateTimePickerColumn)column).CalendarTodayDate = DateTime.Now;
                    ((KryptonDataGridViewDateTimePickerColumn)column).Checked = false;
                    ((KryptonDataGridViewDateTimePickerColumn)column).Format = System.Windows.Forms.DateTimePickerFormat.Short;
                    column.HeaderText = "Order Date";
                    column.Name = "ColumnOrderDate";
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
                    column.Width = 79;
                    return column;
                case SandBoxGridColumn.ColumnProduct:
                    column.HeaderText = "Product";
                    column.Name = "ColumnProduct";
                    column.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
                    column.Width = 79;
                    return column;
                case SandBoxGridColumn.ColumnPrice:
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

        private IOutlookGridGroup GetOutlookGridGroup(string @group)
        {
            switch (@group)
            {
                case "OutlookGridDefaultGroup":
                    return new OutlookGridDefaultGroup(null);
                case "OutlookGridAlphabeticGroup":
                    return new OutlookGridAlphabeticGroup(null);
                case "OutlookGridDateTimeGroup":
                    return new OutlookGridDateTimeGroup(null);
                default:
                    throw new ArgumentException("The group " + @group + " is unknown");
                //Add any custom group you are using in your application
            }
        }

        public void SetupDataGridView(JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.KryptonOutlookGrid Grid, bool RestoreIfPossible)
        {
            if (File.Exists(Application.StartupPath + "grid.xml") & RestoreIfPossible)
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(Application.StartupPath + "grid.xml");

                    if (doc.SelectSingleNode("OutlookGrid").Attributes[0].Value.Equals("1"))
                    {
                        Grid.GroupBox.Visible = CommonHelper.StringToBool(doc.SelectSingleNode("OutlookGrid/GroupBox").InnerText);
                        Grid.HideColumnOnGrouping = CommonHelper.StringToBool(doc.SelectSingleNode("OutlookGrid/HideColumnOnGrouping").InnerText);
                        //initialize
                        DataGridViewColumn[] columnsToAdd = new DataGridViewColumn[doc.SelectNodes("//Column").Count];
                        //Dim l As New List(Of DataGridViewColumn)
                        OutlookGridColumn[] OutlookColumnsToAdd = new OutlookGridColumn[columnsToAdd.Length];
                        SortedList<int, int> hash = new SortedList<int, int>();
                        // (DisplayIndex , Index)

                        int i = 0;
                        IOutlookGridGroup @group = default(IOutlookGridGroup);
                        XmlNode node2 = null;
                        foreach (XmlNode node in doc.SelectSingleNode("OutlookGrid/Columns").ChildNodes)
                        {
                            columnsToAdd[i] = SetupColumn((SandBoxGridColumn)Enum.Parse(typeof(SandBoxGridColumn), node["Name"].InnerText));
                            columnsToAdd[i].Width = int.Parse(node["Width"].InnerText);
                            columnsToAdd[i].Visible = CommonHelper.StringToBool(node["Visible"].InnerText);
                            hash.Add(int.Parse(node["DisplayIndex"].InnerText), i);
                            //columnsToAdd(i).DisplayIndex = Integer.Parse(node("DisplayIndex").InnerText)
                            //l.Add(columnsToAdd(i))
                            //Reinit
                            @group = null;
                            if (!node["GroupingType"].IsEmpty && node["GroupingType"].HasChildNodes)
                            {
                                node2 = node["GroupingType"];
                                @group = GetOutlookGridGroup(node2["Name"].InnerText);
                                @group.OneItemText = node2["OneItemText"].InnerText;
                                @group.XXXItemsText = node2["XXXItemsText"].InnerText;
                                @group.SortBySummaryCount = CommonHelper.StringToBool(node2["SortBySummaryCount"].InnerText);
                                if ((node2["Name"].InnerText == "OutlookGridDateTimeGroup"))
                                {
                                    ((OutlookGridDateTimeGroup)@group).Interval = (OutlookGridDateTimeGroup.DateInterval)Enum.Parse(typeof(OutlookGridDateTimeGroup.DateInterval), node2["GroupDateInterval"].InnerText);
                                }
                            }
                            //Although we could use full reflection generation, for performance improvement we use a static dictionnary
                            //Grid.AddInternalColumn(columnsToAdd(i), group, _
                            //                           [Enum].Parse(GetType(SortOrder), node("SortDirection").InnerText), _
                            //                           Integer.Parse(node("GroupIndex").InnerText), Integer.Parse(node("SortIndex").InnerText))
                            OutlookColumnsToAdd[i] = new OutlookGridColumn(columnsToAdd[i], @group, (SortOrder)Enum.Parse(typeof(SortOrder), node["SortDirection"].InnerText), int.Parse(node["GroupIndex"].InnerText), int.Parse(node["SortIndex"].InnerText));

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
                }
                catch (Exception ex)
                {
                    //TODO log error
                    Console.WriteLine("Error when retrieving configuration : " + ex.Message);
                    Grid.ClearEverything();
                    LoadDefaultConfiguration(Grid);
                }
            }
            else
            {
                LoadDefaultConfiguration(Grid);
            }
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
