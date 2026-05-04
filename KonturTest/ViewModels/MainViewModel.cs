using KonturTest.Models;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace KonturTest.ViewModels
{
    public class MainViewModel
    {
        private const string ConfigPath = "config.json";
        public FirstModuleViewModel Module1 { get; } = new FirstModuleViewModel();
        public SecondModuleViewModel Module2 { get; } = new SecondModuleViewModel();

        public MainViewModel()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            if (File.Exists(ConfigPath))
            {
                var json = File.ReadAllText(ConfigPath);
                var settings = JsonSerializer.Deserialize<AppSettings>(json);
                if (settings != null)
                {
                    Module1.DataFilePath = settings.Path1_Input;
                    Module1.CsvFilePath = settings.Path1_Output;
                    Module2.DataFilePath = settings.Path2_Input;
                    Module2.CsvFilePath = settings.Path2_Output;
                }
            }
        }

        public void SaveSettings()
        {
            var settings = new AppSettings
            {
                Path1_Input = Module1.DataFilePath,
                Path1_Output = Module1.CsvFilePath,
                Path2_Input = Module2.DataFilePath,
                Path2_Output = Module2.CsvFilePath
            };
            var json = JsonSerializer.Serialize(settings);
            File.WriteAllText(ConfigPath, json);
        }
    }
}