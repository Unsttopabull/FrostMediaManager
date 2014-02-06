using System.Windows.Controls;

namespace RibbonUI.UserControls.Edit {
    /// <summary>Interaction logic for EditSubtitles.xaml</summary>
    public partial class EditSubtitles : UserControl {
        public EditSubtitles() {
            InitializeComponent();
        }

        private void SubtitlesList_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            if (e.EditAction == DataGridEditAction.Commit) {
                if (e.Column.Header as string == "Language") {

                }
            }
        }
    }
}
