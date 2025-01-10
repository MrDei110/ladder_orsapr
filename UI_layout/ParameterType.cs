using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LadderPlugin
{
    /// <summary>
    /// Перечисление типов параметров.
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// Общая высота лестницы.
        /// </summary>
        TotalHeight,

        /// <summary>
        /// Количество ступеней.
        /// </summary>
        StepsAmount,

        /// <summary>
        /// Толщина материала.
        /// </summary>
        MaterialThickness,

        /// <summary>
        /// Расстояние между ступенями.
        /// </summary>
        StepsSpacing,

        /// <summary>
        /// Ширина ступени.
        /// </summary>
        StepsWidth,
    }
}
