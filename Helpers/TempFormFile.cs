using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace c_sharp_playground.Helpers
{
    /// <summary>
    /// Helper class to deal with creating and disposing of the demo file
    /// </summary>
    public class TempFormFile : IDisposable
    {
        private readonly string _path;
        private readonly Stream _stream;
        private readonly FormFile _file;

        public TempFormFile(bool shouldCreate, IFileInfo demoFileInfo)
        {
            if (shouldCreate)
            {
                _path = Path.GetTempFileName();
                File.Copy(demoFileInfo.PhysicalPath, _path, true);
                _stream = File.OpenRead(_path);
                _file = new FormFile(_stream, 0, demoFileInfo.Length, Path.GetFileNameWithoutExtension(_path), Path.GetFileName(_path));
            }
        }

        void IDisposable.Dispose()
        {
            // Clean up temporary files
            if (_path != null && _stream != null)
            {
                _stream.Dispose();
                File.Delete(_path);
            }
        }

        public IFormFile FormFile
        {
            get
            {
                return _file;
            }
        }
    }
}
