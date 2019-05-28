using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DataServiceInterface.Models
{


    public interface IInterfaceParameterProvider
    {
        IDictionary<string, object> GetPropertyKeyValueList();
        void SetPropertyValue(string propertyName, object propertyValue);
    }


    
    public class CPUTipusInterfaceOutput : IInterfaceParameterProvider
    {
        delegate void SetValue(object value);

        private readonly Dictionary<string, SetValue> TulajdonsagokSet;
        public CPUTipusInterfaceOutput()
        {
            TulajdonsagokSet = new Dictionary<string, SetValue>();
            TulajdonsagokSet.Add(nameof(Id), (p) => { Id = (int)p; });
            TulajdonsagokSet.Add(nameof(Aktiv), (p) => { Aktiv = (string)p; });
            TulajdonsagokSet.Add(nameof(NyelvKod), (p) => { NyelvKod = (string)p; });
            TulajdonsagokSet.Add(nameof(CpuTipusNev), (p) => { CpuTipusNev = (string)p; });
            TulajdonsagokSet.Add(nameof(Sorrend), (p) => { Sorrend = (int)p; });
        }

        public void SetPropertyValue(string propertyName, object propertyValue)
        {
            SetValue setValue;

            if (TulajdonsagokSet.TryGetValue(propertyName, out setValue))
            {
                setValue(propertyValue);
            }
            else
            {
                throw new ArgumentException($"Property not found: {propertyName}");
            }
        }

        public IDictionary<string, object> GetPropertyKeyValueList()
        {

            IDictionary<string, object> Tulajdonsagok = new Dictionary<string, object>();
            Tulajdonsagok.Add(nameof(Id), Id);
            Tulajdonsagok.Add(nameof(Aktiv), Aktiv);
            Tulajdonsagok.Add(nameof(NyelvKod), NyelvKod);
            Tulajdonsagok.Add(nameof(CpuTipusNev), CpuTipusNev);
            Tulajdonsagok.Add(nameof(Sorrend), Sorrend);


            return Tulajdonsagok;
        }

        /// <summary>
        /// CPUTipusok adatbázisbeli technikai azonosítója. Rendszer tulajdonság.
        /// Értékkészlete : 1 .. N
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// CPUTipusok állaptotát tároló mező.
        /// Értékkészlete : "Y", "N"
        /// </summary>
        public string Aktiv { get; set; }
        
        /// <summary>
        /// CPUTipus lokalizációjához használt nyelvi kód. ISO formátumú.
        /// Értékkészlete : "hu-hu", "de-de"
        /// </summary>
        public string NyelvKod { get; set; }
        /// <summary>
        /// CPUTipus neve a korábban említett nyelv kód szerint.
        /// Értékkészlete : egy soros szöveg
        /// </summary>
        public string CpuTipusNev { get; set; }
        /// <summary>
        /// Megjelenítési sorrend.
        /// Értékkészlete : Pozitív egész számok
        /// </summary>
        public int Sorrend { get; set; }
       
    }

    /// <summary>
    /// CPUTipus webservice meghívázáshoz használható 
    /// kimeneti paramétereit tartalmazó osztály.
    /// </summary>
    public class CPUTipusInterfaceInput : IInterfaceParameterProvider
    {
        public void SetPropertyValue(string propertyName, object propertyValue)
        {
            throw new NotImplementedException("TODO");
        }

        public IDictionary<string, object> GetPropertyKeyValueList()
        {

            IDictionary<string, object> Tulajdonsagok = new Dictionary<string, object>();
            Tulajdonsagok.Add(nameof(Id), Id);
            Tulajdonsagok.Add(nameof(Aktiv), Aktiv);
            Tulajdonsagok.Add(nameof(NyelvKod), NyelvKod);
            

            return Tulajdonsagok;
        }


        /// <summary>
        /// Bemenő paraméter. Az példány adatbázisbeli technikai azonosítója. Rendszer tulajdonság.
        /// Értékkészlete : 1 és attól nagyobb számok.
        /// Ha nem adunk meg értéket, akkor ezen mezőt nem vesszük figyelembe a kimeneti adatok szűrésénél.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Bemenő paraméter. Aktív vagy inaktív állapotot lehet megadni.
        /// Értékkészlete : "Y", "N"
        /// Ha nem adunk meg értéket akkor az "Y"-nal egyenlő.
        /// </summary>
        [RegularExpression("[YN]", ErrorMessage = "Csak Y vagy N betű adható meg. ")]
        public string Aktiv { get; set; }
        /// <summary>
        /// Bemenő paraméter. Nyelvi megjelenítéseket tároló mező.
        /// Értékkészlete : "hu-hu", "de-de"
        /// Ha nem adunk meg értéket, akkor a "hu-hu" alapértelmezett érték kerül megadásra.
        /// Ezen mezőt nem vesszük figyelembe a kimeneti adatok szűrésénél.
        /// </summary>
        [RegularExpression("(hu-hu)?(de-de)?", ErrorMessage = "Csak hu-hu vagy de-de adható meg. ")]
        public string NyelvKod { get; set; } = "hu-hu";
      

    }
}
