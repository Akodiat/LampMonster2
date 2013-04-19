using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace LampMonster
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // Main.Start();
        }
        public void sendToDiagram(double[,] data, string name)
        {
           //var series = new System.Windows.Forms.DataVisualization.Charting.Series();

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    chart1.Series[0].Points.AddXY(i, j, data[i,j]);
                }
            }
            chart1.Series[0].Name = name;
        }
    }
}
