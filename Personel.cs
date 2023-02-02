using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERSONEL_ENCAPSULATION
{
    public class Personel
    {
        private string id;
        private string ad;
        private string soyad;
        private string dogumTarihi;
        private string telefon;
        private string email;
        private string adres;
        private string iseGiris;
        private string unvan;
        private string resim;
        public string Id { get => id; set => id = value; }
        public string Ad { get => ad; set => ad = value; }
        public string Soyad { get => soyad; set => soyad = value; }
        public string DogumTarihi { get => dogumTarihi; set => dogumTarihi = value; }
        public string Telefon { get => telefon; set => telefon = value; }
        public string Email { get => email; set => email = value; }
        public string Adres { get => adres; set => adres = value; }
        public string IseGiris { get => iseGiris; set => iseGiris = value; }
        public string Unvan { get => unvan; set => unvan = value; }
        public string Resim { get => resim; set => resim = value; }
    }
}
