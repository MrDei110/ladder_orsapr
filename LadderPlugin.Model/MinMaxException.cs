using System;

namespace LadderPlugin
{
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
}
