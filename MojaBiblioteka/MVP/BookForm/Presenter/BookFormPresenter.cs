using MojaBiblioteka.MVP.BookForm.View;
using MojaBiblioteka.Utility;
using System;
using System.IO;

namespace MojaBiblioteka.MVP.BookForm.Presenter
{
    public class BookFormPresenter
    {
        private static readonly string[] Genres = new[]
        {
            "Kryminał",
            "Fantastyka",
            "Sci-Fi",
            "Horror",
            "Romans",
            "Historia",
            "Biografia",
            "Poradnik",
            "Przygodowe",
            "Inne"
        };

        private readonly IBookFormView _view;
        private readonly BookModel _book;

        public BookFormPresenter(IBookFormView view, BookModel bookToEdit = null)
        {
            _view = view;
            _book = bookToEdit?.Clone() ?? new BookModel();

            _view.SaveClicked += OnSaveClicked;
            _view.CancelClicked += OnCancelClicked;
            _view.BrowseCoverClicked += OnBrowseCoverClicked;

            InitializeView(bookToEdit == null);
        }

        private void InitializeView(bool isAddMode)
        {
            var formTitle = isAddMode ? "Dodaj książkę" : "Edytuj książkę";
            _view.WindowTitleText = formTitle;
            _view.HeaderTitleText = formTitle;
            _view.SetGenres(Genres);

            _view.TitleText = _book.Title ?? string.Empty;
            _view.AuthorText = _book.Author ?? string.Empty;
            _view.YearText = _book.Year > 0 ? _book.Year.ToString() : string.Empty;
            _view.GenreText = _book.Genre ?? string.Empty;
            _view.DescriptionText = _book.Description ?? string.Empty;
            _view.CoverImagePath = _book.CoverImagePath ?? string.Empty;
            _view.CoverImageData = _book.CoverImageData;
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_view.TitleText) || string.IsNullOrWhiteSpace(_view.AuthorText))
            {
                _view.ShowError("Tytuł i autor są wymagane.", "Brakujące dane");
                return;
            }

            if (!TryResolveCover())
            {
                return;
            }

            _book.Title = _view.TitleText?.Trim();
            _book.Author = _view.AuthorText?.Trim();
            _book.Year = TryParseYear(_view.YearText);
            _book.Genre = string.IsNullOrWhiteSpace(_view.GenreText) ? null : _view.GenreText.Trim();
            _book.Description = string.IsNullOrWhiteSpace(_view.DescriptionText) ? null : _view.DescriptionText.Trim();
            _book.CoverImagePath = string.IsNullOrWhiteSpace(_view.CoverImagePath) ? null : _view.CoverImagePath.Trim();
            _book.CoverImageData = _view.CoverImageData;

            _view.ResultBook = _book;
            _view.CloseWithSuccess();
        }

        private void OnCancelClicked(object sender, EventArgs e) => _view.CloseWithCancel();

        private void OnBrowseCoverClicked(object sender, EventArgs e)
        {
            var path = _view.PickCoverPath();
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            try
            {
                _view.CoverImagePath = path;
                _view.CoverImageData = File.ReadAllBytes(path);
            }
            catch
            {
                _view.ShowError("Nie udało się odczytać pliku okładki.", "Błąd pliku");
            }
        }

        private bool TryResolveCover()
        {
            var path = _view.CoverImagePath;
            if (string.IsNullOrWhiteSpace(path))
            {
                return true;
            }

            try
            {
                if (!File.Exists(path))
                {
                    _view.ShowError("Wybrana ścieżka okładki nie istnieje.", "Błąd pliku");
                    return false;
                }

                _view.CoverImageData = File.ReadAllBytes(path);
                return true;
            }
            catch
            {
                _view.ShowError("Nie udało się odczytać pliku okładki.", "Błąd pliku");
                return false;
            }
        }

        private static int TryParseYear(string yearText)
        {
            return int.TryParse(yearText, out var year) && year > 0 ? year : 0;
        }
    }
}
