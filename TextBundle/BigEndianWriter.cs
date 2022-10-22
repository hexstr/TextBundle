using System.Buffers.Binary;
using System.IO;

namespace TextBundle
{
    public class BigEndianWriter : BinaryWriter
    {
        private readonly byte[] buffer;

        public BigEndianWriter(Stream stream) : base(stream)
        {
            buffer = new byte[8];
        }

        public long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        public void WriteString(string val)
        {
            base.Write(System.BitConverter.GetBytes(val.Length), 0, 1);
            base.Write(System.Text.Encoding.UTF8.GetBytes(val));
        }

        public void WriteInt16(short val)
        {
            BinaryPrimitives.WriteInt16BigEndian(buffer, val);
            Write(buffer, 0, 2);
        }

        public void WriteInt32(int val)
        {
            BinaryPrimitives.WriteInt32BigEndian(buffer, val);
            Write(buffer, 0, 4);
        }

        public void WriteInt64(long val)
        {
            BinaryPrimitives.WriteInt64BigEndian(buffer, val);
            Write(buffer, 0, 8);
        }

        public void WriteUInt16(ushort val)
        {
            BinaryPrimitives.WriteUInt16BigEndian(buffer, val);
            Write(buffer, 0, 2);
        }

        public void WriteUInt32(uint val)
        {
            BinaryPrimitives.WriteUInt32BigEndian(buffer, val);
            Write(buffer, 0, 4);
        }

        public void WriteUInt64(ulong val)
        {
            BinaryPrimitives.WriteUInt64BigEndian(buffer, val);
            Write(buffer, 0, 8);
        }
    }
}
