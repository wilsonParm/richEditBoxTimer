//#define USE_TEXTBLOCK
//#define USE_RICHEDITBOX
#define USE_TEXTBOX

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板


namespace richEditBoxTimer
{


    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        DispatcherTimer timer;
        string InputStr = "";
        int InputLen = 0;

        public MainPage()
        {
            this.InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            timer.Tick += Timer_Tick;//每秒触发这个事件，以刷新指针
            timer.Start();

        }

        int outputLen = 0;
        string get_inputStr = "";
        private void Timer_Tick(object sender, object e)
        {
            string inputStr = "=======----------------------------------------------------------------------------------------------------------------------------------------------------========input string len=" + outputLen + "===========================================================================================================================================\r\n";
            outputLen += inputStr.Length;
            Debug.WriteLine("outputLen:" + outputLen);
            get_inputStr = inputStr;

            //this.xTB_SerialRec.Focus(FocusState.Programmatic);

            //this.xRTB_serialRec.Document.Selection.Move(Windows.UI.Text.TextRangeUnit.Paragraph, outputLen);//光标移动到最后
            //this.xRTB_serialRec.Document.Selection.TypeText(get_inputStr);//在光标处输入

            UpdateUI(inputStr, inputStr.Length);
        }

        private async void UpdateUI(string inputStr, int inputLen)
        {
            await Task.Run(() => {
                InputStr += inputStr;
                InputLen += inputLen;
            });

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                /* 备份
                //this.xRTB_serialRec.Document.SetText(Windows.UI.Text.TextSetOptions.None, get_inputStr);//覆盖文本内容

                //this.xRTB_serialRec.Focus(FocusState.Programmatic);//聚焦哦
                //this.xRTB_serialRec.Document.Selection.EndKey(Windows.UI.Text.TextRangeUnit.Line, false);

                //this.xRTB_serialRec.Document.Selection.SetRange(outputLen, outputLen);
                //this.xRTB_serialRec.Document.Selection.EndKey(Windows.UI.Text.TextRangeUnit.Line, false);
                */

#if SHOW_VIEW_STYLE
                this.xREB.Document.Selection.SetRange(outputLen, outputLen);
                //this.xRTB_serialRec.Document.Selection.Move(Windows.UI.Text.TextRangeUnit.Paragraph, outputLen);//光标移动到最后
                //this.xRTB_serialRec.Document.Selection.TypeText(get_inputStr);//在光标处输入

                this.xREB.Document.Selection.Text = get_inputStr;

                this.xREB.Document.Selection.SetRange(outputLen, outputLen);
                //this.xRTB_serialRec.Document.Selection.Move(Windows.UI.Text.TextRangeUnit.Paragraph, this.serialVM.InputLen);//光标移动到最后
                this.xREB.Document.GetRange(0, outputLen).ScrollIntoView(PointOptions.None);//这个在鼠标点击richeditBox后刷新会变得显示很卡顿，这时候得让这段失效
#endif
#if USE_TEXTBLOCK
                Run run = new Run();
                run.Text = inputStr;
                this.xTBlock.Inlines.Add(run);
#endif

#if USE_TEXTBOX
                //this.xTbox.SelectionStart = InputLen;
                //this.xTbox.SelectionLength = 0;
                //this.xTbox.SelectedText = inputStr;
                this.xTbox.Text = InputStr;
#endif


            });
        }

    }
}
