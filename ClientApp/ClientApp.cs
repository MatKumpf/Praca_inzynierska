using System.Windows.Forms;
using HelperLibrary;
using System.Threading;
using System;
using PerfLibrary;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Xml;
using System.Linq.Expressions;
using System.Drawing;

namespace ClientApp
{
    public partial class ClientApp : Form
    {

        private static readonly string pathConfig = Application.StartupPath + @"\config.xml";
        private static PerfCounters PC;
        private static Thread commService = new Thread(new ThreadStart(CommService));
        private static string communique;
        private static Thread readService = new Thread(new ThreadStart(ReadService));
        private static Listening List = new Listening(20000, true);

        public ClientApp()
        {
            InitializeComponent();
            communique = "";
            if (File.Exists(pathConfig))
            {
                string pathData;
                string pathSummary;
                int interval;
                string[,] data = ReadConfig(out pathData, out pathSummary, out interval);

                textBoxPathData.Text = pathData;
                textBoxPathSummary.Text = pathSummary;
                numericUpDownInterval.Value = interval;
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    if (data[i, 0] == "Processor")
                    {
                        checkBoxProcessor.Enabled = false;
                        checkBoxProcessor.Checked = true;
                        string[] levels = data[i, 3].Split(';');
                        for (int j = 0; j < levels.Length; j++)
                        {
                            this.Controls["textBoxProcessorLevel" + (j + 1).ToString()].Text = levels[j];
                        }
                    }
                    else if (data[i, 0] == "Memory")
                    {
                        checkBoxMemory.Enabled = false;
                        checkBoxMemory.Checked = true;
                        string[] levels = data[i, 2].Split(';');
                        for (int j = 0; j < levels.Length; j++)
                        {
                            this.Controls["textBoxMemoryLevel" + (j + 1).ToString()].Text = levels[j];
                        }
                    }
                    else if (data[i, 0] == "PhysicalDisk")
                    {
                        checkBoxDisk.Enabled = false;
                        checkBoxDisk.Checked = true;
                        string[] levels = data[i, 3].Split(';');
                        for (int j = 0; j < levels.Length; j++)
                        {
                            this.Controls["textBoxDiskLevel" + (j + 1).ToString()].Text = levels[j];
                        }
                    }
                }

                PrepareToCollecting(data, pathData, pathSummary, interval);
                label9.Text = "Włączony";
                PC.Summary();
            }
            else
                label9.Text = "Wyłączony";
            commService.Start();
        }

