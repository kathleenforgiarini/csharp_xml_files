using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

// CODED BY KATHLEEN FORGIARINI
// 2023-03-30
// LAB 8.1

namespace lab8._1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        string dir = @".\XML\";
        private void write_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if (!Regex.IsMatch(textBox1.Text, @"^[A-Z0-9]{4}$"))
                {
                    MessageBox.Show("Product code must be 4 characters of capitalized letters and/or numbers only.");
                    return;
                }

                if (!Regex.IsMatch(textBox2.Text, @"^[A-Za-z0-9\s\W]{0,50}$"))
                {
                    MessageBox.Show("Description must be up to 50 characters of letters, numbers, and special characters.");
                    return;
                }

                decimal price;
                if (!decimal.TryParse(textBox3.Text, out price) || price < 0 || price > 999.99m)
                {
                    MessageBox.Show("Price must be a decimal number representing a value up to 999.99.");
                    return;
                }

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true; settings.IndentChars = (" ");

                XmlWriter xmlOut = XmlWriter.Create(dir + "Product.xml", settings);

                xmlOut.WriteStartDocument();
                xmlOut.WriteStartElement("Users");
                xmlOut.WriteStartElement("Product");
                xmlOut.WriteElementString("Code", textBox1.Text);
                xmlOut.WriteElementString("Description", textBox2.Text);
                xmlOut.WriteElementString("Price", textBox3.Text);
                xmlOut.WriteEndElement();
                xmlOut.Close();

                MessageBox.Show("Product data saved to XML file.");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void read_Click(object sender, EventArgs e)
        {
            XmlReaderSettings settings = new XmlReaderSettings();           
            settings.IgnoreWhitespace = true;           
            settings.IgnoreComments = true;           
            XmlReader xmlIn = XmlReader.Create(dir + "Product.xml", settings);              
            string tempStr = "", CODE="", DESC="", PRICE="";              
            if (xmlIn.ReadToDescendant("Product"))           
            {                 
                do            
                {                      
                    xmlIn.ReadStartElement("Product");                
                    CODE = xmlIn.ReadElementContentAsString();                     
                    DESC = xmlIn.ReadElementContentAsString();
                    PRICE = xmlIn.ReadElementContentAsString();                     
                    tempStr += CODE + ", " + DESC + ", " + PRICE + "\n";           
                }              
                while(xmlIn.ReadToNextSibling("Product"));            
            }           
            xmlIn.Close();             
            MessageBox.Show(tempStr);          
        }
    }
}
