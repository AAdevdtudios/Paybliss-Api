using AutoMapper;
using Paybliss.Models;
using Paybliss.Models.Dto;
using Paybliss.Models.HttpResp;

namespace Paybliss
{
    public class AuthoMapperProfiler: Profile
    {
        public AuthoMapperProfiler()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<User, RegisterDto>();
            CreateMap<RegisterDto, User>();
            CreateMap<CablesDto, CableValues>();
            CreateMap<CableValues, CablesDto>();
        }
    }
}
