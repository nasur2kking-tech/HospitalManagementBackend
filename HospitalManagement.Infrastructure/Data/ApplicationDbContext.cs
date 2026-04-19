using Microsoft.EntityFrameworkCore;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<Bill> Bills => Set<Bill>();
        public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
        public DbSet<DoctorSchedule> DoctorSchedules => Set<DoctorSchedule>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // =========================
            // 🔥 USER
            // =========================
            builder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(150)
                      .IsUnicode(false);

                entity.Property(u => u.PasswordHash)
                      .IsRequired();

                entity.HasIndex(u => u.Email)
                      .IsUnique();

                entity.Property(u => u.Role)
                      .HasConversion<string>();
            });

            // =========================
            // 🔥 PATIENT
            // =========================
            builder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Gender).HasMaxLength(10);
                entity.Property(p => p.Phone).HasMaxLength(15);
                entity.Property(p => p.Address).HasMaxLength(250);

                entity.HasIndex(p => p.UserId).IsUnique();

                entity.HasOne(p => p.User)
                      .WithOne(u => u.Patient)
                      .HasForeignKey<Patient>(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // 🔥 DOCTOR
            // =========================
            builder.Entity<Doctor>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Specialization).HasMaxLength(100);
                entity.Property(d => d.Qualification).HasMaxLength(200);
                entity.Property(d => d.Phone).HasMaxLength(15);

                entity.HasIndex(d => d.UserId).IsUnique();

                entity.HasOne(d => d.User)
                      .WithOne(u => u.Doctor)
                      .HasForeignKey<Doctor>(d => d.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // 🔥 APPOINTMENT
            // =========================
            builder.Entity<Appointment>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.TimeSlot).HasMaxLength(50);
                entity.Property(a => a.Status).HasConversion<string>();

                // ✅ Prevent double booking
                entity.HasIndex(a => new { a.DoctorId, a.AppointmentDate, a.TimeSlot })
                      .IsUnique();

                entity.HasOne(a => a.Patient)
                      .WithMany(p => p.Appointments)
                      .HasForeignKey(a => a.PatientId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Doctor)
                      .WithMany(d => d.Appointments)
                      .HasForeignKey(a => a.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // =========================
            // 🔥 BILL
            // =========================
            builder.Entity<Bill>(entity =>
            {
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Amount)
                      .HasPrecision(18, 2);

                entity.Property(b => b.Status)
                      .HasConversion<string>();

                entity.HasOne(b => b.Patient)
                      .WithMany(p => p.Bills)
                      .HasForeignKey(b => b.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // 🔥 MEDICAL RECORD
            // =========================
            builder.Entity<MedicalRecord>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Diagnosis).HasMaxLength(500);
                entity.Property(m => m.Prescription).HasMaxLength(500);

                entity.HasOne(m => m.Patient)
                      .WithMany(p => p.MedicalRecords)
                      .HasForeignKey(m => m.PatientId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================
            // 🔥 DOCTOR SCHEDULE
            // =========================
            builder.Entity<DoctorSchedule>(entity =>
            {
                entity.HasKey(ds => ds.Id);

                entity.Property(ds => ds.TimeSlot).HasMaxLength(50);

                // ✅ Prevent duplicate schedule slots
                entity.HasIndex(ds => new { ds.DoctorId, ds.AvailableDate, ds.TimeSlot })
                      .IsUnique();

                entity.HasOne(ds => ds.Doctor)
                      .WithMany(d => d.Schedules)
                      .HasForeignKey(ds => ds.DoctorId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}