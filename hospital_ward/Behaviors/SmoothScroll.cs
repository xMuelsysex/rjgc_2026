using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace MyFirstApp.Behaviors;

public class SmoothScroll
{
    private SmoothScroll() { }

    public static readonly AttachedProperty<bool> IsEnabledProperty =
        AvaloniaProperty.RegisterAttached<SmoothScroll, ScrollViewer, bool>("IsEnabled");

    public static readonly AttachedProperty<double> WheelPixelsProperty =
        AvaloniaProperty.RegisterAttached<SmoothScroll, ScrollViewer, double>("WheelPixels", 72);

    public static readonly AttachedProperty<TimeSpan> DurationProperty =
        AvaloniaProperty.RegisterAttached<SmoothScroll, ScrollViewer, TimeSpan>("Duration", TimeSpan.FromMilliseconds(180));

    private static readonly AttachedProperty<Controller?> ControllerProperty =
        AvaloniaProperty.RegisterAttached<SmoothScroll, ScrollViewer, Controller?>("Controller");

    static SmoothScroll()
    {
        IsEnabledProperty.Changed.AddClassHandler<ScrollViewer>(OnIsEnabledChanged);
    }

    public static bool GetIsEnabled(AvaloniaObject obj) => obj.GetValue(IsEnabledProperty);
    public static void SetIsEnabled(AvaloniaObject obj, bool value) => obj.SetValue(IsEnabledProperty, value);

    public static double GetWheelPixels(AvaloniaObject obj) => obj.GetValue(WheelPixelsProperty);
    public static void SetWheelPixels(AvaloniaObject obj, double value) => obj.SetValue(WheelPixelsProperty, value);

    public static TimeSpan GetDuration(AvaloniaObject obj) => obj.GetValue(DurationProperty);
    public static void SetDuration(AvaloniaObject obj, TimeSpan value) => obj.SetValue(DurationProperty, value);

    private static void OnIsEnabledChanged(ScrollViewer scrollViewer, AvaloniaPropertyChangedEventArgs args)
    {
        var enabled = args.GetNewValue<bool>();
        if (enabled)
            Attach(scrollViewer);
        else
            Detach(scrollViewer);
    }

    private static void Attach(ScrollViewer scrollViewer)
    {
        if (scrollViewer.GetValue(ControllerProperty) is not null)
            return;

        scrollViewer.SetValue(ControllerProperty, new Controller(scrollViewer));
    }

    private static void Detach(ScrollViewer scrollViewer)
    {
        scrollViewer.GetValue(ControllerProperty)?.Dispose();
        scrollViewer.ClearValue(ControllerProperty);
    }

    private sealed class Controller : IDisposable
    {
        private const double TimerIntervalSeconds = 1d / 120d;
        private const double MaxFrameDeltaSeconds = 1d / 20d;
        private const double MinFrameDeltaSeconds = 1d / 240d;
        private const double StopVelocityThreshold = 20d;

        private readonly ScrollViewer _scrollViewer;
        private readonly DispatcherTimer _timer;
        private readonly EventHandler<PointerWheelEventArgs> _wheelHandler;
        private bool _disposed;

        private long _lastTickTimestamp;
        private double _velocityY;

        public Controller(ScrollViewer scrollViewer)
        {
            _scrollViewer = scrollViewer;
            _wheelHandler = OnPointerWheelChanged;

            _timer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromSeconds(TimerIntervalSeconds)
            };
            _timer.Tick += OnTick;

            _scrollViewer.AddHandler(InputElement.PointerWheelChangedEvent, _wheelHandler, RoutingStrategies.Tunnel);
            _scrollViewer.DetachedFromVisualTree += OnDetachedFromVisualTree;
        }

        private void OnDetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e) => Dispose();

        private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
        {
            if (_disposed || e.Handled)
                return;

            if (e.KeyModifiers.HasFlag(KeyModifiers.Control))
                return;

            if (!CanScrollVertically(_scrollViewer))
                return;

            var wheelPixels = GetWheelPixels(_scrollViewer);
            if (wheelPixels <= 0)
                return;

            var duration = GetDuration(_scrollViewer);
            var deltaY = -e.Delta.Y * wheelPixels;
            if (Math.Abs(deltaY) < 0.01)
                return;

            if (duration <= TimeSpan.Zero)
            {
                var immediateTarget = ClampY(_scrollViewer.Offset.Y + deltaY, _scrollViewer);
                _scrollViewer.Offset = new Vector(_scrollViewer.Offset.X, immediateTarget);
                e.Handled = true;
                return;
            }

            var damping = GetDamping(duration);
            var impulse = deltaY * damping;
            _velocityY += impulse;
            _velocityY = Math.Clamp(_velocityY, -GetMaxVelocity(wheelPixels, damping), GetMaxVelocity(wheelPixels, damping));

            if (!_timer.IsEnabled)
            {
                _lastTickTimestamp = Stopwatch.GetTimestamp();
                _timer.Start();
            }

            e.Handled = true;
        }

        private void OnTick(object? sender, EventArgs e)
        {
            if (_disposed)
                return;

            if (!CanScrollVertically(_scrollViewer))
            {
                StopMotion();
                return;
            }

            var duration = GetDuration(_scrollViewer);
            if (duration <= TimeSpan.Zero)
            {
                StopMotion();
                return;
            }

            var now = Stopwatch.GetTimestamp();
            var deltaSeconds = (now - _lastTickTimestamp) / (double)Stopwatch.Frequency;
            _lastTickTimestamp = now;
            deltaSeconds = Math.Clamp(deltaSeconds, MinFrameDeltaSeconds, MaxFrameDeltaSeconds);

            var currentY = _scrollViewer.Offset.Y;
            var nextY = ClampY(currentY + (_velocityY * deltaSeconds), _scrollViewer);
            if (Math.Abs(nextY - currentY) > 0.01)
                _scrollViewer.Offset = new Vector(_scrollViewer.Offset.X, nextY);

            var damping = GetDamping(duration);
            _velocityY *= Math.Exp(-damping * deltaSeconds);

            var maxY = GetMaxY(_scrollViewer);
            var hitUpperBound = nextY <= 0.01 && _velocityY < 0;
            var hitLowerBound = nextY >= maxY - 0.01 && _velocityY > 0;
            if (hitUpperBound || hitLowerBound || Math.Abs(_velocityY) < StopVelocityThreshold)
                StopMotion();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            StopMotion();
            _timer.Tick -= OnTick;
            _scrollViewer.RemoveHandler(InputElement.PointerWheelChangedEvent, _wheelHandler);
            _scrollViewer.DetachedFromVisualTree -= OnDetachedFromVisualTree;
        }

        private static bool CanScrollVertically(ScrollViewer scrollViewer)
            => scrollViewer.Extent.Height - scrollViewer.Viewport.Height > 0.5;

        private static double GetMaxY(ScrollViewer scrollViewer)
            => Math.Max(0, scrollViewer.Extent.Height - scrollViewer.Viewport.Height);

        private static double ClampY(double y, ScrollViewer scrollViewer)
        {
            return Math.Clamp(y, 0, GetMaxY(scrollViewer));
        }

        private void StopMotion()
        {
            _velocityY = 0;
            _lastTickTimestamp = 0;
            _timer.Stop();
        }

        private static double GetDamping(TimeSpan duration)
        {
            var seconds = Math.Max(duration.TotalSeconds, 0.08);
            return 6d / seconds;
        }

        private static double GetMaxVelocity(double wheelPixels, double damping)
        {
            var baseVelocity = Math.Max(wheelPixels * damping * 4d, 1800d);
            return baseVelocity;
        }
    }
}
