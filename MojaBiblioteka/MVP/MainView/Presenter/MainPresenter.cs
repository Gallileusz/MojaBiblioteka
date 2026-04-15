using MojaBiblioteka.Data.Repositories;
using MojaBiblioteka.MVP.MainView.View;
using MojaBiblioteka.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MojaBiblioteka.MVP.MainView.Presenter
{
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IBookRepository _bookRepository;
        private List<BookModel> _allBooks = new List<BookModel>();

        public MainPresenter(IMainView view, IBookRepository bookRepository)
        {
            _view = view;
            _bookRepository = bookRepository;

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _view.ViewLoaded += OnViewLoaded;
            _view.AddBookClicked += OnAddBookClicked;
            _view.EditBookClicked += OnEditBookClicked;
            _view.DeleteBookClicked += OnDeleteBookClicked;
            _view.SearchTextChanged += OnSearchTextChanged;
            _view.BookDoubleClicked += OnBookDoubleClicked;
        }

        private void OnViewLoaded(object sender, EventArgs e) => ReloadBooks();

        private void OnAddBookClicked(object sender, EventArgs e)
        {
            try
            {
                var book = _view.ShowBookForm();
                if (book == null)
                {
                    return;
                }

                book.UserId = _view.CurrentUserId;
                _bookRepository.Add(book);
                ReloadBooks();
                _view.ShowMessage("Książka została dodana.", "Sukces");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Nie udało się dodać książki: {ex.Message}", "Błąd");
            }
        }

        private void OnEditBookClicked(object sender, EventArgs e) => EditSelectedBook();

        private void OnDeleteBookClicked(object sender, EventArgs e)
        {
            try
            {
                var selectedBook = _view.SelectedBook;
                if (selectedBook == null)
                {
                    _view.ShowError("Wybierz książkę do usunięcia.", "Brak wyboru");
                    return;
                }

                if (!_view.ConfirmAction($"Czy na pewno usunąć \"{selectedBook.Title}\"?", "Potwierdzenie"))
                {
                    return;
                }

                _bookRepository.Delete(selectedBook.Id, _view.CurrentUserId);
                ReloadBooks();
                _view.ShowMessage("Książka została usunięta.", "Sukces");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Nie udało się usunąć książki: {ex.Message}", "Błąd");
            }
        }

        private void OnSearchTextChanged(object sender, EventArgs e) => ApplyFilter();

        private void OnBookDoubleClicked(object sender, EventArgs e) => EditSelectedBook();

        private void EditSelectedBook()
        {
            try
            {
                var selectedBook = _view.SelectedBook;
                if (selectedBook == null)
                {
                    _view.ShowError("Wybierz książkę do edycji.", "Brak wyboru");
                    return;
                }

                var editedBook = _view.ShowBookForm(selectedBook);
                if (editedBook == null)
                {
                    return;
                }

                editedBook.Id = selectedBook.Id;
                editedBook.UserId = _view.CurrentUserId;
                _bookRepository.Update(editedBook);
                ReloadBooks();
                _view.ShowMessage("Książka została zaktualizowana.", "Sukces");
            }
            catch (Exception ex)
            {
                _view.ShowError($"Nie udało się zapisać zmian: {ex.Message}", "Błąd");
            }
        }

        private void ReloadBooks()
        {
            _allBooks = _bookRepository.GetAll(_view.CurrentUserId).ToList();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var query = (_view.SearchPhrase ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(query))
            {
                _view.SetBooks(_allBooks);
                return;
            }

            var filtered = _allBooks.Where(book =>
                Contains(book.Title, query) ||
                Contains(book.Author, query) ||
                Contains(book.Genre, query) ||
                Contains(book.Isbn, query))
                .ToList();

            _view.SetBooks(filtered);
        }

        private static bool Contains(string source, string value) =>
            !string.IsNullOrEmpty(source) &&
            source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
    }
}
