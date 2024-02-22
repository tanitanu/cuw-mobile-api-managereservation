using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
//using System.Data.OleDb;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using United.Mobile.Model.Shopping;
using System.Security.Cryptography;


namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class Manager
    {
        //public static string InsertState(int applicationId, string sessionId, string tag, object obj, Type type)
        //{
        //    if (obj != null)
        //    {
        //        string xml = Serialization.Serialize(obj, type);

        //        sessionId = string.IsNullOrEmpty(sessionId) ? Guid.NewGuid().ToString().ToUpper() : sessionId.Trim().ToUpper();

        //        Database database = DatabaseFactory.CreateDatabase(Constant.CONNECTION_STRING_STATE);
        //        DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("usp_Insert_State");
        //        database.AddInParameter(dbCommand, "@ApplicationId", DbType.Int32, applicationId);
        //        database.AddInParameter(dbCommand, "@SessionId", DbType.String, sessionId);
        //        database.AddInParameter(dbCommand, "@Tag", DbType.String, string.IsNullOrEmpty(tag) ? type.ToString().ToUpper() : tag.Trim().ToUpper());
        //        database.AddInParameter(dbCommand, "@State", DbType.Xml, xml);
        //        database.ExecuteNonQuery(dbCommand);
        //    }

        //    return sessionId;
        //}

        //public static T GetState<T>(int applicationId, string sessionId, string tag)
        //{
        //    string xml = string.Empty;
        //    if (!string.IsNullOrEmpty(sessionId) && !string.IsNullOrEmpty(tag))
        //    {
        //        Database database = DatabaseFactory.CreateDatabase(Constant.CONNECTION_STRING_STATE);
        //        DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("usp_Select_ApplicationState");
        //        database.AddInParameter(dbCommand, "@ApplicationId", DbType.Int32, applicationId);
        //        database.AddInParameter(dbCommand, "@SessionId", DbType.String, sessionId.Trim().ToUpper());
        //        database.AddInParameter(dbCommand, "@Tag", DbType.String, tag.Trim().ToUpper());
        //        using (IDataReader dataReader = database.ExecuteReader(dbCommand))
        //        {
        //            while (dataReader.Read())
        //            {
        //                xml = dataReader["State"].ToString();
        //            }
        //        }
        //    }

        //    return string.IsNullOrEmpty(xml) ? default(T) : (T)United.Xml.Serialization.Deserialize(xml, typeof(T));
        //}

        //public static string InsertPAOffersState(int applicationId, string sessionId, string tag, string recordLocator, string lastName, string requestObj, string responseObj)
        //{
        //    sessionId = string.IsNullOrEmpty(sessionId) ? Guid.NewGuid().ToString().ToUpper() : sessionId.Trim().ToUpper();

        //    Database database = DatabaseFactory.CreateDatabase(Constant.CONNECTION_STRING_STATE);
        //    DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("usp_Insert_PA_MerchOffers_State");
        //    database.AddInParameter(dbCommand, "@ApplicationId", DbType.Int32, applicationId);
        //    database.AddInParameter(dbCommand, "@SessionId", DbType.String, sessionId);
        //    database.AddInParameter(dbCommand, "@Tag", DbType.String, tag.Trim().ToUpper());
        //    database.AddInParameter(dbCommand, "@Record_Locator", DbType.String, recordLocator);
        //    database.AddInParameter(dbCommand, "@Last_Name", DbType.String, lastName);
        //    database.AddInParameter(dbCommand, "@Request", DbType.Xml, requestObj);
        //    database.AddInParameter(dbCommand, "@Response_State", DbType.Xml, responseObj);
        //    database.ExecuteNonQuery(dbCommand);

        //    return sessionId;
        //}

        //public static T GetPAOffersState<T>(int applicationId, string sessionId, string tag, ref string recordLocator, ref string lastName)
        //{
        //    string xml = string.Empty;
        //    if (!string.IsNullOrEmpty(sessionId) && !string.IsNullOrEmpty(tag))
        //    {
        //        Database database = DatabaseFactory.CreateDatabase(Constant.CONNECTION_STRING_STATE);
        //        DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("usp_Select_PA_Offers_Application_State");
        //        database.AddInParameter(dbCommand, "@ApplicationId", DbType.Int32, applicationId);
        //        database.AddInParameter(dbCommand, "@SessionId", DbType.String, sessionId.Trim().ToUpper());
        //        database.AddInParameter(dbCommand, "@Tag", DbType.String, tag.Trim().ToUpper());
        //        using (IDataReader dataReader = database.ExecuteReader(dbCommand))
        //        {
        //            while (dataReader.Read())
        //            {
        //                xml = dataReader["Response_State"].ToString();
        //                recordLocator = dataReader["Record_Locator"].ToString();
        //                lastName = dataReader["Last_Name"].ToString();
        //            }
        //        }
        //    }

        //    return string.IsNullOrEmpty(xml) ? default(T) : (T)United.Xml.Serialization.Deserialize(xml, typeof(T));
        //}

        //public static bool CheckIfPAFullFillmentPaymentAlreadyDone(int applicationId, string sessionId, string tag, string recordLocator, string lastName)
        //{
        //    if (!string.IsNullOrEmpty(sessionId) && !string.IsNullOrEmpty(tag))
        //    {
        //        Database database = DatabaseFactory.CreateDatabase(Constant.CONNECTION_STRING_STATE);
        //        DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("usp_check__if_PAFullFillment_Payment_Already_Done");
        //        database.AddInParameter(dbCommand, "@ApplicationId", DbType.Int32, applicationId);
        //        database.AddInParameter(dbCommand, "@SessionId", DbType.String, sessionId.Trim().ToUpper());
        //        database.AddInParameter(dbCommand, "@Tag", DbType.String, tag.Trim().ToUpper());
        //        database.AddInParameter(dbCommand, "@RecordLocator", DbType.String, recordLocator.Trim().ToUpper());
        //        database.AddInParameter(dbCommand, "@LastName", DbType.String, lastName.Trim().ToUpper());
        //        using (IDataReader dataReader = database.ExecuteReader(dbCommand))
        //        {
        //            while (dataReader.Read())
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
    }
}
