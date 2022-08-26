using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Words
{
    class LogStatistic : MainWindow
    {
       

        public static void ListLogCount(string pathFile, string word)
        {
            bool flag = false;
            int idList = 0;
            if (ListLog.Count > 0)
            {
                for (int i = 0; i < ListLog.Count; i++)
                {
                    if (ListLog[i].Word == word)
                    {
                        flag = true;
                        idList = i;
                        break;
                    }
                }
                if (flag)
                {
                    ListLog[idList].CountWord++;
                }
                else
                {
                    LogList log = new();
                    log.DateTime = DateTime.Now;
                    log.Path = pathFile;
                    log.Word = word;
                    log.CountWord = 1;
                    ListLog.Add(log);
                }
            }
            else
            {
                LogList log = new();
                log.DateTime = DateTime.Now;
                log.Path = pathFile;
                log.Word = word;
                log.CountWord = 1;
                ListLog.Add(log);
            }
        }

        public static string Statistic()
        {
            int maxCountWord = 0;
            string bestWord = "";
            string statiSticLog = "";
            int i = 0;
            foreach (var listObj in ListLog)
            {
                if (ListLog[i].CountWord > maxCountWord)
                {
                    maxCountWord = ListLog[i].CountWord;
                    bestWord = ListLog[i].Word;
                }
                i++;
                statiSticLog += " " + listObj.Word + ": " + listObj.CountWord.ToString() + "\n";
            }
            TotalStatistic = $" {"\t\t"} *** {"\n"} {DateTime.Now} {"\n"} Всего найдено слов: {"\n"}{statiSticLog}{"\t"} Самое популярное слово: {bestWord}, найдено: {maxCountWord} раз. {"\n"}{"\t\t"} *** {"\n"}";
            return TotalStatistic;
        }
    }
}
