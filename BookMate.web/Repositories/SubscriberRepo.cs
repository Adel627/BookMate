using BookMate.web.Core.Models;
using BookMate.web.Data;
using BookMate.web.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookMate.web.Repositories
{
    public class SubscriberRepo : GenericRepo<Subscriber>, ISubscriberRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public SubscriberRepo(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Subscriber?> GetSubscriber(string searchTerm)
        {
            var subscriber = await _context.Subscribers.Include(s => s.Subscriptions)
                .Include(s => s.Governorate).Include(s => s.Area)
                .SingleOrDefaultAsync(s => s.NationalId == searchTerm || s.MobileNumber == searchTerm || s.Email == searchTerm );
            return subscriber;
        }
        public DateTime RenewSubscription(int subscriberId , string UserId)
        {
            var subscriber = _context.Subscribers
                                        .Include(s => s.Subscriptions)
                                        .SingleOrDefault(s => s.Id == subscriberId);

          
            var lastSubscription = subscriber!.Subscriptions.Last();

            var startDate = lastSubscription.EndDate < DateTime.Today
                            ? DateTime.Today
                            : lastSubscription.EndDate.AddDays(1);

            Subscription newSubscription = new()
            {
                CreatedById = UserId,
                CreatedOn = DateTime.Now,
                StartDate = startDate,
                EndDate = startDate.AddYears(1)
            };

            subscriber.Subscriptions.Add(newSubscription);

            _context.SaveChanges();
            return newSubscription.EndDate;
          
        }
        public async Task< SubscriberFormViewModel> PopulateViewModel(SubscriberFormViewModel? model)
        {
            var governorates = await _context.Governorates.ToListAsync();
            if (model == null) 
             model = new SubscriberFormViewModel();
            
            model.Governorates = _mapper.Map<IEnumerable<SelectListItem>>(governorates);
            return model;
        }
        public async Task<List<Area>> GetGovernorateAreas(int id)
        {
            List <Area> areas = await _context.Areas
                .Where(a => a.GovernorateId == id).ToListAsync(); 
            return areas;
        }
        public bool AllowNationalId(string NationalId)
        {
            var subscriber = _context.Subscribers.SingleOrDefault(b => b.NationalId == NationalId);
            return subscriber is null;
        }

        public bool AllowMobileNumber(string MobileNumber)
        {
            var subscriber = _context.Subscribers.SingleOrDefault(b => b.MobileNumber == MobileNumber);
            return subscriber is null;
        }

        public bool AllowEmail(string Email)
        {
            
            var subscriber = _context.Subscribers.SingleOrDefault(b => b.Email == Email);
            return subscriber is null;

        }


    }
}
