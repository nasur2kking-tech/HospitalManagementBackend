using AutoMapper;
using HospitalManagement.Application.Common;
using HospitalManagement.Application.DTOs.Appointment;
using HospitalManagement.Application.Interfaces.Repositories;
using HospitalManagement.Application.Interfaces.Services;
using HospitalManagement.Domain.Entities;
using HospitalManagement.Domain.Enums;

namespace HospitalManagement.Application.Services
{
    public class AppointmentService(
    IAppointmentRepository repo,
    IMapper mapper)
    : IAppointmentService
    {
        private readonly IAppointmentRepository _repo = repo;
        private readonly IMapper _mapper = mapper;


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

        public async Task<AppointmentResponseDto> GetByIdAsync(
            int id,
            CancellationToken ct = default)
        {
            var appointment = await _repo.GetByIdAsync(id, ct)
                ?? throw new ApplicationException("Appointment not found.");

            return _mapper.Map<AppointmentResponseDto>(appointment);
        }

        public async Task<IEnumerable<AppointmentResponseDto>> GetByDoctorIdAsync(
            int doctorId,
            CancellationToken ct = default)
        {
            var appointments =
                await _repo.GetByDoctorIdAsync(
                    doctorId,
                    ct);

            return _mapper.Map<IEnumerable<AppointmentResponseDto>>(
                appointments);
        }

        public async Task<IEnumerable<AppointmentResponseDto>> GetByPatientIdAsync(
            int patientId,
            CancellationToken ct = default)
        {
            var appointments =
                await _repo.GetByPatientIdAsync(
                    patientId,
                    ct);

            return _mapper.Map<IEnumerable<AppointmentResponseDto>>(
                appointments);
        }

        public async Task<AppointmentResponseDto> CreateAsync(
            CreateAppointmentDto dto,
            CancellationToken ct = default)
        {
            var slotExists =
                await _repo.AppointmentSlotExistsAsync(
                    dto.DoctorId,
                    dto.AppointmentDate,
                    dto.TimeSlot,
                    ct);

            if (slotExists)
            {
                throw new ApplicationException(
                    "Doctor already has an appointment in this time slot.");
            }

            var appointment =
                _mapper.Map<Appointment>(dto);

            appointment.Status =
                AppointmentStatus.Scheduled;

            await _repo.AddAsync(
                appointment,
                ct);

            return _mapper.Map<AppointmentResponseDto>(
                appointment);
        }

        public async Task UpdateAsync(
            int id,
            UpdateAppointmentDto dto,
            CancellationToken ct = default)
        {
            var appointment =
                await _repo.GetByIdAsync(
                    id,
                    ct)
                ?? throw new ApplicationException(
                    "Appointment not found.");

            if (!Enum.TryParse(
                dto.Status,
                true,
                out AppointmentStatus status))
            {
                throw new ApplicationException(
                    "Invalid appointment status.");
            }

            var slotExists =
                await _repo.AppointmentSlotExistsAsync(
                    appointment.DoctorId,
                    dto.AppointmentDate,
                    dto.TimeSlot,
                    ct);

            bool changingSlot =
                appointment.AppointmentDate.Date !=
                    dto.AppointmentDate.Date
                ||
                appointment.TimeSlot !=
                    dto.TimeSlot;

            if (slotExists && changingSlot)
            {
                throw new ApplicationException(
                    "Doctor already has an appointment in this time slot.");
            }

            appointment.AppointmentDate =
                dto.AppointmentDate;

            appointment.TimeSlot =
                dto.TimeSlot;

            appointment.Status =
                status;

            await _repo.UpdateAsync(
                appointment,
                ct);
        }

        public async Task DeleteAsync(
            int id,
            CancellationToken ct = default)
        {
            var appointment =
                await _repo.GetByIdAsync(
                    id,
                    ct)
                ?? throw new ApplicationException(
                    "Appointment not found.");

            await _repo.SoftDeleteAsync(
                appointment,
                ct);
        }

        public async Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default)
        {
            return await _repo.ExistsAsync(
                id,
                ct);
        }
    }


}
