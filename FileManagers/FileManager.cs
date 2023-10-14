namespace MindHeal.FileManagers
{
    public class FileManager : IFileManager
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileManager(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<FileResponseModel> UploadFileToSystem(IFormFile formFile)
        {
            var fileDestinationPath = Path.Combine(_webHostEnvironment.WebRootPath, "Documents");

            if (formFile is null || formFile.Length is 0)
            {
                var response = new FileResponseModel
                {
                    Status = false,
                    Message = "file not found",
                };
                return response;
            }

            if (!Directory.Exists(fileDestinationPath)) Directory.CreateDirectory(fileDestinationPath);

            var fileName = $"{Guid.NewGuid().ToString().Remove(9)}{formFile.FileName}";
            var fileWithoutName = Path.GetFileNameWithoutExtension(formFile.FileName);
            var fileType = formFile.ContentType.ToLower();
            var fileExtension = Path.GetExtension(formFile.FileName);
            var fileSizeInKb = formFile.Length / 1024;
            var fileSourcePath = Path.GetFileName(formFile.FileName);
            var destinationFullPath = Path.Combine(fileDestinationPath, fileName);

            using (var stream = new FileStream(destinationFullPath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            var fileSystemReponse = new FileResponseModel
            {
                Status = true,
                Message = "file successfully uploaded",
                Data = new FileDTO
                {
                    Extension = fileExtension,
                    FileType = fileType,
                    Name = fileName,
                    Title = fileWithoutName,
                    Filesize = fileSizeInKb,
                },
            };
            return fileSystemReponse;
        }
        public async Task<FilesResponseModel> ListOfFilesToSystem(IList<IFormFile> formFiles)
        {
            var fileInfos = new List<FileDTO>();
            foreach (var item in formFiles)
            {
                var fileinfo = await UploadFileToSystem(item);
                fileInfos.Add(fileinfo.Data);
            }
            return new FilesResponseModel
            {
                Status = true,
                Message = "File successfully Saved",
                Datas = fileInfos,
            };
        }
    }
    
}
