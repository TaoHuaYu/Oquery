using System;
using System.Data;
using System.Windows.Forms;
//using Oracle.ManagedDataAccess.Client;
using Npgsql;
using System.Threading;

namespace PostgreDB
{
    public class DBCommon
    {
        public string errorMsg;
        private string _ConnString;

        //OracleConnection conn;
        public NpgsqlConnection conn;
        //OracleCommand cmd;
        public NpgsqlCommand cmd;
        //OracleDataReader dr;
        NpgsqlDataReader dr;
        //OracleDataAdapter da;
        NpgsqlDataAdapter da;

        private Thread ThreadQuery;

        #region "資料庫初始化"

        //資料庫初始化
        public bool InitDB()
        {
            string sErrMsg = "";

            ////以下 2017/09/26 test OK
            //_ConnString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.11.9.80)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=MESTEST)));Persist Security Info=True;User ID=ISTMESDEV;Password=ISTMESDEV;";
            
            _ConnString = "Server=192.168.5.50;Port=5432;User Id=jason;Password=0000;Database=wipdb;";

            try
            {
                //conn = new OracleConnection(_ConnString);
                //conn.Open();

                conn = new NpgsqlConnection(_ConnString);
                conn.Open();
            }
            catch (Exception e)
            {
                sErrMsg = e.Message.ToString();

                if (sErrMsg == "網路傳輸: TCP 傳輸位址連線失敗")
                {
                    sErrMsg = "網路連線失敗！\r\n\r\n請確認網路連線是否正常。";
                }

                System.Windows.Forms.MessageBox.Show(sErrMsg);

                return false;
            }

            return true;
        }

        #endregion


        #region "Insert"

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="sSQL">Insert SQL statement</param>
        /// <returns>Insert 成功，回傳 "1"；失敗則回傳錯誤訊息</returns>
        public string Insert(string sSQL)
        {
            try
            {
                cmd = new NpgsqlCommand(sSQL, conn);
                cmd.ExecuteNonQuery();
                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                cmd.Dispose();
                return errorMsg;
            }
        }

        #endregion


        #region "Update"

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="sSQL">Update SQL statement</param>
        /// <returns>Update 回傳成功，傳回 "1"；失敗則回傳錯誤訊息</returns>
        public string Update(string sSQL)
        {
            //if (InitDB() == true)
            //{
                try
                {
                    cmd = new NpgsqlCommand(sSQL, conn);
                    cmd.ExecuteNonQuery();
                    return "1";
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    cmd.Dispose();
                    return errorMsg;
                }
            //}
            //else
            //{
            //    return "?";
            //}
        }

        #endregion


        #region "Query"

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="sSQL">Query SQL statement</param>
        /// <returns></returns>
        public string QueryData(string sSQL)
        {
            //if (InitDB() == true)
            //{
            try
            {
                cmd = new NpgsqlCommand(sSQL, conn);

                //ThreadQuery = new System.Threading.Thread(oOracleReader.ExecuteQuery);
                //ThreadQuery.IsBackground = true;
                //dStartTime = DateTime.Now;
                //ThreadQuery.Start(texteditorQuery.Text);
                //strStatus = "Executing...";

                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    //conn.Close();
                    return "1";
                }
                else
                {
                    //conn.Close();

                    if (dr != null)
                    {
                        dr.Close();
                    }

                    cmd.Dispose();

                    return "0";
                }

                //////////dr = cmd.ExecuteReader();
                //////////if (dr.Read())
                //////////{
                //////////    //conn.Close();
                //////////    return "1";
                //////////}
                //////////else
                //////////{
                //////////    //conn.Close();

                //////////    if (dr != null)
                //////////    {
                //////////        dr.Close();
                //////////    }

                //////////    cmd.Dispose();

                //////////    return "0";
                //////////}

                //while (dr.Read())
                //{
                //    string shop = dr["seqno"].ToString();
                //    string no = dr["devno"].ToString();

                //    Console.WriteLine(shop + "\t" + no);
                //}

                //cmd = new OracleCommand(sSQL, conn);
                //dr = cmd.ExecuteReader();
                //if (dr.Read())
                //{
                //    conn.Close();
                //    return "1";
                //}
                //else
                //{
                //    conn.Close();
                //    return "0";
                //}
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                //conn.Close();

                if (dr != null)
                {
                    dr.Close();
                }

                cmd.Dispose();

                return errorMsg;
            }
            //}
            //else
            //{
            //    return "?";
            //}
        }

