﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Microsoft.Design.Fluent
{
    public partial class FluentSymbolIcon : Control
    {
        private PathIcon iconDisplay;

        public FluentSymbolIcon()
        {
            this.DefaultStyleKey = typeof(FluentSymbolIcon);
        }

        public FluentSymbol Symbol
        {
            get { return (FluentSymbol)GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Symbol.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(
            "Symbol",
            typeof(FluentSymbol), typeof(FluentSymbolIcon),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSymbolChanged))
        );

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.GetTemplateChild("IconDisplay") is PathIcon pi)
            {
                this.iconDisplay = pi;
                // Awkward workaround for a weird bug where iconDisplay is null
                // when OnSymbolChanged fires in a newly created FluentSymbolIcon
                Symbol = Symbol;
            }
        }

        private static void OnSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FluentSymbolIcon self && (e.NewValue is FluentSymbol || e.NewValue is int) && self.iconDisplay != null)
            {
                // Set internal Image to the SvgImageSource from the look-up table
                self.iconDisplay.Data = GetPathData((FluentSymbol)e.NewValue);
            }
        }

        public static PathIcon GetPathIcon(FluentSymbol symbol)
        {
            return new PathIcon {
                Data = (Geometry)Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(Geometry), GetPathData(symbol)),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }
        
        public static Geometry GetPathData(int symbol)
        {
            return GetPathData((FluentSymbol)symbol);
        }
        public static Geometry GetPathData(FluentSymbol symbol)
        {
            if (AllFluentIcons.TryGetValue(symbol, out string pathData))
            {
                return (Geometry)Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(Geometry), pathData);
            }
            else
            {
                return null;
            }
        }
        
    }
}
