using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KNN
{
    public partial class Form1 : Form
    {
        string[] snf = { "A", "B" };
        private BindingSource bindingSource1;

        public Form1()
        {
            InitializeComponent();

            initApp();
        }

        private void initApp()
        {
            bindingSource1 = new BindingSource();
            dataGridView1.DataSource = bindingSource1;
            listBox1.DisplayMember = "uzaklikDeger";
            listBox2.DisplayMember = "uzaklikDeger";
            for (int i = 0; i < 4; i++)
            {

                bindingSource1.Add(new Kisi(bindingSource1, 20 + i, 20 + 100 * i, snf[(i + 2) % 2]));
                dataGridView1.DataSource = bindingSource1;
            }
        }

        private void buttonEkle_Click(object sender, EventArgs e)
        {
            int yas= Convert.ToInt32(textBox1.Text);
            int gelir=Convert.ToInt32( textBox2.Text);
            string sinif= comboBoxSinif.SelectedItem.ToString();
            bindingSource1.Add(new Kisi(bindingSource1, yas, gelir, sinif));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxSinif.Items.AddRange(snf);
        }

        private void BtnHesapla_Click(object sender, EventArgs e)
        {

            int yeni_yas= Convert.ToInt32(textBoxHY.Text);
            int yeni_gelir =Convert.ToInt32(textBoxHG.Text);
            int k = Convert.ToInt32(textBoxK.Text);

            KisiUzaklik[] k_nn= new KisiUzaklik[k];

            List<KisiUzaklik> uzakliklarA = new List<KisiUzaklik>();
            List<KisiUzaklik> uzakliklarB = new List<KisiUzaklik>();
            double satir = dataGridView1.RowCount - 1;
            for (int i = 0; i < satir; i++)
            {
                int id= Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value);
                int yas= Convert.ToInt32(dataGridView1.Rows[i].Cells[1].Value);
                int gelir= Convert.ToInt32(dataGridView1.Rows[i].Cells[2].Value);
                string sinif= dataGridView1.Rows[i].Cells[3].Value.ToString();
                double x= yas-yeni_yas;
                double y= gelir-yeni_gelir;

                double uzaklik= Math.Sqrt(Math.Pow(x,2)+Math.Pow(y,2));

                KisiUzaklik ks = new KisiUzaklik(id, uzaklik, sinif);

                diziyeEkle(k_nn,ks);

                if (sinif.Equals(snf[0]))
                {
                    uzakliklarA.Add(ks);
                }
                else
                {
                    uzakliklarB.Add(ks);
                }
            }

            string yeni_sinif = uzaklikAnalizEt(k_nn);

            listBox1.DataSource = uzakliklarA;
            listBox2.DataSource = uzakliklarB;
            listBox3.Items.Add("sinif: "+yeni_sinif);
        }

        private void diziyeEkle(KisiUzaklik[] k_nn,KisiUzaklik ks)
        {
            for (int i = 0; i < k_nn.Length; i++)
            {
                if (k_nn[i] == null)
                {
                    k_nn[i] = ks;
                    return;
                }
            }

            int max_index=0;
            double max_uzaklik = k_nn[max_index].uzaklikDouble;
            for (int i = 1; i < k_nn.Length; i++)
            {
                if (k_nn[i].uzaklikDouble > max_uzaklik)
                {
                    max_uzaklik = k_nn[i].uzaklikDouble;
                    max_index = i;
                }
            }
            if (max_uzaklik > ks.uzaklikDouble)
            {
                k_nn[max_index] = ks;
            }
            return;
        }

        private string uzaklikAnalizEt(KisiUzaklik[] k_nn)
        {
            int[] analiz = new int[2];
            analiz[0]=0;
            analiz[1]=0;

            for (int i = 0; i<k_nn.Length; i++)
            {
                if (k_nn[i].snf.Equals(snf[0]))
                {
                    analiz[0] = analiz[0]+1;
                }
                else
                {
                    analiz[1] = analiz[1] + 1;
                }
            }

            if (analiz[0] > analiz[1])
            {
                return snf[0];
            }
            else
            {
                return snf[1];
            }
        }

        #region "kisi object"
        private class Kisi
        {
            private int yas;
            private int id;
            private int gelir;
            private String sinif;

            public Kisi(BindingSource bindingSource, int yas,int gelir, string sinif)
            {
                this.id = bindingSource.Count;
                this.yas=yas;
                this.gelir=gelir;
                this.sinif=sinif;

            }

            public Kisi()
            {

            }

            public int ID
            {
                get
                {
                    return id;
                }

                set
                {
                    id = value;
                }
            }

            public int YAS
            {
                get
                {
                    return yas;
                }

                set
                {
                    yas = value;
                }
            }

            public int GELIR
            {
                get
                {
                    return gelir;
                }
                set
                {
                    gelir = value;
                }
            }

            public string SINIF
            {
                get
                {
                    return sinif;
                }
                set
                {
                    sinif = value;
                }
            }
        }
        #endregion

        #region "KisiUzaklik object"
        private class KisiUzaklik
        {
            private int id;
            private double uzaklik;
            private string sinif;

            public KisiUzaklik(int id, double uzaklik, string sinif)
            {
                this.id = id;
                this.uzaklik = uzaklik;
                this.sinif = sinif;
            }

            public KisiUzaklik()
            {
            }

            public int ID
            {
                get
                {
                    return id;
                }

                set
                {
                    id = value;
                }
            }

            public string uzaklikDeger
            {
                get
                {
                    return id+": "+uzaklik;
                }
                
            }

            public double uzaklikDouble
            {
                get
                {
                    return uzaklik;
                }

            }

            public string snf
            {
                get
                {
                    return sinif;
                }
            }
        }
        #endregion

    }

    
}
