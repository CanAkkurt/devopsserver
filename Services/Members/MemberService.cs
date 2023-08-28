using Domain.Exceptions;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Shared.Members;
using Persistence.Data;
using Persistence.Data.Extensions;
using Shared.VirtualMachines;

namespace Services.Members
{
    public class MemberService : IMemberService
    {
        private readonly DataContext _dbContext;
        private readonly DbSet<Member> _members;


        public MemberService(DataContext dbContext)
        {
            this._dbContext = dbContext;
            this._members = dbContext.Members;
        }

        private IQueryable<Member> GetMemberById(int id) => _members
        .AsNoTracking()
        .Where(a => a.Id == id).Include(a => a.VirtualMachines);

        public async Task<MemberResult.Index> GetIndexAsync(MemberRequest.Index request)
        {
            var query = _dbContext.Members.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Searchterm))
            {
                query = query.Where(x => x.Name.Contains(request.Searchterm, StringComparison.OrdinalIgnoreCase));
            }

            int totalAmount = await query.CountAsync();

            var items = await query
               .Skip((request.Page - 1) * request.PageSize)
               .Take(request.PageSize)
               .OrderBy(x => x.Id)
               .Select(x => new MemberDto.Index
               {
                   Id = x.Id,
                   Name = x.Name,
                   Email = x.Email,
                   Role = x.Role.ToString(),
                   Department = x.Department.ToString()
               }).ToListAsync();

            var result = new MemberResult.Index
            {
                Members = items,
                TotalAmount = totalAmount
            };
            return result;
        }

        public async Task<MemberResult.Detail> GetDetailAsync(MemberRequest.Detail request)
        {
            MemberResult.Detail result = new();

            result.Member = await GetMemberById(request.Id)
                .Select(x => new MemberDto.Detail
            {
                Id = x.Id,
                Name = x.Name,
                Active = x.Active,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Role = x.Role.ToString(),
                Department = x.Department.ToString()

            }).SingleOrDefaultAsync();

            return result;
        }

        public async Task<VirtualMachineResult.Index> GetVirtualMachinesFromMemberAsync(MemberRequest.Detail request)
        {
            VirtualMachineResult.Index result = new();

            var virtualMachines = await GetMemberById(request.Id).Select(m => m.VirtualMachines).SingleOrDefaultAsync();

            result.VirtualMachines = virtualMachines.Select(vm => new VirtualMachineDto.Detail()
            {
                Id = vm.Id,
                Name = vm.Name,
                State = vm.State.ToString(),
                VCPUamount = vm.VCPUamount,
                MemoryAmount = vm.MemoryAmount,
                StorageAmount = vm.StorageAmount,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate
            });

            result.TotalAmount = result.VirtualMachines.Count();

            return result;
        }

        public async Task<MemberResult.Create> CreateAsync(MemberRequest.Create request)
        {

            MemberResult.Create result = new();
            var model = request.Member;

            if (await _members.AnyAsync(x => x.Email == model.Email))
                throw new EntityAlreadyExistsException(nameof(Member), nameof(Member.Email), model.Email);

            Department department;
            MemberRole role;
            Enum.TryParse(model.Department, out department);
            Enum.TryParse(model.Role, out role);

            Member member = new
            (
                model.Name!,
                true,//property "Active"
                model.Email!,
                model.PhoneNumber!,
                role,
                department
            );

            _dbContext.Members.Add(member);
            await _dbContext.SaveChangesAsync();
            result.Id = member.Id;

            return result;
        }

        public async Task<MemberResult.Edit> EditAsync(MemberRequest.Edit request)
        {
            MemberResult.Edit result = new();
            var model = request.Member;

            Member? member = await GetMemberById(request.Id).SingleOrDefaultAsync();

            if (member is null)
                throw new EntityNotFoundException(nameof(Member), request.Id);

            Department department;
            MemberRole role;
            Enum.TryParse(model.Department, out department);
            Enum.TryParse(model.Role, out role);

            member.Name = model.Name!;
            member.Active = model.Active;
            member.Email = model.Email!;
            member.Department = department;
            member.Role = role;
            member.PhoneNumber = model.PhoneNumber!;

            _dbContext.Update(member);
            await _dbContext.SaveChangesAsync();
            result.Id = member.Id;

            return result;
        }

        public async Task DeleteAsync(MemberRequest.Delete request)
        {
            _members.RemoveIf(a => a.Id == request.Id);
            await _dbContext.SaveChangesAsync();
        }
    }
}
