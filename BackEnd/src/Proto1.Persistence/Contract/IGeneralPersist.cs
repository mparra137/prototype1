namespace Proto1.Persistence.Contract;

public interface IGeneralPersist
{
    void Insert<T>(T entity) where T: class;
    void Update<T>(T entity) where T: class;
    void Delete<T>(T entity) where T: class;
    void DeleteRange<T>(List<T> entities) where T: class;
    Task<bool> SaveChangesAsync();
}
