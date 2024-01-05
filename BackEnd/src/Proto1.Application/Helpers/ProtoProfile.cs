using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Proto1.Domain;
using Proto1.Domain.Identity;
using Proto1.Application.DTOs;

public class ProtoProfile : Profile
{
    public ProtoProfile()
    {
        CreateMap<User, UserCreateDto>().ReverseMap();   
        CreateMap<Pessoa, PessoaDto>().ReverseMap();
        CreateMap<User, UserData>().ReverseMap();
    }
}
