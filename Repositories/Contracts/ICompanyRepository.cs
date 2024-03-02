using Dtos.CompanyDto;
using MembersControlSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<CompanyGetDto>> GetAllCompanies();
        Task<CompanyGetDto> GetOneCompanyByCompanyId(int companyId);
        Task<CompanyGetDto> AddCompany(CompanyInsertDto companyInsertDto);
        Task<CompanyGetDto> UpdateCompany(int companyid,CompanyUpdateDto companyUpdatetDto);
        public Task DeleteCompany(int companyId);
        CompanyGetDto GetCompanyRedis(int companyId);
        public void SaveCompanyRedis(CompanyGetDto company);
        public void DeleteCompanyRedis(int companyId);
    }
}
