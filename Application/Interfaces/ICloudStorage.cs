using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICloudStorage
    {
        Task<string> UploadAsync(IFormFile file, string fileNameHint = null, string folderId = null, CancellationToken ct = default);
    }

}
