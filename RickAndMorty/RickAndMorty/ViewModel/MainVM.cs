using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RickAndMorty.Model;
using RickAndMorty.Repository.API;
using RickAndMorty.Repository.Local;
using RickAndMorty.View;

namespace RickAndMorty.ViewModel
{
    public class MainVM : ObservableObject
    {
        public static bool UseLocalAPI; 

        public OverViewPage MainPage { get; set; }
        public CharacterDetailPage CharacterDetailPage { get; set; }
        public EpisodeDetailPage EpisodeDetailPage { get; set; }
        public LocationDetailPage LocationDetailPage { get; set; }
        public Page CurrentPage { get; set; }
        
        public string CommandText { get; set; }
        public RelayCommand SwitchPageCommand { get; set; }
        public string GoBackCommandText { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public string SwitchRepoCommandText { get; set; }
        public RelayCommand SwitchRepoCommand { get; set; }

        public MainVM()
        {
            MainPage = new OverViewPage();
            CharacterDetailPage = new CharacterDetailPage();
            EpisodeDetailPage = new EpisodeDetailPage();
            LocationDetailPage = new LocationDetailPage();
            CurrentPage = MainPage;

            CommandText = "Show Details";
            GoBackCommandText = "Back To Overview";
            SwitchRepoCommandText = "Switch To LOCAL Repository";

            SwitchPageCommand = new RelayCommand(SwitchPage);
            GoBackCommand = new RelayCommand(GoBack);
            SwitchRepoCommand = new RelayCommand(SwitchRepo);
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

        public void SwitchRepo()
        {
            if (UseLocalAPI == false)
            {
                UseLocalAPI = true;
                SwitchRepoCommandText = "Switch To API Repository";
                
                (MainPage.DataContext as OverViewVM)?.SwitchRepo();

                if (CurrentPage is CharacterDetailPage)
                {
                    var currentCharacter = (CurrentPage.DataContext as CharacterDetailPageVM)?.CurrentCharacter;
                
                    var idList = new List<int> { currentCharacter.Id };
                
                    var characterList = CharacterLocalRepository.GetInstance().GetCharactersByIdsAsync(idList).Result;
                    if (characterList == null)
                    {
                        ((CharacterDetailPageVM)CurrentPage.DataContext).CurrentCharacter =
                            new Character()
                            {
                                Id = 1,
                                Name = "Rick Sanchez",
                                Status = "Alive",
                                Species = "Human",
                                Type = "",
                                Gender = "Male",
                                Origin = { },
                                Location = { },
                                Episodes = { }
                            };
                    }
                    else
                        ((CharacterDetailPageVM)CurrentPage.DataContext).CurrentCharacter = characterList[0];
                }
                else if (CurrentPage is EpisodeDetailPage)
                {
                    var currentEpisode = (CurrentPage.DataContext as EpisodeDetailPageVM)?.CurrentEpisode;

                    var idList = new List<int> { currentEpisode.Id };

                    var episodeList = EpisodeLocalRepository.GetInstance().GetEpisodesByIdsAsync(idList).Result;
                    if (episodeList == null || episodeList.Count == 0)
                    {
                        ((EpisodeDetailPageVM)CurrentPage.DataContext).CurrentEpisode =
                            new Episode()
                            {
                                Id = 1,
                                Name = "Pilot",
                                AirDate = "December 2, 2013",
                                EpisodeNumber = "S01E01",
                                Characters = { }
                            };
                    }
                    else
                        ((EpisodeDetailPageVM)CurrentPage.DataContext).CurrentEpisode = episodeList[0];
                }
                else if (CurrentPage is LocationDetailPage)
                {
                    var currentLocation = (CurrentPage.DataContext as LocationDetailPageVM)?.CurrentLocation;

                    currentLocation = LocationLocalRepository.GetInstance().GetLocationByIdAsync(currentLocation.Id).Result;
                    if (currentLocation == null || currentLocation.Id == 0)
                    {
                        ((LocationDetailPageVM)CurrentPage.DataContext).CurrentLocation =
                            new Location()
                            {
                                Id = 1,
                                Name = "Earth (C-137)",
                                Type = "Planet",
                                Dimension = "Dimension C-137",
                                Residents = { }
                            };
                    }
                    else
                        ((LocationDetailPageVM)CurrentPage.DataContext).CurrentLocation = currentLocation;
                }

                OnPropertyChanged(CurrentPage.Name);
            }
            else
            {
                UseLocalAPI = false;
                SwitchRepoCommandText = "Switch To LOCAL Repository";

                (MainPage.DataContext as OverViewVM)?.SwitchRepo();

                if (CurrentPage is CharacterDetailPage)
                {
                    var currentCharacter = (CurrentPage.DataContext as CharacterDetailPageVM)?.CurrentCharacter;

                    var idList = new List<int> { currentCharacter.Id };

                    currentCharacter = CharacterAPIRepository.GetInstance().GetCharactersByIdsAsync(idList).Result[0];

                    ((CharacterDetailPageVM)CurrentPage.DataContext).CurrentCharacter = currentCharacter;
                }
                else if (CurrentPage is EpisodeDetailPage)
                {
                    var currentEpisode = (CurrentPage.DataContext as EpisodeDetailPageVM)?.CurrentEpisode;

                    var idList = new List<int> { currentEpisode.Id };

                    currentEpisode = EpisodeAPIRepository.GetInstance().GetEpisodesByIdsAsync(idList).Result[0];

                    ((EpisodeDetailPageVM)CurrentPage.DataContext).CurrentEpisode = currentEpisode;
                }
                else if (CurrentPage is LocationDetailPage)
                {
                    var currentLocation = (CurrentPage.DataContext as LocationDetailPageVM)?.CurrentLocation;

                    currentLocation = LocationAPIRepository.GetInstance().GetLocationByIdAsync(currentLocation.Id).Result;

                    ((LocationDetailPageVM)CurrentPage.DataContext).CurrentLocation = currentLocation;
                }


                OnPropertyChanged(CurrentPage.Name);
            }
        }
    }
}
