using MojaBiblioteka.Utility;
using System;
using System.Collections.Generic;

namespace MojaBiblioteka.MVP.MainView.View
{
    public interface IMainView
    {
        event EventHandler ViewLoaded;
        event EventHandler AddBookClicked;
        event EventHandler EditBookClicked;
        event EventHandler DeleteBookClicked;
        event EventHandler SearchTextChanged;
        event EventHandler BookDoubleClicked;

        BookModel SelectedBook { get; }
        string SearchPhrase { get; }
        int CurrentUserId { get; }

        void SetBooks(IEnumerable<BookModel> books);
        BookModel ShowBookForm(BookModel bookToEdit = null);
        bool ConfirmAction(string message, string title);
        void ShowMessage(string message, string title);
        void ShowError(string message, string title);
    }
}
