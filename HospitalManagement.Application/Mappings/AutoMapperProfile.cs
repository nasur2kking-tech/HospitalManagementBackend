using AutoMapper;
using HospitalManagement.Domain.Entities;

using HospitalManagement.Application.DTOs.Patient;
using HospitalManagement.Application.DTOs.Doctor;
using HospitalManagement.Application.DTOs.Appointment;
using HospitalManagement.Application.DTOs.Billing;
using HospitalManagement.Application.DTOs.Medical;
using HospitalManagement.Application.DTOs.User;
using HospitalManagement.Application.DTOs.DoctorSchedule;

namespace HospitalManagement.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // =========================
            // PATIENT
            // =========================
            CreateMap<Patient, PatientResponseDto>();
            CreateMap<CreatePatientDto, Patient>();
            CreateMap<UpdatePatientDto, Patient>();


            // =========================
            // DOCTOR
            // =========================
            CreateMap<Doctor, DoctorResponseDto>();
            CreateMap<CreateDoctorDto, Doctor>();
            CreateMap<UpdateDoctorDto, Doctor>();


            // =========================
            // APPOINTMENT
            // =========================
            CreateMap<Appointment, AppointmentResponseDto>();
            CreateMap<CreateAppointmentDto, Appointment>();
            CreateMap<UpdateAppointmentDto, Appointment>();


            // =========================
            // BILLING
            // =========================
            CreateMap<Bill, BillResponseDto>();
            CreateMap<CreateBillDto, Bill>();


            // =========================
            // MEDICAL RECORD
            // =========================
            CreateMap<MedicalRecord, MedicalRecordDto>();
            CreateMap<CreateMedicalRecordDto, MedicalRecord>();


            // =========================
            // DOCTOR SCHEDULE
            // =========================
            CreateMap<DoctorSchedule, DoctorScheduleResponseDto>();
            CreateMap<CreateDoctorScheduleDto, DoctorSchedule>();
            CreateMap<UpdateDoctorScheduleDto, DoctorSchedule>();


            // =========================
            // USER
            // =========================
            CreateMap<User, UserDto>();

            // Uncomment these if they exist
            // CreateMap<CreateUserDto, User>();
            // CreateMap<UpdateUserDto, User>();
            // CreateMap<User, UserResponseDto>();
        }
    }
}