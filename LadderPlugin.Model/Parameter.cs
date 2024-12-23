using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Поле для значения тип параметра.
        /// </summary>
        private ParameterType _typeOfParameter;

        /// <summary>
        /// Gets or sets для поля _maxValue (максимальное значение).
        /// </summary>
        public int MaxValue
        {
            get
            {
                return this._maxValue;
            }
            //TODO: validation?
            set
            {
                this._maxValue = value;
            }
        }

        /// <summary>
        /// Gets or sets для поля _minValue (минимальное значение).
        /// </summary>
        public int MinValue
        {
            get
            {
                return this._minValue;
            }
            //TODO: validation?
            set
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
                this._value = value;
                this.Validator();
            }
        }

        public ParameterType TypeOfParameter
        {
            get
            {
                return this._typeOfParameter;
            }

            set
            {
                this._typeOfParameter = value;
                this.DefineMinMax();
            }
        }

        private void DefineMinMax()
        {
            switch (this._typeOfParameter)
            {
                case ParameterType.TotalHeight:
                    this.MinValue = 900;
                    this.MaxValue = 5000;
                    break;
                case ParameterType.StepsAmount:
                    this.MinValue = 2;
                    this.MaxValue = 14;
                    break;
                case ParameterType.MaterialThickness:
                    this.MinValue = 30;
                    this.MaxValue = 55;
                    break;
                case ParameterType.StepsSpacing:
                    this.MinValue = 300;
                    this.MaxValue = 340;
                    break;
                case ParameterType.StepsWidth:
                    this.MinValue = 460;
                    this.MaxValue = 800;
                    break;
            }
        }

        /// <summary>
        /// Валидация вводимого значения _value в параметр.
        /// </summary>
        /// <exception cref="ArgumentException">Текст ошибки.</exception>
        private void Validator()
        {
            if (this.Value < this._minValue || this.Value > this._maxValue)
            {
                throw new ArgumentException("Простая ошибка");
            }
        }
    }
}
