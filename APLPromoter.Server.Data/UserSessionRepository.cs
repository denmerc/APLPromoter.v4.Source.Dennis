using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APLPromoter.Server.Entity;

namespace APLPromoter.Server.Data
{
    public class UserSessionRepository
    {
        #region Constants...
        const String invalid = "Invalid:";
        const String connectionName = "defaultConnectionString";
        const String aplServiceEventLog = "APLPromoterServices";
        #endregion

        #region Variables...
        private System.Diagnostics.EventLog localServiceLog;
        private EntityService sqlEntity;
        private SqlService sqlService;
        #endregion

        private String sqlConnection
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings[connectionName];
            }
        }

        public UserSessionRepository()
        {
            sqlEntity = new EntityService();
            sqlService = new SqlService(this.sqlConnection);
            localServiceLog = new System.Diagnostics.EventLog();
            //if (!System.Diagnostics.EventLog.SourceExists(cseServiceEventLog)) EventLog.CreateEventSource(cseServiceEventLog, "Application");
            //Setup <cseServiceEventLog> event source manually through registry key: HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Eventlog\Application
            //To resolve message IDs create a RG_EXPAND_SZ attribute, named "EventMessageFile" to: "C:\WINDOWS\Microsoft.NET\Framework\<current version>\EventLogMessages.dll"
            localServiceLog.Source = aplServiceEventLog;

        }

        public Boolean SessionInit(ref UserSession session)
        {
            Boolean sessionStatus = false;
            System.Data.DataTable dataTable;

            if (sqlService.sqlConnectionOk)
            {
                sqlService.sqlParameters = sqlEntity.SessionInitMapParameters(ref session);
                dataTable = sqlService.SqlTableReader(session.sqlCommand);
                if (sqlService.sqlStatusOk)
                    sessionStatus = sqlEntity.SessionInitMapData(sqlService, dataTable, ref session);
                else
                {
                    session.clientMessage = String.Format("{0}: {1}, {2} ", aplServiceEventLog, session.sqlCommand, sqlService.sqlStatusMessage);
                    localServiceLog.WriteEntry(sqlService.sqlStatusMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
                }
            }
            else
            {
                session.clientMessage = String.Format("{0}: {1}, {2} ", aplServiceEventLog, session.sqlCommand, sqlService.sqlStatusMessage);
                localServiceLog.WriteEntry(sqlService.sqlStatusMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            return sessionStatus;
        }

        public Boolean SessionAuthenticate(ref UserSession session)
        {
            Boolean sessionStatus = false;
            System.Data.DataTable dataTable;

            if (sqlService.sqlConnectionOk)
            {
                sqlService.sqlParameters = sqlEntity.SessionAuthenticateMapParameters(ref session);
                dataTable = sqlService.SqlTableReader(session.sqlCommand);
                if (sqlService.sqlStatusOk)
                    sessionStatus = sqlEntity.SessionAuthenticateMapData(sqlService, dataTable, ref session);
                else
                {
                    session.clientMessage = String.Format("{0}: {1}, {2} ", aplServiceEventLog, session.sqlCommand, sqlService.sqlStatusMessage);
                    localServiceLog.WriteEntry(sqlService.sqlStatusMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
                }
            }
            else
            {
                session.clientMessage = String.Format("{0}: {1}, {2} ", aplServiceEventLog, session.sqlCommand, sqlService.sqlStatusMessage);
                localServiceLog.WriteEntry(sqlService.sqlStatusMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            return sessionStatus;
        }

        public Boolean UserLoad(User.Identity data)
        {
            Boolean sessionStatus = false;
            System.Data.DataTable dataTable;

            if (sqlService.sqlConnectionOk)
            {
                sqlService.sqlParameters = sqlEntity.UserLoadMapParameters(ref data);
                dataTable = sqlService.SqlTableReader(data.SqlCommand);
                if (sqlService.sqlStatusOk)
                    sessionStatus = sqlEntity.UserLoadMapData(sqlService, dataTable, ref data);
                else
                {
                    data.ClientMessage = String.Format("{0}: {1}, {2} ", aplServiceEventLog, data.SqlCommand, sqlService.sqlStatusMessage);
                    localServiceLog.WriteEntry(sqlService.sqlStatusMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
                }
            }
            else
            {
                data.ClientMessage = String.Format("{0}: {1}, {2} ", aplServiceEventLog, data.SqlCommand, sqlService.sqlStatusMessage);
                localServiceLog.WriteEntry(sqlService.sqlStatusMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            return sessionStatus;
        }

        public Boolean UserSave(ref User.Identity data)
        {
            Boolean sessionStatus = false;

            if (sqlService.sqlConnectionOk)
            {
                sqlService.sqlParameters = sqlEntity.UserSaveMapParameters(ref data);
                if (sqlService.sqlStatusOk)
                    sessionStatus = sqlService.ExecuteNonQuery(data.SqlCommand);
                else
                {
                    data.ClientMessage = String.Format("{0}: {1}, {2} ", aplServiceEventLog, data.SqlCommand, sqlService.sqlStatusMessage);
                    localServiceLog.WriteEntry(sqlService.sqlStatusMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
                }
            }
            else
            {
                data.ClientMessage = String.Format("{0}: {1}, {2} ", aplServiceEventLog, data.SqlCommand, sqlService.sqlStatusMessage);
                localServiceLog.WriteEntry(sqlService.sqlStatusMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            return sessionStatus;
        }

        public Boolean LoadView(ref UserSession session)
        {
            Boolean sessionStatus = false;
            System.Data.DataTable dataTable;

            if (sqlService.sqlConnectionOk)
            {
                sqlService.sqlParameters = sqlEntity.LoadViewMapParameters(ref session);
                dataTable = sqlService.SqlTableReader(session.sqlCommand);
                if (sqlService.sqlStatusOk)
                    sessionStatus = sqlEntity.LoadViewMapData(sqlService, dataTable, ref session);
                else
                {
                    session.clientMessage = String.Format("{0}: {1}, {2} ", aplServiceEventLog, session.sqlCommand, sqlService.sqlStatusMessage);
                    localServiceLog.WriteEntry(sqlService.sqlStatusMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
                }
            }
            else
            {
                session.clientMessage = String.Format("{0}: {1}, {2} ", aplServiceEventLog, session.sqlCommand, sqlService.sqlStatusMessage);
                localServiceLog.WriteEntry(sqlService.sqlStatusMessage, System.Diagnostics.EventLogEntryType.FailureAudit);
            }
            return sessionStatus;
        }
    }
}
