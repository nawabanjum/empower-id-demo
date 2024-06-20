using AutoMapper;
using EmpowerID.Application.Dtos;
using EmpowerID.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpowerID.CLI.Profiles
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
              CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}
