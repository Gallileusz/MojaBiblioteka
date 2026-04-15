using Microsoft.Win32;
using MojaBiblioteka.Utility;
using System.IO;
using System.Windows;

namespace MojaBiblioteka.MVP.BookForm.View
{
    /// <summary>
    /// Logika interakcji dla klasy BookForm.xaml
    /// </summary>
    public partial class BookForm : Window
    {
        // ViewModel okna – prosty wrapper
        public BookFormViewModel ViewModel { get; }

        public BookForm(BookModel bookToEdit = null)
        {
            InitializeComponent();

            ViewModel = new BookFormViewModel
            {
                // Tryb edycji: klonujemy model żeby nie mutować oryginału przed Save
                // Tryb dodawania: pusty model
                Book = bookToEdit?.Clone() ?? new BookModel(),
                WindowTitle = bookToEdit == null ? "Dodaj książkę" : "Edytuj książkę"
            };

            DataContext = ViewModel;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ViewModel.Book.Title) ||
                string.IsNullOrWhiteSpace(ViewModel.Book.Author))
            {
                MessageBox.Show("Tytuł i autor są wymagane.", "Brakujące dane",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TryLoadCoverImageFromPath())
            {
                return;
            }

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BrowseCover_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Wybierz okładkę",
                Filter = "Obrazy|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Wszystkie pliki|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                ViewModel.Book.CoverImagePath = dialog.FileName;
                ViewModel.Book.CoverImageData = File.ReadAllBytes(dialog.FileName);
            }
        }

        private bool TryLoadCoverImageFromPath()
        {
            var path = ViewModel.Book.CoverImagePath;
            if (string.IsNullOrWhiteSpace(path))
            {
                return true;
            }

            try
            {
                if (!File.Exists(path))
                {
                    MessageBox.Show("Wybrana ścieżka okładki nie istnieje.", "Błąd pliku",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                ViewModel.Book.CoverImageData = File.ReadAllBytes(path);
                return true;
            }
            catch
            {
                MessageBox.Show("Nie udało się odczytać pliku okładki.", "Błąd pliku",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }
    }

    public class BookFormViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private string _windowTitle;
        public string WindowTitle
        {
            get => _windowTitle;
            set { _windowTitle = value; OnPropertyChanged(nameof(WindowTitle)); }
        }

        private BookModel _book;
        public BookModel Book
        {
            get => _book;
            set { _book = value; OnPropertyChanged(nameof(Book)); }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
    }
}
