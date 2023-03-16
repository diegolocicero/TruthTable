using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System;
using SaleWPF.FrameWork;
using System.Collections.Generic;
using System.Linq;

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
        public ObservableCollection<Carattere> Tabella { get; set; }


        public int NColonne => Lettere.Count+1;
        public int NRighe => (int)Math.Pow(2, Lettere.Count);

        public MainViewModel()
        {
            SendInputCommand = new RelayCommand(SendInput);
            Lettere = new ObservableCollection<Carattere>();
            Segni = new ObservableCollection<Carattere>();
            Tabella = new ObservableCollection<Carattere>();
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

            OnPropertyChanged(nameof(NColonne));
            OnPropertyChanged(nameof(NRighe));
            ComponiTabella();
        }

        public void ComponiTabella()
        {
            for (int i = 0; i < Math.Pow(2,Lettere.Count); i++)
            {
                string binary = Convert.ToString(i, 2);
                for (int k = binary.Length; k < Lettere.Count; k++)
                    binary = binary.Insert(0, "0");
                for (int j = 0; j < binary.Length; j++)
                    Tabella.Add(new Carattere(binary[j]));
                Tabella.Add(new Carattere('R'));
            }



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