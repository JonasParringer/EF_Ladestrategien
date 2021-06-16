namespace EF_Ladestrategien.Model
{
    public class Adresse
    {
        public int AdresseID { get; set; }
        public string Strasse { get; set; }
        public string Hausnummer { get; set; }
        public string PLZ { get; set; }
        public string Ort { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}, {2} {3}", Strasse, Hausnummer, PLZ, Ort);
        }
    }
}