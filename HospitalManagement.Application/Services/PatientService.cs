using AutoMapper;
using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Patient;
using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public PatientService(
            IPatientRepository repo,
            IUserRepository userRepo,
            IMapper mapper)
        {
            _repo = repo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        // =========================
        // 🔹 GET ALL (PAGINATION)
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
        // 🔹 GET BY ID
        // =========================
        public async Task<PatientResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                throw new ApplicationException("Invalid patient ID.");

            var patient = await _repo.GetByIdAsync(id, ct);

            if (patient == null)
                throw new ApplicationException("Patient not found.");

            return _mapper.Map<PatientResponseDto>(patient);
        }

        // =========================
        // 🔹 CREATE
        // =========================
        public async Task<PatientResponseDto> CreateAsync(CreatePatientDto dto, CancellationToken ct = default)
        {
            if (dto == null)
                throw new ApplicationException("Invalid request.");

            if (dto.UserId <= 0)
                throw new ApplicationException("Invalid UserId.");

            // 🔥 Check user exists
            var user = await _userRepo.GetByIdAsync(dto.UserId, ct);
            if (user == null)
                throw new ApplicationException("User not found.");

            // 🔥 Prevent duplicate patient per user
            var existing = await _repo.GetByUserIdAsync(dto.UserId, ct);
            if (existing != null)
                throw new ApplicationException("Patient already exists for this user.");

            var patient = _mapper.Map<Patient>(dto);

            await _repo.AddAsync(patient, ct);

            return _mapper.Map<PatientResponseDto>(patient);
        }

        // =========================
        // 🔹 UPDATE
        // =========================
        public async Task UpdateAsync(int id, UpdatePatientDto dto, CancellationToken ct = default)
        {
            if (id <= 0)
                throw new ApplicationException("Invalid patient ID.");

            var patient = await _repo.GetByIdAsync(id, ct);

            if (patient == null)
                throw new ApplicationException("Patient not found.");

            _mapper.Map(dto, patient);

            await _repo.UpdateAsync(patient, ct);
        }

        // =========================
        // 🔹 DELETE
        // =========================
        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                throw new ApplicationException("Invalid patient ID.");

            var patient = await _repo.GetByIdAsync(id, ct);

            if (patient == null)
                throw new ApplicationException("Patient not found.");

            await _repo.DeleteAsync(patient, ct);
        }

        // =========================
        // 🔹 EXISTS
        // =========================
        public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            if (id <= 0)
                return false;

            return await _repo.ExistsAsync(id, ct);
        }
    }
}