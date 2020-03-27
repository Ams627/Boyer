using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boyer
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var fullname = System.Reflection.Assembly.GetEntryAssembly().Location;
                var dir = Path.GetDirectoryName(fullname);
                var testDir = Path.Combine(new DirectoryInfo(dir).Parent.Parent.FullName, "TestData");
                var filename = Path.Combine(testDir, "ada.exe");
                var bytes = File.ReadAllBytes(filename);
                var rlist = BoyerMoore.SearchString(bytes, "TestApplicationToken");
                if (rlist.Length == 0)
                {
                    throw new Exception("No matches");
                }
                if (rlist.Length != 1)
                {
                    Console.Error.WriteLine("Warning: found more than one match");
                }

                var encodedReplacement = Encoding.Unicode.GetBytes("DebugToken").ToList();
                encodedReplacement.AddRange(new byte[] { 0, 0 });

                encodedReplacement.CopyTo(bytes, rlist.First());
                var newFilename = Path.ChangeExtension(filename, "exe2");
                File.WriteAllBytes(newFilename, bytes);
            }
            catch (Exception ex)
            {
                var fullname = System.Reflection.Assembly.GetEntryAssembly().Location;
                var progname = Path.GetFileNameWithoutExtension(fullname);
                Console.Error.WriteLine(progname + ": Error: " + ex.Message);
            }
        }
    }
}
