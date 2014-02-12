using System;
using System.Collections.Generic;
using APLPromoter.Server.Entity;

namespace APLPromoter.Server.Data {

    public interface IAnalyticData {
        Session<List<Server.Entity.Analytic.Identity>> LoadList(Session<Server.Entity.NullT> session);
        Session<Server.Entity.Analytic.Identity> SaveIdentity(Session<Server.Entity.Analytic.Identity> session);
        Session<List<Server.Entity.Filter>> LoadFilters(Session<Server.Entity.Analytic.Identity> session);
        Session<List<Server.Entity.Filter>> SaveFilters(Session<Server.Entity.Analytic> session);
        Session<List<Server.Entity.Analytic.Driver>> LoadDrivers(Session<Server.Entity.Analytic.Identity> session);
        Session<List<Server.Entity.Analytic.Driver>> SaveDrivers(Session<Server.Entity.Analytic> session);
        Session<List<Server.Entity.PriceList>> LoadPriceLists(Session<Server.Entity.Analytic.Identity> session);
        Session<List<Server.Entity.PriceList>> SavePriceLists(Session<Server.Entity.Analytic> session);
    }

    public class AnalyticData : IAnalyticData {

        #region Constants...
        const String invalid = "Invalid:";
        const String connectionName = "defaultConnectionString";
        const String aplServiceEventLog = "APLPromoterServerData";
        #endregion

        #region Variables...
        private System.Diagnostics.EventLog localServiceLog;
        private APLPromoter.Server.Data.AnalyticMap sqlMapper;
        private APLPromoter.Server.Data.SqlService sqlService;

        private readonly List<Analytic.Identity> _SavedAnalytics = new List<Analytic.Identity>();
        private readonly List<Analytic.Driver> _SavedTypes = new List<Analytic.Driver>();
        private readonly List<Filter> _SavedFilters = new List<Filter>();
        #endregion

        private String sqlConnection {
            get {
                return System.Configuration.ConfigurationManager.AppSettings[connectionName];
            }
        }

        public AnalyticData() {

            sqlMapper = new AnalyticMap();            
            sqlService = new SqlService(this.sqlConnection);
            localServiceLog = new System.Diagnostics.EventLog();
            //if (!System.Diagnostics.EventLog.SourceExists(cseServiceEventLog)) EventLog.CreateEventSource(cseServiceEventLog, "Application");
            //Setup <cseServiceEventLog> event source manually through registry key: HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Eventlog\Application
            //To resolve message IDs create a RG_EXPAND_SZ attribute, named "EventMessageFile" to: "C:\WINDOWS\Microsoft.NET\Framework\<current version>\EventLogMessages.dll"
            localServiceLog.Source = aplServiceEventLog;

        }

        ~AnalyticData() {
            if (sqlService != null && sqlService.SqlConnectionOk) sqlService.ExecuteCloseConnection();
        }

        public Session<List<Server.Entity.Analytic.Identity>> LoadList(Session<Server.Entity.NullT> sessionIn) {
            
            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<List<Server.Entity.Analytic.Identity>> sessionOut = new Session<List<Server.Entity.Analytic.Identity>> {
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
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
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

        public Session<Server.Entity.Analytic.Identity> SaveIdentity(Session<Server.Entity.Analytic.Identity> sessionIn) {

            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<Server.Entity.Analytic.Identity> sessionOut = new Session<Server.Entity.Analytic.Identity> {
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
                sqlMapper.SaveIdentityMapParameters(sessionOut, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data.Id = (sqlMapper.SaveIdentityMapData(dataTable, sqlService)).Id;
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionOut.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
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

        public Session<List<Server.Entity.Filter>> LoadFilters(Session<Server.Entity.Analytic.Identity> sessionIn) {

            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<List<Server.Entity.Filter>> sessionOut = new Session<List<Server.Entity.Filter>> {
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
                sqlMapper.LoadFiltersMapParameters(sessionIn, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data = sqlMapper.LoadFiltersMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionOut.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
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

        public Session<List<Server.Entity.Filter>> SaveFilters(Session<Server.Entity.Analytic> sessionIn) {

            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<List<Server.Entity.Filter>> sessionOut = new Session<List<Server.Entity.Filter>> {
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
                sqlMapper.SaveFiltersMapParameters(sessionIn, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data = sqlMapper.LoadFiltersMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionOut.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
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

        public Session<List<Server.Entity.Analytic.Driver>> LoadDrivers(Session<Server.Entity.Analytic.Identity> sessionIn) {

            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<List<Server.Entity.Analytic.Driver>> sessionOut = new Session<List<Server.Entity.Analytic.Driver>> {
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
                sqlMapper.LoadDriversMapParameters(sessionIn, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data = sqlMapper.LoadTypesMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionOut.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
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

        public Session<List<Server.Entity.Analytic.Driver>> SaveDrivers(Session<Server.Entity.Analytic> sessionIn) {

            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<List<Server.Entity.Analytic.Driver>> sessionOut = new Session<List<Server.Entity.Analytic.Driver>> {
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
                sqlMapper.SaveDriversMapParameters(sessionIn, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data = sqlMapper.LoadTypesMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionOut.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
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

        public Session<List<Server.Entity.PriceList>> LoadPriceLists(Session<Server.Entity.Analytic.Identity> sessionIn) {
            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<List<Server.Entity.PriceList>> sessionOut = new Session<List<Server.Entity.PriceList>> {
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
                sqlMapper.LoadPricelistsMapParameters(sessionIn, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data = sqlMapper.LoadPricelistsMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionOut.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
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

        public Session<List<Server.Entity.PriceList>> SavePriceLists(Session<Server.Entity.Analytic> sessionIn) {

            String sqlRequest = String.Empty;
            String sqlResponse = String.Empty;
            //Initialize session...
            Session<List<Server.Entity.PriceList>> sessionOut = new Session<List<Server.Entity.PriceList>> {
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
                sqlMapper.SavePricelistsMapParameters(sessionIn, ref sqlService);
                System.Data.DataTable dataTable = sqlService.ExecuteReader();
                if (sqlService.SqlStatusOk) {
                    sqlRequest = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbValue;
                    sqlResponse = sqlService.sqlParameters[Data.AnalyticMap.Names.sqlMessage].dbOutput;
                    if (sqlRequest == sqlResponse) {
                        sessionOut.Data = sqlMapper.LoadPricelistsMapData(dataTable, sqlService);
                        sessionOut.SessionOk = true;
                    }
                }
            }
            catch (Exception ex) {
                sessionOut.ServerMessage = String.Format("{0}: {1}, {2}, {3}, {4} ", aplServiceEventLog, sqlService.SqlProcedure, sqlRequest, ex.Source, ex.Message);
                localServiceLog.WriteEntry(sessionOut.ServerMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
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
