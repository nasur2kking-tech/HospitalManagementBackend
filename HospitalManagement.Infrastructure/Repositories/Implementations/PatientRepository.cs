using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Infrastructure.Repositories.Implementations;

public class PatientRepository(ApplicationDbContext context) : IPatientRepository
{
    public async Task<(IEnumerable<Patient> Patients, int TotalCount)> GetAllAsync(
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
    {
        var query = context.Patients
            .Include(p => p.User)
            .AsNoTracking();

        var totalCount = await query.CountAsync(ct);

        var patients = await query
            .OrderByDescending(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (patients, totalCount);
    }

    public async Task<Patient?> GetByIdAsync(
        int id,
        CancellationToken ct = default)
    {
        return await context.Patients
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<Patient?> GetByUserIdAsync(
        int userId,
        CancellationToken ct = default)
    {
        return await context.Patients
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);
    }

    public async Task<bool> ExistsAsync(
        int id,
        CancellationToken ct = default)
    {
        return await context.Patients
            .AnyAsync(p => p.Id == id, ct);
    }

    public async Task AddAsync(
        Patient patient,
        CancellationToken ct = default)
    {
        await context.Patients.AddAsync(patient, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(
        Patient patient,
        CancellationToken ct = default)
    {
        context.Patients.Update(patient);
        await context.SaveChangesAsync(ct);
    }

    public async Task SoftDeleteAsync(
        Patient patient,
        CancellationToken ct = default)
    {
        patient.IsDeleted = true;
        patient.DeletedAt = DateTime.UtcNow;

        context.Patients.Update(patient);

        await context.SaveChangesAsync(ct);
    }
}