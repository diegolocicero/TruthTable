﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System;
using System.Linq;
using GalaSoft.MvvmLight.Command;
namespace TruthTable
{


    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region
        public ICommand SendInputCommand { get; set; } //ICommand per settare l'input, si attiva con il button
        public string Input { get; set; } //Prop in binding con la textbox 
        #endregion

        //PROPS NON BINDATE
        public ObservableCollection<Carattere> Lettere { get; set; }
        public ObservableCollection<Carattere> Segni { get; set; }

        public ObservableCollection<Carattere> LettereSegni
        {
            get
            {
                var result = new ObservableCollection<Carattere>(Lettere.Concat(Segni));
                result.Insert(0, new Carattere(' '));
                return result;
            }
        }

        public int NColonne => LettereSegni.Count;
        public int NRighe => (int)Math.Pow(2, Lettere.Count) + 1;

        public MainViewModel()
        {
            SendInputCommand = new RelayCommand(SendInput);
            Lettere = new ObservableCollection<Carattere>();
            Segni = new ObservableCollection<Carattere>();
        }

        public void SendInput() //Assegna a lettere e segni le lettere e i segni, e grazie al cazzo (scarta i numeri)
        {
            Lettere.Clear();
            Segni.Clear();
            foreach (char ch in Input)
            {
                if (!char.IsDigit(ch) && ch != ' ')
                {
                    if (ch == '+' || ch == '*')
                        Segni.Add(new Carattere(ch));
                    else
                        Lettere.Add(new Carattere(ch));
                }
            }

            OnPropertyChanged(nameof(LettereSegni));
            OnPropertyChanged(nameof(NColonne));
            OnPropertyChanged(nameof(NRighe));
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Carattere
    {
        public char Carac { get; set; }
        public Carattere(char carattere)
        {
            Carac = carattere;
        }
    }


}