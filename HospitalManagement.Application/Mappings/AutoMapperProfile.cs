using AutoMapper;
using HospitalManagement.Application.DTOs.Appointment;
using HospitalManagement.Application.DTOs.Billing;
using HospitalManagement.Application.DTOs.Doctor;
using HospitalManagement.Application.DTOs.Medical;
using HospitalManagement.Application.DTOs.Patient;
using HospitalManagement.Application.DTOs.User;
using HospitalManagement.Domain.Entities;

namespace HospitalManagement.Application.Mappings
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            // 🔹 USER
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role,
                    opt => opt.MapFrom(src => src.Role.ToString()));

            // 🔹 PATIENT
            CreateMap<Patient, PatientResponseDto>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name));

            CreateMap<CreatePatientDto, Patient>();

            CreateMap<UpdatePatientDto, Patient>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));

            // 🔹 DOCTOR
            CreateMap<Doctor, DoctorResponseDto>();
            CreateMap<CreateDoctorDto, Doctor>();

            // 🔹 APPOINTMENT
            CreateMap<Appointment, AppointmentResponseDto>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CreateAppointmentDto, Appointment>();

            CreateMap<UpdateAppointmentDto, Appointment>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));

            // 🔹 BILL
            CreateMap<Bill, BillResponseDto>()
                .ForMember(dest => dest.Status,
                    opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<CreateBillDto, Bill>();

            // 🔹 MEDICAL RECORD
            CreateMap<MedicalRecord, MedicalRecordDto>();
            CreateMap<CreateMedicalRecordDto, MedicalRecord>();
        }
    }
}