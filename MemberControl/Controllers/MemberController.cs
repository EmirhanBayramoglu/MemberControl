using Dtos.AddressDto;
using Dtos.CompanyDto;
using Dtos.GeoDto;
using Dtos.MemberDto;
using MassTransit;
using MembersControlSystem.ExceptionClass.ActionsFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Repositories;
using Repositories.Config;
using Repositories.Contracts;
using System.Net;

namespace MembersControlSystem.Controllers
{
    [Route("api/member")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IGeoRepository _geoRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMessageHub _messageHub;
        private readonly IBusControl _bus;
        private readonly IEmailService _emailService;


        public MemberController(IMemberRepository memberRepository,ICompanyRepository companyRepository,
            IAddressRepository addressRepository, IGeoRepository geoRepository
            , IMessageHub messageHub, IEmailService emailService, IBusControl bus)
        {
            _memberRepository = memberRepository;
            _addressRepository = addressRepository;
            _geoRepository = geoRepository;
            _companyRepository = companyRepository;
            _messageHub = messageHub;
            _emailService = emailService;
            _bus = bus;
        }

        [HttpGet("email")]
        public async Task<ActionResult> Email(string to, string subject, string body)
        {
            await _emailService.SendEmailAsync(to, subject, body);

            return Ok();
        }


        [HttpGet("message")]   
        public async Task<ActionResult> Message(string userName)
        {
            await _messageHub.SendAddMessage(userName);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> GetAllMembers()
        {
            var items = await _memberRepository.GetAllMembers();

            return Ok(items);
        }

        [HttpGet("{memberId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<MemberGetDto>> GetOneMemberByMemberId(int memberId)
        {
            var item = await _memberRepository.GetOneMemberByMemberId(memberId);

            return Ok(item);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult<MemberGetDto>> Add([FromBody] MemberInsertDto memberInsertDto)
        {
            var memberDto = await _memberRepository.AddMember(memberInsertDto);

            await _messageHub.SendAddMessage(memberDto.userName);
            await _emailService.SendEmailAsync(memberDto.memberEmail, "Account Created", "Your Account Created.");

            return Ok(memberDto);
        }

        [HttpPut("updateMember/{memberId}")]
        [Authorize(Roles = "User")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> UpdateMember(int memberId,[FromBody] MemberUpdateDto memberUpdateDto)
        {
            var item = await _memberRepository.UpdateMember(memberId, memberUpdateDto);

            return Ok(item);
        }
        
        
        [HttpPut("updateAddress/{memberId}")]
        [Authorize(Roles = "User")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> UpdateAddress(int memberId, [FromBody] AddressUpdateDto addressUpdateDto)
        {
            var member = await _memberRepository.GetOneMemberByMemberId(memberId);

            var item = await _addressRepository.UpdateAddress(member.addressId, addressUpdateDto);

            return Ok(item);
        }

        [HttpPut("updateGeo/{memberId}")]
        [Authorize(Roles = "User")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> UpdateGeo(int memberId, [FromBody] GeoUpdateDto geoUpdateDto)
        {
            var member = await _memberRepository.GetOneMemberByMemberId(memberId);

            var item = await _geoRepository.UpdateGeo(member.address.geoId, geoUpdateDto);

            return Ok(item);
        }

        [HttpPut("updateCompany/{memberId}")]
        [Authorize(Roles = "User")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> UpdateCompany(int memberId, [FromBody] CompanyUpdateDto companyUpdateDto)
        {
            var member = await _memberRepository.GetOneMemberByMemberId(memberId);

            var item = await _companyRepository.UpdateCompany(member.CompanyId, companyUpdateDto);

            return Ok(item);
        }
        
        [HttpDelete("{memberId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> DeleteDoctor(int memberId)
        {
            await _memberRepository.DeleteMember(memberId);

            return Ok();
        }


    }
}
