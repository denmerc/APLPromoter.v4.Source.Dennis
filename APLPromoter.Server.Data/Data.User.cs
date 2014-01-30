using System;
using System.Collections.Generic;
using APLPromoter.Server.Entity;

namespace APLPromoter.Server.Data {

    public interface IUserData {
        Session<Server.Entity.NullT> Initialize(Session<Server.Entity.NullT> session);
        Session<Server.Entity.NullT> Authenticate(Session<Server.Entity.NullT> session);
        Session<Server.Entity.NullT> LoadExplorerPlanning(Session<Server.Entity.NullT> session);
        Session<Server.Entity.NullT> LoadExplorerTracking(Session<Server.Entity.NullT> session);
        Session<Server.Entity.NullT> LoadExplorerReporting(Session<Server.Entity.NullT> session);
        Session<List<Server.Entity.User.Identity>> LoadList(Session<Server.Entity.NullT> session);
        Session<Server.Entity.User.Identity> LoadIdentity(Session<Server.Entity.User.Identity> session);
        Session<Server.Entity.User.Identity> SaveIdentity(Session<Server.Entity.User.Identity> session);
        Session<Server.Entity.NullT> SavePassword(Session<Server.Entity.NullT> session);
    }

    public class UserData : IUserData  {

        #region Constants...
        const String invalid = "Invalid:";
        const String connectionName = "defaultConnectionString";
        const String aplServiceEventLog = "APLPromoterServerData";
        #endregion

        #region Variables...
        private System.Diagnostics.EventLog localServiceLog;
        private APLPromoter.Server.Data.UserMap sqlMapper;
        private APLPromoter.Server.Data.SqlService sqlService;

        #endregion

        private String sqlConnection {
            get {
                return System.Configuration.ConfigurationManager.AppSettings[connectionName];
            }
        }

        public UserData() {

            sqlMapper = new UserMap();            
            sqlService = new SqlService(this.sqlConnection);
            localServiceLog = new System.Diagnostics.EventLog();
            //if (!System.Diagnostics.EventLog.SourceExists(cseServiceEventLog)) EventLog.CreateEventSource(cseServiceEventLog, "Application");
            //Setup <cseServiceEventLog> event source manually through registry key: HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Eventlog\Application
            //To resolve message IDs create a RG_EXPAND_SZ attribute, named "EventMessageFile" to: "C:\WINDOWS\Microsoft.NET\Framework\<current version>\EventLogMessages.dll"
            localServiceLog.Source = aplServiceEventLog;

        }

        ~UserData() {
            if (sqlService != null && sqlService.SqlConnectionOk) sqlService.ExecuteCloseConnection();
        }


