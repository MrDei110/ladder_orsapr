using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BuilderClass;

namespace LadderPlugin
{   /// <summary>
    /// Класс MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Поле хранящее в себе объект класса Builder.
        /// </summary>
        private Builder _builder = new Builder();

        /// <summary>
        /// Поле хранящее в себе объект класса Parameters.
        /// </summary>
        private Parameters _parameters = new Parameters();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Первичная валидация (проверка на введение в текстБоксы целых чисел.
        /// </summary>
        /// <param name="textBox">ТекстБокс.</param>
        /// <param name="parameterType">Тип параметра.</param>
        private void FirstValidate(
            System.Windows.Forms.TextBox textBox,
            ParameterType parameterType)
        {
            try
            {
                int.Parse(textBox.Text);
                this.SetColors(textBox, parameterType, 3, 0, string.Empty);
            }
            catch
            {
                if (textBox.Text != string.Empty)
                {
                    this.SetColors(textBox, parameterType, 1, 0, "Ошибка");
                }
                else
                {
                    this.SetColors(textBox, parameterType, 1, 0, string.Empty);
                }
            }
        }

        /// <summary>
        /// Вспомогательный метод для установки цвета для текстБокса.
        /// </summary>
        /// <param name="textBox">Передаваемый текстБокс.</param>
        /// <param name="parameterType">Тип параметра.</param>
        /// <param name="whatColor">Устанавливаемый цвет.</param>
        /// <param name="whatReason">Причина установки цвета.</param>
        /// <param name="text">Текст устанавливаемый в подсказку.</param>
        private void SetColors(
            System.Windows.Forms.TextBox textBox,
            ParameterType parameterType,
            int whatColor,
            int whatReason,
            string text)
        {
            Parameter parameter = new Parameter();
            parameter.TypeOfParameter = parameterType;
            switch (whatColor)
            {
                case 1:
                    textBox.BackColor = SystemColors.Window;
                    if (text != string.Empty)
                    {
                        this.toolTipWarner.SetToolTip(textBox, "Доступны только целочисленные значения");
                    }

                    textBox.Text = string.Empty;
                    break;
                case 2:
                    textBox.BackColor = Color.Red;
                    if (whatReason == 0)
                    {
                        string toolTipText = "Введите значения от " +
                            parameter.MinValue.ToString() +
                            " до " + parameter.MaxValue.ToString() +
                            " мм";
                        this.toolTipWarner.SetToolTip(textBox, toolTipText);
                    }
                    else if (whatReason == 1)
                    {
                        this.toolTipWarner.SetToolTip(textBox, text);
                    }

                    break;
                case 3:
                    textBox.BackColor = Color.Green;
                    this.toolTipWarner.SetToolTip(textBox, string.Empty);
                    break;
            }
        }

        /// <summary>
        /// Валидация параметров.
        /// </summary>
        /// <param name="textBox">ТекстБокс.</param>
        /// <param name="parameterType">Тип параметра.</param>
        private void SecondValidate(
            System.Windows.Forms.TextBox textBox,
            ParameterType parameterType)
        {
            bool cached = false;
            Parameter parameter = new Parameter();
            parameter.TypeOfParameter = parameterType;
            try
            {
                parameter.Value = int.Parse(textBox.Text);
            }
            catch (Exception e)
            {
                this.SetColors(textBox, parameterType, 2, 0, e.Message);
                cached = true;
            }

            if (!cached)
            {
                try
                {
                    this._parameters.SetParameter(parameterType, parameter);
                    this.SetColors(textBox, parameterType, 3, 0, string.Empty);
                }
                catch (Exception e)
                {
                    this.SetColors(textBox, parameterType, 2, 1, e.Message);
                }
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Создать".
        /// </summary>
        /// <param name="sender">Объект.</param>
        /// <param name="e">Аргумент.</param>
        private void BuildModel(object sender, EventArgs e)
        {
            if (
                this.TextBoxMaterialThickness.BackColor == Color.Green &&
                this.TextBoxStepsAmount.BackColor == Color.Green &&
                this.TextBoxStepsSpacing.BackColor == Color.Green &&
                this.TextBoxStepsWidth.BackColor == Color.Green &&
                this.TextBoxTotalHeight.BackColor == Color.Green
                )
            {
                this._builder.Build(this._parameters);
            }
        }

        private void buttonInfo_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this._parameters.AllParameters = new Dictionary<ParameterType, Parameter>();
            this.toolTipWarner.SetToolTip(
                this.TextBoxTotalHeight,
                "Общая высота лестницы должна быть от 500 до 9000 мм");
            this.toolTipWarner.SetToolTip(
                this.TextBoxStepsSpacing,
                "Расстояние между ступенями должно быть от 300 до 340 мм");
            this.toolTipWarner.SetToolTip(
                this.TextBoxStepsWidth,
                "Ширина ступени должна быть от 460 до 800 мм");
            this.toolTipWarner.SetToolTip(
                this.TextBoxStepsAmount,
                "Количество ступеней должно быть от 2 до 14 мм");
            this.toolTipWarner.SetToolTip(
                this.TextBoxMaterialThickness,
                "Толщина профиля должна быть от 30 до 55 мм");
        }

        /// <summary>
        /// Обработчик выхода из текстБокса "Общая высота H (мм)".
        /// </summary>
        /// <param name="sender">Объект.</param>
        /// <param name="e">Аргумент.</param>
        private void TextBoxTotalHeight_Leave(object sender, EventArgs e)
        {
            ParameterType parameterType = ParameterType.TotalHeight;
            this.FirstValidate(this.TextBoxTotalHeight, parameterType);
            if (this.TextBoxTotalHeight.BackColor != SystemColors.Window)
            {
                this.SecondValidate(this.TextBoxTotalHeight, parameterType);
                this.FirstValidate(this.TextBoxStepsSpacing, ParameterType.StepsSpacing);
                if (this.TextBoxStepsSpacing.BackColor != SystemColors.Window)
                {
                    this.SecondValidate(this.TextBoxStepsSpacing, ParameterType.StepsSpacing);
                }
                this.FirstValidate(this.TextBoxStepsAmount, ParameterType.StepsAmount);
                if (this.TextBoxStepsAmount.BackColor != SystemColors.Window)
                {
                    this.SecondValidate(this.TextBoxStepsAmount, ParameterType.StepsAmount);
                }
                this.FirstValidate(this.TextBoxMaterialThickness, ParameterType.MaterialThickness);
                if (this.TextBoxMaterialThickness.BackColor != SystemColors.Window)
                {
                    this.SecondValidate(this.TextBoxMaterialThickness, ParameterType.MaterialThickness);
                }
            }
        }

        /// <summary>
        /// Обработчик выхода из текстБокса "Расстояние между ступенями".
        /// </summary>
        /// <param name="sender">Объект.</param>
        /// <param name="e">Аргумент.</param>
        private void TextBoxStepsSpacing_Leave(object sender, EventArgs e)
        {
            ParameterType parameterType = ParameterType.StepsSpacing;
            this.FirstValidate(this.TextBoxStepsSpacing, parameterType);
            if (this.TextBoxStepsSpacing.BackColor != SystemColors.Window)
            {
                this.SecondValidate(this.TextBoxStepsSpacing, parameterType);
                this.FirstValidate(this.TextBoxTotalHeight, ParameterType.TotalHeight);
                if (this.TextBoxTotalHeight.BackColor != SystemColors.Window)
                {
                    this.SecondValidate(this.TextBoxTotalHeight, ParameterType.TotalHeight);
                }
            }
        }

        /// <summary>
        /// Обработчик выхода из текстБокса "Ширина ступени".
        /// </summary>
        /// <param name="sender">Объект.</param>
        /// <param name="e">Аргумент.</param>
        private void TextBoxStepsWidth_Leave(object sender, EventArgs e)
        {
            ParameterType parameterType = ParameterType.StepsWidth;
            this.FirstValidate(this.TextBoxStepsWidth, parameterType);
            if (this.TextBoxStepsWidth.BackColor != SystemColors.Window)
            {
                this.SecondValidate(this.TextBoxStepsWidth, parameterType);
            }
        }

        /// <summary>
        /// Обработчик выхода из текстБокса "Общая высота H (мм)".
        /// </summary>
        /// <param name="sender">Объект.</param>
        /// <param name="e">Аргумент.</param>
        private void TextBoxStepsAmount_Leave(object sender, EventArgs e)
        {
            ParameterType parameterType = ParameterType.StepsAmount;
            this.FirstValidate(this.TextBoxStepsAmount, parameterType);
            if (this.TextBoxStepsAmount.BackColor != SystemColors.Window)
            {
                this.SecondValidate(this.TextBoxStepsAmount, parameterType);
                this.FirstValidate(this.TextBoxTotalHeight, ParameterType.TotalHeight);
                if (this.TextBoxTotalHeight.BackColor != SystemColors.Window)
                {
                    this.SecondValidate(this.TextBoxTotalHeight, ParameterType.TotalHeight);
                }
            }
        }

        /// <summary>
        /// Обработчик выхода из текстБокса "Общая высота H (мм)".
        /// </summary>
        /// <param name="sender">Объект.</param>
        /// <param name="e">Аргумент.</param>
        private void TextBoxMaterialThickness_Leave(object sender, EventArgs e)
        {
            ParameterType parameterType = ParameterType.MaterialThickness;
            this.FirstValidate(this.TextBoxMaterialThickness, parameterType);
            if (this.TextBoxMaterialThickness.BackColor != SystemColors.Window)
            {
                this.SecondValidate(this.TextBoxMaterialThickness, parameterType);
                this.FirstValidate(this.TextBoxTotalHeight, ParameterType.TotalHeight);
                if (this.TextBoxTotalHeight.BackColor != SystemColors.Window)
                {
                    this.SecondValidate(this.TextBoxTotalHeight, ParameterType.TotalHeight);
                }
            }
        }
    }
}
