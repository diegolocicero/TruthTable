using SaleWPF.FrameWork;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows.Input;

namespace TruthTable
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region
        public ICommand SendInputCommand { get; set; } //ICommand per settare l'input, si attiva con il button
        public string Input { get; set; } //Prop in binding con la textbox 
        #endregion
        public string InputCopy { get; set; }
        //PROPS NON BINDATE
        public ObservableCollection<Carattere> Lettere { get; set; }
        public ObservableCollection<Carattere> Tabella { get; set; }


        public int NColonne => Lettere.Count + 1;
        public int NRighe => (int)Math.Pow(2, Lettere.Count);

        public MainViewModel()
        {
            SendInputCommand = new RelayCommand(SendInput);
            Lettere = new ObservableCollection<Carattere>();
            Tabella = new ObservableCollection<Carattere>();
        }

        public void SendInput() //Assegna a lettere e segni le lettere e i segni, e grazie al cazzo (scarta i numeri)
        {
            Lettere.Clear();
            InputCopy = string.Empty;
            Tabella.Clear();
            foreach (char ch in Input)
            {
                if (!char.IsDigit(ch) && ch != ' ')
                {
                    if (ch == '+' || ch == '*' || ch == '!')
                        continue;
                    else if (!Lettere.Contains(new Carattere(ch)))
                        Lettere.Add(new Carattere(Convert.ToChar(ch.ToString().ToLower())));
                }
            }
            /*Controlli sull'input aggiuntivi, gestire che non ci possono essere più segni vicini, gestire i segni vietati...*/
            TrasformaInput();
            OnPropertyChanged(nameof(Input));
            OnPropertyChanged(nameof(NColonne));
            OnPropertyChanged(nameof(NRighe));
            ComponiTabella();
        }
        public void TrasformaInput()
        {
            InputCopy = Input;
            InputCopy = InputCopy.Insert(0, "(");
            InputCopy = InputCopy.Insert(InputCopy.Length, ")");
            for (int i = 0; i < InputCopy.Length; i++)
            {
                if (i != InputCopy.Length-1 && char.IsLetter(InputCopy[i+1]) && char.IsLetter(InputCopy[i]) && InputCopy[i] != ')' && InputCopy[i] != '(')
                {
                    InputCopy = InputCopy.Insert(i + 1, "*");
                }
                if (InputCopy[i] == '+')
                {
                    InputCopy = InputCopy.Insert(i, ")");
                    InputCopy = InputCopy.Insert(i+2, "("); //Il problema è qui
                    i++;
                }
            }           
            InputCopy = InputCopy.ToLower().Trim();
            InputCopy = InputCopy.Replace("!", " NOT ");
            InputCopy = InputCopy.Replace("+", " OR ");
            InputCopy = InputCopy.Replace("*", " AND ");
            
        }
        public void ComponiTabella()
        {
            for (int i = 0; i < Math.Pow(2, Lettere.Count); i++)
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
            Dictionary<char, char> dict = new Dictionary<char, char>();
            string copia = InputCopy;
            for (int i = 0; i < Lettere.Count; i++)
            {
                if (!dict.ContainsKey(Lettere[i].Carac))
                    dict.Add(Lettere[i].Carac, binary[i]);
            }
            foreach (char c in dict.Keys)
            {
                copia = copia.Replace(c.ToString(), dict[c] + " ");
            }
            copia = copia.Replace("0", "False");
            copia = copia.Replace("1", "True");
            bool risultato = (bool)new DataTable().Compute(copia, null); //"DFWFEWFEWFWEF"
            return new Carattere(risultato? '1' : '0'); 
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
        public override bool Equals(object? obj)
        {
                if (obj is not Carattere) return false;
            return (obj as Carattere).Carac.Equals(Carac);
        }
    }
}


//Nella stringa input, scandisci la stringa, se dopo una lettere vedi + allora lascia vuoto, se vedi un'altra lettera mettici un OR subito dopo