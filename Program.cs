using System;
using System.IO;

namespace BackupApp
{
    class Program
    {
        //counter - just to show program is running
        private static decimal megabytes = 0;
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
                String fileName = path[path.Length - 1];
                
                //if file exists: if target file was last accessed same time or later than source, skip overwrite
                if (File.Exists($"{targetDir}\\{fileName}") &&
                    DateTime.Compare(File.GetLastAccessTime($"{targetDir}\\{fileName}"), File.GetLastAccessTime(file)) > -1 )
                {
                    continue;
                }

                byte[] b = File.ReadAllBytes(file);
                megabytes += b.Length / 1000000;
                File.WriteAllBytes($"{targetDir}\\{fileName}", b);
                Console.WriteLine($"MB: {megabytes}");
            }

            //dirs contain full path name
            foreach (String dirs in Directory.GetDirectories(sourceDir))
            {
                String[] path = dirs.Split('\\');
                String dir = path[path.Length - 1];
                
                //Skip binaries and object code
                if (!dir.Equals("obj") && !dir.Equals("bin"))
                {
                    if (!Directory.Exists($"{targetDir}\\{dir}"))
                    {
                        Directory.CreateDirectory($"{targetDir}\\{dir}");
                    }
                    BackUpRec($"{targetDir}\\{dir}", $"{sourceDir}\\{dir}");
                }
            }
        }
    }
}
