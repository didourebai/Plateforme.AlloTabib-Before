using System.Collections.Generic;

namespace PlateformeAlloTabib.Standards.Domain
{
    public class ResultOfType<T> : Result
    {
        public virtual T Data { get; set; }

        public virtual List<IAmError> Errors { get; set; }

        public ResultOfType()
        {
            this.Errors = new List<IAmError>();
        }

        public ResultOfType(T Data)
        {
            this.Data = Data;
        }
    }
}
