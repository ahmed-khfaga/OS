using System.IO;
using System;

namespace OS
{
    class Program
    {
        public static Directory? Root;
        public static Directory currentDirectory;
        public static string path = string.Empty;

        static void Main(string[] args)
        {
           
            Console.WriteLine("Welcome to OS_Project_Virtual_DISK_shell ^_^ ");
            Console.WriteLine("developed by AHMED KHFAGA Under Supervision: DR – KHALED GAMAL ELTURKY \n");
            Console.WriteLine();
            Console.WriteLine();
            initialize();
            InitializeFileSystem();
            currentDirectory = Root;
        
            Path();

            while (true)
            {
                Console.Write(path);
                Console.Write(">>");

                string Command = Console.ReadLine();
                var command = new Command_Line(Command);


            }


        }

        static void InitializeFileSystem()
        {

            Root = new Directory("N",0x10, 5, null);
            Root.Read_Directory();

        }

        static void Path()
        {
            if (Root != null)
            {
                string s = new(currentDirectory?.Dir_Namee);
                path = s.Trim();
            }
        }

        public static void initialize()
        {
            Mini_FAT.InitializeOrOpenFileSystem("myDisk");

        }
     
        

    }
}