        public Session<Server.Entity.NullT> Initialize(Session<Server.Entity.NullT> sessionIn) {

            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<Server.Entity.NullT> sessionOut = new Session<Server.Entity.NullT> {
                SqlKey = Server.Data.UserMap.Names.sharedKey
            };

            try {
                sqlMapper.InitializeMapParameters(sessionOut, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut = sqlMapper.InitializeMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionIn.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }

            return sessionOut;
        }

        public Session<Server.Entity.NullT> Authenticate(Session<Server.Entity.NullT> sessionIn) {

            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<Server.Entity.NullT> sessionOut = new Session<Server.Entity.NullT> {
                SessionOk = false,
                ClientMessage = String.Empty,
                ServerMessage = String.Empty,
                UserIdentity = sessionIn.UserIdentity,
                AppOnline = sessionIn.AppOnline,
                Authenticated = sessionIn.Authenticated,
                SqlAuthorization = sessionIn.SqlAuthorization,
                WinAuthorization = sessionIn.WinAuthorization,
                SqlKey = sessionIn.SqlKey
            };

            try {
                sqlMapper.AuthenticateMapParameters(sessionOut, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.UserIdentity = sqlMapper.AuthenticateMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                        sessionOut.Authenticated = sessionOut.SessionOk;
                        sessionOut.SqlKey = sessionOut.UserIdentity.sqlKey;
                        if (sessionOut.SessionOk) sessionOut = LoadExplorerPlanning(sessionOut); 
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionIn.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }

            return sessionOut;
        }

        public Session<Server.Entity.User.Identity> LoadIdentity(Session<Server.Entity.User.Identity> sessionIn) {
            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<Server.Entity.User.Identity> sessionOut = new Session<Server.Entity.User.Identity> {
                SessionOk = false,
                ClientMessage = String.Empty,
                ServerMessage = String.Empty,
                UserIdentity = sessionIn.UserIdentity,
                AppOnline = sessionIn.AppOnline,
                Authenticated = sessionIn.Authenticated,
                SqlAuthorization = sessionIn.SqlAuthorization,
                WinAuthorization = sessionIn.WinAuthorization,
                SqlKey = sessionIn.SqlKey,
                Data = sessionIn.Data
            };

            try {
                sqlMapper.LoadIdentityMapParameters(sessionOut, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data = sqlMapper.LoadIdentityMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionIn.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }

            return sessionOut;
        }

        public Session<List<Server.Entity.User.Identity>> LoadList(Session<Server.Entity.NullT> sessionIn) {
            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<List<Server.Entity.User.Identity>> sessionOut = new Session<List<Server.Entity.User.Identity>> {
                SessionOk = false,
                ClientMessage = String.Empty,
                ServerMessage = String.Empty,
                UserIdentity = sessionIn.UserIdentity,
                AppOnline = sessionIn.AppOnline,
                Authenticated = sessionIn.Authenticated,
                SqlAuthorization = sessionIn.SqlAuthorization,
                WinAuthorization = sessionIn.WinAuthorization,
                SqlKey = sessionIn.SqlKey
            };

            try {
                sqlMapper.LoadListMapParameters(sessionOut, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data = sqlMapper.LoadListMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionIn.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }

            return sessionOut;
        }

        public Session<Server.Entity.NullT> LoadExplorerPlanning(Session<Server.Entity.NullT> sessionIn) {
            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<Server.Entity.NullT> sessionOut = new Session<Server.Entity.NullT> {
                SessionOk = false,
                ClientMessage = String.Empty,
                ServerMessage = String.Empty,
                UserIdentity = sessionIn.UserIdentity,
                AppOnline = sessionIn.AppOnline,
                Authenticated = sessionIn.Authenticated,
                SqlAuthorization = sessionIn.SqlAuthorization,
                WinAuthorization = sessionIn.WinAuthorization,
                SqlKey = sessionIn.SqlKey,
            };

            try {
                sqlMapper.LoadExplorerPlanningMapParameters(sessionOut, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.UserIdentity.Role.Planning = sqlMapper.LoadExplorerPlanningMapData(dataTable, sqlService).Role.Planning;
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionIn.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }

            return sessionOut;
        }

        public Session<Server.Entity.NullT> LoadExplorerTracking(Session<Server.Entity.NullT> sessionIn) {
            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<Server.Entity.NullT> sessionOut = new Session<Server.Entity.NullT> {
                SessionOk = false,
                ClientMessage = String.Empty,
                ServerMessage = String.Empty,
                UserIdentity = sessionIn.UserIdentity,
                AppOnline = sessionIn.AppOnline,
                Authenticated = sessionIn.Authenticated,
                SqlAuthorization = sessionIn.SqlAuthorization,
                WinAuthorization = sessionIn.WinAuthorization,
                SqlKey = sessionIn.SqlKey,
            };

            try {
                sqlMapper.LoadExplorerTrackingMapParameters(sessionOut, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.UserIdentity.Role.Tracking = sqlMapper.LoadExplorerTrackingMapData(dataTable, sqlService).Role.Tracking;
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionIn.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }

            return sessionOut;
        }

        public Session<Server.Entity.NullT> LoadExplorerReporting(Session<Server.Entity.NullT> sessionIn) {
            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<Server.Entity.NullT> sessionOut = new Session<Server.Entity.NullT> {
                SessionOk = false,
                ClientMessage = String.Empty,
                ServerMessage = String.Empty,
                UserIdentity = sessionIn.UserIdentity,
                AppOnline = sessionIn.AppOnline,
                Authenticated = sessionIn.Authenticated,
                SqlAuthorization = sessionIn.SqlAuthorization,
                WinAuthorization = sessionIn.WinAuthorization,
                SqlKey = sessionIn.SqlKey,
            };

            try {
                sqlMapper.LoadExplorerReportingMapParameters(sessionOut, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.UserIdentity.Role.Reporting = sqlMapper.LoadExplorerReportingMapData(dataTable, sqlService).Role.Reporting;
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionIn.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }

            return sessionOut;
        }

        public Session<Server.Entity.NullT> SavePassword(Session<Server.Entity.NullT> sessionIn) {
            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<Server.Entity.NullT> sessionOut = new Session<Server.Entity.NullT> {
                SessionOk = false,
                ClientMessage = String.Empty,
                ServerMessage = String.Empty,
                UserIdentity = sessionIn.UserIdentity,
                AppOnline = sessionIn.AppOnline,
                Authenticated = sessionIn.Authenticated,
                SqlAuthorization = sessionIn.SqlAuthorization,
                WinAuthorization = sessionIn.WinAuthorization,
                SqlKey = sessionIn.SqlKey
            };

            try {
                sqlMapper.SavePasswordMapParameters(sessionOut, ref sqlService);
                if (sqlService.ExecuteNonQuery()) {
                    sqlRequest = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionIn.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                    sessionOut.UserIdentity.Password.New = sessionOut.UserIdentity.Password.Old;
                }
            }

            return sessionOut;

        }

        public Session<Server.Entity.User.Identity> SaveIdentity(Session<Server.Entity.User.Identity> sessionIn) {
            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<Server.Entity.User.Identity> sessionOut = new Session<Server.Entity.User.Identity> {
                SessionOk = false,
                ClientMessage = String.Empty,
                ServerMessage = String.Empty,
                UserIdentity = sessionIn.UserIdentity,
                AppOnline = sessionIn.AppOnline,
                Authenticated = sessionIn.Authenticated,
                SqlAuthorization = sessionIn.SqlAuthorization,
                WinAuthorization = sessionIn.WinAuthorization,
                SqlKey = sessionIn.SqlKey
            };

            try {
                sqlMapper.SaveIdentityMapParameters(sessionIn, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.UserMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data = sqlMapper.SaveIdentityMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionIn.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            finally {
                //SQL Service error...
                if (!sqlService.SqlStatusOk) {
                    sessionOut.SessionOk = sqlService.SqlStatusOk;
                    sessionOut.ClientMessage = sqlService.SqlStatusMessage;
                    sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, sqlService.SqlStatusMessage);
                }
                //SQL Validation warning...
                else if (sqlRequest != sqlResponse) {
                    sessionOut.ClientMessage = sqlResponse;
                }
            }
            return sessionOut;
        }
    }
}
