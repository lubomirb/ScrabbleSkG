using System;
using System.Threading.Tasks;

namespace ScrabbleSkX.Data
{
   
    public class FileReader
    {
        private readonly FileService _fileService;

        public FileReader(FileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<string> ReadFile(string path)
        {
            // path="SK/SK.ini"
            return await _fileService.ReadFileAsync(path);
            //Console.WriteLine(content);
        }
    }

}
