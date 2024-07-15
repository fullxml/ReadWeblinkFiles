using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

namespace ReadWeblinkFiles
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    string folderPath = folderDialog.SelectedPath;
                    ProcessFolder(folderPath);
                }
            }
        }

        private void ProcessFolder(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath, "*.webloc");

            List<string> extractedTexts = new List<string>();

            progressBar.Maximum = files.Length;
            progressBar.Value = 0;

            foreach (string file in files)
            {
                string text = ExtractTextFromWebloc(file);
                extractedTexts.Add(text);
                progressBar.PerformStep();
            }

            WriteSortedTextToFile(extractedTexts);
            MessageBox.Show("Text extraction and sorting completed!");
        }

        private string ExtractTextFromWebloc(string filePath)
        {
            textBox1.Text += filePath + Environment.NewLine + Environment.NewLine;
            XDocument doc = XDocument.Load(filePath, LoadOptions.SetBaseUri);
            XElement stringElement = doc.Descendants("string").FirstOrDefault();
            return stringElement?.Value ?? string.Empty;
        }

        private void WriteSortedTextToFile(List<string> texts)
        {
            texts.Sort();

            using (StreamWriter writer = new StreamWriter("output_sorted.txt"))
            {
                foreach (string text in texts)
                {
                    writer.WriteLine(text);
                }
            }
        }
    }
}
