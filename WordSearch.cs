using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Words
{
    class WordSearch : MainWindow
    {
        public static string WordToStar(string word)
        {
            string star = "";
            foreach (var symbl in word)
            {
                star += "*";
            }
            return star;
        }

        public static string Equel(string word, string forbiddenWord)
        {
            string tempWord = "";
            if (word.Length >= forbiddenWord.Length && word.Length <= forbiddenWord.Length + 2)
            {
                if ((word.ToLower() == forbiddenWord.ToLower()) || word.ToLower().Remove(word.Length - 1) == forbiddenWord.ToLower() ||
                    word.ToLower().Remove(word.Length - 2) == forbiddenWord.ToLower())
                {
                    tempWord = word;
                }
                else
                {
                    tempWord = "";
                }
            }
            return tempWord;
        }

        public static /*async*/ void WriteForbiddenWordToFile(FileList file)
        {
            string newWord = "";
            TextInFile = "";
            foreach (var word in AddEachWordToList(ReadWrite.ReadTXT(file.File), TextList))
            {
                TextInFile += word;
                string words = word;
                int i = 0;
                var tasks = new Task<string>[ForbidenWordsList.Count];
                foreach (var forbiddenWord in ForbidenWordsList)
                {
                    tasks[i] = new Task<string>(() => Equel(word, forbiddenWord));
                    tasks[i].Start();

                    if (/*forbiddenWord.ToLower() == word.ToLower()*/ tasks[i].Result != "")
                    {
                        ReadWrite.WriteLogToFileAsync(file, word);
                        ReadWrite.WriteTextToFileAsync(ReadWrite.ReadTXT(file.File), $"{ListConfig[ListConfig.Count - 1].pathForbidden}/{file.FileName}");
                        words = WordToStar(word);
                    }
                    i++;
                }
                newWord += words;
            }
            /*await Task.Run(() =>*/
            ReadWrite.WriteTextToFileAsync(newWord, file.File)/*)*/;
        }

        private static List<string> AddEachWordToList(string line, List<string> tempList)
        {
            tempList.Clear();
            string word = "";
            string symbl = "";
            foreach (var symblLine in line)
            {
                word += symblLine;
                if (symblLine >= 32 && symblLine <= 47 || symblLine >= 58 && symblLine <= 64 || symblLine >= 91 && symblLine <= 96 ||
                    symblLine >= 123 && symblLine <= 126 || symblLine == '\n' || symblLine == '«' || symblLine == '»')
                {
                    symbl += symblLine;
                    word = word.Remove(word.Length - 1);
                    if (word != "")
                    {
                        tempList.Add(word);
                        word = "";
                    }
                    tempList.Add(symbl);
                    symbl = "";
                }
            }
            return tempList;
        }
    }
}
