using API.Services;
using AutoMapper;
using System.Threading.Tasks;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public IUserRepository UserRepository => new UserRepository(_context, _mapper, _photoService);
        public ISearchRepository SearchRepository => new SearchRepository(_context, _mapper, _photoService);
        public IProductRepository ProductRepository => new ProductRepository(_context, _mapper, _photoService);
        public IOrderRepository OrdersRepository => new OrderRepository(_context, _mapper, _photoService);

        public UnitOfWork(DataContext dataContext, IMapper mapper, IPhotoService photoService)
        {
            _context = dataContext;
            _mapper = mapper;
            _photoService = photoService;
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
