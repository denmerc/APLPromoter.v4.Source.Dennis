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
                    Int32.Parse(reader[AnalyticMap.Names.analyticsId].ToString()),
                    reader[AnalyticMap.Names.analyticsName].ToString(),
                    reader[AnalyticMap.Names.analyticsDescription].ToString(),
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

        #region Load Drivers...
        public void LoadDriversMapParameters(Session<Server.Entity.Analytic.Identity> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = AnalyticMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(AnalyticMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.Data.Id.ToString()),
                new SqlServiceParameter(AnalyticMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey),
                new SqlServiceParameter(AnalyticMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, AnalyticMap.Names.loadDriversMessage)
            }; service.sqlParameters.List = parameters;

        }

        public List<Server.Entity.Analytic.Driver> LoadTypesMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            Boolean reading = true;
            Boolean selected = false;
            Int32 rows = data.Rows.Count;
            String driverNow = String.Empty;
            String driverLast = String.Empty;
            String modeNow = String.Empty;
            String modeLast = String.Empty;
            List<Server.Entity.Analytic.Driver> listDrivers = new List<Analytic.Driver>();
            List<Server.Entity.Analytic.Driver.Mode> listModes = new List<Analytic.Driver.Mode>();
            List<Server.Entity.Analytic.Driver.Mode.Group> listGroups = new List<Analytic.Driver.Mode.Group>();
            System.Data.DataTableReader reader = data.CreateDataReader();

            //From record set...
            while (reading) {
                reading = reader.Read();
                driverNow = (reading) ? reader[AnalyticMap.Names.driverName].ToString() : String.Empty;
                modeNow = (reading) ? reader[AnalyticMap.Names.driverModeName].ToString() : String.Empty;

                if (reading) {
                    listGroups.Add(new Analytic.Driver.Mode.Group(
                        Int32.Parse(reader[AnalyticMap.Names.driverGroupId].ToString()),
                        Int32.Parse(reader[AnalyticMap.Names.driverGroupValue].ToString()),
                        Decimal.Parse(reader[AnalyticMap.Names.driverGroupMinOutlier].ToString()),
                        Decimal.Parse(reader[AnalyticMap.Names.driverGroupMaxOutlier].ToString())
                        ));
                    if (modeLast != modeNow) {
                        listModes.Add(new Entity.Analytic.Driver.Mode(
                           Int32.Parse(reader[AnalyticMap.Names.driverModeKey].ToString()),
                           reader[AnalyticMap.Names.driverModeName].ToString(), //Name
                           reader[AnalyticMap.Names.driverModeName].ToString(), //Tooltip
                           Boolean.Parse(reader[AnalyticMap.Names.driverModeIncluded].ToString()),
                           new List<Analytic.Driver.Mode.Group>()
                            ));
                    }
                    if (driverLast != driverNow) {
                        listDrivers.Add(new Entity.Analytic.Driver(
                            Int32.Parse(reader[AnalyticMap.Names.driverId].ToString()),
                            Int32.Parse(reader[AnalyticMap.Names.driverKey].ToString()),
                            reader[AnalyticMap.Names.driverName].ToString(), //Name
                            reader[AnalyticMap.Names.driverName].ToString(), //Tooltip
                            Boolean.Parse(reader[AnalyticMap.Names.driverIncluded].ToString()),
                            new List<Analytic.Driver.Mode>()
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
                if (!(driverLast.Equals(String.Empty) || driverLast == driverNow)) {
                    if (driverNow.Equals(String.Empty)) {
                        listDrivers[listDrivers.Count - 1].Modes = listModes.GetRange(0, listModes.Count);
                    }
                    else {
                        listDrivers[listDrivers.Count - 2].Modes = listModes.GetRange(0, listModes.Count - 1);
                        listModes.RemoveRange(0, listModes.Count - 1);
                    }
                }
                driverLast = driverNow;
                modeLast = modeNow;
                selected = Boolean.Parse(reader[AnalyticMap.Names.driverIncluded].ToString());
            }
            return listDrivers;
        }
        #endregion

        #region Save Drivers...
        public void SaveDriversMapParameters(Session<Server.Entity.Analytic> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = AnalyticMap.Names.updateCommand;

            //Build comma delimited key list - type;mode;group;min;max, ...
            const System.Char splitter = ',';
            const System.Char delimiter = ';';
            System.Text.StringBuilder driverKeys = new System.Text.StringBuilder();
            foreach (Server.Entity.Analytic.Driver driver in session.Data.Drivers) {
                if (driver.Selected) {
                    foreach (Server.Entity.Analytic.Driver.Mode mode in driver.Modes) {
                        if (mode.Selected) { 
                            foreach (Server.Entity.Analytic.Driver.Mode.Group group in mode.Groups) {
                                driverKeys.Append(driver.Key.ToString() + delimiter);
                                driverKeys.Append(mode.Key.ToString() + delimiter);
                                driverKeys.Append(group.Value.ToString() + delimiter);
                                driverKeys.Append(group.MinOutlier.ToString() + delimiter);
                                driverKeys.Append(group.MaxOutlier.ToString() + splitter);
                            }
                        }
                    }
                }
            }
            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(AnalyticMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.Data.Self.Id.ToString()),
                new SqlServiceParameter(AnalyticMap.Names.drivers, SqlDbType.VarChar, 4000, ParameterDirection.Input, driverKeys.ToString()),
                new SqlServiceParameter(AnalyticMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey),
                new SqlServiceParameter(AnalyticMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, AnalyticMap.Names.saveDriversMessage)
            }; service.sqlParameters.List = parameters;
        }
        #endregion

        #region Load Price lists...
        public void LoadPricelistsMapParameters(Session<Server.Entity.Analytic.Identity> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = AnalyticMap.Names.selectCommand;

            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(AnalyticMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.Data.Id.ToString()),
                new SqlServiceParameter(AnalyticMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey),
                new SqlServiceParameter(AnalyticMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, AnalyticMap.Names.loadPriceListsMessage)
            }; service.sqlParameters.List = parameters;

        }

        public List<Server.Entity.PriceList> LoadPricelistsMapData(System.Data.DataTable data, Server.Data.SqlService service) {

            //Map the entity data...
            Boolean reading = true;
            Int32 rows = data.Rows.Count;
            String listTypeNow = String.Empty;
            String listTypeLast = String.Empty;
            List<Server.Entity.PriceList> priceLists = new List<Entity.PriceList>();
            List<Server.Entity.PriceList.Value> listValues = new List<Entity.PriceList.Value>();
            System.Data.DataTableReader reader = data.CreateDataReader();

            //From record set...
            while (reading) {
                reading = reader.Read();
                listTypeNow = (reading) ? reader[AnalyticMap.Names.priceListTypeName].ToString() : String.Empty;
                if (reading) {
                    listValues.Add(new Entity.PriceList.Value(
                        Int32.Parse(reader[AnalyticMap.Names.priceListId].ToString()),
                        Int32.Parse(reader[AnalyticMap.Names.priceListKey].ToString()),
                        reader[AnalyticMap.Names.priceListCode].ToString(),
                        reader[AnalyticMap.Names.priceListName].ToString(),
                        Boolean.Parse(reader[AnalyticMap.Names.priceListIncluded].ToString())
                        ));
                    if (listTypeLast != listTypeNow) {
                        priceLists.Add(new Entity.PriceList(
                            reader[AnalyticMap.Names.priceListTypeName].ToString(),
                            new List<PriceList.Value>()
                            ));
                    }
                }
                if (!(listTypeLast.Equals(String.Empty) || listTypeLast == listTypeNow)) {
                    if (listTypeNow.Equals(String.Empty)) {
                        priceLists[priceLists.Count - 1].Values = listValues.GetRange(0, listValues.Count);
                    }
                    else {
                        priceLists[priceLists.Count - 2].Values = listValues.GetRange(0, listValues.Count - 1);
                        listValues.RemoveRange(0, listValues.Count - 1);
                    }
                }
                listTypeLast = listTypeNow;
            }
            return priceLists;
        }
        #endregion

        #region Save Price lists...
        public void SavePricelistsMapParameters(Session<Server.Entity.Analytic> session, ref Server.Data.SqlService service) {

            //Map the command...
            service.SqlProcedure = AnalyticMap.Names.updateCommand;

            //Build comma delimited key list...
            const System.Char delimiter = ',';
            System.Text.StringBuilder priceKeys = new System.Text.StringBuilder();
            foreach (Server.Entity.PriceList list in session.Data.PriceLists) {
                foreach (Server.Entity.PriceList.Value value in list.Values) {
                    if (!value.Included) { priceKeys.Append(value.Key.ToString() + delimiter); }
                }
            }
            //Map the parameters...
            APLPromoter.Server.Data.SqlServiceParameter[] parameters = { 
                new SqlServiceParameter(AnalyticMap.Names.id, SqlDbType.Int, 0, ParameterDirection.Input, session.Data.Self.Id.ToString()),
                new SqlServiceParameter(AnalyticMap.Names.pricelists, SqlDbType.VarChar, 4000, ParameterDirection.Input, priceKeys.ToString()),
                new SqlServiceParameter(AnalyticMap.Names.sqlSession, SqlDbType.VarChar, 50, ParameterDirection.Input, session.SqlKey),
                new SqlServiceParameter(AnalyticMap.Names.sqlMessage, SqlDbType.VarChar, 500, ParameterDirection.InputOutput, AnalyticMap.Names.savePriceListsMessage)
            }; service.sqlParameters.List = parameters;
        }
        #endregion

        #region Enumeration map...
        //Database names...
        public static class Names {
            //Select commands...
            public const String selectCommand = "dbo.aplAnalyticsSelect";
            public const String loadIdentitiesMessage = "selectIdentities";
            public const String loadFilterMessage = "selectFilters";
            public const String loadDriversMessage = "selectDrivers";
            public const String loadPriceListsMessage = "selectPriceLists";

            //Update commands...
            public const String updateCommand = "dbo.aplAnalyticsUpdate";
            public const String saveIdentityMessage = "updateIdentity";
            public const String saveFiltersMessage = "updateFilters";
            public const String saveDriversMessage = "updateDrivers";
            public const String savePriceListsMessage = "updatePriceLists";

            //Process commands...
            public const String runProcessMarkup = "updateProcessMarkup";
            public const String runProcessMovement = "updateProcessMover";
            public const String runProcessDaysOnHand = "updateProcessDaysOnHand";

            //Default parameters...
            public const String id = "id";
            public const String name = "name";
            public const String description = "description";
            public const String filters = "filterKeys";
            public const String drivers = "driverKeys";
            public const String pricelists = "priceListKeys";
            public const String sqlSession = "session";
            public const String sqlMessage = "message";

            #region Fields Identity...
            public const String analyticsId = "analyticsId";
            public const String analyticsName = "analyticsName";
            public const String analyticsDescription = "analyticsDescription";
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
            public const String filterId = "filterId";
            public const String filterKey = "filterKey";
            public const String filterCode = "filterCode";
            public const String filterName = "filterText";
            public const String filterIncluded = "included";
            public const String filterTypeName = "filterTypeText";
            #endregion

            #region Fields Drivers...
            public const String driverId = "driverId";
            public const String driverKey = "driverKey";
            public const String driverName = "driverText";
            public const String driverModeKey = "modeKey";
            public const String driverModeName = "modeText";
            public const String driverGroupId = "groupId";
            public const String driverGroupValue = "groupValue";
            public const String driverGroupMinOutlier = "minOutlier";
            public const String driverGroupMaxOutlier = "maxOutlier";
            public const String driverIncluded = "driverIncluded";
            public const String driverModeIncluded = "modeIncluded";
            #endregion

            #region Fields Pricelists...
            public const String priceListId = "listId";
            public const String priceListKey = "listKey";
            public const String priceListCode = "listCode";
            public const String priceListName = "listName";
            public const String priceListText = "listText";
            public const String priceListTypeId = "listTypeId";
            public const String priceListTypeName = "listTypeName";
            public const String priceListIncluded = "included";
            #endregion
        }
        #endregion

        #region Message map...
        //selectIdentities  - analyticsId,analyticsName,analyticsDescription,refreshedText,createdText,editedText,refreshed,created,edited,authorText,editorText,ownerText,active
        //selectDrivers     - analyticsId,driverId,driverKey,driverText,modeKey,modeText,groupId,groupValue,minOutlier,maxOutlier,driverIncluded,modeIncluded
        //selectFilters       - analyticsId,filterId,filterKey,filterCode,filterText,filterTypeId,filterTypeText,included
        //selectPriceLists  - analyticsId,listId,listKey,listCode,listName,listText,listTypeId,listTypeName,included
        #endregion

        //TODO - Determine result view for validation; by workflow, validation messages, validation warnings, icons
    }
}
