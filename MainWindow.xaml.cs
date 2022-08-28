using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Microsoft.Win32;


namespace Words
{
    public class DirectoryList : INotifyPropertyChanged
    {
        public DirectoryList() { }

        public DirectoryList(string folderUp, string folderDown, int folderCount)
        {
            FolderUp = folderUp;
            FolderDown = folderDown;
            FolderCount = folderCount;
        }

        private string folderUp;
        private string folderDown;
        private int folderCount;

        public string FolderUp
        {
            get { return folderUp; }
            set
            {
                folderUp = value;
                OnPropertyChanged("folderUp");
            }
        }

        public string FolderDown
        {
            get { return folderDown; }
            set
            {
                folderDown = value;
                OnPropertyChanged("folderDown");
            }
        }

        public int FolderCount
        {
            get { return folderCount; }
            set
            {
                folderCount = value;
                OnPropertyChanged("folderCount");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }
    }

    public class FileList : INotifyPropertyChanged
    {
        public FileList() { }

        public FileList(string file, string fileName, int fileCount, long fileLength)
        {
            File = file;
            FileName = fileName;
            FileCount = fileCount;
            FileLength = fileLength;
        }

        private string file;
        private string fileName;
        private int fileCount;
        private long fileLength;

        public string File
        {
            get { return file; }
            set
            {
                file = value;
                OnPropertyChanged("Файл");
            }
        }

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged("Файл");
            }
        }

        public int FileCount
        {
            get { return fileCount; }
            set
            {
                fileCount = value;
                OnPropertyChanged("Файл");
            }
        }

        public long FileLength
        {
            get { return fileLength; }
            set
            {
                fileLength = value;
                OnPropertyChanged("Файл");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }
    }

    public class LogList
    {
        public DateTime DateTime { get; set; }
        public string Word { get; set; }
        public string Path { get; set; }
        public int CountWord { get; set; }
    }

    public class ConfigList 
    {
        public DateTime dateTime { get; set; }
        public string path { get; set; }
        public string pathForbidden { get; set; }
    }

    public partial class MainWindow : Window
    {
        public static ObservableCollection<DirectoryList> ListDirectory = new();

        public static ObservableCollection<DirectoryList> CreateDirectory(DirectoryList data)
        {
            ListDirectory.Add(data);
            return ListDirectory;
        }

        public static ObservableCollection<FileList> ListFile = new();

        public static ObservableCollection<FileList> CreateFile(FileList data)
        {
            ListFile.Add(data);
            return ListFile;
        }

        public static List<ConfigList> ListConfig = new();
      
        public static List<LogList> ListLog = new();

        public static List<LogList> TempListLog = new();

        public static List<string> TextList = new();

        public static List<string> ForbidenWordsList = new();

        public static string TotalStatistic;

        public static string TextInFile;

