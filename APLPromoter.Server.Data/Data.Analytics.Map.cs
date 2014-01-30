using System;
using System.Data;
using System.Collections.Generic;
using APLPromoter.Server.Entity;

namespace APLPromoter.Server.Data {

    class AnalyticMap {

        #region Load Identities...
        public void LoadListMapParameters(Session<List<Server.Entity.Analytic.Identity>> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = AnalyticMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(AnalyticMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey),
                new SqlServiceParameter(AnalyticMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, AnalyticMap.Names.loadIdentitiesMessage)
            }; service.sqlParameters.List = parameters;

        }

        public List<Server.Entity.Analytic.Identity> LoadListMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            List<Server.Entity.Analytic.Identity> list = new List<Analytic.Identity>(data.Rows.Count);
            //Record set...
            while (reader.Read()) {
                list.Add(new Analytic.Identity (
                    Int32.Parse(reader[AnalyticMap.Names.id].ToString()),
                    reader[AnalyticMap.Names.name].ToString(),
                    reader[AnalyticMap.Names.description].ToString(),
                    reader[AnalyticMap.Names.refreshedText].ToString(),
                    reader[AnalyticMap.Names.createdText].ToString(),
                    reader[AnalyticMap.Names.editedText].ToString(),
                    DateTime.Parse(reader[AnalyticMap.Names.refreshed].ToString()),
                    DateTime.Parse(reader[AnalyticMap.Names.created].ToString()),
                    DateTime.Parse(reader[AnalyticMap.Names.edited].ToString()),
                    reader[AnalyticMap.Names.authorText].ToString(),
                    reader[AnalyticMap.Names.editorText].ToString(),
                    reader[AnalyticMap.Names.ownerText].ToString(),
                    Boolean.Parse(reader[AnalyticMap.Names.active].ToString())
                ));
            }

