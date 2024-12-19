using System;
using System.Collections.Generic;
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
                        Console.WriteLine("\t\t<File> " + m);
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
            else if (commandArray_2Agr[0].ToLower() == "md")
            {
                if (commandArray_2Agr.Length < 2 || string.IsNullOrWhiteSpace(commandArray_2Agr[1]) || commandArray_2Agr[1] == ".")
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

                Directory newDir = new Directory(dirName, 0x10, 0, Program.currentDirectory);
                if (Program.currentDirectory.Can_Add_Entry(newDir))
                {
                    Program.currentDirectory.add_Entry(newDir);  
                    

                    Console.WriteLine($"Directory '{dirName}' created successfully.");
                    
                }
                else
                {
                    Console.WriteLine("Error: Could not create the directory.");
                }
            }

            
            else if (commandArray_2Agr[0].ToLower() == "rd")
            {
                string name = commandArray_2Agr[1].Trim();
                int index = Program.currentDirectory.search_Directory(name);

                if (index == -1)
                {
                    Console.WriteLine($"Error: Directory '{name}' not found.");
                }
                else
                {
                    Directory_Entry entry = Program.currentDirectory.DirectoryTable[index];

                    if (entry.dir_Attr != 0x10)
                    {
                        Console.WriteLine($"Error: '{name}' is not a directory.");
                        return;
                    }

                    int firstCluster = entry.dir_First_Cluster;
                    Directory dirToDelete = new Directory(name, entry.dir_Attr, firstCluster, Program.currentDirectory);

                    dirToDelete.Read_Directory();
                    if (dirToDelete.DirectoryTable.Count > 0)
                    {
                        Console.WriteLine($"Error: Directory '{name}' is not empty.");
                    }
                    else
                    {
                        dirToDelete.delete_Directory();
                        Program.currentDirectory.DirectoryTable.RemoveAt(index); 
                        Program.currentDirectory.Write_Directory(); 
                        Console.WriteLine($"Directory '{name}' deleted successfully.");
                    }
                }
            }
            #region cd command
            else if (commandArray_2Agr[0].ToLower() == "cd")
            {
                string directoryName = commandArray_2Agr[1];


                if (directoryName == "..")
                {

                    if (Program.currentDirectory.Parent != null)
                    {
                        Program.currentDirectory = Program.currentDirectory.Parent;

                        if (Program.currentDirectory == Program.Root)
                        {
                            Program.path = new string(Program.Root.Dir_Namee);
                        }
                        else
                        {
                            int lastBackslashIndex = Program.path.LastIndexOf(@"\");

                            if (lastBackslashIndex >= 0)
                            {
                                Program.path = Program.path.Substring(0, lastBackslashIndex);
                            }
                        }

                        Console.WriteLine($"Moved to parent directory: {new string(Program.currentDirectory.Dir_Namee)}");
                    }
                    else
                    {
                        Console.WriteLine("Error: Already at the root directory.");
                    }
                }
                else
                {

                    int index = Program.currentDirectory.search_Directory(directoryName);
                    if (index != -1)
                    {
                        byte Attribute = Program.currentDirectory.DirectoryTable[index].dir_Attr;
                        if (Attribute == 0x10)
                        {
                            int FirstCluster = Program.currentDirectory.DirectoryTable[index].dir_First_Cluster;

                            Directory d = new Directory(directoryName, 0x10, FirstCluster, Program.currentDirectory);
                            Program.currentDirectory = d;
                            Program.path += @"\" + directoryName;

                            Program.currentDirectory.Read_Directory();
                            Console.WriteLine($"Moved to directory: {directoryName}");
                        }
                        else
                        {
                            Console.WriteLine("Error: Specified folder isn't a directory, it's a file.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Directory does not exist.");
                    }
                }
            }
            #endregion

            #region import
            //else if (commandArray_2Agr[0].ToLower() == "import")
            //{
            //    string filePath = commandArray_2Agr[1];

            //    // Handle paths enclosed in quotes (remove quotes)
            //    if (filePath.StartsWith("\"") && filePath.EndsWith("\""))
            //    {
            //        filePath = filePath.Substring(1, filePath.Length - 2);
            //    }

            //    try
            //    {
            //        if (!File.Exists(filePath))
            //        {
            //            Console.WriteLine("The file does not exist.");
            //            return;
            //        }


            //        string fileName = Path.GetFileName(filePath); // Use Path.GetFileName for reliability


            //        string fileContent = File.ReadAllText(filePath);
            //        int fileSize = fileContent.Length;



            //        int indexFile = Program.currentDirectory.search_Directory(fileName);
            //        if (indexFile != -1)
            //        {
            //            Console.WriteLine("A file or directory with that name already exists.");
            //            return;
            //        }



            //        File_Entry newFile = new File_Entry(fileName.ToCharArray(), 0x0,0, Program.currentDirectory, fileContent);
            //        newFile.Write_File_Content();//most important line no remove or delete : the file content is now saved
            //        Program.currentDirectory.add_Entry(newFile); // Add the new file entry to the current directory: directory updated


            //    }
            //    catch (Exception ex)
            //    {

            //        Console.WriteLine($"Error importing file: {ex.Message}"); // Informative error message
            //    }
            //} 
            #endregion
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
                        File_Entry f = new File_Entry(fileName.ToCharArray(), 0x0, fc, Program.currentDirectory, fileContent);
                        f.Write_File_Content();
                        Directory_Entry d = new Directory_Entry(fileName, 0, f.dir_First_Cluster);
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
