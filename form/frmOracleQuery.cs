using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace PostgreDB
{
    public partial class frmOracleQuery : Form
    {
        private DBCommon dBCommon = new DBCommon();
        private OracleTransaction myTrans;
        private clsOracleReader oOracleReader = new clsOracleReader();
        private Thread ThreadQuery;
        private delegate void BindDatagrid();
        private DateTime dStartTime;
        private DateTime dEndTime;

        public frmOracleQuery()
        {
            InitializeComponent();
        }

        private void frmOracleQuery_Load(object sender, EventArgs e)
        {
            
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string sTemp = txtQuery.SelectedText;

            oOracleReader.QueryCompleted += QueryCompleted;

            dStartTime = DateTime.Now;
            txtStatus.Text = "Executing...";

            this.Cursor = Cursors.Cross;

            if (txtQuery.Text != "")
            {
                ThreadQuery = new Thread(oOracleReader.ExecuteQuery);
                ThreadQuery.IsBackground = true;
                ThreadQuery.Start(txtQuery.Text);
            }
        }

        private void btnInterruptQuery_Click(object sender, EventArgs e)
        {
            oOracleReader.InterruptQuery();

            if (ThreadQuery != null)
            {
                ThreadQuery.Abort();
            }

            ThreadQuery = null;
            txtStatus.Text = "Execution aborted...";
            this.Cursor = Cursors.Default;
        }

        public void QueryCompleted()
        {
            if (this.InvokeRequired)
            {
                try
                {
                    BindDatagrid d = new BindDatagrid(BindDatagridHandler);
                    Invoke(d);
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                BindDatagridHandler();
            }

            ThreadQuery = null;
            oOracleReader.Clear();
        }

        private void BindDatagridHandler()
        {
            DataTable oDataTable = oOracleReader.Datatable;

            if (oDataTable != null)
            {
                //dtGrid.DataSource = oDataTable; // oDataset.Tables[0];
                //dtGrid.Show();
                dEndTime = DateTime.Now;
                txtStatus.Text = oDataTable.Rows.Count + " rows selected in " + Math.Round(dEndTime.Subtract(dStartTime).TotalSeconds, 3) + " seconds";
                this.Cursor = Cursors.Default;
            }

            ArrangeDataTable(oDataTable);
        }

        private string GetFormatPostgre(string sType)
        {
            string sTemp = "";
            //Boolean

            switch (sType.ToUpper())
            {
                case "INT16":
                case "INT32":
                case "INT64":
                case "UINT16":
                case "UINT32":
                case "UINT64":
                    //sTemp = Convert.ToDateTime(sTemp).ToString("yyyy/MM/dd HH:mm:ss");
                    break;
                case "BYTE":
                case "CHAR":
                case "STRING":
                    break;
                /*DateTime
                Decimal
                Double
                SByte
                Single
                TimeSpan*/
                case "TIMESPAN":
                    sTemp = Convert.ToDateTime(sTemp).ToString("HH:mm:ss");
                    break;
                default:
                    //sTemp = sType.ToUpper();
                    break;
            }

            return "";
        }

        private void ArrangeDataTable(DataTable dtData, bool bSort = true)
        {
            string sCSVContent = "", sTemp = "", sHeader = "";
            string[] sArrayHeader = null, sArraySeparators = { "," };
            DataTable dtSortedData = new DataTable();
            DataRow rowData;
            string sFormatDataType = "", sFormatPostgre = "";

            if (1 == 1) //(dtData.Rows.Count > 0)
            {
                for (int i = 0; i < dtData.Columns.Count; i++)
                {
                    sTemp = dtData.Columns[i].DataType.ToString().Replace("System.", "");
                    sFormatDataType = sTemp;
                    sFormatPostgre = GetFormatPostgre(sTemp);
                    sHeader += dtData.Columns[i].ColumnName.ToString() + "\r\n" + sFormatDataType + ","; //"\r\n" + sFormatPostgre +
                }

                sArrayHeader = sHeader.Split(sArraySeparators, StringSplitOptions.RemoveEmptyEntries);

                if (bSort == true)
                {
                    Array.Sort(sArrayHeader, string.CompareOrdinal); //ASCII排序
                }

                for (int i = 0; i < dtData.Columns.Count; i++)
                {
                    //System.Type typeInt32 = System.Type.GetType("System.Int32");
                    //DataColumn column = new DataColumn("id", typeInt32);
                    dtSortedData.Columns.Add(sArrayHeader[i].ToString(), typeof(String));
                }

                for (int i = 0; i < dtData.Rows.Count; i++)
                {
                    rowData = dtSortedData.NewRow();

                    for (int j = 0; j < dtData.Columns.Count; j++)
                    {
                        sTemp = dtData.Rows[i][dtData.Columns[j].ColumnName.ToString()].ToString();

                        switch (dtData.Columns[j].DataType.ToString().Replace("System.", "").ToUpper())
                        {
                            case "DATETIME":
                                if (sTemp.ToString() != "")
                                {
                                    sTemp = Convert.ToDateTime(sTemp).ToString("yyyy/MM/dd HH:mm:ss");
                                }

                                break;
                            case "TIMESPAN":
                                if (sTemp.ToString() != "")
                                {
                                    sTemp = Convert.ToDateTime(sTemp).ToString("HH:mm:ss");
                                }
                                
                                break;
                            default:
                                break;
                        }

                        rowData[dtData.Columns[j].ColumnName.ToString() + "\r\n" + dtData.Columns[j].DataType.ToString().Replace("System.", "")] = sTemp;
                    }

                    dtSortedData.Rows.Add(rowData);
                }
            }

            dtGrid.DataSource = dtSortedData;
            dtGrid.Show();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string strConnectionString = "";

            //strConnectionString = "User Id=mestest; Password=sa; Data Source=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.5.49)(PORT = 1521))(CONNECT_DATA=(SERVICE_NAME=MESDB));"; // DBA Privilege=SYSDBA;";
            strConnectionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.5.49)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=MESDB)));Persist Security Info=True;User ID=mestest;Password=sa;";
            strConnectionString = "User Id=sys; Password=84193721; Data Source=(ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521))(CONNECT_DATA = (SERVICE_NAME = XE)); DBA Privilege=SYSDBA;";

            if (oOracleReader.ConnectTo(strConnectionString))
            {
                //ToolStripProgressBar.Value = 80;
                //ConnectToDatabase = Microsoft.VisualBasic.TriState.True;
                //ToolStripStatusDB.Text = login.UsernameTextBox.Text + "@" + login.DatabaseTextBox.Text;
            }
            else
            {
                //ToolStripProgressBar.Value = 80;
                //ConnectToDatabase = Microsoft.VisualBasic.TriState.False;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool bValue = oOracleReader.oCommit();
        }
    }
}