using System;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RickAndMorty.Model;
using RickAndMorty.View;

namespace RickAndMorty.ViewModel
{
    public class MainVM : ObservableObject
    {
        public OverViewPage MainPage { get; set; }
        public CharacterDetailPage CharacterDetailPage { get; set; }
        public EpisodeDetailPage EpisodeDetailPage { get; set; }
        public Page CurrentPage { get; set; }
        
        public string CommandText { get; set; }
        public RelayCommand SwitchPageCommand { get; set; }
        public string GoBackCommandText { get; set; }
        public RelayCommand GoBackCommand { get; set; }

        public MainVM()
        {
            MainPage = new OverViewPage();
            CharacterDetailPage = new CharacterDetailPage();
            EpisodeDetailPage = new EpisodeDetailPage();
            CurrentPage = MainPage;

            CommandText = "Show Details";
            GoBackCommandText = "Back To Overview";

            SwitchPageCommand = new RelayCommand(SwitchPage);
            GoBackCommand = new RelayCommand(GoBack);
        }

        public void SwitchPage()
        {
            if (CurrentPage is OverViewPage)
            {
                Character selectedCharacter = (CurrentPage.DataContext as OverViewVM)?.SelectedCharacter;
                if (selectedCharacter == null)
                    return;

                ((CharacterDetailPageVM)CharacterDetailPage.DataContext).CurrentCharacter = selectedCharacter;

                CurrentPage = CharacterDetailPage;
                OnPropertyChanged(nameof(CurrentPage));

                //CommandText = "Go Back";
                //OnPropertyChanged(nameof(CommandText));
            }
            else if (CurrentPage is CharacterDetailPage)
            {
                //CurrentPage = MainPage;
                //OnPropertyChanged(nameof(CurrentPage));
                //
                //CommandText = "Show Details";
                //OnPropertyChanged(nameof(CommandText));

                if (!(CurrentPage.DataContext is CharacterDetailPageVM characterPageVm))
                {
                    Console.WriteLine("error");
                    return;
                }

                Episode selectedEpisode = characterPageVm.SelectedEpisode;
                if (selectedEpisode == null)
                {
                    Console.WriteLine("SelectedEpisode was null");
                    return;
                }

                ((EpisodeDetailPageVM)EpisodeDetailPage.DataContext).CurrentEpisode = selectedEpisode;

                CurrentPage = EpisodeDetailPage;
                OnPropertyChanged(nameof(CurrentPage));
            }
            else if (CurrentPage is EpisodeDetailPage)
            {
                Character selectedCharacter = (CurrentPage.DataContext as EpisodeDetailPageVM)?.SelectedCharacter;
                if (selectedCharacter == null)
                    return;

                ((CharacterDetailPageVM)CharacterDetailPage.DataContext).CurrentCharacter = selectedCharacter;

                CurrentPage = CharacterDetailPage;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        public void GoBack()
        {
            CurrentPage = MainPage;
            OnPropertyChanged(nameof(CurrentPage));
        }
    }
}
