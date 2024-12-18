using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS
{
    class Mini_FAT
    {
        public static int[] FAT = new int[1024];
       
        
        public static void Initialization()
        {
            FAT[0] = -1;
            FAT[1] = 2;
            FAT[2] = 3;
            FAT[3] = 4;
            FAT[4] = -1;
            for (int i = 5; i < FAT.Length; i++) 
            {
                FAT[i] = 0;
            }
        }

        public static byte[] create_Super_Block()
        {
            // byte[] bytes = new byte[1024];
            // for(int i = 0;i < 1024;i++)
            // {
            //     bytes[i] = 0;
            // }

            //return bytes; // initializes all elements to 0
            byte[] bytes = new byte[1024];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = 0;
               // pos++;
            }

            return bytes;
        }



        public static void write_FAT()
        {

            #region ks
            //Virtual_Disk.create_Or_Open_Disk(
            //byte[] FATBYTES = Converter.IntArrayToByteArray(FAT);//4096

            //List<byte[]> cluster = Converter.SplitBytes(FATBYTES);// هنا هنشوف هنحتاج كام كلاستر 
            //for (int i = 0; i < cluster.Count; i++)
            //{
            //    byte[] bytes = Encoding.ASCII.GetBytes("$");
            //    StringBuilder hashstrings = new StringBuilder();

            //    for (int j = i + 1; j < FAT.Length; j++)
            //    {
            //        hashstrings.Append("$");
            //    }

            //    Virtual_Disk.write_Cluster(Converter.StringToByteArray(hashstrings.ToString()), i + 1); // ()
            //} 
            #endregion

            byte[] Fatbytes = Converter.IntArrayToByteArray(FAT);
            List<byte[]> bytes = Converter.SplitBytes(Fatbytes);// size 4  {0 , 1, 2 , 3}
            for (int i = 0;i< bytes.Count;i++) 
            {
                Virtual_Disk.write_Cluster(bytes[i], i + 1); // 1 2 3 
            }

        }
        public static void readFAT()
        {
            List<byte> bytes = [];
            // i start from 1 cause 0 for superBlock 
            for (int i = 1; i <= 4; i++)
            {
                bytes.AddRange(Virtual_Disk.read_Cluster(i));
            }
            FAT = Converter.ByteArrayToIntArray(bytes.ToArray());
        }
        public static void print_Fat()
        {
            Console.WriteLine("FAT has the following");
            for (int i = 0; i < 1024; i++)
            {
                Console.WriteLine($" FAT[{i}] =  {FAT[i]} ");
            }
        }   
        public static void set_FAT(int[] fatArray)
        {
            if (fatArray.Length == FAT.Length)
            {
                Array.Copy(fatArray, FAT, fatArray.Length);

            }
            else
                Console.WriteLine("Out Of Range , Your size must be 1024");
        }

        public static int get_Availabel_Clusters()
        {
            int counter = 0;
            for (int i = 0; i < 1024; i++)
            {
                if (FAT[i] == 0)
                {
                    counter++;
                }
            }
            return counter;
        }
        public static int get_Availabel_Cluster()
        {
            for (int i = 0; i < 1024; i++)
            {
                if (FAT[i] == 0)
                {
                    return i;
                }
            }
            return -1;
        }


        public static int get_Free_Size()
        {
            return get_Availabel_Clusters() * 1024;
        }

        public static void InitializeOrOpenFileSystem(string name)
        {
            #region ss
            //Virtual_Disk.create_Or_Open_Disk(name);
            //if (Virtual_Disk.is_New())
            //{
            //    byte[] superBlock = Converter.StringToByteArray(create_Super_Block());//create_Super_Block();
            //    Virtual_Disk.write_Cluster(superBlock, 0);
            //    Initialization();
            //    write_FAT();
            //}
            //else
            //    readFAT(); 
            #endregion

            Virtual_Disk.create_Or_Open_Disk(name);

            if (Virtual_Disk.is_New())
            {
               // byte[] superBlock = Converter.StringToByteArray(create_Super_Block());
                byte[] superBlock = create_Super_Block();

                Virtual_Disk.write_Cluster(superBlock, 0);

                Initialization();

                write_FAT();
                
                #region s
                //byte[] emptyCluster = new byte[1024];
                //for (int clusterIndex = 5; clusterIndex < 1024; clusterIndex++)
                //{
                //    byte[] data = Encoding.ASCII.GetBytes("0");

                //    StringBuilder hashstringsdata = new StringBuilder();

                //    for (int j = 0; j < FAT.Length; j++)
                //    {
                //        hashstringsdata.Append("0");
                //    }
                //    Virtual_Disk.write_Cluster(Converter.StringToByteArray(hashstringsdata.ToString()), clusterIndex);
                //} 
                #endregion
            }
            else
            {
                readFAT();
            }

        }

        //setClusterPointer
        public static void setNext(int clusterIndex, int status)
        {
            if (clusterIndex >= 0 && clusterIndex < 1024)
            {
                FAT[clusterIndex] = status;
            }
        }
        //getClusterStatus
        public static int getNext(int clusterindex)
        {
            if (clusterindex >= 0 && clusterindex < 1024)
                return FAT[clusterindex];
            else
            return -1;
        }

        public static void closeTheSystem()
        {
            write_FAT();
        }
    }
}
