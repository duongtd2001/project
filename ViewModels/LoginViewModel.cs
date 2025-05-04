using Caliburn.Micro;
using project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IWindowManager _windowManager;
        private readonly IUserRepository _userRepo;
        private readonly MainViewModel _mainViewModel;

        public LoginViewModel(
            IWindowManager windowManager,
            IUserRepository userRepo,
            MainViewModel mainViewModel)
        {
            _windowManager = windowManager;
            _userRepo = userRepo;
            _mainViewModel = mainViewModel;
        }

        public async Task LoginCommand()
        {
            await TryCloseAsync();
            // Mở MainWindow
            await _windowManager.ShowWindowAsync(_mainViewModel);
           
        }
    }

}
