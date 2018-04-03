using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HelperLibrary;
using System.Net;
using System.Threading;
using PerfLibrary;
using System.IO;
using System.Diagnostics;
using System.Xml;

namespace AdminApp
{
    public partial class AdminApp : Form
    {
        private static readonly string pathData = Application.StartupPath + @"\data.xml";
        private Connection Conn = new Connection(20000);
        private Communication Comm = new Communication(true, true, false);
        private IPAddress address;
        private string[] CounterUse;
        private string[] CounterCollected;
        private string[] resultValue;

        public AdminApp()
        {
            InitializeComponent();
            label4.Text = "";
            label5.Text = "";
            comboBox1.Items.AddRange(Comm.GetAllCommand());
            CounterCollected = null;
            CounterUse = null;
        }

        private void textBoxIP_TextChanged(object sender, EventArgs e)
        {
            if (IPAddress.TryParse(textBoxIP.Text, out address) && textBoxIP.Text.Split('.').Length == 4)
            {
                label4.Text = "Poprawna struktura adresu IP";
                label4.ForeColor = Color.DarkGreen;
            }
            else
            {
                label4.Text = "Niepoprawna struktura adresu IP";
                label4.ForeColor = Color.DarkRed;
            }
        }

        private void textBoxIP_Leave(object sender, EventArgs e)
        {
            if (label4.ForeColor == Color.DarkGreen)
            {
                button2.Enabled = true;
                textBoxIP.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InvisibleCalender();
            InvisibleTrackBar();
            textBoxIP.Enabled = true;
            textBoxIP.Text = String.Empty;
            label4.ForeColor = SystemColors.Control;
            button2.Enabled = false;
            CounterCollected = null;
            CounterUse = null;
            comboBox1.Text = String.Empty;
            comboBox2.Text = String.Empty;
            comboBox3.Text = String.Empty;
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "GET":
                    switch (comboBox2.Text)
                    {
                        case "DATA":
                            if (comboBox3.Text.Length == 0)
                            {
                                MessageBox.Show("Nie wybrano danych.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                            if (monthCalendar1.SelectionRange.Start >= DateTime.Now)
                            {
                                MessageBox.Show("Należy wybrać datę mniejszą niż dzień dzisiejszy.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                            try
                            {
                                Conn.Connect(address, 5000);
                                Comm.PrepareDataFrame(comboBox1.Text, comboBox2.Text, monthCalendar1.SelectionRange.Start.ToString("dd-MM-yyyy"), comboBox3.Text, true, false);
                                Conn.Send(Comm.BufferString);
                                lock (this)
                                {
                                    string communique = "";
                                    string Frame = Conn.Read();
                                    Conn.Disconnect();
                                    Comm.InterpretationOfReceived(Frame, ref communique);
                                    if (communique == "OK")
                                    {
                                        resultValue = Comm.GetResultInterpretation();
                                        string strLevel = "";
                                        for (int i = 0; i < resultValue.Length; i++)
                                            strLevel += resultValue[i] + ";";
                                        strLevel = strLevel.Remove(strLevel.Length - 1);


                                        XmlDocument doc = new XmlDocument();
                                        if (!File.Exists(pathData))
                                        {
                                            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                                            XmlElement DE = doc.DocumentElement;
                                            doc.InsertBefore(xmlDeclaration, DE);
                                            XmlElement root = doc.CreateElement("DATA");
                                            doc.AppendChild(root);
                                        }
                                        else
                                        {
                                            doc.Load(pathData);
                                        }

                                        XmlNodeList clientList = doc.GetElementsByTagName("Client");
                                        XmlElement clientElement = null;

                                        for (int i = 0; i < clientList.Count; i++)
                                        {
                                            if (((XmlElement)clientList[i]).GetAttribute("IP") == textBoxIP.Text)
                                            {
                                                clientElement = (XmlElement)clientList[i];
                                                break;
                                            }
                                        }

                                        if (clientElement == null)
                                        {
                                            clientElement = doc.CreateElement("Client");
                                            clientElement.SetAttribute("IP", textBoxIP.Text);
                                        }

                                        if (clientElement.HasChildNodes)
                                        {
                                            XmlNodeList counterList = clientElement.ChildNodes;
                                            XmlElement counterElement = null;
                                            for (int i = 0; i < counterList.Count; i++)
                                            {
                                                if (((XmlElement)counterList[i]).GetAttribute("Date") == monthCalendar1.SelectionRange.Start.ToString("dd-MM-yyyy") &&
                                                    ((XmlElement)counterList[i]).Name == comboBox3.Text)
                                                {
                                                    counterElement = (XmlElement)counterList[i];
                                                    break;
                                                }
                                            }

                                            if (counterElement == null)
                                            {
                                                counterElement = doc.CreateElement(comboBox3.Text);
                                                counterElement.SetAttribute("Date", monthCalendar1.SelectionRange.Start.ToString("dd-MM-yyyy"));
                                                XmlText valueText = doc.CreateTextNode(strLevel);
                                                counterElement.AppendChild(valueText);
                                                clientElement.AppendChild(counterElement);
                                                doc.DocumentElement.AppendChild(clientElement);
                                            }
                                            else
                                            {
                                                counterElement.InnerXml = strLevel;
                                            }
                                        }
                                        else
                                        {
                                            XmlElement counterElement = doc.CreateElement(comboBox3.Text);
                                            counterElement.SetAttribute("Date", monthCalendar1.SelectionRange.Start.ToString("dd-MM-yyyy"));
                                            XmlText valueText = doc.CreateTextNode(strLevel);
                                            counterElement.AppendChild(valueText);
                                            clientElement.AppendChild(counterElement);
                                            doc.DocumentElement.AppendChild(clientElement);
                                        }

                                        doc.Save(pathData);

                                        MessageBox.Show("Dane zostały pobrane poprawnie.");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Stacja nie posiada danych z wskazanego dnia.");
                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Wystąpił problem z połączeniem z podanym klientem.\nUpewnij się że stacja wraz z aplikacją są uruchomione i spróbuj ponownie.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;

                        case "COUNTUSE":
                            try
                            {
                                Conn.Connect(address, 5000);
                                Comm.PrepareDataFrame(comboBox1.Text, comboBox2.Text, comboBox3.Text, true, false);
                                Conn.Send(Comm.BufferString);
                                lock (this)
                                {
                                    string communique = "";
                                    string Frame = Conn.Read();
                                    Conn.Disconnect();
                                    Comm.InterpretationOfReceived(Frame, ref communique);
                                    if (communique == "OK")
                                    {
                                        CounterUse = Comm.GetResultInterpretation();
                                        MessageBox.Show("Dane zostały pobrane poprawnie.");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Błąd danych.");
                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Wystąpił problem z połączeniem z podanym klientem.\nUpewnij się że stacja wraz z aplikacją są uruchomione i spróbuj ponownie.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;

                        case "COUNTCOL":
                            try
                            {
                                Conn.Connect(address, 5000);
                                Comm.PrepareDataFrame(comboBox1.Text, comboBox2.Text, comboBox3.Text, true, false);
                                Conn.Send(Comm.BufferString);
                                lock (this)
                                {
                                    string communique = "";
                                    string Frame = Conn.Read();
                                    Conn.Disconnect();
                                    Comm.InterpretationOfReceived(Frame, ref communique);
                                    if (communique == "OK")
                                    {
                                        CounterCollected = Comm.GetResultInterpretation();
                                        MessageBox.Show("Dane zostały pobrane poprawnie.");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Błąd danych.");
                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Wystąpił problem z połączeniem z podanym klientem.\nUpewnij się że stacja wraz z aplikacją są uruchomione i spróbuj ponownie.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;

                        default:
                            MessageBox.Show("Nie wybrano parametru.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                    break;


                case "SET":
                    switch (comboBox2.Text)
                    {
                        case "LEVEL":
                            if (comboBox3.Text.Length == 0)
                            {
                                MessageBox.Show("Nie wybrano danych.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                            try
                            {
                                Conn.Connect(address, 5000);
                                Comm.PrepareDataFrame(comboBox1.Text, comboBox2.Text, textBoxLevel1.Text + ";" + textBoxLevel2.Text + ";" + textBoxLevel3.Text, comboBox3.Text, true, false);
                                Conn.Send(Comm.BufferString);
                                lock (this)
                                {
                                    string communique = "";
                                    string Frame = Conn.Read();
                                    Conn.Disconnect();
                                    Comm.InterpretationOfReceived(Frame, ref communique);
                                    if (communique == "OK")
                                    {
                                        MessageBox.Show("Dane zostały zaakceptowane i zmienione po stronie klienta.");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Na stacji najprawdopodobniej nie jest uruchomiony wskazany licznik lub chwilowo dane nie mogą być zmienione.\nJeżeli dane są poprawne spróbuj zmiany za krótką chwilę.");
                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Wystąpił problem z połączeniem z podanym klientem.\nUpewnij się że stacja wraz z aplikacją są uruchomione i spróbuj ponownie.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;

                        default:
                            MessageBox.Show("Nie wybrano parametru.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                    break;

                default:
                    MessageBox.Show("Nie wybrano komendy.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            comboBox2.Text = "";
            comboBox3.Text = "";
            comboBox2.Items.AddRange(Comm.GetAllParam(comboBox1.Text));
            comboBox3.Enabled = true;
            label5.Text = "";
            InvisibleCalender();
            InvisibleTrackBar();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.Text)
            {
                case "DATA":
                    if (CounterCollected != null)
                    {
                        comboBox3.Items.AddRange(CounterCollected);
                        comboBox3.Enabled = true;
                        VisibleCalender();
                        label5.Text = "";
                    }
                    else
                    {
                        label5.Text = "Przed wykonaniem tego polecenia GET->DATA należy\nprzesłać polecenie GET->COUNTCOL";
                        label5.ForeColor = Color.DarkRed;
                        comboBox3.Enabled = false;
                        InvisibleCalender();
                    }
                    break;

                case "COUNTUSE":
                    comboBox3.Enabled = false;
                    label5.Text = "";
                    InvisibleCalender();
                    InvisibleTrackBar();
                    break;

                case "COUNTCOL":
                    goto case "COUNTUSE";

                case "LEVEL":
                    if (CounterUse != null)
                    {
                        comboBox3.Items.AddRange(CounterUse);
                        VisibleTrackBar();
                        label5.Text = "";
                    }
                    else
                    {
                        label5.Text = "Przed wykonaniem tego polecenia SET->LEVEL należy\nprzesłać polecenie GET->COUNTUSE";
                        label5.ForeColor = Color.DarkRed;
                        comboBox3.Enabled = false;
                        InvisibleTrackBar();
                    }
                    break;

            }

        }

        private void InvisibleCalender()
        {
            monthCalendar1.Visible = false;
        }

        private void VisibleCalender()
        {
            monthCalendar1.Visible = true;
        }

        private void InvisibleTrackBar()
        {
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            trackBarLevel1.Visible = false;
            trackBarLevel2.Visible = false;
            trackBarLevel3.Visible = false;
            textBoxLevel1.Visible = false;
            textBoxLevel2.Visible = false;
            textBoxLevel3.Visible = false;
        }

        private void VisibleTrackBar()
        {
            label6.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            label9.Visible = true;
            trackBarLevel1.Visible = true;
            trackBarLevel2.Visible = true;
            trackBarLevel3.Visible = true;
            textBoxLevel1.Visible = true;
            textBoxLevel2.Visible = true;
            textBoxLevel3.Visible = true;
        }

        private void trackBarLevel1_ValueChanged(object sender, EventArgs e)
        {
            textBoxLevel1.Text = trackBarLevel1.Value.ToString();
            trackBarLevel2.Minimum = trackBarLevel1.Value + 1;
            textBoxLevel2.Text = trackBarLevel2.Value.ToString();
        }

        private void trackBarLevel2_ValueChanged(object sender, EventArgs e)
        {
            textBoxLevel2.Text = trackBarLevel2.Value.ToString();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;
            XmlDocument doc = new XmlDocument();
            doc.Load(pathData);

            XmlNodeList clientList = doc.GetElementsByTagName("Client");
            XmlNodeList counterList = null;

            for (int i = 0; i < clientList.Count; i++)
            {
                if (((XmlElement)clientList[i]).GetAttribute("IP") == listBox1.SelectedItem.ToString())
                {
                    counterList = clientList[i].ChildNodes;
                    break;
                }
            }

            for (int i = 0; i < counterList.Count; i++)
            {
                if (!listBox2.Items.Contains(((XmlElement)counterList[i]).Name))
                    listBox2.Items.Add(((XmlElement)counterList[i]).Name);
            }

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
                return;
            listBox3.Items.Clear();
            XmlDocument doc = new XmlDocument();
            doc.Load(pathData);

            XmlNodeList clientList = doc.GetElementsByTagName("Client");
            XmlNodeList counterList = null;

            for (int i = 0; i < clientList.Count; i++)
            {
                if (((XmlElement)clientList[i]).GetAttribute("IP") == listBox1.SelectedItem.ToString())
                {
                    counterList = clientList[i].ChildNodes;
                    break;
                }
            }

            for (int i = 0; i < counterList.Count; i++)
            {
                if (!listBox2.Items.Contains(((XmlElement)counterList[i]).GetAttribute("Date")) && ((XmlElement)counterList[i]).Name == listBox2.SelectedItem.ToString())
                    listBox3.Items.Add(((XmlElement)counterList[i]).GetAttribute("Date"));
            }
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();

            if (!File.Exists(pathData))
            {
                listBox1.Items.Add("Brak danych");
                listBox2.Items.Add("Brak danych");
                listBox3.Items.Add("Brak danych");
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(pathData);

            XmlNodeList clientList = doc.GetElementsByTagName("Client");
            for (int i = 0; i < clientList.Count; i++)
            {
                listBox1.Items.Add(((XmlElement)clientList[i]).GetAttribute("IP"));
            }

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex == -1)
                return;
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
                return;
            listBox3.Items.Clear();
            XmlDocument doc = new XmlDocument();
            doc.Load(pathData);

            XmlNodeList clientList = doc.GetElementsByTagName("Client");
            XmlNodeList counterList = null;

            for (int i = 0; i < clientList.Count; i++)
            {
                if (((XmlElement)clientList[i]).GetAttribute("IP") == listBox1.SelectedItem.ToString())
                {
                    counterList = clientList[i].ChildNodes;
                    break;
                }
            }

            for (int i = 0; i < counterList.Count; i++)
            {
                if (!listBox2.Items.Contains(((XmlElement)counterList[i]).GetAttribute("Date")) && ((XmlElement)counterList[i]).Name == listBox2.SelectedItem.ToString())
                    listBox3.Items.Add(((XmlElement)counterList[i]).GetAttribute("Date"));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Chart formChart = new Chart();
            if (listBox1.SelectedItem == null || listBox2.SelectedItem == null || listBox3.SelectedItem == null)
            {
                MessageBox.Show("Należy zaznaczyć wszystkie pola.");
                return;
            }
            XmlDocument doc = new XmlDocument();
            string strLevelTemp = null;
            string[] strLevel;
            int[] intLevel;
            doc.Load(pathData);

            XmlNodeList clientList = doc.GetElementsByTagName("Client");
            XmlNodeList counterList = null;

            for (int i = 0; i < clientList.Count; i++)
            {
                if (((XmlElement)clientList[i]).GetAttribute("IP") == listBox1.SelectedItem.ToString())
                {
                    counterList = clientList[i].ChildNodes;
                    break;
                }
            }

            for (int i = 0; i < counterList.Count; i++)
            {
                if (((XmlElement)counterList[i]).Name == listBox2.SelectedItem.ToString() && ((XmlElement)counterList[i]).GetAttribute("Date") == listBox3.SelectedItem.ToString())
                    strLevelTemp = ((XmlElement)counterList[i]).InnerXml;
            }
            
            strLevel = strLevelTemp.Split(';');
            intLevel = new int[strLevel.Length];
            string[] label = new string[]{"Niski", "Średni", "Wysoki"};
            Color[] color = new Color[] { Color.Green, Color.Orange, Color.Red };
            for (int i = 0; i < intLevel.Length; i++)
            {
                intLevel[i] = Convert.ToInt32(strLevel[i]);
                   
                formChart.chart1.Series["Stany"].Points.Add(intLevel[i]);
                formChart.chart1.Series["Stany"].Points[i].Color = color[i];
                formChart.chart1.Series["Stany"].Points[i].AxisLabel = label[i];
                formChart.chart1.Series["Stany"].Points[i].LegendText = label[i];
                formChart.chart1.Series["Stany"].Points[i].Label = strLevel[i];
            }
            formChart.chart1.ChartAreas[0].AxisX.Title = "Stany";
            formChart.chart1.ChartAreas[0].AxisY.Title = "Czas";
            formChart.chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            formChart.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            formChart.chart1.Legends[0].Enabled = false;
            formChart.Show();
        }
    }
}
