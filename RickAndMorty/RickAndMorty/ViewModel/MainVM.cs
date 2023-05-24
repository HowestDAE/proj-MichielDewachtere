using System;
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
        public static bool UseLocalAPI = true; 

        public OverViewPage MainPage { get; set; }
        public CharacterDetailPage CharacterDetailPage { get; set; }
        public EpisodeDetailPage EpisodeDetailPage { get; set; }
        public LocationDetailPage LocationDetailPage { get; set; }
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
            LocationDetailPage = new LocationDetailPage();
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
            }
            else if (CurrentPage is CharacterDetailPage)
            {
                if (!(CurrentPage.DataContext is CharacterDetailPageVM characterPageVm))
                {
                    Console.WriteLine("error");
                    return;
                }

                if (characterPageVm.OriginSelected || characterPageVm.LocationSelected)
                {
                    Location selectedLocation = characterPageVm.SelectedLocation;
                    if (selectedLocation == null) 
                        return;

                    ((LocationDetailPageVM)LocationDetailPage.DataContext).CurrentLocation = selectedLocation;

                    characterPageVm.OriginSelected = false;
                    characterPageVm.LocationSelected = false;

                    CurrentPage = LocationDetailPage;
                    OnPropertyChanged(nameof(CurrentPage));
                }
                else
                {
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
            else if (CurrentPage is LocationDetailPage)
            {
                Character selectedCharacter = (CurrentPage.DataContext as LocationDetailPageVM)?.SelectedResident;
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
