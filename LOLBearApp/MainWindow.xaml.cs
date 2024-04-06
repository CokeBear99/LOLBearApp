using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

namespace LOLBearApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>


    public partial class MainWindow : Window
    {

        private bool isMaximized = false;
        private UserControl currentPanel;


        public MainWindow()
        {
            InitializeComponent();
            // 홈 패널을 초기화
            currentPanel = new HomePannel();
            MainSection.Children.Add(currentPanel);
            currentPanel.Width = double.NaN; // 원래 너비로 복원
            currentPanel.Height = double.NaN; // 원래 높이로 복원

        }





        //창 드래그
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        //트레이 
        private void TrayButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.MainWindow.Hide();
        }


        //창 최소화
        private void Minimized_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.MainWindow.WindowState = System.Windows.WindowState.Minimized;
        }

        //창 최대화 및 복원

        private void ToggleMaximize()
        {
            if (isMaximized)
            {
                RestoreWindow();
            }
            else
            {
                MaximizeWindow();
            }
        }


        private void MaximizeWindow()
        {
            // 현재 패널을 최대화합니다.
            currentPanel.VerticalAlignment = VerticalAlignment.Stretch;
            WindowState = WindowState.Maximized;
            isMaximized = true;
        }

        private void RestoreWindow()
        {
            // 현재 패널을 복원합니다.
            currentPanel.Width = double.NaN; // 원래 너비로 복원
            currentPanel.Height = double.NaN; // 원래 높이로 복원
            WindowState = WindowState.Normal;
            isMaximized = false;
        }


        private void Maxmized_Click(object sender, RoutedEventArgs e)
        {
            ToggleMaximize();
        }


        //종료버튼
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
            // 여기에 로그아웃에 필요한 로직을 추가하세요.
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // 현재 창을 닫습니다. (유저 컨트롤을 포함하는 부모 창)
            Window.GetWindow(this).Close();
        }



        //패널 전환 버튼 모음 
        private void IronBT_Click(object sender, RoutedEventArgs e)
        {
            // MainSection에 있는 현재 패널을 제거합니다.
            MainSection.Children.Clear();

            // BronzePannel을 생성하고 MainSection에 추가하여 표시합니다.
            IronPannel IronPN = new IronPannel();
            MainSection.Children.Add(IronPN);

            // 현재 패널을 BronzePannel로 업데이트합니다.
            currentPanel = IronPN;
            currentPanel.Width = double.NaN; // 원래 너비로 복원
            currentPanel.Height = double.NaN; // 원래 높이로 복원
        }

        private void BronzeBT_Click(object sender, RoutedEventArgs e)
        {
            MainSection.Children.Clear();

            BronzePannel BronzePN = new BronzePannel();
            MainSection.Children.Add(BronzePN);

            currentPanel = BronzePN;
            currentPanel.Width = double.NaN;
            currentPanel.Height = double.NaN;
        }



        private void SilverBT_Click(object sender, RoutedEventArgs e)
        {
            MainSection.Children.Clear();

            SilverPannel SilverPN = new SilverPannel();
            MainSection.Children.Add(SilverPN);

            currentPanel = SilverPN;
            currentPanel.Width = double.NaN;
            currentPanel.Height = double.NaN;
        }


        private void GoldBT_Click(object sender, RoutedEventArgs e)
        {
            MainSection.Children.Clear();

            GoldPannel GoldPN = new GoldPannel();
            MainSection.Children.Add(GoldPN);

            currentPanel = GoldPN;
            currentPanel.Width = double.NaN;
            currentPanel.Height = double.NaN;
        }


        private void PlatinumBT_Click(object sender, RoutedEventArgs e)
        {
            MainSection.Children.Clear();

            PlatinumPannel PlatinumPN = new PlatinumPannel();
            MainSection.Children.Add(PlatinumPN);

            currentPanel = PlatinumPN;
            currentPanel.Width = double.NaN;
            currentPanel.Height = double.NaN;
        }


        private void EmeraldBT_Click(object sender, RoutedEventArgs e)
        {
            MainSection.Children.Clear();

            EmeraldPannel EmeraldPN = new EmeraldPannel();
            MainSection.Children.Add(EmeraldPN);

            currentPanel = EmeraldPN;
            currentPanel.Width = double.NaN;
            currentPanel.Height = double.NaN;
        }



        private void DiamondBT_Click(object sender, RoutedEventArgs e)
        {
            MainSection.Children.Clear();

            DiamondPannel DiamondPN = new DiamondPannel();
            MainSection.Children.Add(DiamondPN);

            currentPanel = DiamondPN;
            currentPanel.Width = double.NaN;
            currentPanel.Height = double.NaN;
        }


        private void HomeBT_Click(object sender, RoutedEventArgs e)
        {
            MainSection.Children.Clear();

            HomePannel HomePN = new HomePannel();
            MainSection.Children.Add(HomePN);

            currentPanel = HomePN;
            currentPanel.Width = double.NaN;
            currentPanel.Height = double.NaN;
        }


        private void SettingBT_Click(object sender, RoutedEventArgs e)
        {
            MainSection.Children.Clear();

            SettingPannel SettingPN = new SettingPannel();
            MainSection.Children.Add(SettingPN);

            currentPanel = SettingPN;
            currentPanel.Width = double.NaN;
            currentPanel.Height = double.NaN;
        }













    }
}