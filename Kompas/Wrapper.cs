using System;
using System.Runtime.InteropServices;
using Kompas6API5;
using Kompas6Constants3D;
using KompasAPI7;
using KompasLibrary;

namespace Kompas
{
    /// <summary>
    /// Класс для работы с API Компас.
    /// </summary>
    public class Wrapper
    {
        /// <summary>
        /// Поле для хранения приложения Компас.
        /// </summary>
        private KompasObject _kompas;

        /// <summary>
        /// Поле для хранения выбранной 3d детали.
        /// </summary>
        private Kompas6API5.ksPart _part;

        /// <summary>
        /// Поле для хранения выбранного эскиза.
        /// </summary>
        private Kompas6API5.ksEntity _sketchEntity;

        /// <summary>
        /// Поле для хранения выбранной плоскости.
        /// </summary>
        private Kompas6API5.ksEntity _plane;


        /// <summary>
        /// Создание смещённой плоскости.
        /// </summary>
        /// <param name="offset">Сдвиг.</param>
        public void CreateOffsetPlane(double offset)
        {
            this._plane = (ksEntity)this._part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition planeDef = (ksPlaneOffsetDefinition)this._plane.GetDefinition();
            ksEntity basePlane = (ksEntity)this._part.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);
            planeDef.offset = offset;
            planeDef.SetPlane(basePlane);
            planeDef.direction = true;
            this._plane.Create();
        }

        /// <summary>
        /// Создание отклонённой плоскости.
        /// </summary>
        /// <param name="angle">Угол.</param>
        public void CreateAnglePlane(double angle)
        {
            ksEntity newPlane = (ksEntity)this._part.NewEntity((short)Obj3dType.o3d_planeAngle);
            ksPlaneAngleDefinition planeDef = (ksPlaneAngleDefinition)newPlane.GetDefinition();
            ksEntity axis = (ksEntity)this._part.GetDefaultEntity((short)Obj3dType.o3d_axisOX);
            planeDef.angle = angle;
            planeDef.SetAxis(axis);
            planeDef.SetPlane(this._plane);
            newPlane.Create();
            this._plane = newPlane;
        }

        /// <summary>
        /// Создание эскиза в компасе.
        /// </summary>
        /// <param name="perspective">Выбранная плоскость.</param>
        public void CreateSketch(int perspective)
        {
            this._sketchEntity = (ksEntity)this._part.NewEntity((short)Obj3dType.o3d_sketch);
            ksSketchDefinition sketchDef = (ksSketchDefinition)this._sketchEntity.GetDefinition();
            switch (perspective)
            {
                case 1:
                {
                    this._plane = (ksEntity)this._part.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);
                    break;
                }

                case 2:
                {
                    this._plane = (ksEntity)this._part.GetDefaultEntity((short)Obj3dType.o3d_planeXOZ);
                    break;
                }

                case 3:
                {
                    this._plane = (ksEntity)this._part.GetDefaultEntity((short)Obj3dType.o3d_planeYOZ);
                    break;
                }
            }

