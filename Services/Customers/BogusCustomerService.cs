using Bogus;
using Domain;
using Shared.Customers;
using Shared.Projects;

namespace Services.Customers;

public class BogusCustomerService : ICustomerService
{
    private readonly List<CustomerDto.Detail> _customers = new();

    public BogusCustomerService()
    {
        var departments = new[] { Department.DOG.ToString(), Department.DGZ.ToString(), Department.DBO.ToString(), Department.DLO.ToString(), Department.DGZ.ToString() };
        string[] educations = { "TI", "other education" };
        string[] externTypes = { "VOKA", "UNIZO" };

        var customers = new[] { new Customer("Jonas Verschueren", "jonas.ver@hotmail.com", "+32 494 58 04 73"), new Customer("Sean Van den Branden", "Sean.branden@hotmail.com", "+32 564 18 04 73") };

        var AccountFaker = new Faker<CustomerDto.Detail>("nl_BE")
            .RuleFor(a => a.Id, f => f.IndexFaker)
            .RuleFor(a => a.Name, f => f.Person.FullName)
            .RuleFor(a => a.Email, f => f.Person.Email)
            .RuleFor(a => a.PhoneNumber, f => f.Person.Phone)
            .RuleFor(a => a.Department, f => f.PickRandom(departments))
            .RuleFor(a => a.ExternType, f => f.PickRandom(externTypes))
            .RuleFor(a => a.Education, f => f.PickRandom(educations))
            .RuleFor(a => a.BackupContactId, f => 0);
        _customers = AccountFaker.Generate(26);
    }

    public async Task<CustomerResult.Index> GetIndexAsync(CustomerRequest.Index request)
    {
        await Task.Delay(200);

        List<CustomerDto.Index> customers = new();

        foreach (CustomerDto.Detail c in _customers)
        {
            CustomerDto.Index i = new()
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber
            };
            customers.Add(i);
        }

        var result = new CustomerResult.Index
        {
            Customers = customers,
            TotalAmount = _customers.Count()
        };

        return result;
    }
    public async Task<CustomerDto.Detail> GetDetailAsync(int customerId)
    {
        await Task.Delay(200);
        return _customers.First(c => c.Id == customerId);
    }

    public async Task<int> CreateAsync(CustomerDto.Create model)
    {
        Department department;
        Enum.TryParse(model.Department, out department);
        //Customer customer = new(model.Name, model.Email, model.PhoneNumber, department, model.ExternType, model.Education);
        _customers.Add(new()
        {
            Id = _customers.Count,
            Name = model.Name,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            ExternType = model.ExternType,
            Education = model.Education,
            Department = model.Department,
            BackupContactId = model.BackupContactId
        });
        return _customers.Count - 1;
    }

    public async Task EditAsync(int customerId, CustomerDto.Update model)
    {
        Department department;
        Enum.TryParse(model.Department, out department);
        //Customer customer = new(model.Name, model.Email, model.PhoneNumber, department, model.ExternType, model.Education);
        var customerToEdit = _customers.Where(c => c.Id == customerId);
        if(customerToEdit.Count() > 0)
        {
            var toEdit = customerToEdit.First();

            toEdit.Name= model.Name;
            toEdit.Email= model.Email;
            toEdit.PhoneNumber= model.PhoneNumber;
            toEdit.ExternType= model.ExternType;
            toEdit.Education = model.Education;
            toEdit.Department= model.Department;
            toEdit.BackupContactId = model.BackupContactId;

            _customers.Remove(toEdit);
            _customers.Add(toEdit);
        }
    }

    public async Task DeleteAsync(int customerId)
    {
        await Task.Delay(200);
        var accountToDelete = _customers.SingleOrDefault(x => x.Id == customerId);
        if (accountToDelete != null)
        {
            _customers.Remove(accountToDelete);
        }
    }

    public Task<CustomerResult.Detail> GetDetailAsync(CustomerRequest.Detail request)
    {
        throw new NotImplementedException();
    }

    public Task<CustomerResult.Create> CreateAsync(CustomerRequest.Create request)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(CustomerRequest.Delete request)
    {
        throw new NotImplementedException();
    }

    public Task<CustomerResult.Edit> EditAsync(CustomerRequest.Edit request)
    {
        throw new NotImplementedException();
    }

    public Task<ProjectResult.Index> GetProjectsFromCustomerAsync(CustomerRequest.Detail request)
    {
        throw new NotImplementedException();
    }
}
