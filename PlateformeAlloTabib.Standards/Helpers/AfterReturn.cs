using System;
using PlateformeAlloTabib.Standards.Domain;

namespace PlateformeAlloTabib.Standards.Helpers
{
    public class AfterReturn<T> : ISetStatusDetail<T>, IAddErrorsOrFinalize<T>, IAddAllKindOfErrors<T>, IAddValidationStuff<T>, IAddGenericErrors<T>, IFinalize<T>
    {
        private ResultOfType<T> _envelope;

        public AfterReturn(ResultOfType<T> envelope)
        {
            this._envelope = envelope;
        }

        public IAddErrorsOrFinalize<T> As(EStatusDetail statusDetail)
        {
            this._envelope.StatusDetail = statusDetail;
            return (IAddErrorsOrFinalize<T>)this;
        }

        public IAddErrorsOrFinalize<T> AsNotFound()
        {
            this._envelope.StatusDetail = EStatusDetail.NotFound;
            return (IAddErrorsOrFinalize<T>)this;
        }

        public IAddValidationStuff<T> AsValidationFailure()
        {
            this._envelope.StatusDetail = EStatusDetail.BadRequest;
            return (IAddValidationStuff<T>)this;
        }

        public IFinalize<T> AsValidationFailure(ValidationResponse validation)
        {
            this.AsValidationFailure();
            return (IFinalize<T>)this.AddingValidationResponse(validation);
        }

        public IFinalize<T> AsValidationFailure(ValidationError validation)
        {
            this.AsValidationFailure();
            return (IFinalize<T>)this.AddingValidationError(validation);
        }

        public IFinalize<T> AsValidationFailure(string attemptedValue, string message, string propertyName)
        {
            this.AsValidationFailure();
            return (IFinalize<T>)this.AddingValidationError(attemptedValue, message, propertyName);
        }

        public IAddAllKindOfErrors<T> AsGenericError()
        {
            this._envelope.StatusDetail = EStatusDetail.InternalServerError;
            return (IAddAllKindOfErrors<T>)this;
        }

        public IAddErrorsOrFinalize<T> AddingError(IAmError error)
        {
            this._envelope.Errors.Add(error);
            return (IAddErrorsOrFinalize<T>)this;
        }

        public IAddErrorsOrFinalize<T> AddingException(Exception exception)
        {
            ExceptionError exceptionError = new ExceptionError();
            exceptionError.Message = exception.Message;
            exceptionError.Type = EErrorType.EXCEPTION;
            exceptionError.Exception = exception;
            this._envelope.Errors.Add((IAmError)exceptionError);
            return (IAddErrorsOrFinalize<T>)this;
        }

        public IAddErrorsOrFinalize<T> AddingValidationResponse(ValidationResponse validation)
        {
            foreach (ValidationError error in validation.Errors)
                this.AddingValidationError(error);
            return (IAddErrorsOrFinalize<T>)this;
        }

        public IAddErrorsOrFinalize<T> AddingValidationError(ValidationError error)
        {
            this._envelope.Errors.Add((IAmError)error);
            return (IAddErrorsOrFinalize<T>)this;
        }

        public IAddErrorsOrFinalize<T> AddingValidationError(string attemptedValue, string message, string propertyName)
        {
            this._envelope.Errors.Add((IAmError)new ValidationError()
            {
                Message = message,
                AttemptedValue = (object)attemptedValue,
                PropertyName = propertyName
            });
            return (IAddErrorsOrFinalize<T>)this;
        }

        public IAddErrorsOrFinalize<T> AddingGenericError(string Code, string Message)
        {
            this._envelope.Errors.Add((IAmError)new GenericError()
            {
                Code = Code,
                Message = Message
            });
            return (IAddErrorsOrFinalize<T>)this;
        }

        public IAddErrorsOrFinalize<T> AddingDataAccessError()
        {
            this._envelope.Errors.Add((IAmError)new GenericError()
            {
                Code = "0",
                Message = "Error in data Layer"
            });
            return (IAddErrorsOrFinalize<T>)this;
        }

        public ResultOfType<T> WithDefaultResult()
        {
            this._envelope.Data = default(T);
            return this._envelope;
        }

        public ResultOfType<T> WithResult(T result)
        {
            this._envelope.Data = result;
            return this._envelope;
        }
    }
}
