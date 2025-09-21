using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KameraSteuerungDeLuxe;

public class ManualControlViewModel
{
    private AppSettings _settings;

    public ManualControlViewModel(AppSettings settings)
    {
        _settings = settings;
        UpCommand = new RelayCommand(_ => HttpHelper.CameraMove(_settings.CameraIP, HttpHelper.MoveCommand.up));
        DownCommand = new RelayCommand(_ => HttpHelper.CameraMove(_settings.CameraIP, HttpHelper.MoveCommand.down));
        LeftCommand = new RelayCommand(_ => HttpHelper.CameraMove(_settings.CameraIP, HttpHelper.MoveCommand.left));
        RightCommand = new RelayCommand(_ => HttpHelper.CameraMove(_settings.CameraIP, HttpHelper.MoveCommand.right));
        ZoomInCommand = new RelayCommand(_ => HttpHelper.CameraMove(_settings.CameraIP, HttpHelper.MoveCommand.zoomin));
        ZoomOutCommand = new RelayCommand(_ => HttpHelper.CameraMove(_settings.CameraIP, HttpHelper.MoveCommand.zoomout));
    }

    public RelayCommand UpCommand { get; }
    public RelayCommand DownCommand { get; }
    public RelayCommand LeftCommand { get; }
    public RelayCommand RightCommand { get; }
    public RelayCommand ZoomInCommand { get; }
    public RelayCommand ZoomOutCommand { get; }

}




