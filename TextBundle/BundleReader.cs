using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.IO;

namespace TextBundle
{
    public class BundleReader
    {
        public static void Read(string input, string output, bool ls)
        {
            if (File.Exists(input))
            {
                if (string.IsNullOrEmpty(output))
                {
                    output = $"{input}_Output";
                }
                Console.WriteLine($"文件会写入到: {output}");
                Directory.CreateDirectory(output);
                using (var file_stream = File.OpenRead(input))
                {
                    using (var reader = new BigEndianReader(file_stream))
                    {
                        var magic = reader.ReadInt32();
                        if (magic == 0x7A684244)
                        {
                            Bundle bundle = new Bundle
                            {
                                file_number_ = reader.ReadInt32()
                            };

                            // file info
                            for (int i = 0; i < bundle.file_number_; ++i)
                            {
                                bundle.file_info_.Add(new BundleInfo
                                {
                                    index_ = reader.ReadUInt32(),
                                    id_ = reader.ReadString(),
                                    size_ = reader.ReadUInt32(),
                                    crc_ = reader.ReadUInt32()
                                });
                            }

                            // file data
                            foreach (var info in bundle.file_info_)
                            {
                                reader.Position = info.index_;
                                var raw_data = reader.ReadBytes((int)info.size_);
                                var decrypted = Cipher.Decrypt(raw_data);

                                using (var output_stream = new MemoryStream())
                                {
                                    using (var memory_stream = new MemoryStream(decrypted))
                                    {
                                        using (var zlib_stream = new InflaterInputStream(memory_stream))
                                        {
                                            zlib_stream.CopyTo(output_stream);
                                        }
                                    }
                                    File.WriteAllBytes($@"{output}\{info.id_}", output_stream.ToArray());
                                }

                                if (ls)
                                {
                                    Console.WriteLine($"id: {info.id_} idx: {info.index_} size: {info.size_}");
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}