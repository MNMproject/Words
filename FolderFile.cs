using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Words
{
    class FolderFile : MainWindow
    {
        public static void SelectFile(string folderName)
        {
            int countFile = 0;
            string[] files = Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories);
            foreach (var s in files)
            {
                string fileName = "";
                fileName += s;
                countFile++;
                FileList file = new();
                FileInfo fileInfo = new FileInfo(s);
                file.File = fileName;
                file.FileName = Path.GetFileName(fileName);
                file.FileCount = countFile;
                if (fileInfo.Exists)
                {
                    file.FileLength = fileInfo.Length;
                }
               
                if (ListFile.Count == 0)
                {
                    CreateFile(file);
                }
                else
                {
                    bool flagFileFind = false;
                    for (int i = 0; i < ListFile.Count; i++)
                    {
                        if (ListFile[i].File == fileName)
                        {
                            flagFileFind = true;
                        }
                    }
                    if (!flagFileFind)
                    {
                        CreateFile(file);
                    }
                }
            }
        }

        public static void SelectFolder(string folderName)
        {
            string dirName = $"{folderName}";
            if (Directory.Exists(dirName))
            {
                int countFolder = 0;
                string[] dirs = Directory.GetDirectories(dirName, "", SearchOption.AllDirectories);
                foreach (string s in dirs)
                {
                    folderName = "";
                    countFolder++;
                    folderName += s;
                    DirectoryList folder = new();
                    folder.FolderUp = folderName;
                    folder.FolderDown = dirName;
                    folder.FolderCount = countFolder;
                    CreateDirectory(folder);
                    SelectFile(folderName);
                }
            }
        }

        public static void DialogWindiowSeachFile(string path)
        {
            if (File.Exists(path))
            {
                FileList file = new();
                FileInfo fileInfo = new FileInfo(path);
                file.File = path;
                file.FileName = Path.GetFileName(path);
                file.FileCount = 1;
                if (fileInfo.Exists)
                {
                    file.FileLength = fileInfo.Length;
                }
                CreateFile(file);
            }
            else
            {
                MessageBox.Show($"Выбранный файл не существует!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
