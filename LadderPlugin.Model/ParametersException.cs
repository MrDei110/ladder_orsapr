using System;

namespace LadderPlugin
{
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
