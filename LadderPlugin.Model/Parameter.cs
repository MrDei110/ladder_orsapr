using System;

namespace LadderPlugin
{
    /// <summary>
    /// Класс параметр.
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// Поле для максимального значения параметра.
        /// </summary>
        private int _maxValue;

        /// <summary>
        /// Поле для минимального значения параметра.
        /// </summary>
        private int _minValue;

        /// <summary>
        /// Поле для значения параметра.
        /// </summary>
        private int _value;

        /// <summary>
        /// Поле для значения типа параметра.
        /// </summary>
        private ParameterType _typeOfParameter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameter"/> class.
        /// </summary>
        /// <param name="maxValue">Максимальное значение.</param>
        /// <param name="minValue">Минимальное значение.</param>]
        public Parameter(int maxValue, int minValue)
        {
            try
            {
                this._maxValue = maxValue;
                this._minValue = minValue;
                this.MinMaxValidate();
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets для поля _maxValue (максимальное значение).
        /// </summary>
        public int MaxValue
        {
            get
            {
                return this._maxValue;
            }

            private set
            {
                this._maxValue = value;
            }
        }

        /// <summary>
        /// Gets для поля _minValue (минимальное значение).
        /// </summary>
        public int MinValue
        {
            get
            {
                return this._minValue;
            }

            private set
            {
                this._minValue = value;
            }
        }

        /// <summary>
        /// Gets or sets для поля _value (значение).
        /// </summary>
        public int Value
        {
            get
            {
                return this._value;
            }

            set
            {
                try
                {
                    this._value = value;
                    this.Validate();
                }
                catch (ArgumentException ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Gets or sets для поля _typeOfParameter (значение).
        /// </summary>
        public ParameterType TypeOfParameter
        {
            get
            {
                return this._typeOfParameter;
            }

            set
            {
                this._typeOfParameter = value;
            }
        }

        /// <summary>
        /// Валидация вводимого значения _value в параметр.
        /// </summary>
        /// <exception cref="ArgumentException">Текст ошибки.</exception>
        private void Validate()
        {
            if (this._value < this._minValue || this._value > this._maxValue)
            {
                throw new ArgumentException("Значение за граничными пределами");
            }
        }

        /// <summary>
        /// Валидация на определение граничных условий.
        /// </summary>
        /// <exception cref="ArgumentException">Текст ошибки.</exception>
        private void MinMaxValidate()
        {
            if (this._maxValue <= this._minValue || this._minValue < 0)
            {
                throw new ArgumentException("Нарушение в определении граничных условий");
            }
        }
    }
}