            sketchDef.SetPlane(this._plane);
            this._sketchEntity.Create();
        }

        /// <summary>
        /// Создание линии в компасе.
        /// </summary>
        /// <param name="pointsArray">Массив точек по которым строятся линии.</param>
        /// <param name="start">Стартовый индекс массива.</param>
        /// <param name="count">Количество считываемых строк из массива.</param>
        public void CreateLine(double[,] pointsArray, int start, int count)
        {
            ksSketchDefinition sketchDef = (ksSketchDefinition)this._sketchEntity.GetDefinition();
            ksDocument2D document2D = (ksDocument2D)sketchDef.BeginEdit();
            if (document2D != null)
            {
                for (int i = start; i < start + count; i++)
                {
                    document2D.ksLineSeg(
                        pointsArray[i, 0],
                        pointsArray[i, 1],
                        pointsArray[i, 2],
                        pointsArray[i, 3],
                        (int)pointsArray[i, 4]);
                }

                sketchDef.EndEdit();
            }
        }

        /// <summary>
        /// Выдавливание в компасе.
        /// </summary>
        /// <param name="parameter">Метод выдавливания.</param>
        /// <param name="length">Глубина выдавливания.</param>
        public void Extrusion(int type, double length)
        {
            switch (type)
            {
                case 1:
                {
                    BossExtrusion(length);
                    break;
                }

                case 2:
                {
                    BothBossExtrusion(length);
                    break;
                }
            }
        }

        /// <summary>
        /// Открытие компаса.
        /// </summary>
        public void OpenCAD()
        {
            try
            {
                this._kompas = (KompasObject)Marshal.GetActiveObject("KOMPAS.Application.5");
                Console.WriteLine("Kompas3D уже запущен.");
            }
            catch
            {
                Type kompasType = Type.GetTypeFromProgID("KOMPAS.Application.5");
                this._kompas = (KompasObject)Activator.CreateInstance(kompasType);
                Console.WriteLine("Запущен новый экземпляр Kompas3D.");
            }

            if (this._kompas != null)
            {
                this._kompas.Visible = true;
                this._kompas.ActivateControllerAPI();
                Console.WriteLine("Kompas3D успешно запущен и доступен.");
            }
            else
            {
                Console.WriteLine("Не удалось запустить Kompas3D.");
            }

            Console.ReadLine();
        }

        /// <summary>
        /// Создание документа в компасе.
        /// </summary>
        public void CreateFile()
        {
            ksDocument3D document3D = (ksDocument3D)this._kompas.Document3D();
            document3D.Create();
            this._part = (ksPart)document3D.GetPart((short)Part_Type.pTop_Part);
        }

        /// <summary>
        /// Выдавливание.
        /// </summary>
        /// <param name="length">Глубина выдавливания.</param>
        private void BossExtrusion(double length)
        {
            ksEntity entityExtrusion =
                (ksEntity)this._part.NewEntity((short)Obj3dType.o3d_bossExtrusion);
            if (entityExtrusion != null)
            {
                ksBossExtrusionDefinition extrusionDef =
                    (ksBossExtrusionDefinition)entityExtrusion.GetDefinition();
                if (extrusionDef != null)
                {
                    ksExtrusionParam extrusionProp =
                        (ksExtrusionParam)extrusionDef.ExtrusionParam();
                    ksThinParam thinProp = (ksThinParam)extrusionDef.ThinParam();
                    if (extrusionProp != null && thinProp != null)
                    {
                        extrusionDef.SetSketch(this._sketchEntity);

                        extrusionProp.direction = (short)Direction_Type.dtNormal;
                        extrusionProp.typeNormal = (short)End_Type.etBlind;
                        extrusionProp.depthNormal = length;

                        thinProp.thin = false;

                        entityExtrusion.Create();
                    }
                }
            }
        }

        /// <summary>
        /// Выдавливание в обе стороны.
        /// </summary>
        /// <param name="length">Глубина выдавливания.</param>
        private void BothBossExtrusion(double length)
        {
            ksEntity entityExtrusion =
                (ksEntity)this._part.NewEntity((short)Obj3dType.o3d_bossExtrusion);
            if (entityExtrusion != null)
            {
                ksBossExtrusionDefinition extrusionDef =
                    (ksBossExtrusionDefinition)entityExtrusion.GetDefinition();
                if (extrusionDef != null)
                {
                    ksThinParam thinProp = (ksThinParam)extrusionDef.ThinParam();
                    extrusionDef.SetSketch(this._sketchEntity);

                    extrusionDef.directionType = (short)Direction_Type.dtBoth;
                    extrusionDef.SetSideParam(
                        true,
                        (short)End_Type.etBlind,
                        length,
                        0,
                        false);
                    extrusionDef.SetSideParam(
                        false,
                        (short)End_Type.etBlind,
                        length,
                        0,
                        false);

                    thinProp.thin = false;

                    entityExtrusion.Create();
                }
            }
        }
    }
}
