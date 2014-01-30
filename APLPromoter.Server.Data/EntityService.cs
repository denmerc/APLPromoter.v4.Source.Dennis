using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APLPromoter.Server.Entity;
using System.Data;

namespace APLPromoter.Server.Data
{
    public class EntityService
    {

        #region Constants...
        const int unassigned = 0;
        const String invalid = "Invalid:";
        #endregion

        public EntityService()
        {
        }

        #region Session initialize...
        public SqlServiceParameter[] SessionInitMapParameters(ref UserSession session)
        {

            session.sqlCommand = EntityMap.userInitCommand;
            session.sqlMessage = EntityMap.userInitMessage;
            SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(EntityMap.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, session.sqlMessage)
            };

            return parameters;
        }
        public Boolean SessionInitMapData(SqlService service, System.Data.DataTable data, ref UserSession session)
        {

            Boolean mapStatus = false;
            String commandMessage = service.sqlParameters[(int)EntityMap.userInitParameters.sqlMessage].dbOutput;
            if (commandMessage.Contains(invalid))
                session.clientMessage = commandMessage;
            else
            {
                System.Data.DataTableReader reader = data.CreateDataReader();
                try
                {
                    if (reader.Read())
                    { //Session reader returns a single record...
                        session.appOnline = Boolean.Parse(reader[EntityMap.userInitDataOnline].ToString());
                        session.sqlAuthorization = Boolean.Parse(reader[EntityMap.userInitDataSqlAuthentication].ToString());
                        session.winAuthorization = Boolean.Parse(reader[EntityMap.userInitDataWinAuthentication].ToString());
                        session.clientMessage = (session.appOnline) ? EntityMap.userInitMessageAuthenticated : reader[EntityMap.userInitDataMessageOnline].ToString();
                        mapStatus = true;
                    }
                }
                catch (System.Exception ex) { session.clientMessage = ex.Message; }
            }
            return mapStatus;
        }
        #endregion

        #region Session authenticate...
        public SqlServiceParameter[] SessionAuthenticateMapParameters(ref UserSession session)
        {

            session.sqlCommand = EntityMap.userAuthenticateCommand;
            session.sqlMessage = (session.sqlAuthorization) ? EntityMap.userAuthenticateSqlMessage : EntityMap.userAuthenticateWinMessage;
            SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(EntityMap.userLogin, SqlDbType.VarChar, 100, ParameterDirection.Input, session.login),
                new SqlServiceParameter(EntityMap.userPassword, SqlDbType.VarChar, 100, ParameterDirection.Input, session.password),
                new SqlServiceParameter(EntityMap.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, session.sqlMessage)
            };

