using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace MojaBiblioteka.MVP.BookCard.View
{
    public partial class BookCard : UserControl, IBookCardView
    {
        public MojaBiblioteka.Utility.BookModel Book { get; set; }

        public BookCard()
        {
            InitializeComponent();
        }

        public string TitleText
        {
            set
            {
                TitleTextBlock.Text = value ?? string.Empty;
                TitleTextBlock.ToolTip = TitleTextBlock.Text;
            }
        }

        public string AuthorText
        {
            set
            {
                AuthorTextBlock.Text = value ?? string.Empty;
                AuthorTextBlock.ToolTip = AuthorTextBlock.Text;
            }
        }

        public void SetCoverImage(byte[] coverImageData)
        {
            var source = ToImageSource(coverImageData);
            BookCoverImage.Source = source;
            CoverPlaceholderBorder.Visibility = source == null
                ? System.Windows.Visibility.Visible
                : System.Windows.Visibility.Collapsed;
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
