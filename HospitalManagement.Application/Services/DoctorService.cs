using AutoMapper;
using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Doctor;
using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Services
{
    public class DoctorService(
        IDoctorRepository repo,
        IMapper mapper) : IDoctorService
    {
        private readonly IDoctorRepository _repo = repo;
        private readonly IMapper _mapper = mapper;

        public async Task<PagedResult<DoctorResponseDto>> GetAllAsync(
            PaginationParams pagination,
            CancellationToken ct = default)
        {
            var (items, totalCount) = await _repo.GetAllAsync(
                pagination.PageNumber,
                pagination.PageSize,
                ct);

            return new PagedResult<DoctorResponseDto>
            {
                Items = _mapper.Map<IEnumerable<DoctorResponseDto>>(items),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<DoctorResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var doctor = await _repo.GetByIdAsync(id, ct)
                         ?? throw new ApplicationException("Doctor not found.");

            return _mapper.Map<DoctorResponseDto>(doctor);
        }

        public async Task<DoctorResponseDto> CreateAsync(
            CreateDoctorDto dto,
            CancellationToken ct = default)
        {
            var existing = await _repo.GetByUserIdAsync(dto.UserId, ct);

            if (existing is not null)
                throw new ApplicationException("Doctor profile already exists for this user.");

            var doctor = _mapper.Map<Doctor>(dto);

            await _repo.AddAsync(doctor, ct);

            return _mapper.Map<DoctorResponseDto>(doctor);
        }

        // ✅ FIXED METHOD SIGNATURE
        public async Task UpdateAsync(
            int id,
            UpdateDoctorDto dto,
            CancellationToken ct = default)
        {
            var doctor = await _repo.GetByIdAsync(id, ct)
                         ?? throw new ApplicationException("Doctor not found.");

            _mapper.Map(dto, doctor);

            await _repo.UpdateAsync(doctor, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var doctor = await _repo.GetByIdAsync(id, ct)
                         ?? throw new ApplicationException("Doctor not found.");

            await _repo.DeleteAsync(doctor, ct);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return await _repo.ExistsAsync(id, ct);
        }
    }
}