using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerfLibrary
{
    public class Communication
    {
        private List<ICommand> Command;
        private char[] bufferChar;
        private string bufferString;
        private string lastSendCommand;
        private string lastSendParameter;
        private string lastSendData;
        private string lastReceivedCommand;
        private string lastReceivedParameter;
        private string lastReceivedData;
        private bool positiveLastReceived;
        private Dictionary<string, string[]> result;

        public Communication(bool useCommandGET, bool useCommandSET, bool useCommandDATA)
        {
            Command = new List<ICommand>();
            result = new Dictionary<string, string[]>();
            if (useCommandGET)
            {
                Command.Add(new GET());
                result.Add("DATA", new string[] { });
                result.Add("COUNTCOL", new string[] { });
                result.Add("COUNTUSE", new string[] { });
            }
            if (useCommandSET)
            {
                Command.Add(new SET());
                result.Add("LEVEL", new string[] { });
            }
            if (useCommandDATA)
            {
                Command.Add(new DATA());
                result.Add("DATA", new string[] { });
                result.Add("COUNTCOL", new string[] { });
                result.Add("COUNTUSE", new string[] { });
            }
            ZeroAllValue();

            lastReceivedCommand = null;
            lastReceivedParameter = null;
            lastReceivedData = null;
            positiveLastReceived = false;
        }

        public string LastSendCommand
        {
            get { return LastSendCommand; }
        }

        public string LastSendParameter
        {
            get { return lastSendParameter; }
        }

        public string LastSendData
        {
            get { return lastSendData; }
        }

        public string LastReceivedCommand
        {
            get { return lastReceivedCommand; }
        }

        public string LastReceivedParameter
        {
            get { return lastReceivedParameter; }
        }

        public string LastReceivedData
        {
            get { return lastReceivedData; }
        }

        public string BufferString
        {
            get { return bufferString; }
        }

        public char[] BufferChar
        {
            get { return bufferChar; }
        }

        private void ZeroAllValue()
        {
            bufferChar = new char[0];
            bufferString = "";
            lastSendCommand = null;
            lastSendParameter = null;
            lastSendData = null;
        }

        public string GetNowDateToString()
        {
            return DateTime.Now.ToString(Config.DateFormat);
        }

        public string[] GetAllCommand()
        {
            string[] temp = new string[Command.Count];
            for (int i = 0; i < Command.Count; i++)
                temp[i] = Command[i].Name;

            return temp;
        }

        public string[] GetAllParam(string command)
        {
            int index = GetIndex(command);
            if (index == -1)
                return null;

            return Command[index].GetAllParam();
        }

        public bool LevelsPercentToString(int[] percent, ref string levelString)
        {
            if (percent.Length < Config.MinimumLevels)
                return false;

            Array.Sort(percent);

            levelString = "";
            for (int i = 0; i < percent.Length; i++)
            {
                if (percent[i] <= 0 || percent[i] > 100)
                {
                    levelString = null;
                    return false;
                }
                levelString += Convert.ToString(percent[i]) + ";";
            }
            levelString = levelString.Remove(levelString.Length - 1);

            return true;
        }

        public bool LevelsPercentToString(string[] percent, ref string levelString)
        {
            if (percent.Length < Config.MinimumLevels)
                return false;

            Array.Sort(percent);
            levelString = "";
            for (int i = 0; i < percent.Length; i++)
            {
                try
                {
                    int temp = Convert.ToInt32(percent[i]);
                    if (temp <= 0 || temp > 100)
                    {
                        levelString = null;
                        return false;
                    }
                }
                catch
                {
                    levelString = null;
                    return false;
                }
                levelString += percent[i] + ";";
            }
            levelString = levelString.Remove(levelString.Length - 1);

            return true;
        }

        public string[] GetResultInterpretation()
        {
            if (lastReceivedParameter != null && lastReceivedParameter != "")
            {
                return result[lastReceivedParameter];
            }

            return null;
        }

        public bool InterpretationOfReceived(string dataFrame, ref string communique)
        {
            char[] charArray = dataFrame.ToCharArray();
            lastReceivedCommand = "";
            lastReceivedParameter = "";
            lastReceivedData = "";
            string length = "";
            positiveLastReceived = false;

            try
            {
                for (int i = 0; i < Config.AmountCharInFrame; i++)
                {
                    lastReceivedCommand += charArray[i];
                    lastReceivedParameter += charArray[i + Config.AmountCharInFrame];
                    length += charArray[i + 2 * Config.AmountCharInFrame];
                }
                for (int i = 0; i < charArray.Length - 3 * Config.AmountCharInFrame; i++)
                {
                    lastReceivedData += charArray[i + 3 * Config.AmountCharInFrame];
                }

                char[] zero = new char[] { '\0' };
                lastReceivedCommand = lastReceivedCommand.Trim(zero);
                lastReceivedParameter = lastReceivedParameter.Trim(zero);
                length = length.Trim(zero);

                if (lastReceivedData.Length != Convert.ToInt32(length[0]) && lastReceivedParameter != "OK" && lastReceivedParameter != "ERROR")
                    return false;
            }
            catch
            {
                return false;
            }

            switch (lastReceivedCommand)
            {
                case "DATA":
                    switch (lastReceivedParameter)
                    {
                        case "DATA":
                            string[] tempL = lastReceivedData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            if (tempL.Length < Config.MinimumLevels)
                                return false;

                            result["DATA"] = tempL;
                            communique = "OK";

                            break;

                        case "COUNTUSE":
                            string[] tempCU = lastReceivedData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            if (tempCU.Length != 0)
                            {
                                result["COUNTUSE"] = tempCU;
                                communique = "OK";
                                break;
                            }
                            else
                                return false;

                        case "COUNTCOL":
                            string[] tempCC = lastReceivedData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            if (tempCC.Length != 0)
                            {
                                result["COUNTCOL"] = tempCC;
                                communique = "OK";
                                break;
                            }
                            else
                                return false;

                        case "OK":
                            communique = lastReceivedParameter;
                            break;

                        case "ERROR":
                            goto case "OK";

                        default:
                            return false;
                    }
                    break;

                default:
                    return false;
            }

            positiveLastReceived = true;
            return true;

        }

        private bool AcceptChar(char character)
        {
            if (char.IsLetterOrDigit(character) || char.IsSymbol(character))
                return true;
            return false;
        }

        public bool InterpretationOfCommandFrame(string dataFrame, ref PerfCounters PC)
        {
            char[] charArray = dataFrame.ToCharArray();
            lastReceivedCommand = "";
            lastReceivedParameter = "";
            lastReceivedData = "";
            string length = "";
            positiveLastReceived = false;

            try
            {
                for (int i = 0; i < Config.AmountCharInFrame; i++)
                {
                    lastReceivedCommand += charArray[i];
                    lastReceivedParameter += charArray[i + Config.AmountCharInFrame];
                    length += charArray[i + 2 * Config.AmountCharInFrame];
                }
                for (int i = 0; i < charArray.Length - 3 * Config.AmountCharInFrame; i++)
                {
                    lastReceivedData += charArray[i + 3 * Config.AmountCharInFrame];
                }

                char[] zero = new char[] { '\0' };
                lastReceivedCommand = lastReceivedCommand.Trim(zero);
                lastReceivedParameter = lastReceivedParameter.Trim(zero);
                length = length.Trim(zero);

                if (lastReceivedData.Length != Convert.ToInt32(length[0]) && lastReceivedParameter != "COUNTCOL" && lastReceivedParameter != "COUNTUSE")
                    return false;
            }
            catch
            {
                return false;
            }

            switch (lastReceivedCommand)
            {
                case "GET":
                    switch (lastReceivedParameter)
                    {
                        case "DATA":
                            string[] tempD = lastReceivedData.Split(':');
                            if (tempD.Length != 2)
                                return false;

                            if (PC.DateExists(tempD[1], tempD[0]))
                            {
                                result["DATA"] = PC.ValueInDate(tempD[1], tempD[0]);
                                break;
                            }
                            else
                                return false;

                        case "COUNTCOL":
                            string[] tempCC = PC.CollectedCounters();
                            if (tempCC != null)
                            {
                                result["COUNTCOL"] = tempCC;
                                break;
                            }
                            else
                                return false;
                        case "COUNTUSE":
                            string[] tempCU = PC.CounterInUse();
                            if (tempCU != null)
                            {
                                result["COUNTUSE"] = tempCU;
                                break;
                            }
                            else
                                return false;
                        default:
                            return false;
                    }
                    break;

                case "SET":
                    switch (lastReceivedParameter)
                    {
                        case "LEVEL":
                            string[] tempD = lastReceivedData.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                            if (tempD.Length != 2)
                                return false;

                            string[] tempStrL = tempD[1].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            if (tempStrL.Length < Config.MinimumLevels)
                                return false;

                            int[] levels = new int[tempStrL.Length];
                            for (int i = 0; i < levels.Length; i++)
                            {
                                levels[i] = Convert.ToInt32(tempStrL[i]);
                            }

                            if (PC.ChangeLevels(tempD[0], levels))
                                break;
                            else
                                return false;

                        default:
                            return false;
                    }
                    break;

                default:
                    return false;
            }

            positiveLastReceived = true;
            return true;
        }

        public bool PrepareDataFrame(string command, string param, string data, bool createString, bool createCharArray)
        {

            command = command.ToUpper();
            param = param.ToUpper();

            int index = GetIndex(command);

            try
            {
                if ((!Command[index].IsAvailable(param) || !Command[index].ValidationAndPrepareData(param, ref data) || (!createCharArray && !createString)))
                    return false;
            }
            catch (IndexOutOfRangeException) { return false; }

            CreateData(index, param, data, createString, createCharArray);

            return true;
        }

        public bool PrepareDataFrame(string command, string param, string data, string counter, bool createString, bool createCharArray)
        {

            command = command.ToUpper();
            param = param.ToUpper();

            int index = GetIndex(command);

            try
            {
                if ((!Command[index].IsAvailable(param) || !Command[index].ValidationAndPrepareData(param, ref data, counter) || (!createCharArray && !createString)))
                    return false;
            }
            catch (IndexOutOfRangeException) { return false; }

            CreateData(index, param, data, createString, createCharArray);

            return true;
        }

        public bool PrepareDataFrameWithReceivedFrame(bool createString, bool createCharArray)
        {
            if (positiveLastReceived)
            {
                switch (LastReceivedCommand)
                {
                    case "GET":
                        switch (LastReceivedParameter)
                        {
                            case "DATA":
                                string data = "";
                                int index = GetIndex("DATA");
                                for (int i = 0; i < result[LastReceivedParameter].Length; i++)
                                {
                                    data += result[LastReceivedParameter][i] + ";";
                                }
                                data = data.Remove(data.Length - 1);
                                try
                                {
                                    if ((!Command[index].ValidationAndPrepareData(LastReceivedParameter, ref data) || (!createCharArray && !createString)))
                                        return false;
                                }
                                catch (IndexOutOfRangeException) { return false; }
                                CreateData(index, LastReceivedParameter, data, createString, createCharArray);
                                break;

                            case "COUNTCOL":
                                goto case "DATA";

                            case "COUNTUSE":
                                goto case "DATA";
                        }

                        break;

                    case "SET":
                        switch (LastReceivedParameter)
                        {
                            case "LEVEL":
                                string data = "";
                                int index = GetIndex("DATA");
                                try
                                {
                                    if ((!Command[index].ValidationAndPrepareData("OK", ref data) || (!createCharArray && !createString)))
                                        return false;
                                }
                                catch (IndexOutOfRangeException) { return false; }
                                CreateData(index, "OK", data, createString, createCharArray);
                                break;

                            default:
                                return false;
                        }
                        break;

                    default:
                        return false;
                }
                return true;
            }
            else
            {
                string data = "\0";
                int index = GetIndex("DATA");
                CreateData(index, "ERROR", data, createString, createCharArray);
                return true;
            }
        }

        private void CreateData(int index, string param, string data, bool createString, bool createCharArray)
        {
            ZeroAllValue();

            if (createString)
                bufferString = Command[index].PrepareDataString(param, data);

            if (createCharArray)
                bufferChar = Command[index].PrepareDataCharArray(param, data);

            lastSendCommand = Command[index].Name;
            lastSendParameter = param;
            lastSendData = data;
        }

        private int GetIndex(string command)
        {
            for (int i = 0; i < Command.Count; i++)
            {
                if (command == Command[i].Name)
                    return i;
            }

            return -1;
        }


    }
}
