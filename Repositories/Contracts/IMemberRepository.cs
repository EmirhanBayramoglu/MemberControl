using Dtos.AddressDto;
using Dtos.CompanyDto;
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
    public interface IMemberRepository
    {
        Task<IEnumerable<MemberGetDto>> GetAllMembers();
        Task<MemberGetDto> GetOneMemberByMemberId(int memberId);
        Task<MemberGetDto> AddMember(MemberInsertDto memberInserrtDto);
        Task<MemberGetDto> UpdateMember(int memberId,MemberUpdateDto memberUpdateDto);
        public Task DeleteMember(int memberId);
        public void SaveMemberRedis(MemberGetDto memberDto);
        MemberGetDto GetMemberRedis(int memberId);
        public void DeleteMemberRedis(int memberId);
    }
}
