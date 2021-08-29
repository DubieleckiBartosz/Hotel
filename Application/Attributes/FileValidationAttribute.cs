using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FileValidationAttribute : ValidationAttribute
    {
        private readonly string[] _fileExtensions;//= new string[] { ".jpg", ".png", ".pdf" };
        public FileValidationAttribute()
        {
            _fileExtensions =  new string[] { ".jpg", ".png", ".pdf" };
        }

    public FileValidationAttribute(string[] fileExtensions)
        {
            _fileExtensions = fileExtensions; //?? new string[] { ".jpg", ".png", ".pdf" };
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file is null || !(file.Length>0))
            {
                return new ValidationResult("File is required");
            }
            var extension = Path.GetExtension(file.FileName);

            if (!_fileExtensions.Contains(extension))
            {
                return new ValidationResult($"This photo extension is not allowed!");
            }
            return ValidationResult.Success;
        }
    }
}
//0A6D484B-0A66-4E5D-0767-08D960F6F671