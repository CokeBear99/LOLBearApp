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
    public partial class InputPannel : UserControl
    {

        private AccountManage accountManager;

        public InputPannel(AccountManage manager)
        {
            InitializeComponent();
            accountManager = manager;
        }

        private async void AddCheckBT_Click(object sender, RoutedEventArgs e)
        {
            // 입력된 정보를 딕셔너리에 추가
            string nickname = NickNameInputBox.Text;
            string id = IDInputBox.Text;
            string password = PWInputBox.Text;

            // accountManager.AddAccount 호출을 비동기적으로 처리
            await accountManager.AddAccount(nickname, id, password);

            // 추가 작업 수행
        }






        //창 닫기
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // 현재 창을 닫습니다.
            Window.GetWindow(this).Close();
        }

        // 창 최소화
        private void Minimized_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }








    }
}
