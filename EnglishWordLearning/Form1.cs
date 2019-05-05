using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common.DTO;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace EnglishWordLearning
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private IWebDriver driver = new PhantomJSDriver();
        List<Word> wordlist = new List<Word>();
        DictioneryDataset IKnowWordsDs = new DictioneryDataset();
        DictioneryDataset IgnoreWordsDs = new DictioneryDataset();
        Dictionary<string, string> MyDic = new Dictionary<string, string>();
        private void Form1_Load(object sender, EventArgs e)
        {



            //IWebDriver driver = new ChromeDriver();
        }
        // IWebDriver driver = new PhantomJSDriver();

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = GetFrom_amdzSite(textBox1.Text);
            button1.Enabled = false;
            Thread.Sleep(4000);
            button1.Enabled = true;
        }

        private void AddToIKnow(String str)
        {
            IKnowWordsDs.DicTable.AddDicTableRow(str, "");
            lbliKnow.Text = IKnowWordsDs.DicTable.Count.ToString();
        }

        private void AddToIgnore(String str)
        {
            IgnoreWordsDs.DicTable.AddDicTableRow(str, "");
            lblIgnore.Text = IgnoreWordsDs.DicTable.Count.ToString();
        }


        private string GetFrom_amdzSite(String Word)
        {
            try
            {
                driver.Navigate().GoToUrl(@"http://dic.amdz.com");
                IWebElement EnglishInputElement
                    = driver.FindElement(By.XPath("//*[@id='leftside']/form/table/tbody/tr[1]/td[2]/div/input"));
                EnglishInputElement.SendKeys(Word);

                IWebElement SubmitBtn =
                    driver.FindElement(By.XPath("//*[@id='leftside']/form/table/tbody/tr[3]/td[2]/div/input[2]"));
                SubmitBtn.Submit();

                IWebDriver IframeDriver = driver.SwitchTo().Frame(driver.FindElement(By.Name("result")));
                IWebElement ResultElement = IframeDriver.FindElement(By.CssSelector("body > div .definition"));

                return ResultElement.Text;
            }

            catch (Exception e)
            {
                return "";
            }


        }
        private string GetFrom_tukarailSite(String Word)
        {
            try
            {
                driver.Navigate().GoToUrl(@"https://pichak.net/blogcod/dictionary/dic/index.php");

                IWebElement EnglishInputElement
                    = driver.FindElement(By.XPath("//*[@id='myForm']/table/tbody/tr[2]/td/p/input"));
                EnglishInputElement.Clear();
                Thread.Sleep(100);

                EnglishInputElement.SendKeys(Word);
                Thread.Sleep(100);

                IWebElement fromElement =
                                    driver.FindElement(By.XPath("//*[@id='myForm']/table/tbody/tr[3]/td[2]/select"));
                fromElement.SendKeys("English");
                fromElement.Submit();
                Thread.Sleep(100);

                IWebElement toElement =
                    driver.FindElement(By.XPath("//*[@id='myForm']/table/tbody/tr[4]/td[2]/select"));
                toElement.SendKeys("پارسي");
                toElement.Submit();
                Thread.Sleep(100);

                IWebElement SubmitBtn =
                    driver.FindElement(By.XPath("//*[@id='myForm']/table/tbody/tr[5]/td/p/input"));
                SubmitBtn.Submit();
                Thread.Sleep(2000);


                IWebElement ResultElement =
                    driver.FindElement(By.CssSelector("#main-right"));
                return ResultElement.Text;
            }
            catch (Exception e)
            {
                return "";// + " : " + e.Message;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox3.Text))
               textBox3_DoubleClick(sender, e);
             // --------------  load dictionery 
             FileInfo fileInfo = new System.IO.FileInfo(textBox3.Text);
            List<DicEntity> allDic = Business.Dictionery.instance.GetAll();

            foreach (DicEntity dicEntity in allDic)
            {
                if (!MyDic.ContainsKey(dicEntity.Word))
                    MyDic.Add(dicEntity.Word, dicEntity.Meaning);
            }

            //------------------------------------

            List<string> RemoveWorsd = new List<string>();
            string readAllText = System.IO.File.ReadAllText(textBox3.Text.Trim());
            string[] StopWords = System.IO.File.ReadAllLines(System.IO.Path.Combine(Application.StartupPath, "StopWord.txt"));
            RemoveWorsd.AddRange(StopWords.ToList());

            //************************** remove Ignore and IKnow ********************

            String IgnoreWordsFileName = fileInfo.Name.Replace(fileInfo.Extension, "").Replace(".", "_") + "IgnoreWords.xml";
            String IKnowWordsFileName = fileInfo.Name.Replace(fileInfo.Extension, "").Replace(".", "_") + "IKnowWords.xml";

            DictioneryDataset RemoveWordDataset = new DictioneryDataset();
            if (System.IO.File.Exists(System.IO.Path.Combine(fileInfo.DirectoryName, IgnoreWordsFileName)))
            {
                RemoveWordDataset.ReadXml(System.IO.Path.Combine(fileInfo.DirectoryName, IgnoreWordsFileName));
                for (int i = 0; i < RemoveWordDataset.DicTable.Rows.Count; i++)
                {
                    RemoveWorsd.Add(RemoveWordDataset.DicTable[i].Word);
                    AddToIgnore(RemoveWordDataset.DicTable[i].Word);
                }


            }
            if (System.IO.File.Exists(System.IO.Path.Combine(fileInfo.DirectoryName, IKnowWordsFileName)))
            {
                RemoveWordDataset = new DictioneryDataset();
                RemoveWordDataset.ReadXml(System.IO.Path.Combine(fileInfo.DirectoryName, IKnowWordsFileName));
                for (int i = 0; i < RemoveWordDataset.DicTable.Rows.Count; i++)
                {
                    RemoveWorsd.Add(RemoveWordDataset.DicTable[i].Word);
                    AddToIKnow(RemoveWordDataset.DicTable[i].Word);

                }
            }
            //************************** remove Ignore and IKnow ********************


            string[] Signs = System.IO.File.ReadAllLines(System.IO.Path.Combine(Application.StartupPath, "Signs.txt"));
            Signs.ToList().ForEach(f => readAllText = readAllText.Replace(f, " "));

            List<Word> words = readAllText.Replace("—", " ").Split(' ').Select(s => new Word() { Text = s.ToLower().Trim() }).Where(w => !RemoveWorsd.Any(a => a.ToLower().Trim() == w.Text.Trim()) && w.TextLength >= 3).ToList();

            wordlist = words.GroupBy(g => g.Text).Select(group => new Word()
            {
                Text = group.Key,
                Count = group.Count(),
                Meaning = MyDic.ContainsKey(group.Key) ? MyDic[group.Key] : ""
            }).OrderByDescending(o => o.Count).ToList();


            dataGridView1.DataSource = wordlist;

            label1.Text = "Records = " + dataGridView1.RowCount;
        }

        private void textBox3_DoubleClick(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "*.txt";
            openFileDialog1.ShowDialog();
            textBox3.Text = openFileDialog1.FileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            int counter = 0;
            foreach (Word word in wordlist)
            {
                if (String.IsNullOrEmpty(word.Meaning))
                {
                    if (!MyDic.ContainsKey(word.Text))
                    {
                        counter++;
                        this.Text = counter.ToString();
                    }
                    textBox1.Text = word.Text;
                    word.Meaning = MyDic.ContainsKey(word.Text) ? MyDic[word.Text] : GetAndUpdateDicWord(word.Text);
                    textBox2.Text = word.Meaning;
                    if (string.IsNullOrEmpty(word.Meaning))
                    {
                        if (word.Text.EndsWith("ed")) word.Text = word.Text.Substring(0, word.Text.Length - 2);
                        if (word.Text.EndsWith("’s")) word.Text = word.Text.Substring(0, word.Text.Length - 2);
                        if (word.Text.EndsWith("s")) word.Text = word.Text.Substring(0, word.Text.Length - 1);


                        button1.Enabled = false;
                        Thread.Sleep(MyDic.ContainsKey(word.Text) ? 0 : 4000);
                        button1.Enabled = true;
                        textBox1.Text = word.Text;
                        word.Meaning = MyDic.ContainsKey(word.Text) ? MyDic[word.Text] : GetAndUpdateDicWord(word.Text);
                        textBox2.Text = word.Meaning;
                    }
                    this.Refresh();
                    button1.Enabled = false;
                    Thread.Sleep(MyDic.ContainsKey(word.Text) ? 0 : 4000);
                    button1.Enabled = true;

                    if (counter >= 400)
                        break;
                }

                RefreshGrid();

            }
        }

        public String GetAndUpdateDicWord(String word)
        {
            string Result = GetFrom_amdzSite(word);
            Business.Dictionery.instance.AddWord(new DicEntity() { Word = word, Meaning = Result });
            return Result;
        }
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Word firstOrDefault = wordlist.Where(w => w.Text == dataGridView1[0, e.RowIndex].Value.ToString()).FirstOrDefault();
            if (rdoStopWord.Checked)
            {
                System.IO.File.AppendAllText(System.IO.Path.Combine(Application.StartupPath, "StopWord.txt"), Environment.NewLine + firstOrDefault.Text);
            }
            else if (rdioIgnore.Checked)
            {
                AddToIgnore(firstOrDefault.Text);
            }
            else if (rdioIknow.Checked)
            {
                AddToIKnow(firstOrDefault.Text);
            }

            wordlist.Remove(firstOrDefault);
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = wordlist.OrderByDescending(o => o.Count).ToList();
            label1.Text = "Records = " + dataGridView1.RowCount;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new System.IO.FileInfo(textBox3.Text);
            String FileName = fileInfo.Name.Replace(fileInfo.Extension, "").Replace(".", "_") + "IgnoreWords.xml";
            IgnoreWordsDs.WriteXml(System.IO.Path.Combine(fileInfo.DirectoryName, FileName));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new System.IO.FileInfo(textBox3.Text);
            String FileName = fileInfo.Name.Replace(fileInfo.Extension, "").Replace(".", "_") + "IKnowWords.xml";
            IKnowWordsDs.WriteXml(System.IO.Path.Combine(fileInfo.DirectoryName, FileName));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FileInfo fileInfo = new System.IO.FileInfo(textBox3.Text);
            String FileName = fileInfo.Name.Replace(fileInfo.Extension, "").Replace(".", "_") + "Dictionery.xml";
            DictioneryDataset Dic = new DictioneryDataset();
            wordlist.Where(w => !string.IsNullOrEmpty(w.Meaning)).ToList().ForEach(f => Dic.DicTable.AddDicTableRow(f.Text, f.Meaning));

            Dic.WriteXml(System.IO.Path.Combine(fileInfo.DirectoryName, FileName));
        }


    }
}
