using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models
{
    public class MovieViewModel : IValidatableObject
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Year { get; set; }

        public string Producer { get; set; }

        public string Added { get; set; }

        public IFormFile Poster { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new();

            if (string.IsNullOrWhiteSpace(this.Title))
            {
                errors.Add(new ValidationResult("Введите название фильма!", new List<string>() { nameof(Title) }));
            }
            if (string.IsNullOrWhiteSpace(this.Description))
            {
                errors.Add(new ValidationResult("Введите описание фильма!", new List<string>() { nameof(Description) }));
            }
            if (!this.Year.HasValue)
            {
                errors.Add(new ValidationResult("Введите год выпуска!", new List<string>() { nameof(Year) }));
            }
            else if (this.Year.Value > DateTime.Today || this.Year.Value == default)
            {
                errors.Add(new ValidationResult("Неверный год выпуска фильма!", new List<string>() { nameof(Year) }));
            }
            if (!ValidateImage(this.Poster))
            {
                errors.Add(new ValidationResult("Неверный тип файла!", new List<string>() { nameof(Poster) }));
            }
            if (string.IsNullOrWhiteSpace(this.Producer))
            {
                errors.Add(new ValidationResult("Введите режисера фильма!", new List<string>() { nameof(Producer) }));
            }

            return errors;
        }

        private bool ValidateImage(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
                if (resolutions.Contains(uploadedFile.ContentType) &&
                uploadedFile.Length < MAX_SIZE_IN_BYTES)
                    return true;

            return false;
        }

        private const long MAX_SIZE_IN_BYTES = 1500000;

        private readonly List<string> resolutions = new()
        {
            "image/jpeg",
            "image/bmp",
            "image/jpg",
            "image/gif",
            "image/png",
            "image/png"
        };
    }
}