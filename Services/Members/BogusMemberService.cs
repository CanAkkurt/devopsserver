using Bogus;
using Domain;
using Shared.Customers;
using Shared.Members;
using Shared.VirtualMachines;

namespace Services.Members;

public class BogusMemberService : IMemberService
{
    private readonly List<MemberDto.Detail> _members = new();

    public BogusMemberService()
    {
        var roles = new[] { MemberRole.Admin.ToString(), MemberRole.Manager.ToString(), MemberRole.Viewer.ToString() };
        var Departments = new[] { Department.DIT.ToString(), Department.DBO.ToString(),
            Department.DLO.ToString(), Department.DOG.ToString(), Department.DGZ.ToString() };

        var vmFaker = new Faker<MemberDto.Detail>("nl_BE")
                .RuleFor(x => x.Id, f => f.IndexFaker)
                .RuleFor(a => a.Name, f => f.Person.FullName)
                .RuleFor(a => a.Email, f => f.Person.Email)
                .RuleFor(a => a.PhoneNumber, f => f.Person.Phone)
                .RuleFor(a => a.Role, f => f.PickRandom(roles.ToString()))
                .RuleFor(a => a.Department, f => f.PickRandom(Departments));

        _members = vmFaker.Generate(25);
    }

    public async Task<MemberResult.Index> GetIndexAsync(MemberRequest.Index request)
    {
        await Task.Delay(200);

        List<MemberDto.Index> members = new();

        foreach (MemberDto.Detail c in _members)
        {
            MemberDto.Index i = new()
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Role = c.Role,
                Department = c.Department
            };
            members.Add(i);
        }

        var result = new MemberResult.Index
        {
            Members = members,
            TotalAmount = _members.Count()
        };

        return result;
    }

    public async Task<MemberDto.Detail> GetDetailAsync(int id)
    {
        await Task.Delay(200);
        return _members.First(c => c.Id == id);
    }

    public async Task DeleteAsync(int id)
    {
        await Task.Delay(200);
        var accountToDelete = _members.SingleOrDefault(x => x.Id == id);
        if (accountToDelete != null)
        {
            _members.Remove(accountToDelete);
        }
    }

    public Task<MemberResult.Detail> GetDetailAsync(MemberRequest.Detail request)
    {
        throw new NotImplementedException();
    }

    public Task<MemberResult.Create> CreateAsync(MemberRequest.Create request)
    {
        throw new NotImplementedException();
    }

    public Task<MemberResult.Edit> EditAsync(MemberRequest.Edit request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(MemberRequest.Delete request)
    {
        throw new NotImplementedException();
    }

    public Task<VirtualMachineResult.Index> GetVirtualMachinesFromMemberAsync(MemberRequest.Detail request)
    {
        throw new NotImplementedException();
    }
}
