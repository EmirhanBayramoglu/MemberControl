using AutoMapper;
using Dtos.AddressDto;
using Dtos.CompanyDto;
using Dtos.GeoDto;
using MembersControlSystem.ExceptionsClass;
using MembersControlSystem.Models;
using MembersControlSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repositories.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class GeoRepository : IGeoRepository
    {
        private readonly IMapper _mapper;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _cache;
        private readonly RepositoryContext _context;
        private TimeSpan ExpireTime => TimeSpan.FromDays(1);

        public GeoRepository(IMapper mapper, RepositoryContext context,IConnectionMultiplexer connectionMultiplexer)
        {
            _mapper = mapper;
            _connectionMultiplexer = connectionMultiplexer;
            _cache = _connectionMultiplexer.GetDatabase();
            _context = context;
        }

        public async Task<GeoGetDto> AddGeo(GeoInsertDto geoInsertDto)
        {
            if (geoInsertDto == null)
            {
                throw new ObjectIsNull("geoInsertDto");
            }
            var geo = _mapper.Map<Geo>(geoInsertDto);

            await _context.Geos.AddAsync(geo);
            await _context.SaveChangesAsync();

            return _mapper.Map<GeoGetDto>(geo);
        }

        public async Task DeleteGeo(int geoId)
        {
            var geoDto = await GetOneGeoByGeoId(geoId);

            var geo = _mapper.Map<Geo>(geoDto);

            if (geo == null)
                throw new ObjectIsNull("Geo");

            _context.Geos.Remove(geo);
            await _context.SaveChangesAsync();

            DeleteGeoRedis(geoId);
        }

        public async Task<IEnumerable<GeoGetDto>> GetAllGeos()
        {
            var geo = await _context.Geos.OrderBy(x => x.geoId).ToListAsync();

            return _mapper.Map<IEnumerable<GeoGetDto>>(geo);
        }

        public async Task<GeoGetDto> GetOneGeoByGeoId(int gId)
        {
            var geo = await _context.Geos.FirstOrDefaultAsync(x => x.geoId == gId);
            if (geo == null)
                throw new ObjectIsNull("Geo");

            var geoDto = _mapper.Map<GeoGetDto>(geo);

            SaveGeoRedis(geoDto);

            return geoDto;
        }

        public async Task<GeoGetDto> UpdateGeo(int geoid, GeoUpdateDto geoUpdateDto)
        {
           
            var geo = _mapper.Map<Geo>(geoUpdateDto);
            geo.geoId = geoid;

            _context.Geos.Update(geo);

            await _context.SaveChangesAsync();

            var geoDto = _mapper.Map<GeoGetDto>(geo);

            SaveGeoRedis(geoDto);

            return geoDto;
        }

        public void SaveGeoRedis(GeoGetDto geo)
        {
            var geoKey = $"Geo:{geo.geoId}";
            var geoJson = JsonConvert.SerializeObject(geo);
            _cache.StringSet(geoKey, geoJson);
        }

        public GeoGetDto GetGeoRedis(int geoId)
        {
            var geoKey = $"Geo:{geoId}";
            var geoJson = _cache.StringGet(geoKey);

            if (!geoJson.IsNull)
            {
                return JsonConvert.DeserializeObject<GeoGetDto>(geoJson);
            }

            return null;
        }

        public void DeleteGeoRedis(int geoId)
        {
            var memberKey = $"Geo:{geoId}";
            _cache.KeyDelete(memberKey);
        }

    }
}
