using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace TayaIT.Enterprise.EMadbatah.EParlimentService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IEPService
    {

        [OperationContract]
        string GetData(int value);

        //[OperationContract]
        //CompositeType GetDataUsingDataContract(CompositeType composite);

        //private static DataSet _epData = null;
        [OperationContract]
        DataSet GetEParlimentSessionDetails(long sessionID);

        [OperationContract]
        DataSet UpdateEParlimentSessionDetails(long sessionID);

        [OperationContract]
        bool IngestContentsForFinalApprove(long sessionID);

        [OperationContract]
        int CheckHealth(int id);


        //private static DataSet _epData = null;
        //[OperationContract]
        //DataSet TasdeekFinalApproveMadbatah(List<Model.sessio);
        

        // TODO: Add your service operations here
    }




    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}