            return list;
        }
        #endregion

        #region Save Identity...
        public void SaveIdentityMapParameters(Session<Server.Entity.Analytic.Identity> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = AnalyticMap.Names.updateCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(AnalyticMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.Data.Id.ToString()),
                new SqlServiceParameter(AnalyticMap.Names.name, SqlDbType.VarChar, 105, ParameterDirection.Input, session.Data.Name),
                new SqlServiceParameter(AnalyticMap.Names.description, SqlDbType.VarChar, 255, ParameterDirection.Input, session.Data.Description),
                new SqlServiceParameter(AnalyticMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey),
                new SqlServiceParameter(AnalyticMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, AnalyticMap.Names.saveIdentityMessage)
            }; service.sqlParameters.List= parameters;
        }

        public Server.Entity.Analytic.Identity SaveIdentityMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            System.Data.DataTableReader reader = data.CreateDataReader();
            Server.Entity.Analytic.Identity item = new Analytic.Identity();
            //Single record...
            if (reader.Read()) {
                item.Id = Int32.Parse(reader[AnalyticMap.Names.id].ToString());
            }

            return item;
        }
        #endregion

        #region Load Filters...
        public void LoadFiltersMapParameters(Session<Server.Entity.Analytic.Identity> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = AnalyticMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(AnalyticMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.Data.Id.ToString()),
                new SqlServiceParameter(AnalyticMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey),
                new SqlServiceParameter(AnalyticMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, AnalyticMap.Names.loadFilterMessage)
            }; service.sqlParameters.List = parameters;

        }
        
        public List<Server.Entity.Filter> LoadFiltersMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            Boolean reading = true;
            Int32 rows = data.Rows.Count;
            String filterTypeNow = String.Empty;
            String filterTypeLast = String.Empty;
            List<Server.Entity.Filter> listFilters = new List<Entity.Filter>();
            List<Server.Entity.Filter.Value> listValues = new List<Entity.Filter.Value>();
            System.Data.DataTableReader reader = data.CreateDataReader();

            //From record set...
            while (reading) {
                reading = reader.Read();
                filterTypeNow = (reading) ? reader[AnalyticMap.Names.filterTypeName].ToString() : String.Empty;
                if (reading) {
                    listValues.Add(new Entity.Filter.Value(
                        Int32.Parse(reader[AnalyticMap.Names.filterId].ToString()),
                        Int32.Parse(reader[AnalyticMap.Names.filterKey].ToString()),
                        reader[AnalyticMap.Names.filterCode].ToString(),
                        reader[AnalyticMap.Names.filterName].ToString(),
                        Boolean.Parse(reader[AnalyticMap.Names.filterIncluded].ToString())
                        ));
                    if (filterTypeLast != filterTypeNow) {
                        listFilters.Add(new Entity.Filter(
                            reader[AnalyticMap.Names.filterTypeName].ToString(),
                            new List<Filter.Value>()
                            ));
                    }
                }
                if (!(filterTypeLast.Equals(String.Empty) || filterTypeLast == filterTypeNow) ) {
                    if (filterTypeNow.Equals(String.Empty)) {
                        listFilters[listFilters.Count - 1].Values = listValues.GetRange(0, listValues.Count);
                    }
                    else {
                        listFilters[listFilters.Count - 2].Values = listValues.GetRange(0, listValues.Count - 1);
                        listValues.RemoveRange(0, listValues.Count - 1);
                    }
                }
                filterTypeLast = filterTypeNow;
            }
            return listFilters;
        }
        #endregion

        #region Save Filters...
        public void SaveFiltersMapParameters(Session<Server.Entity.Analytic> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = AnalyticMap.Names.updateCommand;

            //Build comma delimited key list...
            const System.Char delimiter = ',';
            System.Text.StringBuilder filterKeys = new System.Text.StringBuilder();
            foreach (Server.Entity.Filter filter in session.Data.Filters) { 
                foreach (Server.Entity.Filter.Value value in filter.Values) {
                    if (!value.Included) { filterKeys.Append(value.Key.ToString() + delimiter); }
                }
            }
            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(AnalyticMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.Data.Self.Id.ToString()),
                new SqlServiceParameter(AnalyticMap.Names.filters, SqlDbType.VarChar, 4000, ParameterDirection.Input, filterKeys.ToString()),
                new SqlServiceParameter(AnalyticMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey),
                new SqlServiceParameter(AnalyticMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, AnalyticMap.Names.saveFiltersMessage)
            }; service.sqlParameters.List = parameters;
        }
        #endregion

        #region Load Types...
        public void LoadTypesMapParameters(Session<Server.Entity.Analytic.Identity> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = AnalyticMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(AnalyticMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.Data.Id.ToString()),
                new SqlServiceParameter(AnalyticMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey),
                new SqlServiceParameter(AnalyticMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, AnalyticMap.Names.loadTypesMessage)
            }; service.sqlParameters.List = parameters;

        }

        public List<Server.Entity.Analytic.Type> LoadTypesMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            Boolean reading = true;
            Boolean selected = false;
            Int32 rows = data.Rows.Count;
            String typeNow = String.Empty;
            String typeLast = String.Empty;
            String modeNow = String.Empty;
            String modeLast = String.Empty;
            List<Server.Entity.Analytic.Type> listTypes = new List<Analytic.Type>();
            List<Server.Entity.Analytic.Type.Mode> listModes = new List<Analytic.Type.Mode>();
            List<Server.Entity.Analytic.Type.Mode.Group> listGroups = new List<Analytic.Type.Mode.Group>();
            System.Data.DataTableReader reader = data.CreateDataReader();

            //From record set...
            while (reading) {
                reading = reader.Read();
                typeNow = (reading) ? reader[AnalyticMap.Names.typeName].ToString() : String.Empty;
                modeNow = (reading) ? reader[AnalyticMap.Names.typeModeName].ToString() : String.Empty;

                if (reading) {
                    listGroups.Add(new Analytic.Type.Mode.Group(
                        Int32.Parse(reader[AnalyticMap.Names.typeGroupId].ToString()),
                        Int32.Parse(reader[AnalyticMap.Names.typeGroupValue].ToString()),
                        Decimal.Parse(reader[AnalyticMap.Names.typeGroupMinOutlier].ToString()),
                        Decimal.Parse(reader[AnalyticMap.Names.typeGroupMaxOutlier].ToString())
                        ));
                    if (modeLast != modeNow) {
                        listModes.Add(new Entity.Analytic.Type.Mode(
                           Int32.Parse(reader[AnalyticMap.Names.typeModeKey].ToString()),
                           reader[AnalyticMap.Names.typeModeName].ToString(), //Name
                           reader[AnalyticMap.Names.typeModeName].ToString(), //Tooltip
                           Boolean.Parse(reader[AnalyticMap.Names.typeModeIncluded].ToString()),
                           new List<Analytic.Type.Mode.Group>()
                            ));
                    }
                    if (typeLast != typeNow) {
                        listTypes.Add(new Entity.Analytic.Type(
                            Int32.Parse(reader[AnalyticMap.Names.typeId].ToString()),
                            Int32.Parse(reader[AnalyticMap.Names.typeKey].ToString()),
                            reader[AnalyticMap.Names.typeName].ToString(), //Name
                            reader[AnalyticMap.Names.typeName].ToString(), //Tooltip
                            Boolean.Parse(reader[AnalyticMap.Names.typeIncluded].ToString()),
                            new List<Analytic.Type.Mode>()
                            ));
                    }
                }

                if (!(modeLast.Equals(String.Empty) || modeLast == modeNow)) {
                    if (modeNow.Equals(String.Empty)) {
                        listModes[listModes.Count - 1].Groups = listGroups.GetRange(0, listGroups.Count);
                    }
                    else {
                        listModes[listModes.Count - 2].Groups = listGroups.GetRange(0, listGroups.Count - 1);
                        listGroups.RemoveRange(0, listGroups.Count - 1);
                    }
                }
                if (!(typeLast.Equals(String.Empty) || typeLast == typeNow)) {
                    if (typeNow.Equals(String.Empty)) {
                        listTypes[listTypes.Count - 1].Modes = listModes.GetRange(0, listModes.Count);
                    }
                    else {
                        listTypes[listTypes.Count - 2].Modes = listModes.GetRange(0, listModes.Count - 1);
                        listModes.RemoveRange(0, listModes.Count - 1);
                    }
                }
                typeLast = typeNow;
                modeLast = modeNow;
                selected = Boolean.Parse(reader[AnalyticMap.Names.typeIncluded].ToString());
            }
            return listTypes;
        }
        #endregion

        #region Save Types...
        public void SaveTypesMapParameters(Session<Server.Entity.Analytic> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = AnalyticMap.Names.updateCommand;

            //Build comma delimited key list - type;mode;group;min;max, ...
            const System.Char splitter = ',';
            const System.Char delimiter = ';';
            System.Text.StringBuilder typeKeys = new System.Text.StringBuilder();
            foreach (Server.Entity.Analytic.Type type in session.Data.Types) {
                if (type.Selected) {
                    foreach (Server.Entity.Analytic.Type.Mode mode in type.Modes) {
                        if (mode.Selected) { 
                            foreach (Server.Entity.Analytic.Type.Mode.Group group in mode.Groups) {
                                typeKeys.Append(type.Key.ToString() + delimiter);
                                typeKeys.Append(mode.Key.ToString() + delimiter);
                                typeKeys.Append(group.Value.ToString() + delimiter);
                                typeKeys.Append(group.MinOutlier.ToString() + delimiter);
                                typeKeys.Append(group.MaxOutlier.ToString() + splitter);
                            }
                        }
                    }
                }
            }
            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(AnalyticMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.Data.Self.Id.ToString()),
                new SqlServiceParameter(AnalyticMap.Names.types, SqlDbType.VarChar, 4000, ParameterDirection.Input, typeKeys.ToString()),
                new SqlServiceParameter(AnalyticMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey),
                new SqlServiceParameter(AnalyticMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, AnalyticMap.Names.saveTypesMessage)
            }; service.sqlParameters.List = parameters;
        }
        #endregion

        #region Enumeration map...
        //Database names...
        public static class Names {
            //Select commands...
            public const String selectCommand = "dbo.aplAnalyticsSelect";
            public const String loadIdentitiesMessage = "selectIdentities";
            public const String loadTypesMessage = "selectTypes";
            public const String loadFilterMessage = "selectFilters";

            //Update commands...
            public const String updateCommand = "dbo.aplAnalyticsUpdate";
            public const String saveIdentityMessage = "updateIdentity";
            public const String saveFiltersMessage = "updateFilters";
            public const String saveTypesMessage = "updateTypes";

            //Default parameters...
            public const String id = "id";
            public const String types = "typeKeys";
            public const String filters = "filterKeys";
            public const String sqlSession = "session";
            public const String sqlMessage = "message";

            #region Fields Identity...
            public const String analyticId = "analyticId";
            public const String name = "name";
            public const String description = "description";
            public const String refreshedText = "refreshedText";
            public const String createdText = "createdText";
            public const String editedText = "editedText";
            public const String refreshed = "refreshed";
            public const String created = "created";
            public const String edited = "edited";
            public const String authorText = "authorText";
            public const String editorText = "editorText";
            public const String ownerText = "ownerText";
            public const String active = "active";
            #endregion

            #region Fields Filters...
            public const String filterTypeName = "filterTypeText";
            public const String filterId = "filterId";
            public const String filterKey = "filterKey";
            public const String filterCode = "filterCode";
            public const String filterName = "filterText";
            public const String filterIncluded = "included";
            #endregion

            #region Fields Types...
            public const String typeId = "typeId";
            public const String typeKey = "typeKey";
            public const String typeName = "typeText";
            public const String typeModeKey = "modeKey";
            public const String typeModeName = "modeText";
            public const String typeGroupId = "groupId";
            public const String typeGroupValue = "groupValue";
            public const String typeGroupMinOutlier = "minOutlier";
            public const String typeGroupMaxOutlier = "maxOutlier";
            public const String typeIncluded = "typeIncluded";
            public const String typeModeIncluded = "modeIncluded";
            #endregion
        }
        #endregion

        #region Message map...
        //selectIdentities  - id,name,description,refreshedText,createdText,editedText,refreshed,created,edited,authorText,editorText,ownerText,active
        //selectTypes       - analyticsId,typeId,typeKey,typeText,modeKey,modeText,groupId,groupValue,minOutlier,maxOutlier,included
        //selectFilters       - analyticsId,filterId,filterKey,filterCode,filterText,filterTypeId,filterTypeText,included
        #endregion

        //TODO - Determine result view for validation; by workflow, validation messages, validation warnings, icons
    }
}
