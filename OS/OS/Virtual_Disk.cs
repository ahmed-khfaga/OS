using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS
{
    class Virtual_Disk
    {
        public static FileStream disk;
        public const int clusterSize = 1024;
        public const int clusters = 1024;
        public const int diskSize = clusterSize * clusters;
        private static bool isnew = false;
            

        public static void create_Or_Open_Disk(string path)
        {

            #region s
            //try
            //{
            //    // here we Open File 
            //    disk = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);


            //    // check if File can't Read or Can't Write g Exeption and create New File 
            //    if (!disk.CanRead || !disk.CanWrite)
            //    {
            //        throw new IOException("Failed to open disk for reading and writing.");
            //    }

            //}
            //catch (FileNotFoundException) // here we catch Exeption and Create New File 
            //{
            //    disk = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);


            //    disk.Close();

            //    // After Make File We Open it 
            //    disk = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            //} 
            #endregion

            if(!File.Exists(path))
            {
                 disk = new FileStream(path,FileMode.Create,FileAccess.ReadWrite);
                                    
                 isnew = true;                    
                
            }
          
            else
            {

                disk = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);

                //close_Disk();
            }

        }

        public static void write_Cluster(byte[] clusterdata, int clusterIndex)
        {
           
            
            disk.Seek(clusterIndex * clusterSize, SeekOrigin.Begin);
            disk.Write(clusterdata, 0, clusterdata.Length);
            disk.Flush();
            //close_Disk();

        }
        public static byte[] read_Cluster(int clusterIndex)
        {
            
                disk.Seek(clusterIndex * 1024, SeekOrigin.Begin);
                byte[] bytes = new byte[1024];
                disk.Read(bytes, 0, 1024);
                return bytes;

        }

        public static int get_Free_Space()
        {
            disk.Seek(0, SeekOrigin.End);  
            return diskSize - (int)disk.Position; 
        }

        public static bool is_New()
        {

          
            return isnew == true ? true : false;
        }

     
    }
}
