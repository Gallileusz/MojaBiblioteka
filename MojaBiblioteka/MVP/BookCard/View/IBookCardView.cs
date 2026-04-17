namespace MojaBiblioteka.MVP.BookCard.View
{
    public interface IBookCardView
    {
        string TitleText { set; }
        string AuthorText { set; }
        void SetCoverImage(byte[] coverImageData);
    }
}
