using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataServiceInterface.Models
{


    
    public class DataAccessLayer<T, K> where T : IInterfaceParameterProvider, new() where K : IInterfaceParameterProvider
    {

        private string DataBaseCommand;
        private const string DataBaseConnectionString = "GV_DBConnectionString_GV";

        public DataAccessLayer(string DBCommand)
        {
            DataBaseCommand = DBCommand;
        }

        
        public List<T> GetModelListFromDataBase(K inputInterfaceProperties)
        {


            if (inputInterfaceProperties == null)
            {
                throw new ArgumentNullException("inputInterfaceProperties");
            }
            else
            {

                List<T> result = new List<T>();

                string connString = string.Empty;
                try
                {
                    connString = ConfigurationManager.ConnectionStrings[DataBaseConnectionString].ConnectionString;
                }
                catch (ConfigurationErrorsException)
                {

                    throw new HttpResponseException
                    (
                        new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {

                            Content = new StringContent("Error-" +
                            System.Configuration.ConfigurationManager.AppSettings["GV_MicroService-ErrorCode-MainName_GV"] +
                            "-04 : " +
                            "Az adatbázis nem elérhető kérem vegye fel a kapcsolatot az üzemeltetőkkel." +
                            "Hivatkozzon a csatolt hibaüzenetre, és adja meg nekik a híváshoz használt URL-t."
                            ),
                            ReasonPhrase = "Belső működési rendellenesség"
                        }
                    );

                }




                using (SqlConnection dbConn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(DataBaseCommand, dbConn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        IDictionary<string, object> InputTulajdonsagLista = inputInterfaceProperties.GetPropertyKeyValueList();

                        foreach (var TulajdonsagElem in InputTulajdonsagLista)
                        {
                            cmd.Parameters.AddWithValue(TulajdonsagElem.Key, TulajdonsagElem.Value);
                        }



                        dbConn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {

                                T OutputObject = new T();

                                IDictionary<string, object> OutputTulajdonsagLista = OutputObject.GetPropertyKeyValueList();

                                foreach (var TulajdonsagElem in OutputTulajdonsagLista)
                                {
                                    OutputObject.SetPropertyValue(TulajdonsagElem.Key, reader.GetValue(reader.GetOrdinal(TulajdonsagElem.Key)));
                                }

                                result.Add(OutputObject);

                            }
                        }
                    }
                }



                return result;
            }
        }
    }
};