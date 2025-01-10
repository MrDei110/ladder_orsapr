using System;
using System.Data.SqlClient;
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
        /// Постоянная разница в 2 раза.
        /// </summary>
        private const double SCALETWO = 2;

        /// <summary>
        /// Постоянная корень из 2 на 2.
        /// </summary>
        private const double SQRT2DIVIDED2 = 0.707;

        /// <summary>
        /// Угол pi/4.
        /// </summary>
        private const double PIDiVIDED4ANGLE = 45;

        /// <summary>
        /// Экземпляр класс Wrapper.
        /// </summary>
        private Wrapper _wrapper = new Wrapper();

        /// <summary>
        /// Построение отвёртки.
        /// </summary>
        /// <param name="parameters">Параметры отвёртки.</param>
        public void Build(Parameters parameters, int type)
        {
            this._wrapper.OpenCAD();
            this._wrapper.CreateFile();
            this.BuildLadder(parameters, type);
        }

        /// <summary>
        /// Построение лестницы.
        /// </summary>
        /// <param name="parameters">Параметры отвёртки.</param>
        private void BuildLadder(Parameters parameters, int type)
        {
            switch (type)
            {
                case 0:
                {
                    this._wrapper.CreateSketch(1);
                    parameters.AllParameters.TryGetValue(
                        ParameterType.TotalHeight,
                        out Parameter totalHeight);
                    parameters.AllParameters.TryGetValue(
                        ParameterType.StepsAmount,
                        out Parameter stepsAmount);
                    parameters.AllParameters.TryGetValue(
                        ParameterType.MaterialThickness,
                        out Parameter materialThickness);
                    parameters.AllParameters.TryGetValue(
                        ParameterType.StepsSpacing,
                        out Parameter stepsSpacing);
                    parameters.AllParameters.TryGetValue(
                        ParameterType.StepsWidth,
                        out Parameter stepsWidth);
                    double y = (double)totalHeight.Value;
                    double x = (double)stepsWidth.Value;
                    int n = stepsAmount.Value;
                    double d = (double)materialThickness.Value;
                    double step = (double)stepsSpacing.Value;
                    double halfX = x / SCALETWO;
                    double halfWithThicknessX = halfX + d;
                    double[,] pointsArray =
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
                    this._wrapper.CreateLine(pointsArray, 0, 8);
                    this._wrapper.Extrusion(1, d);
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
                        this._wrapper.Extrusion(1, d);
                    }

                    break;
                }
                case 1:
                {
                    Parameter totalHeight;
                    parameters.AllParameters.TryGetValue(
                        ParameterType.TotalHeight,
                        out totalHeight);
                    Parameter stepsAmount;
                    parameters.AllParameters.TryGetValue(
                        ParameterType.StepsAmount,
                        out stepsAmount);
                    Parameter materialThickness;
                    parameters.AllParameters.TryGetValue(
                        ParameterType.MaterialThickness,
                        out materialThickness);
                    Parameter stepsSpacing;
                    parameters.AllParameters.TryGetValue(
                        ParameterType.StepsSpacing,
                        out stepsSpacing);
                    Parameter stepsWidth;
                    parameters.AllParameters.TryGetValue(
                        ParameterType.StepsWidth,
                        out stepsWidth);
                    double y = (double)totalHeight.Value;
                    double x = (double)stepsWidth.Value;
                    int n = stepsAmount.Value;
                    double d = (double)materialThickness.Value;
                    double step = (double)stepsSpacing.Value;
                    double halfX = x / SCALETWO;
                    double halfWithThicknessX = halfX + d;
                    double offset = y * SQRT2DIVIDED2;
                    this._wrapper.CreateOffsetPlane(offset);
                    this._wrapper.CreateAnglePlane(PIDiVIDED4ANGLE);
                    this._wrapper.CreateSketch(0);
                    double[,] pointsArray =
                    {
                        { halfX, 0, halfX, y, 1 },
                        { halfX, 0, halfWithThicknessX, 0, 1 },
                        { halfX, y, halfWithThicknessX, y, 1 },
                        { halfWithThicknessX, y, halfWithThicknessX, 0, 1 },
                        { -halfX, 0, -halfX, y, 1 },
                        { -halfX, 0, -halfWithThicknessX, 0, 1 },
                        { -halfX, y, -halfWithThicknessX, y, 1 },
                        { -halfWithThicknessX, y, -halfWithThicknessX, 0, 1 },
                        {
                            y / SCALETWO * SQRT2DIVIDED2 - d * SQRT2DIVIDED2,
                            - y / SCALETWO * SQRT2DIVIDED2 + d,
                            y / SCALETWO * SQRT2DIVIDED2 - d * SQRT2DIVIDED2,
                            - y / SCALETWO * SQRT2DIVIDED2,
                            1
                        },
                        {
                            - y / SCALETWO * SQRT2DIVIDED2 - d * SQRT2DIVIDED2,
                            - y / SCALETWO * SQRT2DIVIDED2,
                            - y / SCALETWO * SQRT2DIVIDED2 - d * SQRT2DIVIDED2,
                            - y / SCALETWO * SQRT2DIVIDED2 + d,
                            1
                        },
                        {
                            - y / SCALETWO * SQRT2DIVIDED2 - d * SQRT2DIVIDED2,
                            - y / SCALETWO * SQRT2DIVIDED2 + d,
                            y / SCALETWO * SQRT2DIVIDED2 - d * SQRT2DIVIDED2,
                            - y / SCALETWO * SQRT2DIVIDED2 + d,
                            1
                        },
                        {
                            y / SCALETWO * SQRT2DIVIDED2 - d * SQRT2DIVIDED2,
                            - y / SCALETWO * SQRT2DIVIDED2,
                            -y / SCALETWO * SQRT2DIVIDED2 - d * SQRT2DIVIDED2,
                            - y / SCALETWO * SQRT2DIVIDED2,
                            1
                        },
                    };
                    this._wrapper.CreateLine(pointsArray, 0, 8);
                    this._wrapper.Extrusion(1, d);
                    for (int i = 0; i < n + 1; i++)
                    {
                        this._wrapper.CreateSketch(0);
                        double delta = step;
                        double upperY = (d + step) * i;
                        double lowerY = (d + step) * i + d;
                        double[,] pointsForStep =
                        {
                            { halfX, upperY, halfX, lowerY, 1},
                            { halfX, upperY, -halfX, upperY, 1},
                            { -halfX, upperY, -halfX, lowerY, 1},
                            { halfX, lowerY, -halfX, lowerY, 1},
                        };
                        this._wrapper.CreateLine(pointsForStep, 0, 4);
                        this._wrapper.Extrusion(1, d);
                    }
                    this._wrapper.CreateOffsetPlane(-offset);
                    this._wrapper.CreateAnglePlane(-PIDiVIDED4ANGLE);
                    this._wrapper.CreateSketch(0);
                    double[,] pointsArraySecond =
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
                    this._wrapper.CreateLine(pointsArraySecond, 0, 8);
                    this._wrapper.Extrusion(1, d);
                    this._wrapper.CreateSketch(3);
                    this._wrapper.CreateLine(pointsArray, 8, 4);
                    this._wrapper.Extrusion(2, x / 2);
                    break;
                }
            }
        }
    }
}
