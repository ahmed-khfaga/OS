using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace OS
{
    class Directory : Directory_Entry
    {
        public Directory? Parent;
        public List<Directory_Entry> DirectoryTable;
        public int cluster_Index;

        public Directory(string name, byte attr, int first_Cluster ,Directory Parent) : base(name.ToCharArray(), attr, first_Cluster)
        {
            DirectoryTable = new List<Directory_Entry>();

            if (Parent != null)
            {
                this.Parent = Parent;
            }
        }
        public Directory_Entry Get_Directory_Entry()
        {
            Directory_Entry me = new Directory_Entry(this.Dir_Namee, this.dir_Attr, this.dir_First_Cluster);
            for (int i = 0; i < 12; i++)
            {
                me.Dir_Empty[i] = this.Dir_Empty[i];
            }
            me.dir_FileSize = this.dir_FileSize;
            return me;
        }

        public int Get_My_Size_On_Disk()
        {
            int size = 0;
            if (this.dir_First_Cluster != 0)
            {
                int cluster = this.dir_First_Cluster;
                int next = Mini_FAT.getNext(cluster);
                do
                {
                    size++;
                    cluster = next;
                    if (cluster != -1)
                        next = Mini_FAT.getNext(cluster);
                } while (cluster != -1);
            }
            return size;
        }

        public bool Can_Add_Entry(Directory d)
        {
            bool can = false;
            int needed_Size = (DirectoryTable.Count + 1) * 32;
            int needed_Cluster = needed_Size / 1024;
            int rem = needed_Size % 1024; 
            if (rem > 0) 
            {
                needed_Cluster++; 
            }
            needed_Cluster += d.dir_FileSize / 1024;
            int rem1 = d.dir_FileSize % 1024;
            if (rem1 > 0)
            {
                needed_Cluster++;
            }
            if (Get_My_Size_On_Disk() + Mini_FAT.get_Availabel_Clusters() >= needed_Cluster)
            {
                can = true;
            }
            return can;
        }
        public void Empty_My_Clusters()
        {
            if (this.dir_First_Cluster == 0)
                return; // no cluster to empty 
            if (this.dir_First_Cluster != 0)
            {
                int cluster = this.dir_First_Cluster;
                int next = Mini_FAT.getNext(cluster);

                if (cluster == 5 && next == 0)
                    return;
                do
                {
                    Mini_FAT.setNext(cluster, 0);
                    cluster = next;
                    if (cluster != -1)
                        next = Mini_FAT.getNext(cluster);

                } while (cluster != -1);
            }
        }

        public void Read_Directory()
        {
            DirectoryTable = new List<Directory_Entry>();

            if (dir_First_Cluster != 0 )
            {
                
                cluster_Index = dir_First_Cluster;
                int next = Mini_FAT.getNext(cluster_Index);
                List<byte> ls = new List<byte>();
                if (cluster_Index == 5 && next == 0)
                    return;
                do
                {
                    ls.AddRange(Virtual_Disk.read_Cluster(cluster_Index));
                    cluster_Index = next;
                    if (cluster_Index != -1)
                    {
                        next = Mini_FAT.getNext(cluster_Index);
                    }
                } while (next != -1);

                DirectoryTable = Converter.BytesToDirectory_Entries(ls);
            }

           
        }
        #region write
        //public void WriteDirectory()
        //{
        //    List<byte> DirData = new List<byte>();

        //    foreach (Directory_Entry d in DirectoryTable)
        //    {
        //        DirData.AddRange(Converter.Directory_EntryToBytes(d));
        //    }

        //    int totalClusterNeed = (int)Math.Ceiling((double)DirData.Count / Virtual_Disk.clusterSize);
        //    int [] availableClusters = (int[])Mini_FAT.get_Availabel_Cluster();

        //    if (availableClusters.Length < totalClusterNeed)
        //    {
        //        Console.WriteLine("Not enough space to write!");
        //        return;
        //    }

        //    Empty_My_Clusters();

        //    dir_First_Cluster = Mini_FAT.get_Availabel_Cluster();

        //    if (dir_First_Cluster == -1)
        //    {
        //        Console.WriteLine("No space left.");
        //        return;
        //    }

        //    int bytesWritten = 0;
        //    int currentCluster = dir_First_Cluster;

        //    while (bytesWritten < DirData.Count)
        //    {
        //        int bytesToWrite = Math.Min(DirData.Count - bytesWritten, Virtual_Disk.clusterSize);
        //        byte[] dataToWrite = new byte[Virtual_Disk.clusterSize];

        //        Array.Copy(DirData.ToArray(), bytesWritten, dataToWrite, 0, bytesToWrite);
        //        Virtual_Disk.write_Cluster(dataToWrite, currentCluster);

        //        bytesWritten += bytesToWrite;

        //        if (bytesWritten < DirData.Count)
        //        {
        //            int nextCluster = Mini_FAT.get_Availabel_Cluster();
        //            if (nextCluster != -1)
        //            {
        //                Mini_FAT.setNext(currentCluster, nextCluster);
        //                currentCluster = nextCluster;
        //            }
        //            else
        //            {
        //                Mini_FAT.setNext(currentCluster, -1);
        //                break;
        //            }
        //        }
        //        else
        //        {
        //            Mini_FAT.setNext(currentCluster, -1);
        //        }
        //    }

        //    if (Parent != null)
        //    {
        //        int parentIndex = Parent.DirectoryTable.FindIndex(e => new string(e.Dir_Namee) == new string (this.Dir_Namee));
        //        if (parentIndex != -1)
        //        {
        //            Parent.DirectoryTable[parentIndex] = this.Get_Directory_Entry();
        //            Parent.WriteDirectory();
        //        }
        //    }

        //    Mini_FAT.write_FAT();
        //} 
        #endregion

        public void Write_Directory()
        {

            Directory_Entry o = Get_Directory_Entry();
            byte[] dirs_Or_Files_Bytes = new byte[DirectoryTable.Count * 32];//بعد كل ده كل البيانات متخزنى هنا
            
            List<byte> Directory_entry_byte = new List<byte>(32);
           
            for (int i=0;i<DirectoryTable.Count;i++)
            {
                Directory_entry_byte = Converter.Directory_EntryToBytes(DirectoryTable[i]);

                for(int j = i * 32 , c = 0 ; c < 32 ; c++ , j++)
                {
                    dirs_Or_Files_Bytes[j] = Directory_entry_byte[c]; // يخزن كل الداتا بتاعت الانتري جوه الاراي 
                }
            }

            List<byte[]> bytes = Converter.SplitBytes(dirs_Or_Files_Bytes);
            if (this.dir_First_Cluster != 0)
            {
                Empty_My_Clusters();
                cluster_Index = Mini_FAT.get_Availabel_Cluster();
                this.dir_First_Cluster = cluster_Index;
            }
            else
            {
                cluster_Index = Mini_FAT.get_Availabel_Cluster();
                this.dir_First_Cluster = cluster_Index;
            }
            int last_Cluster = -1;
            for (int i = 0; i < bytes.Count; i++)
            {
                if (cluster_Index != -1)
                {
                    Virtual_Disk.write_Cluster(bytes[i], cluster_Index);
                    Mini_FAT.setNext(cluster_Index, -1);
                    if (last_Cluster != -1)
                    {
                        Mini_FAT.setNext(last_Cluster, cluster_Index);
                    }
                   // Mini_FAT.write_FAT();
                    last_Cluster = cluster_Index;
                    cluster_Index = Mini_FAT.get_Availabel_Cluster();
                }
            }
            if (DirectoryTable.Count == 0)
            {
                if (Parent != null)
                {
                    // Mini_FAT.setNext(dir_First_Cluster, 0);
                    Empty_My_Clusters();
                    dir_First_Cluster = 0;
                }
            }
            Directory_Entry n = Get_Directory_Entry();
            if (this.Parent != null)
            {
                this.Parent.Update_Content(o, n);
               // this.Parent.Write_Directory();
            }
            Mini_FAT.write_FAT();

        }


        public void Update_Content(Directory_Entry OLD, Directory_Entry NEW)
        {
            Read_Directory();
            char[] old_Name = OLD.Dir_Namee;
            string O_Name = new string(old_Name); // convert from char to string 
            int index = search_Directory(O_Name);
            if (index != -1)
            {
                DirectoryTable[index] = NEW;
                Write_Directory();
            }
        }

        
        public int search_Directory(string name)
        {
            // If DirectoryTable is out of date, then read it.
            // This condition is optional. You could handle this logic differently.
            if (DirectoryTable == null || DirectoryTable.Count == 0)
            {
                Read_Directory();
            }

            for (int i = 0; i < DirectoryTable.Count; i++)
            {
                string dirNameInTable = new string(DirectoryTable[i].Dir_Namee.Where(c => c != '\0').ToArray()).Trim();
                if (dirNameInTable.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return i; // Directory found
                }
            }
            return -1; // Directory not found
        }

        public void add_Entry(Directory_Entry d)
        {
            DirectoryTable.Add(d);
            Write_Directory();
          
        }
        public void remove_Entry(Directory_Entry d)
        {
            //Read_Directory();
            //string name = new string(d.Dir_Namee);
            //int index = search_Directory(name);
            //DirectoryTable.RemoveAt(index);
            //Write_Directory();
            //Read_Directory();
            if(DirectoryTable.Count() != 0)
            {
                string o = new string(d.Dir_Namee);
                DirectoryTable.Remove(d);               
            }
            else
            {
                Console.WriteLine($"Error Entry \"{d.Dir_Namee}\" not found");
            }
        }

        public void delete_Directory()
        {
            Empty_My_Clusters();
            //DirectoryTable.Clear();
            if (this.Parent != null)
            {
                Directory_Entry dir_Entry = Get_Directory_Entry();
                Parent.remove_Entry(dir_Entry);
                Parent.Write_Directory();
            }
            if (this.dir_First_Cluster != 0)
                this.dir_First_Cluster = 0;

            Mini_FAT.write_FAT();
        }
    }
}
