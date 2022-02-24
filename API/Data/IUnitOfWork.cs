using System.Threading.Tasks;

namespace API.Data
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ISearchRepository SearchRepository { get; }
        IProductRepository ProductRepository { get; }
        IOrderRepository OrdersRepository { get; }
        IPayRepository PayRepository { get; }
        IStoreRepository StoreRepository { get; }
        ITrackRepository TrackRepository { get; }
        IRoleRepository RoleRepository { get; }
        Task<bool> SaveChanges();
        bool HasChanges();
    }
}
