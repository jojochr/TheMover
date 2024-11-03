namespace TheMover.Domain.Model.Primitives {
    public readonly struct Result<T, E> {
        private readonly bool _Success;
        private readonly T _Value;
        private readonly E _Error;

        private Result(T v, E e, bool success) {
            _Value = v;
            _Error = e;
            _Success = success;
        }

        public bool IsOk => _Success;
        public bool IsNotOk => !IsOk;

        public static Result<T, E> Ok(T v) => new(v, default!, true);

        public static Result<T, E> Err(E e) => new(default!, e, false);

        public static implicit operator Result<T, E>(T v) => new(v, default!, true);
        public static implicit operator Result<T, E>(E e) => new(default!, e, false);

        /// <summary>
        /// Can be used to get one single type out of value and error.<br></br>
        /// </summary>
        public R Match<R>(
            Func<T, R> success
          , Func<E, R> failure) =>
            _Success ? success(_Value) : failure(_Error);

        public void Match(Action<T> success, Action<E> failure) {
            if(_Success) {
                success(_Value);
            }
            failure(_Error);
        }

        /// <summary>
        /// This can be used to resolve the result.
        /// </summary>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        /// <returns>This returns:<br></br>True -> If the result contains a value.<br></br>False -> If the Result contains an error.</returns>
        public bool Resolve(Action<T> success, Action<E> failure) {
            if(_Success) {
                success(_Value);
                return true;
            }

            failure(_Error);
            return false;
        }

        /// <summary>
        /// This can be used to transform the values inside a result without resolving it.
        /// </summary>
        /// <param name="success">This method gets run in the value case with the value as parameter</param>
        /// <param name="failure">This method gets run in the error case with the error as parameter</param>
        /// <returns>The result with its transformed values</returns>
        public Result<T, E> Transform(Action<T> success, Action<E> failure) {
            if(_Success) {
                success(_Value);
                return this;
            }

            failure(_Error);
            return this;
        }

        /// <summary>This just returns the entire content of the result file. You wave all safety guarantees if you use this</summary>
        /// <returns>-> True if <see cref="IsOk"/><br/>-> False if <see cref="IsNotOk"/></returns>
        public bool Dump(out T? value, out E? error, out bool success) {
            value = _Value;
            error = _Error;
            success = _Success;
            return _Success;
        }

        public T Unwrap() {
            if(IsOk) {
                return _Value;
            }

            throw ResultUnwrapException<T, E>.GetException(this);
        }
    }

    /// <summary>
    /// This exception is not meant to be handled.<br></br>
    /// This should be thrown whenever you use the <see cref="Result{T, E}.Unwrap"/>-Method.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TError"></typeparam>
    public class ResultUnwrapException<TValue, TError> : Exception {
        private ResultUnwrapException(string message, Result<TValue, TError> result) : base(message) {
            Origin = result;
        }
        /// <summary>The Result that got unwrapped even though it contained an Exception</summary>
        public readonly Result<TValue, TError> Origin;
        public static ResultUnwrapException<TValue, TError> GetException(Result<TValue, TError> result) => new ResultUnwrapException<TValue, TError>(message: "A Result got unwrapped that contained an Exception.", result);
    }
}
