namespace MindHeal.FileManagers
{
    public interface IFileManager
    {
        Task<FileResponseModel> UploadFileToSystem(IFormFile formFile);
        Task<FilesResponseModel> ListOfFilesToSystem(IList<IFormFile> formFiles);
    }
}
