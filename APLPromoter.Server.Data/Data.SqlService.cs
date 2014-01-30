using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace APLPromoter.Server.Data {

    public class SqlService {
        private String sqlMessage;
        private String sqlProcedure;
        private Boolean sqlExecuted;
        private Boolean sqlConnected;
        private System.Data.SqlClient.SqlConnection sqlConnection;
        public Boolean SqlStatusOk { get { return sqlExecuted; } }
        public Boolean SqlConnectionOk { get { return sqlConnected; } }
        public String SqlStatusMessage { get { return sqlMessage; } }
        public String SqlProcedure { get { return sqlProcedure; } set { sqlProcedure = value; } }

        public Parameters sqlParameters = new Parameters();

        public SqlService(String ConnectionStringName) {
            sqlExecuted = false;
            System.Configuration.ConnectionStringSettingsCollection connections = System.Configuration.ConfigurationManager.ConnectionStrings;
            if (connections == null)
                sqlMessage = "APLPromoterServices.sqlService, Invalid App.config: SQL Connections section missing or invalid";
            else {
                try {
                    System.Configuration.ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName];
                    if (connectionString == null)
                        sqlMessage = "APLPromoterServices.sqlService, Invalid App.config: SQL Connection name " + ConnectionStringName + " missing or invalid";
                    else {
                        sqlConnection = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString.ToString());
                        sqlConnection.Open();
                        if (sqlConnection.State == System.Data.ConnectionState.Open) { sqlConnected = true; sqlExecuted = true; }
                        sqlMessage = "APLPromoterServices.sqlService initialized, connection status: " + sqlConnection.State.ToString();
                    }
                }
                catch (System.Configuration.ConfigurationException ex1) {
                    sqlMessage = "APLPromoterServices.sqlService, Invalid configuration, " + ex1.Source + ", " + ex1.Message;
                }
                catch (System.Data.SqlClient.SqlException ex2) {
                    sqlMessage = "APLPromoterServices.sqlService, Invalid connection, " + ex2.Source + ", " + ex2.Message;
                }
                catch (System.Exception ex3) {
                    sqlMessage = "APLPromoterServices.sqlService, " + ex3.Source + ", " + ex3.Message;
                }
            }
        }

        ~SqlService() {
            this.ExecuteCloseConnection();
        }

        public DataTable ExecuteReader() {
            sqlExecuted = false;
            DataTable sqlDataTable = null;

            if (sqlConnection.State == ConnectionState.Open) {
                try {
                    System.Data.SqlClient.SqlDataAdapter sqlAdapter = new SqlDataAdapter();
                    sqlAdapter.SelectCommand = BuildParameters(this.sqlParameters.List);
                    sqlDataTable = new System.Data.DataTable("reader");
                    if (sqlAdapter.Fill(sqlDataTable) == 0) {
                        sqlMessage = "APLPromoterServices.sqlService.ExecuteReader request returned zero records.";
                    }
                    for (int i = 0; i < this.sqlParameters.List.Length; i++) {
                        if (this.sqlParameters.List[i].dbDirection == ParameterDirection.InputOutput || this.sqlParameters.List[i].dbDirection == ParameterDirection.Output) {
                            this.sqlParameters.List[i].dbOutput = sqlAdapter.SelectCommand.Parameters[this.sqlParameters.List[i].dbName].Value.ToString();
                        }
                    }
                    sqlExecuted = true;
                }
                catch (DataException ex1) {
                    sqlMessage = "APLPromoterServices.sqlService.ExecuteReader, Invalid data adapter, " + ex1.Source + ", " + ex1.Message;
                }
                catch (System.InvalidOperationException ex2) {
                    sqlMessage = "APLPromoterServices.sqlService.ExecuteReader, Invalid data table, " + ex2.Source + ", " + ex2.Message;
                }
                catch (Exception ex3) {
                    sqlMessage = "APLPromoterServices.sqlService.ExecuteReader, " + ex3.Source + ", " + ex3.Message;
                }
            }
            return sqlDataTable;
        }

        public Boolean ExecuteNonQuery() {
            sqlExecuted = false;

            if (sqlConnection.State == ConnectionState.Open) {
                try {
                    System.Data.SqlClient.SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand = BuildParameters(this.sqlParameters.List);
                    sqlCommand.ExecuteNonQuery();
                    for (int i = 0; i < this.sqlParameters.List.Length; i++) {
                        if (this.sqlParameters.List[i].dbDirection == System.Data.ParameterDirection.InputOutput || this.sqlParameters.List[i].dbDirection == ParameterDirection.Output) {
                            this.sqlParameters.List[i].dbOutput = sqlCommand.Parameters[this.sqlParameters.List[i].dbName].Value.ToString();
                        }
                    }
                    sqlExecuted = true;
                }
                catch (System.Data.DataException ex1) {
                    sqlMessage = "APLPromoterServices.sqlService.executeNonQuery, Invalid command, " + ex1.Source + ", " + ex1.Message;
                }
                catch (Exception ex2) {
                    sqlMessage = "APLPromoterServices.sqlService.executeNonQuery, " + ex2.Source + ", " + ex2.Message;
                }
            }
            return sqlExecuted;
        }

        public Boolean ExecuteCloseConnection() {
            sqlExecuted = false;
            try {
                if (sqlConnection != null & sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                sqlExecuted = true;
            }
            catch (Exception ex2) {
                sqlMessage = "APLPromoterServices.sqlService.executeCloseConnection, " + ex2.Source + ", " + ex2.Message;
            }

            return sqlExecuted;
        }

        private SqlCommand BuildParameters(SqlServiceParameter[] Parameters) {
            SqlCommand sqlCommand = new SqlCommand(this.sqlProcedure);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.Connection = sqlConnection;
            foreach (SqlServiceParameter parameter in Parameters) {
                System.Data.SqlClient.SqlParameter param = new SqlParameter(parameter.dbName, parameter.dbType, parameter.dbSize);
                param.Direction = parameter.dbDirection;
                param.Value = parameter.dbValue;
                sqlCommand.Parameters.Add(param);
            }

            return sqlCommand;
        }

        public class Parameters {
            public SqlServiceParameter[] List;

            public SqlServiceParameter this[String index] {
                get {
                    SqlServiceParameter parameter = new SqlServiceParameter();
                    foreach(SqlServiceParameter item in this.List) {
                        if (item.dbName == index) {
                            parameter = item;
                            break;
                        }
                    }
                    return parameter;
                }
            }
        }
    }

    public enum sqlCommandType { Select, Update, Insert, Delete, Execute };
    public struct SqlServiceParameter {
        public Int32 dbSize;
        public String dbValue, dbName, dbOutput;
        public System.Data.SqlDbType dbType;
        public System.Data.ParameterDirection dbDirection;

        public SqlServiceParameter(String name, SqlDbType type, Int32 size, System.Data.ParameterDirection direction, String value) {
            dbName = name; dbType = type; dbSize = size; dbDirection = direction; dbValue = value; dbOutput = String.Empty;
        }
    }

}
