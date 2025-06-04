namespace Eletronic_Api.Repository.Abastract
{
    public interface IFileService
    {

        Tuple<int ,string > SaveImage(IFormFile file);
        bool DeleteImage(string fileName);
    }
}
