using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HttpClientGrafikus
{
    class Auto
    {
        int id;
        string gyarto;
        string modell;
        int uzembehelyezes;

        public Auto(string gyarto, string modell, int uzembehelyezes, int id = 0)
        {
            this.id = id;
            this.gyarto = gyarto;
            this.modell = modell;
            this.uzembehelyezes = uzembehelyezes;
        }
        [JsonProperty("id")]
        public int Id { get => id; set => id = value; }
        [JsonProperty("gyarto")]
        public string Gyarto { get => gyarto; set => gyarto = value; }
        [JsonProperty("modell")]
        public string Modell { get => modell; set => modell = value; }
        [JsonProperty("uzembehelyezes")]
        public int Uzembehelyezes { get => uzembehelyezes; set => uzembehelyezes = value; }

        public override string ToString()
        {
            return String.Format($"{id,4}. {gyarto,-15} {modell,-10} ({uzembehelyezes})");
        }
    }
}
