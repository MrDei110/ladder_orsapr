using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
namespace LadderPlugin.UnitTests
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
            Parameters expected = new Parameters();
            var actual = this._parameters;
            Assert.AreEqual(
                expected.AllParameters[ParameterType.TotalHeight].Value,
                actual.AllParameters[ParameterType.TotalHeight].Value);
        }

        /// <summary>
        /// Позитивный тест метода SetParameter.
        /// </summary>
        [Test(Description = "Позитивный тест метода SetParameter.")]
        public void TestProjectSetParameter()
        {
            Parameters expected = new Parameters();
            this._parameters.SetParameter(ParameterType.StepsAmount, 13);
            expected.SetParameter(ParameterType.StepsAmount, 13);
            var actual = this._parameters;
            Assert.AreEqual(
                expected.AllParameters[ParameterType.StepsAmount].Value,
                actual.AllParameters[ParameterType.StepsAmount].Value);
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
            Parameters parameters = new Parameters();
            parameters.SetParameter(ParameterType.TotalHeight, 960);
            parameters.SetParameter(ParameterType.MaterialThickness, 30);
            parameters.SetParameter(ParameterType.StepsAmount, 2);
            parameters.SetParameter(ParameterType.StepsSpacing, 300);
            Assert.Throws<ParametersException>(
            () => { parameters.SetParameter(parameterType, wrongArgument); },
            message);
        }
    }
}
