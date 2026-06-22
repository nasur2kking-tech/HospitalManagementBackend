using AutoMapper;
using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Patient;
using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Services
{
    public class PatientService(
        IPatientRepository repo,
        IUserRepository userRepo,
        IMapper mapper)
        : IPatientService
    {
        private readonly IPatientRepository _repo = repo;
        private readonly IUserRepository _userRepo = userRepo;
        private readonly IMapper _mapper = mapper;

        // =========================
        // GET ALL
        // =========================
        public async Task<PagedResult<PatientResponseDto>> GetAllAsync(
            PaginationParams pagination,
            CancellationToken ct = default)
        {
            if (pagination.PageNumber <= 0 || pagination.PageSize <= 0)
                throw new ApplicationException("Invalid pagination parameters.");

            var (items, totalCount) = await _repo.GetAllAsync(
                pagination.PageNumber,
                pagination.PageSize,
                ct);

            return new PagedResult<PatientResponseDto>
            {
                Items = _mapper.Map<IEnumerable<PatientResponseDto>>(items),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<PatientResponseDto> GetByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            if (id <= 0)
                throw new ApplicationException("Invalid patient ID.");

            var patient = await _repo.GetByIdAsync(id, ct);

            if (patient is null)
                throw new ApplicationException("Patient not found.");

            return _mapper.Map<PatientResponseDto>(patient);
        }

        // =========================
        // CREATE
        // =========================
        public async Task<PatientResponseDto> CreateAsync(
            CreatePatientDto dto,
            CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (dto.UserId <= 0)
                throw new ApplicationException("Invalid UserId.");

            var user = await _userRepo.GetByIdAsync(dto.UserId, ct);

            if (user is null)
                throw new ApplicationException("User not found.");

            var existingPatient = await _repo.GetByUserIdAsync(dto.UserId, ct);

            if (existingPatient is not null)
                throw new ApplicationException("Patient already exists for this user.");

            var patient = _mapper.Map<Patient>(dto);

            await _repo.AddAsync(patient, ct);

            return _mapper.Map<PatientResponseDto>(patient);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task UpdateAsync(
            int id,
            UpdatePatientDto dto,
            CancellationToken ct = default)
        {
            if (id <= 0)
                throw new ApplicationException("Invalid patient ID.");

            var patient = await _repo.GetByIdAsync(id, ct);

            if (patient is null)
                throw new ApplicationException("Patient not found.");

            _mapper.Map(dto, patient);

            await _repo.UpdateAsync(patient, ct);
        }

        // =========================
        // SOFT DELETE
        // =========================
        public async Task DeleteAsync(
            int id,
            CancellationToken ct = default)
        {
            if (id <= 0)
                throw new ApplicationException("Invalid patient ID.");

            var patient = await _repo.GetByIdAsync(id, ct);

            if (patient is null)
                throw new ApplicationException("Patient not found.");

            await _repo.SoftDeleteAsync(patient, ct);
        }

        // =========================
        // EXISTS
        // =========================
        public async Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default)
        {
            if (id <= 0)
                return false;

            return await _repo.ExistsAsync(id, ct);
        }
    }
}