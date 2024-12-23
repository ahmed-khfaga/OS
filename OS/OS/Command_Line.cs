using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace OS
{
    class Command_Line
    {
        public string[] comm_Arg;


        #region movetoDir me
        //public static Directory? MoveToDir(string fullPath)
        //{
        //    // Split the path into its parts
        //    string[] pathParts = fullPath.Split("\\");

        //    // First, check if the root directory matches
        //    string rootName = new string(Mini_FAT.Root.Dir_Namee).PadRight(11, '\0');
        //    if (rootName.Contains("\0"))
        //    {
        //        rootName = rootName.Replace('\0', ' ');
        //    }

        //    if (pathParts[0].Trim() != rootName.Trim())
        //    {
        //        Console.WriteLine($"Error: The root in the specified path \"{fullPath}\" does not exist.");
        //        return null;
        //    }

        //    // Start from the root directory
        //    Directory currentDir = Mini_FAT.Root;
        //    currentDir.Read_Directory();

        //    for (int i = 1; i < pathParts.Length; i++)
        //    {
        //        string dirName = pathParts[i].Trim();
        //        int index = currentDir.search_Directory(dirName);

        //        if (index == -1)
        //        {
        //            Console.WriteLine($"Error: The path \"{fullPath}\" does not exist.");
        //            return null;
        //        }

        //        Directory_Entry entry = currentDir.DirectoryTable[index];
        //        if (entry.dir_Attr != 0x10) 
        //        {
        //            Console.WriteLine($"Error: The path \"{fullPath}\" is not a directory.");
        //            return null;
        //        }

        //        int firstCluster = entry.dir_First_Cluster;
        //        Directory nextDir = new Directory(dirName.ToCharArray(), 0x10, firstCluster, currentDir);
        //        nextDir.Read_Directory();

        //        // Move to the next directory
        //        currentDir = nextDir;
        //    }

        //    Program.currentDirectory = currentDir;
        //    Program.path = fullPath;

        //    return currentDir;
        //} 
        #endregion

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
            if (parts[0].Equals (s.Trim('\0'), StringComparison.OrdinalIgnoreCase))
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
                    Console.WriteLine($"Error: Directory '{part}' not found.");
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
        public Command_Line(string command)
        {
            comm_Arg = command.Split(" ");
            if (comm_Arg.Length == 1)
            {
                Command(comm_Arg);
            }
            else if (comm_Arg.Length > 1)
            {
                Commmand2Arg(comm_Arg);
            }
        }

        static void Command(string[] command_Array)
        {
            if (command_Array[0].ToLower().Trim() == "quit")
            {
                Program.currentDirectory.Write_Directory();
                Environment.Exit(0);
            }
            else if (command_Array[0].ToLower() == "cls")
            {
                Console.Clear();
            }
            else if (command_Array[0].ToLower() == "cd")
            {
                Console.WriteLine(Program.currentDirectory.Dir_Namee);
            }
            else if ( string.IsNullOrWhiteSpace(command_Array[0].ToLower()))
            {
                return;
            }
            else if (command_Array[0].ToLower() == "help")
            {
                Console.WriteLine("cd\t\t- Change the current default directory to .\n\t\tIf the argument is not present, report the current directory.\n\t\tIf the directory does not exist an appropriate error should be reported.");
                Console.WriteLine("cls\t\t- Clear the screen.");
                Console.WriteLine("dir\t\t- List the contents of directory.");
                Console.WriteLine("quit\t\t- Quit the shell.");
                Console.WriteLine("copy\t\t- Copies one or more files to another location.");
                Console.WriteLine("del\t\t- Deletes one or more files.");

                Console.WriteLine("help\t\t- Provides Help information for commands.");
                Console.WriteLine("md\t\t- Creates a directory.");
                Console.WriteLine("rd\t\t- Removes a directory.");
                Console.WriteLine("rename\t\t- Renames a file.");
                Console.WriteLine("type\t\t- Displays the contents of a text file.");
                Console.WriteLine("import\t\t- import text file(s) from your computer.");
                Console.WriteLine("export\t\t- export text file(s) to your computer.");
            }
            else if (command_Array[0].ToLower() == "md")
            {
                Console.WriteLine("Error : md command syntax is \n md [directory] \n[directory] can be a new directory name or fullpath of a new directory\nCreates a directory.");
            }
            #region dir
            else if (command_Array[0].ToLower() == "dir") 
            {

                int file_Counter = 0;
                int folder_Counter = 0;
                int file_Sizes = 0;

                string name = new string(Program.currentDirectory.Dir_Namee);
                Console.WriteLine($"Directory of {name} : \n");
                for (int i = 0; i < Program.currentDirectory.DirectoryTable.Count; i++)
                {
                    if (Program.currentDirectory.DirectoryTable[i].dir_Attr == 0x0)
                    {
                        file_Counter++;
                        file_Sizes += Program.currentDirectory.DirectoryTable[i].dir_FileSize;
                        string m = string.Empty;
                        m += new string(Program.currentDirectory.DirectoryTable[i].Dir_Namee);
                        Console.WriteLine("\t\t<File>\t\t" + m);
                        Console.WriteLine();
                    }
                    else if (Program.currentDirectory.DirectoryTable[i].dir_Attr == 0x10) // لو في فولدر 
                    {
                        folder_Counter++;
                        string S = new string(Program.currentDirectory.DirectoryTable[i].Dir_Namee);
                        Console.WriteLine("\t\t<DIR>\t\t" + S.Trim());
                    }

                }
                Console.Write($"\t\t\t{file_Counter} File(s)\t ");
                if (file_Counter > 0)
                {
                    Console.Write(file_Sizes);
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine();
                }
                Console.WriteLine($"\t\t\t{folder_Counter} Dir(s)\t {Mini_FAT.get_Free_Size()} bytes free");

            }
            #endregion           
            else
            {
                Console.WriteLine(command_Array[0] + " is not a valid command");
                Console.WriteLine("please valid Command");
            }
        }
        static void Commmand2Arg(string[] commandArray_2Agr)
        {
            if (commandArray_2Agr[0].ToLower() == "help" && commandArray_2Agr[1].ToLower() == "cd")
            {
                Console.WriteLine("cd\t\t - Change the current default directory to .\n\t\tIf the argument is not present, report the current directory.\n\t\tIf the directory does not exist an appropriate error should be reported.");
            }
            else if (commandArray_2Agr[0].ToLower() == "help" && commandArray_2Agr[1].ToLower() == "cls")
            {
                Console.WriteLine("cls\t\t- Clear the screen.");

            }
            else if (commandArray_2Agr[0].ToLower() == "help" && commandArray_2Agr[1].ToLower() == "dir")
            {
                Console.WriteLine("dir\t\t- List the contents of directory.");

            }
            else if (commandArray_2Agr[0].ToLower() == "help" && commandArray_2Agr[1].ToLower() == "quit")
            {
                Console.WriteLine("quit\t\t- Quit the shell.");

            }
            else if (commandArray_2Agr[0].ToLower() == "help" && commandArray_2Agr[1].ToLower() == "copy")
            {
                Console.WriteLine("copy\t\t- Copies one or more files to another location.");

            }
            else if (commandArray_2Agr[0].ToLower() == "help" && commandArray_2Agr[1].ToLower() == "del")
            {
                Console.WriteLine("del\t\t- Deletes one or more files.");

            }
            else if (commandArray_2Agr[0].ToLower() == "help" && commandArray_2Agr[1].ToLower() == "help")
            {
                Console.WriteLine("help\t\t- Provides Help information for commands.");

            }
            else if (commandArray_2Agr[0].ToLower() == "help" && commandArray_2Agr[1].ToLower() == "md")
            {
                Console.WriteLine("md\t\t- Creates a directory.");

            }
            else if (commandArray_2Agr[0].ToLower() == "help" && commandArray_2Agr[1].ToLower() == "rd")
            {
                Console.WriteLine("rd\t\t- Removes a directory.");

            }
            else if (commandArray_2Agr[0].ToLower() == "help" && commandArray_2Agr[1].ToLower() == "rename")
            {
                Console.WriteLine("rename\t\t- Renames a file.");
            }
            else if (commandArray_2Agr[0].ToLower() == "help" && commandArray_2Agr[1].ToLower() == "type")
            {
                Console.WriteLine("type\t\t- Displays the contents of a text file.");

            }
            else if (commandArray_2Agr[0].ToLower() == "help" && commandArray_2Agr[1].ToLower() == "import")
            {
                Console.WriteLine("import\t\t- import text file(s) from your computer.");

            }
            else if (commandArray_2Agr[0].ToLower() == "help" && commandArray_2Agr[1].ToLower() == "export")
            {
                Console.WriteLine("export\t\t- export text file(s) to your computer.");

            }
            else if (commandArray_2Agr[0].ToLower() == "cls")
            {
                Console.WriteLine("Error : cls command syntax is \n cls \n function: Clear the screen");
            }
            else if (commandArray_2Agr[0].ToLower() == "quit")
            {
                Console.WriteLine("Error : quit command syntax is \n quit \n function: Quit the shell");
            }

            // dir fullpath

            else if (commandArray_2Agr[0].ToLower() == "dir")
            {
                
                string dname = commandArray_2Agr[1];
                if(dname.Contains("\\"))
                {
                    MoveToDir(dname, Program.currentDirectory);
                    int file_Counter = 0;
                    int folder_Counter = 0;
                    int file_Sizes = 0;

                    string name = new string(Program.currentDirectory.Dir_Namee);
                    Console.WriteLine($"Directory of {name} : \n");
                    for (int i = 0; i < Program.currentDirectory.DirectoryTable.Count; i++)
                    {
                        if (Program.currentDirectory.DirectoryTable[i].dir_Attr == 0x0)
                        {
                            file_Counter++;
                            file_Sizes += Program.currentDirectory.DirectoryTable[i].dir_FileSize;
                            string m = string.Empty;
                            m += new string(Program.currentDirectory.DirectoryTable[i].Dir_Namee);
                            Console.WriteLine("\t\t<File>\t\t" + m);
                            Console.WriteLine();
                        }
                        else if (Program.currentDirectory.DirectoryTable[i].dir_Attr == 0x10) // لو في فولدر 
                        {
                            folder_Counter++;
                            string S = new string(Program.currentDirectory.DirectoryTable[i].Dir_Namee);
                            Console.WriteLine("\t\t<DIR>\t\t" + S);
                        }

                    }
                    Console.Write($"\t\t\t{file_Counter} File(s)\t ");
                    if (file_Counter > 0)
                    {
                        Console.Write(file_Sizes);
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine();
                    }
                    Console.WriteLine($"\t\t\t{folder_Counter} Dir(s)\t {Mini_FAT.get_Free_Size()} bytes free");

                }
            }
            
            // work
            else if (commandArray_2Agr[0].ToLower() == "md")
            {
                if (commandArray_2Agr.Length < 2 || string.IsNullOrWhiteSpace(commandArray_2Agr[1]) || commandArray_2Agr[1] == "." || commandArray_2Agr[1].ToLower().Contains("."))
                {
                    Console.WriteLine("Error : md command syntax is \n md [directory] \n[directory] can be a new directory name or fullpath of a new directory\nCreates a directory.");
                    return;
                }

                string dirName = commandArray_2Agr[1];

                if (Program.currentDirectory.search_Directory(dirName) != -1)
                {
                    Console.WriteLine("Folder already exists.");
                    return;
                }
                //int fc = Mini_FAT.get_Availabel_Cluster();
                Directory newDir = new Directory(dirName.ToCharArray(), 0x10,0, Program.currentDirectory);
                if (Program.currentDirectory.Can_Add_Entry(newDir))
                {
                    Program.currentDirectory.add_Entry(newDir);
                    Program.currentDirectory.Write_Directory();
                    if(Program.currentDirectory.Parent != null)
                    {
                        Program.currentDirectory.Parent.Write_Directory();
                        Program.currentDirectory.Update_Content(Program.currentDirectory.Get_Directory_Entry(), Program.currentDirectory.Parent.Get_Directory_Entry());    
                    }

                    Console.WriteLine($"Directory '{dirName}' created successfully.");
                    
                }
                else
                {
                    Console.WriteLine("Error: Could not create the directory.");
                }
            }

            // work
            else if (commandArray_2Agr[0].ToLower() == "rd")
            {
                if (commandArray_2Agr.Length < 2)
                {
                    Console.WriteLine("Error: No directories specified to remove.");
                }
                else
                {
                    string[] directoriesToDelete = commandArray_2Agr.Skip(1).ToArray();
                    ParserClass.RemoveDirectory(directoriesToDelete);
                }
            }

            //work xd 
            #region cd command
            //else if (commandArray_2Agr[0].ToLower() == "cd")
            //{
            //    string dname = commandArray_2Agr[1];

            //    if(dname == ".")
            //    {
            //        return;
            //    }
            //    // if cd N:\f 
            //    if(dname.Contains("\\"))
            //    {

            //        MoveToDir(dname,Program.currentDirectory);
            //        return;


            //    }
            //    if (dname == "..")
            //    {
            //        if (Program.currentDirectory.Parent != null)
            //        {
            //            Program.currentDirectory = Program.currentDirectory.Parent;
            //            // Correctly update Program.Path
            //            int lastBackslash = Program.path.LastIndexOf('\\');
            //            if (lastBackslash > 0) // Avoid going beyond the root
            //            {
            //                Program.path = Program.path.Substring(0, lastBackslash);
            //                Program.currentDirectory.Read_Directory();

            //            }                      
            //            return; // Important: Exit after handling ".."
            //        }
            //        else
            //        {
            //            Console.WriteLine("Already at the root directory."); // Inform the user
            //            return;
            //        }

            //    }
            //    else
            //    {
            //        int index = Program.currentDirectory.search_Directory(dname);
            //        if (index == -1)
            //        {
            //            Console.WriteLine("Error : this Directory does not exits ");
            //        }
            //        else
            //        {

            //            Directory_Entry entry = Program.currentDirectory.DirectoryTable[index];

            //            Directory newDir = new Directory(dname.ToCharArray(), 0x10, entry.dir_First_Cluster, Program.currentDirectory);
            //            Program.currentDirectory = newDir;
            //            Program.path += "\\" + dname;
            //            Program.currentDirectory.Read_Directory();

            //        }
            //    }
            //}                       
            #endregion
            else if (commandArray_2Agr[0].ToLower() == "cd")
            {
                ParserClass.ChangeDirectory(commandArray_2Agr[1]);
            }
        
            // work
            else if (commandArray_2Agr[0].ToLower() == "rename")
            {
                int indexOld = Program.currentDirectory.search_Directory(commandArray_2Agr[1].ToString());
                if (indexOld == -1)
                {
                    Console.WriteLine("The File is not Exist");
                }
                else
                {
                    int indexNew = Program.currentDirectory.search_Directory(commandArray_2Agr[2].ToString());
                    if (indexNew != -1)
                    {
                        Console.WriteLine("can't Rename");
                    }
                    else
                    {
                        Directory_Entry o = Program.currentDirectory.DirectoryTable[indexOld]; 
                        int firstClusterOld = Program.currentDirectory.DirectoryTable[indexOld].dir_First_Cluster;
                        int FileSize = Program.currentDirectory.DirectoryTable[indexOld].dir_FileSize;
                        if (Program.currentDirectory.DirectoryTable[indexOld].dir_Attr == 0x0)
                        {
                            File_Entry f = new File_Entry(commandArray_2Agr[1].ToCharArray(), 0, firstClusterOld, FileSize, Program.currentDirectory, "");
                            f.Dir_Namee = commandArray_2Agr[2].ToCharArray();
                            f.Write_File_Content();
                            f.Read_File_Content();
                            Program.currentDirectory.Write_Directory();
                        }
                        else
                        {

                            Directory_Entry d = new Directory_Entry(commandArray_2Agr[2].ToCharArray(), 0x10, firstClusterOld);
                            Program.currentDirectory.DirectoryTable.RemoveAt(indexOld);
                            Program.currentDirectory.DirectoryTable.Insert(indexOld, d);
                            // Program.currentDirectory.Update_Content(o, d);
                            Program.currentDirectory.Write_Directory();
                        }

                    }
                }
            }
            
            // work 
            else if (commandArray_2Agr[0].ToLower() == "import")
            {
                string name = commandArray_2Agr[1];
                if (System.IO.File.Exists(name))
                {
                    Directory_Entry o = Program.currentDirectory.Get_Directory_Entry();

                    string[] pathParts = name.Split('\\'); // Split the path(extract name & size)
                    string fileName = pathParts[pathParts.Length - 1];
                    string fileContent = System.IO.File.ReadAllText(name);
                    int size = fileContent.Length;
                    int index = Program.currentDirectory.search_Directory(fileName);
                    int fc = 0;

                    if (index == -1)
                    {
                        File_Entry f = new File_Entry(fileName.ToCharArray(), 0x0, fc, size,Program.currentDirectory, fileContent);
                        f.Write_File_Content();
                        Directory_Entry d = new Directory_Entry(fileName.ToCharArray(), 0x0, f.dir_First_Cluster);
                        Program.currentDirectory.DirectoryTable.Add(d);
                        Program.currentDirectory.Write_Directory();

                        if (Program.currentDirectory.Parent != null)
                        {
                            Program.currentDirectory.Parent.Update_Content(o, Program.currentDirectory.Get_Directory_Entry());
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: File with the same name already exists.");
                    }
                }
                else
                {
                    Console.WriteLine("Error: The specified name is not a file.");
                }
            }
            
            // work
            else if (commandArray_2Agr[0].ToLower() == "export")
            {
                int index = Program.currentDirectory.search_Directory(commandArray_2Agr[1].ToString());
                if (index == -1)
                {
                    Console.WriteLine("The File is not Exist");
                }
                else
                {
                    if (!System.IO.Directory.Exists(commandArray_2Agr[2].ToString()))
                    {
                        Console.WriteLine("The System Canot find the folder Destination in your computer");
                    }
                    else
                    {
                        if (Program.currentDirectory.DirectoryTable[index].dir_Attr == 0x0)
                        {
                            int FirstCluster = Program.currentDirectory.DirectoryTable[index].dir_First_Cluster;
                            int fileSize = Program.currentDirectory.DirectoryTable[index].dir_FileSize;
                            string temp = "";
                            File_Entry f = new File_Entry(commandArray_2Agr[1].ToCharArray(), 0, FirstCluster, fileSize, Program.currentDirectory, temp);
                            f.Write_File_Content();
                            f.Read_File_Content();
                            StreamWriter StreamWriter = new StreamWriter(commandArray_2Agr[2].ToString() + "\\" + commandArray_2Agr[1].ToString());
                            StreamWriter.Write(f.content);
                            StreamWriter.Flush();
                            StreamWriter.Close();
                        }
                        else if (Program.currentDirectory.DirectoryTable[index].dir_Attr == 0x10)
                        {
                            int FirstCluster = Program.currentDirectory.DirectoryTable[index].dir_First_Cluster;
                            int fileSize = Program.currentDirectory.DirectoryTable[index].dir_FileSize;
                            Directory f = new Directory(commandArray_2Agr[1].ToCharArray(), 1, FirstCluster,Program.currentDirectory);
                            f.Write_Directory();
                            f.Read_Directory();
                            StreamWriter StreamWriter = new StreamWriter(commandArray_2Agr[2].ToString() + "\\" + commandArray_2Agr[1].ToString());
                            StreamWriter.Write(f);
                            StreamWriter.Flush();
                            StreamWriter.Close();
                        }
                    }
                }
            }

            //del not work
            else if (commandArray_2Agr[0].ToLower() == "del") // for only files
            {
                int index = Program.currentDirectory.search_Directory(commandArray_2Agr[1].ToString());
                if (index == -1)
                {
                    Console.WriteLine("The File is not Exist");
                }
                else
                {
                    if (Program.currentDirectory.DirectoryTable[index].dir_Attr == 0x10)
                    {
                        Console.WriteLine("this is a folder");
                    }
                    else
                    {
                        int f_cluster = Program.currentDirectory.DirectoryTable[index].dir_First_Cluster;
                        int file_size = Program.currentDirectory.DirectoryTable[index].dir_FileSize;
                        File_Entry f = new File_Entry(commandArray_2Agr[1].ToCharArray(), 0, f_cluster, file_size, Program.currentDirectory, "");
                        f.Delete_File(commandArray_2Agr[1].ToString());
                        Program.currentDirectory.Write_Directory();
                        Program.currentDirectory.Read_Directory();
                    }
                }
            }

            // work
            else if (commandArray_2Agr[0].ToLower() == "type")//display the file content
            {
                int index = Program.currentDirectory.search_Directory(commandArray_2Agr[1].ToString());
                if (index != -1)
                {
                    int firstcluster = Program.currentDirectory.DirectoryTable[index].dir_First_Cluster;
                    int filesize = Program.currentDirectory.DirectoryTable[index].dir_FileSize;
                    string content = "";
                    File_Entry FE = new File_Entry(commandArray_2Agr[1].ToCharArray(), 0, firstcluster, filesize, Program.currentDirectory, content);
                    FE.Read_File_Content();
                    Console.WriteLine(FE.content);
                }
                else
                {
                    Console.WriteLine("The system cannot find the file specified.");
                }
            }
            
            // work 
            else if (commandArray_2Agr[0].ToLower() == "copy")// for only files
            {
                int indexSource = Program.currentDirectory.search_Directory(commandArray_2Agr[1].ToString());
                if (indexSource == -1)  // السورس مش موجود
                {
                    Console.WriteLine("The File is not Exist");
                }
                else // لقى السورس
                {
                    string fileName = "";

                    fileName = commandArray_2Agr[2].ToString();
                    int destination_index = Program.currentDirectory.search_Directory(fileName);
                    if (destination_index != -1)
                    {
                        if (Program.currentDirectory.Dir_Namee == commandArray_2Agr[2].ToCharArray())
                        {
                            Console.WriteLine(" The main destination and the new one are the same.. please enter another destination");
                        }
                        else
                        {
                            int F_Cluster = Program.currentDirectory.DirectoryTable[destination_index].dir_First_Cluster;
                            Directory d = new Directory(commandArray_2Agr[2].ToCharArray(), 1, F_Cluster, Program.currentDirectory);
                            int f_cluster = Program.currentDirectory.DirectoryTable[indexSource].dir_First_Cluster;
                            int file_size = Program.currentDirectory.DirectoryTable[indexSource].dir_FileSize;
                            Program.currentDirectory = d;

                            Program.path += "\\" + commandArray_2Agr[2].ToString();
                            File_Entry f = new File_Entry(commandArray_2Agr[1].ToCharArray(), 0, f_cluster, file_size, Program.currentDirectory, "");
                            Program.currentDirectory.DirectoryTable.Add(f);
                            Program.currentDirectory.Write_Directory();
                            Program.currentDirectory.Read_Directory();
                        }
                    }
                    else
                    {
                        int F_Cluster = Mini_FAT.get_Availabel_Cluster();
                        Directory d = new Directory(commandArray_2Agr[2].ToCharArray(), 0, F_Cluster, Program.currentDirectory);
                        Program.currentDirectory.DirectoryTable.Add(d);

                        int f_cluster = Program.currentDirectory.DirectoryTable[indexSource].dir_First_Cluster;
                        int file_size = Program.currentDirectory.DirectoryTable[indexSource].dir_FileSize;
                        Program.currentDirectory = d;

                        Program.path += "\\" + commandArray_2Agr[2].ToString();
                        File_Entry f = new File_Entry(commandArray_2Agr[1].ToCharArray(), 0, f_cluster, file_size, Program.currentDirectory, "");
                        Program.currentDirectory.DirectoryTable.Add(f);
                        Program.currentDirectory.Write_Directory();
                        Program.currentDirectory.Read_Directory();

                    }
                }
            }

            else if (
                commandArray_2Agr[0].ToLower() == "help" && commandArray_2Agr[1].ToLower() != "cd" && commandArray_2Agr[1].ToLower() != "cls" && commandArray_2Agr[1].ToLower() != "quit" && commandArray_2Agr[1].ToLower() != "copy" && commandArray_2Agr[1].ToLower() != "del"
                && commandArray_2Agr[1].ToLower() !="help" && commandArray_2Agr[1].ToLower() != "md" && commandArray_2Agr[1].ToLower() !="rd" && commandArray_2Agr[1].ToLower() !="rename" && commandArray_2Agr[1].ToLower() !="type" 
                && commandArray_2Agr[1].ToLower() !="import" && commandArray_2Agr[1].ToLower() !="export"
                )
                
            {
                Console.WriteLine(commandArray_2Agr[1].ToLower() + " is not a valid command.");
                Console.WriteLine("please valid Command ");
            }
          
            else
            {
                Console.WriteLine(commandArray_2Agr[0] + " is not a valid command.");
                Console.WriteLine("please valid Command ");
            }
        }
    }
}
