using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KonturTest.ViewModels
{
    public abstract class ModuleBase : INotifyPropertyChanged
    {
        private string _dataFilePath;
        public string DataFilePath { get => _dataFilePath; set { _dataFilePath = value; OnPropertyChanged(); } }

        private string _csvFilePath;
        public string CsvFilePath { get => _csvFilePath; set { _csvFilePath = value; OnPropertyChanged(); } }

        private int _progressValue;
        public int ProgressValue { get => _progressValue; set { _progressValue = value; OnPropertyChanged(); } }

        private bool _isButtonEnabled = true;
        public bool IsButtonEnabled { get => _isButtonEnabled; set { _isButtonEnabled = value; OnPropertyChanged(); } }

        public ICommand OpenDataCommand { get; }
        public ICommand OpenCsvCommand { get; }
        public ICommand StartCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ModuleBase()
        {
            OpenDataCommand = new RelayCommand(OpenData);
            OpenCsvCommand = new RelayCommand(OpenCsv);
            StartCommand = new RelayCommand(async () => await StartProcessInternal());
        }

        private void OpenData()
        {
            var dialog = new OpenFileDialog();

            dialog.Title = "Выберите файл с данными";

            if (dialog.ShowDialog() == true) DataFilePath = dialog.FileName;
        }
        private void OpenCsv()
        {
            var dialog = new SaveFileDialog();

            dialog.Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";
            dialog.FileName = "result.csv";
            dialog.Title = "Выберите путь для сохранения результата";

            if (dialog.ShowDialog() == true) CsvFilePath = dialog.FileName;
        }

        private async Task StartProcessInternal()
        {
            if (string.IsNullOrEmpty(DataFilePath) || string.IsNullOrEmpty(CsvFilePath)) return;
            IsButtonEnabled = false;
            try { await DoWorkAsync(); }
            finally { IsButtonEnabled = true; }
        }

        protected abstract Task DoWorkAsync();
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
