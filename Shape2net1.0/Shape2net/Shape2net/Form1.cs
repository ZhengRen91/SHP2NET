using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;




namespace Shape2net
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string ShapeFilePath = "";
        private string ShapeFileName = "";
        private string NetFilePath = "";

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = @"D:\study\ao\Shape2net";
            openFileDialog1.FileName = "Inputshapefile";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Filter = "shapefile(*.shp)|*.shp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                int index  = textBox1.Text.LastIndexOf(@"\");
                string path = textBox1.Text.Substring(0,index);
                ShapeFilePath = path;
                int length = textBox1.Text.Length - path.Length - 1;
                ShapeFileName = textBox1.Text.Substring(index+1,length);
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = @"D:\study\ao\Shape2net";
            saveFileDialog1.Filter = "net(*.net)|*.net";
            saveFileDialog1.FileName = "pajeknetfile";
            saveFileDialog1.Title = "Save net file";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) 
            {
                textBox2.Text = saveFileDialog1.FileName;
                NetFilePath = textBox2.Text;
            }

        }
            
        private void button3_Click(object sender, EventArgs e)
        {
            ConvertToNet shpToNet = new ConvertToNet(this.ShapeFilePath,this.ShapeFileName, this.NetFilePath);
            shpToNet.SaveNetFile(NetFilePath);
            MessageBox.Show("Net file saved!");
        }
    }
}
