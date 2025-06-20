// NotificationRow.xaml.cs

using System.Windows.Controls;

namespace Ezan_Vakti_Plus
{
    public partial class NotificationRow
    {
        public NotificationRow() => InitializeComponent();

        public string Label { get; set; }
        public string TextBoxName { get; set; }
        public int Default { get; set; }

        // Window Loaded olduktan sonra ana pencere TextBox'lara erişebilsin
        public TextBox GetBox() => InputBox;
    }
}