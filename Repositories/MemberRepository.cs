using AutoMapper;
using Dtos.AddressDto;
using Dtos.CompanyDto;
using Dtos.GeoDto;
using Dtos.MemberDto;
using MembersControlSystem.ExceptionsClass;
using MembersControlSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Auth;
using Newtonsoft.Json;
using Repositories;
using Repositories.Contracts;
using StackExchange.Redis;

namespace MembersControlSystem.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly IMapper _mapper;
        private readonly RepositoryContext _context;
        private readonly IAddressRepository _addressRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _cache;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IGeoRepository _geoRepository;

        private User? _user;

        private TimeSpan ExpireTime => TimeSpan.FromDays(1);


        public MemberRepository(IMapper mapper, RepositoryContext context,IAddressRepository addressRepository
            , ICompanyRepository companyRepository
            ,IConnectionMultiplexer connectionMultiplexer
            , UserManager<User> userManager
            , IAuthenticationService authenticationService
            , IGeoRepository geoRepository)
        {
            _mapper = mapper;
            _context = context;
            _addressRepository = addressRepository;
            _companyRepository = companyRepository;
            _connectionMultiplexer = connectionMultiplexer;
            _cache = _connectionMultiplexer.GetDatabase();
            _userManager = userManager;
            _authenticationService = authenticationService;
            _geoRepository = geoRepository;
        }

        public async Task<MemberGetDto> AddMember(MemberInsertDto memberInserrtDto)
        {
            if (memberInserrtDto == null)
            {
                throw new ObjectIsNull("memberInserrtDto");
            }
            
            CompanyGetDto company = await _companyRepository.AddCompany(memberInserrtDto.companyInsertDto);
            AddressGetDto address = await _addressRepository.AddAddress(memberInserrtDto.addressInsertDto);

            var member = _mapper.Map<Member>(memberInserrtDto);

            member.password = member.userName + "123";

            member.addressId = address.addressId;
            member.companyId = company.companyId;

            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();

            member.address = _mapper.Map<Address>(address);
            member.company = _mapper.Map<Company>(company);


            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string memberJson = JsonConvert.SerializeObject(member, settings);
            await _cache.StringSetAsync($"Member:{Convert.ToString(member.memberId)}", memberJson, ExpireTime);

            return _mapper.Map<MemberGetDto>(member);
        }

        public async Task DeleteMember(int memberid)
        {      

            var member = await _context.Members.FirstOrDefaultAsync(x => x.memberId == memberid);

            if (member == null)
                throw new ObjectIsNull("member");

            await _companyRepository.DeleteCompany(member.companyId);
            await _addressRepository.DeleteAddress(member.addressId);

            _context.Members.Remove(member);

            await _context.SaveChangesAsync();

            DeleteMemberRedis(memberid);

        }

        public async Task<IEnumerable<MemberGetDto>> GetAllMembers()
        {
            var members = await _context.Members.OrderBy(x => x.memberId).ToListAsync();
            
            var addresses = await _addressRepository.GetAllAddresses();
            var companies = await _companyRepository.GetAllCompanies();
            var membersDto = _mapper.Map<IEnumerable<MemberGetDto>>(members);

            foreach (var member in membersDto)
            {
                if (member.address != null && member.company != null)
                {
                    var addressDto = addresses.FirstOrDefault(addr => addr.addressId == member.address.addressId);

                    member.address = _mapper.Map<Address>(addressDto);

                    var companyDto = companies.FirstOrDefault(comp => comp.companyId == member.company.companyId);

                    member.company = _mapper.Map<Company>(companyDto);

                    SaveMemberRedis(member);

                    _companyRepository.SaveCompanyRedis(companyDto);
                    _addressRepository.SaveAddressRedis(addressDto);
                    _geoRepository.SaveGeoRedis(_mapper.Map<GeoGetDto>(addressDto.geo));
                }   
            }
            
            
            return membersDto ;
        }

        public async Task<MemberGetDto> GetOneMemberByMemberId(int membId)
        {
            Member member = null;

            var resultmember = GetMemberRedis(membId);

            if (resultmember != null)
            {
                member = _mapper.Map<Member>(resultmember);
                member.company = _mapper.Map<Company>(_companyRepository.GetCompanyRedis(membId));
                member.address = _mapper.Map<Address>(_addressRepository.GetAddressRedis(membId));
                member.address.geo = _mapper.Map<Geo>(_geoRepository.GetGeoRedis(membId));
                return _mapper.Map<MemberGetDto>(member);
            }
            else
            {
                member = await _context.Members.FirstOrDefaultAsync(x => x.memberId == membId);
                if (member == null)
                    throw new ObjectNotFound("Members",Convert.ToString(membId)); 
            }

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }; 

            var memberDto = _mapper.Map<MemberGetDto>(member);

            AddressGetDto addressDto = await _addressRepository.GetOneAddressByAddressId(member.addressId);
            memberDto.address = _mapper.Map<Address>(addressDto);

            CompanyGetDto companyDto = await _companyRepository.GetOneCompanyByCompanyId(member.companyId);
            memberDto.company = _mapper.Map<Company>(companyDto);

            SaveMemberRedis(memberDto);

            return memberDto;
        }

        public async Task<MemberGetDto> UpdateMember(int memberId, MemberUpdateDto memberUpdateDto)
        {

            var memberGetDto = await GetOneMemberByMemberId(memberId);
            if (memberGetDto == null)
                throw new ObjectNotFound("memberGetDto", "");

            if (memberUpdateDto == null)
            {
                throw new Exception("MemberUpdateDto is null.");
            }

            // Varlığı çek
            var existingMember = await _context.Members.FirstOrDefaultAsync(x => x.memberId == memberId);

            var oldPassword = existingMember.password;

            if (existingMember == null)
                throw new ObjectNotFound("existingMember", Convert.ToString(memberId));

            // Varlığı güncelle
            existingMember.memberName = memberUpdateDto.memberName;
            existingMember.memberEmail = memberUpdateDto.memberEmail;
            existingMember.memberPhoneNumber = memberUpdateDto.memberPhoneNumber;
            existingMember.webSite = memberUpdateDto.webSite;
            existingMember.password = memberUpdateDto.password;
            existingMember.address = memberGetDto.address;
            existingMember.company = memberGetDto.company;
            

            //authentication parolasını değiştirme
            _user = await _userManager.FindByNameAsync(memberGetDto.userName);
            if (_user != null)
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(_user, oldPassword, memberUpdateDto.password);
            }

            var memberDto = _mapper.Map<MemberGetDto>(existingMember);
            SaveMemberRedis(memberDto);

            // Değişiklikleri kaydet
            await _context.SaveChangesAsync();

            return memberDto;
        }

        public void SaveMemberRedis(MemberGetDto memberDto)
        {
            var memberKey = $"Member:{memberDto.memberId}";
            var memberJson = JsonConvert.SerializeObject(memberDto);
            _cache.StringSet(memberKey, memberJson);
        }

        public MemberGetDto GetMemberRedis(int memberId)
        {
            var memberKey = $"Member:{memberId}";
            var memberJson = _cache.StringGet(memberKey);

            if (!memberJson.IsNull)
            {
                return JsonConvert.DeserializeObject<MemberGetDto>(memberJson);
            }

            return null;
        }

        public void DeleteMemberRedis(int memberId)
        {
            var memberKey = $"Member:{memberId}";
            _cache.KeyDelete(memberKey);
        }

    }
}
