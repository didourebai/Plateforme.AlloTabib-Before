using PlateformeAlloTabib.Standards.Domain;

namespace PlateformeAlloTabib.Standards.Helpers
{
    public class Return<T> : ISetStatus<T>
    {
        private ResultOfType<T> _envelope;

        public Return()
        {
            this._envelope = new ResultOfType<T>();
        }

        public ISetStatusDetail<T> Error()
        {
            this._envelope.Status = EResultStatus.Error;
            return (ISetStatusDetail<T>)new AfterReturn<T>(this._envelope);
        }

        public IFinalize<T> OK()
        {
            this._envelope.Status = EResultStatus.Ok;
            this._envelope.StatusDetail = EStatusDetail.OK;
            return (IFinalize<T>)new AfterReturn<T>(this._envelope);
        }
    }
}