        #endregion


        #region "ExecNonQuery"

        /// <summary>
        /// ExecNonQuery
        /// </summary>
        /// <param name="sSQL">Excecute SQL statement None Query</param>
        /// <returns></returns>
        public string ExecNonQuery(string sSQL)
        {
            //InitDB();
            int i = 0;

            try
            {
                cmd = new NpgsqlCommand(sSQL, conn);
                cmd.ExecuteNonQuery();

                return "1";
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                //conn.Close();
                cmd.Dispose();
                return errorMsg;
            }
        }

        #endregion


        #region "Delete"

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="sSQL">Delete SQL statement</param>
        /// <returns></returns>
        public string Delete(string sSQL)
        {
            //InitDB();
            int i = 0;

            try
            {
                cmd = new NpgsqlCommand(sSQL, conn);
                i = cmd.ExecuteNonQuery(); //若沒有任何資料進行異動,則會回傳 0
                //conn.Close();

                if (i == 1)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                //conn.Close();
                cmd.Dispose();
                return errorMsg;
            }
        }

        #endregion


        #region "將查詢結果以DataTable方式回傳"

        /// <summary>
        /// 將查詢結果以DataTable方式回傳
        /// </summary>
        /// <param name="sSQL">Query SQL statement</param>
        /// <returns></returns>
        public DataTable QueryDataToDataTable(string sSQL)
        {
            DataTable DT = new DataTable();

            //if (InitDB() == true)
            //{
                da = new NpgsqlDataAdapter(sSQL, conn);
                //da = new OracleDataAdapter(sSQL, conn);
                
                da.Fill(DT);

                da.Dispose();
                //conn.Close();
            //}

            return DT;
        }

        #endregion


        #region 將資料庫對應的值加到ComboBox清單內

