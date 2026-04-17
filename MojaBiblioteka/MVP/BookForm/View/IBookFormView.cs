using MojaBiblioteka.Utility;
using System;
using System.Collections.Generic;

namespace MojaBiblioteka.MVP.BookForm.View
{
    public interface IBookFormView
    {
        event EventHandler SaveClicked;
        event EventHandler CancelClicked;
        event EventHandler BrowseCoverClicked;

        string WindowTitleText { set; }
        string HeaderTitleText { set; }
        string TitleText { get; set; }
        string AuthorText { get; set; }
        string YearText { get; set; }
        string GenreText { get; set; }
        string DescriptionText { get; set; }
        string CoverImagePath { get; set; }
        byte[] CoverImageData { get; set; }

        BookModel ResultBook { get; set; }

        void SetGenres(IEnumerable<string> genres);
        string PickCoverPath();
        void ShowError(string message, string title);
        void CloseWithSuccess();
        void CloseWithCancel();
    }
}
