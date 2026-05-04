using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonturTest.Models
{
    public class BitDataProcessor
    {
        public async Task StartProcessAsync(string inputPath, string outputPath, Action<int> onProgressChanged)
        {
            /* формат данных
            * 
            * IsEnabled1: 1 бит.
            * Value11: 3 бита.
            * Value12: 3 бита.
            * Value13: 9 бит.
            * IsEnabled2: 1 бит.
            * Value21: 11 бит.
            * Value22: 4 бита.
            * 
            */

            await Task.Run(async () =>
            {
                using (var reader = new BinaryReader(File.Open(inputPath, FileMode.Open)))
                using (var writer = new StreamWriter(outputPath))
                {
                    long totalBytes = new FileInfo(inputPath).Length;

                    // запишем в файл заголовки
                    await writer.WriteLineAsync("IsEnabled1;Value11;Value12;Value13;IsEnabled2;Value21;Value22;");

                    while (reader.BaseStream.Position < totalBytes)
                    {
                        Record record = new();

                        // чтение
                        uint numb = reader.ReadUInt32();
                        record.IsEnabled1 = Convert.ToBoolean(numb & 0b1);
                        record.Value11 = (numb >> 1) & 0b111;
                        record.Value12 = (numb >> 4) & 0b111;
                        record.Value13 = (numb >> 7) & 0b111_111_111;
                        record.IsEnabled2 = Convert.ToBoolean((numb >> 16) & 0b1);
                        record.Value21 = (numb >> 17) & 0b111_111_111_11;
                        record.Value22 = (numb >> 28) & 0b1111;

                        // запись
                        await writer.WriteLineAsync($"{record.IsEnabled1};{record.Value11};{record.Value12};{record.Value13};{record.IsEnabled2};{record.Value21};{record.Value22};");

                        onProgressChanged?.Invoke((int)(reader.BaseStream.Position / totalBytes * 100)); // изменение прогресс бара
                    }
                }
            });
        }

        private class Record
        {
            public bool IsEnabled1 { get; set; }
            public uint Value11 { get; set; }
            public uint Value12 { get; set; }
            public uint Value13 { get; set; }
            public bool IsEnabled2 { get; set; }
            public uint Value21 { get; set; }
            public uint Value22 { get; set; }

        }
    }
}
