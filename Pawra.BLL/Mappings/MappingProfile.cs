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
        }
    }
}
