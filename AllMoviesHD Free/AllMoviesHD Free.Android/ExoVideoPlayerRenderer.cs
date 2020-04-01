using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoviesHD.Droid;
using MoviesHD.Models;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Google.Android.Exoplayer2;
using Com.Google.Android.Exoplayer2.Extractor;
using Com.Google.Android.Exoplayer2.Source;
using Com.Google.Android.Exoplayer2.Source.Hls;
using Com.Google.Android.Exoplayer2.Source.Smoothstreaming;
using Com.Google.Android.Exoplayer2.Trackselection;
using Com.Google.Android.Exoplayer2.UI;
using Com.Google.Android.Exoplayer2.Upstream;
using Com.Google.Android.Exoplayer2.Video;
using Java.IO;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(ExoVideoPlayer), typeof(ExoVideoPlayerRenderer))]
namespace MoviesHD.Droid
{
    class Listener :Java.Lang.Object, ITransferListener
    {
       

        public void OnBytesTransferred(IDataSource source, DataSpec dataSpec, bool isNetwork, int bytesTransferred)
        {
            
        }


        public void OnTransferEnd(IDataSource source, DataSpec dataSpec, bool isNetwork)
        {
            
        }

        public void OnTransferInitializing(IDataSource source, DataSpec dataSpec, bool isNetwork)
        {
            
        }

       

        public void OnTransferStart(IDataSource source, DataSpec dataSpec, bool isNetwork)
        {
            
        }
    }

    public class ExoVideoPlayerRenderer: ViewRenderer<ExoVideoPlayer, PlayerView>, IPlayerEventListener, IVideoListener, IPlayerRenderListener
    {
        public override bool OnCapturedPointerEvent(MotionEvent e)
        {
            return base.OnCapturedPointerEvent(e);
        }
        public override bool OnTouchEvent(MotionEvent e)
        {           
            return base.OnTouchEvent(e);
        }
        public ExoVideoPlayerRenderer(Context context) : base(context)
        {
            
        }
        long lastDownTime;
        private void Control_Touch(object sender, TouchEventArgs e)
        {

            e.Handled = false;
            if (e.Event.Action == MotionEventActions.Down)
            {
                long dif = e.Event.EventTime - lastDownTime;
                if (dif > 50 && dif < 1000)
                {
                    this.Element?.RaiseDoubleClick();
                    lastDownTime = 0;
                }
                else
                {
                    lastDownTime = e.Event.EventTime;
                }

            }
            Control.ShowController();
        }


        long lastDownTime2=0;
        void IndicatorDown()
        {

            Control.PerformClick();


            long dif = GetCurrentTime() - lastDownTime2;
            if (dif > 50 && dif < 1000)
            {
                this.Element?.RaiseDoubleClick();
                lastDownTime2 = 0;
            }
            else
            {
                lastDownTime2 = GetCurrentTime();
            }
        }
       
        private PlayerView _playerView;
        private SimpleExoPlayer _player;

        public event Action<bool> LoadingChanged;
        public event Action<ExoVideoPlayerPlaybackParameters> PlaybackParametersChanged;
        public event Action<System.Exception> PlayerError;
        public event Action<bool, PlayerState> PlayerStateChanged;
        public event Action<int> PositionDiscontinuity;
        public event Action<int> RepeatModeChanged;
        public event Action SeekProcessed;
        public event Action<bool> ShuffleModeEnabledChanged;
        public event Action RenderedFirstFrame;
        public event Action<int, int, int, float> VideoSizeChanged;

        void SetFullScrenIcon()
        {
            var res = FindViewById<Android.Widget.ImageButton>(Resource.Id.exo_ToggleFullscreen);
            switch (this.Element.IsFullScreen)
            {
                case true:
                    {
                        res.SetImageResource(Resource.Drawable.ic_fullscreen_skrink);
                        break;
                    }
                case false:
                    {
                        res.SetImageResource(Resource.Drawable.ic_fullscreen_expand);
                        break;
                    }
            }
            

        }
        private void InitializePlayer()
        {
          
            _player = ExoPlayerFactory.NewSimpleInstance(Context, new DefaultTrackSelector());
            _player.AddListener(this);

            _player.AddVideoListener(this);
            _player.PlayWhenReady = true;
            _playerView = new PlayerView(Context) { Player = _player };
           
            SetNativeControl(_playerView);
            var res = FindViewById<Android.Widget.ImageButton>(Resource.Id.exo_ToggleFullscreen);
            SetFullScrenIcon();
            this.Element.FullscreenSetFromControl = () => { SetFullScrenIcon(); };
            res.Click += (arg1,arg2) => { this.Element.ToggleFullscreen(); SetFullScrenIcon(); };
            Element.IndicatorDown = IndicatorDown;
            Element.Renderer = this;
            Element.Play = Play;
            Element.PlayHls = PlayHls;
            Element.Release = Release;
            Control.Touch += Control_Touch;
            Control.Click += Control_Click;
        }

        private void Control_Click(object sender, EventArgs e)
        {
            Control.ShowController();
        }

