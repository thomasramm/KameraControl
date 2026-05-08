using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KameraSteuerungDeLuxe.Core;

public static class TextBlockExtensions
{
    public static readonly DependencyProperty AutoShrinkFontProperty =
        DependencyProperty.RegisterAttached(
            "AutoShrinkFont",
            typeof(bool),
            typeof(TextBlockExtensions),
            new PropertyMetadata(false, OnAutoShrinkFontChanged));

    public static bool GetAutoShrinkFont(DependencyObject obj) =>
        (bool)obj.GetValue(AutoShrinkFontProperty);

    public static void SetAutoShrinkFont(DependencyObject obj, bool value) =>
        obj.SetValue(AutoShrinkFontProperty, value);

    // MinFontSize
    public static readonly DependencyProperty MinFontSizeProperty =
        DependencyProperty.RegisterAttached(
            "MinFontSize",
            typeof(double),
            typeof(TextBlockExtensions),
            new PropertyMetadata(8.0));

    public static double GetMinFontSize(DependencyObject obj) =>
        (double)obj.GetValue(MinFontSizeProperty);

    public static void SetMinFontSize(DependencyObject obj, double value) =>
        obj.SetValue(MinFontSizeProperty, value);

    // MaxFontSize
    public static readonly DependencyProperty MaxFontSizeProperty =
        DependencyProperty.RegisterAttached(
            "MaxFontSize",
            typeof(double),
            typeof(TextBlockExtensions),
            new PropertyMetadata(18.0));

    public static double GetMaxFontSize(DependencyObject obj) =>
        (double)obj.GetValue(MaxFontSizeProperty);

    public static void SetMaxFontSize(DependencyObject obj, double value) =>
        obj.SetValue(MaxFontSizeProperty, value);

    private static void OnAutoShrinkFontChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TextBlock tb && (bool)e.NewValue)
        {
            tb.SizeChanged += (s, _) => ResizeText(tb);
            tb.Loaded += (s, _) => ResizeText(tb);
        }
    }

    private static void ResizeText(TextBlock tb)
    {
        if (string.IsNullOrEmpty(tb.Text) || tb.ActualWidth <= 0 || tb.ActualHeight <= 0)
            return;

        double maxFontSize = GetMaxFontSize(tb);
        double minFontSize = GetMinFontSize(tb);

        double fontSize = maxFontSize;
        var formatted = CreateFormattedText(tb, fontSize);

        while ((formatted.Width > tb.ActualWidth || formatted.Height > tb.ActualHeight)
               && fontSize > minFontSize)
        {
            fontSize -= 0.5;
            formatted = CreateFormattedText(tb, fontSize);
        }

        tb.FontSize = fontSize;
    }

    private static FormattedText CreateFormattedText(TextBlock tb, double fontSize)
    {
        return new FormattedText(
            tb.Text,
            System.Globalization.CultureInfo.CurrentCulture,
            tb.FlowDirection,
            new Typeface(tb.FontFamily, tb.FontStyle, tb.FontWeight, tb.FontStretch),
            fontSize,
            Brushes.Black,
            new NumberSubstitution(),
            1.0);
    }
}