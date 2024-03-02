using AutoMapper.Configuration;
using Dtos.RegistrationDto;
using MembersControlSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Models.Auth;
using Newtonsoft.Json;
using System.Reflection;
using System.Xml;
using System.Linq;
using Repositories.Config;
using System.Reflection.Emit;

namespace MembersControlSystem.Repositories
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        protected readonly Microsoft.Extensions.Configuration.IConfiguration Configuration;

        public RepositoryContext(DbContextOptions opt) :
            base(opt)
        {
            
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Geo> Geos { get; set; }
        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Örnek ilişkiler
            if (modelBuilder == null)
                throw new ArgumentNullException("modelBuilder");

            modelBuilder.Entity<Member>().HasIndex(e => e.userName).IsUnique();

            modelBuilder.Entity<Geo>().HasData(GeoSeedData());
            modelBuilder.Entity<Address>().HasData(AddressSeedData());
            modelBuilder.Entity<Company>().HasData(CompanySeedData());
            modelBuilder.Entity<Member>().HasData(MembersSeedData());

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public List<Member> MembersSeedData()
        {
            var members = new List<Member>();
            using (StreamReader r = new StreamReader(@"users.json"))
            {
                string json = r.ReadToEnd();
                members = JsonConvert.DeserializeObject<List<Member>>(json);
            }


            return members;
        }

        public List<Geo> GeoSeedData()
        {
            var geo = new List<Geo>();
            using (StreamReader r = new StreamReader(@"geos.json"))
            {
                string json = r.ReadToEnd();
                geo = JsonConvert.DeserializeObject<List<Geo>>(json);
            }



            return geo;
        }

        public List<Address> AddressSeedData()
        {
            var address = new List<Address>();
            using (StreamReader r = new StreamReader(@"addresses.json"))
            {
                string json = r.ReadToEnd();
                address = JsonConvert.DeserializeObject<List<Address>>(json);
            }
            return address;
        }

        public List<Company> CompanySeedData()
        {
            var company = new List<Company>();
            using (StreamReader r = new StreamReader(@"companies.json"))
            {
                string json = r.ReadToEnd();
                company = JsonConvert.DeserializeObject<List<Company>>(json);
            }


            return company;
        }

    }
}