        /// <summary>
        /// 將資料庫對應的值加到ComboBox清單內
        /// </summary>
        /// <param name="objComboBox">ComboBox物件</param>
        /// <param name="sSQL">SQL陳述式</param>
        /// <param name="bAutoWidth">是否要自動調整 ComboBox 的寬度</param>
        /// <param name="bSorted">是否要自動排序</param>
        public void GetComboBox(System.Windows.Forms.ComboBox objComboBox, string sSQL, bool bAutoWidth = false, bool bSorted = true)
        {
            string errorMsg = "";
            string FieldValue = "";
            int iCount = 0;

            objComboBox.Items.Clear();

            if (InitDB() == true)
            {
                try
                {
                    cmd = new NpgsqlCommand(sSQL, conn);
                    //cmd = new OracleCommand(sSQL, conn);
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        FieldValue = dr[0].ToString();
                        objComboBox.Items.Add(FieldValue);
                        iCount = iCount + 1;
                    }

                    if (iCount > 0 && bAutoWidth == true)
                    {
                        int maxWidth = 0;
                        int temp = 0;
                        System.Windows.Forms.Label label1 = new System.Windows.Forms.Label();

                        foreach (var obj in objComboBox.Items)
                        {
                            label1.Text = obj.ToString();
                            temp = label1.PreferredWidth;
                            if (temp > maxWidth)
                            {
                                maxWidth = temp;
                            }
                        }

                        label1.Dispose();

                        objComboBox.DropDownWidth = objComboBox.Width;

                        if (objComboBox.DropDownWidth < maxWidth)
                        {
                            objComboBox.DropDownWidth = maxWidth;
                        }
                    }

                    if (iCount == 1)
                    {
                        objComboBox.SelectedIndex = 0;
                    }
                    else
                    {
                        objComboBox.SelectedIndex = -1;
                    }

                    objComboBox.Sorted = bSorted;
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                }
                finally
                {
                    conn.Close();

                    if (dr != null)
                    {
                        dr.Close();
                    }

                    cmd.Dispose();
                }
            }
        }

        #endregion


        #region 將資料庫對應的值加到ComboBox清單內(畫面上顯示的是 Display, 取值要取 ID)

        /// <summary>
        /// 將資料庫對應的值加到ComboBox清單內
        /// </summary>
        /// <param name="objComboBox">ComboBox物件</param>
        /// <param name="sSQL">SQL陳述式</param>
        /// <param name="bAutoWidth">是否要自動調整 ComboBox 的寬度</param>
        public void GetComboBoxWithValueAndID(System.Windows.Forms.ComboBox objComboBox, string sSQL, bool bAutoWidth = false)
        {
            string errorMsg = "";
            int iCount = 0;

            objComboBox.DataSource = null;
            objComboBox.Items.Clear();

            if (InitDB() == true)
            {
                try
                {
                    DataTable list = new DataTable();
                    list.Columns.Add(new DataColumn("Display", typeof(string)));
                    list.Columns.Add(new DataColumn("ID", typeof(string)));

                    int maxWidth = 0;
                    int temp = 0;
                    System.Windows.Forms.Label label1 = new System.Windows.Forms.Label();

                    cmd = new NpgsqlCommand(sSQL, conn);
                    //cmd = new OracleCommand(sSQL, conn);
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        list.Rows.Add(list.NewRow());
                        list.Rows[iCount][0] = dr[0].ToString();

                        label1.Text = dr[0].ToString();
                        temp = label1.PreferredWidth;
                        if (temp > maxWidth)
                        {
                            maxWidth = temp;
                        }

                        list.Rows[iCount][1] = dr[1].ToString();

                        label1.Text = dr[1].ToString();
                        temp = label1.PreferredWidth;
                        if (temp > maxWidth)
                        {
                            maxWidth = temp;
                        }

                        iCount = iCount + 1;
                    }

                    objComboBox.DataSource = list;
                    objComboBox.DisplayMember = "Display";
                    objComboBox.ValueMember = "ID";

                    if (iCount == 1)
                    {
                        objComboBox.SelectedIndex = 0;
                    }
                    else
                    {
                        objComboBox.SelectedIndex = -1;
                    }

                    label1.Dispose();

                    if (iCount > 0 && bAutoWidth == true)
                    {
                        objComboBox.DropDownWidth = objComboBox.Width;

                        if (objComboBox.DropDownWidth < maxWidth)
                        {
                            objComboBox.DropDownWidth = maxWidth;
                        }
                    }
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                }
                finally
                {
                    conn.Close();

                    if (dr != null)
                    {
                        dr.Close();
                    }

                    cmd.Dispose();
                }
            }
        }

        #endregion


        #region 將資料庫對應的值加到 ListView 清單內

        /// <summary>
        /// 將資料庫對應的值加到 LisView 清單內
        /// </summary>
        /// <param name="objListView">ListView物件</param>
        /// <param name="sSQL">SQL statement</param>
        public void SettingListView(System.Windows.Forms.ListView objListView, string sSQL)
        {
            string errorMsg = "";
            string FieldValue = "";
            int i = 0;

            objListView.Items.Clear();

            if (InitDB() == true)
            {
                try
                {
                    cmd = new NpgsqlCommand(sSQL, conn);
                    //cmd = new OracleCommand(sSQL, conn);
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        FieldValue = dr[0].ToString();
                        objListView.Items.Add(FieldValue);
                        i = i + 1;
                    }
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                }
                finally
                {
                    conn.Close();
                    dr.Close();
                    cmd.Dispose();
                }
            }
        }
        #endregion
        

        #region 取得系統日期
        /// <summary>
        /// 取得系統日期
        /// </summary>
        /// <param name="strDateFormat">YYYY/MM/DD HH24:MI:SS   YYYYMMDD HH24MISS</param>
        /// <returns>Date</returns>
        public string GetSystemDate(string strDateFormat = "YYYY/MM/DD HH24:MI:SS")
        {
            string strTemp = "";
            string sSQL;
            DataTable DT;

            sSQL = "Select To_Char(SysDate, '" + strDateFormat + "') SDate From Dual"; //YYYYMMDDHH24MISS
            DT = QueryDataToDataTable(sSQL);
            if (DT.Rows.Count > 0)
            {
                strTemp = DT.Rows[0]["SDate"].ToString();
            }

            return strTemp;
        }

        #endregion
    }
}