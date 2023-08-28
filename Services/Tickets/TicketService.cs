using Domain.Exceptions;
using Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence.Data;
using Shared.VirtualMachines;
using Persistence.Data.Extensions;

namespace Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly DataContext _dbContext;
        private readonly DbSet<Ticket> _tickets;
        private readonly DbSet<Customer> _customers;
        private readonly DbSet<VirtualMachine> _vms;

        public TicketService(DataContext dbContext)
        {
            this._dbContext = dbContext;
            this._tickets = dbContext.Tickets;
            this._customers = dbContext.Customers;
            this._vms = dbContext.VirtualMachines;
        }

        private IQueryable<Ticket> GetTicketById(int id) => _tickets
        .AsNoTracking()
        .Where(a => a.Id == id);

        private IQueryable<Customer> GetCustomerById(int id) => _customers
        .AsNoTracking()
        .Where(a => a.Id == id);

        private IQueryable<VirtualMachine> GetVirtualMachineById(int id) => _vms
        .AsNoTracking()
        .Where(a => a.Id == id);

        public async Task<TicketResult.Index> GetIndexAsync(TicketRequest.Index request)
        {
            var query = _tickets.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Searchterm))
            {
                query = query.Where(x => x.Title.Contains(request.Searchterm, StringComparison.OrdinalIgnoreCase));
            }

            int totalAmount = await query.CountAsync();

            var items = await query
               .Skip((request.Page - 1) * request.PageSize)
               .Take(request.PageSize)
                .OrderBy(x => x.Id)
               .Select(x => new TicketDto.Index
               {
                   Id = x.Id,
                   Title = x.Title,
                   CreatedAt = x.CreatedAt,
                   LastUpdatedAt = x.LastUpdatedAt,
                   Severity = x.Severity.ToString(),
                   State = x.State.ToString(),
                   Description = x.Description,
                   //VirtualMachineId = x.VirtualMachineId,
                   CustomerId = x.CustomerId
               }).ToListAsync();

            var result = new TicketResult.Index
            {
                Tickets = items,
                TotalAmount = totalAmount
            };
            return result;
        }

        public async Task<TicketResult.Detail> GetDetailAsync(TicketRequest.Detail request)
        {
            TicketResult.Detail result = new();

            result.Ticket = await GetTicketById(request.Id)
                .Select(x => new TicketDto.Detail
                {
                    Id = x.Id,
                    Title = x.Title,
                    CreatedAt = x.CreatedAt,
                    LastUpdatedAt = x.LastUpdatedAt,
                    Severity = x.Severity.ToString(),
                    State = x.State.ToString(),
                    Description = x.Description,
                    //VirtualMachineId = x.VirtualMachineId,
                    CustomerId = x.CustomerId
                }).SingleOrDefaultAsync();

            return result;
        }

        public async Task<TicketResult.Create> CreateAsync(TicketRequest.Create request)
        {
            TicketResult.Create result = new();
            var model = request.Ticket;

            //TODO: get customer by session id ! (model.customerId)
            //var customer = await GetCustomerById(1).SingleOrDefaultAsync();
            

            //var vm = await GetVirtualMachineById(model.VirtualMachineId).SingleOrDefaultAsync();
            var customer = await GetCustomerById(model.CustomerId).SingleOrDefaultAsync();

            Ticket ticket = new(
                model.Title,
                model.Description,
                //vm,
                model.Severity,
                DateTime.Now,
                customer.Id
            );

            _dbContext.Attach(customer);
            //_dbContext.Attach(vm);

            _tickets.Add(ticket);
            await _dbContext.SaveChangesAsync();
            result.Id = ticket.Id;

            return result;
        }

        public async Task<TicketResult.Edit> EditAsync(TicketRequest.Edit request)
        {
            TicketResult.Edit result = new();
            var model = request.Ticket;

            Ticket ticket = await GetTicketById(request.Id).SingleOrDefaultAsync();

            if (ticket is null)
                throw new EntityNotFoundException(nameof(Ticket), request.Id);

            TicketSeverity severity;
            TicketState state;
            Enum.TryParse(model.Severity, out severity);
            Enum.TryParse(model.State, out state);

            ticket.Title = model.Title;
            ticket.Description = model.Description;
            ticket.Severity = severity;
            ticket.State = state;
            ticket.LastUpdatedAt = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            result.Id = ticket.Id;

            return result;
        }

        public async Task DeleteAsync(TicketRequest.Delete request)
        {
            _tickets.RemoveIf(a => a.Id == request.Id);
            await _dbContext.SaveChangesAsync();
        }
    }
}
