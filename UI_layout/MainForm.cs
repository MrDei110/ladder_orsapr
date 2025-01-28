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
            Color whatColor,
            string text)
        {
            textBox.BackColor = whatColor;
            var message = textBox.Text != string.Empty
                    ? "Доступны только целочисленные значения"
                    : text;
            this.toolTipWarner.SetToolTip(textBox, message);

            if (whatColor == SystemColors.Window)
            {
                textBox.Text = string.Empty;
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
            try
            {
                this._parameters.SetParameter(parameterType, int.Parse(textBox.Text));
                this.SetColors(
                    textBox,
                    Color.Green,
                    string.Empty);
            }
            catch (MinMaxException)
            {
                MessageBox.Show(
                            "Критическая ошибка системы!",
                            "Ошибка!",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                this.buttonBuild.Enabled = false;
            }
            catch(Exception ex)
            {
                var exceptionDictionary = new Dictionary<System.Type, (Color color, string text)>
                {
                    { typeof(FormatException),
                        (Color.Gainsboro,
                        this.RangeTextCaster(this._parameters.AllParameters[parameterType])) },
                    { typeof(ParametersException), (Color.Crimson, ex.Message) },
                    { typeof(ValueException),
                        (Color.Crimson,
                        this.RangeTextCaster(this._parameters.AllParameters[parameterType])) }
                };
                var chained = exceptionDictionary[ex.GetType()];
                this.SetColors(
                    textBox,
                    chained.color,
                    chained.text);
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

        /// <summary>
        /// Метод, вызываемый при загрузке MainForm.
        /// </summary>
        /// <param name="sender">Объект.</param>
        /// <param name="e">Аргумент.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.ComboBoxLadderType.SelectedIndex = 0;
            string toolTipTotalHeightText =
                this.RangeTextCaster(this._parameters.AllParameters[ParameterType.TotalHeight]);
            this.toolTipWarner.SetToolTip(this.TextBoxTotalHeight, toolTipTotalHeightText);
            string toolTipStepsSpacingText =
                this.RangeTextCaster(this._parameters.AllParameters[ParameterType.StepsSpacing]);
            this.toolTipWarner.SetToolTip(this.TextBoxStepsSpacing, toolTipStepsSpacingText);
            string toolTipStepsWidthText =
                this.RangeTextCaster(this._parameters.AllParameters[ParameterType.StepsWidth]);
            this.toolTipWarner.SetToolTip(this.TextBoxStepsWidth, toolTipStepsWidthText);
            string toolTipStepsAmountText =
                this.RangeTextCaster(this._parameters.AllParameters[ParameterType.StepsAmount]);
            this.toolTipWarner.SetToolTip(this.TextBoxStepsAmount, toolTipStepsAmountText);
            string toolTipMaterialThicknessText =
                this.RangeTextCaster(
                    this._parameters.AllParameters[ParameterType.MaterialThickness]);
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

        /// <summary>
        /// Вспомогательный метод для генерации текста граничных условий.
        /// </summary>
        /// <param name="parameter">Передаваемый параметр.</param>
        /// <returns>Текст для подсказки.</returns>
        private string RangeTextCaster(Parameter parameter)
        {
            return "Введите значение от " +
                parameter.MinValue.ToString() +
                " до " +
                parameter.MaxValue.ToString() +
                " мм";
        }
    }
}
