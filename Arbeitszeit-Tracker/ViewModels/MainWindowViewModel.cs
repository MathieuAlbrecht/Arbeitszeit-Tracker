using System.Collections.ObjectModel;

namespace Arbeitszeitaufschreiben.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<ArbeitszeitEintrag> Arbeitszeiten { get; } = new();
    }
}