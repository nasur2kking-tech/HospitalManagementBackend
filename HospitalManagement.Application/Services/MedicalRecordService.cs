using AutoMapper;
using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Medical;
using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Services
{
    public class MedicalRecordService(
        IMedicalRecordRepository repo,
        IMapper mapper) : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _repo = repo;
        private readonly IMapper _mapper = mapper;

        public async Task<PagedResult<MedicalRecordDto>> GetAllAsync(
            PaginationParams pagination,
            CancellationToken ct = default)
        {
            var (items, totalCount) = await _repo.GetAllAsync(
                pagination.PageNumber,
                pagination.PageSize,
                ct);

            return new PagedResult<MedicalRecordDto>
            {
                Items = _mapper.Map<IEnumerable<MedicalRecordDto>>(items),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<MedicalRecordDto> GetByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            var record = await _repo.GetByIdAsync(id, ct)
                ?? throw new ApplicationException("Medical record not found.");

            return _mapper.Map<MedicalRecordDto>(record);
        }

        public async Task<MedicalRecordDto> CreateAsync(
            CreateMedicalRecordDto dto,
            CancellationToken ct = default)
        {
            var record = _mapper.Map<MedicalRecord>(dto);

            await _repo.AddAsync(record, ct);

            return _mapper.Map<MedicalRecordDto>(record);
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken ct = default)
        {
            var record = await _repo.GetByIdAsync(id, ct)
                ?? throw new ApplicationException("Medical record not found.");

            await _repo.DeleteAsync(record, ct);
        }

        public async Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _repo.ExistsAsync(id, ct);
        }
    }
}