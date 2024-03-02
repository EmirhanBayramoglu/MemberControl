using AutoMapper;
using Dtos.AddressDto;
using Dtos.CompanyDto;
using Dtos.MemberDto;
using MembersControlSystem.ExceptionsClass;
using MembersControlSystem.Models;
using MembersControlSystem.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repositories.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IMapper _mapper;
        private readonly RepositoryContext _context;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _cache;
        private TimeSpan ExpireTime => TimeSpan.FromDays(1);

        public CompanyRepository(IMapper mapper, RepositoryContext context, IConnectionMultiplexer connectionMultiplexer)
        {
            _mapper = mapper;
            _context = context;
            _connectionMultiplexer = connectionMultiplexer;
            _cache = _connectionMultiplexer.GetDatabase();
        }

        public async Task<CompanyGetDto> AddCompany(CompanyInsertDto companyInsertDto)
        {
            if (companyInsertDto == null)
            {   
                    throw new ObjectIsNull("companyInsertDto");
            }
            var company = _mapper.Map<Company>(companyInsertDto);

            await _context.Companies.AddAsync(company);

            await _context.SaveChangesAsync();

            return _mapper.Map<CompanyGetDto>(company);
        }

        public async Task DeleteCompany(int companyId)
        {
            var companyDto = await GetOneCompanyByCompanyId(companyId);

            var company = _mapper.Map<Company>(companyDto);

            if (company == null)
                throw new ObjectIsNull("company");

            _context.Companies.Remove(company);

            await _context.SaveChangesAsync();

            DeleteCompanyRedis(companyId);
        }

        public async Task<IEnumerable<CompanyGetDto>> GetAllCompanies()
        {
            var company = await _context.Companies.OrderBy(x => x.companyId).ToListAsync();

            return _mapper.Map<IEnumerable<CompanyGetDto>>(company);
        }

        public async Task<CompanyGetDto> GetOneCompanyByCompanyId(int compId)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(x => x.companyId == compId);
            if (company == null)
                throw new ObjectNotFound("Cpmpany", Convert.ToString(compId));

            var companyDto = _mapper.Map<CompanyGetDto>(company);

            SaveCompanyRedis(companyDto);

            return companyDto;
        }

        public async Task<CompanyGetDto> UpdateCompany(int companyid,CompanyUpdateDto companyUpdateDto)
        {
            var company = _mapper.Map<Company>(companyUpdateDto);
            company.companyId = companyid;

            _context.Companies.Update(company);

            await _context.SaveChangesAsync();

            var companyDto = _mapper.Map<CompanyGetDto>(company);

            SaveCompanyRedis(companyDto);

            return companyDto;
        }

        public void SaveCompanyRedis(CompanyGetDto company)
        {
            var companyKey = $"Company:{company.companyId}";
            var companyJson = JsonConvert.SerializeObject(company);
            _cache.StringSet(companyKey, companyJson);
        }

        public CompanyGetDto GetCompanyRedis(int companyId)
        {
            var companyKey = $"Company:{companyId}";
            var companyJson = _cache.StringGet(companyKey);

            if (!companyJson.IsNull)
            {
                return JsonConvert.DeserializeObject<CompanyGetDto>(companyJson);
            }

            return null;
        }
        public void DeleteCompanyRedis(int companyId)
        {
            var memberKey = $"Company:{companyId}";
            _cache.KeyDelete(memberKey);
        }


    }
}