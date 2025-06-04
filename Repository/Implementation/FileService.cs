using Eletronic_Api.Repository.Abastract;

namespace Eletronic_Api.Repository.Implementation
{
    public class FileService: IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif",".svg" };
        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public Tuple<int, string> SaveImage(IFormFile file)
        {
            try
            {
                var path = Path.Combine(_environment.ContentRootPath, "Images");
                Directory.CreateDirectory(path);

                var ext = Path.GetExtension(file.FileName).ToLower();
                if (!_allowedExtensions.Contains(ext))
                    return new Tuple<int, string>(0, "Invalid file extension");

                var newFileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(path, newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return File.Exists(filePath) ? new Tuple<int, string>(1, newFileName) : new Tuple<int, string>(0, "File save failed");
            }
            catch (Exception ex)
            {
                return new Tuple<int, string>(0, ex.Message);
            }
        }

        public bool DeleteImage(string fileName)
        {
            try
            {
                var path = Path.Combine(_environment.ContentRootPath, "Images", fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

    }
}
