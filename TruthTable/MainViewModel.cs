using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TruthTable
{
    internal class MainViewModel
    {
        #region
        public ICommand SendInputCommand { get; set; } //ICommand per settare l'input, si attiva con il button
        public string Input { get; set; } //Prop in binding con la textbox 
        #endregion

        //PROPS NON BINDATE
        public string Lettere { get; set; }
        public string Segni { get; set; }

        public MainViewModel()
        {
            SendInputCommand = new RelayCommand(SendInput);
        }
        public void SendInput() //Assegna a lettere e segni le lettere e i segni, e grazie al cazzo (scarta i numeri)
        {
            foreach(char ch in Input)
            {
                if(!char.IsDigit(ch) && ch != ' ')
                {
                    if (ch == '+' || ch == '*')
                        Segni += ch;
                    else
                        Lettere += ch;
                }
            }
            MessageBox.Show("Lettere: " + Lettere + "   Segni: " + Segni);

        }
    }
}
