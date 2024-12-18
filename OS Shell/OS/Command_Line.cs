using System;
using System.Collections.Generic;
using System.Linq;
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
                Console.WriteLine("Error : Please Enter Right Command Enter \"help\" to show Our Command .");
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
            else if (command_Array[0].ToLower() == "dir") // for directory without path
            {
                int file_Counter = 0;
                int folder_Counter = 0;
                int file_Sizes = 0;

                string name = new string(Program.currentDirectory.Dir_Namee);
                Console.WriteLine($"Directory of {name} : \n");
                for (int i = 0; i < Program.currentDirectory.DirectoryTable.Count; i++)
                {
                    if (Program.currentDirectory.DirectoryTable[i].dir_Attr == 1)
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
       
            #region mdv02
            //else if (commandArray_2Agr[0].ToLower() == "md")
            //{
            //    if (commandArray_2Agr.Length < 2 || string.IsNullOrWhiteSpace(commandArray_2Agr[1]))
            //    {
            //        //Console.WriteLine("Error: You must specify a directory name. Syntax: md [directory_name]");
            //        Console.WriteLine("Error : md command syntax is \n md [directory] \n[directory] can be a new directory name or fullpath of a new directory\nCreates a directory.");

            //        return;
            //    }

            //    string dirName = commandArray_2Agr[1].Trim();
            //    if (dirName == ".")
            //    {
            //        Console.WriteLine("Error : md command syntax is \n md [directory] \n[directory] can be a new directory name or fullpath of a new directory\nCreates a directory.");
            //        // Console.WriteLine("Error: Invalid directory name. A directory cannot be named '.'");
            //        return;
            //    }

            //    Directory o = new Directory(dirName, 0x10, 0, Program.currentDirectory);
            //    string s = new string(o.Dir_Namee.Where(c => c != '\0').ToArray()).Trim();

            //    if (Program.currentDirectory.search_Directory(s) == -1)
            //    {
            //        if (Program.currentDirectory.Can_Add_Entry(o))
            //        {
            //            Program.currentDirectory.add_Entry(o);
            //            Program.currentDirectory.Write_Directory();
            //            if (Program.currentDirectory.Parent != null)
            //            {
            //                Program.currentDirectory.Parent.Write_Directory();
            //            }
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine("Folder Exists");
            //    }
            //} 
            #endregion

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
                string name = commandArray_2Agr[1].Trim(); // Ensure no trailing spaces
                int index = Program.currentDirectory.search_Directory(name);

                if (index == -1)
                {
                    Console.WriteLine($"Error: Directory '{name}' not found.");
                }
                else
                {
                    // Get the directory entry to delete
                    Directory_Entry entry = Program.currentDirectory.DirectoryTable[index];

                    if (entry.dir_Attr != 0x10) // Ensure it's a directory
                    {
                        Console.WriteLine($"Error: '{name}' is not a directory.");
                        return;
                    }

                    // Load the directory to delete
                    int firstCluster = entry.dir_First_Cluster;
                    Directory dirToDelete = new Directory(name, entry.dir_Attr, firstCluster, Program.currentDirectory);

                    // Ensure directory is empty before deletion
                    dirToDelete.Read_Directory();
                    if (dirToDelete.DirectoryTable.Count > 0)
                    {
                        Console.WriteLine($"Error: Directory '{name}' is not empty.");
                    }
                    else
                    {
                        // Delete the directory from FAT and remove the entry
                        dirToDelete.delete_Directory();
                        Program.currentDirectory.DirectoryTable.RemoveAt(index); // Remove from parent's table
                        Program.currentDirectory.Write_Directory(); // Save changes to disk
                        Console.WriteLine($"Directory '{name}' deleted successfully.");
                    }
                }
            }


            #region cdv01

            //else if (commandArray_2Agr[0].ToLower() == "cd")
            //{
            //    string k = new string(Program.Root.Dir_Namee);
            //    string l = new string(commandArray_2Agr[1]);
            //    var x = commandArray_2Agr[1].Length;
            //    if (x < 11)
            //    {
            //        for (int i = x; i < 11; i++)
            //        {
            //            l += " ";

            //        }
            //    }
            //    Directory temp = Program.Root;
            //    if (commandArray_2Agr[1] == "..")
            //    {
            //        Program.currentDirectory = temp;
            //        string back = new string(Program.currentDirectory.Dir_Namee);
            //        Program.path = back;
            //    }
            //    int index = Program.currentDirectory.search_Directory(commandArray_2Agr[1]);
            //    if (index != -1)
            //    {
            //        byte Attribute = Program.currentDirectory.DirectoryTable[index].dir_Attr;
            //        if (Attribute == 0x10)
            //        {
            //            int FirstCluster = Program.currentDirectory.DirectoryTable[index].dir_First_Cluster;

            //            Directory d = new Directory(commandArray_2Agr[1].ToLower(), 0x10, FirstCluster, Program.currentDirectory);
            //            Program.currentDirectory = d;
            //            Program.path += "\\" + commandArray_2Agr[1].ToString();
            //            Program.currentDirectory.Read_Directory();
            //        }
            //        else
            //        {
            //            Console.WriteLine("Specified Folder isn't exist , it's File.");
            //        }
            //    }
            //    else if (l.Length != k.Length)
            //    {
            //        Console.WriteLine("Folder Name isn't Exist");
            //    }
            //}
            #endregion

            #region cdv05
            //else if (commandArray_2Agr[0].ToLower() == "cd")
            //{
            //    string directoryName = commandArray_2Agr[1];

            //    if (directoryName == "..")
            //    {
            //        // If we're at the root directory, reset the path to "N"
            //        if (Program.currentDirectory.Parent != null)
            //        {
            //            Program.currentDirectory = Program.currentDirectory.Parent; // Move to the parent directory
            //            Program.path = new string (Program.Root.Dir_Namee) + @"\"+new string(Program.currentDirectory.Dir_Namee); // Update the path correctly
            //            Console.WriteLine($"Moved to parent directory: {new string(Program.currentDirectory.Dir_Namee)}");
            //        }
            //        else
            //        {
            //            Program.path = new string(Program.Root.Dir_Namee); // Reset the path to root
            //            Console.WriteLine($"Moved to parent directory: {new string(Program.Root.Dir_Namee)}");
            //        }
            //    }
            //    else
            //    {
            //        // Search for the directory in the current directory table
            //        int index = Program.currentDirectory.search_Directory(directoryName);
            //        if (index != -1)
            //        {
            //            byte Attribute = Program.currentDirectory.DirectoryTable[index].dir_Attr;
            //            if (Attribute == 0x10) // Check if it's a directory
            //            {
            //                int FirstCluster = Program.currentDirectory.DirectoryTable[index].dir_First_Cluster;

            //                Directory d = new Directory(directoryName, 0x10, FirstCluster, Program.currentDirectory); // Parent passed here
            //                Program.currentDirectory = d;

            //                // Update the path to include the current directory
            //                if (Program.path == "N")
            //                {
            //                    Program.path = "N\\" + directoryName; // If at root, just set the path to N\directory
            //                }
            //                else
            //                {
            //                    Program.path += "\\" + directoryName; // Append to the path for subdirectories
            //                }

            //                Program.currentDirectory.Read_Directory();
            //                Console.WriteLine($"Moved to directory: {directoryName}");
            //            }
            //            else
            //            {
            //                Console.WriteLine("Error: Specified folder isn't a directory, it's a file.");
            //            }
            //        }
            //        else
            //        {
            //            Console.WriteLine("Error: Directory does not exist.");
            //        }
            //    }
            //} 
            #endregion
           
            else if (commandArray_2Agr[0].ToLower() == "cd")
            {
                string directoryName = commandArray_2Agr[1];

                
                if (directoryName == "..")
                {
                    
                    if (Program.currentDirectory.Parent != null)
                    {
                        Program.currentDirectory = Program.currentDirectory.Parent;  

                        // Update the path correctly
                        if (Program.currentDirectory == Program.Root)
                        {
                            Program.path = new string(Program.Root.Dir_Namee);  // Set path to root if we're at the root directory
                        }
                        else
                        {
                            // Remove the last directory from the path
                            int lastBackslashIndex = Program.path.LastIndexOf(@"\");

                            if (lastBackslashIndex >= 0)
                            {
                                Program.path = Program.path.Substring(0, lastBackslashIndex);  // Update the path to the parent directory
                            }
                        }

                        Console.WriteLine($"Moved to parent directory: {new string(Program.currentDirectory.Dir_Namee)}");
                    }
                    else
                    {
                        // Already at the root directory, no move possible
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
