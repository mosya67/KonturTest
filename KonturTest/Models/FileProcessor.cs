using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace KonturTest.Models
{
    public class FileProcessor
    {
        public async Task StartProcessAsync(string inputPath, string outputPath, Action<int> onProgressChanged)
        {
            await Task.Run(async () =>
            {
                using (var reader = new BinaryReader(File.Open(inputPath, FileMode.Open)))
                using (var writer = new StreamWriter(outputPath))
                {
                    long totalBytes = new FileInfo(inputPath).Length;

                    // запишем в файл заголовки
                    await writer.WriteLineAsync("Packet;Channel 1;Channel 2;Channel 3;Channel 4;Channel 5;Channel 6;");

                    // Чтение пакетов
                    while (reader.BaseStream.Position < totalBytes)
                    {
                        Packet packet = ReadPacket(reader);

                        // запись в файл
                        await WritePacket(writer, packet);

                        onProgressChanged?.Invoke((int)(reader.BaseStream.Position / totalBytes * 100)); // изменение прогресс бара
                    }
                }
            });
        }

        private Packet ReadPacket(BinaryReader reader)
        {
            Packet packet = new();
            const int numberBlocksInPacket = 60;

            reader.BaseStream.Position += 8; // информация о размере и типе нам не нужна (первые 8 байтов)
            packet.PacketNumber = reader.ReadUInt32();
            reader.BaseStream.Position += 4; // Информация о времени нам тоже не нужна

            // Чтение блоков с данными
            for (int i = 0; i < numberBlocksInPacket; i++)
            {
                // Считаем сумму каналов. Потом нужно будет считать среднее
                packet.ChannelSums[0] += reader.ReadInt32();
                packet.ChannelSums[1] += reader.ReadInt32();
                packet.ChannelSums[2] += reader.ReadInt32();
                packet.ChannelSums[3] += reader.ReadInt32();
                packet.ChannelSums[4] += reader.ReadInt32();
                packet.ChannelSums[5] += reader.ReadInt32();
            }

            return packet;
        }
        
        private async Task WritePacket(StreamWriter writer, Packet packet)
        {
            const int numberBlocksInPacket = 60;
            long averageChannel1 = packet.ChannelSums[0] / numberBlocksInPacket;
            long averageChannel2 = packet.ChannelSums[1] / numberBlocksInPacket;
            long averageChannel3 = packet.ChannelSums[2] / numberBlocksInPacket;
            long averageChannel4 = packet.ChannelSums[3] / numberBlocksInPacket;
            long averageChannel5 = packet.ChannelSums[4] / numberBlocksInPacket;
            long averageChannel6 = packet.ChannelSums[5] / numberBlocksInPacket;

            await writer.WriteLineAsync($"{packet.PacketNumber};{averageChannel1};{averageChannel2};{averageChannel3};{averageChannel4};{averageChannel5};{averageChannel6};");
        }

        private class Packet
        {
            public uint PacketNumber { get; set; }
            public long[] ChannelSums { get; set; } = new long[6];
        }
    }
}
