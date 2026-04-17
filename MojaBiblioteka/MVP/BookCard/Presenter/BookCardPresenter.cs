using MojaBiblioteka.MVP.BookCard.View;
using MojaBiblioteka.Utility;

namespace MojaBiblioteka.MVP.BookCard.Presenter
{
    public class BookCardPresenter
    {
        public BookCardPresenter(IBookCardView view, BookModel book)
        {
            view.TitleText = book?.Title ?? string.Empty;
            view.AuthorText = book?.Author ?? string.Empty;
            view.SetCoverImage(book?.CoverImageData);
        }
    }
}
