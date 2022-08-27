using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Words
{
    class ReadWrite : MainWindow
    {
        public static string ReadTXT(string file)
        {
            string words = "";
            if (!File.Exists(file))
            {
                MessageBox.Show($"Файл: {file} не существует!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                StreamReader data = new StreamReader(file);

                while (!data.EndOfStream)
                {
                    words += data.ReadLine() + '\n';
                }
                words = words.Remove(words.Length - 1);
                data.Close();
                return words;
            }
            return "";
        }

        //public static void WriteFile(string textFile, string fileName)
        //{
        //    StreamWriter newFile = new(fileName);
        //    newFile.WriteLine(textFile);
        //    newFile.Close();
        //}

        //public static /*string*/ StringBuilder EditText(string file)
        //{
        //    string text = ReadWrite.ReadTXT(file);
        //    var newtext = new StringBuilder(text);
        //    LogList tempList = new();
        //    foreach (string forbiddenWord in MainWindow.ForbidenWordsList)
        //    {
        //        string starsWord = "";
        //        int count = forbiddenWord.Length;
        //        for (int i = 0; i < count; i++)
        //        {
        //            starsWord += "*";
        //        }
        //        /*text = Convert.ToString(*/
        //        newtext.Replace(forbiddenWord, starsWord)/*)*/;
        //        //tempList.DateTime = DateTime.Now;
        //        //tempList.Word = forbiddenWord;
        //        //ListLog.Add(tempList);
        //    }
        //    return /*text*/ newtext;
        //}

        //public static void WriteLogToFile(FileList file, string word)
        //{
        //    StreamWriter dataWord = new("C:/Users/admin/Desktop/ЭкзаменСлова/ForbiddenWordLog.txt", true);
        //    LogStatistic.ListLogCount(file.File, word);
        //    string data = WriteLog(file, word);
        //    dataWord.WriteLine(data);
        //    dataWord.Close();
        //}

        //public static void WriteLogStatistic()
        //{
        //    StreamWriter dataWord = new("C:/Users/admin/Desktop/ЭкзаменСлова/ForbiddenWordLog.txt", true);
        //    string data = LogStatistic.Statistic();
        //    dataWord.WriteLine(data);
        //    dataWord.Close();
        //}

        public async static void WriteTextToFileAsync(string text, string pathFile)
        {
            await File.WriteAllTextAsync(pathFile, text);
        }

        public async static void WriteLogStatisticAsync()
        {
            string data = LogStatistic.Statistic();
            await File.AppendAllTextAsync(@$"{ListConfig[ListConfig.Count-1].pathForbidden}/ForbiddenWordLog.txt", data);
        }

        public async static void WriteLogToFileAsync(FileList file, string word)
        {
            LogStatistic.ListLogCount(file.File, word);
            string data = WriteLog(file, word);
            await File.AppendAllLinesAsync(@$"{ListConfig[ListConfig.Count - 1].pathForbidden}/ForbiddenWordLog.txt", new[] { data });
        }

        public static string WriteLog(FileList file, string word)
        {
            return $"[{DateTime.Now}] в файле [{file.File}] размер файла [{file.FileLength} байт] найдено слово: {word}";
        }
    }
}
