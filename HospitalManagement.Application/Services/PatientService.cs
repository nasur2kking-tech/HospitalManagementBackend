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
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResult<PatientResponseDto>> GetAllAsync(PaginationParams pagination, CancellationToken ct = default)
        {
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

        public async Task<PatientResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var patient = await _repo.GetByIdAsync(id, ct);

            if (patient == null)
                throw new ApplicationException("Patient not found.");

            return _mapper.Map<PatientResponseDto>(patient);
        }

        public async Task<PatientResponseDto> CreateAsync(CreatePatientDto dto, CancellationToken ct = default)
        {
            var patient = _mapper.Map<Patient>(dto);

            await _repo.AddAsync(patient, ct);

            return _mapper.Map<PatientResponseDto>(patient);
        }

        public async Task UpdateAsync(int id, UpdatePatientDto dto, CancellationToken ct = default)
        {
            var patient = await _repo.GetByIdAsync(id, ct);

            if (patient == null)
                throw new ApplicationException("Patient not found.");

            _mapper.Map(dto, patient);

            await _repo.UpdateAsync(patient, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var patient = await _repo.GetByIdAsync(id, ct);

            if (patient == null)
                throw new ApplicationException("Patient not found.");

            await _repo.DeleteAsync(patient, ct);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return await _repo.ExistsAsync(id, ct);
        }
    }
}
