using System.Collections.Generic;

namespace EF_Ladestrategien.Model
{
    public class Person
    {
        public int PersonID { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }

        public virtual Adresse Anschrift { get; set; }

        public virtual List<Telefonnummer> Telefonnummern { get; set; }

        public Person()
        {
            Telefonnummern = new List<Telefonnummer>();
        }

        public string AusgabeOhneAdresse()
        {
            return string.Format("{0} {1} ", Vorname, Nachname);
        }

        public string AusgabeMitAdresse()
        {
            return AusgabeOhneAdresse() + Anschrift.ToString();
        }

        public string AusgabeKomplett()
        {
            return AusgabeOhneAdresse() + Anschrift.ToString() + " Nummern: " + string.Join(", ", Telefonnummern);
        }
    }
}