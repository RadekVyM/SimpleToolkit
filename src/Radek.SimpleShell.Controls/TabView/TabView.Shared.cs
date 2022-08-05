namespace Radek.SimpleShell.Controls
{
    public partial class TabView : ContentView, IView
    {
        private HorizontalStackLayout stackLayout;
        private Grid rootGrid;
        private GraphicsView graphicsView;
        private ScrollView scrollView;
        private Size iconSize;
        private Thickness iconMargin;
        private Thickness buttonPadding;
        private double tabViewHeight;
        private double fontSize;
        private double realMinimalItemWidth;
        private double itemWidth => double.IsNaN(MinimumItemWidth) ? (IsScrollable ? double.NaN : Width / (Items?.Count() ?? 1)) : Math.Max(MinimumItemWidth, realMinimalItemWidth);
        private double scrollPosition = 0;
        private TextTransform buttonTextTransform;

        public event TabViewItemSelectedEventHandler ItemSelected;

        public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IEnumerable<BaseShellItem>), typeof(TabView), propertyChanged: OnItemsChanged);
        public static readonly BindableProperty DesignLanguageProperty = BindableProperty.Create(nameof(DesignLanguage), typeof(DesignLanguage), typeof(TabView), propertyChanged: OnDesignLanguageChanged, defaultValue: DesignLanguage.Material3);
        public static readonly BindableProperty TypeProperty = BindableProperty.Create(nameof(Type), typeof(TabViewType), typeof(TabView), propertyChanged: OnTypeChanged, defaultValue: TabViewType.Bottom);
        public static readonly BindableProperty MinimumItemWidthProperty = BindableProperty.Create(nameof(MinimumItemWidth), typeof(double), typeof(TabView), propertyChanged: OnMinimumItemWidthChanged, defaultValue: double.NaN);
        public static readonly BindableProperty ItemsAlignmentProperty = BindableProperty.Create(nameof(ItemsAlignment), typeof(LayoutOptions), typeof(TabView), propertyChanged: OnItemsAlignmentChanged, defaultValue: LayoutOptions.Center);
        public static readonly BindableProperty IsScrollableProperty = BindableProperty.Create(nameof(IsScrollable), typeof(bool), typeof(TabView), propertyChanged: OnIsScrollableChanged, defaultValue: false);
        public static readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(TabView), propertyChanged: OnIconColorChanged, defaultValue: null);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TabView), propertyChanged: OnTextColorChanged, defaultValue: Colors.Black);
        public static readonly BindableProperty PrimaryBrushProperty = BindableProperty.Create(nameof(PrimaryBrush), typeof(Brush), typeof(TabView), propertyChanged: OnPrimaryBrushChanged, defaultValue: null);

        public virtual IEnumerable<BaseShellItem> Items
        {
            get => (IEnumerable<BaseShellItem>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public virtual DesignLanguage DesignLanguage
        {
            get => (DesignLanguage)GetValue(DesignLanguageProperty);
            set => SetValue(DesignLanguageProperty, value);
        }

        public virtual TabViewType Type
        {
            get => (TabViewType)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public virtual double MinimumItemWidth
        {
            get => (double)GetValue(MinimumItemWidthProperty);
            set => SetValue(MinimumItemWidthProperty, value);
        }

        public virtual LayoutOptions ItemsAlignment
        {
            get => (LayoutOptions)GetValue(ItemsAlignmentProperty);
            set => SetValue(ItemsAlignmentProperty, value);
        }

        public virtual bool IsScrollable
        {
            get => (bool)GetValue(IsScrollableProperty);
            set => SetValue(IsScrollableProperty, value);
        }

        public virtual Color IconColor
        {
            get => (Color)GetValue(IconColorProperty);
            set => SetValue(IconColorProperty, value);
        }

        public virtual Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public virtual Brush PrimaryBrush
        {
            get => (Brush)GetValue(PrimaryBrushProperty);
            set => SetValue(PrimaryBrushProperty, value);
        }


        public TabView()
        {
            HandlerChanged += TabViewHandlerChanged;
            SizeChanged += TabViewSizeChanged;
            Unloaded += TabViewUnloaded;
        }


        private ScrollBarVisibility GetScrollBarVisibility()
        {
#if WINDOWS || MACCATALYST
            return IsScrollable ? ScrollBarVisibility.Default : ScrollBarVisibility.Never;
#else
            return ScrollBarVisibility.Never;
#endif
        }

        private void InvalidateGraphicsView()
        {
            switch (DesignLanguage)
            {
                case DesignLanguage.Cupertino:
                    UpdateDrawableToCupertino();
                    break;
                case DesignLanguage.Material3:
                    UpdateDrawableToMaterial3();
                    break;
            }

            graphicsView?.Invalidate();
        }

        private void SetUpBaseStructure()
        {
            rootGrid = new Grid();
            graphicsView = new GraphicsView();
            scrollView = new ScrollView
            {
                HorizontalOptions = ItemsAlignment,
                Orientation = ScrollOrientation.Horizontal,
                HorizontalScrollBarVisibility = GetScrollBarVisibility(),
                VerticalScrollBarVisibility = ScrollBarVisibility.Never
            };
            scrollView.Scrolled += ScrollViewScrolled;

            rootGrid.Children.Add(graphicsView);
            rootGrid.Children.Add(scrollView);

            stackLayout = new HorizontalStackLayout();

            scrollView.Content = stackLayout;

            Content = rootGrid;
        }

        private void UpdateValuesAccordingToLanguage()
        {
            switch (DesignLanguage)
            {
                case DesignLanguage.Cupertino:
                    UpdateValuesToCupertino();
                    break;
                case DesignLanguage.Material3:
                    UpdateValuesToMaterial3();
                    break;
            }

            UpdateControls();
        }

        private void UpdateControls()
        {
            HeightRequest = tabViewHeight;

            if (rootGrid is null || graphicsView is null || scrollView is null || stackLayout is null)
                return;

            rootGrid.HeightRequest = tabViewHeight;
            graphicsView.HeightRequest = tabViewHeight;
            scrollView.HeightRequest = tabViewHeight;
            scrollView.HorizontalOptions = ItemsAlignment;
            scrollView.HorizontalScrollBarVisibility = GetScrollBarVisibility();
            stackLayout.HeightRequest = tabViewHeight;

            foreach (var item in stackLayout)
            {
                var grid = item as Grid;
                var image = grid.Children[0] as Image;
                var button = grid.Children[1] as Button;

                grid.HeightRequest = tabViewHeight;
                grid.WidthRequest = itemWidth;
                button.HeightRequest = tabViewHeight;
                button.Padding = buttonPadding;
                button.TextColor = TextColor;
                button.TextTransform = buttonTextTransform;
                button.FontSize = fontSize;
                image.HeightRequest = iconSize.Height;
                image.WidthRequest = iconSize.Width;
                image.Margin = iconMargin;

                image.ApplyTintToImage(IconColor);
            }
        }

        private IEnumerable<IView> CreateButtons(IEnumerable<BaseShellItem> items)
        {
            foreach (var item in items)
            {
                var grid = new Grid
                {
                    HeightRequest = tabViewHeight,
                    WidthRequest = itemWidth
                };
                var image = new Image
                {
                    Source = item.Icon,
                    HeightRequest = iconSize.Height,
                    WidthRequest = iconSize.Width,
                    Margin = iconMargin,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Aspect = Aspect.AspectFill
                };
                var button = new Button
                {
                    Text = item.Title,
                    HeightRequest = tabViewHeight,
                    Padding = buttonPadding,
                    BackgroundColor = Colors.Transparent,
                    TextColor = TextColor,
                    BorderWidth = 0,
                    FontSize = fontSize
                };

                grid.Children.Add(image);
                grid.Children.Add(button);

                yield return grid;
            }
        }

        private void UpdateButtons(IEnumerable<BaseShellItem> items)
        {
            if (stackLayout is null || items is null || !items.Any())
                return;

            var buttons = CreateButtons(items);
            stackLayout.Children.Clear();
            foreach (var button in buttons)
                stackLayout.Children.Add(button);
        }

        private void TabViewSizeChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void TabViewHandlerChanged(object sender, EventArgs e)
        {
            SetUpBaseStructure();
            UpdateButtons(Items);
            UpdateValuesAccordingToLanguage();
            UpdateControls();
        }

        private void ScrollViewScrolled(object sender, ScrolledEventArgs e)
        {
            scrollPosition = e.ScrollX;
            InvalidateGraphicsView();
        }

        private void TabViewUnloaded(object sender, EventArgs e)
        {
            Unloaded -= TabViewUnloaded;
        }

        private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var items = newValue as IEnumerable<BaseShellItem>;
            var tabView = bindable as TabView;

            if (oldValue is not null)
            {
                foreach (var item in oldValue as IEnumerable<BaseShellItem>)
                {
                    item.PropertyChanged -= tabView.ShellItemPropertyChanged;
                }
            }
            if (items is not null)
            {
                foreach (var item in items)
                {
                    item.PropertyChanged += tabView.ShellItemPropertyChanged;
                }
            }

            tabView.UpdateButtons(items);
            tabView.UpdateValuesAccordingToLanguage();
            tabView.UpdateControls();
        }

        private void ShellItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != BaseShellItem.TitleProperty.PropertyName && e.PropertyName != BaseShellItem.IconProperty.PropertyName)
                return;

            var shellItem = sender as BaseShellItem;

            foreach (var item in stackLayout)
            {
                var grid = item as Grid;
                var image = grid.Children[0] as Image;
                var button = grid.Children[1] as Button;

                button.Text = shellItem.Title;
                image.Source = shellItem.Icon;

                image.ApplyTintToImage(IconColor);
            }
        }

        private static void OnDesignLanguageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabView = bindable as TabView;
            tabView.UpdateControls();
        }

        private static void OnTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabView = bindable as TabView;
            tabView.UpdateControls();
        }

        private static void OnMinimumItemWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabView = bindable as TabView;
            tabView.UpdateControls();
        }

        private static void OnItemsAlignmentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabView = bindable as TabView;
            tabView.UpdateControls();
        }

        private static void OnIsScrollableChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabView = bindable as TabView;
            tabView.UpdateControls();
        }

        private static void OnIconColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabView = bindable as TabView;
            tabView.UpdateControls();
        }

        private static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabView = bindable as TabView;
            tabView.UpdateControls();
        }

        private static void OnPrimaryBrushChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabView = bindable as TabView;
            tabView.InvalidateGraphicsView();
        }
    }
}