            return parameters;
        }
        public Boolean SessionAuthenticateMapData(SqlService service, System.Data.DataTable data, ref UserSession session)
        {

            Boolean mapStatus = false;
            String commandMessage = service.sqlParameters[(int)EntityMap.userAuthenticateParameters.sqlMessage].dbOutput;
            if (commandMessage.Contains(invalid))
                session.clientMessage = commandMessage;
            else
            {
                System.Data.DataTableReader reader = data.CreateDataReader();
                try
                {
                    if (reader.Read())
                    { //Session reader returns a single record...
                        session.sqlKey = reader[EntityMap.userAuthenticateDataSession].ToString();
                        session.clientMessage = reader[EntityMap.userAuthenticateDataMessage].ToString();
                        session.authenticated = Boolean.Parse(reader[EntityMap.userAuthenticateDataAuthenticated].ToString());
                        mapStatus = true;
                    }
                }
                catch (System.Exception ex) { session.clientMessage = ex.Message; }
            }
            return mapStatus;
        }
        #endregion

        #region User load...
        public SqlServiceParameter[] UserLoadMapParameters(ref User.Identity data)
        {

            data.SqlCommand = EntityMap.userLoadCommand;
            data.SqlMessage = (data.Id == unassigned && data.DataOk== false) ? EntityMap.userAuthenticatedMessage : EntityMap.userLoadMessage;
            SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(EntityMap.userId, SqlDbType.Int, 0, ParameterDirection.Input, data.Id.ToString()),
                new SqlServiceParameter(EntityMap.userSession, SqlDbType.VarChar, 50, ParameterDirection.Input, data.SqlKey),
                new SqlServiceParameter(EntityMap.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, data.SqlMessage)
            };

            return parameters;
        }
        public Boolean UserLoadMapData(SqlService service, System.Data.DataTable dataTable, ref User.Identity data)
        {

            Boolean mapStatus = false;
            String commandMessage = service.sqlParameters[(int)EntityMap.userLoadParameters.sqlMessage].dbOutput;
            if (commandMessage.Contains(invalid))
                data.ClientMessage = commandMessage;
            else
            {
                System.Data.DataTableReader reader = dataTable.CreateDataReader();
                try
                {
                    if (reader.Read())
                    { //Load reader returns a single record...
                        data.dataView = dataTable;
                        data.Id = Int32.Parse(reader[EntityMap.userId].ToString());
                        data.Type.Id = Int32.Parse(reader[EntityMap.userType].ToString());
                        data.Type.Name = reader[EntityMap.userTypeName].ToString();
                        data.Type.Text = reader[EntityMap.userTypeText].ToString();
                        data.Login = reader[EntityMap.userLogin].ToString();
                        data.FirstName = reader[EntityMap.userFirstName].ToString();
                        data.LastName = reader[EntityMap.userLastName].ToString();
                        data.Email = reader[EntityMap.userEmail].ToString();
                        data.IsActive = Boolean.Parse(reader[EntityMap.userIsActive].ToString());
                        data.Created = DateTime.Parse(reader[EntityMap.userCreated].ToString());
                        data.CreatedText = reader[EntityMap.userCreatedText].ToString();
                        data.Edited = DateTime.Parse(reader[EntityMap.userEdited].ToString());
                        data.EditedText = reader[EntityMap.userEditedText].ToString();
                        data.LastLogin = DateTime.Parse(reader[EntityMap.userLastLogin].ToString());
                        data.LastLoginText = reader[EntityMap.userLastLoginText].ToString();
                        mapStatus = true;
                    }
                }
                catch (System.Exception ex) { data.ClientMessage = ex.Message; }
            }
            return mapStatus;
        }
        #endregion

        #region User save...
        public SqlServiceParameter[] UserSaveMapParameters(ref User.Identity data)
        {

            data.SqlCommand = EntityMap.userSaveCommand;
            data.SqlMessage = (data.Id == unassigned && data.DataOk == false) ? EntityMap.userAddMessage : EntityMap.userSaveMessage;
            SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(EntityMap.userId, SqlDbType.Int, 0, ParameterDirection.Input, data.Id.ToString()),
                new SqlServiceParameter(EntityMap.userType, SqlDbType.Int, 0, ParameterDirection.Input, data.Id.ToString()),
                new SqlServiceParameter(EntityMap.userEmail, SqlDbType.VarChar, 100, ParameterDirection.Input, data.Email),
                new SqlServiceParameter(EntityMap.userFirstName, SqlDbType.VarChar, 100, ParameterDirection.Input, data.FirstName),
                new SqlServiceParameter(EntityMap.userLastName, SqlDbType.VarChar, 100, ParameterDirection.Input, data.LastName),
                new SqlServiceParameter(EntityMap.userSession, SqlDbType.VarChar, 50, ParameterDirection.Input, data.SqlKey),
                new SqlServiceParameter(EntityMap.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, data.SqlMessage)
            };

            return parameters;
        }
        #endregion

        #region Load view...
        public SqlServiceParameter[] LoadViewMapParameters(ref UserSession session)
        {

            switch (session.modelView)
            {
                case ModelView.xmlTreeViewPlanning:
                    session.sqlCommand = EntityMap.userViewCommand;
                    session.sqlMessage = EntityMap.userViewXmlPlanningMessage;
                    break;
                case ModelView.datasetAdminUsers:
                    session.sqlCommand = EntityMap.userViewCommand;
                    session.sqlMessage = EntityMap.userViewDatasetAdminUsersMessage;
                    break;
                case ModelView.datalistUserTypes:
                    session.sqlCommand = EntityMap.enumListCommand;
                    session.sqlMessage = EntityMap.enumListUserTypesMessage;
                    break;
            }
            SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(EntityMap.userSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.sqlKey),
                new SqlServiceParameter(EntityMap.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, session.sqlMessage)
            };

            return parameters;
        }
        public Boolean LoadViewMapData(SqlService service, System.Data.DataTable dataTable, ref UserSession session)
        {

            Boolean mapStatus = false;
            String commandMessage = String.Empty;
            System.Text.StringBuilder xmlStream = new System.Text.StringBuilder();
            switch (session.modelView)
            {
                case ModelView.xmlTreeViewPlanning:
                    commandMessage = service.sqlParameters[(int)EntityMap.userViewParameters.sqlMessage].dbOutput;
                    break;
                case ModelView.datasetAdminUsers:
                    commandMessage = service.sqlParameters[(int)EntityMap.userViewParameters.sqlMessage].dbOutput;
                    break;
                case ModelView.datalistUserTypes:
                    commandMessage = service.sqlParameters[(int)EntityMap.enumViewParameters.sqlMessage].dbOutput;
                    break;
            }

            if (commandMessage.Contains(invalid))
                session.clientMessage = commandMessage;
            else
            {
                System.Data.DataTableReader reader = dataTable.CreateDataReader();
                try
                {
                    switch (session.modelView)
                    {
                        case ModelView.xmlTreeViewPlanning:
                            while (reader.Read()) xmlStream.Append(reader[(int)EntityMap.userViewColumns.xml].ToString());
                            session.xmlView = xmlStream.ToString();
                            mapStatus = true;
                            break;
                        case ModelView.datasetAdminUsers:
                            session.dataView = dataTable;
                            mapStatus = true;
                            break;
                        case ModelView.datalistUserTypes:
                            session.dataView = dataTable;
                            mapStatus = true;
                            break;
                    }
                }
                catch (System.Exception ex) { session.clientMessage = ex.Message; }
            }
            return mapStatus;
        }
        #endregion

    }

    public static class EntityMap
    {
        #region Enumeration map...
        //Enumerations...
        public const String sqlMessage = "message";
        public const String enumListCommand = "dbo.sysEnumerationSelect";
        public const String enumListUserTypesMessage = "selectAplUserType";
        public enum enumViewParameters { session = 0, sqlMessage };
        #endregion

        #region User map...
        //User initialization...
        public const String userInitMessage = "selectSystemDefaults";
        public const String userInitCommand = "dbo.sysEnumerationSelect";
        public const String userInitDataOnline = "appOnline";
        public const String userInitDataMessageOnline = "messageOnline";
        public const String userInitDataSqlAuthentication = "sqlAuthentication";
        public const String userInitDataWinAuthentication = "winAuthentication";
        public const String userInitMessageAuthenticated = "User request not authenticated";
        public enum userInitParameters { sqlMessage = 0 };
        //User authentication...
        public const String userAuthenticatedMessage = "selectAuthenticatedUser";
        public const String userAuthenticateWinMessage = "authenticateWinUser";
        public const String userAuthenticateSqlMessage = "authenticateSqlUser";
        public const String userAuthenticateCommand = "dbo.aplUserSelect";
        public const String userAuthenticateDataSession = "session";
        public const String userAuthenticateDataAuthenticated = "authenticated";
        public const String userAuthenticateDataMessage = "message";
        public enum userAuthenticateParameters { login = 0, password, sqlMessage };
        //User data...
        public const String userAddMessage = "insertUser";
        public const String userSaveMessage = "updateUser";
        public const String userLoadMessage = "selectDataSetUser";
        public const String userLoadCommand = "dbo.aplUserSelect";
        public const String userSaveCommand = "dbo.aplUserUpdate";
        public enum userLoadParameters { id = 0, session, sqlMessage };
        public enum userSaveParameters { id = 0, email, lastname, firstname, session, sqlMessage };
        //User views...
        public const String userViewCommand = "dbo.aplUserSelect";
        public const String userViewXmlPlanningMessage = "selectXmlAutoExplorer";
        public const String userViewDatasetAdminUsersMessage = "selectDataSetUsers";
        public enum userViewParameters { session = 0, sqlMessage };
        public enum userViewColumns { xml = 0 };
        //User fields...
        public const String userId = "identyId";
        public const String userIsActive = "active";
        public const String userName = "username";
        public const String userFirstName = "firstname";
        public const String userLastName = "lastname";
        public const String userLogin = "login";
        public const String userEmail = "email";
        public const String userMessage = "message";
        public const String userPassword = "password";
        public const String userOldPassword = "oldpassword";
        public const String userPost = "post";
        public const String userSession = "session";
        public const String userType = "type";
        public const String userTypeName = "typename";
        public const String userTypeText = "typetext";
        public const String userCreated = "created";
        public const String userCreatedText = "createdtext";
        public const String userEdited = "edited";
        public const String userEditedText = "editedtext";
        public const String userLastEditor = "lastEditor";
        public const String userLastLogin = "lastlogin";
        public const String userLastLoginText = "lastlogintext";
        #endregion
    }
}
