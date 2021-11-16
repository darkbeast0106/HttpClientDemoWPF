using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
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

namespace HttpClientGrafikus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string url = "http://localhost/_oktatas/14s/2021_10_14/?vegpont=";
        public MainWindow()
        {
            InitializeComponent();
            ListAsync();
        }

        private async Task ListAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(url + "listaz");
                Valasz<Auto> valasz = JsonConvert.DeserializeObject<Valasz<Auto>>(content);
                // http://www.newtonsoft.com/json/help/html/SerializingJSON.htm
                Feltolt(valasz);
            }
        }

        private void Feltolt(Valasz<Auto> valasz)
        {
            if (valasz == null)
            {
                MessageBox.Show("Hiba történt a kérés feldolgozása során");
                return;
            }
            if (valasz.Error)
            {
                MessageBox.Show(valasz.Uzenet);
                return;
            }
            listBox.Items.Clear();
            foreach (Auto auto in valasz.Adatok)
            {
                listBox.Items.Add(auto);
            }
        }

        private void buttonHozzaad_Click(object sender, RoutedEventArgs e)
        {
            string gyarto = textBoxGyarto.Text.Trim();
            string modell = textBoxModell.Text.Trim();
            string uzembe = textBoxUzembehelyezes.Text.Trim();
            if (string.IsNullOrEmpty(gyarto) || string.IsNullOrEmpty(modell) || string.IsNullOrEmpty(uzembe))
            {
                MessageBox.Show("Minden mező kitöltése kötelező");
                return;
            }
            int uzembehelyezes = int.Parse(uzembe);
            int iden = DateTime.Now.Year;
            if (uzembehelyezes < 1900 || uzembehelyezes > iden)
            {
                MessageBox.Show($"Az üzembehelyezés évének 1900 és a idei évszám ({iden}) közé kell esnie");
                return;
            }
            Auto auto = new Auto(gyarto, modell, uzembehelyezes);
            buttonHozzaad.IsEnabled = false;
            RogzitAsync(auto);
        }

        private async Task RogzitAsync(Auto auto)
        {
            using (HttpClient client = new HttpClient())
            {

                var json = JsonConvert.SerializeObject(auto);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                var result = await client.PostAsync(url + "felvesz", stringContent);

                string content = result.Content.ReadAsStringAsync().Result;
                Valasz<Auto> valasz = JsonConvert.DeserializeObject<Valasz<Auto>>(content);
                buttonHozzaad.IsEnabled = true;
                Feltolt(valasz);
            }
        }
        private async Task RogzitAsyncUrlEncoded(Auto auto)
        {
            using (HttpClient client = new HttpClient())
            {

                var data = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("gyarto", auto.Gyarto),
                    new KeyValuePair<string, string>("modell", auto.Modell),
                    new KeyValuePair<string, string>("uzembehelyezes", auto.Uzembehelyezes.ToString())
                });
                var result = await client.PostAsync(url + "felvesz", data);

                string content = result.Content.ReadAsStringAsync().Result;
                Valasz<Auto> valasz = JsonConvert.DeserializeObject<Valasz<Auto>>(content);
                buttonHozzaad.IsEnabled = true;
                Feltolt(valasz);
            }
        }

        private void textBoxUzembehelyezes_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
