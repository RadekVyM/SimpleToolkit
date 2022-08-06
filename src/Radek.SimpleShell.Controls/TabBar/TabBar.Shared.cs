namespace Radek.SimpleShell.Controls
{
    // TODO: Inherit TabBar from Border?
    public partial class TabBar : ContentView, IView
    {
        private HorizontalStackLayout stackLayout;
        private Grid rootGrid;
        private GraphicsView graphicsView;
        private ScrollView scrollView;
        private Border border;
        private Size iconSize;
        private Thickness iconMargin;
        private Thickness buttonPadding;
        private double tabBarHeight;
        private double fontSize;
        private double realMinimalItemWidth;
        private double itemWidth
        {
            get
            {
                double width = double.NaN;

                if (double.IsNaN(ItemWidthRequest) && !IsScrollable)
                    width = Math.Floor(Width / (Items?.Count() ?? 1));
                if (!double.IsNaN(ItemWidthRequest) && !IsScrollable)
                    width = Math.Min(Math.Max(ItemWidthRequest, realMinimalItemWidth), Math.Floor(Width / (Items?.Count() ?? 1)));
                if (!double.IsNaN(ItemWidthRequest) && IsScrollable)
                    width = Math.Max(ItemWidthRequest, realMinimalItemWidth);

                return width;
            }
        }
        private double scrollPosition = 0;
        private TextTransform buttonTextTransform;

        public event TabViewItemSelectedEventHandler ItemSelected;

        public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IEnumerable<BaseShellItem>), typeof(TabBar), propertyChanged: OnItemsChanged);
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(BaseShellItem), typeof(TabBar), propertyChanged: OnSelectedItemChanged);
        public static readonly BindableProperty DesignLanguageProperty = BindableProperty.Create(nameof(DesignLanguage), typeof(DesignLanguage), typeof(TabBar), propertyChanged: OnDesignLanguageChanged, defaultValue: DesignLanguage.Material3);
        public static readonly BindableProperty ItemWidthRequestProperty = BindableProperty.Create(nameof(ItemWidthRequest), typeof(double), typeof(TabBar), propertyChanged: OnItemWidthRequestChanged, defaultValue: double.NaN);
        public static readonly BindableProperty ItemsAlignmentProperty = BindableProperty.Create(nameof(ItemsAlignment), typeof(LayoutAlignment), typeof(TabBar), propertyChanged: OnItemsAlignmentChanged, defaultValue: LayoutAlignment.Center);
        public static readonly BindableProperty IsScrollableProperty = BindableProperty.Create(nameof(IsScrollable), typeof(bool), typeof(TabBar), propertyChanged: OnIsScrollableChanged, defaultValue: false);
        public static readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(TabBar), propertyChanged: OnIconColorChanged, defaultValue: null);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TabBar), propertyChanged: OnTextColorChanged, defaultValue: Colors.Black);
        public static readonly BindableProperty PrimaryBrushProperty = BindableProperty.Create(nameof(PrimaryBrush), typeof(Brush), typeof(TabBar), propertyChanged: OnPrimaryBrushChanged, defaultValue: null);

        public virtual IEnumerable<BaseShellItem> Items
        {
            get => (IEnumerable<BaseShellItem>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public virtual BaseShellItem SelectedItem
        {
            get => (BaseShellItem)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public virtual DesignLanguage DesignLanguage
        {
            get => (DesignLanguage)GetValue(DesignLanguageProperty);
            set => SetValue(DesignLanguageProperty, value);
        }

        public virtual double ItemWidthRequest
        {
            get => (double)GetValue(ItemWidthRequestProperty);
            set => SetValue(ItemWidthRequestProperty, value);
        }

        public virtual LayoutAlignment ItemsAlignment
        {
            get => (LayoutAlignment)GetValue(ItemsAlignmentProperty);
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


        public TabBar()
        {
            HandlerChanged += TabViewHandlerChanged;
            Unloaded += TabViewUnloaded;
            SizeChanged += TabViewSizeChanged;
        }


        private ScrollBarVisibility GetScrollBarVisibility()
        {
#if WINDOWS || MACCATALYST
            return IsScrollable ? ScrollBarVisibility.Default : ScrollBarVisibility.Never;
#else
            return ScrollBarVisibility.Never;
#endif
        }

        private int GetSelectedItemIndex()
        {
            int i = -1;

            if (Items?.Contains(SelectedItem) == true)
            {
                foreach (var item in Items)
                {
                    i++;
                    if (item == SelectedItem)
                        break;
                }
            }

            return i;
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

        private void SwapScrollView()
        {
            if (scrollView is null || border is null)
                return;

            scrollView.SizeChanged -= TabViewSizeChanged;
            border.SizeChanged -= TabViewSizeChanged;

            if (IsScrollable)
            {
                rootGrid.Children.Remove(border);
                rootGrid.Children.Add(scrollView);
                border.Content = null;
                scrollView.Content = stackLayout;

                scrollView.SizeChanged += TabViewSizeChanged;
            }
            else
            {
                rootGrid.Children.Remove(scrollView);
                rootGrid.Children.Add(border);
                scrollView.Content = null;
                border.Content = stackLayout;

                border.SizeChanged += TabViewSizeChanged;
            }
        }

        private void SetUpBaseStructure()
        {
            rootGrid = new Grid
            {
                Style = new Style(typeof(Grid))
            };
            graphicsView = new GraphicsView
            {
                Style = new Style(typeof(GraphicsView))
            };
            scrollView = new ScrollView
            {
                HorizontalOptions = new LayoutOptions(ItemsAlignment, false),
                Orientation = ScrollOrientation.Horizontal,
                HorizontalScrollBarVisibility = GetScrollBarVisibility(),
                VerticalScrollBarVisibility = ScrollBarVisibility.Never,
                Style = new Style(typeof(ScrollView))
            };
            border = new Border
            {
                HorizontalOptions = new LayoutOptions(ItemsAlignment, false),
                StrokeThickness = 0,
                Stroke = Colors.Transparent,
                Background = Colors.Transparent,
                Shadow = null,
                Style = new Style(typeof(Border))
            };
            scrollView.Scrolled += ScrollViewScrolled;

            rootGrid.Children.Add(graphicsView);

            stackLayout = new HorizontalStackLayout
            {
                Style = new Style(typeof(HorizontalStackLayout))
            };

            SwapScrollView();

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
            HeightRequest = tabBarHeight;

            if (rootGrid is null || graphicsView is null || scrollView is null || stackLayout is null)
                return;

            rootGrid.HeightRequest = tabBarHeight;
            graphicsView.HeightRequest = tabBarHeight;

            if (IsScrollable)
            {
                scrollView.HeightRequest = tabBarHeight;
                scrollView.HorizontalOptions = new LayoutOptions(ItemsAlignment, false);
                scrollView.HorizontalScrollBarVisibility = GetScrollBarVisibility();
            }
            else
            {
                border.HeightRequest = tabBarHeight;
                border.HorizontalOptions = new LayoutOptions(ItemsAlignment, false);
            }
            stackLayout.HeightRequest = tabBarHeight;

            foreach (var item in stackLayout.Children)
            {
                var grid = item as Grid;
                var image = grid.Children[0] as BitmapIcon;
                var button = grid.Children[1] as Button;

                var shellItem = grid.BindingContext as BaseShellItem;

                grid.HeightRequest = tabBarHeight;
                grid.WidthRequest = itemWidth;
                button.HeightRequest = tabBarHeight;
                button.Padding = buttonPadding;
                button.TextColor = TextColor;
                button.TextTransform = buttonTextTransform;
                button.FontSize = fontSize;
                button.FontAttributes = button.BindingContext == SelectedItem ? FontAttributes.Bold : FontAttributes.None;
                image.HeightRequest = iconSize.Height;
                image.WidthRequest = iconSize.Width;
                image.Margin = iconMargin;
                image.TintColor = IconColor;
                if (shellItem == SelectedItem)
                {
                    var selectedIcon = SimpleShell.GetSelectedIcon(shellItem);
                    if (selectedIcon is not null)
                        image.Source = selectedIcon;
                }
                else if (image.Source != shellItem.Icon)
                {
                    image.Source = shellItem.Icon;
                }
            }

            InvalidateGraphicsView();
        }

        private IEnumerable<IView> CreateItemViews(IEnumerable<BaseShellItem> items)
        {
            foreach (var item in items)
            {
                var grid = new Grid
                {
                    HeightRequest = tabBarHeight,
                    WidthRequest = itemWidth,
                    Style = new Style(typeof(Grid)),
                    BindingContext = item
                };
                var image = new BitmapIcon
                {
                    Source = item.Icon,
                    HeightRequest = iconSize.Height,
                    WidthRequest = iconSize.Width,
                    Margin = iconMargin,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Start,
                    Aspect = Aspect.AspectFill,
                    TintColor = IconColor,
                    Style = new Style(typeof(BitmapIcon)),
                    BindingContext = item
                };
                var button = new Button
                {
                    Text = item.Title,
                    HeightRequest = tabBarHeight,
                    Padding = buttonPadding,
                    BackgroundColor = Colors.Transparent,
                    TextColor = TextColor,
                    BorderWidth = 0,
                    FontSize = fontSize,
                    Style = new Style(typeof(Button)),
                    BindingContext = item
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

            var itemViews = CreateItemViews(items);

            foreach (var view in stackLayout.Children)
            {
                var grid = view as Grid;
                var button = grid.Children[1] as Button;

                button.Clicked -= ItemButtonClicked;
            }
            stackLayout.Children.Clear();

            foreach (var view in itemViews)
            {
                var grid = view as Grid;
                var button = grid.Children[1] as Button;

                button.Clicked += ItemButtonClicked;

                stackLayout.Children.Add(view);
            }
        }

        private void ItemButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            ItemSelected?.Invoke(sender, new TabViewItemSelectedEventArgs
            {
                ShellItem = button.BindingContext as BaseShellItem
            });
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
        }

        private void ScrollViewScrolled(object sender, ScrolledEventArgs e)
        {
            scrollPosition = e.ScrollX;
            InvalidateGraphicsView();
        }

        private void TabViewUnloaded(object sender, EventArgs e)
        {
            Unloaded -= TabViewUnloaded;
            SizeChanged -= TabViewSizeChanged;
        }

        private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var items = newValue as IEnumerable<BaseShellItem>;
            var tabBar = bindable as TabBar;

            if (oldValue is not null)
            {
                foreach (var item in oldValue as IEnumerable<BaseShellItem>)
                {
                    item.PropertyChanged -= tabBar.ShellItemPropertyChanged;
                }
            }
            if (items is not null)
            {
                foreach (var item in items)
                {
                    item.PropertyChanged += tabBar.ShellItemPropertyChanged;
                }
            }

            tabBar.UpdateButtons(items);
            tabBar.UpdateValuesAccordingToLanguage();
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabBar = bindable as TabBar;
            tabBar.UpdateControls();
        }

        private void ShellItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != BaseShellItem.TitleProperty.PropertyName && e.PropertyName != BaseShellItem.IconProperty.PropertyName)
                return;
            
            if (stackLayout is null)
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
            var tabBar = bindable as TabBar;
            tabBar.UpdateValuesAccordingToLanguage();
        }

        private static void OnItemWidthRequestChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabBar = bindable as TabBar;

            if (tabBar.stackLayout is null)
                return;

            foreach (var item in tabBar.stackLayout.Children)
            {
                var grid = item as Grid;
                grid.WidthRequest = (double)newValue;
            }

            tabBar.InvalidateGraphicsView();
        }

        private static void OnItemsAlignmentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabBar = bindable as TabBar;

            if (tabBar.scrollView is null || tabBar.border is null)
                return;

            if (tabBar.IsScrollable)
            {
                tabBar.scrollView.HorizontalOptions = new LayoutOptions((LayoutAlignment)newValue, false);
                tabBar.scrollView.HorizontalScrollBarVisibility = tabBar.GetScrollBarVisibility();
            }
            else
            {
                tabBar.border.HorizontalOptions = new LayoutOptions((LayoutAlignment)newValue, false);
            }

            tabBar.InvalidateGraphicsView();
        }

        private static void OnIsScrollableChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabBar = bindable as TabBar;
            tabBar.SwapScrollView();
            tabBar.UpdateControls();
        }

        private static void OnIconColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabBar = bindable as TabBar;

            if (tabBar.stackLayout is null)
                return;

            foreach (var item in tabBar.stackLayout.Children)
            {
                var grid = item as Grid;
                var image = grid.Children[0] as BitmapIcon;

                image.TintColor = newValue as Color;
            }
        }

        private static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabBar = bindable as TabBar;

            if (tabBar.stackLayout is null)
                return;

            foreach (var item in tabBar.stackLayout.Children)
            {
                var grid = item as Grid;
                var button = grid.Children[1] as Button;

                button.TextColor = newValue as Color;
            }
        }

        private static void OnPrimaryBrushChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabBar = bindable as TabBar;
            tabBar.InvalidateGraphicsView();
        }
    }
}
