using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PERSONEL_ENCAPSULATION
{
    public class DataBaseKontrol
    {

        private List<Personel> personelKayitlari = new List<Personel>();

        public List<Personel> PersonelKayitlari { get => personelKayitlari; set => personelKayitlari = value; }


        public void PersonelKayitlariniYukle()
        {
            string filePath = "personelKayitlari.txt";
            if (File.Exists(filePath))
            {

                FileStream fileoku = null;
                StreamReader streamoku = null;

                try
                {
                    fileoku = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
                    streamoku = new StreamReader(fileoku);

                    string okunan = streamoku.ReadLine();

                    while (okunan != null)
                    {
                        string[] personelSatiri = okunan.Split(';');

                        Personel personel = new Personel();

                        int index = 0;
                        foreach (var personelBilgileri in personel.GetType().GetProperties())
                        {
                            personelBilgileri.SetValue(personel, personelSatiri[index]);
                            index++;
                        }

                        //personel.Id = uyelikSatiri[0];
                        //personel.Ad = uyelikSatiri[1];

                        personelKayitlari.Add(personel);
                        okunan = streamoku.ReadLine();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    streamoku.Close();
                    streamoku = null;
                    fileoku.Close();
                    fileoku = null;
                }

            }

           

        }

        /// <summary>
        /// Girdi Kontrolü yapıp kayıt yoksa kayıt ekler
        /// </summary>
        /// <param name="form1"></param>
        /// <returns></returns>
        public bool GirdiKontrol(Form1 form1)
        {

            GroupBox groupBoxPersonelBilgileri = (GroupBox)(form1.Controls["groupBoxPersonelBilgileri"]);
            bool kayitOlusturuldu = false;
            string Id = groupBoxPersonelBilgileri.Controls["textBoxPersonelId"].Text;
            string Ad = groupBoxPersonelBilgileri.Controls["textBoxAd"].Text;
            string Soyad = groupBoxPersonelBilgileri.Controls["textBoxSoyad"].Text;
            string DogumTarihiMetinsel = groupBoxPersonelBilgileri.Controls["dateTimePickerDogumTarihi"].Text;
            int DogumTarihiYiliSayisal = ((DateTimePicker)(groupBoxPersonelBilgileri.Controls["dateTimePickerDogumTarihi"])).Value.Year;
            string Telefon = groupBoxPersonelBilgileri.Controls["maskedTextBoxTelefon"].Text;
            string Email = groupBoxPersonelBilgileri.Controls["textBoxEmail"].Text;
            string Adres = groupBoxPersonelBilgileri.Controls["textBoxAdres"].Text;
            string IseGiris = groupBoxPersonelBilgileri.Controls["dateTimePickerIseGiris"].Text;
            ComboBox comboBoxUnvan = (ComboBox)groupBoxPersonelBilgileri.Controls["comboBoxUnvan"];
            string Unvan = comboBoxUnvan.GetItemText(comboBoxUnvan.SelectedItem);
            string Resim = ((PictureBox)groupBoxPersonelBilgileri.Controls["pictureBox1"]).ImageLocation;

           

            char[] alphaNumeric = "!@#$%^&*()_-+=[{]}:<>|./?".ToCharArray();

            try
            {
                if (string.IsNullOrWhiteSpace(Id))
                    throw new Exception("Lütfen bir personel id'si giriniz");

                if (!(Id.Length == 5))
                    throw new Exception("Personel id'si 5 haneli olmalıdır. Tekrar Deneyiniz.");

                if (KarakterKontrol(Id, alphaNumeric) == true)
                    throw new Exception("Personel id'si !@#$%^&*()_-+=[{]}:<>|./? gibi karakterler içermemelidir.");

                if (string.IsNullOrWhiteSpace(Ad))
                    throw new Exception("Lütfen bir ad giriniz");

                if (string.IsNullOrWhiteSpace(Soyad))
                    throw new Exception("Lütfen bir soyad giriniz");
               
                if ((DateTime.Now.Year - DogumTarihiYiliSayisal) <18)
                    throw new Exception("Girilen yaş 18den büyük olmalıdır");

                if (string.IsNullOrWhiteSpace(Telefon))
                    throw new Exception("Lütfen bir telefon giriniz");

                if (string.IsNullOrWhiteSpace(Email))
                    throw new Exception("Lütfen bir email giriniz");
                
                if (!(Email).Contains("@bilgeadam.com"))
                    throw new Exception("Lütfen sonu @bilgeadam ile biten firma maili giriniz");

                if (string.IsNullOrWhiteSpace(Adres))
                    throw new Exception("Lütfen bir adres giriniz");




                if (Id.Contains(';') || Ad.Contains(';') || Soyad.Contains(';') || Email.Contains(';') || Adres.Contains(';'))
                    throw new Exception("Hiçbir bilgi ; karakterini içermemelidir.");




                Personel personel = new Personel();
                personel.Id = Id;
                personel.Ad = Ad;
                personel.Soyad = Soyad;
                personel.DogumTarihi = DogumTarihiMetinsel;
                personel.Telefon = Telefon;
                personel.Email = Email;
                personel.Adres = Adres;
                personel.IseGiris = IseGiris;
                personel.Unvan = Unvan;
                personel.Resim = Resim;

                if (!personelKayitlari.Contains(personel))
                {
                    personelKayitlari.Add(personel);
                    PERSONELBILGILERINIKAYDET();
                    kayitOlusturuldu = true;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return kayitOlusturuldu;
        }

        /// <summary>
        /// Girdi Kontrolü Yapıp Kaydı Günceller
        /// </summary>
        /// <param name="form1"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool GirdiKontrol(Form1 form1,int index)
        {

            GroupBox groupBoxPersonelBilgileri = (GroupBox)(form1.Controls["groupBoxPersonelBilgileri"]);
            bool kayitGuncellendi = false;
            string Id = groupBoxPersonelBilgileri.Controls["textBoxPersonelId"].Text;
            string Ad = groupBoxPersonelBilgileri.Controls["textBoxAd"].Text;
            string Soyad = groupBoxPersonelBilgileri.Controls["textBoxSoyad"].Text;
            string DogumTarihiMetinsel = groupBoxPersonelBilgileri.Controls["dateTimePickerDogumTarihi"].Text;
            int DogumTarihiYiliSayisal = ((DateTimePicker)(groupBoxPersonelBilgileri.Controls["dateTimePickerDogumTarihi"])).Value.Year;
            string Telefon = groupBoxPersonelBilgileri.Controls["maskedTextBoxTelefon"].Text;
            string Email = groupBoxPersonelBilgileri.Controls["textBoxEmail"].Text;
            string Adres = groupBoxPersonelBilgileri.Controls["textBoxAdres"].Text;
            string IseGiris = groupBoxPersonelBilgileri.Controls["dateTimePickerIseGiris"].Text;
            ComboBox comboBoxUnvan = (ComboBox)groupBoxPersonelBilgileri.Controls["comboBoxUnvan"];
            string Unvan = comboBoxUnvan.GetItemText(comboBoxUnvan.SelectedItem);
            string Resim = ((PictureBox)groupBoxPersonelBilgileri.Controls["pictureBox1"]).ImageLocation;



            char[] alphaNumeric = "!@#$%^&*()_-+=[{]}:<>|./?".ToCharArray();

            try
            {
                if (string.IsNullOrWhiteSpace(Id))
                    throw new Exception("Lütfen bir personel id'si giriniz");

                if (!(Id.Length == 5))
                    throw new Exception("Personel id'si 5 haneli olmalıdır. Tekrar Deneyiniz.");

                if (KarakterKontrol(Id, alphaNumeric) == true)
                    throw new Exception("Personel id'si !@#$%^&*()_-+=[{]}:<>|./? gibi karakterler içermemelidir.");

                if (string.IsNullOrWhiteSpace(Ad))
                    throw new Exception("Lütfen bir ad giriniz");

                if (string.IsNullOrWhiteSpace(Soyad))
                    throw new Exception("Lütfen bir soyad giriniz");

                if ((DateTime.Now.Year - DogumTarihiYiliSayisal) < 18)
                    throw new Exception("Girilen yaş 18den büyük olmalıdır");

                if (string.IsNullOrWhiteSpace(Telefon))
                    throw new Exception("Lütfen bir telefon giriniz");

                if (string.IsNullOrWhiteSpace(Email))
                    throw new Exception("Lütfen bir email giriniz");

                if (!(Email).Contains("@bilgeadam.com"))
                    throw new Exception("Lütfen sonu @bilgeadam ile biten firma maili giriniz");

                if (string.IsNullOrWhiteSpace(Adres))
                    throw new Exception("Lütfen bir adres giriniz");




                if (Id.Contains(';') || Ad.Contains(';') || Soyad.Contains(';') || Email.Contains(';') || Adres.Contains(';'))
                    throw new Exception("Hiçbir bilgi ; karakterini içermemelidir.");



                personelKayitlari[index].Id = Id;
                personelKayitlari[index].Ad = Ad;
                personelKayitlari[index].Soyad = Soyad;
                personelKayitlari[index].DogumTarihi = DogumTarihiMetinsel;
                personelKayitlari[index].Telefon = Telefon;
                personelKayitlari[index].Email = Email;
                personelKayitlari[index].Adres = Adres;
                personelKayitlari[index].IseGiris = IseGiris;
                personelKayitlari[index].Unvan = Unvan;
                personelKayitlari[index].Resim = Resim;



                PERSONELBILGILERINIKAYDET();
                kayitGuncellendi = true;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return kayitGuncellendi;
        }







        public void PERSONELBILGILERINIKAYDET()
        {
            string filePath = "personelKayitlari.txt";
            FileStream fileyaz = null;
            StreamWriter streamyaz = null;
            try
            {
                fileyaz = new FileStream(filePath, FileMode.Truncate, FileAccess.Write);
                streamyaz = new StreamWriter(fileyaz);

                foreach (Personel personel in personelKayitlari)
                {
                    streamyaz.WriteLine(String.Join(";", personel.Id, personel.Ad, personel.Soyad, personel.DogumTarihi, personel.Telefon, personel.Email, personel.Adres, personel.IseGiris, personel.Unvan, personel.Resim));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace);
            }
            finally
            {
                streamyaz.Close();
                streamyaz = null;
                fileyaz.Close();
                fileyaz = null;
                filePath = null;
            }
        }


        public bool KarakterKontrol(string sifre, char[] karakterler)
        {
            int sayacKarakterler = 0;
            for (int i = 0; i < karakterler.Length; i++)
            {
                if (sifre.Contains(karakterler[i]))
                {
                    sayacKarakterler++;
                }

            }
            bool karakterVarMi = sayacKarakterler > 0 ? true : false;
            return karakterVarMi;
        }
    }
}
