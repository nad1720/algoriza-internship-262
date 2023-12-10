using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VezeetaDomainLayer.Models;


namespace VezeetaRepositoryLayer
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
       
       
        private static void SeedRole(ModelBuilder modelBuilder) {
            modelBuilder.Entity<IdentityRole<int>>().HasData(
             new IdentityRole<int> { Id = 1, Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
             new IdentityRole<int> { Id = 2, Name = "Patient", ConcurrencyStamp = "2", NormalizedName = "Patient" },
             new IdentityRole<int> { Id = 3, Name = "Doctor", ConcurrencyStamp = "3", NormalizedName = "Doctor" }
            );
     
        
        }
        private static void SeedAdminUser(ModelBuilder modelBuilder)
        {
            var admin = new Admin
            {
                Id= 1,
                UserName = "lolotamaga.com@gmail.com",
                Email = "lolotamaga.com@gmail.com",
            };


            admin.PasswordHash = "P@$sw0rd";

            modelBuilder.Entity<Admin>().HasData(admin);
        }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedRole(modelBuilder);
            SeedAdminUser(modelBuilder);
           
            

            modelBuilder.Entity<Doctor>()
            .HasMany(d => d.Appointments)
            .WithOne(a => a.Doctor)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Appointment>()
              .HasMany(a => a.Requests)
              .WithOne(r => r.Appointment)
              .HasForeignKey(r => r.AppointmentId)
              .OnDelete(DeleteBehavior.NoAction);  

            modelBuilder.Entity<Request>()
            .HasOne(r => r.TimeSlot)
            .WithMany()
            .HasForeignKey(r => r.TimeSlotId)
            .OnDelete(DeleteBehavior.Restrict);

           

            modelBuilder.Entity<Request>()
             .HasOne(r => r.Appointment)
             .WithMany(a => a.Requests)
             .HasForeignKey(r => r.AppointmentId)
             .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
                .HasMany(a => a.Times)
                .WithOne(t => t.Appointment)
                .HasForeignKey(t => t.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TimeSlot>()
                .HasMany(ts => ts.Requests)
                .WithOne(booking => booking.TimeSlot)
                .HasForeignKey(booking => booking.TimeSlotId)
                .OnDelete(DeleteBehavior.Cascade); 



           
        }
    }
    
}
