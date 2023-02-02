using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PERSONEL_ENCAPSULATION
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataBaseKontrol dataBaseKontrol;
        List<Personel> personelListesi;
        private void Form1_Load(object sender, EventArgs e)
        {
            dataBaseKontrol = new DataBaseKontrol();
            dataBaseKontrol.PersonelKayitlariniYukle();
            personelListesi = dataBaseKontrol.PersonelKayitlari;
            AddPersonelListToListView(personelListesi);

            comboBoxUnvan.Items.Add("Stajyer");
            comboBoxUnvan.Items.Add("Danışman");
            comboBoxUnvan.Items.Add("İnsan Kaynakları Yöneticisi");
            comboBoxUnvan.Items.Add("Takım Lideri");
            comboBoxUnvan.Items.Add("Birim Müdürü");
            comboBoxUnvan.Items.Add("Eğitmen");
            comboBoxUnvan.Items.Add("Yazılımcı");
            comboBoxUnvan.SelectedIndex = 0;
        }

        private void buttonKaydet_Click(object sender, EventArgs e)
        {

            if (dataBaseKontrol.GirdiKontrol(this))
            {
                ClearForm();

                personelListesi = dataBaseKontrol.PersonelKayitlari;
                AddPersonelListToListView(personelListesi);
            }

        
        }


        private void AddPersonelListToListView(List<Personel> personelListesi)
        {
            listViewPersonel.Items.Clear();
            int _index = 0;
            foreach (Personel personel in personelListesi)
            {
                listViewPersonel.Items.Add(personel.Id);
                listViewPersonel.Items[_index].SubItems.Add(personel.Ad);
                listViewPersonel.Items[_index].SubItems.Add(personel.Soyad);
                listViewPersonel.Items[_index].SubItems.Add(personel.IseGiris);
                listViewPersonel.Items[_index].SubItems.Add(personel.Email);
                _index++;
            }

        }



        private void ClearForm()
        {

            foreach (Control item in groupBoxPersonelBilgileri.Controls)
            {
                Type type = item.GetType();
                switch (type.ToString())
                {
                    case "System.Windows.Forms.TextBox":
                        {
                            TextBox _txt = (TextBox)item;
                            _txt.Clear();
                            break;
                        }
                    case "System.Windows.Forms.DateTimePicker":
                        {
                            DateTimePicker _dtp = (DateTimePicker)item;
                            _dtp.Value = DateTime.Now;
                            break;
                        }
                    case "System.Windows.Forms.MaskedTextBox":
                        {
                            MaskedTextBox mTxt = (MaskedTextBox)item;
                            mTxt.Clear();
                            break;
                        }
                    case "System.Windows.Forms.NumericUpDown":
                        {
                            NumericUpDown _nUpDown = (NumericUpDown)item;
                            _nUpDown.Value = 1;
                            break;
                        }
                    case "System.Windows.Forms.ComboBox":
                        {
                            ComboBox cBox = (ComboBox)item;
                            cBox.SelectedItem = null;
                            break;
                        }
                    case "System.Windows.Forms.PictureBox":
                        {
                            PictureBox pBox = (PictureBox)item;
                            if (pBox.Image != null)
                            {
                                pBox.Image.Dispose();
                                pBox.Image = null;
                                pBox.Update();
                            }
                            break;
                        }
                }
            }
            listViewPersonel.SelectedItems.Clear();
        }

        private void buttonSil_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(listViewPersonel.SelectedItems.Count > 0))
                    throw new Exception("Lütfen aşağıdaki listeden bir personel kaydı seçiniz");

                if (listViewPersonel.SelectedItems[0] != null)
                {
                    int selectedIndex = listViewPersonel.Items.IndexOf(listViewPersonel.SelectedItems[0]);
                    DialogResult sil = new DialogResult();
                    sil = MessageBox.Show("Kaydı silmek istediğinizden Emin misiniz?", "Uyarı", MessageBoxButtons.YesNo);
                    if (sil == DialogResult.Yes)
                    {

                        personelListesi.RemoveAt(selectedIndex);
                        listViewPersonel.Items.RemoveAt(selectedIndex);

                        dataBaseKontrol.PERSONELBILGILERINIKAYDET();

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listViewPersonel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (listViewPersonel.SelectedItems.Count > 0)
                { 
                    if (listViewPersonel.SelectedItems[0] != null)
                    {
                        int selectedIndex = listViewPersonel.Items.IndexOf(listViewPersonel.SelectedItems[0]);

                        textBoxPersonelId.Text = personelListesi[selectedIndex].Id;
                        textBoxAd.Text = personelListesi[selectedIndex].Ad;
                        textBoxSoyad.Text = personelListesi[selectedIndex].Soyad;
                        dateTimePickerDogumTarihi.Value = Convert.ToDateTime(personelListesi[selectedIndex].DogumTarihi);
                        maskedTextBoxTelefon.Text = personelListesi[selectedIndex].Telefon;
                        textBoxEmail.Text = personelListesi[selectedIndex].Email;
                        textBoxAdres.Text = personelListesi[selectedIndex].Adres;
                        dateTimePickerIseGiris.Value = Convert.ToDateTime(personelListesi[selectedIndex].IseGiris);
                        comboBoxUnvan.SelectedIndex = comboBoxUnvan.FindStringExact(personelListesi[selectedIndex].Unvan);
                        if (!string.IsNullOrWhiteSpace(personelListesi[selectedIndex].Resim))
                        {
                            pictureBox1.ImageLocation = personelListesi[selectedIndex].Resim;
                        }
                        else
                        {
                            if (pictureBox1.Image != null)
                            {
                                pictureBox1.Image.Dispose();
                                pictureBox1.Image = null;
                                pictureBox1.Update();
                            }
                        }
                        buttonKaydet.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonResimSec_Click(object sender, EventArgs e)
        {
            // Wrap the creation of the OpenFileDialog instance in a using statement,
            // rather than manually calling the Dispose method to ensure proper disposal
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Resim Seç";
                dlg.Filter = "Resim Dosyaları (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // Create a new Bitmap object from the picture file on disk,
                    // and assign that to the PictureBox.Image property
                    //pictureBox1.Image = new Bitmap(dlg.FileName);

                    pictureBox1.ImageLocation = dlg.FileName;

                }
            }
        }

        private void buttonGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(listViewPersonel.SelectedItems.Count > 0))
                    throw new Exception("Lütfen aşağıdaki listeden güncellemek istediğiniz personel kaydını seçiniz");

                if (listViewPersonel.SelectedItems[0] != null)
                {
                    int selectedIndex = listViewPersonel.Items.IndexOf(listViewPersonel.SelectedItems[0]);
                    
                    DialogResult sil = new DialogResult();
                    sil = MessageBox.Show("Kaydı güncellemek istediğinizden Emin misiniz?", "Uyarı", MessageBoxButtons.YesNo);
                    if (sil == DialogResult.Yes)
                    {

                        if (dataBaseKontrol.GirdiKontrol(this, selectedIndex))
                        {
                            ClearForm();

                            personelListesi = dataBaseKontrol.PersonelKayitlari;
                            AddPersonelListToListView(personelListesi);
                        }
                        buttonKaydet.Enabled = true;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
