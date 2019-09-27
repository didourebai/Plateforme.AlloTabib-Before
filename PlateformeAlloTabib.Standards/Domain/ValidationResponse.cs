using System.Collections.Generic;

namespace PlateformeAlloTabib.Standards.Domain
{
    public class ValidationResponse
    {
        public bool IsValid { get; set; }

        public List<ValidationError> Errors { get; set; }

        public ValidationResponse()
        {
            this.Errors = new List<ValidationError>();
        }
    }
}
