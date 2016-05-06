using JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid;
using JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.CustomColumns;
using JDHSoftware.Krypton.Toolkit.KryptonOutlookGrid.Formatting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

namespace KryptonOutlookGrid.SandBox
{
    public partial class MainForm : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        static Random rand = new Random();

        DateTime GetRandomDate(DateTime dtStart, DateTime dtEnd)
        {
            int cdayRange = (dtEnd - dtStart).Days;

            return dtStart.AddDays(rand.NextDouble() * cdayRange);
        }

        private void LoadData()
        {
            //Setup Rows
            OutlookGridRow row = new OutlookGridRow();
            List<OutlookGridRow> l = new List<OutlookGridRow>();
            OutlookGrid1.SuspendLayout();
            OutlookGrid1.ClearInternalRows();
            OutlookGrid1.FillMode = FillMode.GroupsAndNodes;

            List<Token> tokensList = new List<Token>();
            tokensList.Add(new Token("Best seller", Color.Orange, Color.Black));
            tokensList.Add(new Token("New", Color.LightGreen, Color.Black));
            tokensList.Add(null);
            tokensList.Add(null);
            tokensList.Add(null);

            Random random = new Random();
            //.Next permet de retourner un nombre aléatoire contenu dans la plage spécifiée entre parenthèses.
            XmlDocument doc = new XmlDocument();
            doc.Load("invoices.xml");
            IFormatProvider culture = new CultureInfo("en-US", true);
            foreach (XmlNode customer in doc.SelectNodes("//invoice")) //TODO for instead foreach for perfs...
            {
                try
                {
                    row = new OutlookGridRow();
                    row.CreateCells(OutlookGrid1, new object[] {
                        customer["CustomerID"].InnerText,
                        customer["CustomerName"].InnerText,
                        customer["Address"].InnerText,
                        customer["City"].InnerText,
                        new TextAndImage(customer["Country"].InnerText,GetFlag(customer["Country"].InnerText)),
                        DateTime.Parse(customer["OrderDate"].InnerText,culture),
                        customer["ProductName"].InnerText,
                        double.Parse(customer["Price"].InnerText, CultureInfo.InvariantCulture), //We put a float the formatting in design does the rest
                        (double)random.Next(101) /100,
                        tokensList[random.Next(5)]
                    });
                    if (random.Next(2) == 1)
                    {
                        //Sub row
                        OutlookGridRow row2 = new OutlookGridRow();
                        row2.CreateCells(OutlookGrid1, new object[] {
                            customer["CustomerID"].InnerText + " 2",
                            customer["CustomerName"].InnerText + " 2",
                            customer["Address"].InnerText + "2",
                            customer["City"].InnerText + " 2",
                            new TextAndImage(customer["Country"].InnerText,GetFlag(customer["Country"].InnerText)),
                            DateTime.Now,
                            customer["ProductName"].InnerText + " 2",
                            (double)random.Next(1000),
                            (double)random.Next(101) /100,
                            tokensList[random.Next(5)]
                        });
                        row.Nodes.Add(row2);
                        ((KryptonDataGridViewTreeTextCell)row2.Cells[1]).UpdateStyle(); //Important : after added to the parent node
                    }
                    l.Add(row);
                    ((KryptonDataGridViewTreeTextCell)row.Cells[1]).UpdateStyle(); //Important : after added to the rows list
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gasp...Something went wrong ! " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

       

            OutlookGrid1.ResumeLayout();
            OutlookGrid1.AssignRows(l);
            OutlookGrid1.ForceRefreshGroupBox();
            OutlookGrid1.Fill();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            OutlookGrid1.GroupBox = KryptonOutlookGridGroupBox1;
            OutlookGrid1.RegisterGroupBoxEvents();

            DataGridViewSetup setup = new DataGridViewSetup();
            setup.SetupDataGridView(this.OutlookGrid1, true);

            OutlookGrid1.ShowLines = true;
            LoadData();
        }

        private Image GetFlag(string country)
        {
            //Icons from http://365icon.com/icon-styles/ethnic/classic2/

            switch (country)
            {
                case "France":
                    return Properties.Resources.fr;
                case "Germany":
                    return Properties.Resources.de;
                case "Argentina":
                    return Properties.Resources.ar;
                case "Austria":
                    return Properties.Resources.au;
                case "Belgium":
                    return Properties.Resources.be;
                case "Brazil":
                    return Properties.Resources.br;
                case "Canada":
                    return Properties.Resources.ca;
                case "Denmark":
                    return Properties.Resources.dk;
                case "Finland":
                    return Properties.Resources.fi;
                case "Ireland":
                    return Properties.Resources.ie;
                case "Italy":
                    return Properties.Resources.it;
                case "Mexico":
                    return Properties.Resources.mx;
                case "Norway":
                    return Properties.Resources.no;
                case "Poland":
                    return Properties.Resources.pl;
                case "Portugal":
                    return Properties.Resources.pt;
                case "Spain":
                    return Properties.Resources.es;
                case "Sweden":
                    return Properties.Resources.se;
                case "Switzerland":
                    return Properties.Resources.ch;
                case "UK":
                    return Properties.Resources.gb;
                case "USA":
                    return Properties.Resources.us;
                case "Venezuela":
                    return Properties.Resources.ve;
                default:
                    return null;
            }
        }

        private void OutlookGrid1_Resize(object sender, EventArgs e)
        {
            int PreferredTotalWidth = 0;
            //Calculate the total preferred width
            foreach (DataGridViewColumn c in OutlookGrid1.Columns)
            {
                PreferredTotalWidth += Math.Min(c.GetPreferredWidth(DataGridViewAutoSizeColumnMode.DisplayedCells, true), 250);
            }

            if (OutlookGrid1.Width > PreferredTotalWidth)
            {
                OutlookGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                OutlookGrid1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            }
            else
            {
                OutlookGrid1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                foreach (DataGridViewColumn c in OutlookGrid1.Columns)
                {
                    c.Width = Math.Min(c.GetPreferredWidth(DataGridViewAutoSizeColumnMode.DisplayedCells, true), 250);
                }
            }
        }

        private void OutlookGrid1_GroupImageClick(object sender, OutlookGridGroupImageEventArgs e)
        {
            MessageBox.Show("Group Image clicked for group row : " + e.Row.Group.Text);
        }

        private void buttonSpecHeaderGroup1_Click(object sender, EventArgs e)
        {
            DataGridViewSetup setup = new DataGridViewSetup();
            setup.SetupDataGridView(this.OutlookGrid1, true);
            LoadData();

        }

        private void buttonSpecHeaderGroup2_Click(object sender, EventArgs e)
        {
            OutlookGrid1.PersistConfiguration(Application.StartupPath + "/grid.xml", StaticInfos._GRIDCONFIG_VERSION.ToString());
        }

        bool expand = true;

        private void buttonSpecHeaderGroup3_Click(object sender, EventArgs e)
        {
            if (expand)
                OutlookGrid1.ExpandAllNodes();
            else
                OutlookGrid1.CollapseAllNodes();

            expand = !expand;
        }
    }
}

