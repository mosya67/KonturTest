using KonturTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KonturTest.ViewModels
{
    public class FirstModuleViewModel : ModuleBase
    {
        protected override async Task DoWorkAsync()
        {
            if (string.IsNullOrEmpty(DataFilePath) || string.IsNullOrEmpty(CsvFilePath))
            {
                MessageBox.Show("Сначала выберите файлы!");
                return;
            }

            try
            {
                IsButtonEnabled = false;

                var processor = new FileProcessor();
                await processor.StartProcessAsync(DataFilePath, CsvFilePath, p => ProgressValue = p);
                MessageBox.Show($"Готово!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
            finally
            {
                IsButtonEnabled = true;
                ProgressValue = 0;
            }
        }
    }
}
