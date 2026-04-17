using MojaBiblioteka.Data.Repositories;
using MojaBiblioteka.MVP.BookCard.Presenter;
using MojaBiblioteka.MVP.MainView.Presenter;
using MojaBiblioteka.Utility;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BookCardControl = MojaBiblioteka.MVP.BookCard.View.BookCard;
using BookFormWindow = MojaBiblioteka.MVP.BookForm.View.BookForm;

namespace MojaBiblioteka.MVP.MainView.View
{
    public partial class MainView : Window, IMainView
    {
        private readonly MainPresenter _presenter;
        private readonly int _currentUserId;

        public event EventHandler ViewLoaded;
        public event EventHandler AddBookClicked;
        public event EventHandler EditBookClicked;
        public event EventHandler DeleteBookClicked;
        public event EventHandler SearchTextChanged;
        public event EventHandler BookDoubleClicked;

        public BookModel SelectedBook => (lvBooks.SelectedItem as BookCardControl)?.Book;
        public string SearchPhrase => txtSearch.Text;
        public int CurrentUserId => _currentUserId;

        public MainView(int currentUserId, string currentUserLogin)
        {
            _currentUserId = currentUserId;
            InitializeComponent();
            txtWelcome.Text = $"Witaj, {currentUserLogin}";

            var repository = new BookRepository();
            _presenter = new MainPresenter(this, repository);
        }

        public void SetBooks(IEnumerable<BookModel> books)
        {
            lvBooks.Items.Clear();
            foreach (var book in books)
            {
                var card = new BookCardControl
                {
                    Width = 170,
                    Height = 260,
                    Book = book
                };
                new BookCardPresenter(card, book);
                lvBooks.Items.Add(card);
            }
        }

        public BookModel ShowBookForm(BookModel bookToEdit = null)
        {
            var form = new BookFormWindow(bookToEdit) { Owner = this };
            var accepted = form.ShowDialog() == true;
            return accepted ? form.ResultBook : null;
        }

        public bool ConfirmAction(string message, string title) =>
            MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;

        public void ShowMessage(string message, string title) =>
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

        public void ShowError(string message, string title) =>
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);

        private void MainView_OnLoaded(object sender, RoutedEventArgs e) =>
            ViewLoaded?.Invoke(sender, EventArgs.Empty);

        private void btnDodaj_Click(object sender, RoutedEventArgs e) =>
            AddBookClicked?.Invoke(sender, EventArgs.Empty);

        private void btnEdytuj_Click(object sender, RoutedEventArgs e) =>
            EditBookClicked?.Invoke(sender, EventArgs.Empty);

        private void btnUsun_Click(object sender, RoutedEventArgs e) =>
            DeleteBookClicked?.Invoke(sender, EventArgs.Empty);

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e) =>
            SearchTextChanged?.Invoke(sender, EventArgs.Empty);

        private void lvBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e) =>
            BookDoubleClicked?.Invoke(sender, EventArgs.Empty);
    }
}
