
namespace tar
{


    internal class Program
    {


        private static async System.Threading.Tasks.Task Main(string[] args)
        {
            // await Test(args);

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

                            using(System.IO.Stream rdr = System.IO.File.OpenRead(files[i]))
                            {
                                CopyStream(rdr, ms);
                            }

                            ms.Seek(0, System.IO.SeekOrigin.Begin);
                            await tar.WriteAsync(ms, ms.Length, fileName);
                        }

                    } // Next i 

                } // End Using tar 

            } // End Using fs 


            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();

            await System.Threading.Tasks.Task.CompletedTask;
        } // End Task Main 

        public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }


        private static async System.Threading.Tasks.Task Test(string[] args)
        {
            if (args.Length < 2)
            {
                System.Console.WriteLine("USAGE: ArchiveMaker fileName.tar <fileToAdd.ext> [. more files..]");
                return;
            } // End if (args.Length < 2) 


            using (System.IO.Stream archUsTar = System.IO.File.Create(args[0]))
            {
                using (tar_cs.TarWriter tar = new tar_cs.TarWriter(archUsTar))
                {
                    await tar.WriteDirectoryEntryAsync("test_dir");
                    for (int i = 1; i < args.Length; ++i)
                    {
                        await tar.WriteAsync(args[i]);
                    } // Next i 

                } // End Using tar 

            } // End Using archUsTar 

            System.Console.WriteLine("Examine tar file: {0}", args[0]);
            using (System.IO.Stream examiner = System.IO.File.OpenRead(args[0]))
            {
                tar_cs.TarReader tar = new tar_cs.TarReader(examiner);
                while (await tar.MoveNextAsync(true))
                {
                    System.Console.WriteLine("File: {0}, Owner: {1}", tar.FileInfo.FileName, tar.FileInfo.UserName);
                } // Whend 

            } // End Using examiner 

            using (System.IO.Stream unarchFile = System.IO.File.OpenRead(args[0]))
            {
                tar_cs.TarReader reader = new tar_cs.TarReader(unarchFile);
                await reader.ReadToEndAsync("out_dir\\data");
            } // End Using unarchFile 

        } // End Task Test 


    } // End Class Program 


} // End Namespace tar 
