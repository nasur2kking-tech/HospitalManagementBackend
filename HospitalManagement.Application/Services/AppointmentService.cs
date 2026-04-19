using AutoMapper;
using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Appointment;
using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly IMapper _mapper;

        public AppointmentService(IAppointmentRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<PagedResult<AppointmentResponseDto>> GetAllAsync(
            PaginationParams pagination,
            CancellationToken ct = default)
        {
            var (items, totalCount) = await _repo.GetAllAsync(
                pagination.PageNumber,
                pagination.PageSize,
                ct);

            return new PagedResult<AppointmentResponseDto>
            {
                Items = _mapper.Map<IEnumerable<AppointmentResponseDto>>(items),
                TotalCount = totalCount,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }

        public async Task<AppointmentResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var appointment = await _repo.GetByIdAsync(id, ct);

            if (appointment == null)
                throw new ApplicationException("Appointment not found.");

            return _mapper.Map<AppointmentResponseDto>(appointment);
        }

        public async Task<AppointmentResponseDto> CreateAsync(
            CreateAppointmentDto dto,
            CancellationToken ct = default)
        {
            var appointment = _mapper.Map<Appointment>(dto);

            appointment.Status = AppointmentStatus.Scheduled;

            await _repo.AddAsync(appointment, ct);

            return _mapper.Map<AppointmentResponseDto>(appointment);
        }

        public async Task UpdateAsync(
            int id,
            UpdateAppointmentDto dto,
            CancellationToken ct = default)
        {
            var appointment = await _repo.GetByIdAsync(id, ct);

            if (appointment == null)
                throw new ApplicationException("Appointment not found.");

            if (!Enum.TryParse<AppointmentStatus>(dto.Status, true, out var status))
                throw new ApplicationException("Invalid appointment status.");

            appointment.AppointmentDate = dto.AppointmentDate;
            appointment.TimeSlot = dto.TimeSlot;
            appointment.Status = status;

            await _repo.UpdateAsync(appointment, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var appointment = await _repo.GetByIdAsync(id, ct);

            if (appointment == null)
                throw new ApplicationException("Appointment not found.");

            await _repo.DeleteAsync(appointment, ct);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return await _repo.ExistsAsync(id, ct);
        }
    }
}
