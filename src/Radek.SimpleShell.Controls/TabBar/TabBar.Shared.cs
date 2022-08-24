namespace Radek.SimpleShell.Controls
{
    public partial class TabBar : ContentView, IView
    {
        private HorizontalStackLayout stackLayout;
        private Grid rootGrid;
        private GraphicsView graphicsView;
        private ScrollView scrollView;
        private Border border;
        private Size iconSize;
        private Thickness iconMargin;
        private Thickness itemStackLayoutPadding;
        private double tabBarHeight;
        private double fontSize;
        private double itemStackLayoutSpacing;
        private double realMinimumItemWidth;
        private double itemWidth
        {
            get
            {
                double width = double.NaN;

                if (double.IsNaN(ItemWidthRequest) && !IsScrollable && DesignLanguage is not DesignLanguage.Fluent)
                {
                    var count = stackLayout?.Children?.Count ?? 1;
                    count = count == 0 ? 1 : count;
                    width = Math.Max(Math.Floor(Width / count), realMinimumItemWidth);
                }
                if (!double.IsNaN(ItemWidthRequest) && !IsScrollable)
                {
                    var count = stackLayout?.Children?.Count ?? 1;
                    count = count == 0 ? 1 : count;
                    width = Math.Min(Math.Max(ItemWidthRequest, realMinimumItemWidth), Math.Floor(Width / count));
                }
                if (!double.IsNaN(ItemWidthRequest) && IsScrollable)
                    width = Math.Max(ItemWidthRequest, realMinimumItemWidth);

                return width;
            }
        }
        private int itemsInStacklayoutCount
        {
            get
            {
                var count = stackLayout?.Children?.Count ?? 1;

                if (stackLayout?.Children?.Contains(moreButton) == true)
                    return count - 1;

                return count;
            }
        }
        private double scrollPosition = 0;
        private TextTransform labelTextTransform;
        private FontAttributes labelAttributes;
        private FontAttributes labelSelectionAttributes;
        private StackOrientation itemStackLayoutOrientation = StackOrientation.Vertical;

        private IList<Grid> allItemViews = new List<Grid>();
        private IList<Grid> hiddenItems = new List<Grid>();
        private Grid moreButton = null;

        public event TabItemSelectedEventHandler ItemSelected;

        public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IEnumerable<BaseShellItem>), typeof(TabBar), propertyChanged: OnItemsChanged);
        public static readonly BindableProperty HiddenItemsProperty = BindableProperty.Create(nameof(HiddenItems), typeof(IReadOnlyList<BaseShellItem>), typeof(TabBar), defaultBindingMode: BindingMode.OneWayToSource);
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(BaseShellItem), typeof(TabBar), propertyChanged: OnSelectedItemChanged);
        public static readonly BindableProperty DesignLanguageProperty = BindableProperty.Create(nameof(DesignLanguage), typeof(DesignLanguage), typeof(TabBar), propertyChanged: OnDesignLanguageChanged, defaultValue: DesignLanguage.Material3);
        public static readonly BindableProperty ItemWidthRequestProperty = BindableProperty.Create(nameof(ItemWidthRequest), typeof(double), typeof(TabBar), propertyChanged: OnItemWidthRequestChanged, defaultValue: double.NaN);
        public static readonly BindableProperty ItemsAlignmentProperty = BindableProperty.Create(nameof(ItemsAlignment), typeof(LayoutAlignment), typeof(TabBar), propertyChanged: OnItemsAlignmentChanged, defaultValue: LayoutAlignment.Center);
        public static readonly BindableProperty IsScrollableProperty = BindableProperty.Create(nameof(IsScrollable), typeof(bool), typeof(TabBar), propertyChanged: OnIsScrollableChanged, defaultValue: false);
        public static readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(TabBar), propertyChanged: OnIconColorChanged, defaultValue: Colors.Black);
        public static readonly BindableProperty IconSelectionColorProperty = BindableProperty.Create(nameof(IconSelectionColor), typeof(Color), typeof(TabBar), propertyChanged: OnIconSelectionColorChanged, defaultValue: Colors.Black);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(TabBar), propertyChanged: OnTextColorChanged, defaultValue: Colors.Black);
        public static readonly BindableProperty TextSelectionColorProperty = BindableProperty.Create(nameof(TextSelectionColor), typeof(Color), typeof(TabBar), propertyChanged: OnTextSelectionColorChanged, defaultValue: Colors.Black);
        public static readonly BindableProperty PrimaryBrushProperty = BindableProperty.Create(nameof(PrimaryBrush), typeof(Brush), typeof(TabBar), propertyChanged: OnPrimaryBrushChanged, defaultValue: null);
        public static readonly BindableProperty ShowButtonAndMenuWhenMoreItemsDoNotFitProperty = BindableProperty.Create(nameof(ShowButtonAndMenuWhenMoreItemsDoNotFit), typeof(bool), typeof(TabBar), propertyChanged: OnShowButtonAndMenuWhenMoreItemsDoNotFitChanged, defaultValue: true); // Naming is hard 😶
        public static readonly BindableProperty MoreTitleProperty = BindableProperty.Create(nameof(MoreTitle), typeof(string), typeof(TabBar), propertyChanged: OnMoreTitleChanged, defaultValue: "More");
        public static readonly BindableProperty MoreIconProperty = BindableProperty.Create(nameof(MoreIcon), typeof(ImageSource), typeof(TabBar), propertyChanged: OnMoreIconChanged, defaultValue: null);

        public virtual IEnumerable<BaseShellItem> Items
        {
            get => (IEnumerable<BaseShellItem>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public virtual IReadOnlyList<BaseShellItem> HiddenItems
        {
            get => (IReadOnlyList<BaseShellItem>)GetValue(HiddenItemsProperty);
            private set => SetValue(HiddenItemsProperty, value);
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

        public virtual Color IconSelectionColor
        {
            get => (Color)GetValue(IconSelectionColorProperty);
            set => SetValue(IconSelectionColorProperty, value);
        }

        public virtual Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public virtual Color TextSelectionColor
        {
            get => (Color)GetValue(TextSelectionColorProperty);
            set => SetValue(TextSelectionColorProperty, value);
        }

        public virtual Brush PrimaryBrush
        {
            get => (Brush)GetValue(PrimaryBrushProperty);
            set => SetValue(PrimaryBrushProperty, value);
        }

        public virtual bool ShowButtonAndMenuWhenMoreItemsDoNotFit
        {
            get => (bool)GetValue(ShowButtonAndMenuWhenMoreItemsDoNotFitProperty);
            set => SetValue(ShowButtonAndMenuWhenMoreItemsDoNotFitProperty, value);
        }

        public virtual string MoreTitle
        {
            get => (string)GetValue(MoreTitleProperty);
            set => SetValue(MoreTitleProperty, value);
        }

        public virtual ImageSource MoreIcon
        {
            get => (ImageSource)GetValue(MoreIconProperty);
            set => SetValue(MoreIconProperty, value);
        }


        public TabBar()
        {
            HorizontalOptions = LayoutOptions.Fill;

            HandlerChanged += TabBarHandlerChanged;
            Unloaded += TabBarUnloaded;
            SizeChanged += TabBarSizeChanged;

            CreateMoreButton();
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
                    if (IsSelected(item))
                        break;
                }
            }

            return i;
        }

        // TODO: Update selection of more button - if hidden item is selected, show more button as selected
        private bool IsSelected(BindableObject bindableObject)
        {
            // (bindableObject == moreButton && hiddenItems.Any(h => h.BindingContext == SelectedItem))

            return bindableObject.BindingContext == SelectedItem || bindableObject == SelectedItem;
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
                case DesignLanguage.Fluent:
                    UpdateDrawableToFluent();
                    break;
            }

            graphicsView?.Invalidate();
        }

        private void SwapScrollView()
        {
            if (scrollView is null || border is null)
                return;

            scrollView.SizeChanged -= TabBarSizeChanged;
            border.SizeChanged -= TabBarSizeChanged;

            if (IsScrollable)
            {
                rootGrid.Children.Remove(border);
                rootGrid.Children.Add(scrollView);
                border.Content = null;
                scrollView.Content = stackLayout;

                scrollView.SizeChanged += TabBarSizeChanged;
            }
            else
            {
                rootGrid.Children.Remove(scrollView);
                rootGrid.Children.Add(border);
                scrollView.Content = null;
                border.Content = stackLayout;

                border.SizeChanged += TabBarSizeChanged;
            }
        }

        private void SetUpBaseStructure()
        {
            rootGrid = new Grid
            {
                Style = new Style(typeof(Grid)),
                Background = Colors.Transparent
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

            CompressedLayout.SetIsHeadless(rootGrid, true);

            Content = rootGrid;
        }

        private IEnumerable<Grid> CreateItemViews(IEnumerable<BaseShellItem> items)
        {
            foreach (var item in items)
            {
                yield return CreateButton(item);
            }
        }

        private Grid CreateButton(BaseShellItem item)
        {
            var grid = new Grid
            {
                HeightRequest = tabBarHeight,
                WidthRequest = itemWidth,
                Style = new Style(typeof(Grid)),
                BindingContext = item
            };
            var stackLayout = new StackLayout
            {
                Padding = itemStackLayoutPadding,
                Orientation = itemStackLayoutOrientation,
                Spacing = itemStackLayoutSpacing,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Fill,
                Style = new Style(typeof(StackLayout)),
                BindingContext = item
            };
            var button = new Button
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BackgroundColor = Colors.Transparent,
                BorderWidth = 0,
                Style = new Style(typeof(Button)),
                BindingContext = item
            };

            var label = new Label
            {
                Text = item.Title,
                TextColor = TextColor,
                FontSize = fontSize,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Style = new Style(typeof(Label)),
                BindingContext = item
            };
            var image = new Icon
            {
                Source = item.Icon,
                HeightRequest = iconSize.Height,
                WidthRequest = iconSize.Width,
                Margin = iconMargin,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TintColor = IconColor,
                Style = new Style(typeof(Icon)),
                BindingContext = item
            };

            stackLayout.Children.Add(image);
            stackLayout.Children.Add(label);

            grid.Children.Add(stackLayout);
            grid.Children.Add(button);

            CompressedLayout.SetIsHeadless(stackLayout, true);
            CompressedLayout.SetIsHeadless(grid, true);

            return grid;
        }

        private Grid CreateMoreButton()
        {
            var shellItem = new BaseShellItem
            {
                Title = MoreTitle,
                Icon = MoreIcon
            };
            return moreButton = CreateButton(shellItem);
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
                case DesignLanguage.Fluent:
                    UpdateValuesToFluent();
                    break;
            }

            UpdateControls();
        }

        private async void UpdateControls()
        {
            HeightRequest = tabBarHeight;

            if (rootGrid is null || graphicsView is null || scrollView is null || stackLayout is null)
                return;

            rootGrid.HeightRequest = tabBarHeight;
            graphicsView.HeightRequest = tabBarHeight;

            if (IsScrollable)
            {
                scrollView.HeightRequest = tabBarHeight;
                if (scrollView.HorizontalOptions.Alignment != ItemsAlignment)
                    scrollView.HorizontalOptions = new LayoutOptions(ItemsAlignment, false);
                scrollView.HorizontalScrollBarVisibility = GetScrollBarVisibility();
            }
            else
            {
                border.HeightRequest = tabBarHeight;
                if (border.HorizontalOptions.Alignment != ItemsAlignment)
                    border.HorizontalOptions = new LayoutOptions(ItemsAlignment, false);
            }

            stackLayout.HeightRequest = tabBarHeight;

            foreach (var item in allItemViews)
            {
                UpdateButton(item);
            }

            UpdateMoreButton();

            // Wait for remeasuring of Width property of all items
            await Task.Delay(10);

            if (!IsScrollable)
            {
                var visibleItemsChanged = UpdateHiddenItems();

                if (visibleItemsChanged)
                    UpdateSizeOfItems();
            }

            InvalidateGraphicsView();
        }

        private void UpdateButton(Grid item)
        {
            var stackLayout = item.Children[0] as StackLayout;
            var image = stackLayout.Children[0] as Icon;
            var label = stackLayout.Children[1] as Label;

            var shellItem = item.BindingContext as BaseShellItem;

            item.HeightRequest = tabBarHeight;
            item.WidthRequest = itemWidth;
            label.TextTransform = labelTextTransform;
            label.TextColor = IsSelected(shellItem) ? TextSelectionColor : TextColor;
            label.FontSize = fontSize;
            label.FontAttributes = IsSelected(shellItem) ? labelSelectionAttributes : labelAttributes;

            if (IsSelected(shellItem))
            {
                var selectedIcon = SimpleIcon.GetSelectedIcon(shellItem);
                if (selectedIcon is not null && image.Source != selectedIcon)
                    image.Source = selectedIcon;
            }
            else if (image.Source != shellItem.Icon)
            {
                image.Source = shellItem.Icon;
            }

            image.HeightRequest = iconSize.Height;
            image.WidthRequest = image.Source is not null ? iconSize.Width : 0;
            if (image.Margin != iconMargin)
                image.Margin = iconMargin;
            image.TintColor = IsSelected(shellItem) ? IconSelectionColor : IconColor;

            stackLayout.Orientation = itemStackLayoutOrientation;
            stackLayout.Spacing = image.Source is null && itemStackLayoutOrientation is StackOrientation.Horizontal ? 0 : itemStackLayoutSpacing;
            if (stackLayout.Padding != itemStackLayoutPadding)
                stackLayout.Padding = itemStackLayoutPadding;
        }

        private void UpdateButtons(IEnumerable<BaseShellItem> items)
        {
            if (stackLayout is null || items is null)
                return;

            var itemViews = CreateItemViews(items);

            foreach (var view in allItemViews)
            {
                var button = view.Children[1] as Button;

                button.Clicked -= ItemButtonClicked;
            }

            stackLayout.Children.Clear();
            allItemViews = itemViews.ToList();

            foreach (var view in allItemViews)
            {
                var button = view.Children[1] as Button;

                button.Clicked += ItemButtonClicked;

                stackLayout.Children.Add(view);
            }
        }

        private void UpdateMoreButton()
        {
            if (moreButton is null)
            {
                moreButton = CreateMoreButton();
                return;
            }

            var shellItem = moreButton.BindingContext as BaseShellItem;

            shellItem.Title = MoreTitle;
            shellItem.Icon = MoreIcon;

            UpdateButton(moreButton);
        }

        private void UpdateSizeOfItems()
        {
            foreach (var item in allItemViews)
            {
                item.HeightRequest = tabBarHeight;
                item.WidthRequest = itemWidth;
            }

            moreButton.HeightRequest = tabBarHeight;
            moreButton.WidthRequest = itemWidth;
        }

        private bool UpdateHiddenItems()
        {
            bool changed = false;
            bool addMoreButton = false;
            double totalWidth = 0;
            double currentItemWidth = itemWidth;
            double moreButtonWidth = DesignLanguage is DesignLanguage.Fluent ? (moreButton?.Measure(double.PositiveInfinity, double.PositiveInfinity).Request.Width ?? realMinimumItemWidth) : realMinimumItemWidth;
            List<Grid> hidden = new List<Grid>();

            stackLayout.Children.Remove(moreButton);

            // Find items that should be hidden
            FindNewHiddenItems(ref changed, ref totalWidth, hidden, currentItemWidth);

            // Try to show some already hidden items
            if (stackLayout.Children.Count < allItemViews.Count && !hidden.Any())
            {
                TryShowHiddenItems(ref changed, ref totalWidth, ref addMoreButton, moreButtonWidth);
            }

            // Hide last item if more button should be shown
            if (ShowButtonAndMenuWhenMoreItemsDoNotFit && (addMoreButton || hidden.Any()))
            {
                totalWidth -= DesignLanguage is DesignLanguage.Fluent ? ((hidden.FirstOrDefault() as View)?.Width ?? 0) : currentItemWidth;

                if (totalWidth + moreButtonWidth > Width)
                {
                    var last = stackLayout.Children.Except(hidden).LastOrDefault() as Grid;

                    if (last is not null)
                        hidden.Insert(0, last);
                }
            }

            // Hide items that should be hidden
            addMoreButton = HideItems(hidden) || addMoreButton;

            // Show more button
            if (addMoreButton)
                stackLayout.Children.Add(moreButton);

            return changed;
        }

        private bool HideItems(List<Grid> hidden)
        {
            bool addMoreButton = false;

            if (hidden.Any())
            {
                hiddenItems = hidden.Concat(hiddenItems).ToList();

                hidden.ForEach(v =>
                {
                    stackLayout.Children.Remove(v);
                });

                if (ShowButtonAndMenuWhenMoreItemsDoNotFit)
                    addMoreButton = true;
            }
            HiddenItems = hiddenItems.Select(v => v.BindingContext as BaseShellItem).ToList();

            return addMoreButton;
        }

        private void TryShowHiddenItems(ref bool changed, ref double totalWidth, ref bool addMoreButton, double moreButtonWidth)
        {
            totalWidth = DesignLanguage is DesignLanguage.Fluent ? totalWidth : stackLayout.Children.Count * realMinimumItemWidth;
            totalWidth += ShowButtonAndMenuWhenMoreItemsDoNotFit ? moreButtonWidth : 0;

            while (totalWidth < Width)
            {
                var item = hiddenItems.FirstOrDefault();

                if (item is null)
                    break;

                var newTotalWidth = totalWidth + (DesignLanguage is DesignLanguage.Fluent ? item.Width : realMinimumItemWidth);

                if (newTotalWidth > Width)
                    break;

                changed = true;
                totalWidth = newTotalWidth;
                stackLayout.Children.Add(item);
                hiddenItems.Remove(item);
            }

            // Show more button or rest of the already hidden items
            if (hiddenItems.Any() && ShowButtonAndMenuWhenMoreItemsDoNotFit)
            {
                var restItemsWidth = DesignLanguage is DesignLanguage.Fluent ? hiddenItems.Sum(c => (c as View).Width) : hiddenItems.Count * realMinimumItemWidth;

                if (restItemsWidth <= Width - (totalWidth - moreButtonWidth))
                {
                    foreach (var item in hiddenItems)
                        stackLayout.Children.Add(item);
                    hiddenItems.Clear();
                }
                else
                {
                    addMoreButton = true;
                }
            }
        }

        private void FindNewHiddenItems(ref bool changed, ref double totalWidth, List<Grid> hidden, double currentItemWidth)
        {
            bool remove = false;

            for (int i = 0; i < stackLayout.Children.Count; i++)
            {
                var view = stackLayout.Children[i] as Grid;
                totalWidth += DesignLanguage is DesignLanguage.Fluent ? view.Width : currentItemWidth;

                if (remove)
                {
                    changed = true;
                    hidden.Add(view);
                }
                else if (totalWidth > Width)
                {
                    remove = true;
                    hidden.Add(view);
                }
            }
        }

        #region Event callbacks

        private void ItemButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            ItemSelected?.Invoke(sender, new TabItemSelectedEventArgs
            {
                ShellItem = button.BindingContext as BaseShellItem
            });
        }

        private void TabBarSizeChanged(object sender, EventArgs e)
        {
            UpdateValuesAccordingToLanguage();
        }

        private void TabBarHandlerChanged(object sender, EventArgs e)
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

        private void TabBarUnloaded(object sender, EventArgs e)
        {
            Unloaded -= TabBarUnloaded;
            SizeChanged -= TabBarSizeChanged;
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

        private static async void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabBar = bindable as TabBar;

            if (tabBar.IsScrollable && tabBar.stackLayout is not null && tabBar.scrollView is not null)
            {
                var element = tabBar.stackLayout.Children.Cast<Element>().FirstOrDefault(e => e.BindingContext == newValue);

                if (element is not null)
                    _ = tabBar.scrollView.ScrollToAsync(element, ScrollToPosition.MakeVisible, true);
            }

            if (tabBar.DesignLanguage is DesignLanguage.Fluent)
                await tabBar.AnimateFluentToSelected();

            tabBar.UpdateValuesAccordingToLanguage();
        }

        private void ShellItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != BaseShellItem.TitleProperty.PropertyName && e.PropertyName != BaseShellItem.IconProperty.PropertyName)
                return;
            
            if (stackLayout is null)
                return;

            var shellItem = sender as BaseShellItem;

            foreach (var item in allItemViews)
            {
                var stackLayout = item.Children[0] as StackLayout;
                var image = stackLayout.Children[0] as Icon;
                var label = stackLayout.Children[1] as Label;

                label.Text = shellItem.Title;
                image.Source = shellItem.Icon;

                image.TintColor = IsSelected(item) ? IconSelectionColor : IconColor;
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

            foreach (var item in tabBar.allItemViews)
            {
                item.WidthRequest = (double)newValue;
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

            foreach (var item in tabBar.allItemViews)
            {
                if (tabBar.IsSelected(item))
                    continue;

                var stackLayout = item.Children[0] as StackLayout;
                var image = stackLayout.Children[0] as Icon;

                if (newValue is not null)
                    image.TintColor = newValue as Color;
            }
        }

        private static void OnIconSelectionColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabBar = bindable as TabBar;

            if (tabBar.stackLayout is null)
                return;

            foreach (var item in tabBar.allItemViews)
            {
                if (!tabBar.IsSelected(item))
                    continue;

                var stackLayout = item.Children[0] as StackLayout;
                var image = stackLayout.Children[0] as Icon;

                if (newValue is not null)
                    image.TintColor = newValue as Color;
            }
        }

        private static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabBar = bindable as TabBar;

            if (tabBar.stackLayout is null)
                return;

            foreach (var item in tabBar.allItemViews)
            {
                if (tabBar.IsSelected(item))
                    continue;

                var stackLayout = item.Children[0] as StackLayout;
                var label = stackLayout.Children[1] as Label;

                label.TextColor = newValue as Color;
            }
        }

        private static void OnTextSelectionColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabBar = bindable as TabBar;

            if (tabBar.stackLayout is null)
                return;

            foreach (var item in tabBar.allItemViews)
            {
                if (!tabBar.IsSelected(item))
                    continue;

                var stackLayout = item.Children[0] as StackLayout;
                var label = stackLayout.Children[1] as Label;

                label.TextColor = newValue as Color;
            }
        }

        private static void OnPrimaryBrushChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabBar = bindable as TabBar;
            tabBar.InvalidateGraphicsView();
        }

        private static void OnShowButtonAndMenuWhenMoreItemsDoNotFitChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabBar = bindable as TabBar;
            tabBar.UpdateMoreButton();
        }

        private static void OnMoreTitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabBar = bindable as TabBar;
            tabBar.UpdateMoreButton();
        }

        private static void OnMoreIconChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var tabBar = bindable as TabBar;
            tabBar.UpdateMoreButton();
        }

        #endregion
    }
}
