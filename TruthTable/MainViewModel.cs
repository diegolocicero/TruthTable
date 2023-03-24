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

        #region Gestione input
        public ICommand SendInputCommand { get; set; }
        public string Input { get; set; }
        #endregion
        #region Roba nostra
        public string InputCopy { get; set; }
        public ObservableCollection<char> Lettere { get; set; }
        #endregion
        #region Gestione Output

        public ObservableCollection<Char> Tabella { get; set; }


        public int NColonne => Lettere.Count + 1;
        public int NRighe => (int)Math.Pow(2, Lettere.Count);
        #endregion


        public MainViewModel()
        {
            SendInputCommand = new RelayCommand(SendInput);
            Lettere = new ObservableCollection<Char>();
            Tabella = new ObservableCollection<Char>();
        }
        private void Pulisci()
        {
            Lettere.Clear();
            Tabella.Clear();
        }
        public void SendInput()
        {
            Pulisci();
            InputCopy = Input.ToLower().Replace("and", "+").Replace(" ", "").Replace("xor", "^").Replace("or", "").Replace("not", "!").Replace("*", "");
            foreach (char ch in InputCopy)
            {
                if (!char.IsDigit(ch))
                {
                    if (ch == '+' || ch == '*' || ch == '!' || ch == '^' || ch == '(' || ch == ')')
                        continue;
                    else if (!Lettere.Contains(ch))
                        Lettere.Add(Convert.ToChar(ch.ToString().ToLower()));
                }
            }
            /*Controlli sull'input aggiuntivi, gestire che non ci possono essere più segni vicini, gestire i segni vietati...*/
            TrasformaInput();
            OnPropertyChanged(nameof(NColonne));
            OnPropertyChanged(nameof(NRighe));
            ComponiTabella();
        }
        public void TrasformaInput()
        {
            InputCopy = InputCopy.Insert(0, "(");
            InputCopy = InputCopy.Insert(InputCopy.Length, ")");
            for (int i = 0; i < InputCopy.Length; i++)
            {
                if (i != InputCopy.Length - 1 && char.IsLetter(InputCopy[i + 1]) && char.IsLetter(InputCopy[i]) && InputCopy[i] != ')' && InputCopy[i] != '(')
                {
                    InputCopy = InputCopy.Insert(i + 1, "*");
                }
                if (InputCopy[i] == '+')
                {
                    InputCopy = InputCopy.Insert(i, ")");
                    InputCopy = InputCopy.Insert(i + 2, "(");
                    i++;
                }
                if (InputCopy[i] == '!' && char.IsLetter(InputCopy[i - 1]))
                {
                    InputCopy = InputCopy.Insert(i, "*");
                    i++;
                }
            }

            InputCopy = InputCopy.Replace("!", " NOT ");
            InputCopy = InputCopy.Replace("^", " <> ");
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
                    Tabella.Add(binary[j]);
                Tabella.Add(CalcolaRisultato(binary));
            }
        }
        public char CalcolaRisultato(string binary)
        {
            Dictionary<char, char> dict = new Dictionary<char, char>();
            string copia = InputCopy;
            for (int i = 0; i < Lettere.Count; i++)
            {
                if (!dict.ContainsKey(Lettere[i])) //Forse si può togliere
                    dict.Add(Lettere[i], binary[i]);
            }
            foreach (char c in dict.Keys)
            {
                copia = copia.Replace(c.ToString(), dict[c] + " ");
            }
            copia = copia.Replace("0", "False");
            copia = copia.Replace("1", "True");
            bool risultato = (bool)new DataTable().Compute(copia, null);    //(A+B)*C
            return risultato ? '1' : '0';
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}


//Nella stringa input, scandisci la stringa, se dopo una lettere vedi + allora lascia vuoto, se vedi un'altra lettera mettici un OR subito dopo