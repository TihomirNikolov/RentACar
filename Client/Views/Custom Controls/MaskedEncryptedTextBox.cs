using Client.Helpers;
using System.Windows;
using System.Windows.Controls;

namespace Client.Views.Custom_Controls
{
    public class MaskedEncryptedTextBox : TextBox
    {
        #region Declarations

        private string text = "";

        #endregion

        #region Properties

        #region DependencyProperties

        public static readonly DependencyProperty EncryptedTextProperty = DependencyProperty.Register(nameof(EncryptedText),
                                                                                             typeof(string),
                                                                                             typeof(MaskedEncryptedTextBox),
                                                                                             new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register(nameof(Mask),
                                                                                             typeof(char),
                                                                                             typeof(MaskedEncryptedTextBox));

        public static readonly DependencyProperty IsMaskedProperty = DependencyProperty.Register(nameof(IsMasked),
                                                                                                 typeof(bool),
                                                                                                 typeof(MaskedEncryptedTextBox),
                                                                                                 new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIsMaskedPropertyChanged)));

        #endregion

        public string EncryptedText
        {
            get { return (string)GetValue(EncryptedTextProperty); }
            set { SetValue(EncryptedTextProperty, value); }
        }

        public char Mask
        {
            get { return (char)GetValue(MaskProperty); }
            set { SetValue(MaskProperty, value); }
        }

        public bool IsMasked
        {
            get { return (bool)GetValue(IsMaskedProperty); }
            set { SetValue(IsMaskedProperty, value); }
        }

        #endregion

        #region Methods

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name == nameof(Text) && Text != new string(Mask, text.Length))
            {
                if(IsMasked)
                {
                    if (Text.Length < text.Length)
                    {
                        text = text.Substring(0, Text.Length);
                        Text = new string(Mask, text.Length);
                    }
                    else if (Text.Length > text.Length)
                    {
                        if(Text.Length - text.Length > 1)
                        {
                            text += Text.Substring(text.Length, Text.Length - text.Length - 1);
                        }
                        text += Text[^1];
                        Text = new string(Mask, text.Length);
                    }
                }
                else
                {
                    text = Text;
                }
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            EncryptedText = CryptHelper.Instance.Encrypt(text);
        }

        private static void OnIsMaskedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (MaskedEncryptedTextBox)d;

            if (!(bool)e.NewValue)
            {
                textBox.Text = textBox.text;
            }
            else
            {
                textBox.Text = new string(textBox.Mask, textBox.text.Length);
            }
        }

        #endregion
    }
}