        public MainWindow()
        {
            if (File.Exists("ForbiddenList.json"))
            {
                //LoadJsonToList(ForbidenWordsList);
                SaveLoad.LoadJsonToList(ForbidenWordsList);
            }
            else
            {
                MessageBox.Show($"Список запретных сдов пуст", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (File.Exists("config.json"))
            {
                SaveLoad.LoadConfig(ListConfig);
            }
            else
            {
                MessageBox.Show($"Выберите дирректорию и {"\n"}папку для запретных слов", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            InitializeComponent();
            Vizor1.ItemsSource = ListDirectory;
            Vizor2.ItemsSource = ListFile;
            if (File.Exists("config.json"))
            {
                VizorPath.Text = ListConfig[ListConfig.Count-1].path;
                VizorPathForbidden.Text = ListConfig[ListConfig.Count - 1].pathForbidden;
            }
        }

        //private void SaveJson()
        //{
        //    string fileName = "ForbiddenList.json";
        //    File.WriteAllText(fileName, JsonConvert.SerializeObject(ForbidenWordsList));
        //}

        //public void LoadJsonToList(List<string> tempList)
        //{
        //    ForbidenWordsList.Clear();
        //    if (File.Exists("ForbiddenList.json"))
        //    {
        //        var json = File.ReadAllText("ForbiddenList.json");
        //        var loading = JsonConvert.DeserializeObject<List<string>>(json);
        //        foreach (var list in loading)
        //        {
        //            tempList.Add(list);
        //        }
        //    }
        //    //else
        //    //{
        //    //    MessageBox.Show($"Список запрещенных слов пуст. {'\n'} Добавьте хотя бы одно слово", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
        //    //    Keyboard.Focus(EnterText);
        //    //}
        //}

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            string save = "";
            
            foreach (var symbl in EnterText.Text)
            {
                save += symbl;
                if (symbl == ' ')
                {
                    save += '\n';
                }
                else if ((symbl >= 33 && symbl <= 47) || (symbl >= 58 && symbl <= 63))
                {
                    save = save.Remove(save.Length - 1);
                }
            }
            if((save != "" || save != " ") && save.Length > 2)
            {
                StreamWriter data = new(@$"{ListConfig[ListConfig.Count-1].pathForbidden}/ForbiddenWord.txt", true);
                data.WriteLine(save);
                data.Close();
                ForbidenWordsList.Add(save);
                SaveLoad.SaveJson();
                //SaveJson();
                EnterText.Text = "";
            }
            else
            {
                MessageBox.Show($"Пустая строка или один символ!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private static void SelectFile(string folderName)
        //{
        //    int countFile = 0;
        //    string[] files = Directory.GetFiles(folderName, "*.txt", SearchOption.AllDirectories);
        //    foreach (var s in files)
        //    {
        //        string fileName = "";
        //        fileName += s;
        //        countFile++;
        //        FileList file = new();
        //        file.File = fileName;
        //        file.FileName = System.IO.Path.GetFileName(fileName);
        //        file.FileCount = countFile;

        //        if (ListFile.Count == 0)
        //        {
        //            CreateFile(file);
        //        }
        //        else
        //        {
        //            bool flagFileFind = false;
        //            for (int i = 0; i < ListFile.Count; i++)
        //            {
        //                if (ListFile[i].File == fileName)
        //                {
        //                    flagFileFind = true;
        //                }
        //            }
        //            if (!flagFileFind)
        //            {
        //                CreateFile(file);
        //            }
        //        }
        //    }
        //}

        //private static void SelectFolder(string folderName)
        //{
        //    string dirName = $"{folderName}";
        //    if (Directory.Exists(dirName))
        //    {
        //        int countFolder = 0;
        //        string[] dirs = Directory.GetDirectories(dirName, "", SearchOption.AllDirectories);
        //        foreach (string s in dirs)
        //        {
        //            folderName = "";
        //            countFolder++;
        //            folderName += s;
        //            DirectoryList folder = new();
        //            folder.FolderUp = folderName;
        //            folder.FolderDown = dirName;
        //            folder.FolderCount = countFolder;
        //            CreateDirectory(folder);
        //            SelectFile(folderName);
        //        }
        //    }
        //}

        //private void SelectDir(string dirName)
        //{
        //    //ListDirectory.Clear();
        //    string[] dir = Directory.GetDirectories(dirName, "", SearchOption.AllDirectories);
        //    if (Directory.Exists(dirName))
        //    {
        //        foreach (string s in dir)
        //        {
        //            string nameDir = "";
        //            dirName += s;
        //            DirectoryList directory = new();
        //            directory.FolderUp = nameDir;
        //            directory.FolderDown = dirName;
        //            CreateDirectory(directory);

        //        }
        //        Vizor1.ItemsSource = ListDirectory;
        //    }
        //}

        private void SearchFolder_Click(object sender, RoutedEventArgs e)
        {
            string dirName = ListConfig[ListConfig.Count - 1].path/*@"C:/Users/admin/Desktop/МТС"*/;
            if (Directory.Exists(dirName))
            {
                Label_Folder.Visibility = Visibility.Visible;
                int countFolder = 0;
                string[] dirs = Directory.GetDirectories(dirName, "", SearchOption.AllDirectories);
                foreach (string s in dirs)
                {
                    string folderName = "";
                    countFolder++;
                    folderName += s;
                    DirectoryList folder = new();
                    folder.FolderUp = folderName;
                    folder.FolderDown = dirName;
                    folder.FolderCount = countFolder;
                    CreateDirectory(folder);
                    FolderFile.SelectFile(folderName);
                }
            }
        }

        //public static string ReadTXT(string file)
        //{
        //    string words = "";
        //    if (!File.Exists(file))
        //    {
        //        MessageBox.Show($"Файл: {file} не существует!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //    else
        //    {
        //        StreamReader data = new StreamReader(file);

        //        while (!data.EndOfStream)
        //        {
        //            words += data.ReadLine() + '\n';
        //        }

        //        data.Close();
        //        return words;
        //    }
        //    return "";
        //}

        //private static List<string> AddEachWordToList(string line, List<string> tempList)
        //{
        //    tempList.Clear();
        //    string word = "";
        //    string symbl = "";
        //    foreach (var symblLine in line)
        //    {
        //        word += symblLine;
        //        if (symblLine >= 32 && symblLine <= 47 || symblLine >= 58 && symblLine <= 64 || symblLine >= 91 && symblLine <= 96 ||
        //            symblLine >= 123 && symblLine <= 126 || symblLine == '\n' || symblLine == '«' || symblLine == '»')
        //        {
        //            symbl += symblLine;
        //            word = word.Remove(word.Length - 1);
        //            if (word != "")
        //            {
        //                tempList.Add(word);
        //                word = "";
        //            }
        //            tempList.Add(symbl);
        //            symbl = "";
        //        }
        //    }
        //    return tempList;
        //}

        //private static string WriteLog(string pathFile, string word)
        //{
        //    return $"[{DateTime.Now}] в файле [{pathFile}] найдено слово: {word}";
        //}

        //private static void ListLogCount(string pathFile, string word)
        //{
        //    bool flag = false;
        //    int idList = 0;
        //    if (ListLog.Count > 0)
        //    {
        //        for (int i = 0; i < ListLog.Count; i++)
        //        {
        //            if (ListLog[i].Word == word)
        //            {
        //                flag = true;
        //                idList = i;
        //                break;
        //            }
        //        }
        //        if (flag)
        //        {
        //            ListLog[idList].CountWord++;
        //        }
        //        else
        //        {
        //            LogList log = new();
        //            log.DateTime = DateTime.Now;
        //            log.Path = pathFile;
        //            log.Word = word;
        //            log.CountWord = 1;
        //            ListLog.Add(log);
        //        }
        //    }
        //    else
        //    {
        //        LogList log = new();
        //        log.DateTime = DateTime.Now;
        //        log.Path = pathFile;
        //        log.Word = word;
        //        log.CountWord = 1;
        //        ListLog.Add(log);
        //    }
        //}

        //private static string Statistic()
        //{
        //    int maxCountWord = 0;
        //    string bestWord = "";
        //    string statiSticLog = "";
        //    int i = 0;
        //    foreach (var listObj in ListLog)
        //    {
        //        if (ListLog[i].CountWord > maxCountWord)
        //        {
        //            maxCountWord = ListLog[i].CountWord;
        //            bestWord = ListLog[i].Word;
        //        }
        //        i++;
        //        statiSticLog += " " + listObj.Word + ": " + listObj.CountWord.ToString() + "\n";
        //    }
        //    TotalStatistic = $" {"\t\t"} *** {"\n"} {DateTime.Now} {"\n"} Всего найдено слов: {"\n"}{statiSticLog}{"\t"} Самое популярное слово: {bestWord}, найдено: {maxCountWord} раз. {"\n"}{"\t\t"} *** {"\n"}";
        //    return TotalStatistic;
        //}

        private void Vizor1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListFile.Clear();
            DirectoryList directory = (DirectoryList)Vizor1.SelectedItem;
            
            if (directory != null)
            {
                FolderFile.SelectFile(directory.FolderUp);
            }
        }

        private void Vizor2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            FileList directory = (FileList)Vizor2.SelectedItem;

            if(directory != null)
            {
                VizorWord.Text = ReadWrite.ReadTXT(directory.File);
            }
        }

        private double progresCount;

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            progresCount += 100 / ListFile[ListFile.Count - 1].FileCount;
            (sender as BackgroundWorker).ReportProgress((int)Math.Round(progresCount));
            Thread.Sleep(100);
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void SearchWord_Click(object sender, RoutedEventArgs e)
        {
            //AddEachWordToList(ReadTXT("C:/Users/admin/Desktop/ЭкзаменСлова/ForbiddenWord.txt"), ForbidenWordsList);
            SaveLoad.LoadJsonToList(ForbidenWordsList);
            if (ForbidenWordsList.Count != 0)
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;

                foreach (var file in ListFile)
                {
                    WordSearch.WriteForbiddenWordToFile(file);

                    worker.DoWork += worker_DoWork;
                    worker.ProgressChanged += worker_ProgressChanged;

                    //WrireFile(ReadTXT(file.File), $"C:/Users/admin/Desktop/ЭкзаменСлова/{file.FileName}");
                    //WrireFile(Convert.ToString(EditText(file.File)), file.File);
                    VizorWord.Text = TextInFile;
                }
                
                ReadWrite.WriteLogStatisticAsync();
                worker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show($"Список запрещенных слов пуст. {'\n'} Добавьте хотя бы одно слово", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
                Keyboard.Focus(EnterText);
            }
            progresCount = 0;
            WindowSearch.Text = TotalStatistic;
            AllDiagrams();
        }

        //private static void WrireFile(string textFile, string fileName)
        //{
        //    StreamWriter newFile = new(fileName);
        //    newFile.WriteLine(textFile);
        //    newFile.Close();
        //}

        //private static /*string*/ StringBuilder EditText(string file)
        //{
        //    string text = ReadWrite.ReadTXT(file);
        //    var newtext = new StringBuilder(text);
        //    LogList tempList = new();
        //    foreach (string forbiddenWord in ForbidenWordsList)
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

        //private /*async*/ void WriteForbiddenWordToFile(FileList file)
        //{
        //    string newWord = "";
        //    VizorWord.Text = "";
        //    foreach (var word in AddEachWordToList(ReadWrite.ReadTXT(file.File), TextList))
        //    {
        //        VizorWord.Text += word;
        //        string words = word;
        //        int i = 0;
        //        var tasks = new Task<string>[ForbidenWordsList.Count];
        //        foreach (var forbiddenWord in ForbidenWordsList)
        //        {
        //            tasks[i] = new Task<string>(() => WordSearch.Equel(word, forbiddenWord));
        //            tasks[i].Start();

        //            if (/*forbiddenWord.ToLower() == word.ToLower()*/ tasks[i].Result != "")
        //            {
        //                ReadWrite.WriteLogToFile(file.File, word);
        //                ReadWrite.WrireFile(ReadWrite.ReadTXT(file.File), $"C:/Users/admin/Desktop/ЭкзаменСлова/{file.FileName}");
        //                words = WordSearch.WordToStar(word);
        //            }
        //            i++;
        //        }
        //        newWord += words;
        //    }
        //    /*await Task.Run(() =>*/
        //     ReadWrite.WrireFile(newWord, file.File)/*)*/;
        //}

        //private static void WriteLogToFile(string file, string word)
        //{
        //    StreamWriter dataWord = new("C:/Users/admin/Desktop/ЭкзаменСлова/ForbiddenWordLog.txt", true);
        //    LogStatistic.ListLogCount(file, word);
        //    string data = ReadWrite.WriteLog(file, word);
        //    dataWord.WriteLine(data);
        //    dataWord.Close();
        //}

        //private static void WriteLogStatistic()
        //{
        //    StreamWriter dataWord = new("C:/Users/admin/Desktop/ЭкзаменСлова/ForbiddenWordLog.txt", true);
        //    string data = LogStatistic.Statistic();
        //    dataWord.WriteLine(data);
        //    dataWord.Close();
        //}

        //private static string WordToStar(string word)
        //{
        //    string star = "";
        //    foreach (var symbl in word)
        //    {
        //        star += "*";
        //    }
        //    return star;
        //}

        //private static string Equel(string word, string forbiddenWord)
        //{
        //    string tempWord = "";
        //    if (word.Length >= forbiddenWord.Length && word.Length <= forbiddenWord.Length + 2)
        //    {
        //        if ((word.ToLower() == forbiddenWord.ToLower()) || word.ToLower().Remove(word.Length - 1) == forbiddenWord.ToLower() ||
        //            word.ToLower().Remove(word.Length - 2) == forbiddenWord.ToLower())
        //        {
        //            tempWord = word;
        //        }
        //        else
        //        {
        //            tempWord = "";
        //        }
        //    }
        //    return tempWord;
        //}

        //public static void ShowDiagramPie(double[] startNum, string[] word)
        //{
        //    //var plt = new ScottPlot.Plot(600, 400);
        //    //var pie = plt.AddPie(startNum);
        //    //pie.Explode = true;
        //    //pie.SliceLabels = word;
        //    //pie.ShowLabels = true;
        //    //pie.ShowPercentages = true;
        //    //pie.ShowValues = true;
        //    //plt.Legend();
        //    //new ScottPlot.FormsPlotViewer(plt).ShowDialog();
        //}

        private void SearchDiagramRound_Click(object sender, RoutedEventArgs e)
        {
            SearchDiagramRound.IsChecked = true;
            SearchDiagramColumn.IsChecked = false;
            SearchDiagramRadius.IsChecked = false;
            AllDiagrams();
        }

        private void SearchDiagramColumn_Click(object sender, RoutedEventArgs e)
        {
            SearchDiagramRound.IsChecked = false;
            SearchDiagramColumn.IsChecked = true;
            SearchDiagramRadius.IsChecked = false;
            AllDiagrams();
        }

        private void SearchDiagramRadius_Click(object sender, RoutedEventArgs e)
        {
            SearchDiagramRound.IsChecked = false;
            SearchDiagramColumn.IsChecked = false;
            SearchDiagramRadius.IsChecked = true;
            AllDiagrams();
        }

        private void AllDiagrams()
        {
            if (ListLog.Count > 0)
            {
                ScottPlot.WpfPlot diagramma = DiagramView;
                if (SearchDiagramRound.IsChecked == true)
                {
                    Diagrams.RoundDiagram(diagramma);
                }
                else if (SearchDiagramColumn.IsChecked == true)
                {
                    Diagrams.ColumnDiagram(diagramma);
                }
                else if (SearchDiagramRadius.IsChecked == true)
                {
                    Diagrams.RadialDiagram(diagramma);
                }
                
            }
            else
            {
                MessageBox.Show($"Запрещенные слова не найдены.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                Keyboard.Focus(EnterText);
            }
        }

        private void Diagram_Click(object sender, RoutedEventArgs e)
        {
            AllDiagrams();
        }

        private void OpenDirectory_Click(object sender, RoutedEventArgs e)
        {
            string filename = "";
            OpenFileDialog dialogWindow = new OpenFileDialog();
            dialogWindow.FileName = "";
            //dialogWindow.DefaultExt = ".txt";
            dialogWindow.Filter = null /*"Text documents (.txt)|*.txt"*/;
            dialogWindow.Multiselect = true;
            dialogWindow.InitialDirectory = ListConfig[ListConfig.Count - 1].path /*@"C:\Users\admin\Desktop\МТС"*/;

            bool? result = dialogWindow.ShowDialog();

            if (result == true)
            {
                filename = dialogWindow.FileName;
            }
            FolderFile.DialogWindiowSeachFile(filename);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ListDirectory.Clear();
            ListFile.Clear();
            DiagramView.Reset();
            VizorWord.Text = "";
            WindowSearch.Text = "";
        }

        private void SetDirectory_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveLoad.SaveConfigDirectory(folderDialog);
                VizorPath.Text = folderDialog.SelectedPath;
            }   
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.IsEnabled = true;
            timer.Tick += (start, t) => { Time.Text = DateTime.Now.ToLongTimeString(); };
            timer.Tick += (o, t) => { Date.Text = DateTime.Now.ToLongDateString(); };
            timer.Start();
        }

        private void DirectoryForbidden(object sender, RoutedEventArgs e)
        {
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveLoad.SaveConfigDirectoryForbidden(folderDialog);
                VizorPathForbidden.Text = folderDialog.SelectedPath;
            }
        }
    }
}
