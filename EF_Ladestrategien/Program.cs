using EF_Ladestrategien.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EF_Ladestrategien
{
    public class Program
    {
        private static void Main(string[] args)
        {
            /*
             * Benötigt die Pakete
             * Microsoft.EntityFrameworkCore
             * Microsoft.EntityFrameworkCore.SqlServer
             * Microsoft.EntityFrameworkCore.Proxies
             */

            using (PersonContext context = new PersonContext())
            {

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                //context.Database.EnsureDeleted();

                DatenAnlegen(context);

                //Load(context);

                //EagerLoading(context);

                LazyLoading(context);

                //ExplicitlyLoading(context);
            }

            Console.WriteLine("Taste zum Beenden");
            Console.ReadLine();
        }

        private static void DatenAnlegen(PersonContext context)
        {
            try
            {
                Adresse lennershof = new Adresse()
                {
                    Strasse = "Lennershofstrasse",
                    Hausnummer = "160",
                    PLZ = "44801",
                    Ort = "Bochum"
                };

                Adresse konradzuse = new Adresse()
                {
                    Strasse = "Konrad-Zuse-Strasse",
                    Hausnummer = "10",
                    PLZ = "44801",
                    Ort = "Bochum"
                };

                Adresse erkratherstr = new Adresse()
                {
                    Strasse = "Erkrather Strasse",
                    Hausnummer = "343",
                    PLZ = "40231",
                    Ort = "Düsseldorf"
                };

                Adresse konradzuseAlt = new Adresse()
                {
                    Strasse = "Konrad-Zuse-Strasse",
                    Hausnummer = "12",
                    PLZ = "44801",
                    Ort = "Bochum"
                };

                Person tim = new Person()
                {
                    Vorname = "Tim",
                    Nachname = "Meyer",
                    Anschrift = lennershof
                };

                Person tom = new Person()
                {
                    Vorname = "Tom",
                    Nachname = "Müller",
                    Anschrift = konradzuse
                };

                Person lisa = new Person()
                {
                    Vorname = "Lisa",
                    Nachname = "Schulz",
                    Anschrift = erkratherstr
                };

                Person frank = new Person()
                {
                    Vorname = "Frank",
                    Nachname = "Schuster",
                    Anschrift = erkratherstr
                };

                Random random = new Random();

                List<Telefonnummer> telefonnummern = new List<Telefonnummer>();

                for (int i = 0; i < 10; i++)
                {
                    telefonnummern.Add(
                        new Telefonnummer()
                        {
                            Nummer = random.Next().ToString(),
                            Typ = TelefonnummerTyp.Privat
                        });
                }


                for (int i = 0; i < 10; i++)
                {
                    telefonnummern.Add(
                        new Telefonnummer()
                        {
                            Nummer = random.Next().ToString(),
                            Typ = TelefonnummerTyp.Geschäftlich
                        });
                }

                tim.Telefonnummern.AddRange(telefonnummern.Take(2));
                tom.Telefonnummern.AddRange(telefonnummern.Skip(2).Take(2));
                lisa.Telefonnummern.AddRange(telefonnummern.Skip(4).Take(2));
                frank.Telefonnummern.AddRange(telefonnummern.Skip(6).Take(2));
                
                if (context.Personen.Count() == 0)
                {
                    context.Personen.Add(tim);
                    context.Personen.Add(tom);
                    context.Personen.Add(lisa);
                    context.Personen.Add(frank);
                }

                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void EagerLoading(PersonContext context)
        {
            Console.WriteLine("-- Eager Loading --");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Eager Loading durch das Include
            var p1 = (from p in context.Personen.Include(pers => pers.Anschrift).Include(pers => pers.Telefonnummern)
                      where p.Nachname == "Meyer"
                      select p).FirstOrDefault();

            Console.WriteLine(p1.AusgabeOhneAdresse());

            sw.Stop();

            Console.WriteLine("Dauer: {0} ms\n", sw.ElapsedMilliseconds);

            sw.Reset();
            sw.Start();

            Console.WriteLine(p1.AusgabeMitAdresse());

            sw.Stop();

            Console.WriteLine("Dauer: {0} ms\n", sw.ElapsedMilliseconds);

            sw.Reset();
            sw.Start();

            Console.WriteLine(p1.AusgabeKomplett());

            sw.Stop();

            Console.WriteLine("Dauer: {0} ms\n", sw.ElapsedMilliseconds);

            Console.WriteLine();
        }

        private static void LazyLoading(PersonContext ctx)
        {
            Console.WriteLine("-- Lazy Loading --");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var p1 = (from p in ctx.Personen
                      where p.Nachname == "Müller"
                      select p).FirstOrDefault<Person>();

            Console.WriteLine(p1.AusgabeOhneAdresse());

            sw.Stop();

            Console.WriteLine("Dauer: {0} ms\n", sw.ElapsedMilliseconds);

            sw.Reset();
            sw.Start();
            
            Console.WriteLine(p1.AusgabeMitAdresse());

            sw.Stop();

            Console.WriteLine("Dauer: {0} ms\n", sw.ElapsedMilliseconds);

            sw.Reset();
            sw.Start();

            Console.WriteLine(p1.AusgabeKomplett());

            sw.Stop();

            Console.WriteLine("Dauer: {0} ms\n", sw.ElapsedMilliseconds);

            Console.WriteLine();
        }

        private static void ExplicitlyLoading(PersonContext ctx)
        {
            Console.WriteLine("-- Explicitly Loading --");

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var p1 = (from p in ctx.Personen
                      where p.Nachname == "Schulz"
                      select p).FirstOrDefault();

            Console.WriteLine(p1.AusgabeOhneAdresse());

            sw.Stop();

            Console.WriteLine("Dauer: {0} ms", sw.ElapsedMilliseconds);

            sw.Reset();
            sw.Start();

            // Explicitly Loading durch den Aufruf von Entry() und Load()
            ctx.Entry(p1).Reference(pers => pers.Anschrift).Load();

            Console.WriteLine(p1.AusgabeMitAdresse());

            sw.Stop();

            Console.WriteLine("Dauer: {0}", sw.ElapsedMilliseconds);

            sw.Reset();
            sw.Start();

            // Explicitly Loading durch den Aufruf von Entry() und Load()
            ctx.Entry(p1).Collection(pers => pers.Telefonnummern).Load();

            Console.WriteLine(p1.AusgabeKomplett());

            sw.Stop();

            Console.WriteLine("Dauer: {0} ms\n", sw.ElapsedMilliseconds);

            Console.WriteLine();
        }

        private static void Load(PersonContext ctx)
        {
            var p1 = (from p in ctx.Personen
                      where p.Nachname == "Schuster"
                      select p).FirstOrDefault<Person>();
        }
    }
}