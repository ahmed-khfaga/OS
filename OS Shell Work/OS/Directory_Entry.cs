using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS
{
    class Directory_Entry
    {
        public char[] Dir_Namee = new char[11];
        public byte dir_Attr;
        public byte[] Dir_Empty = new byte[12];
        public int dir_First_Cluster;
        public int dir_FileSize;

        public Directory_Entry(char[] Dir_Name, byte dir_Attribute, int f_Cluster)
        {

            
            string DIR_NAME = new string(Dir_Name);
            assign_DirName(DIR_NAME);         
            this.dir_Attr = dir_Attribute;
            this.dir_First_Cluster = f_Cluster;
           
            Array.Clear(Dir_Empty, 0, Dir_Empty.Length);

        }

        public void assign_File_Name(string Name, string extension)
        {
            
            string cleaned_Name = clean_The_Name(Name);
            if (cleaned_Name.Length > 7)
            {
                cleaned_Name = cleaned_Name.Substring(0, 7);
            }

            string cleaned_Extension = clean_The_Name(extension);
            if (cleaned_Extension.Length > 3)
            {
                cleaned_Extension = cleaned_Extension.Substring(0, 3);
            }
            string full_Name = $"{cleaned_Name.PadRight(7)}.{cleaned_Extension}";
            Array.Clear(Dir_Namee, 0, Dir_Namee.Length); 


            // Ensure the full name is exactly 11 characters long
            Array.Copy(full_Name.ToCharArray(), Dir_Namee, full_Name.Length);
        }

        private string clean_The_Name(string name)
        {
           // string cleaned = name.Trim();
            char[] prohibitedChars = new char[]
            {
                '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+', '[', ']', '{', '}',
                ';', ':', ',', '.', '<', '>', '/', '?', '\\', '|', '~', '`'
            };
            string cleaned = new string(name.Where(c => !prohibitedChars.Contains(c)).ToArray());

            if(cleaned.Length < 11 ) 
            {
                for (int i = cleaned.Length; i < 11; i++)
                    cleaned += " ";
            }
            return cleaned;
        }
        public void assign_DirName(string name)
        {
            string cleaned_Name = clean_The_Name(name);

           
            if (cleaned_Name.Length > 11)
            {               
                cleaned_Name = cleaned_Name.Substring(0, 11);
            }
            if(cleaned_Name.Length < 11 ) 
            {
                cleaned_Name += " ";
            }

            
            Array.Clear(Dir_Namee, 0, Dir_Namee.Length); // Clear the array
            Array.Copy(cleaned_Name.ToCharArray(), Dir_Namee, cleaned_Name.Length);
        }
    }
}
