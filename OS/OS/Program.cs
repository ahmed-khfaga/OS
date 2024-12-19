using System.IO;
using System;

namespace OS
{
    class Program
    {
        public static Directory currentDirectory;
        public static string path = "disk";

        static void Main(string[] args)
        {
           
            Console.WriteLine("Welcome to OS_Project_Virtual_DISK_shell ^_^ ");
            Console.WriteLine("developed by AHMED KHFAGA Under Supervision: DR – KHALED GAMAL ELTURKY \n");
            Console.WriteLine();
            Console.WriteLine();
            Mini_FAT.InitializeOrOpenFileSystem(path);
            currentDirectory = Mini_FAT.Root;

            path = new string(currentDirectory.Dir_Namee).Trim('\0');
            while (true)
            {
                Console.Write(path+ ">>");               
                string Command = Console.ReadLine();
                var command = new Command_Line(Command);


            }


        }

       



       
        

    }
}
