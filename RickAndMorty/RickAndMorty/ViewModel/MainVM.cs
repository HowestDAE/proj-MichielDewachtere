using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RickAndMorty.Model;
using RickAndMorty.View;

namespace RickAndMorty.ViewModel
{
    public class MainVM : ObservableObject
    {
        public string CommandText { get; set; }
        public OverViewPage MainPage { get; set; }
        public CharacterDetailPage CharacterDetailPage { get; set; }
        public Page CurrentPage { get; set; }
        public RelayCommand SwitchPageCommand { get; set; }

        public MainVM()
        {
            MainPage = new OverViewPage();
            CharacterDetailPage = new CharacterDetailPage();
            CurrentPage = MainPage;

            CommandText = "Show Details";

            SwitchPageCommand = new RelayCommand(SwitchPage);
        }

        public void SwitchPage()
        {
            if (CurrentPage is OverViewPage)
            {
                Character selectedCharacter = (MainPage.DataContext as OverViewVM)?.SelectedCharacter;
                if (selectedCharacter == null)
                    return;

                ((CharacterDetailPageVM)CharacterDetailPage.DataContext).CurrentCharacter = selectedCharacter;

                CurrentPage = CharacterDetailPage;
                OnPropertyChanged(nameof(CurrentPage));

                CommandText = "Go Back";
                OnPropertyChanged(nameof(CommandText));
            }
            else if (CurrentPage is CharacterDetailPage)
            {
                CurrentPage = MainPage;
                OnPropertyChanged(nameof(CurrentPage));

                CommandText = "Show Details";
                OnPropertyChanged(nameof(CommandText));
            }
        }
    }
}
