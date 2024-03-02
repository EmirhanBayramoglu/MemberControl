using AutoMapper;
using Dtos.AddressDto;
using Dtos.GeoDto;
using Dtos.MemberDto;
using MembersControlSystem.ExceptionsClass;
using MembersControlSystem.Models;
using MembersControlSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Repositories.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IMapper _mapper;
        private readonly RepositoryContext _context;
        private readonly IGeoRepository _geoRepository;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _cache;
        private TimeSpan ExpireTime => TimeSpan.FromDays(1);

        public AddressRepository(IMapper mapper, RepositoryContext context
            ,IGeoRepository geoRepository
            ,IConnectionMultiplexer connectionMultiplexer)
        {
            _mapper = mapper;
            _context = context;
            _geoRepository = geoRepository;
            _connectionMultiplexer = connectionMultiplexer;
            _cache = _connectionMultiplexer.GetDatabase();
        }

        public async Task<AddressGetDto> AddAddress(AddressInsertDto addressInsertDto)
        {
            if (addressInsertDto == null)
            {
                throw new ObjectIsNull("addressInsertDto");
            }
            
            GeoGetDto geo = await _geoRepository.AddGeo(addressInsertDto.geoInsertDto);

            var address = _mapper.Map<Address>(addressInsertDto);

            address.geoId = geo.geoId;
            
            await _context.Addresses.AddAsync(address);

            await _context.SaveChangesAsync();

            var addressGetDto = _mapper.Map<AddressGetDto>(address);

            addressGetDto.geo = _mapper.Map<Geo>(geo);

            return addressGetDto;
        }

        public async Task DeleteAddress(int addressId)
        {
            var addressDto = await GetOneAddressByAddressId(addressId);

            var address = _mapper.Map<Address>(addressDto);

            if (address == null)
                throw new ObjectIsNull("Address");

            await _geoRepository.DeleteGeo(address.geoId);

            _context.Addresses.Remove(address);

            await _context.SaveChangesAsync();

            DeleteAddressRedis(addressId);
        }

        public async Task<IEnumerable<AddressGetDto>> GetAllAddresses()
        {
            var addresses = await _context.Addresses.OrderBy(x => x.addressId).ToListAsync();

            var geos = await _geoRepository.GetAllGeos();

            foreach (var address in addresses)
            {

                if (address.geo != null)
                {
                    var geoDto = geos.FirstOrDefault(geo => geo.geoId == address.geo.geoId);
                    address.geo = _mapper.Map<Geo>(geoDto);
                }
            }

            var addressDto = _mapper.Map<IEnumerable<AddressGetDto>>(addresses);

            return addressDto;
        }

        public async Task<AddressGetDto> GetOneAddressByAddressId(int addreId)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.addressId == addreId);
            if (address == null)
                throw new ObjectNotFound("Address", Convert.ToString(addreId));

            var addressDto = _mapper.Map<AddressGetDto>(address);

            SaveAddressRedis(addressDto);

            GeoGetDto geoDto = await _geoRepository.GetOneGeoByGeoId(address.geoId);
            addressDto.geo = _mapper.Map<Geo>(geoDto);
            return addressDto;
        }

        public async Task<AddressGetDto> UpdateAddress(int memberId, AddressUpdateDto addressUpdateDto)
        {


            Address existingAddress = _mapper.Map<Address>(addressUpdateDto);
            existingAddress.addressId = memberId;

            // ExistingAddress üzerinde güncelleme işlemlerini yap
            existingAddress.streetName = addressUpdateDto.streetName;
            existingAddress.suiteName = addressUpdateDto.suiteName;
            existingAddress.cityName = addressUpdateDto.cityName;
            existingAddress.zipcode = addressUpdateDto.zipcode;

            var addressDto = _mapper.Map<AddressGetDto>(existingAddress);

            SaveAddressRedis(addressDto);

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            // Güncellenmiş varlığı AddressGetDto'ya dönüştür ve geri döndür
            return addressDto;
        }

        public void SaveAddressRedis(AddressGetDto addressDto)
        {
            var addressKey = $"Address:{addressDto.addressId}";
            var addressJson = JsonConvert.SerializeObject(addressDto);
            _cache.StringSet(addressKey, addressJson);
        }

        public AddressGetDto GetAddressRedis(int addressId)
        {
            var addressKey = $"Address:{addressId}";
            var addressJson = _cache.StringGet(addressKey);

            if (!addressJson.IsNull)
            {
                return JsonConvert.DeserializeObject<AddressGetDto>(addressJson);
            }

            return null;
        }

        public void DeleteAddressRedis(int addressId)
        {
            var memberKey = $"Address:{addressId}";
            _cache.KeyDelete(memberKey);
        }

    }
}
