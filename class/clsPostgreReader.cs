using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Threading.Tasks;
using Npgsql;

namespace PostgreDB
{
    class clsPostgreReader
    {
        /// <summary>
        /// Connection to the Oracle database 
        /// </summary>
        /// <remarks>This member of the class is shared, because we use the same connection for all the project</remarks>
        private static NpgsqlConnection mConn;

        /// <summary>
        /// Command object used to make queries
        /// </summary>
        /// <remarks></remarks>
        private NpgsqlCommand oCommand = new NpgsqlCommand();

        /// <summary>
        /// Adapter object used to make queries
        /// </summary>
        /// <remarks></remarks>
        private NpgsqlDataAdapter oDataAdapter;

        /// <summary>
        /// Dataset object used to make queries
        /// </summary>
        /// <remarks></remarks>
        private DataSet oDataset;

        private DataTable oDataTable;

        /// <summary>
        /// Event raised at the end of a query, used to bind the datagridview async
        /// </summary>
        /// <remarks></remarks>
        public delegate void QueryCompletedEventHandler();
        public event QueryCompletedEventHandler QueryCompleted;

        /// <summary>
        /// Dataset Property
        /// </summary>
        /// <value></value>
        /// <returns>Return the dataset after the QueryCompleted() event was raised</returns>
        /// <remarks>ReadOnly Property</remarks>
        public DataSet Dataset
        {
            get
            {
                return oDataset;
            }
        }

        public DataTable Datatable
        {
            get
            {
                return oDataTable;
            }
        }

        /// <summary>
        /// Execute the connection to the database with the provided connection string
        /// </summary>
        /// <param name="strConnectionString">Connection string</param>
        /// <returns>Return the status of the connection</returns>
        /// <remarks></remarks>
        public bool ConnectTo(string strConnectionString)
        {
            bool tempConnectTo = false;

            mConn = new NpgsqlConnection(strConnectionString);

            try
            {
                mConn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (mConn.State == ConnectionState.Open)
            {
                tempConnectTo = true;
            }
            else
            {
                tempConnectTo = false;
            }

            return tempConnectTo;
        }

        /// <summary>
        /// Get the status of the connection to the database
        /// </summary>
        /// <returns>Return a value from the Enum ConnectionState</returns>
        /// <remarks></remarks>
        public ConnectionState GetState()
        {
            return mConn.State;
        }

        /// <summary>
        /// Abort the running query
        /// </summary>
        /// <remarks></remarks>
        public void InterruptQuery()
        {
            try
            {
                oCommand.Cancel();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Execute a query
        /// </summary>
        /// <param name="sqlQuery">SQL Query string</param>
        /// <remarks></remarks>
        public void ExecuteQuery(object sqlQuery)
        {
            try
            {
                oCommand.CommandType = CommandType.Text;
                oCommand.CommandText = Convert.ToString(sqlQuery);
                oCommand.Connection = mConn;
                oCommand.ExecuteNonQuery();
                //oCommand.ExecuteReader();

                oDataAdapter = new NpgsqlDataAdapter(oCommand);
                //oDataset = new DataSet();
                //oDataAdapter.Fill(oDataset);
                oDataTable = new DataTable();
                oDataAdapter.Fill(oDataTable);
            }
            catch (System.Threading.ThreadAbortException e)
            {
                MessageBox.Show("Current operation was aborted.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (QueryCompleted != null)
                    QueryCompleted();
            }
        }

        /// <summary>
        /// Clear the internal object after executed a query
        /// </summary>
        /// <remarks></remarks>
        public void Clear()
        {
            if (oDataAdapter != null)
            {
                oDataAdapter.Dispose();
            }
            oDataAdapter = null;
            if (oDataset != null)
            {
                oDataset.Dispose();
            }
            oDataset = null;
        }

        /// <summary>
        /// Close the connection to the database
        /// </summary>
        /// <remarks></remarks>
        public void Disconnect()
        {
            try
            {
                if (mConn != null)
                {
                    if (mConn.State == ConnectionState.Open)
                    {
                        mConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        ~clsPostgreReader()
        {
            //INSTANT C# NOTE: The base class Finalize method is automatically called from the destructor:
            //base.Finalize();
        }
    }
}
