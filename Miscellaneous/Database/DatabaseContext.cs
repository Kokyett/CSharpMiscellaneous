using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Miscellaneous.Database {
    internal class DatabaseContext : DbContext {
        public virtual DbSet<Person> Persons => Set<Person>();

        private readonly IEnumerable<DbCommandInterceptor>? _interceptors;
        public DatabaseContext() {

        }

        public DatabaseContext(DbCommandInterceptor interceptor) {
            _interceptors = new DbCommandInterceptor[] { interceptor };
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=Resources\\database.db");
            if (_interceptors != null) {
                optionsBuilder.AddInterceptors(_interceptors);
            }
        }
    }
}
