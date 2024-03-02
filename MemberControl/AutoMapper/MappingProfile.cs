using AutoMapper;
using Dtos.AddressDto;
using Dtos.CompanyDto;
using Dtos.GeoDto;
using Dtos.MemberDto;
using Dtos.RegistrationDto;
using MembersControlSystem.Models;
using Models.Auth;

namespace MembersControlSystem.AutoMapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<AddressUpdateDto, Address>().ReverseMap();
            CreateMap<Address, AddressGetDto>().ReverseMap();
            CreateMap<AddressInsertDto, Address>();

            CreateMap<CompanyUpdateDto, Company>().ReverseMap();
            CreateMap<Company, CompanyGetDto>().ReverseMap();
            CreateMap<CompanyInsertDto, Company>();

            CreateMap<GeoUpdateDto, Geo>().ReverseMap();
            CreateMap<Geo, GeoGetDto>().ReverseMap();
            CreateMap<GeoInsertDto, Geo>();

            CreateMap<MemberUpdateDto, Member>().ReverseMap();
            CreateMap<Member, MemberGetDto>().ReverseMap();
            CreateMap<MemberInsertDto, Member>();

            CreateMap<UserForRegistration, User>();
        }
    }
}
