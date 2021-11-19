using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientGrafikus
{
    class ApiValasz<T>
    {
        bool error;
        string uzenet;
        List<T> adatok;

        public ApiValasz(bool error, string uzenet, List<T> adatok)
        {
            this.error = error;
            this.uzenet = uzenet;
            this.adatok = adatok;
        }

        public bool Error { get => error; set => error = value; }
        public string Uzenet { get => uzenet; set => uzenet = value; }
        public List<T> Adatok { get => adatok; set => adatok = value; }

        public override string ToString()
        {
            string s = "{" + Environment.NewLine +
                "\terror: " + error + Environment.NewLine +
                "\tuzenet: " + uzenet;

            if (!error)
            {
                s += Environment.NewLine + "\tadatok: {";
                foreach (var item in adatok)
                {
                    s += Environment.NewLine + "\t\t" + item.ToString();
                }
                s += Environment.NewLine + "\t}";
            }
            s += Environment.NewLine + "}";
            return s;
        }
    }
}
