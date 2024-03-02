using Dtos.AddressDto;
using Dtos.GeoDto;
using Dtos.MemberDto;
using MembersControlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IAddressRepository
    {
        Task<IEnumerable<AddressGetDto>> GetAllAddresses();
        Task<AddressGetDto> GetOneAddressByAddressId(int addressId);
        Task<AddressGetDto> AddAddress(AddressInsertDto addressInsertDto);
        Task<AddressGetDto> UpdateAddress(int addressId, AddressUpdateDto addressUpdateDto);
        public Task DeleteAddress(int addressId);
        AddressGetDto GetAddressRedis(int addressId);
        public void SaveAddressRedis(AddressGetDto addressDto);
        public void DeleteAddressRedis(int addressId);
    }
}
