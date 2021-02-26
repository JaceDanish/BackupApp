using System;
using System.IO;

namespace BackupApp
{
    class Program
    {
        private static int fileCount = 1;
        static void Main(string[] args)
        {
            String sourceDir = Directory.GetCurrentDirectory();
            String targetDir;
            
            if (args.Length == 1 && Directory.Exists(args[0]))
            {
                Console.WriteLine("Destination folder found");
                targetDir = args[0];
            }
            else
            {
                Console.WriteLine("Destination folder not found");
                Console.WriteLine("Usage example: BackupApp D:\\directory");
                return;
            }
            BackUpRec(targetDir, sourceDir);

            Console.WriteLine("Back up complete");
        }

        static void BackUpRec(String targetDir, String sourceDir)
        {
            //file contains full path name
            foreach (String file in Directory.GetFiles(sourceDir))
            {
                String[] path = file.Split('\\');
                byte[] b = File.ReadAllBytes(file);

                File.WriteAllBytes($"{targetDir}\\{path[path.Length - 1]}", b);
                Console.WriteLine(fileCount++);
            }

            //dirs contain full path name
            foreach (String dirs in Directory.GetDirectories(sourceDir))
            {
                String[] path = dirs.Split('\\');
                String dir = path[path.Length - 1];

                if (!Directory.Exists($"{targetDir}\\{dir}"))
                {
                    Directory.CreateDirectory($"{targetDir}\\{dir}");
                }
                BackUpRec($"{targetDir}\\{dir}", $"{sourceDir}\\{dir}");
            }
        }
    }
}
