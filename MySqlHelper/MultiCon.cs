using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace MySql.MysqlHelper
{
    /// <summary>
    /// Opens and closes a connection for each query. When not doing transactions or working with memory tables
    /// </summary>
    public class MultiCon : Default
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public MultiCon(ConnectionString options)
        {
            base.SetConnectionString(options);
        }

        /// <summary>
        /// Inserts a row
        /// </summary>
        /// <param name="database">Destination database</param>
        /// <param name="table">Destination table</param>
        /// <param name="listColData">Columns and their data</param>
        /// <param name="onDupeUpdate">If duplicate, update duplicate with new values</param>
        /// <returns>Returns last insertion ID</returns>
        public override long InsertRow(string database, string table, IEnumerable<ColumnData> listColData, bool onDupeUpdate = false)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.InsertRow(mysqlCommand, database, table, listColData, onDupeUpdate);
        }

        /// <summary>
        /// Updates a row or rows
        /// </summary>
        /// <param name="database">Destination database</param>
        /// <param name="table">Destination table</param>
        /// <param name="listColData">Columns and their data</param>
        /// <param name="where">Which row(s) to update, null = all</param>
        /// <param name="limit">amount of rows to update. 0 = all</param>
        /// <returns>Returns update count</returns>
        public override long UpdateRow(string database, string table, IEnumerable<ColumnData> listColData, string where = null, int limit = 0)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.UpdateRow(mysqlCommand, database, table, listColData, where, limit);
        }

        /// <summary>
        /// Sends entire datatable to database. Name of column in datatable should equal name of column in database table
        /// </summary>
        public override void BulkSend(string database, string table, string column, IEnumerable<object> listData)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                base.BulkSend(mysqlCommand, database, table, column, listData);
        }

        /// <summary>
        /// Sends an entire datatable to specified table. Make sure that column names of table correspond to database
        /// </summary>
        public override void BulkSend(string database, string table, DataTable dataTable, int updateBatchSize=100)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                base.BulkSend(mysqlCommand, database, table, dataTable, updateBatchSize);
        }

        /// <summary>
        /// Returns a list containing the first column
        /// </summary>
        public override IEnumerable<T> GetFirst<T>(string query)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.GetFirst<T>(mysqlCommand, query);
        }

        /// <summary>
        /// Returns a field from the server as a object
        /// </summary>
        public override object GetObject(string query)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.GetObject(mysqlCommand, query);
        }

        /// <summary>
        /// Returns a field from the server as specified type 
        /// </summary>
        public T GetObject<T>(string query)
        {
            return (T)GetObject(query);
        }

        /// <summary>
        /// Sends query to server
        /// </summary>
        public override int SendQuery(string query)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
            using (MySqlCommand mysqlCommand = mysqlConnection.CreateCommand())
                return base.SendQuery(mysqlCommand, query);
        }

        /// <summary>
        /// Returns all selected data as a datatable
        /// </summary>
        public override DataTable GetDataTable(string query)
        {
            using (MySqlConnection mysqlConnection = GetMysqlConnection())
                return base.GetDataTable(mysqlConnection, query);
        }

        /// <summary>
        /// Returns the default connecition data
        /// </summary>
        private MySqlConnection GetMysqlConnection()
        {
            MySqlConnection mysqlConnection = new MySqlConnection(base.connectionString);
            if (!base.OpenConnection(mysqlConnection, 10)) throw new Exception("Unable to connect");
            return mysqlConnection;
        }
    }
}