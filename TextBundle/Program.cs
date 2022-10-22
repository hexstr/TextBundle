using System;

namespace TextBundle
{
    class Program
    {
        static void Main(params string[] args)
        {
            try
            {
                if (args.Length <= 1)
                {
                    Console.WriteLine(
    @"参数:
        -i      - 输入文件名或目录，可以是完整路径或者直接输入运行目录下的文件/文件夹名
        -o      - 输出文件名或目录，同上，可留空，为空会和输入创建在同一级目录
        -c      - 创建.bin文件
        -d      - 从.bin文件导出
        -v      - 输出详细信息

例子：
        C:\Desktop\TextBundle> .\TextBundle.exe -i C:\Desktop\4\ -o 4.chara -c -v
        从C:\Desktop\4\读取所有文件（包括子目录），打包为C:\Desktop\TextBundle\4.chara

        C:\Desktop\TextBundle> .\TextBundle.exe -i C:\Desktop\TextBundle\4.chara -o D:\4 -d -v
        读取C:\Desktop\TextBundle\4.chara，输出到D:\4"
                    );
                    Console.ReadKey(true);
                    return;
                }

                string input = string.Empty, output = string.Empty;
                bool create = false, dump = false, verbose = false;

                var itor = args.GetEnumerator();
                while (itor.MoveNext())
                {
                    var current = itor.Current.ToString();
                    switch (current)
                    {
                        case "-i":
                            {
                                if (itor.MoveNext())
                                {
                                    input = itor.Current.ToString();
                                }
                                break;
                            }
                        case "-o":
                            if (itor.MoveNext())
                            {
                                output = itor.Current.ToString();
                            }
                            break;
                        case "-c":
                            create = true;
                            break;
                        case "-d":
                            dump = true;
                            break;
                        case "-v":
                            verbose = true;
                            break;
                    }
                }
                if (input != string.Empty)
                {
                    if (create)
                    {
                        BundleWriter.Write(input, output, verbose);
                    }
                    else if (dump)
                    {
                        BundleReader.Read(input, output, verbose);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("按任意键退出...");
            Console.ReadKey(true);
        }
    }
}
