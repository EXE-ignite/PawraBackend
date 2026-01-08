using AutoMapper;
using Pawra.BLL.DTOs;
using Pawra.DAL.Entities;

namespace Pawra.BLL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // AccountRole Mappings
            CreateMap<AccountRole, AccountRoleDto>();
            CreateMap<CreateAccountRoleDto, AccountRole>();
            CreateMap<UpdateAccountRoleDto, AccountRole>();

            // Account Mappings
            CreateMap<Account, LoginResponseDto>()
                .ForMember(dest => dest.Token, opt => opt.Ignore())
                .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<RegisterRequestDto, Account>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.RoleId, opt => opt.Ignore());

            // ===== Clinic Mappings =====
            CreateMap<Clinic, ClinicDto>();
            CreateMap<CreateClinicDto, Clinic>();
            CreateMap<UpdateClinicDto, Clinic>();

            // Customer Mappings
            CreateMap<Customer, CustomerDto>();
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<UpdateCustomerDto, Customer>();

            // Pet Mappings
            CreateMap<Pet, PetDto>();
            CreateMap<CreatePetDto, Pet>();
            CreateMap<UpdatePetDto, Pet>();

            // Vaccine Mappings
            CreateMap<Vaccine, VaccineDto>();
            CreateMap<CreateVaccineDto, Vaccine>();
            CreateMap<UpdateVaccineDto, Vaccine>();

            // Service Mappings
            CreateMap<Pawra.DAL.Entities.Service, ServiceDto>();
            CreateMap<CreateServiceDto, Pawra.DAL.Entities.Service>();
            CreateMap<UpdateServiceDto, Pawra.DAL.Entities.Service>();

            // PaymentMethod Mappings
            CreateMap<PaymentMethod, PaymentMethodDto>();
            CreateMap<CreatePaymentMethodDto, PaymentMethod>();
            CreateMap<UpdatePaymentMethodDto, PaymentMethod>();

            // SubscriptionPlan Mappings
            CreateMap<SubscriptionPlan, SubscriptionPlanDto>();
            CreateMap<CreateSubscriptionPlanDto, SubscriptionPlan>();
            CreateMap<UpdateSubscriptionPlanDto, SubscriptionPlan>();

            // Appointment Mappings
            CreateMap<Appointment, AppointmentDto>();
            CreateMap<CreateAppointmentDto, Appointment>();
            CreateMap<UpdateAppointmentDto, Appointment>();

            // Veterinarian Mappings
            CreateMap<Veterinarian, VeterinarianDto>();
            CreateMap<CreateVeterinarianDto, Veterinarian>();
            CreateMap<UpdateVeterinarianDto, Veterinarian>();

            // ClinicManager Mappings
            CreateMap<ClinicManager, ClinicManagerDto>();
            CreateMap<CreateClinicManagerDto, ClinicManager>();
            CreateMap<UpdateClinicManagerDto, ClinicManager>();

            // ClinicService Mappings
            CreateMap<ClinicService, ClinicServiceDto>();
            CreateMap<CreateClinicServiceDto, ClinicService>();
            CreateMap<UpdateClinicServiceDto, ClinicService>();

            // ClinicVaccine Mappings
            CreateMap<ClinicVaccine, ClinicVaccineDto>();
            CreateMap<CreateClinicVaccineDto, ClinicVaccine>();
            CreateMap<UpdateClinicVaccineDto, ClinicVaccine>();

            // Payment Mappings
            CreateMap<Payment, PaymentDto>();
            CreateMap<CreatePaymentDto, Payment>();
            CreateMap<UpdatePaymentDto, Payment>();

            // Prescription Mappings
            CreateMap<Prescription, PrescriptionDto>();
            CreateMap<CreatePrescriptionDto, Prescription>();
            CreateMap<UpdatePrescriptionDto, Prescription>();

            // SubscriptionAccount Mappings
            CreateMap<SubscriptionAccount, SubscriptionAccountDto>();
            CreateMap<CreateSubscriptionAccountDto, SubscriptionAccount>();
            CreateMap<UpdateSubscriptionAccountDto, SubscriptionAccount>();

            // VaccinationRecord Mappings
            CreateMap<VaccinationRecord, VaccinationRecordDto>();
            CreateMap<CreateVaccinationRecordDto, VaccinationRecord>();
            CreateMap<UpdateVaccinationRecordDto, VaccinationRecord>();
        }
    }
}
