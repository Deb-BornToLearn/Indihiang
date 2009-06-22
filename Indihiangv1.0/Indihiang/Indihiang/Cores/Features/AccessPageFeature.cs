using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indihiang.Cores.Features
{
    public class AccessPageFeature : BaseLogAnalyzeFeature
    {
        public AccessPageFeature(EnumLogFile logFile)
            : base(logFile)
        {
            _featureName = LogFeature.ACCESS;

            LogCollection log = new LogCollection();
            _logs.Add("General", log);
        }
        protected override bool RunFeature(List<string> header, string[] item)
        {
            switch (_logFile)
            {
                case EnumLogFile.NCSA:
                    break;
                case EnumLogFile.MSIISLOG:
                    break;
                case EnumLogFile.W3CEXT:
                    RunW3cext(header, item);
                    break;
            }

            return true;
        }
        private void RunW3cext(List<string> header, string[] item)
        {
            //if (header.Exists(FindPage))
            //{
                int val = 0;
                //int index = header.FindIndex(0,FindDate);
                //int index2 = header.FindIndex(0,FindPage);
                int index = header.IndexOf("date");
                int index2 = header.IndexOf("cs-uri-stem");
                
                if (index == -1 || index2 == -1)
                    return;

                string key = item[index];
                string dataKey = item[index2];

                if (dataKey != "" && dataKey != null && dataKey != "-")
                {
                    if (_logs["General"].Colls.ContainsKey(key))
                    {
                        if (_logs["General"].Colls[key].Items.ContainsKey(dataKey))
                        {                            
                            //lock (this) 
                            //{
                                val = Convert.ToInt32(_logs["General"].Colls[key].Items[dataKey]);
                                val++;
                                _logs["General"].Colls[key].Items[dataKey] = val.ToString(); 
                            //}
                        }
                        else
                        {
                            //lock (this) { 
                                _logs["General"].Colls[key].Items.Add(dataKey, "1"); 
                            //}
                        }
                    }
                    else
                        //lock (this) { 
                            _logs["General"].Colls.Add(key, new WebLog(dataKey, "1")); 
                        //}
                }
            //}
        }
        private static bool FindPage(string item)
        {
            if (item == "cs-uri-stem")
                return true;

            return false;
        }
        private static bool FindDate(string item)
        {
            if (item == "date")
                return true;

            return false;
        }
    }
}
