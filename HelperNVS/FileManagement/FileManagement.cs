using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HelperNVS.FileManagement
{
    public class FileManagement
    {
        #region "Memory Stream"

        public bool MemoryStreamWrite(string path, string directoryPath, MemoryStream ms, ref string messageError)
        {
            try
            {
                bool exists = System.IO.Directory.Exists(directoryPath);

                if (!exists)
                    System.IO.Directory.CreateDirectory(directoryPath);

                File.WriteAllBytes(path, ms.ToArray());
            }
            catch (Exception e)
            {
                messageError = e.Message;
                return false;
            }

            return true;
        }

        public MemoryStream MemoryStreamRead(string path, ref string messageError)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                    file.CopyTo(ms);
            }
            catch (Exception e)
            {
                messageError = e.Message;
                return null;
            }

            return ms;
        }

        public List<ClassesHelper.ClassesHelper.FliesName> GetFilesWithoutExtensionFromDirectory(string dir, ref string messageError)
        {
            List<ClassesHelper.ClassesHelper.FliesName> ms = new List<ClassesHelper.ClassesHelper.FliesName>();
            try
            {
                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                {
                    ms.Add(new ClassesHelper.ClassesHelper.FliesName
                    {
                        Url = Path.GetFileNameWithoutExtension(file)
                    }
                    );
                }
            }
            catch (Exception e)
            {
                messageError = e.Message;
                return null;
            }

            return ms;
        }

        #endregion


        public bool DeleteFileByPath(string path, ref string messageError)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception e)
            {
                messageError = e.Message;
                return false;
            }

            return true;
        }



    }
}
