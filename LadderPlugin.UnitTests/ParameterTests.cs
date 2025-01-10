using System;
using NUnit.Framework;

namespace LadderPlugin.UnitTests
{
    /// <summary>
    /// Класс Unit тестов класса <see cref="Parameter"/>.
    /// </summary>
    [TestFixture]
    public class ParameterTests
    {
        /// <summary>
        /// Тестовый параметр.
        /// </summary>
        private Parameter _parameter = new Parameter(16, 14);

        /// <summary>
        /// Позитивный тест геттера MaxValue.
        /// </summary>
        [Test(Description = "Позитивный тест геттера MaxValue.")]
        public void TestProjectGetMaxValue()
        {
            var expected = 16;
            var actual = this._parameter.MaxValue;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Позитивный тест геттера MinValue.
        /// </summary>
        [Test(Description = "Позитивный тест геттера MinValue.")]
        public void TestProjectGetMinValue()
        {
            var expected = 14;
            var actual = this._parameter.MinValue;
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// Позитивный тест геттера Value.
        /// </summary>
        [Test(Description = "Позитивный тест геттера Value.")]
        public void TestProjectGetValue()
        {
            var expected = 15;
            var actual = this._parameter.Value = 15;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Позитивный тест сеттера Value.
        /// </summary>
        [Test(Description = "Позитивный тест сеттера Value.")]
        public void TestProjectSetValue()
        {
            Parameter expected = new Parameter(16, 14);
            expected.Value = 15;
            var actual = this._parameter.Value = 15;
            Assert.AreEqual(expected.Value, actual);
        }

        /// <summary>
        /// TestCase методов проверки сеттера свойства Value.
        /// </summary>
        /// <param name="wrongValue">Неверное поле текст.</param>
        /// <param name="message">Текст ошибки.</param>
        [TestCase(
            10,
            "Нарушение в определении граничных условий",
            TestName = "Нарушение в определении граничных условий")]
        [TestCase(
            20,
            "Должно возникать исключение, если значение больше MaxValue",
            TestName = "Значение за граничными пределами")]
        public void TestSetArgumentException(int wrongValue, string message)
        {
            Assert.Throws<ArgumentException>(
            () => { this._parameter.Value = wrongValue; },
            message);
        }
    }
}
