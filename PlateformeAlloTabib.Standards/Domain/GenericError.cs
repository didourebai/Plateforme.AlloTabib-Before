using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlateformeAlloTabib.Standards.Domain
{
    public class GenericError : IAmError
    {
        public EErrorType Type { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }
    }
}
