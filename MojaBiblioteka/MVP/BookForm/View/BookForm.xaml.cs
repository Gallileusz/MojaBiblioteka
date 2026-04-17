using Microsoft.Win32;
using MojaBiblioteka.MVP.BookForm.Presenter;
using MojaBiblioteka.Utility;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MojaBiblioteka.MVP.BookForm.View
{
    public partial class BookForm : Window, IBookFormView
    {
        private readonly BookFormPresenter _presenter;
        private byte[] _coverImageData;

        public event System.EventHandler SaveClicked;
        public event System.EventHandler CancelClicked;
        public event System.EventHandler BrowseCoverClicked;

        public BookForm(BookModel bookToEdit = null)
        {
            InitializeComponent();
            _presenter = new BookFormPresenter(this, bookToEdit);
        }

        public string WindowTitleText
        {
            set => Title = value;
        }

        public string HeaderTitleText
        {
            set => HeaderTitleTextBlock.Text = value;
        }

        public string TitleText
        {
            get => TitleTextBox.Text;
            set => TitleTextBox.Text = value ?? string.Empty;
        }

        public string AuthorText
        {
            get => AuthorTextBox.Text;
            set => AuthorTextBox.Text = value ?? string.Empty;
        }

        public string YearText
        {
            get => YearTextBox.Text;
            set => YearTextBox.Text = value ?? string.Empty;
        }

        public string GenreText
        {
            get => GenreComboBox.SelectedItem as string;
            set => GenreComboBox.SelectedItem = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        public string DescriptionText
        {
            get => DescriptionTextBox.Text;
            set => DescriptionTextBox.Text = value ?? string.Empty;
        }

        public string CoverImagePath
        {
            get => CoverPathTextBox.Text;
            set => CoverPathTextBox.Text = value ?? string.Empty;
        }

        public byte[] CoverImageData
        {
            get => _coverImageData;
            set
            {
                _coverImageData = value;
                UpdateCoverPreview();
            }
        }

        public BookModel ResultBook { get; set; }

        public void SetGenres(System.Collections.Generic.IEnumerable<string> genres)
        {
            GenreComboBox.Items.Clear();
            foreach (var genre in genres)
            {
                GenreComboBox.Items.Add(genre);
            }
        }

        public string PickCoverPath()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Wybierz okładkę",
                Filter = "Obrazy|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Wszystkie pliki|*.*"
            };

            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        public void ShowError(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void CloseWithSuccess()
        {
            DialogResult = true;
            Close();
        }

        public void CloseWithCancel()
        {
            DialogResult = false;
            Close();
        }

        private void UpdateCoverPreview()
        {
            var imageSource = ToImageSource(_coverImageData);
            CoverPreviewImage.Source = imageSource;
            CoverPreviewImage.Visibility = imageSource == null ? Visibility.Collapsed : Visibility.Visible;
            CoverPlaceholderBorder.Visibility = imageSource == null ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveClicked?.Invoke(sender, e);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CancelClicked?.Invoke(sender, e);
        }

        private void BrowseCover_Click(object sender, RoutedEventArgs e)
        {
            BrowseCoverClicked?.Invoke(sender, e);
        }

        private static ImageSource ToImageSource(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            using (var stream = new System.IO.MemoryStream(bytes))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }
    }
}
