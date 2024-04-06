using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;





namespace LOLBearApp
{
    public partial class SettingPannel : UserControl
    {
        private InputPannel inputPanelWindow;
        private bool isDragging = false;
        private Point lastMouseDownPosition;

        public SettingPannel()
        {
            InitializeComponent();
        }

        // 인풋 패널 띄우기
        private void AddAccountBT_Click(object sender, RoutedEventArgs e)
        {
            // ShowInputPanelWindow 메서드 호출
            ShowInputPanelWindow();
        }

        // 입력 패널 창 띄우기
        private void ShowInputPanelWindow()
        {
            // 새로운 윈도우 인스턴스 생성
            Window inputWindow = new Window();

            // 윈도우 스타일 설정
            inputWindow.WindowStyle = WindowStyle.None;
            inputWindow.AllowsTransparency = true;
            inputWindow.Background = Brushes.Transparent;
            inputWindow.Height = 300;
            inputWindow.Width = 520;

            // 윈도우 위치를 화면 중앙으로 설정
            inputWindow.WindowStartupLocation = WindowStartupLocation.Manual;

            // 윈도우의 가로 위치를 화면 중앙으로 설정
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double windowLeft = (screenWidth - inputWindow.Width) / 2;
            inputWindow.Left = windowLeft;

            // 윈도우의 세로 위치를 화면 중앙보다 약간 위쪽으로 설정
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowTop = (screenHeight * 0.5) - (inputWindow.Height * 1.3); // 중앙보다 약간 위쪽으로 설정
            inputWindow.Top = windowTop;



            // 윈도우 내에 유저 컨트롤을 추가
            AccountManage accountManager = new AccountManage();
            inputPanelWindow = new InputPannel(accountManager);
            inputWindow.Content = inputPanelWindow;

            // 윈도우를 보이게 함
            inputWindow.Show();

            // 윈도우의 마우스 이벤트 핸들러 등록
            inputWindow.MouseLeftButtonDown += InputWindow_MouseLeftButtonDown;
            inputWindow.MouseLeftButtonUp += InputWindow_MouseLeftButtonUp;
            inputWindow.MouseMove += InputWindow_MouseMove;
        }


        // 마우스 왼쪽 버튼이 눌렸을 때
        private void InputWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            lastMouseDownPosition = e.GetPosition(sender as Window);
        }

        // 마우스 왼쪽 버튼이 떼어졌을 때
        private void InputWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }

        // 마우스가 움직일 때
        private void InputWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Window window = sender as Window;
                Point currentPosition = e.GetPosition(window);
                Vector diff = currentPosition - lastMouseDownPosition;

                window.Left += diff.X;
                window.Top += diff.Y;
            }
        }

    }
}
