using System.Windows;

namespace Ezan_Vakti_Plus
{
    public partial class UpdateDialog : Window
    {
        public bool IsAccepted { get; private set; }

        public UpdateDialog(string current, string latest)
        {
            InitializeComponent();
            txtMessage.Text =
                $"Yeni sürüm bulundu: v{latest}\n" +
                $"Mevcut sürümün: v{current}\n\n" +
                "Güncellemeyi indirip kurmak ister misiniz?";
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            IsAccepted = true;
            Close();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
