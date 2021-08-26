using System;
using System.IO;
using System.Text;

namespace BinaryShenanigans.Benchmarks
{
    public static class Utils
    {
        public static BinaryReader SetupBinaryReader(Action<byte[]> writeValue, int size, bool exposedBuffer = true)
        {
            var buffer = new byte[size];
            writeValue(buffer);
            
            var ms = new MemoryStream(buffer, 0, buffer.Length, false, exposedBuffer);
            var br = new BinaryReader(ms, Encoding.UTF8, false);

            return br;
        }

        public static BinaryReader SetupBinaryReaderWithFileStream(Action<byte[]> writeValue, int size)
        {
            var buffer = new byte[size];
            writeValue(buffer);

            var file = Path.GetRandomFileName();
            File.WriteAllBytes(file, buffer);

            var fs = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            var br = new BinaryReader(fs, Encoding.UTF8, false);

            return br;
        }
        
        public static System.IO.BinaryWriter SetupBinaryBinaryWriter(int size)
        {
            var buffer = new byte[size];
            
            var ms = new MemoryStream(buffer, 0, buffer.Length, true, true);
            var bw = new System.IO.BinaryWriter(ms, Encoding.UTF8, false);

            return bw;
        }
    }
}