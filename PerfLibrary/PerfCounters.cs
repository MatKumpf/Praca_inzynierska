using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace PerfLibrary
{
    public class PerfCounters
    {
        private static List<Counter> Counters;
        private static Thread threadCollecting;
        private string pathData;
        private string pathSummary;
        private int Interval;
        private string Today;
        private int levelCount;

        public PerfCounters(string pathData, string pathSummary, int interval)
        {
            Counters = new List<Counter>();
            threadCollecting = new Thread(new ThreadStart(Collecting));

            if (pathData[pathData.Length - 1] != '\\')
                this.pathData = pathData + "\\";
            else
                this.pathData = pathData;

            if (pathSummary[pathSummary.Length - 1] != '\\')
                this.pathSummary = pathSummary + "\\";
            else
                this.pathSummary = pathSummary;

            Interval = interval;

            Today = DateTime.Now.ToString(Config.DateFormat);

        }

        public bool AddCounter(string CategoryName, string CounterName, string InstanceName, int[] Levels)
        {
            try
            {
                levelCount = Levels.Length;
                string CounterNameWithoutSC = Regex.Replace(CategoryName, "[^0-9a-zA-Z]+", "") + "-" + Regex.Replace(CounterName, "[^0-9a-zA-Z]+", "");
                string FileName = Today + " " + CounterNameWithoutSC;
                Counters.Add(new Counter(CategoryName, CounterName, InstanceName, Levels, FileName));
            }
            catch { return false; }

            return true;
        }

        public bool AddCounter(string CategoryName, string CounterName, int[] Levels)
        {
            try
            {
                levelCount = Levels.Length;
                string CounterNameWithoutSC = Regex.Replace(CategoryName, "[^0-9a-zA-Z]+", "") + "-" + Regex.Replace(CounterName, "[^0-9a-zA-Z]+", "");
                string FileName = Today + " " + CounterNameWithoutSC;
                Counters.Add(new Counter(CategoryName, CounterName, Levels, FileName));
            }
            catch { return false; }

            return true;
        }

        public bool AddCounter(string CategoryName, string CounterName, string InstanceName, int[] Levels, int maxValue)
        {
            try
            {
                levelCount = Levels.Length;
                string CounterNameWithoutSC = Regex.Replace(CategoryName, "[^0-9a-zA-Z]+", "") + "-" + Regex.Replace(CounterName, "[^0-9a-zA-Z]+", "");
                string FileName = Today + " " + CounterNameWithoutSC;
                Counters.Add(new Counter(CategoryName, CounterName, InstanceName, Levels, FileName, maxValue));
            }
            catch { return false; }

            return true;
        }

        public bool AddCounter(string CategoryName, string CounterName, int[] Levels, int maxValue)
        {
            try
            {
                levelCount = Levels.Length;
                string CounterNameWithoutSC = Regex.Replace(CategoryName, "[^0-9a-zA-Z]+", "") + "-" + Regex.Replace(CounterName, "[^0-9a-zA-Z]+", "");
                string FileName = Today + " " + CounterNameWithoutSC;
                Counters.Add(new Counter(CategoryName, CounterName, Levels, FileName, maxValue));
            }
            catch { return false; }

            return true;
        }

        private bool IsNotInCollectingCounter(string fileName)
        {
            for (int i = 0; i < Counters.Count; i++)
            {
                if (fileName == Counters[i].FileName)
                    return false;
            }
            return true;
        }


        public void Summary()
        {
            string[] dataFile = Directory.GetFiles(pathData, "*.data");

            foreach (string DF in dataFile)
            {
                string fileName = DF.Replace(pathData, "");
                if (IsNotInCollectingCounter(fileName))
                {
                    XmlDocument SummaryXML = new XmlDocument();
                    string counterName = fileName.Remove(fileName.Length - 5).Split(' ')[1];
                    string summaryFile = pathSummary + "Summary " + counterName + ".xml";
                    if (!File.Exists(pathSummary + "Summary " + counterName + ".xml"))
                    {
                        XmlDeclaration xmlDeclaration = SummaryXML.CreateXmlDeclaration("1.0", "UTF-8", null);
                        XmlElement DE = SummaryXML.DocumentElement;
                        SummaryXML.InsertBefore(xmlDeclaration, DE);
                        XmlElement root = SummaryXML.CreateElement(counterName);
                        SummaryXML.AppendChild(root);
                    }
                    else
                    {
                        SummaryXML.Load(summaryFile);
                    }
                    StreamReader SR = new StreamReader(DF);

                    string DateInFile = fileName.Remove(10);
                    XmlElement dateElement = SummaryXML.CreateElement("Date");
                    dateElement.SetAttribute("Value", DateInFile);

                    char[] Data = SR.ReadToEnd().ToCharArray();

                    Dictionary<char, int> dict = new Dictionary<char, int>();
                    foreach (char c in Data)
                    {
                        if (dict.ContainsKey(c))
                        {
                            dict[c]++;
                        }
                        else
                        {
                            dict.Add(c, 1);
                        }
                    }

                    XmlElement dataElement;
                    XmlText dataText;

                    for (int i = 0; i < levelCount; i++)
                    {
                        try
                        {
                            dataElement = SummaryXML.CreateElement("Range" + (i + 1).ToString());
                            dataText = SummaryXML.CreateTextNode(dict[(i + 1).ToString().ToCharArray()[0]].ToString());
                            dataElement.AppendChild(dataText);
                            dateElement.AppendChild(dataElement);
                        }
                        catch
                        {
                            dataElement = SummaryXML.CreateElement("Range" + (i + 1).ToString());
                            dataText = SummaryXML.CreateTextNode(Convert.ToString(0));
                            dataElement.AppendChild(dataText);
                            dateElement.AppendChild(dataElement);
                        }
                    }

                    SummaryXML.DocumentElement.AppendChild(dateElement);
                    SummaryXML.Save(summaryFile);
                    SR.Close();
                    File.Delete(pathData + fileName);
                }
            }
        }

        public bool DateExists(string DateTime, string counter)
        {
            try
            {
                XmlDocument SummaryXML = new XmlDocument();
                FileStream file = new FileStream(pathSummary + "Summary " + counter + ".xml", FileMode.Open, FileAccess.Read);
                SummaryXML.Load(file);

                if (GetChildDate(SummaryXML.GetElementsByTagName("Date"), DateTime) != null)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public string[] CounterInUse()
        {
            if (Counters.Count != 0)
            {
                string[] temp = new string[Counters.Count];

                for (int i = 0; i < Counters.Count; i++)
                {
                    temp[i] = Counters[i].ID;
                }
                return temp;
            }
            else
                return null;
        }

        public string[] CollectedCounters()
        {
            string[] summaryFile = Directory.GetFiles(pathSummary, "*.xml");
            List<string> listCounter = new List<string>();

            foreach (string SF in summaryFile)
            {
                string fileName = SF.Replace(pathSummary + "Summary ", "");
                fileName = fileName.Remove(fileName.Length - 4);
                if (!listCounter.Contains(fileName))
                    listCounter.Add(fileName);
            }

            if (listCounter.Count != 0)
            {
                string[] temp = new string[listCounter.Count];
                for (int i = 0; i < listCounter.Count; i++)
                {
                    temp[i] = listCounter[i];
                }

                return temp;
            }
            else
                return null;
        }

        public bool ChangeLevels(string counter, int[] levels)
        {
            for (int i = 0; i < Counters.Count; i++)
            {
                if (Counters[i].ID == counter)
                {
                    Counters[i].Levels = levels;
                    return true;
                }
            }
            return false;
        }

        private XmlNodeList GetChildDate(XmlNodeList listDate, string DateTime)
        {
            if (listDate.Count != 0)
            {
                for (int i = 0; i < listDate.Count; i++)
                {
                    XmlElement element = (XmlElement)listDate[i];
                    if (DateTime == element.GetAttribute("Value"))
                    {
                        return listDate[i].ChildNodes;
                    }
                }
            }
            return null;
        }

        public string[] ValueInDate(string DateTime, string counter)
        {
            XmlDocument SummaryXML = new XmlDocument();
            FileStream file = new FileStream(pathSummary + "Summary " + counter + ".xml", FileMode.Open, FileAccess.Read);
            SummaryXML.Load(file);
            XmlNodeList listDate = GetChildDate(SummaryXML.GetElementsByTagName("Date"), DateTime);
            if (listDate.Count != 0)
            {
                string[] RangeValues = new string[listDate.Count];
                for (int i = 0; i < listDate.Count; i++)
                {
                    XmlElement element = (XmlElement)listDate[i];
                    RangeValues[i] = element.InnerXml;
                }
                file.Close();
                return RangeValues;
            }
            file.Close();
            return null;

        }

        public void DeleteAllCounters()
        {
            Counters.Clear();
        }

        public bool StartCollecting()
        {
            try
            {
                threadCollecting.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool StopCollecting()
        {
            try
            {
                threadCollecting.Abort();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Collecting()
        {
            while (true)
            {
                try
                {
                    if (Today != DateTime.Now.ToString(Config.DateFormat))
                    {
                        Today = DateTime.Now.ToString(Config.DateFormat);
                        for (int i = 0; i < Counters.Count; i++)
                        {
                            Counters[i].FileName = Counters[i].FileName.Insert(0, Today);
                        }
                    }

                    if (Counters.Count > 0)
                    {
                        for (int i = 0; i < Counters.Count; i++)
                        {
                            FileStream file = new FileStream(pathData + Counters[i].FileName, FileMode.Append, FileAccess.Write, FileShare.Inheritable);
                            StreamWriter SW = new StreamWriter(file, Encoding.Unicode);
                            SW.Write(Counters[i].GetLevel());
                            SW.Close();
                            file.Close();
                        }
                    }
                }
                catch { }
                try
                {
                    Thread.Sleep(Interval);
                }
                catch { }

            }
        }

        public string[] GetCategory()
        {
            string[] CategoryName;
            try
            {
                PerformanceCounterCategory[] CounterCategory = PerformanceCounterCategory.GetCategories();
                CategoryName = new string[CounterCategory.Length];
                for (int i = 0; i < CounterCategory.Length; i++)
                    CategoryName[i] = CounterCategory[i].CategoryName;
            }
            catch
            {
                return null;
            }

            Array.Sort(CategoryName);

            return CategoryName;
        }

        public string[] GetInstance(string CategoryName)
        {
            string[] InstanceName;
            try
            {
                PerformanceCounterCategory CounterCategory = new PerformanceCounterCategory(CategoryName);
                InstanceName = CounterCategory.GetInstanceNames();
            }
            catch
            {
                return null;
            }

            Array.Sort(InstanceName);

            return InstanceName;
        }

        public string[] GetCounters(string CategoryName, string InstanceName)
        {
            string[] CountersName;
            try
            {
                PerformanceCounterCategory CounterCategory = new PerformanceCounterCategory(CategoryName);
                PerformanceCounter[] Counters = CounterCategory.GetCounters(InstanceName);
                CountersName = new string[Counters.Length];
                for (int i = 0; i < Counters.Length; i++)
                    CountersName[i] = Counters[i].CounterName;
            }
            catch
            {
                return null;
            }

            Array.Sort(CountersName);

            return CountersName;
        }

        public string[] GetCounters(string CategoryName)
        {
            string[] CountersName;
            try
            {
                PerformanceCounterCategory CounterCategory = new PerformanceCounterCategory(CategoryName);
                PerformanceCounter[] Counters = CounterCategory.GetCounters();
                CountersName = new string[Counters.Length];
                for (int i = 0; i < Counters.Length; i++)
                    CountersName[i] = Counters[i].CounterName;
            }
            catch
            {
                return null;
            }

            Array.Sort(CountersName);

            return CountersName;
        }
    }

}