using AutoMapper;
using MoneyTransfer.BLL.Dtos;
using MoneyTransfer.BLL.Dtos.Auth;
using MoneyTransfer.Data.Entites;

namespace MoneyTransfer.Presentation.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, RegisterDto>().ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<TransactionInformation, TransactionInformationDto>().ReverseMap();
            CreateMap<AppUser, SenderDetails>().ReverseMap();
        }
    }
}
