using DirectumRX_beta.Models;
using DirectumRX_beta.Services;
using Microsoft.AspNetCore.Mvc;

namespace DirectumRX_beta.Controllers
{
    public class FilesController : Controller
    {
        private readonly FileService _fileService;
        private readonly LocalFileServices _localFileServices;

        public FilesController(FileService fileService, LocalFileServices localFileServices)
        {
            _fileService = fileService;
            _localFileServices = localFileServices;
        }

        [HttpGet]
        public IActionResult Public(int id, string password)
        {
            var docId = _localFileServices.GetDocumentBodyId(id, password);

            if (string.IsNullOrEmpty(docId))
            {
                var resultPath = _fileService.GetDocumentPath(id, password);

                if (string.IsNullOrEmpty(resultPath))
                {
                    return BadRequest("Not Found");
                }
                var signDate = Convert.ToDateTime(_fileService.GetSignDate(id));

                var model = new FileModel
                {
                    FileName = CheckData(_fileService.GetDocumentName(id)),
                    RegistrationNumber = CheckData(_fileService.GetRegistrationNumber(id)),
                    DocumentDate = Convert.ToDateTime(CheckData(_fileService.GetDocumentDate(id))),
                    SignDate = signDate.ToLocalTime(),
                    FilePath = resultPath,
                    SignatoryName = CheckData(_fileService.GetSignatoryFullName(id)),
                };

                _localFileServices.InsertFileData(id, _fileService.GetDocumentBodyId(id, password), model.RegistrationNumber, _fileService.GetDocumentDate(id), _fileService.GetSignDate(id), model.FileName, model.SignatoryName, password);
                return View(model);
            }
            else
            {
                var resultPath = _localFileServices.GetDocumentPath(id, password);

                if (string.IsNullOrEmpty(resultPath))
                {
                    return BadRequest("Not Found");
                }
                var signDate = Convert.ToDateTime(_localFileServices.GetSignDate(id));

                var model = new FileModel
                {
                    FileName = CheckData(_localFileServices.GetDocumentName(id)),
                    RegistrationNumber = CheckData(_localFileServices.GetRegistrationNumber(id)),
                    DocumentDate = Convert.ToDateTime(CheckData(_localFileServices.GetDocumentDate(id))),
                    SignDate = signDate.ToLocalTime(),
                    FilePath = resultPath,
                    SignatoryName = CheckData(_localFileServices.GetSignatoryFullName(id)),
                    //SignId=CheckData(_fileService.GetSignId(id)),
                };

                return View(model);
            }

        }

        private string CheckData(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                data = "Не задано";
            }
            return data;
        }
    }
}
