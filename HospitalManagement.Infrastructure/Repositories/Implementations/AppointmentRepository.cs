using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories.Implementations
{
    public class AppointmentRepository(ApplicationDbContext context)
        : IAppointmentRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<(IEnumerable<Appointment> Items, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .AsNoTracking();

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(a => a.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (items, totalCount);
        }

        public async Task<Appointment?> GetByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id, ct);
        }

        public async Task<IEnumerable<Appointment>> GetByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Include(a => a.Patient)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(
            int patientId,
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .Include(a => a.Doctor)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .AnyAsync(a => a.Id == id, ct);
        }

        // NEW METHOD
        public async Task<bool> AppointmentSlotExistsAsync(
            int doctorId,
            DateTime appointmentDate,
            string timeSlot,
            CancellationToken ct = default)
        {
            return await _context.Appointments
                .AnyAsync(a =>
                    a.DoctorId == doctorId &&
                    a.AppointmentDate.Date == appointmentDate.Date &&
                    a.TimeSlot == timeSlot,
                    ct);
        }

        public async Task AddAsync(
            Appointment appointment,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(appointment);

            await _context.Appointments.AddAsync(appointment, ct);

            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(
            Appointment appointment,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(appointment);

            _context.Appointments.Update(appointment);

            await _context.SaveChangesAsync(ct);
        }

        public async Task SoftDeleteAsync(
            Appointment appointment,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(appointment);

            appointment.IsDeleted = true;
            appointment.DeletedAt = DateTime.UtcNow;

            _context.Appointments.Update(appointment);

            await _context.SaveChangesAsync(ct);
        }
    }
}