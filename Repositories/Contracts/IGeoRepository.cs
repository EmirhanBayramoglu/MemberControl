using Dtos.GeoDto;
using MembersControlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IGeoRepository
    {
        Task<IEnumerable<GeoGetDto>> GetAllGeos();
        Task<GeoGetDto> GetOneGeoByGeoId(int geoId);
        Task<GeoGetDto> AddGeo(GeoInsertDto geoInsertDto);
        Task<GeoGetDto> UpdateGeo(int geoId, GeoUpdateDto geoUpdateDto);
        public Task DeleteGeo(int geoId);
        GeoGetDto GetGeoRedis(int geoId);
        public void SaveGeoRedis(GeoGetDto geo);
        public void DeleteGeoRedis(int geoId);
    }
}
