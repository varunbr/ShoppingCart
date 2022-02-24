using API.Entities;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly UserManager<User> _userManager;

        public IUserRepository UserRepository => new UserRepository(_context, _mapper, _photoService);
        public ISearchRepository SearchRepository => new SearchRepository(_context, _mapper, _photoService);
        public IProductRepository ProductRepository => new ProductRepository(_context, _mapper, _photoService);
        public IOrderRepository OrdersRepository => new OrderRepository(_context, _mapper, _photoService);
        public IPayRepository PayRepository => new PayRepository(_context, _mapper, _photoService);
        public IStoreRepository StoreRepository => new StoreRepository(_context, _mapper, _photoService);
        public ITrackRepository TrackRepository => new TrackRepository(_context, _mapper, _photoService);
        public IRoleRepository RoleRepository => new RoleRepository(_context, _mapper, _photoService, _userManager);

        public UnitOfWork(DataContext dataContext, IMapper mapper, IPhotoService photoService, UserManager<User> userManager)
        {
            _context = dataContext;
            _mapper = mapper;
            _photoService = photoService;
            _userManager = userManager;
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}
