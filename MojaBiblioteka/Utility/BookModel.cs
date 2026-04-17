using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MojaBiblioteka.Utility
{
    public class BookModel : INotifyPropertyChanged
    {
        private int _id;
        private int _userId;
        private string _title;
        private string _author;
        private int _year;
        private string _genre;
        private string _description;
        private string _coverImagePath;
        private byte[] _coverImageData;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public int UserId
        {
            get => _userId;
            set { _userId = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string Author
        {
            get => _author;
            set { _author = value; OnPropertyChanged(); }
        }

        public int Year
        {
            get => _year;
            set { _year = value; OnPropertyChanged(); }
        }

        public string Genre
        {
            get => _genre;
            set { _genre = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        public string CoverImagePath
        {
            get => _coverImagePath;
            set { _coverImagePath = value; OnPropertyChanged(); }
        }

        public byte[] CoverImageData
        {
            get => _coverImageData;
            set { _coverImageData = value; OnPropertyChanged(); }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public BookModel Clone() => new BookModel
        {
            Id = Id,
            UserId = UserId,
            Title = Title,
            Author = Author,
            Year = Year,
            Genre = Genre,
            Description = Description,
            CoverImagePath = CoverImagePath,
            CoverImageData = CoverImageData
        };

        public void CopyFrom(BookModel source)
        {
            UserId = source.UserId;
            Title = source.Title;
            Author = source.Author;
            Year = source.Year;
            Genre = source.Genre;
            Description = source.Description;
            CoverImagePath = source.CoverImagePath;
            CoverImageData = source.CoverImageData;
        }
    }
}
