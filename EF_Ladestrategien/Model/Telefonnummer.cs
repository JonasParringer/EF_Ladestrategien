using System;

namespace EF_Ladestrategien.Model
{
    public enum TelefonnummerTyp
    {
        Privat,
        Geschäftlich
    }

    public class Telefonnummer
    {
        public int Id { get; set; }

        public string Nummer { get; set; }

        public TelefonnummerTyp Typ { get; set; }

        public DateTime Anlegedatum { get; set; }

        public Telefonnummer()
        {
            Anlegedatum = DateTime.Now;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Typ, Nummer);
        }
    }   
}