        private static long GetCurrentTime()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
        }
        void Release()
        {
            _player?.Release();
            _player = null;
        }
        private void Play(string url,string referer)
        {
            Element.SourceUrl = url;
            Android.Net.Uri sourceUri = Android.Net.Uri.Parse(Element.SourceUrl);
            Listener l = new Listener();
            
            
            DefaultHttpDataSourceFactory httpDataSourceFactory = new DefaultHttpDataSourceFactory(Strings.UserAgent, l,20000,20000,true);
            httpDataSourceFactory.DefaultRequestProperties.Set("User-Agent", Strings.UserAgent);
            httpDataSourceFactory.DefaultRequestProperties.Set("Accept", "*/*");
            if(referer?.Length>0)
            {
                httpDataSourceFactory.DefaultRequestProperties.Set("Referer", referer);
            }
          
            httpDataSourceFactory.DefaultRequestProperties.Set("Connection", "keep-alive");
            httpDataSourceFactory.DefaultRequestProperties.Set("Accept-Encoding", "identity;q=1, *;q=0");
            DefaultExtractorsFactory extractorsFactory = new DefaultExtractorsFactory();
            ExtractorMediaSource mediaSource = new ExtractorMediaSource(Android.Net.Uri.Parse(Element.SourceUrl),
                httpDataSourceFactory, extractorsFactory, null, null);
            Handler emptyHandler = new Handler();


            //SsMediaSource ssMediaSource = new SsMediaSource(sourceUri, httpDataSourceFactory, ssChunkFactory, emptyHandler, this);
            _player.Prepare(mediaSource);
           
        }
        private void PlayHls(string url, string referer)
        {
            Element.SourceUrl = url;
            Android.Net.Uri sourceUri = Android.Net.Uri.Parse(Element.SourceUrl);
            Listener l = new Listener();


            DefaultHttpDataSourceFactory httpDataSourceFactory = new DefaultHttpDataSourceFactory(Strings.UserAgent, l, 20000, 20000, true);
            httpDataSourceFactory.DefaultRequestProperties.Set("User-Agent", Strings.UserAgent);
            httpDataSourceFactory.DefaultRequestProperties.Set("Accept", "*/*");
            if (referer?.Length > 0)
            {
                httpDataSourceFactory.DefaultRequestProperties.Set("Referer", referer);
            }

            httpDataSourceFactory.DefaultRequestProperties.Set("Connection", "keep-alive");
            httpDataSourceFactory.DefaultRequestProperties.Set("Accept-Encoding", "identity;q=1, *;q=0");
            DefaultExtractorsFactory extractorsFactory = new DefaultExtractorsFactory();



            HlsMediaSource hms = new HlsMediaSource(Android.Net.Uri.Parse(Element.SourceUrl), httpDataSourceFactory, null, null);

            //ExtractorMediaSource mediaSource = new ExtractorMediaSource(Android.Net.Uri.Parse(Element.SourceUrl),
            //    httpDataSourceFactory, extractorsFactory, null, null);
            //DefaultSsChunkSource.Factory ssChunkFactory = new DefultSsChunkSource.Factory(httpDataSourceFactory);
            //Handler emptyHandler = new Handler();

            //SsMediaSource ssMediaSource = new SsMediaSource(sourceUri, httpDataSourceFactory, ssChunkFactory, emptyHandler, this);
            _player.Prepare(hms);
        }
        protected override void OnElementChanged(ElementChangedEventArgs<ExoVideoPlayer> e)
        {
            base.OnElementChanged(e);
            if (_player == null)
            {
                InitializePlayer();
            }          
        }

        public void OnLoadingChanged(bool p0)
        {
            LoadingChanged?.Invoke(p0);
        }

        public void OnPlaybackParametersChanged(PlaybackParameters p0)
        {
            PlaybackParametersChanged?.Invoke(new ExoVideoPlayerPlaybackParameters() { Pitch = p0.Pitch, SkipSilence = p0.SkipSilence, Speed = p0.Speed });
        }

        public void OnPlayerError(ExoPlaybackException p0)
        {
            PlayerError?.Invoke(p0.SourceException as System.Exception);
        }

        public void OnPlayerStateChanged(bool p0, int p1)
        {
            PlayerStateChanged?.Invoke(p0, (PlayerState)p1);
        }

        public void OnPositionDiscontinuity(int p0)
        {
            PositionDiscontinuity?.Invoke(p0);
        }

        public void OnRepeatModeChanged(int p0)
        {
            RepeatModeChanged?.Invoke(p0);
        }

        public void OnSeekProcessed()
        {
            SeekProcessed?.Invoke();
        }

        public void OnShuffleModeEnabledChanged(bool p0)
        {
            ShuffleModeEnabledChanged?.Invoke(p0);
        }

        public void OnTimelineChanged(Timeline p0, Java.Lang.Object p1, int p2)
        {
            
        }

        public void OnTracksChanged(TrackGroupArray p0, TrackSelectionArray p1)
        {
            
        }

        public void OnRenderedFirstFrame()
        {
            RenderedFirstFrame?.Invoke();
        }

        public void OnVideoSizeChanged(int p0, int p1, int p2, float p3)
        {
            VideoSizeChanged?.Invoke(p0, p1, p2, p3);
        }

        public void OnSurfaceSizeChanged(int width, int height)
        {
            
        }
    }
}