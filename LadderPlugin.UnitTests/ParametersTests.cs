using System;
using System.Collections.Generic;
using LadderPlugin;
using System.Runtime.InteropServices;
using NUnit.Framework;
namespace ScrewdriverPlugin.UnitTests
{
    /// <summary>
    /// Класс Unit тестов класса <see cref="Parameters"/>.
    /// </summary>
    [TestFixture]
    public class ParametersTests
    {
        /// <summary>
        /// Тестовые параметры.
        /// </summary>
        private Parameters _parameters = new Parameters();

        /// <summary>
        /// Позитивный тест геттера AllParameters.
        /// </summary>
        [Test(Description = "Позитивный тест геттера AllParameters.")]
        public void TestProjectGetParameters()
        {
            Dictionary<ParameterType, Parameter> expected = new Dictionary<ParameterType,
                Parameter>()
            {
            };
            this._parameters.AllParameters = new Dictionary<ParameterType, Parameter>()
            {
            };
            var actual = this._parameters;
            Assert.AreEqual(expected, actual.AllParameters);
        }

        /// <summary>
        /// Позитивный тест сеттера AllParameters.
        /// </summary>
        [Test(Description = "Позитивный тест сеттера AllParameters.")]
        public void TestProjectSetParameters()
        {
            this._parameters.AllParameters = new Dictionary<ParameterType, Parameter>()
            {
            };
            Parameters expected = new Parameters();
            expected.AllParameters = new Dictionary<ParameterType, Parameter>()
            {
            };
            var actual = this._parameters;
            Assert.AreEqual(expected.AllParameters, actual.AllParameters);
        }

        /// <summary>
        /// Позитивный тест метода SetParameter.
        /// </summary>
        [Test(Description = "Позитивный тест метода SetParameter.")]
        public void TestProjectSetParameter()
        {
            Parameter parameter = new Parameter();
            parameter.MaxValue = 20;
            parameter.MinValue = 10;
            parameter.Value = 15;
            Parameters expected = new Parameters();
            this._parameters.AllParameters = new Dictionary<ParameterType, Parameter>();
            expected.AllParameters = new Dictionary<ParameterType, Parameter>();
            this._parameters.SetParameter(ParameterType.TotalHeight, parameter);
            expected.SetParameter(ParameterType.TotalHeight, parameter);
            var actual = this._parameters;
            Assert.AreEqual(expected.AllParameters, actual.AllParameters);
        }

        /// <summary>
        /// TestCase методов проверки сеттера свойства Value.
        /// </summary>
        /// <param name="parameterType">Тип параметра.</param>
        /// <param name="wrongArgument">Неверный аргумент.</param>
        /// <param name="message">Текст ошибки.</param>
        [TestCase(
            ParameterType.TotalHeight,
            900,
            "Должно возникать исключение, если TotalHeight меньше чем результат вычисления других параметров",
            TestName = "Общая высота лестницы меньше чем сумма её ступеней увеличьте заданное значение минимум до")]
        [TestCase(
            ParameterType.MaterialThickness,
            55,
            "Должно возникать исключение, если уравнение всех параметров больше TotalHeight",
            TestName = "Общая сумма ступеней больше высоты лестницы уменьшите толщину профиля до")]
        [TestCase(
            ParameterType.StepsAmount,
            14,
            "Должно возникать исключение, если уравнение всех параметров больше TotalHeight",
            TestName = "Общая сумма ступеней больше высоты лестницы уменьшите количество ступеней до")]
        [TestCase(
            ParameterType.StepsSpacing,
            340,
            "Должно возникать исключение, если уравнение всех параметров больше TotalHeight",
            TestName = "Общая сумма ступеней больше высоты лестницы уменьшите пространство между ступенями до")]
        public void TestSetArgumentException(
            ParameterType parameterType,
            int wrongArgument,
            string message)
        {
            Parameter totalHeight = new Parameter();
            totalHeight.TypeOfParameter = ParameterType.TotalHeight;
            totalHeight.Value = 960;
            Parameter materialThickness = new Parameter();
            materialThickness.TypeOfParameter = ParameterType.MaterialThickness;
            materialThickness.Value = 30;
            Parameter stepsAmount = new Parameter();
            stepsAmount.TypeOfParameter = ParameterType.StepsAmount;
            stepsAmount.Value = 2;
            Parameter stepsSpacing = new Parameter();
            stepsSpacing.TypeOfParameter = ParameterType.StepsSpacing;
            stepsSpacing.Value = 300;
            this._parameters.AllParameters = new Dictionary<ParameterType, Parameter>();
            this._parameters.SetParameter(ParameterType.TotalHeight, totalHeight);
            this._parameters.SetParameter(ParameterType.MaterialThickness, materialThickness);
            this._parameters.SetParameter(ParameterType.StepsAmount, stepsAmount);
            this._parameters.SetParameter(ParameterType.StepsSpacing, stepsSpacing);
            Parameter newParameter = new Parameter();
            newParameter.MaxValue = 5000;
            newParameter.MinValue = 2;
            newParameter.Value = wrongArgument;
            Assert.Throws<ArgumentException>(
            () => { this._parameters.SetParameter(parameterType, newParameter); },
            message);
        }
    }
}
