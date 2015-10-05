using System.IO;

namespace Core
{
    public class File : IFile
    {
        public void WriteAllText(string path, string contents)
        {
            var fi = new FileInfo(path);
            if (fi.Directory != null && !fi.Directory.Exists)
            {
                fi.Directory.Create();
            }

            System.IO.File.WriteAllText(path, contents);
        }
    }
}