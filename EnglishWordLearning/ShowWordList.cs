using Common.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnglishWordLearning
{
    public partial class ShowWordList : Form
    {
        List<Word> Myentities = null;
        public ShowWordList(List<Word> entities )
        {
            Myentities = entities;
            InitializeComponent();
        }

        private void ShowWordList_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Myentities;
            label1.Text = "Records Count = " + Myentities.Count.ToString();
        }
    }
}
