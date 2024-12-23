using System;
using System.Runtime.InteropServices;
using Kompas;
using LadderPlugin;

namespace BuilderClass
{
    /// <summary>
    /// Класс для построения модели отвёртки в Компас.
    /// </summary>
    public class Builder
    {
        /// <summary>
        /// Экземпляр класс Wrapper.
        /// </summary>
        private Wrapper _wrapper = new Wrapper();

        /// <summary>
        /// Построение отвёртки.
        /// </summary>
        /// <param name="parameters">Параметры отвёртки.</param>
        public void Build(Parameters parameters)
        {
            this._wrapper.OpenCAD();
            this._wrapper.CreateFile();
            this.BuildLadder(parameters);
        }

        /// <summary>
        /// Построение лестницы.
        /// </summary>
        /// <param name="parameters">Параметры отвёртки.</param>
        private void BuildLadder(Parameters parameters)
        {
            this._wrapper.CreateSketch(1);
            Parameter totalHeight;
            parameters.AllParameters.TryGetValue(ParameterType.TotalHeight, out totalHeight);
            Parameter stepsAmount;
            parameters.AllParameters.TryGetValue(ParameterType.StepsAmount, out stepsAmount);
            Parameter materialThickness;
            parameters.AllParameters.TryGetValue(ParameterType.MaterialThickness, out materialThickness);
            Parameter stepsSpacing;
            parameters.AllParameters.TryGetValue(ParameterType.StepsSpacing, out stepsSpacing);
            Parameter stepsWidth;
            parameters.AllParameters.TryGetValue(ParameterType.StepsWidth, out stepsWidth);
            double y = (double)totalHeight.Value;
            double x = (double)stepsWidth.Value;
            int n = stepsAmount.Value;
            double d = (double)materialThickness.Value;
            double step = (double)stepsSpacing.Value;
            double halfX = x / 2;
            double halfWithThicknessX = halfX + d;
            double[,] pointsArrayCruciform =
                {
                    { halfX, 0, halfX, y, 1 },
                    { halfX, 0, halfWithThicknessX, 0, 1 },
                    { halfX, y, halfWithThicknessX, y, 1 },
                    { halfWithThicknessX, y, halfWithThicknessX, 0, 1 },
                    { -halfX, 0, -halfX, y, 1 },
                    { -halfX, 0, -halfWithThicknessX, 0, 1 },
                    { -halfX, y, -halfWithThicknessX, y, 1 },
                    { -halfWithThicknessX, y, -halfWithThicknessX, 0, 1 },
                };
            this._wrapper.CreateLine(pointsArrayCruciform, 0, 8);
            this._wrapper.Extrusion(d / 2);
            for (int i = 0; i < n; i++)
            {
                this._wrapper.CreateSketch(1);
                double delta = step;
                double upperY = y - ((d + step) * i + delta);
                double lowerY = y - ((d + step) * i + d + delta);
                double[,] pointsForStep =
                {
                    { halfX, upperY, halfX, lowerY, 1},
                    { halfX, upperY, -halfX, upperY, 1},
                    { -halfX, upperY, -halfX, lowerY, 1},
                    { halfX, lowerY, -halfX, lowerY, 1},
                };
                this._wrapper.CreateLine(pointsForStep, 0, 4);
                this._wrapper.Extrusion(d / 2);
            }
        }

    }
}
