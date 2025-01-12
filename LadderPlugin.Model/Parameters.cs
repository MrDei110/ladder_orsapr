using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LadderPlugin
{
    /// <summary>
    /// Класс параметры.
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// Максимальное значение общей высоты.
        /// </summary>
        private const int TOTAL_HEIGHT_MAX_VALUE = 5000;

        /// <summary>
        /// Минимальное значение общей высоты.
        /// </summary>
        private const int TOTAL_HEIGHT_MIN_VALUE = 900;

        /// <summary>
        /// Максимальное значение количества ступеней.
        /// </summary>
        private const int STEPS_AMOUNT_MAX_VALUE = 14;

        /// <summary>
        /// Минимальное значение количества ступеней.
        /// </summary>
        private const int STEPS_AMOUNT_MIN_VALUE = 2;

        /// <summary>
        /// Максимальное значение толщины профиля.
        /// </summary>
        private const int MATERIAL_THICKNESS_MAX_VALUE = 55;

        /// <summary>
        /// Минимальное значение толщины профиля.
        /// </summary>
        private const int MATERIAL_THICKNESS_MIN_VALUE = 30;

        /// <summary>
        /// Максимальное значение расстояния между ступенями.
        /// </summary>
        private const int STEPS_SPACING_MAX_VALUE = 340;

        /// <summary>
        /// Минимальное значение расстояния между ступенями.
        /// </summary>
        private const int STEPS_SPACING_MIN_VALUE = 300;

        /// <summary>
        /// Максимальное значение ширины ступеней.
        /// </summary>
        private const int STEPS_WIDTH_MAX_VALUE = 800;

        /// <summary>
        /// Минимальное значение ширины ступеней.
        /// </summary>
        private const int STEPS_WIDTH_MIN_VALUE = 460;

        /// <summary>
        /// Поле хранящее в себе словарь всех параметров.
        /// </summary>
        private Dictionary<ParameterType, Parameter> _parameters;

        /// <summary>
        /// Gets or sets для _parameters.
        /// </summary>
        public  Dictionary<ParameterType, Parameter> AllParameters
        {
            get
            {
                return this._parameters;
            }

            private set
            {
                this._parameters = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameters"/> class.
        /// </summary>
        public Parameters()
        {
            this.AllParameters = new Dictionary<ParameterType, Parameter>()
            {
                {
                    ParameterType.TotalHeight,
                    new Parameter(
                        TOTAL_HEIGHT_MAX_VALUE,
                        TOTAL_HEIGHT_MIN_VALUE,
                        ParameterType.TotalHeight)
                },
                {
                    ParameterType.StepsAmount,
                    new Parameter(
                        STEPS_AMOUNT_MAX_VALUE,
                        STEPS_AMOUNT_MIN_VALUE,
                        ParameterType.StepsAmount)
                },
                {
                    ParameterType.MaterialThickness,
                    new Parameter(
                        MATERIAL_THICKNESS_MAX_VALUE,
                        MATERIAL_THICKNESS_MIN_VALUE,
                        ParameterType.MaterialThickness)
                },
                {
                    ParameterType.StepsSpacing,
                    new Parameter(
                        STEPS_SPACING_MAX_VALUE,
                        STEPS_SPACING_MIN_VALUE,
                        ParameterType.StepsSpacing)
                },
                {
                    ParameterType.StepsWidth,
                    new Parameter(
                        STEPS_WIDTH_MAX_VALUE,
                        STEPS_WIDTH_MIN_VALUE,
                        ParameterType.StepsWidth)
                },
            };
        }

        /// <summary>
        /// Метод для добавления нового параметра в словарь.
        /// </summary>
        /// <param name="parameterType">Тип параметра.</param>
        /// <param name="value">Значение.</param>
        public void SetParameter(ParameterType parameterType, int value)
        {
            try
            {
                this.AllParameters[parameterType].Value = value;
                this.ValidateParameters(this.AllParameters[parameterType]);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Валидация зависимых параметров.
        /// </summary>
        /// <param name="parameter">Параметр.</param>
        /// <exception cref="ParametersException">Текст ошибки.</exception>
        private void ValidateParameters(Parameter parameter)
        {
            string message = string.Empty;
            switch (parameter.TypeOfParameter)
            {
                case ParameterType.TotalHeight:
                {
                    Parameter stepsAmount = this.AllParameters[ParameterType.StepsAmount];
                    Parameter stepsSpacing = this.AllParameters[ParameterType.StepsSpacing];      
                    Parameter materialThickness =
                            this.AllParameters[ParameterType.MaterialThickness];  
                    if (stepsAmount.Value != 0 &&
                        stepsSpacing.Value != 0 &&
                        materialThickness.Value != 0)
                    {
                        double minValue =
                            (stepsAmount.Value + 1) *
                            stepsSpacing.Value +
                            stepsAmount.Value *
                            materialThickness.Value;
                        if (parameter.Value != minValue)
                        {
                            message = "Общая высота лестницы меньше чем сумма её ступеней" +
                                    this.TextCaster(minValue);
                            }
                    }

                    break;
                }

                case ParameterType.StepsAmount:
                {
                    Parameter stepsSpacing = this.AllParameters[ParameterType.StepsSpacing];      
                    Parameter materialThickness =
                        this.AllParameters[ParameterType.MaterialThickness];
                    Parameter totalHeight = this.AllParameters[ParameterType.TotalHeight];
                    if (stepsSpacing.Value != 0 &&
                        materialThickness.Value != 0 &&
                        totalHeight.Value != 0)
                    {
                        double maxValue = (totalHeight.Value - stepsSpacing.Value) /
                            (stepsSpacing.Value + materialThickness.Value);
                        if (parameter.Value != maxValue)
                        {
                            message = this.NoHeightTextCaster() + this.TextCaster(maxValue);
                            }
                    }

                    break;
                }

                case ParameterType.StepsSpacing:
                {
                    Parameter materialThickness =
                        this.AllParameters[ParameterType.MaterialThickness];
                    Parameter totalHeight = this.AllParameters[ParameterType.TotalHeight];
                    Parameter stepsAmount = this.AllParameters[ParameterType.StepsAmount];
                    if (materialThickness.Value != 0 &&
                        totalHeight.Value != 0 &&
                        stepsAmount.Value != 0)
                    {
                        double maxValue = (totalHeight.Value - stepsAmount.Value *
                            materialThickness.Value) / (stepsAmount.Value + 1);
                        if (parameter.Value != maxValue)
                        {
                            message = this.NoHeightTextCaster() + this.TextCaster(maxValue);
                            }
                    }

                    break;
                }

                case ParameterType.MaterialThickness:
                {
                    Parameter totalHeight = this.AllParameters[ParameterType.TotalHeight];
                    Parameter stepsAmount = this.AllParameters[ParameterType.StepsAmount];
                    Parameter stepsSpacing = this.AllParameters[ParameterType.StepsSpacing];  
                    if (totalHeight.Value != 0 &&
                        stepsAmount.Value != 0 &&
                        stepsSpacing.Value != 0)
                    {
                        double maxValue = (totalHeight.Value - (stepsAmount.Value + 1) *
                            stepsSpacing.Value) / stepsAmount.Value;
                        if (parameter.Value != maxValue)
                        {
                            message = this.NoHeightTextCaster() + this.TextCaster(maxValue);
                        }
                    }

                    break;
                }
            }

            if (message != string.Empty)
            {
                throw new ParametersException(message);
            }
        }

        /// <summary>
        /// Вспомогательный метод формирующий текст для сообщения.
        /// </summary>
        /// <param name="value">Значение для формирования текста.</param>
        /// <returns>Текст сообщения.</returns>
        private string TextCaster(double value)
        {
            return $", измените заданное значение на {value.ToString()}\n";
        }

        /// <summary>
        /// Вспомогательный метод формирующий текст для сообщения, используемый везде, кроме высоты.
        /// </summary>
        /// <returns>Текст для сообщения.</returns>
        private string NoHeightTextCaster()
        {
            return "Общая сумма ступеней больше высоты лестницы";
        }
    }

    /// <summary>
    /// Класс пользовательского исключения на ввод параметров.
    /// </summary>
    public class ParametersException : ArgumentOutOfRangeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParametersException"/> class.
        /// </summary>
        /// <param name="message">Передаваемое сообщение.</param>
        public ParametersException(string message)
            : base(message)
        {
        }
    }
}
