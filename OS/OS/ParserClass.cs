using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace OS
{
    internal class ParserClass
    {
        public static void ChangeDirectory(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                Console.WriteLine(Program.currentDirectory.Dir_Namee);
                return;
            }
            if (path == ".")
            {               
                return;
            }
            if (path.StartsWith(".."))
            {
                string[] levelsUp = path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
                int levels = levelsUp.Length;
                for (int i = 0; i < levels; i++)
                {
                    int lastBackslash = Program.path.LastIndexOf("\\");

                    if (Program.currentDirectory.Parent != null)
                    {
                        Program.currentDirectory = Program.currentDirectory.Parent;
                        Program.path = Program.path.Substring(0, lastBackslash);
                        Program.currentDirectory.Read_Directory();
                        
                    }
                    else
                    {
                        Console.WriteLine("Error: Cannot move above the root directory.");
                        return;
                    }
                }
                Console.WriteLine($"Changed to directory: '{new string (Program.currentDirectory.Dir_Namee).Trim()}'");
                return;
            }
            if (path.Contains("\\") || path.Contains("/"))
            {
                Directory targetDir = Command_Line.MoveToDir(path, Program.currentDirectory);
                if (targetDir != null)
                {
                    Program.currentDirectory = targetDir;
                    // need to update path here 
                    Program.path = path;
                    Console.WriteLine($"Changed to directory: '{new string(Program.currentDirectory.Dir_Namee).Trim()}'");
                }
                else
                {
                    Console.WriteLine("Error: The system cannot find the specified folder.");
                }
                return;
            }
            int index = Program.currentDirectory.search_Directory(path);
            if (index == -1)
            {
                Console.WriteLine($"Error: Directory '{path}' not found.");
                return;
            }

            Directory_Entry entry = Program.currentDirectory.DirectoryTable[index];
            if (entry.dir_Attr != 0x10)
            {
                Console.WriteLine($"Error: '{path}' is not a directory.");
                return;
            }

            else
            {
                string name = new string(entry.Dir_Namee).Trim();
                if (name.Contains("\0"))
                {
                    name = name.Replace("\0", " ");
                }
                name = name.Trim();
                Directory newDir = new Directory(name.ToCharArray(), entry.dir_Attr, entry.dir_First_Cluster, Program.currentDirectory);
                newDir.Read_Directory();
                Program.currentDirectory = newDir;
                Program.path = Program.path + "\\" + path;
                Console.WriteLine($"Changed to directory: '{new string(Program.currentDirectory.Dir_Namee).Trim()}'");
            }
                
            }
           
        }
    }

