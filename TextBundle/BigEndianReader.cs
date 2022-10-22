using System.Buffers.Binary;
using System.IO;

namespace TextBundle
{
    public class BigEndianReader : BinaryReader
    {
        private readonly byte[] buffer;

        public BigEndianReader(Stream stream) : base(stream)
        {
            buffer = new byte[8];
        }

        public long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        public override string ReadString()
        {
            int str_size = base.ReadByte();
            var str = base.ReadBytes(str_size);
            return System.Text.Encoding.UTF8.GetString(str);
        }

        public override short ReadInt16()
        {
            Read(buffer, 0, 2);
            return BinaryPrimitives.ReadInt16BigEndian(buffer);
        }

        public override int ReadInt32()
        {
            Read(buffer, 0, 4);
            return BinaryPrimitives.ReadInt32BigEndian(buffer);
        }

        public override long ReadInt64()
        {
            Read(buffer, 0, 8);
            return BinaryPrimitives.ReadInt64BigEndian(buffer);
        }

        public override ushort ReadUInt16()
        {
            Read(buffer, 0, 2);
            return BinaryPrimitives.ReadUInt16BigEndian(buffer);
        }

        public override uint ReadUInt32()
        {
            Read(buffer, 0, 4);
            return BinaryPrimitives.ReadUInt32BigEndian(buffer);
        }

        public override ulong ReadUInt64()
        {
            Read(buffer, 0, 8);
            return BinaryPrimitives.ReadUInt64BigEndian(buffer);
        }
    }
}
