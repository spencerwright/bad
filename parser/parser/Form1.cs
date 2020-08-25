using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace parser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Naming the Buttons
            textBox1.Text = @"C:\Users\Spencer Wright\Desktop\Lrad20200810.txt";
            button1.Text = "Parse";
            textBox2.Text = @"C:\Users\Spencer Wright\Desktop\Test.txt";
            textBox3.Text = "VBatt:";
            button2.Text = "4 hour shift";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox2.Text) == true)
            {
                // Checks to see if the file exist
                DialogResult button = MessageBox.Show("Do you want to overwrite text file?", "Warning", MessageBoxButtons.OKCancel);
                if (button != DialogResult.OK)
                {
                    return;
                }
            }
            Console.WriteLine("Parse started");
            ReadFile(textBox1.Text);
            Console.WriteLine("Parse Complete");
            MessageBox.Show("Parse Complete", "Done", MessageBoxButtons.OKCancel);
        }
        public void ReadFile(string fileName)
        {
            // This method will sort and format the inputed text file
            string data = "";
            List<string> one = new List<string>();
            try
            {
                string[] lines = System.IO.File.ReadAllLines(fileName);
                // This loop iterates through each line in the text file 
                foreach (string line in lines)
                {
                    if( line.Contains("PowerModule: Status:") == false)
                    {
                        // This line does not contain Powermodule: Status: move on to the next line 
                        continue;
                    }

                    string[] words = line.Split(' ');
                    //  This loop checks each word in the line in the text file
                    for (int index = 0; index < words.Length; index++)
                    {
                        string word = words[index];                        
                        if (word == "State:")
                        {
                            // If the word is State:                  
                            bool found = false;
                            int i = index + 1;

                            // 
                            while (i< words.Length)
                            {
                                string element = words[i];
                                if (element.Contains("Solar_Charging"))
                                {
                                    // Checks to see if element is Solar_Charging then appends green to data 
                                    index = i;
                                    data += "green ";
                                    found = true;
                                    break;
                                }
                                else if (word == "VBatt:")
                                {
                                    // VBatt: is the last element in the state array in the text file so there is no need to continue checking the line 
                                    index = i - 1;
                                    break;
                                }
                                i++;                                
                            }
                            if (found == false)
                            {
                                // Appends red to data string because Solar_Charging was not found in the line
                                data += "red ";
                            }
      
                        }
                        else if (word =="VBatt:" )
                        {
                            // Checks to see if the word is VBatt: and appends the next element to the data string 
                            string value = words[index + 1];
                            data += value;
                            data = data.Substring(1,data.Length-2);
                            one.Add(data);
                            data = "";
                        }
                        
                        else if (index == 0)
                        {
                            // Appends the first element to the data string and formats the date
                            string date = words[index];
                            string[] value = date.Split(',');
                            DateTime original = DateTime.Parse(value[0]);
                            date = original.ToString("MM/dd/yyyy H:mm");
                            data += " ";
                            data += date + " ";
                            string time = words[index + 1];
                            data += time + " ";
                        }
                    }
                }
            }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            WriteToText(one);
        }

        public void WriteToText(List<string> value)
        {
            // This method writes to a specifed text file 
            string filename = textBox2.Text;
            try
            {
                System.IO.File.AppendAllLines(filename, value);
                
            }
            
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox2.Text) == true)
            {
                // Checks to see if the file exist
                DialogResult button = MessageBox.Show("Do you want to overwrite text file?", "Warning", MessageBoxButtons.OKCancel);
                if (button != DialogResult.OK)
                {
                    return;
                }
            }
            Console.WriteLine("Parse started");
            TimeFile(textBox1.Text);
            Console.WriteLine("Parse Complete");
            MessageBox.Show("Parse Complete", "Done", MessageBoxButtons.OKCancel);
        }
        public void TimeFile(string fileName)
        {
            // This method will find the time in the text file and subtract four hours from the time in the file
            List<string> one = new List<string>();
            try
            {
                string[] lines = System.IO.File.ReadAllLines(fileName);
                
                // This loop will go through each line in the text file, take the time from the text file, and subtract four ours from the time
                foreach (string line in lines)
                {
                    string[] words = line.Split(' ');
                    string setup = words[0] +" " +words[1];
                    DateTime original = DateTime.Parse(setup);
                    DateTime update = original.Add(new TimeSpan(-4, 0, 0));
                    string data = update.ToString("MM/dd/yyyy HH:mm");
                    string power = " "+words[2]+" "+words[3]+" "+words[4];
                    data += power;
                    one.Add(data);                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            WriteToText(one);
        }
    }
}
    
    