using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.IO;

namespace TextBundle
{
    public class BundleWriter
    {
        public static void Write(string input, string output, bool ls)
        {
            if (Directory.Exists(input))
            {
                if (string.IsNullOrEmpty(output))
                {
                    output = $"{input}.bin";
                }
                Console.WriteLine($"文件会写入到: {output}");
                if (string.IsNullOrEmpty(Path.GetDirectoryName(output)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(output));
                }
                using (var file_stream_ = File.OpenWrite(output))
                {
                    using (var writer_ = new BigEndianWriter(file_stream_))
                    {
                        var input_path_ = new DirectoryInfo(input);
                        var file_data = new List<Tuple<BundleInfo, byte[]>>();

                        writer_.WriteInt32(0x7A684244); // magic number

                        foreach (var file in input_path_.GetFiles("*", SearchOption.AllDirectories))
                        {
                            var data = File.ReadAllBytes(file.FullName);
                            file_data.Add(new Tuple<BundleInfo, byte[]>
                                (
                                    new BundleInfo
                                    {
                                        index_ = 0,
                                        id_ = file.Name,
                                        size_ = 0,
                                        crc_ = 0
                                    },
                                    data
                                )
                            );
                        }

                        writer_.WriteInt32(file_data.Count); // files number

                        // reserved for files info
                        foreach (var file in file_data)
                        {
                            var placeholder = new byte[12 + 1 + file.Item1.id_.Length];
                            writer_.Write(placeholder);
                        }

                        // file data
                        int head_total_size = 4 + 4;

                        foreach (var file in file_data)
                        {
                            using (var output_stream = new MemoryStream())
                            {
                                using (var memory_stream = new MemoryStream(file.Item2))
                                {
                                    using (var zlib_stream = new DeflaterOutputStream(output_stream))
                                    {
                                        memory_stream.CopyTo(zlib_stream);
                                    }
                                }
                                var encrypted = Cipher.Encrypt(output_stream.ToArray());
                                writer_.Write(encrypted);
                                file.Item1.size_ = (uint)encrypted.Length;
                            }

                            head_total_size += 12 + 1 + file.Item1.id_.Length;
                        }

                        file_data[0].Item1.index_ = (uint)head_total_size;
                        for (int i = 1; i < file_data.Count; ++i)
                        {
                            file_data[i].Item1.index_ = file_data[i - 1].Item1.index_ + file_data[i - 1].Item1.size_;
                        }

                        writer_.Position = 8;

                        // file info
                        foreach (var file in file_data)
                        {
                            writer_.WriteUInt32(file.Item1.index_);
                            writer_.WriteString(file.Item1.id_);
                            writer_.WriteUInt32(file.Item1.size_);
                            writer_.WriteUInt32(file.Item1.crc_);

                            if (ls)
                            {
                                Console.WriteLine($"id: {file.Item1.id_} idx: {file.Item1.index_} size: {file.Item1.size_}");
                            }
                        }
                    }
                }
            }
        }
    }
}
