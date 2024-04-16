using System.Data.Entity;
using System.Linq;
using MySql.Data.EntityFramework;

namespace backend.Data
{
    /// <summary>
    /// Database entity model map. 
    /// </summary>
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class IotDbContext : DbContext
    {
        /// <summary>
        /// Default class constructor
        /// </summary>
        /// <param name="connectionString">Database connection parameters</param>
        public IotDbContext(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Build indirect relationships.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        /// <summary>
        /// Application Register's data set.
        /// </summary>
        public virtual DbSet<Register> Registers { get; set; }

    }

}