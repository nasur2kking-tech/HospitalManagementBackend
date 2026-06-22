using AutoMapper;
using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.DoctorSchedule;
using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Services
{
    public class DoctorScheduleService(
        IDoctorScheduleRepository repo,
        IMapper mapper) : IDoctorScheduleService
    {
        private readonly IDoctorScheduleRepository _repo = repo;
        private readonly IMapper _mapper = mapper;

        public async Task<PagedResult<DoctorScheduleResponseDto>> GetAllAsync(
            PaginationParams pagination,
            CancellationToken ct = default)
        {
            var (items, totalCount) = await _repo.GetAllAsync(
                pagination.PageNumber,
                pagination.PageSize,
                ct);

            return new PagedResult<DoctorScheduleResponseDto>
            {
                Items = _mapper.Map<IEnumerable<DoctorScheduleResponseDto>>(items),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<DoctorScheduleResponseDto> GetByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            var schedule = await _repo.GetByIdAsync(id, ct)
                ?? throw new ApplicationException("Doctor schedule not found.");

            return _mapper.Map<DoctorScheduleResponseDto>(schedule);
        }

        public async Task<IEnumerable<DoctorScheduleResponseDto>> GetByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default)
        {
            var schedules = await _repo.GetByDoctorIdAsync(doctorId, ct);

            return _mapper.Map<IEnumerable<DoctorScheduleResponseDto>>(schedules);
        }

        public async Task<DoctorScheduleResponseDto> CreateAsync(
            CreateDoctorScheduleDto dto,
            CancellationToken ct = default)
        {
            var schedule = _mapper.Map<DoctorSchedule>(dto);

            await _repo.AddAsync(schedule, ct);

            return _mapper.Map<DoctorScheduleResponseDto>(schedule);
        }

        public async Task UpdateAsync(
            int id,
            UpdateDoctorScheduleDto dto,
            CancellationToken ct = default)
        {
            var schedule = await _repo.GetByIdAsync(id, ct)
                ?? throw new ApplicationException("Doctor schedule not found.");

            schedule.AvailableDate = dto.AvailableDate;
            schedule.TimeSlot = dto.TimeSlot;

            await _repo.UpdateAsync(schedule, ct);
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken ct = default)
        {
            var schedule = await _repo.GetByIdAsync(id, ct)
                ?? throw new ApplicationException("Doctor schedule not found.");

            await _repo.DeleteAsync(schedule, ct);
        }

        public async Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _repo.ExistsAsync(id, ct);
        }
    }
}