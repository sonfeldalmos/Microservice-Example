using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using DataServiceInterface.Models;
using System.Web.Http.ModelBinding;


namespace DataServiceInterface.Controllers.Functions
{
    public class MicroserviceOutputControllerFunctions<T> where T : IInterfaceParameterProvider
    {
        public static string GetErrorTextOutputIsEmpty(T InterfaceInputData)
        {
            StringBuilder SzovegEpito = new StringBuilder();
            SzovegEpito.Clear();
            SzovegEpito.Append("\r\n");
            foreach (var TulajdonsagElem in InterfaceInputData.GetType().GetProperties())
            {
                if (TulajdonsagElem.GetIndexParameters().Length == 0)
                {
                    SzovegEpito.Append(TulajdonsagElem.Name);
                    SzovegEpito.Append(" (");
                    SzovegEpito.Append(TulajdonsagElem.PropertyType.Name);
                    SzovegEpito.Append("): ");
                    SzovegEpito.Append(TulajdonsagElem.GetValue(InterfaceInputData));
                    SzovegEpito.Append("\r\n");
                }
                else
                {
                    SzovegEpito.Append(TulajdonsagElem.Name);
                    SzovegEpito.Append(" (");
                    SzovegEpito.Append(TulajdonsagElem.PropertyType.Name);
                    SzovegEpito.Append("): <Indexed>\r\n");
                }

            }
            return SzovegEpito.ToString();
        }

    }


    /// <summary>
    /// Microservice  kontroller osztály. A webservice hívást innen vezéreljük.
    /// </summary>
    public class MicroserviceOutputController<Out, In> where Out : IInterfaceParameterProvider, new() where In : IInterfaceParameterProvider
    {

        /// <summary>
        /// Az adott microservicet visszaadó webservice.
        /// </summary>
        /// <param name="InterfaceInputData"></param>
        /// <returns>MicroserviceInterfaceOutput objektumok listája</returns>
        public List<Out> Post
            (In InterfaceInputData, string DataBaseCommand)
        {
            if (InterfaceInputData == null)
            {
                throw new HttpResponseException
                (
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Error-" +
                            System.Configuration.ConfigurationManager.AppSettings["GV_MicroService-ErrorCode-MainName_GV"] +
                            "-03 : Bemenő paraméternek null érkezett. \r\n" +
                            "MicroServiceInterfaceInput == null "),
                        ReasonPhrase = "Belső működési rendellenesség"
                    }
                );
            }
            else
            {
                StringBuilder SzovegEpito = new StringBuilder();
               


                var DataLayer = new DataAccessLayer<Out, In>
                    (DataBaseCommand);

                List<Out> InterfaceOutputData;

                InterfaceOutputData = DataLayer.GetModelListFromDataBase(InterfaceInputData);


                if (!InterfaceOutputData.Any())
                {
                    string ErrorTextOutputIsEmpty = MicroserviceOutputControllerFunctions<In>.GetErrorTextOutputIsEmpty(InterfaceInputData);
                    throw new HttpResponseException
                    (
                        new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            Content = new StringContent("Error-" +
                                System.Configuration.ConfigurationManager.AppSettings["GV_MicroService-ErrorCode-MainName_GV"] +
                                "-02 : " +
                                "Nincs a szűrésnek megfelelő adat. " +
                                ErrorTextOutputIsEmpty
                                ),
                            ReasonPhrase = "Belső működési rendellenesség"
                        }
                    );

                }
                else
                {
                    return InterfaceOutputData;
                }
              

            }
        }
    }

}