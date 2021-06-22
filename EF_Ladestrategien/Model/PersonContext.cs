using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EF_Ladestrategien.Model
{
    internal class PersonContext : DbContext
    {
        ILoggerFactory loggerFactory;

        public PersonContext()
        {
            // install-package Microsoft.Extensions.Logging.Console
            loggerFactory = new LoggerFactory().AddConsole((categoryName, logLevel) => (logLevel == LogLevel.Information) && (categoryName == DbLoggerCategory.Database.Command.Name));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Der Connection String sollte besser in einer Konfigurationsdatei gespeichert werden

            // Ohne Unterstützung für Lazy Loading
            //optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=PersonDB;Integrated Security=True;").UseLoggerFactory(loggerFactory);

            // Mit Unterstützung für Lazy Loading
            //optionsBuilder.UseLazyLoadingProxies().UseSqlServer(@"Server=PC0373;Database=PersonDB;Integrated Security=True;").UseLoggerFactory(loggerFactory);
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(@"Server=PC0373;Database=PersonDB;Integrated Security=True;").UseLoggerFactory(loggerFactory);
        }

        public virtual DbSet<Person> Personen { get; set; }

        public virtual DbSet<Adresse> Adressen { get; set; }
    }
}