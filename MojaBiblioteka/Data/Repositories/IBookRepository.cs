using MojaBiblioteka.Utility;
using System.Collections.Generic;

namespace MojaBiblioteka.Data.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<BookModel> GetAll(int userId);
        BookModel Add(BookModel book);
        void Update(BookModel book);
        void Delete(int id, int userId);
    }
}
