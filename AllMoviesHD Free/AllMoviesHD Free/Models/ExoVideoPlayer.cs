using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MoviesHD.Models
{
    public enum PlayerState
    {
        Idle = 1,
        Buffering = 2,
        Ready = 3,
        Ended = 4
    }
    public interface IPlayerRenderListener
    {
        event Action<bool> LoadingChanged;
        event Action<ExoVideoPlayerPlaybackParameters> PlaybackParametersChanged;
        event Action<Exception> PlayerError;
        event Action<bool, PlayerState> PlayerStateChanged;
        event Action<int> PositionDiscontinuity;
        event Action<int> RepeatModeChanged;
        event Action SeekProcessed;
        event Action<bool> ShuffleModeEnabledChanged;
        event Action RenderedFirstFrame;
        event Action<int, int, int, float> VideoSizeChanged;
    }
    public class ExoVideoPlayerPlaybackParameters
    {
        public float Pitch { get; set; }
        public bool SkipSilence { get; set; }
        public float Speed { get; set; }
    }

    public class ExoVideoPlayer : Xamarin.Forms.View
    {
        public event Action<bool> FullscreenToggled;
        public Action FullscreenSetFromControl;

        public event Action SeekProcessed;
        public event Action<Exception> PlayerError;
        public Action IndicatorDown { get; set; }

        private bool _IsLoading;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { _IsLoading = value; OnPropertyChanged(); }
        }

        private bool _IsFullScreen;
        public bool IsFullScreen
        {
            get { return _IsFullScreen; }
            set { _IsFullScreen = value; OnPropertyChanged(); FullscreenSetFromControl?.Invoke(); }
        }

        public void ToggleFullscreen()
        {
            _IsFullScreen = !_IsFullScreen;
            OnPropertyChanged("IsFullScreen");
            FullscreenToggled?.Invoke(IsFullScreen);
        }

        private PlayerState _State;
        public PlayerState State
        {
            get { return _State; }
            set
            { _State = value;
                OnPropertyChanged();
            }
        }


        private IPlayerRenderListener _Renderer;
        public IPlayerRenderListener Renderer
        {
            get { return _Renderer; }
            set
            {
                _Renderer = value;
                _Renderer.LoadingChanged += OnLoadingChanged;
                _Renderer.PlaybackParametersChanged += OnPlaybackParametersChanged;
                _Renderer.PlayerError += OnPlayerError;
                _Renderer.PlayerStateChanged += OnPlayerStateChanged;
                _Renderer.PositionDiscontinuity += OnPositionDiscontinuity;
                _Renderer.RenderedFirstFrame += OnRenderedFirstFrame;
                _Renderer.RepeatModeChanged += OnRepeatModeChanged;
                _Renderer.SeekProcessed += OnSeekProcessed;
                _Renderer.ShuffleModeEnabledChanged += OnShuffleModeEnabledChanged;
                _Renderer.VideoSizeChanged += OnVideoSizeChanged;

            }
        }


        public event Action DoubleClick;

        public void RaiseDoubleClick()
        {
            DoubleClick?.Invoke();
        }
        public static readonly BindableProperty SourceUrlProperty =
            BindableProperty.Create("SourceUrl", typeof(string), typeof(ExoVideoPlayer));

        public string SourceUrl
        {
            get => (string)GetValue(SourceUrlProperty);
            set => SetValue(SourceUrlProperty, value);
        }
        public Action Release;
        public Action<string, string> Play;
        public Action<string, string> PlayHls;

        protected virtual void OnLoadingChanged(bool isLoading)
        {
            IsLoading = isLoading;
        }

        protected virtual void OnPlaybackParametersChanged(ExoVideoPlayerPlaybackParameters p0)
        {

        }

        protected virtual void OnPlayerError(Exception e)
        {
            string error = e.Message;
            PlayerError(new Exception(error));
        }

        protected virtual void OnPlayerStateChanged(bool p0, PlayerState p1)
        {
            State = p1;
        }

        protected virtual void OnPositionDiscontinuity(int p0)
        {

        }

        protected virtual void OnRepeatModeChanged(int p0)
        {

        }

        protected virtual void OnSeekProcessed()
        {
            SeekProcessed?.Invoke();
        }

        protected virtual void OnShuffleModeEnabledChanged(bool p0)
        {

        }



        protected virtual void OnRenderedFirstFrame()
        {

        }

        protected virtual void OnVideoSizeChanged(int p0, int p1, int p2, float p3)
        {

        }
    }
}
