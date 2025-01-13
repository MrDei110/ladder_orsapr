using System;

namespace LadderPlugin
{
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
