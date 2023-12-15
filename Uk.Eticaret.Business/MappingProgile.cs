using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uk.Eticaret.Business.Dtos.AddressDtos;
using Uk.Eticaret.Business.Dtos.CategoryDtos;
using Uk.Eticaret.Persistence.Entities;

namespace Uk.Eticaret.Business
{
    public class MappingProgile : Profile
    {
        protected MappingProgile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Category, CreateOrEditCategoryDto>();
            CreateMap<Category, ViewCategoryDto>();

            CreateMap<Address, ViewAdressDto>();
            CreateMap<Address, CreateOrEditAddressDto>();
            CreateMap<Address, AddressDto>();
        }
    }
}