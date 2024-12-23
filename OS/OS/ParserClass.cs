using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OS
{
    internal class ParserClass
    {
        // method dir to help me 
        public static void Dir(string name)
        {
           
            int file_Counter = 0;
            int folder_Counter = 0;
            int file_Sizes = 0;
            int total_File_Size = 0;
            Directory cc = ParserClass.MoveToDir(name, Program.currentDirectory);
            string name_l = new string(cc.Dir_Namee);
            Console.WriteLine($"Directory of {name} : \n");
            for (int i = 0; i < cc.DirectoryTable.Count; i++)
            {
                if (cc.DirectoryTable[i].dir_Attr == 0x0)
                {
                    file_Counter++;
                    file_Sizes += cc.DirectoryTable[i].dir_FileSize;
                    total_File_Size += file_Sizes;
                    string m = string.Empty;
                    m += new string(cc.DirectoryTable[i].Dir_Namee);
                    Console.WriteLine($"\t\t{file_Sizes}\t\t" + m);
                   // Console.WriteLine();
                }
                else if (cc.DirectoryTable[i].dir_Attr == 0x10) // لو في فولدر 
                {
                    folder_Counter++;
                    string S = new string(cc.DirectoryTable[i].Dir_Namee);
                    Console.WriteLine("\t\t<DIR>\t\t" + S);
                }

            }
            Console.Write($"\t\t\t{file_Counter} File(s)\t ");
            if (file_Counter > 0)
            {
                Console.Write(total_File_Size);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
            }
            Console.WriteLine($"\t\t\t{folder_Counter} Dir(s)\t {Mini_FAT.get_Free_Size()} bytes free");
        }


        public static Directory MoveToDir(string fullPath, Directory currentDirectory)
        {
            string[] parts = fullPath.Split('\\');
            if (parts.Length == 0)
            {
                Console.WriteLine("Error: Invalid path.");
                return null;
            }

            Directory targetDirectory = currentDirectory;

            string s = new string(currentDirectory.Dir_Namee);
            if (s.Contains("\0"))
                s = s.Replace("\0", " ");
            s = s.Trim();
            if (parts[0].Equals(s.Trim('\0'), StringComparison.OrdinalIgnoreCase))
            {
                parts = parts.Skip(1).ToArray();
            }

            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                {
                    continue;
                }

                int index = targetDirectory.search_Directory(part);
                if (index == -1)
                {
                    //Console.WriteLine($"Error: Directory '{part}' not found.");
                    return null;
                }

                Directory_Entry entry = targetDirectory.DirectoryTable[index];

                if ((entry.dir_Attr & 0x10) != 0x10)
                {
                    Console.WriteLine($"Error: '{part}' is not a directory.");
                    return null;
                }

                targetDirectory = new Directory(entry.Dir_Namee, entry.dir_Attr, entry.dir_First_Cluster, targetDirectory);
                targetDirectory.Read_Directory();
            }


            return targetDirectory;
        }
        public static Directory MoveToFile(string fullPath, Directory currentDirectory)
        {
            string[] parts = fullPath.Split('\\');
            if (parts.Length == 0)
            {
                Console.WriteLine("Error: Invalid path.");
                return null;
            }

            Directory targetDirectory = currentDirectory;

            string s = new string(currentDirectory.Dir_Namee);
            if (s.Contains("\0"))
                s = s.Replace("\0", " ");
            s = s.Trim();
            if (parts[0].Equals(s.Trim('\0'), StringComparison.OrdinalIgnoreCase))
            {
                parts = parts.Skip(1).ToArray();
            }

            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                {
                    continue;
                }

                int index = targetDirectory.search_Directory(part);
                if (index == -1)
                {
                    //Console.WriteLine($"Error: Directory '{part}' not found.");
                    return null;
                }

                Directory_Entry entry = targetDirectory.DirectoryTable[index];

                if (entry.dir_Attr == 0x10)
                {
                    Console.WriteLine($"Error: '{part}' is  a Directory.");
                    return null;
                }

                targetDirectory = new Directory(entry.Dir_Namee, entry.dir_Attr, entry.dir_First_Cluster, targetDirectory);
                targetDirectory.Read_Directory();
            }


            return targetDirectory;
        }

        public static void ChangeDirectory(string path)
        {
           
            if (path == ".")
            {
                return;
            }
            if (path.StartsWith(".."))
            {
                string[] levelsUp = path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
                int l = levelsUp.Length;
                for (int i = 0; i < l; i++)
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
                Console.WriteLine($"Changed to directory: '{new string(Program.currentDirectory.Dir_Namee).Trim()}'");
                return;
            }
            if (path.Contains("\\") || path.Contains("/"))
            {
                Directory targetDir = MoveToDir(path, Program.currentDirectory);
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
                Console.WriteLine($"Changed to directory to '{new string(Program.currentDirectory.Dir_Namee).Trim()}'");
            }

        } //cd 

        public static void RemoveDirectory(string[] names)
        {
            foreach (var originalName in names)
            {
                string name = originalName.Trim();

                int index = Program.currentDirectory.search_Directory(name);

                if (index == -1)
                {
                    Console.WriteLine($"Error: Directory '{name}' not found.");
                    continue;
                }

                Directory_Entry entry = Program.currentDirectory.DirectoryTable[index];

                if (entry.dir_Attr != 0x10)
                {
                    Console.WriteLine($"Error: '{name}' is not a directory.");
                    continue;
                }

                int firstCluster = entry.dir_First_Cluster;
                Directory dirToDelete = new Directory(name.ToCharArray(), entry.dir_Attr, firstCluster, Program.currentDirectory);
                dirToDelete.Read_Directory();

                if (dirToDelete.DirectoryTable.Count > 0)
                {
                    Console.WriteLine($"Error: Directory '{name}' is not empty.");
                    Console.Write($"Are you sure you want to delete the directory '{name}' and all its contents? [yes, no]: ");
                    string answer = Console.ReadLine();

                    if (answer?.ToLower() == "yes" || answer?.ToLower() == "y")
                    {
                        dirToDelete.delete_Directory();
                        Program.currentDirectory.DirectoryTable.RemoveAt(index);
                        Program.currentDirectory.Write_Directory();
                        Console.WriteLine($"Directory '{name}' and its contents have been deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Directory '{name}' was not deleted.");
                    }
                }
                else
                {
                    Console.Write($"Are you sure you want to delete the empty directory '{name}'? [yes, no]: ");
                    string answer = Console.ReadLine();

                    if (answer?.ToLower() == "yes" || answer?.ToLower() == "y")
                    {
                        dirToDelete.delete_Directory();
                        Program.currentDirectory.DirectoryTable.RemoveAt(index);
                        Program.currentDirectory.Write_Directory();
                        Console.WriteLine($"Directory '{name}' has been deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Directory '{name}' was not deleted.");
                    }
                }
            }
        }//rd



        public static void list_OF_Directory(string name)
        {
            if(name == "")
            {
                Console.WriteLine($"Error : dir command syntax is \r\ndir \r\nor \r\ndir [directory] \r\n[directory] can be directory name or fullpath of a directory \r\nor file name or full path of a file.");
                return;
            }
            if (name.Contains("\\"))
            {
                object o;
                o = ParserClass.MoveToDir(name, Program.currentDirectory);
                if (o != null)
                {
                    //Directory cc = ParserClass.MoveToDir(name, Program.currentDirectory);
                    //int file_Counter = 0;
                    //int folder_Counter = 0;
                    //int file_Sizes = 0;

                    //string name_l = new string(cc.Dir_Namee);
                    //Console.WriteLine($"Directory of {name} : \n");
                    //for (int i = 0; i < cc.DirectoryTable.Count; i++)
                    //{
                    //    if (cc.DirectoryTable[i].dir_Attr == 0x0)
                    //    {
                    //        file_Counter++;
                    //        file_Sizes += cc.DirectoryTable[i].dir_FileSize;
                    //        string m = string.Empty;
                    //        m += new string(cc.DirectoryTable[i].Dir_Namee);
                    //        Console.WriteLine("\t\t<File>\t\t" + m);
                    //        Console.WriteLine();
                    //    }
                    //    else if (cc.DirectoryTable[i].dir_Attr == 0x10) // لو في فولدر 
                    //    {
                    //        folder_Counter++;
                    //        string S = new string(cc.DirectoryTable[i].Dir_Namee);
                    //        Console.WriteLine("\t\t<DIR>\t\t" + S);
                    //    }

                    //}
                    //Console.Write($"\t\t\t{file_Counter} File(s)\t ");
                    //if (file_Counter > 0)
                    //{
                    //    Console.Write(file_Sizes);
                    //    Console.WriteLine();
                    //}
                    //else
                    //{
                    //    Console.WriteLine();
                    //}
                    //Console.WriteLine($"\t\t\t{folder_Counter} Dir(s)\t {Mini_FAT.get_Free_Size()} bytes free");
                    Dir(name);
                }
                else
                {
                    Console.WriteLine($"Error this path \"{name}\" is not exists ! ");
                    return;
                }


            }

            int index = Program.currentDirectory.search_Directory(name);
            if(index == -1) 
            {
                Console.WriteLine($"Error : Directory \"{name}\" not exists !");
                return;
            }
            else
            {
                Dir(name);
            }
        }//dir

        public static void Rename(string _Old,string _New) // rename
        {
            int index_old = Program.currentDirectory.search_Directory(_Old);
            int index_new = Program.currentDirectory.search_Directory(_New);

            if (index_old != -1) 
            {
                if(index_new == -1) 
                {
                    Directory_Entry e = Program.currentDirectory.DirectoryTable[index_old];

                    e.Dir_Namee = _New.ToCharArray();
                    Program.currentDirectory.Write_Directory();
                }
                else
                {
                    Console.WriteLine($"Error : {_New} already exists !");
                    return;
                }
               
            }
            else
            {
                Console.WriteLine($"Error this Filename \"{_Old}\" not exists !");
                return;
            }
        }



    }
}

