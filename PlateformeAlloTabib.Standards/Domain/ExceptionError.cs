using System;

namespace PlateformeAlloTabib.Standards.Domain
{
    public class ExceptionError : GenericError
    {
        public Exception Exception { get; set; }
    }
}
