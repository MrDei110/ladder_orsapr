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
        /// <param name="minValue">Минимальное значение.</param>
        /// <param name="parameterType">Тип параметра.</param>
        public Parameter(int maxValue, int minValue, ParameterType parameterType)
        {
            try
            {
                this.MinMaxValidate(minValue, maxValue);
                this.MinValue = minValue;
                this.MaxValue = maxValue;
                this.TypeOfParameter = parameterType;
            }
            catch (MinMaxException ex)
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
                    this.ValueValidate(value);
                    this._value = value;
                }
                catch (ValueException ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Gets для поля _typeOfParameter (значение).
        /// </summary>
        public ParameterType TypeOfParameter
        {
            get
            {
                return this._typeOfParameter;
            }

            private set
            {
                this._typeOfParameter = value;
            }
        }

        /// <summary>
        /// Валидация вводимого значения _value в параметр.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <exception cref="ArgumentException">Текст ошибки.</exception>
        private void ValueValidate(int value)
        {
            if (value < this.MinValue || value > this.MaxValue)
            {
                throw new ValueException();
            }
        }

        /// <summary>
        /// Валидация на определение граничных условий.
        /// </summary>
        /// <param name="maxValue">Максимальное значение.</param>
        /// <param name="minValue">Минимальное значение.</param>
        /// <exception cref="ArgumentException">Текст ошибки.</exception>
        private void MinMaxValidate(int minValue, int maxValue)
        {
            if (maxValue <= minValue || minValue < 0)
            {
                throw new MinMaxException();
            }
        }
    }

    /// <summary>
    /// Класс пользовательского исключения на ввод Min и Max.
    /// </summary>
    public class MinMaxException : ArgumentOutOfRangeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MinMaxException"/> class.
        /// </summary>
        public MinMaxException()
        {
        }
    }

    /// <summary>
    /// Класс пользовательского исключения на ввод Value.
    /// </summary>
    public class ValueException : ArgumentOutOfRangeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueException"/> class.
        /// </summary>
        public ValueException()
        {
        }
    }
}
