using Bogus;
using Domain;
using Shared.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Tickets
{
    public class BogusTicketService : ITicketService
    {
        private readonly List<TicketDto.Detail> _tickets = new();

        public BogusTicketService()
        {
            var customers = new[] { new Customer("Jonas Verschueren", "jonas.ver@hotmail.com", "+32 494 58 04 73"), new Customer("Sean Van den Branden", "Sean.branden@hotmail.com", "+32 564 18 04 73") };
            var vms = new[] {
                new VirtualMachine(),
                new VirtualMachine(),
                new VirtualMachine()
            };

            var severity = new[] { TicketSeverity.UNDEFINED, TicketSeverity.MINOR, TicketSeverity.MAJOR, TicketSeverity.CRITICAL };
            var state = new[] { TicketState.Done, TicketState.Started, TicketState.InProgress };

            var vmFaker = new Faker<TicketDto.Detail>("nl_BE")
                    .RuleFor(x => x.Id, f => f.IndexFaker)
                    .RuleFor(a => a.Title, f => f.Lorem.Sentence(5))
                    .RuleFor(a => a.CreatedAt, f => f.Date.Past())
                    .RuleFor(a => a.LastUpdatedAt, f => f.Date.Past())
                    .RuleFor(a => a.Severity, f => f.PickRandom(severity.ToString()))
                    .RuleFor(a => a.State, f => f.PickRandom(state.ToString()))
                    .RuleFor(a => a.Description, f => f.Lorem.Text());

            _tickets = vmFaker.Generate(25);
        }

        public Task<TicketResult.Index> GetIndexAsync(TicketRequest.Index request)
        {
            throw new NotImplementedException();
        }

        public Task<TicketDto.Detail> GetDetailAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateAsync(TicketDto.Create model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditAsync(int id, TicketDto.Update model)
        {
            throw new NotImplementedException();
        }

        public Task<TicketResult.Detail> GetDetailAsync(TicketRequest.Detail request)
        {
            throw new NotImplementedException();
        }

        public Task<TicketResult.Create> CreateAsync(TicketRequest.Create request)
        {
            throw new NotImplementedException();
        }

        public Task<TicketResult.Edit> EditAsync(TicketRequest.Edit request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TicketRequest.Delete request)
        {
            throw new NotImplementedException();
        }
    }
}