        private bool TestAccess(string path)
        {
            if (!CanWrite(path + "temp.txt"))
            {
                return false;
            }
            else if (!CanRead(path + "temp.txt"))
            {
                return false;
            }
            else if (!CanDelete(path + "temp.txt"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CanDelete(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        private bool CanWrite(string path)
        {
            try
            {
                File.WriteAllBytes(path, new byte[] { 0 });
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        private bool CanRead(string path)
        {
            try
            {
                FileStream fs = File.OpenRead(path);
                fs.Close();
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        private static void CommService()
        {
            List.StartListening();

            while (true)
            {
                Thread.Sleep(100);
                if (List.Connected)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    sw.Start();
                    readService.Start();
                    communique = "";
                    while (true)
                    {
                        Thread.Sleep(100);
                        if ((sw.ElapsedMilliseconds > 10000 && communique == "") || communique == null)
                        {
                            List.StopListeningOrBreakConnection();
                            sw.Stop();
                            readService.Abort();
                            readService = new Thread(new ThreadStart(ReadService)); 
                            break;
                        }
                        if (communique != null && communique != "")
                        {
                            PerfLibrary.Communication Comm = new PerfLibrary.Communication(false, false, true);
                            Comm.InterpretationOfCommandFrame(communique, ref PC);
                            Comm.PrepareDataFrameWithReceivedFrame(true, false);
                            List.Send(Comm.BufferString);
                            if (Comm.LastReceivedParameter == "LEVEL")
                            {
                                string levels = Comm.LastReceivedData.Split(':')[1];
                                string counter = Comm.LastReceivedData.Split(':')[0].Split('-')[0];
                                if (levels.Split(';').Length == 3)
                                {
                                    XmlDocument doc = new XmlDocument();
                                    doc.Load(pathConfig);
                                    XmlNodeList xnl = doc.GetElementsByTagName("Counters");
                                    XmlNodeList xnlChild = xnl[0].ChildNodes;
                                    for (int i = 0; i < xnlChild.Count; i++)
                                    {
                                        XmlElement element = (XmlElement)xnlChild[i];
                                        if (element.GetAttribute("Category") == counter)
                                        {
                                            element.InnerXml = levels;
                                            break;
                                        }
                                    }
                                    doc.Save(pathConfig);
                                }
                            }
                            communique = "";
                            sw.Restart();
                        }
                    }
                }
            }

        }

        private static void ReadService()
        {
            while (true)
            {
                try
                {
                    if(List.Connected)
                        communique = List.Read();
                }
                catch
                {
                    communique = null;
                }
            }
        } 

        private void ClientApp_FormClosed(object sender, FormClosedEventArgs e)
        {
            commService.Abort();
            PC.StopCollecting();
        }

        private void checkBoxProcessor_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxProcessor.Checked && checkBoxProcessor.Enabled)
            {
                trackBarProcessorLevel1.Enabled = true;
                trackBarProcessorLevel2.Enabled = true;
            }
            else
            {
                trackBarProcessorLevel1.Enabled = false;
                trackBarProcessorLevel2.Enabled = false;
            }
        }

        private void checkBoxMemory_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxMemory.Checked && checkBoxProcessor.Enabled)
            {
                trackBarMemoryLevel1.Enabled = true;
                trackBarMemoryLevel2.Enabled = true;
            }
            else
            {
                trackBarMemoryLevel1.Enabled = false;
                trackBarMemoryLevel2.Enabled = false;
            }
        }

        private void checkBoxDisk_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxDisk.Checked && checkBoxProcessor.Enabled)
            {
                trackBarDiskLevel1.Enabled = true;
                trackBarDiskLevel2.Enabled = true;
            }
            else
            {
                trackBarDiskLevel1.Enabled = false;
                trackBarDiskLevel2.Enabled = false;
            }
        }

        private void trackBarProcessorLevel1_ValueChanged(object sender, EventArgs e)
        {
            textBoxProcessorLevel1.Text = trackBarProcessorLevel1.Value.ToString();
            trackBarProcessorLevel2.Minimum = trackBarProcessorLevel1.Value + 1;
            textBoxProcessorLevel2.Text = trackBarProcessorLevel2.Value.ToString();
        }

        private void trackBarProcessorLevel2_ValueChanged(object sender, EventArgs e)
        {
            textBoxProcessorLevel2.Text = trackBarProcessorLevel2.Value.ToString();
        }

        private void trackBarMemoryLevel1_ValueChanged(object sender, EventArgs e)
        {
            textBoxMemoryLevel1.Text = trackBarMemoryLevel1.Value.ToString();
            trackBarMemoryLevel2.Minimum = trackBarMemoryLevel1.Value + 1;
            textBoxMemoryLevel2.Text = trackBarMemoryLevel2.Value.ToString();
        }

        private void trackBarMemoryLevel2_ValueChanged(object sender, EventArgs e)
        {
            textBoxMemoryLevel2.Text = trackBarMemoryLevel2.Value.ToString();
        }

        private void trackBarDiskLevel1_ValueChanged(object sender, EventArgs e)
        {
            textBoxDiskLevel1.Text = trackBarDiskLevel1.Value.ToString();
            trackBarDiskLevel2.Minimum = trackBarDiskLevel1.Value + 1;
            textBoxDiskLevel2.Text = trackBarDiskLevel2.Value.ToString();
        }

        private void trackBarDiskLevel2_ValueChanged(object sender, EventArgs e)
        {
            textBoxDiskLevel2.Text = trackBarDiskLevel2.Value.ToString();
        }

        private void textBoxPathData_TextChanged(object sender, EventArgs e)
        {
            textBoxPathData.SelectionStart = textBoxPathData.Text.Length - 1;
        }

        private void textBoxPathSummary_TextChanged(object sender, EventArgs e)
        {
            textBoxPathSummary.SelectionStart = textBoxPathSummary.Text.Length - 1;
        }

        private void buttonPathData_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            if (TestAccess(fbd.SelectedPath))
                textBoxPathData.Text = @fbd.SelectedPath;
            else
                MessageBox.Show("Wybrany katalog nie posiada jednego z niezbędnych uprawnień (odczyt, zapis lub możliwość usunięcia pliku).\n" +
                    "Wskaż katalog który spełnia te wymagania.", "Błąd wyboru katalogu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void buttonPathSummary_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            if (TestAccess(fbd.SelectedPath))
                textBoxPathSummary.Text = @fbd.SelectedPath;
            else
                MessageBox.Show("Wybrany katalog nie posiada jednego z niezbędnych uprawnień (odczyt, zapis lub możliwość usunięcia pliku).\n" +
                    "Wskaż katalog który spełnia te wymagania.", "Błąd wyboru katalogu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (!checkBoxProcessor.Checked && !checkBoxMemory.Checked && !checkBoxDisk.Checked)
            {
                MessageBox.Show("Należy wybrać co najmniej jeden licznik.", "Błąd zapisu ustawień", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBoxPathData.Text == "")
            {
                MessageBox.Show("Należy wybrać katalog docelowy dla statystyk dziennych.", "Błąd zapisu ustawień", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBoxPathSummary.Text == "")
            {
                MessageBox.Show("Należy wybrać katalog docelowy dla statystyk zbiorczych.", "Błąd zapisu ustawień", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                XmlDocument ConfigXML = new XmlDocument();

                if (File.Exists(pathConfig))
                {
                    File.Delete(pathConfig);
                }

                FileStream fs = new FileStream(pathConfig, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                XmlDeclaration xmlDeclaration = ConfigXML.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement DE = ConfigXML.DocumentElement;
                ConfigXML.InsertBefore(xmlDeclaration, DE);
                XmlElement root = ConfigXML.CreateElement("Config");
                ConfigXML.AppendChild(root);

                XmlElement Counters = ConfigXML.CreateElement("Counters");
                if (checkBoxProcessor.Checked)
                {
                    XmlElement Processor = ConfigXML.CreateElement("Processor");
                    Processor.SetAttribute("Category", "Processor");
                    Processor.SetAttribute("Instance", "_Total");
                    Processor.SetAttribute("Counter", "% Processor Time");
                    XmlText ProcessorLevel = ConfigXML.CreateTextNode(textBoxProcessorLevel1.Text + ";" + textBoxProcessorLevel2.Text + ";" + textBoxProcessorLevel3.Text);
                    Processor.AppendChild(ProcessorLevel);
                    Counters.AppendChild(Processor);
                }
                if (checkBoxMemory.Checked)
                {
                    XmlElement Memory = ConfigXML.CreateElement("Memory");
                    Memory.SetAttribute("Category", "Memory");
                    Memory.SetAttribute("Counter", "% Committed Bytes in Use");
                    XmlText MemoryLevel = ConfigXML.CreateTextNode(textBoxMemoryLevel1.Text + ";" + textBoxMemoryLevel2.Text + ";" + textBoxMemoryLevel3.Text);
                    Memory.AppendChild(MemoryLevel);
                    Counters.AppendChild(Memory);
                }
                if (checkBoxDisk.Checked)
                {
                    XmlElement Disk = ConfigXML.CreateElement("Disk");
                    Disk.SetAttribute("Category", "PhysicalDisk");
                    Disk.SetAttribute("Instance", "_Total");
                    Disk.SetAttribute("Counter", "% Idle Time");
                    XmlText DiskLevel = ConfigXML.CreateTextNode(textBoxDiskLevel1.Text + ";" + textBoxDiskLevel2.Text + ";" + textBoxDiskLevel3.Text);
                    Disk.AppendChild(DiskLevel);
                    Counters.AppendChild(Disk);
                }

                root.AppendChild(Counters);

                XmlElement Interval = ConfigXML.CreateElement("Interval");
                XmlText IntervalText = ConfigXML.CreateTextNode(numericUpDownInterval.Value.ToString());
                Interval.AppendChild(IntervalText);

                root.AppendChild(Interval);

                XmlElement pathData = ConfigXML.CreateElement("PathData");
                XmlText pathDataText = ConfigXML.CreateTextNode(textBoxPathData.Text);
                pathData.AppendChild(pathDataText);

                root.AppendChild(pathData);

                XmlElement pathSummary = ConfigXML.CreateElement("PathSummary");
                XmlText pathSummaryText = ConfigXML.CreateTextNode(textBoxPathSummary.Text);
                pathSummary.AppendChild(pathSummaryText);

                root.AppendChild(pathSummary);

                ConfigXML.Save(fs);
                fs.Close();
            }
            string[,] data = ReadConfig();
            PrepareToCollecting(data, textBoxPathData.Text, textBoxPathSummary.Text, (int)numericUpDownInterval.Value);
            label9.Text = "Włączony";

        }

        private string[,] ReadConfig(out string PathData, out string PathSummary, out int Interval)
        {
            XmlDocument ConfigXML = new XmlDocument();
            FileStream fs = new FileStream(pathConfig, FileMode.Open, FileAccess.Read);
            ConfigXML.Load(fs);

            XmlNodeList pathDataXml = ConfigXML.GetElementsByTagName("PathData");
            XmlNodeList pathSummaryXml = ConfigXML.GetElementsByTagName("PathSummary");
            XmlNodeList intervalXml = ConfigXML.GetElementsByTagName("Interval");

            string pathData;
            PathData = pathData = ((XmlElement)pathDataXml[0]).InnerXml;
            string pathSummary;
            PathSummary = pathSummary = ((XmlElement)pathSummaryXml[0]).InnerXml;
            int interval;
            Interval = interval = Convert.ToInt32(((XmlElement)intervalXml[0]).InnerXml);

            PC = new PerfCounters(pathData, pathSummary, interval);

            XmlNodeList Counters = ConfigXML.DocumentElement.FirstChild.ChildNodes;

            string[,] temp = new string[Counters.Count, 4];

            for (int i = 0; i < Counters.Count; i++)
            {
                XmlElement element = (XmlElement)Counters[i];
                string category = element.GetAttribute("Category");
                string instance = element.GetAttribute("Instance");
                string counter = element.GetAttribute("Counter");
                string[] strLevel = element.InnerXml.Split(';');
                int[] level = new int[strLevel.Length];
                for (int j = 0; j < level.Length; j++)
                    level[j] = Convert.ToInt32(strLevel[j]);
                if (instance != "")
                {
                    temp[i, 0] = category;
                    temp[i, 1] = counter;
                    temp[i, 2] = instance;
                    temp[i, 3] = element.InnerXml;
                }
                else
                {
                    temp[i, 0] = category;
                    temp[i, 1] = counter;
                    temp[i, 2] = element.InnerXml;
                    temp[i, 3] = null;
                }
            }

            return temp;
        }

        private string[,] ReadConfig()
        {
            XmlDocument ConfigXML = new XmlDocument();
            FileStream fs = new FileStream(pathConfig, FileMode.Open, FileAccess.Read);
            ConfigXML.Load(fs);

            XmlNodeList pathDataXml = ConfigXML.GetElementsByTagName("PathData");
            XmlNodeList pathSummaryXml = ConfigXML.GetElementsByTagName("PathSummary");
            XmlNodeList intervalXml = ConfigXML.GetElementsByTagName("Interval");

            string pathData = ((XmlElement)pathDataXml[0]).InnerXml;
            string pathSummary = ((XmlElement)pathSummaryXml[0]).InnerXml;
            int interval = Convert.ToInt32(((XmlElement)intervalXml[0]).InnerXml);

            PC = new PerfCounters(pathData, pathSummary, interval);

            XmlNodeList Counters = ConfigXML.DocumentElement.FirstChild.ChildNodes;

            string[,] temp = new string[Counters.Count, 4];

            for (int i = 0; i < Counters.Count; i++)
            {
                XmlElement element = (XmlElement)Counters[i];
                string category = element.GetAttribute("Category");
                string instance = element.GetAttribute("Instance");
                string counter = element.GetAttribute("Counter");
                string[] strLevel = element.InnerXml.Split(';');
                int[] level = new int[strLevel.Length];
                for (int j = 0; j < level.Length; j++)
                    level[j] = Convert.ToInt32(strLevel[j]);
                if (instance != "")
                {
                    temp[i, 0] = category;
                    temp[i, 1] = counter;
                    temp[i, 2] = instance;
                    temp[i, 3] = element.InnerXml;
                }
                else
                {
                    temp[i, 0] = category;
                    temp[i, 1] = counter;
                    temp[i, 2] = element.InnerXml;
                    temp[i, 3] = null;
                }
            }

            return temp;
        }

        private void PrepareToCollecting(string[,] data, string pathData, string pathSummary, int interval)
        {
            buttonSave.Enabled = false;
            buttonModify.Enabled = true;
            buttonPathData.Enabled = false;
            buttonPathSummary.Enabled = false;
            checkBoxProcessor.Enabled = false;
            checkBoxMemory.Enabled = false;
            checkBoxDisk.Enabled = false;
            numericUpDownInterval.Enabled = false;

            PC = new PerfCounters(pathData, pathSummary, interval);
            for (int i = 0; i < data.GetLength(0); i++)
            {
                if (data[i, 3] != null)
                {
                    string[] strLevel = data[i, 3].Split(';');
                    int[] levels = new int[strLevel.Length];
                    for (int j = 0; j < levels.Length; j++)
                        levels[j] = Convert.ToInt32(strLevel[j]);
                    PC.AddCounter(data[i, 0], data[i, 1], data[i, 2], levels);
                }
                else
                {
                    string[] strLevel = data[i, 2].Split(';');
                    int[] levels = new int[strLevel.Length];
                    for (int j = 0; j < levels.Length; j++)
                        levels[j] = Convert.ToInt32(strLevel[j]);
                    PC.AddCounter(data[i, 0], data[i, 1], levels);
                }

            }
            PC.StartCollecting();
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            string value = "";
            if (InputBox("Wymagane hasło", "Podaj hasło:", out value) == DialogResult.OK)
            {
                if (value == "1234")
                {
                    MessageBox.Show("Hasło poprawne.");
                    PC.StopCollecting();
                    label9.Text = "Wyłączony";
                    PC.DeleteAllCounters();
                    buttonSave.Enabled = true;
                    buttonModify.Enabled = false;
                    buttonPathData.Enabled = true;
                    buttonPathSummary.Enabled = true;
                    checkBoxProcessor.Enabled = true;
                    checkBoxMemory.Enabled = true;
                    checkBoxDisk.Enabled = true;
                    numericUpDownInterval.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Hasło niepoprawne");
                }
            }
            
        }

        private DialogResult InputBox(string title, string promptText, out string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = "";
            textBox.UseSystemPasswordChar = true;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ControlBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private void ClientApp_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Visible = true;
                Hide();
            }
            else if (FormWindowState.Normal == WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void ClientApp_Shown(object sender, EventArgs e)
        {
            if (label9.Text == "Włączony")
            {
                WindowState = FormWindowState.Minimized;
                ClientApp_Resize(null, null);
            }
        }
    }
}
