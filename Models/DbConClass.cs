using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace ModelDbCon.Models
{
    //here you add the db conn string and db name with the help of override method OnConfiguring
    
    
    //inherit from dbContext class its a refrence represent a db session

    public class DbConClass:DbContext

    { //constructor
        public DbConClass():base()
            { 
        
        }

        //add the dbset which is the domain model class that represent the db tables

        public DbSet<Student> Student { get; set; }
        public DbSet<Department> Department { get; set; }

        public DbSet<Instructor> Instructor { get; set; }

        public DbSet<Course> Course { get; set; }


        //this is to specify which db server you're using and the db name and the auth type
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //pass the connection string, initial catalog is the db name u creating and integrated security is the auth true-> windows auth
            //false-> pass the username and pass if u choose auth in ur db

            optionsBuilder.UseSqlServer("Data Source=Atty;Initial Catalog=ITI_Db;Integrated Security=True");
        }

    }
}
