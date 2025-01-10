using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using BuilderClass;
using UI_layout;
using StressTesting;

namespace LadderPlugin
{   
    /// <summary>
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

            //StressTester stress = new StressTester();
            //stress.StressTesting();
        }

        /// <summary>
        /// Gets or sets AboutForm.
        /// </summary>
        private AboutForm AboutForm { get; set; }

        /// <summary>
        /// Вспомогательный метод для установки цвета для текстБокса.
        /// </summary>
        /// <param name="textBox">Передаваемый текстБокс.</param>
        /// <param name="whatColor">Устанавливаемый цвет.</param>
        /// <param name="text">Текст устанавливаемый в подсказку.</param>
        private void SetColors(
            System.Windows.Forms.TextBox textBox,
            Parameter parameter,
            int whatColor,
            string text)
        {
            switch (whatColor)
            {
                case 1:
                {
                    textBox.BackColor = SystemColors.Window;
                    var message = textBox.Text != string.Empty
                        ? "Доступны только целочисленные значения"
                        : "Введите значения от " +
                          parameter.MinValue.ToString() +
                          " до " + parameter.MaxValue.ToString() +
                          " мм";
                    this.toolTipWarner.SetToolTip(textBox, message);

                    textBox.Text = string.Empty;
                    break;
                }

                case 2:
                {
                    textBox.BackColor = Color.Red;
                    this.toolTipWarner.SetToolTip(textBox, text);
                    break;
                }

                case 3:
                {
                    textBox.BackColor = Color.Green;
                    this.toolTipWarner.SetToolTip(textBox, string.Empty);
                    break;
                }
            }
        }

        /// <summary>
        /// Вторичная валидация, попытка создания параметра, попытка добавления
        /// корректного параметра в словарь.
        /// </summary>
        /// <param name="textBox">Используемый текстБокс.</param>
        /// <param name="parameterType">Тип параметра.</param>
        private void Validate(
            System.Windows.Forms.TextBox textBox,
            ParameterType parameterType)
        {
            //TODO: mistype +
            //TODO: FormatException +
            try
            {
                this._parameters.SetParameter(parameterType, int.Parse(textBox.Text));
                this.SetColors(
                    textBox,
                    this._parameters.AllParameters[parameterType],
                    3,
                    string.Empty);
            }
            catch (FormatException)
            {
                var message = textBox.Text != string.Empty
                    ? "Ошибка"
                    : string.Empty;

                this.SetColors(
                    textBox,
                    this._parameters.AllParameters[parameterType],
                    1,
                    message);
            }
            catch (ArgumentException e)
            {
                switch (e.Message)
                {
                    case "Значение за граничными пределами":
                    {
                        string toolTipText = "Введите значения от " +
                        this._parameters.AllParameters[parameterType].MinValue.ToString() +
                        " до " +
                        this._parameters.AllParameters[parameterType].MaxValue.ToString() +
                        " мм";
                        this.SetColors(
                            textBox,
                            this._parameters.AllParameters[parameterType],
                            2,
                            toolTipText);
                        break;
                    }

                    case "Нарушение в определении граничных условий":
                    {
                        MessageBox.Show(
                            "Критическая ошибка системы!",
                            "Ошибка!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        this.buttonBuild.Enabled = false;
                        break;
                    }

                    default:
                    {
                        this.SetColors(
                            textBox,
                            this._parameters.AllParameters[parameterType],
                            2,
                            e.Message);
                        break;
                    }
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
                this._builder.Build(this._parameters, this.ComboBoxLadderType.SelectedIndex);
            }
        }

        /// <summary>
        /// Обработчик нажатия на кнопку "Подробнее".
        /// </summary>
        /// <param name="sender">Объект.</param>
        /// <param name="e">Аргумент.</param>
        private void ButtonInfo_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["AboutForm"] == null)
            {
                this.AboutForm = new AboutForm();
                this.AboutForm.Show();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.ComboBoxLadderType.SelectedIndex = 0;
            Parameter totalHeight = this._parameters.AllParameters[ParameterType.TotalHeight];
            string toolTipTotalHeightText = "Общая высота лестницы должна быть от " +
                totalHeight.MinValue.ToString() + " до " +
                totalHeight.MaxValue.ToString() + " мм";
            this.toolTipWarner.SetToolTip(this.TextBoxTotalHeight, toolTipTotalHeightText);
            Parameter stepsSpacing = this._parameters.AllParameters[ParameterType.StepsSpacing];
            string toolTipStepsSpacingText = "Расстояние между ступенями должно быть от " +
                stepsSpacing.MinValue.ToString() + " до " +
                stepsSpacing.MaxValue.ToString() + " мм";
            this.toolTipWarner.SetToolTip(this.TextBoxStepsSpacing, toolTipStepsSpacingText);
            Parameter stepsWidth = this._parameters.AllParameters[ParameterType.StepsWidth];
            string toolTipStepsWidthText = "Ширина ступени должна быть от " +
                stepsWidth.MinValue.ToString() + " до " +
                stepsWidth.MaxValue.ToString() + " мм";
            this.toolTipWarner.SetToolTip(this.TextBoxStepsWidth, toolTipStepsWidthText);
            Parameter stepsAmount = this._parameters.AllParameters[ParameterType.StepsAmount];
            string toolTipStepsAmountText = "Количество ступеней должно быть от " +
                stepsAmount.MinValue.ToString() + " до " +
                stepsAmount.MaxValue.ToString() + " мм";
            this.toolTipWarner.SetToolTip(this.TextBoxStepsAmount, toolTipStepsAmountText);
            Parameter materialThickness =
                this._parameters.AllParameters[ParameterType.MaterialThickness];
            string toolTipMaterialThicknessText = "Толщина профиля должна быть от " +
                materialThickness.MinValue.ToString() + " до " +
                materialThickness.MaxValue.ToString() + " мм";
            this.toolTipWarner.SetToolTip(
                this.TextBoxMaterialThickness,
                toolTipMaterialThicknessText);
        }

        /// <summary>
        /// Обработчик выхода из текстБокса "Общая высота H (мм)".
        /// </summary>
        /// <param name="sender">Объект.</param>
        /// <param name="e">Аргумент.</param>
        private void TextBoxTotalHeight_Leave(object sender, EventArgs e)
        {
            ParameterType parameterType = ParameterType.TotalHeight;
            this.Validate(this.TextBoxTotalHeight, parameterType);
            if (this.TextBoxTotalHeight.BackColor != SystemColors.Window)
            {
                this.Validate(this.TextBoxStepsSpacing, ParameterType.StepsSpacing);
                this.Validate(this.TextBoxStepsAmount, ParameterType.StepsAmount);
                this.Validate(this.TextBoxMaterialThickness, ParameterType.MaterialThickness);
            }
        }

        /// <summary>
        /// Обработчик выхода из текстБоксов с 1 зависимым параметром.
        /// </summary>
        /// <param name="sender">Объект.</param>
        /// <param name="e">Аргумент.</param>
        private void TextBoxOneChained_Leave(object sender, EventArgs e)
        {
            var chainedTextBoxesDictionary =
                new Dictionary<TextBox, ParameterType>
            {
                { this.TextBoxStepsAmount, ParameterType.StepsAmount },
                { this.TextBoxStepsSpacing, ParameterType.StepsSpacing },
                { this.TextBoxMaterialThickness, ParameterType.MaterialThickness },
            };
            TextBox textBox = (TextBox)sender;
            ParameterType parameterType = chainedTextBoxesDictionary[textBox];
            this.Validate(textBox, parameterType);
            if (textBox.BackColor != SystemColors.Window)
            {
                this.Validate(this.TextBoxTotalHeight, ParameterType.TotalHeight);
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
            this.Validate(this.TextBoxStepsWidth, parameterType);
        }
    }
}
