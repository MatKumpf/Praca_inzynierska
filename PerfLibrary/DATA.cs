using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfLibrary
{
    class DATA : ICommand
    {

        enum Param
        {
            DATA, COUNTUSE, COUNTCOL, OK, ERROR
        };

        public DATA()
        {

        }

        public string[] GetAllParam()
        {
            string[] temp = new string[GetLengthEnum()];
            for (int i = 0; i < GetLengthEnum(); i++)
                temp[i] = Enum.GetName(typeof(Param), i);
            return temp;
        }

        public int GetLengthEnum()
        {
            return Enum.GetNames(typeof(Param)).Length;
        }

        public bool IsAvailable(string parameter)
        {
            parameter = parameter.ToUpper();
            foreach (string temp in GetAllParam())
            {
                if (temp == parameter)
                    return true;
            }
            return false;
        }

        public string Name
        {
            get { return typeof(DATA).Name; }
        }

        private void AddNull(ref string buffer, int it)
        {
            for (int i = 0; i < it; i++)
                buffer += '\0';
        }

        public char[] PrepareDataCharArray(string parameter, string data)
        {
            parameter = parameter.ToUpper();
            char[] length = new char[1] { Convert.ToChar(data.Length) };

            char[] buffer = new char[3 * Config.AmountCharInFrame + data.Length];

            for (int i = 0; i < Config.AmountCharInFrame; i++)
            {
                try { buffer[i] = Name[i]; }
                catch { buffer[i] = '\0'; }
                try { buffer[Config.AmountCharInFrame + i] = parameter[i]; }
                catch { buffer[Config.AmountCharInFrame + i] = '\0'; }
                try { buffer[2 * Config.AmountCharInFrame + i] = length[i]; }
                catch { buffer[2 * Config.AmountCharInFrame + i] = '\0'; }
            }

            for (int i = 0; i < data.Length; i++)
            {
                buffer[3 * Config.AmountCharInFrame + i] = data[i];
            }

            return buffer;
        }

        public string PrepareDataString(string parameter, string data)
        {
            parameter = parameter.ToUpper();
            string buffer = "";

            buffer += Name;
            AddNull(ref buffer, Config.AmountCharInFrame - Name.Length);
            buffer += parameter;
            AddNull(ref buffer, Config.AmountCharInFrame - parameter.Length);
            buffer += Convert.ToChar(data.Length);
            AddNull(ref buffer, Config.AmountCharInFrame - 1);
            buffer += data;

            return buffer;
        }

        public bool ValidationAndPrepareData(string parameter, ref string data)
        {
            switch (parameter)
            {
                case "DATA":
                    try
                    {
                        string[] temp = data.Split(';');
                        if (temp.Length < Config.MinimumLevels)
                            return false;
                        else
                        {
                            for (int i = 0; i < temp.Length; i++)
                                Convert.ToInt32(temp[i]);

                            return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                case "COUNTUSE":
                    if(data.Length == 0)
                        return false;

                    return true;
                case "COUNTCOL":
                    goto case "COUNTUSE";
                case "OK":
                    data = "\0";
                    return true;
                case "ERROR":
                    data = "\0";
                    return true;

                default:
                    return false;
            }
        }

        public bool ValidationAndPrepareData(string parameter, ref string data, string counter)
        {
            switch (parameter)
            {
                case "DATA":
                    try
                    {
                        string[] temp = data.Split(';');
                        if (temp.Length < Config.MinimumLevels)
                            return false;
                        else
                        {
                            for (int i = 0; i < temp.Length; i++)
                                Convert.ToInt32(temp[i]);

                            return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                case "COUNTUSE":
                    if (counter.Length == 0)
                        return false;
                    data = counter;

                    return true;
                case "COUNTCOL":
                    goto case "COUNTUSE";
                case "OK":
                    data = "\0";
                    return true;
                case "ERROR":
                    data = "\0";
                    return true;
                default:
                    return false;
            }
        }
    }
}
