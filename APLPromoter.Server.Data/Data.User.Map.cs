using System;
using System.Data;
using System.Collections.Generic;
using APLPromoter.Server.Entity;

namespace APLPromoter.Server.Data {

    class UserMap {

        #region Initialize...
        public void InitializeMapParameters(Session<Server.Entity.NullT> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = UserMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //Shared client key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, UserMap.Names.initializeMessage)
            }; service.sqlParameters.List = parameters;

        }

        public Server.Entity.Session<NullT> InitializeMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            Server.Entity.Session<NullT> init = new Session<NullT>();
            //Single record...
            if (reader.Read()) {
                init.SessionOk = false;
                init.ClientMessage = String.Empty;
                init.ServerMessage = String.Empty;
                init.SqlKey = reader[UserMap.Names.privateKey].ToString();
                init.AppOnline = Boolean.Parse(reader[UserMap.Names.appOnline].ToString());
                init.SqlAuthorization = Boolean.Parse(reader[UserMap.Names.sqlAuthorization].ToString());
                init.WinAuthorization = Boolean.Parse(reader[UserMap.Names.winAuthorization].ToString());
            }

            return init;
        }

        #endregion

        #region Authenticate...
        public void AuthenticateMapParameters(Session<Server.Entity.NullT> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = UserMap.Names.selectCommand;
            String authenticateMessage = (session.SqlAuthorization) ? UserMap.Names.authenticateSqlUserMessage : ( (session.WinAuthorization) ? UserMap.Names.authenticateWinUserMessage : String.Empty );

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.login, SqlDbType.VarChar, 100, ParameterDirection.Input, session.UserIdentity.Login),
                new SqlServiceParameter(UserMap.Names.password, SqlDbType.VarChar, 100, ParameterDirection.Input, session.UserIdentity.Password.Old),
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //Tenant private client key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, authenticateMessage)
            }; service.sqlParameters.List = parameters;

        }

        public Server.Entity.User.Identity AuthenticateMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            Server.Entity.User.Identity user = null;
            //Record set...
            if (reader.Read()) {
                user = new User.Identity(
                    Int32.Parse(reader[UserMap.Names.id].ToString()),
                    reader[UserMap.Names.sqlSession].ToString(),
                    reader[UserMap.Names.login].ToString(),
                    Boolean.Parse(reader[UserMap.Names.active].ToString()),
                    reader[UserMap.Names.email].ToString(),
                    reader[UserMap.Names.userName].ToString(),
                    reader[UserMap.Names.firstName].ToString(),
                    reader[UserMap.Names.lastName].ToString(),
                    DateTime.Parse(reader[UserMap.Names.lastLogin].ToString()),
                    reader[UserMap.Names.lastLoginText].ToString(),
                    DateTime.Parse(reader[UserMap.Names.created].ToString()),
                    reader[UserMap.Names.createdText].ToString(),
                    DateTime.Parse(reader[UserMap.Names.edited].ToString()),
                    reader[UserMap.Names.editedText].ToString(),
                    reader[UserMap.Names.editor].ToString(),
                    new User.Role(
                        Int32.Parse(reader[UserMap.Names.roleId].ToString()),
                        reader[UserMap.Names.roleName].ToString(),
                        reader[UserMap.Names.roleDescription].ToString()
                    )
                );
            }
            return user;
        }
        #endregion

        #region Load Identity...
        public void LoadIdentityMapParameters(Session<Server.Entity.User.Identity> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = UserMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.Data.Id.ToString()),
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //session key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, UserMap.Names.loadIdentityMessage)
            }; service.sqlParameters.List = parameters;
        }

        public Server.Entity.User.Identity LoadIdentityMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            Server.Entity.User.Identity identity = null;
            //Single record...
            if (reader.Read()) {
                identity = new User.Identity(
                    Int32.Parse(reader[UserMap.Names.id].ToString()),
                    UserMap.Names.sharedKey.ToString(),
                    reader[UserMap.Names.login].ToString(),
                    Boolean.Parse(reader[UserMap.Names.active].ToString()),
                    reader[UserMap.Names.email].ToString(),
                    reader[UserMap.Names.userName].ToString(),
                    reader[UserMap.Names.firstName].ToString(),
                    reader[UserMap.Names.lastName].ToString(),
                    DateTime.Parse(reader[UserMap.Names.lastLogin].ToString()),
                    reader[UserMap.Names.lastLoginText].ToString(),
                    DateTime.Parse(reader[UserMap.Names.created].ToString()),
                    reader[UserMap.Names.createdText].ToString(),
                    DateTime.Parse(reader[UserMap.Names.edited].ToString()),
                    reader[UserMap.Names.editedText].ToString(),
                    reader[UserMap.Names.editor].ToString(),
                    new User.Role(
                        Int32.Parse(reader[UserMap.Names.roleId].ToString()),
                        reader[UserMap.Names.roleName].ToString(),
                        reader[UserMap.Names.roleDescription].ToString()
                    )
                );
            }
            return identity;
        }
        #endregion

        #region Load List...
        public void LoadListMapParameters(Session<List<Server.Entity.User.Identity>> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = UserMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //session key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, UserMap.Names.loadIdentitiesMessage)
            }; service.sqlParameters.List = parameters;
        }

        public List<Server.Entity.User.Identity> LoadListMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            List<Server.Entity.User.Identity> list = new List<User.Identity>(data.Rows.Count);
            //Record set...
            while (reader.Read()) {
                list.Add(new User.Identity(
                    Int32.Parse(reader[UserMap.Names.id].ToString()),
                    UserMap.Names.sharedKey.ToString(),
                    reader[UserMap.Names.login].ToString(),
                    Boolean.Parse(reader[UserMap.Names.active].ToString()),
                    reader[UserMap.Names.email].ToString(),
                    reader[UserMap.Names.userName].ToString(),
                    reader[UserMap.Names.firstName].ToString(),
                    reader[UserMap.Names.lastName].ToString(),
                    DateTime.Parse(reader[UserMap.Names.lastLogin].ToString()),
                    reader[UserMap.Names.lastLoginText].ToString(),
                    DateTime.Parse(reader[UserMap.Names.created].ToString()),
                    reader[UserMap.Names.createdText].ToString(),
                    DateTime.Parse(reader[UserMap.Names.edited].ToString()),
                    reader[UserMap.Names.editedText].ToString(),
                    reader[UserMap.Names.editor].ToString(),
                    new User.Role(
                        Int32.Parse(reader[UserMap.Names.roleId].ToString()),
                        reader[UserMap.Names.roleName].ToString(),
                        reader[UserMap.Names.roleDescription].ToString()
                    )
                ));
            }
            return list;
        }
       #endregion

        #region Load Explorer Planning...
        public void LoadExplorerPlanningMapParameters(Session<Server.Entity.NullT> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = UserMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.UserIdentity.Id.ToString()),
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //session key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, UserMap.Names.loadExplorerPlanningMessage)
            }; service.sqlParameters.List = parameters;
        }

        public Server.Entity.User.Identity LoadExplorerPlanningMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            System.Text.StringBuilder xmlStream = new System.Text.StringBuilder();
            Server.Entity.User.Identity identity = new User.Identity { Role = new User.Role() };
            //Single record, chunk reader...
            while (reader.Read()) xmlStream.Append(reader[UserMap.Names.xmlDataColumn].ToString());
            identity.Role.Planning = xmlStream.ToString();

            return identity;
        }
        #endregion

        #region Load Explorer Tracking...
        public void LoadExplorerTrackingMapParameters(Session<Server.Entity.NullT> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = UserMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.UserIdentity.Id.ToString()),
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //session key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, UserMap.Names.loadExplorerTrackingMessage)
            }; service.sqlParameters.List = parameters;
        }

        public Server.Entity.User.Identity LoadExplorerTrackingMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            Server.Entity.User.Identity identity = new User.Identity();
            //Single record...
            if (reader.Read()) {
                identity.Role.Tracking = reader[UserMap.Names.xmlDataColumn].ToString();
            }
            return identity;
        }
        #endregion

        #region Load Explorer Reporting...
        public void LoadExplorerReportingMapParameters(Session<Server.Entity.NullT> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = UserMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.UserIdentity.Id.ToString()),
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //session key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, UserMap.Names.loadExplorerReportingMessage)
            }; service.sqlParameters.List = parameters;
        }

        public Server.Entity.User.Identity LoadExplorerReportingMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            Server.Entity.User.Identity identity = new User.Identity();
            //Single record...
            if (reader.Read()) {
                identity.Role.Reporting = reader[UserMap.Names.xmlDataColumn].ToString();
            }
            return identity;
        }
        #endregion

        #region Save Password...
        public void SavePasswordMapParameters(Session<Server.Entity.NullT> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = UserMap.Names.updateCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.login, SqlDbType.VarChar, 100, ParameterDirection.Input, session.UserIdentity.Login),
                new SqlServiceParameter(UserMap.Names.password, SqlDbType.VarChar, 100, ParameterDirection.Input, session.UserIdentity.Password.New),
                new SqlServiceParameter(UserMap.Names.oldPassword, SqlDbType.VarChar, 100, ParameterDirection.Input, session.UserIdentity.Password.Old),
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //user session key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, UserMap.Names.savePasswordMessage)
            }; service.sqlParameters.List = parameters;
        }
        #endregion

        #region Save Identity...
        public void SaveIdentityMapParameters(Session<Server.Entity.User.Identity> session, ref Server.Data.SqlService service) {

            //Map the command...
            Int16 insertId = 0;
            service.SqlProcedure = UserMap.Names.updateCommand;
            String updateCommandMessage = (session.Data.Id == insertId) ? UserMap.Names.saveIdentityInsertMessage : UserMap.Names.saveIdentityUpdateMessage;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(UserMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.Data.Id.ToString()),
                new SqlServiceParameter(UserMap.Names.roleId, SqlDbType.Int, 0, ParameterDirection.Input, session.Data.Role.Id.ToString()),
                new SqlServiceParameter(UserMap.Names.login, SqlDbType.VarChar, 100, ParameterDirection.Input, session.Data.Login),
                new SqlServiceParameter(UserMap.Names.firstName, SqlDbType.VarChar, 100, ParameterDirection.Input, session.Data.FirstName),
                new SqlServiceParameter(UserMap.Names.lastName, SqlDbType.VarChar, 100, ParameterDirection.Input, session.Data.LastName),
                new SqlServiceParameter(UserMap.Names.email, SqlDbType.VarChar, 100, ParameterDirection.Input, session.Data.Email),
                new SqlServiceParameter(UserMap.Names.password, SqlDbType.VarChar, 100, ParameterDirection.Input, session.Data.Password.New),
                new SqlServiceParameter(UserMap.Names.oldPassword, SqlDbType.VarChar, 100, ParameterDirection.Input, session.Data.Password.Old),
                new SqlServiceParameter(UserMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey), //user session key
                new SqlServiceParameter(UserMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, updateCommandMessage)
            }; service.sqlParameters.List = parameters;
        }
        
        public Server.Entity.User.Identity SaveIdentityMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            Server.Entity.User.Identity identity = null;
            //Single record...
            if (reader.Read()) {
                identity = new User.Identity(
                    Int32.Parse(reader[UserMap.Names.id].ToString()),
                    UserMap.Names.sharedKey.ToString(),
                    reader[UserMap.Names.login].ToString(),
                    Boolean.Parse(reader[UserMap.Names.active].ToString()),
                    reader[UserMap.Names.email].ToString(),
                    reader[UserMap.Names.userName].ToString(),
                    reader[UserMap.Names.firstName].ToString(),
                    reader[UserMap.Names.lastName].ToString(),
                    DateTime.Parse(reader[UserMap.Names.lastLogin].ToString()),
                    reader[UserMap.Names.lastLoginText].ToString(),
                    DateTime.Parse(reader[UserMap.Names.created].ToString()),
                    reader[UserMap.Names.createdText].ToString(),
                    DateTime.Parse(reader[UserMap.Names.edited].ToString()),
                    reader[UserMap.Names.editedText].ToString(),
                    reader[UserMap.Names.editor].ToString(),
                    new User.Role(
                        Int32.Parse(reader[UserMap.Names.roleId].ToString()),
                        reader[UserMap.Names.roleName].ToString(),
                        reader[UserMap.Names.roleDescription].ToString()
                    ));
            }
            return identity;
        }
        #endregion

        #region Entity map...
        //Database names...
        public class Names {
            //Select commands...
            public const String selectCommand = "dbo.aplUserSelect";
            public const String initializeMessage = "selectInitialize";
            public const String loadIdentityMessage = "selectIdentity";
            public const String loadIdentitiesMessage = "selectIdentities";
            public const String loadAuthenticatedUserMessage = "selectUser";
            public const String loadExplorerPlanningMessage = "selectXmlExplorerPlanning";
            public const String loadExplorerTrackingMessage = "selectXmlExplorerTracking";
            public const String loadExplorerReportingMessage = "selectXmlExplorerReporting";
            public const String authenticateSqlUserMessage = "selectSqlUser";
            public const String authenticateWinUserMessage = "selectWinUser";

            //Update commands...
            public const String updateCommand = "dbo.aplUserUpdate";
            public const String savePasswordMessage = "updatePassword";
            public const String saveIdentityInsertMessage = "insertIdentity";
            public const String saveIdentityUpdateMessage = "updateIdentity";

            //Default parameters...
            public const String id = "id";
            public const String sqlSession = "session";
            public const String sqlMessage = "message";

            #region Fields Identity...
            public const String userId = "id";
            public const String login = "login";
            public const String active = "active";
            public const String password = "password";
            public const String oldPassword = "passwordold";
            public const String roleId = "role";
            public const String roleName = "roleName";
            public const String roleDescription = "roleText";
            public const String email = "email";
            public const String userName = "username";
            public const String firstName = "firstname";
            public const String lastName = "lastname";
            public const String lastLoginText = "lastLoginText";
            public const String createdText = "createdText";
            public const String editedText = "editedText";
            public const String lastLogin = "lastLogin";
            public const String created = "created";
            public const String edited = "edited";
            public const String editor = "lastEditor";
            public const String xmlPlanning = "xmlPlanning";
            public const String xmlTracking = "xmlTracking";
            public const String xmlReporting = "xmlReporting";
            public const Int32 xmlDataColumn = 0;
            #endregion

            #region Fields Session...
            //public const String sqlKey = "sqlKey";
            public const String appOnline = "appOnline";
            public const String privateKey = "privateKey";
            public const String sqlAuthorization = "sqlAuthorization";
            public const String winAuthorization = "winAuthorization";
            public const String sharedKey = "72B9ED08-5D12-48FD-9CF7-56A3CA30E660";
            #endregion
        }
        #endregion

        #region Message map result sets...
        //selectInitialize - sqlAuthorization,winAuthorization,appOnline,privateKey
        //selectSqlUser - id,session,role,roleName,roleText,login,firstname,lastname,username,email,active,created,createdText,edited,editedText,lastLogin,lastLoginText,lastEditor 
        //selectWinUser - id,session,role,roleName,roleText,login,firstname,lastname,username,email,active,created,createdText,edited,editedText,lastLogin,lastLoginText,lastEditor 
        //selectAuthenticatedUser - id,session,role,roleName,roleText,login,firstname,lastname,username,email,active,created,createdText,edited,editedText,lastLogin,lastLoginText,lastEditor 
        //selectIdentity - id,role,roleName,roleText,login,firstname,lastname,username,email,active,created,createdText,edited,editedText,lastLogin,lastLoginText,lastEditor 
        //selectIdentities - id,role,roleName,roleText,login,firstname,lastname,username,email,active,created,createdText,edited,editedText,lastLogin,lastLoginText,lastEditor 
        //updateIdentity - id,role,roleName,roleText,login,firstname,lastname,username,email,active,created,createdText,edited,editedText,lastLogin,lastLoginText,lastEditor 
        #endregion

    }
}
