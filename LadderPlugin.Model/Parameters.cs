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
        /// Поле хранящее в себе текущий параметр.
        /// </summary>
        private Dictionary<ParameterType, Parameter> _parameter;

        /// <summary>
        /// Поле хранящее в себе словарь всех параметров.
        /// </summary>
        private Dictionary<ParameterType, Parameter> _parameters;

        /// <summary>
        /// Gets or sets для _parameters.
        /// </summary>
        public Dictionary<ParameterType, Parameter> AllParameters
        {
            get
            {
                return this._parameters;
            }

            set
            {
                this._parameters = value;
            }
        }

        /// <summary>
        /// Метод для добавления нового параметра в словарь.
        /// </summary>
        /// <param name="parameterType">Тип добавляемого параметра.</param>
        /// <param name="parameter">Параметр.</param>
        public void SetParameter(ParameterType parameterType, Parameter parameter)
        {
            try
            {
                this._parameter = new Dictionary<ParameterType, Parameter>()
                {
                    { parameterType, parameter },
                };
                this.AllParameters.Remove(parameterType);
                this.AllParameters.Add(parameterType, parameter);
                this.ValidateParameters();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        /// <summary>
        /// Валидация зависимых параметров.
        /// </summary>
        /// <exception cref="ArgumentException">Текст ошибки.</exception>
        private void ValidateParameters()
        {
            string exception = string.Empty;
            ParameterType parameterType = this._parameter.ElementAt(0).Key;
            Parameter parameter = this._parameter.ElementAt(0).Value;
            Parameter chainedParameterFirst;
            Parameter chainedParameterSecond;
            Parameter chainedParameterThird;
            switch (parameterType)
            {
                case ParameterType.TotalHeight:
                    if (this.AllParameters.TryGetValue(
                        ParameterType.StepsAmount,
                        out chainedParameterFirst) == true &&
                    this.AllParameters.TryGetValue(
                        ParameterType.StepsSpacing,
                        out chainedParameterSecond) == true &&
                    this.AllParameters.TryGetValue(
                        ParameterType.MaterialThickness,
                        out chainedParameterThird) == true)
                    {
                        double stepsAmount = chainedParameterFirst.Value;
                        double stepsSpacing = chainedParameterSecond.Value;
                        double materialThickness = chainedParameterThird.Value;
                        double minValue = (stepsAmount + 1) * stepsSpacing + stepsAmount * materialThickness;
                        if (parameter.Value < minValue)
                        {
                            exception += "Общая высота лестницы меньше чем сумма её ступеней" +
                                    ", увеличьте заданное значение минимум до "
                                    + minValue.ToString() + '\n';
                        }
                    }

                    break;
                case ParameterType.StepsAmount:
                    if (this.AllParameters.TryGetValue(
                        ParameterType.TotalHeight,
                        out chainedParameterFirst) == true &&
                    this.AllParameters.TryGetValue(
                        ParameterType.StepsSpacing,
                        out chainedParameterSecond) == true &&
                    this.AllParameters.TryGetValue(
                        ParameterType.MaterialThickness,
                        out chainedParameterThird) == true)
                    {
                        double totalHeight = chainedParameterFirst.Value;
                        double stepsSpacing = chainedParameterSecond.Value;
                        double materialThickness = chainedParameterThird.Value;
                        double maxValue = (totalHeight - stepsSpacing) / (stepsSpacing + materialThickness);
                        if (parameter.Value > maxValue)
                        {
                            exception += "Общая сумма ступеней больше высоты лестницы" +
                                    ", уменьшите количество ступеней до "
                                    + maxValue.ToString() + '\n';
                        }
                    }

                    break;
                case ParameterType.StepsSpacing:
                    if (this.AllParameters.TryGetValue(
                        ParameterType.TotalHeight,
                        out chainedParameterFirst) == true &&
                    this.AllParameters.TryGetValue(
                        ParameterType.StepsAmount,
                        out chainedParameterSecond) == true &&
                    this.AllParameters.TryGetValue(
                        ParameterType.MaterialThickness,
                        out chainedParameterThird) == true)
                    {
                        double totalHeight = chainedParameterFirst.Value;
                        double stepsAmount = chainedParameterSecond.Value;
                        double materialThickness = chainedParameterThird.Value;
                        double maxValue = (totalHeight - stepsAmount * materialThickness) / (stepsAmount + 1);
                        if (parameter.Value > maxValue)
                        {
                            exception += "Общая сумма ступеней больше высоты лестницы" +
                                    ", уменьшите пространство между ступенями до "
                                    + maxValue.ToString() + '\n';
                        }
                    }

                    break;
                case ParameterType.MaterialThickness:
                    if (this.AllParameters.TryGetValue(
                        ParameterType.TotalHeight,
                        out chainedParameterFirst) == true &&
                    this.AllParameters.TryGetValue(
                        ParameterType.StepsAmount,
                        out chainedParameterSecond) == true &&
                    this.AllParameters.TryGetValue(
                        ParameterType.StepsSpacing,
                        out chainedParameterThird) == true)
                    {
                        double totalHeight = chainedParameterFirst.Value;
                        double stepsAmount = chainedParameterSecond.Value;
                        double stepsSpacing = chainedParameterThird.Value;
                        double maxValue = (totalHeight - (stepsAmount + 1) * stepsSpacing) / stepsAmount;
                        if (parameter.Value > maxValue)
                        {
                            exception += "Общая сумма ступеней больше высоты лестницы" +
                                    ", уменьшите толщину профиля до  "
                                    + maxValue.ToString() + '\n';
                        }
                    }

                    break;
            }

            if (exception != string.Empty)
            {
                throw new ArgumentException(exception);
            }
        }
    }
}
