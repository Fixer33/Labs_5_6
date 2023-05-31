using Labs_5_6.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Labs_5_6.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string _name;
        private List<string> _members;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            _members = new List<string>();
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(Name)
                && _members != null && _members.Count > 0;
        }

        public void AddMember(string member)
        {
            _members.Add(member);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
            _members.Clear();
        }

        private async void OnSave()
        {
            List<string> members = new List<string>();
            foreach (string member in _members)
            {
                members.Add(member);
            }
            members.Add(App.UserId);

            Network.CreateChat(Name, members);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
            _members.Clear();
        }
    }
}
