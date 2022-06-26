using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mini_Projekt
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ObliczDate(string pesel)
        {
            int miesiac1;
            string miesiac2, rok, miesiac, dzien, nr_seryjny, 
            plec, cyfra_kontrolna;
            bool czyParzysta;
            
            miesiac1 = Convert.ToInt32(pesel.Substring(2, 1));
            miesiac2 = pesel.Substring(3, 1);
            czyParzysta = true;
            if(miesiac1 % 2 == 1)
            {
                miesiac1 -= 1;
                czyParzysta = false;
            }
            rok = ObliczRok(pesel, miesiac1);
            miesiac = ObliczMiesiac(miesiac1, miesiac2, czyParzysta);
            dzien = pesel.Substring(4, 2);
            nr_seryjny = pesel.Substring(6, 3);
            plec = ObliczPlec(pesel);
            cyfra_kontrolna = pesel.Substring(10, 1);
            
            //Wypisuje wszystkie dane do listboxa
            lb1.Items.Add("Rok urodzenia: " + rok);
            lb1.Items.Add("Miesiąc urodzenia: " + miesiac);
            lb1.Items.Add("Dzień urodzenia: " + dzien);
            lb1.Items.Add("Nr seryjny: " + nr_seryjny);
            lb1.Items.Add("Płeć: " + plec);
            lb1.Items.Add("cyfra_kontrolna: " + cyfra_kontrolna);
        }
        private string ObliczRok(string pesel, int miesiac1)
        {
            // Oblicza rok na podstawie pierwszej cyfry miesiąca,
            // zwraca odpowiedni rok np: 2001
            string rok_2_cyfry = pesel.Substring(0, 2);
            switch (miesiac1)
            {
                case 8:
                    return "18" + rok_2_cyfry;
                case 0:
                    return "19" + rok_2_cyfry;
                case 2:
                    return "20" + rok_2_cyfry;
                case 4:
                    return "21" + rok_2_cyfry;
                case 6:
                    return "22" + rok_2_cyfry;
                default: return "Niepoprawny pesel";
            }
        }
        private string ObliczMiesiac(int miesiac1, string miesiac2, bool czyParzysta)
        {
            // na podstawie 3 i 4 cyfry peselu oblicza miesiąć urodzenia
            // konwertuje na poprawną liczbę miesiąca np: z 32 na 12
            int a, b;
            if(czyParzysta == false)
            {
                miesiac1 += 1;
                a = Convert.ToInt32(miesiac1 + "0");
                miesiac1 -= 1;
            } else a = Convert.ToInt32(miesiac1 + "0");

            b = Convert.ToInt32($"{miesiac1}{miesiac2}");
            if (b - a == 0)
            {
                return "10";
            }
            else return Convert.ToString(b - a);
        }
        private string ObliczPlec(string pesel)
        {
            // oblicza płeć na podstawie 10 cyfry peselu, jeżeli parzysta = kobieta,
            // jeżeli nieparzysta = mężczyzna
            int plec = Convert.ToInt32(pesel.Substring(9, 1));
            if (plec % 2 == 1)
            {
                return "Mężczyzna";
            }
            else return "Kobieta";
        }
        private bool ObslugaBledow(string pesel)
        {
            long test = 0;
            bool result = long.TryParse(pesel, out test);
            if (pesel.Length == 11 && result)
            {
                return true;
            }
            else return false;
        }
        private bool ObliczCyfreKontrolna(string pesel)
        {
            // oblicza cyfrę kontrolną na podstawie wzoru
            // jeżeli cyfra jest prawidłowa to zwraca true
            int[] wagaCyfry = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3};
            int cyfra_kontrolna;
            int suma_iloczynow = 0;
            // pętla oblicza dla pierwszych 10 cyfr peselu iloczyn tej cyfry * jej waga
            // która jest do niej przypisana zgodnie z rozporządzeniem,
            // a następnie sumuje wszystkie do wyniki
            for(int i=0; i<10; i++)
            {
                int wynik_iloczynu = (pesel[i]-48) * wagaCyfry[i];
                suma_iloczynow += wynik_iloczynu;
            }
           
            int M = suma_iloczynow % 10;
            // jeżeli M = 0 to cyfra kontrolna też jest równa 0, jeżeli różna od zera 
            // to odejmuje 10 - M i przypisuje do zmiennej "cyfra_kontrolna"
            if (M == 0)
            {
                cyfra_kontrolna = 0;
            }
            else
            {
                cyfra_kontrolna = 10 - M;
            }
            // jeżeli obliczona cyfra kontrolna jest równa tej podanej w peselu 
            // to zwraca true
            if (cyfra_kontrolna == pesel[10]-48)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            string pesel = Convert.ToString(input_pesel.Text);
            input_pesel.Clear();
            lb1.Items.Clear();
            bool testBledow = ObslugaBledow(pesel);
            if (testBledow)
            {
                bool testCyfryKontrolnej = ObliczCyfreKontrolna(pesel);
                if (testCyfryKontrolnej)
                {
                    ObliczDate(pesel);
                }
                else MessageBox.Show("Niepoprawny pesel");
            }   
        }
    }
}
