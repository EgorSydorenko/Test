using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xaml;

namespace BoardControls
{
    public class BoardTextBox : TextBox
    {
        public int TabSize { get; set; }
        public double LineHeight 
        {
            get
            {
                Size sz = MeasureString("X");
                return sz.Height;
            }
            private set { } 
        }

        private int _maximumLines;
        private string _texBeforeChanging;
        private int _caretPosition;

        public BoardTextBox()
        {
            this.AcceptsReturn = true;
            this.AcceptsTab = true;
            this.TextWrapping = TextWrapping.NoWrap;
            this.SnapsToDevicePixels = true;
            this.IsUndoEnabled = false;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Size sz = MeasureString("X");
            this._maximumLines = (int)(this.ActualHeight / sz.Height);
            this.LineHeight = sz.Height;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            this._texBeforeChanging = this.Text;
            this._caretPosition = this.CaretIndex;

            if (e.Key == Key.Tab && this.TabSize != 0)
            {
                int rowIndex = this.GetRowIndex();
                int caretPosition = this.CaretIndex;
                int spaceCount = this.GetSpaceCount(rowIndex);
                string tab = new string(' ', spaceCount);
                this.Text = this.Text.Insert(caretPosition, tab);
                this.CaretIndex = caretPosition + spaceCount;
                e.Handled = true;
            }
            base.OnPreviewKeyDown(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (this.LineCount > this._maximumLines)
            {
                SystemSounds.Beep.Play();
                this.Text = this._texBeforeChanging;
                this.CaretIndex = this._caretPosition;
            }
            base.OnTextChanged(e);
        }

        private Size MeasureString(string text)
        {
            var formattedText = new FormattedText(
                text,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(this.FontFamily, this.FontStyle, this.FontWeight, this.FontStretch),
                this.FontSize,
                Brushes.Black);

            return new Size(formattedText.Width, formattedText.Height);
        }

        private int GetRowIndex()
        {
            if (!String.IsNullOrEmpty(this.Text))
            {
                string txt = this.Text.Substring(0, this.CaretIndex);
                var pMass = txt.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
                return pMass.Last().Length;
            }
            else
                return 0;
        }

        private int GetSpaceCount(int position)
        {
            int mp_pos = position == 0 ? position + 1 : position;
            for (int i = position; ; i++)
            {
                if (mp_pos % this.TabSize == 0)
                    break;
                mp_pos += 1;
            }

            return (mp_pos - position) == 0 ? 4 : (mp_pos - position);
        }
    }
}