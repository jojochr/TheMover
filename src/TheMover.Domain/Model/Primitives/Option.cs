namespace TheMover.Domain.Model.Primitives {
    public readonly struct Option<T> {
        private Option(T value) {
            _Value = value;
            _IsSome = value != null;
        }

        private readonly bool _IsSome;
        private readonly T _Value;

        public static implicit operator Option<T>(T value) => new(value);

        public static Option<T> None => default;
        public static Option<T> Some(T value) => new(value);

        /// <summary>
        /// This Method can be used to modify the options value in some way without fully resolving it.<br></br>
        /// This does nothing if tho option does not contain a value.
        /// </summary>
        /// <param name="transformingAction">An action that gets performed on the options value if it contains a value</param>
        /// <returns>Returns the unresolved option type with its modified value</returns>
        public Option<T> TransformIfValue(Action<T> transformingAction) {
            if(!_IsSome) {
                return this;
            }

            transformingAction(_Value);

            return this;
        }

        /// <summary>
        /// This method can be used to get the Value of the <see cref="Option{T}"/> while using a default Value if this instance of the <see cref="Option{T}"/> did not contain a Value.<br/>
        /// If you just want to identify if the option contained a value, you can just put null in and check if the Value was null afterward.
        /// </summary>
        public T Reduce(T valueIfOptionWasNone) => !_IsSome ? valueIfOptionWasNone : _Value;


        /// <summary>
        /// This method can be used to access (dump) the contents of an option instance.
        /// </summary>
        /// <param name="value">Will be filled with the options value, be careful and only access this if <paramref name="isSome"/> is true</param>
        /// <param name="isSome">Indicates if the <paramref name="value"/> can be used</param>
        public void Dump(out T value, out bool isSome) {
            value = _Value;
            isSome = _IsSome;
        }
    }
}
