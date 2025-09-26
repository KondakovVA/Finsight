using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Finsight.Client.AppFrame.CustomControls
{
    public class MaskedTextBox : TextBox
    {
        private readonly char PromptChar = '_';
        public static readonly DependencyProperty MaskProperty =
            DependencyProperty.Register("Mask", typeof(string), typeof(MaskedTextBox), new PropertyMetadata(""));

        public string Mask
        {
            get => (string)GetValue(MaskProperty);
            set => SetValue(MaskProperty, value);
        }

        public string RawValue => new string(_digits.ToArray());

        private readonly List<char> _digits = new();

        public MaskedTextBox()
        {
            PreviewTextInput += OnPreviewTextInput;
            PreviewKeyDown += OnPreviewKeyDown;
            DataObject.AddPastingHandler(this, OnPaste);
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.All(char.IsDigit))
            {
                _digits.AddRange(e.Text);
                RefreshTextAndCaret(_digits.Count); // каретка в конец
            }
            e.Handled = true; // блокируем прямой ввод
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var (startDigit, endDigit) = SelectionToDigitRange();
            bool hasSelection = endDigit > startDigit;

            if (e.Key == Key.Back)
            {
                if (hasSelection)
                {
                    // удаляем выделенный диапазон цифр
                    _digits.RemoveRange(startDigit, endDigit - startDigit);
                    RefreshTextAndCaret(startDigit);
                }
                else
                {
                    // нет выделения: удаляем цифру слева от каретки (если есть)
                    if (startDigit > 0)
                    {
                        _digits.RemoveAt(startDigit - 1);
                        RefreshTextAndCaret(startDigit - 1);
                    }
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Delete)
            {
                if (hasSelection)
                {
                    _digits.RemoveRange(startDigit, endDigit - startDigit);
                    RefreshTextAndCaret(startDigit);
                }
                else
                {
                    // нет выделения: удаляем цифру под кареткой (если есть)
                    if (startDigit < _digits.Count)
                    {
                        _digits.RemoveAt(startDigit);
                        RefreshTextAndCaret(startDigit);
                    }
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Left)
            {
                // шаг по «цифровым» позициям
                int newDigitPos = Math.Max(0, startDigit - 1);
                // двигаем каретку к соответствующей визуальной позиции
                SelectionStart = DigitIndexToCaret(newDigitPos);
                SelectionLength = 0;
                e.Handled = true;
            }
            else if (e.Key == Key.Right)
            {
                int newDigitPos = Math.Min(_digits.Count, startDigit + 1);
                SelectionStart = DigitIndexToCaret(newDigitPos);
                SelectionLength = 0;
                e.Handled = true;
            }
            // Остальные клавиши — по умолчанию
        }

        private void RefreshTextAndCaret(int targetDigitIndex)
        {
            // Собираем строку по маске
            var sb = new StringBuilder();
            int di = 0; // индекс в _digits

            foreach (char c in Mask)
            {
                if (c == '0')
                {
                    if (di < _digits.Count)
                        sb.Append(_digits[di++]);
                    else
                        sb.Append(PromptChar); // например '_'
                }
                else
                {
                    sb.Append(c);
                }
            }

            string newText = sb.ToString();
            if (Text != newText)
                Text = newText;

            // Ставим каретку после обновления layout, иначе WPF «залипает» в начале
            int caretVisualIndex = DigitIndexToCaret(targetDigitIndex);
            Dispatcher.BeginInvoke(new Action(() =>
            {
                SelectionStart = Math.Min(caretVisualIndex, Text.Length);
                SelectionLength = 0;
            }), System.Windows.Threading.DispatcherPriority.Background);
        }


        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);

            // Дадим WPF поставить каретку, затем поправим на ближайшую «цифровую» позицию
            Dispatcher.BeginInvoke(new Action(() =>
            {
                int digitPos = CaretToDigitIndex(SelectionStart);
                SelectionStart = DigitIndexToCaret(digitPos);
                SelectionLength = 0;
            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                foreach (var ch in text.Where(char.IsDigit))
                    _digits.Add(ch);
                RefreshText();
            }
            e.CancelCommand();
        }

        // === Хелперы: маппинг каретки ===

        // сколько «нулей» (редактируемых позиций) встретилось до визуального индекса
        private int CaretToDigitIndex(int caretVisualIndex)
        {
            int digitPos = 0;
            for (int i = 0; i < Mask.Length && i < caretVisualIndex; i++)
                if (Mask[i] == '0') digitPos++;
            return Math.Clamp(digitPos, 0, _digits.Count);
        }

        // визуальная позиция каретки по «цифровому» индексу
        private int DigitIndexToCaret(int digitIndex)
        {
            if (digitIndex <= 0)
            {
                int firstZero = Mask.IndexOf('0');
                return firstZero >= 0 ? firstZero : 0;
            }

            int seen = 0;
            for (int i = 0; i < Mask.Length; i++)
            {
                if (Mask[i] == '0')
                {
                    if (seen == digitIndex) return i; // позиция ПЕРЕД этой ячейкой
                    seen++;
                }
            }
            return Mask.Length; // после последней цифры
        }

        // диапазон цифр, соответствующий выделению (SelectionStart..SelectionStart+SelectionLength)
        private (int startDigit, int endDigit) SelectionToDigitRange()
        {
            int selStart = SelectionStart;
            int selEnd = SelectionStart + SelectionLength;
            int startDigit = CaretToDigitIndex(selStart);
            int endDigit = CaretToDigitIndex(selEnd);
            return (startDigit, endDigit);
        }


        private void RefreshText()
        {
            int oldCaret = CaretIndex;

            var result = new StringBuilder();
            int digitIndex = 0;

            foreach (char c in Mask)
            {
                if (c == '0')
                {
                    if (digitIndex < _digits.Count)
                        result.Append(_digits[digitIndex++]);
                    else
                        result.Append('_');
                }
                else
                {
                    result.Append(c);
                }
            }

            Text = result.ToString();

            // возвращаем курсор максимально близко к старой позиции
            CaretIndex = Math.Min(oldCaret, Text.Length);
        }

    }

}
