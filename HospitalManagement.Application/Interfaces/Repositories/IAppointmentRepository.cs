using System;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task<(IEnumerable<Appointment> Items, int TotalCount)> GetAllAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);

        Task<Appointment?> GetByIdAsync(
            int id,
            CancellationToken ct = default);

        Task<IEnumerable<Appointment>> GetByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default);

        Task<IEnumerable<Appointment>> GetByPatientIdAsync(
            int patientId,
            CancellationToken ct = default);

        Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default);

        Task<bool> AppointmentSlotExistsAsync(
            int doctorId,
            DateTime appointmentDate,
            string timeSlot,
            CancellationToken ct = default);

        Task AddAsync(
            Appointment appointment,
            CancellationToken ct = default);

        Task UpdateAsync(
            Appointment appointment,
            CancellationToken ct = default);

        Task SoftDeleteAsync(
            Appointment appointment,
            CancellationToken ct = default);
    }
}