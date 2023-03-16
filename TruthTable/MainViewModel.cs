using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System;
using SaleWPF.FrameWork;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Data;
using System.Linq.Expressions;

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
            /*Controlli sull'input aggiuntivi, gestire che non ci possono essere più segni vicini, gestire i segni vietati...*/
            Input = Input.Replace("+", " AND ");
            OnPropertyChanged(nameof(Input));
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
                Tabella.Add(CalcolaRisultato(binary));
            }
        }
        public Carattere CalcolaRisultato(string binary)
        {
            binary = binary.Replace("1", "True");
            binary = binary.Replace("0", "True");
            string espressione = binary; // Esempio di espressione booleana in forma testuale
            bool risultato = (bool)new DataTable().Compute(espressione, null); // Valuta l'espressione e converte il risultato in un valore booleano
            MessageBox.Show(risultato.ToString());
            return new Carattere('T'); //PER OGNI ITERAZIONE DELLA RIGA DELLA TABLE SE A è 0 SOSTITUARLA CON FALSE E SE è 1 CON TRUE
                Tabella.Add(new Carattere('R'));
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