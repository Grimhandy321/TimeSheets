namespace TimeSheets.Database
{
    public interface ISqlRepository<T>
    {
        IEnumerable<T> GetAll(); 
        T? GetById(int id);
        int Insert(T entity);
        void Update(T entity);
        void Delete(int id);

        void InsertList(IEnumerable<T> entities);
    }

}
