using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MojaBiblioteka.Data.Repositories;
using MojaBiblioteka.MVP.MainView.Presenter;
using MojaBiblioteka.Utility;
using BookFormWindow = MojaBiblioteka.MVP.BookForm.View.BookForm;

namespace MojaBiblioteka.MVP.MainView.View
{
    /// <summary>
    /// Logika interakcji dla klasy MainView.xaml
    /// </summary>
    public partial class MainView : Window, IMainView
    {
        private readonly MainPresenter _presenter;
        private readonly int _currentUserId;
        public ObservableCollection<BookModel> Books { get; } = new ObservableCollection<BookModel>();

        public event EventHandler ViewLoaded;
        public event EventHandler AddBookClicked;
        public event EventHandler EditBookClicked;
        public event EventHandler DeleteBookClicked;
        public event EventHandler SearchTextChanged;
        public event EventHandler BookDoubleClicked;

        public BookModel SelectedBook => lvBooks.SelectedItem as BookModel;
        public string SearchPhrase => txtSearch.Text;
        public int CurrentUserId => _currentUserId;

        public MainView(int currentUserId, string currentUserLogin)
        {
            _currentUserId = currentUserId;
            InitializeComponent();
            lvBooks.ItemsSource = Books;
            txtWelcome.Text = $"Witaj, {currentUserLogin}";

            var dbDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "MojaBiblioteka");
            var databasePath = Path.Combine(dbDirectory, "moja-biblioteka.sqlite");

            var repository = new BookRepository(databasePath);
            _presenter = new MainPresenter(this, repository);
        }

        public void SetBooks(IEnumerable<BookModel> books)
        {
            Books.Clear();
            foreach (var book in books)
            {
                Books.Add(book);
            }
        }

        public BookModel ShowBookForm(BookModel bookToEdit = null)
        {
            var form = new BookFormWindow(bookToEdit) { Owner = this };
            var accepted = form.ShowDialog() == true;
            return accepted ? form.ViewModel.Book : null;
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
