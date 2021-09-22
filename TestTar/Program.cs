
namespace TestTar
{


    class Program
    {


        static void Main(string[] args)
        {
            string fn = @"D:\username\Documents\Visual Studio 2017\TFS\SQLReporting_VS2008\SNB_Berichte\SqlUpdates\Test";
            string[] files = System.IO.Directory.GetFiles(fn, "*.sql", System.IO.SearchOption.AllDirectories);

            using (System.IO.Stream fs = System.IO.File.OpenWrite(@"D:\composite.tar"))
            {
                
                using (tar_cs.TarWriter tar = new tar_cs.TarWriter(fs))
                {

                    for (int i = 0; i < files.Length; ++i)
                    {
                        string fileName = System.IO.Path.GetFileName(files[i]);

                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {

                            using (System.IO.Stream rdr = System.IO.File.OpenRead(files[i]))
                            {
                                CopyStream(rdr, ms);
                            }

                            ms.Seek(0, System.IO.SeekOrigin.Begin);
                            tar.Write(ms, ms.Length, fileName);
                        }

                    } // Next i 

                } // End Using tar 

            } // End Using fs 


            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();

            System.Console.WriteLine("Hello World!");
        }


        public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }


    }


}
