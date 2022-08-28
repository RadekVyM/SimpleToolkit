using Microsoft.Maui.Controls.Shapes;

namespace Radek.SimpleShell.Controls
{
    // TODO: A11y

    [ContentProperty(nameof(Items))]
    public partial class ListPopover : Popover
    {
        private Border rootBorder;
        private Grid rootGrid;
        private GraphicsView graphicsView;
        private ScrollView scrollView;
        private VerticalStackLayout stackLayout;
        private IEnumerable<ListItem> listItems = new List<ListItem>();
        private IList<Grid> allItemViews = new List<Grid>();
        private double fontSize;
        private double containerCornerRadius;
        private double listItemHeight;
        private Size iconSize;
        private Thickness iconMargin;
        private Thickness labelMargin;
        private Thickness stackLayoutPadding;
        private bool iconsAreVisible = true;
        private double scrollPosition;

        public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IEnumerable<object>), typeof(ListPopover), propertyChanged: OnItemsChanged);
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(ListPopover), propertyChanged: OnSelectedItemChanged);
        public static readonly BindableProperty DesignLanguageProperty = BindableProperty.Create(nameof(DesignLanguage), typeof(DesignLanguage), typeof(ListPopover), propertyChanged: OnDesignLanguageChanged, defaultValue: DesignLanguage.Material3);
        public static readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(ListPopover), propertyChanged: OnIconColorChanged, defaultValue: Colors.Black);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ListPopover), propertyChanged: OnTextColorChanged, defaultValue: Colors.Black);
        public static readonly BindableProperty BackgroundProperty = BindableProperty.Create(nameof(Background), typeof(Brush), typeof(ListPopover), propertyChanged: OnBackgroundChanged, defaultValue: null);
        public static readonly BindableProperty SelectionBrushProperty = BindableProperty.Create(nameof(SelectionBrush), typeof(Brush), typeof(ListPopover), propertyChanged: OnSelectionBrushChanged, defaultValue: null);
        public static readonly BindableProperty MinimumWidthRequestProperty = BindableProperty.Create(nameof(MinimumWidthRequest), typeof(double), typeof(ListPopover), propertyChanged: OnMinimumWidthRequestChanged, defaultValue: View.MinimumWidthRequestProperty.DefaultValue);
        public static readonly BindableProperty MaximumWidthRequestProperty = BindableProperty.Create(nameof(MaximumWidthRequest), typeof(double), typeof(ListPopover), propertyChanged: OnMaximumWidthRequestChanged, defaultValue: View.MaximumWidthRequestProperty.DefaultValue);

        public event ListPopoverItemSelectedEventHandler ItemSelected;

        public virtual IEnumerable<object> Items
        {
            get => (IEnumerable<object>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public virtual object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public virtual DesignLanguage DesignLanguage
        {
            get => (DesignLanguage)GetValue(DesignLanguageProperty);
            set => SetValue(DesignLanguageProperty, value);
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

        public virtual Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public virtual Brush SelectionBrush
        {
            get => (Brush)GetValue(SelectionBrushProperty);
            set => SetValue(SelectionBrushProperty, value);
        }

        public virtual double MinimumWidthRequest
        {
            get => (double)GetValue(MinimumWidthRequestProperty);
            set => SetValue(MinimumWidthRequestProperty, value);
        }

        public virtual double MaximumWidthRequest
        {
            get => (double)GetValue(MaximumWidthRequestProperty);
            set => SetValue(MaximumWidthRequestProperty, value);
        }


        public ListPopover() : base()
        {
            SetUpBaseStructure();
            HandlerChanged += ListPopoverHandlerChanged;
        }


        public override void Show(View parentView)
        {
            if (Items?.Any() == true)
            {
                base.Show(parentView);
            }
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

        private bool IsSelected(object item)
        {
            return item == SelectedItem || (item is BindableObject bindableObject && bindableObject.BindingContext == item);
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

        private void SetUpBaseStructure()
        {
            rootBorder = new Border
            {
                StrokeThickness = 0,
                Stroke = Colors.Transparent,
                Background = Background,
                Shadow = null,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = containerCornerRadius
                },
                Style = new Style(typeof(Border))
            };
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
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Style = new Style(typeof(ScrollView))
            };
            stackLayout = new VerticalStackLayout
            {
                Padding = stackLayoutPadding,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
                Style = new Style(typeof(VerticalStackLayout))
            };

            rootBorder.Content = rootGrid;
            rootGrid.Children.Add(graphicsView);
            rootGrid.Children.Add(scrollView);
            scrollView.Content = stackLayout;

            scrollView.Scrolled += ScrollViewScrolled;

            CompressedLayout.SetIsHeadless(rootGrid, true);
            CompressedLayout.SetIsHeadless(stackLayout, true);

            Content = rootBorder;
        }

        private IEnumerable<Grid> CreateItemViews(IEnumerable<ListItem> listItems)
        {
            foreach (var item in listItems)
            {
                yield return CreateButton(item);
            }
        }

        private Grid CreateButton(ListItem item)
        {
            // I have to do weird stuff here to have sizing sort of working - layouts are just broken:
            // https://github.com/dotnet/maui/issues/7531
            
            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection(
                    new ColumnDefinition(GridLength.Auto)),
                Background = Colors.Transparent,
                HorizontalOptions = LayoutOptions.Fill,
                Style = new Style(typeof(Grid)),
                BindingContext = item,
            };

            var innerGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection(
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Auto)),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Style = new Style(typeof(Grid)),
                BindingContext = item,
            };

            var label = new Label
            {
                Text = item.Title,
                TextColor = TextColor,
                FontSize = fontSize,
                LineBreakMode = LineBreakMode.TailTruncation,
                HorizontalOptions = LayoutOptions.Fill,
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
                TintColor = IconColor,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Style = new Style(typeof(Icon)),
                BindingContext = item
            };

            innerGrid.Children.Add(image);
            innerGrid.Children.Add(label);

            Grid.SetColumn(image, 0);
            Grid.SetColumn(label, 1);

            grid.Children.Add(innerGrid);

            //CompressedLayout.SetIsHeadless(grid, true);
            CompressedLayout.SetIsHeadless(innerGrid, true);

            return grid;
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

        private void UpdateControls()
        {
            if (rootBorder is null || stackLayout is null)
                return;

            rootBorder.Background = Background;
            rootBorder.StrokeShape = new RoundRectangle
            {
                CornerRadius = containerCornerRadius
            };
            stackLayout.Padding = stackLayoutPadding;

            foreach (var item in allItemViews)
            {
                UpdateButton(item);
            }

            InvalidateGraphicsView();
        }

        private void UpdateButton(Grid item)
        {
            item.HeightRequest = listItemHeight;

            var innerGrid = item.Children[0] as Grid;
            var image = innerGrid.Children[0] as Icon;
            var label = innerGrid.Children[1] as Label;

            var listItem = item.BindingContext as ListItem;

            label.TextColor = TextColor;
            label.FontSize = fontSize;
            if (label.Margin != labelMargin)
                label.Margin = labelMargin;

            image.IsVisible = iconsAreVisible;
            image.Source = listItem.Icon;
            image.HeightRequest = iconSize.Height;
            image.WidthRequest = iconSize.Width;
            if (image.Margin != iconMargin)
                image.Margin = iconMargin;
            image.TintColor = IconColor;
        }

        private void UpdateButtons(IEnumerable<ListItem> items)
        {
            if (stackLayout is null || items is null)
                return;

            var itemViews = CreateItemViews(items);

            foreach (var view in allItemViews)
            {
                view.GestureRecognizers.Clear();
            }

            stackLayout.Children.Clear();
            allItemViews = itemViews.ToList();

            foreach (var view in allItemViews)
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += ItemClicked;

                view.GestureRecognizers.Add(tapGestureRecognizer);

                stackLayout.Children.Add(view);
            }
        }

        private void UpdateIconsVisibility(IEnumerable<ListItem> items)
        {
            var newVisibility = !items.All(i => i.Icon is null);

            if (iconsAreVisible == newVisibility)
                return;

            iconsAreVisible = newVisibility;

            UpdateControls();
        }

        private IEnumerable<ListItem> ItemsToListItems(IEnumerable<object> items)
        {
            foreach (var item in items)
            {
                var titleIcon = GetTitleIconFromItem(item);

                yield return new ListItem(titleIcon.Title, titleIcon.Icon, item);
            }
        }

        private (string Title, ImageSource Icon) GetTitleIconFromItem(object item)
        {
            string title = item switch
            {
                BaseShellItem shellItem => shellItem.Title,
                MenuItem menuItem => menuItem.Text,
                _ => item?.ToString()
            };
            ImageSource icon = item switch
            {
                BaseShellItem shellItem => shellItem.Icon,
                MenuItem menuItem => menuItem.IconImageSource,
                _ => null
            };

            return new(title, icon);
        }

        private void ListPopoverHandlerChanged(object sender, EventArgs e)
        {
            UpdateButtons(listItems);
            UpdateValuesAccordingToLanguage();
        }

        private void ScrollViewScrolled(object sender, ScrolledEventArgs e)
        {
            scrollPosition = e.ScrollX;
            InvalidateGraphicsView();
        }

        private void ItemClicked(object sender, EventArgs e)
        {
            var item = sender as Grid;

            ItemSelected?.Invoke(sender, new ListPopoverItemSelectedEventArgs
            {
                Item = (item.BindingContext as ListItem).Item
            });
        }

        private void ItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ((e.PropertyName != BaseShellItem.TitleProperty.PropertyName && e.PropertyName != BaseShellItem.IconProperty.PropertyName) ||
                (e.PropertyName != MenuItem.TextProperty.PropertyName && e.PropertyName != MenuItem.IconImageSourceProperty.PropertyName))
                return;

            if (stackLayout is null)
                return;

            var titleIcon = GetTitleIconFromItem(sender);

            foreach (var item in allItemViews)
            {
                var grid = item.Children[0] as Grid;
                var image = grid.Children[0] as Icon;
                var label = grid.Children[1] as Label;

                label.Text = titleIcon.Title;
                image.Source = titleIcon.Icon;
            }

            UpdateIconsVisibility(listItems);
        }

        private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var listPopover = bindable as ListPopover;

            if (oldValue is not null)
            {
                var oldItems = listPopover.ItemsToListItems(oldValue as IEnumerable<object>);

                foreach (var item in oldItems)
                {
                    if (item.Item is BindableObject bindableObject)
                        bindableObject.PropertyChanged -= listPopover.ItemPropertyChanged;
                }
            }
            if (newValue is not null)
            {
                listPopover.listItems = listPopover.ItemsToListItems(newValue as IEnumerable<object>).ToList();

                foreach (var item in listPopover.listItems)
                {
                    if (item.Item is BindableObject bindableObject)
                        bindableObject.PropertyChanged += listPopover.ItemPropertyChanged;
                }
            }

            listPopover.UpdateButtons(listPopover.listItems);
            listPopover.UpdateValuesAccordingToLanguage();
            listPopover.UpdateIconsVisibility(listPopover.listItems);
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var listPopover = bindable as ListPopover;

            listPopover.UpdateValuesAccordingToLanguage();
        }

        private static void OnDesignLanguageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var listPopover = bindable as ListPopover;
            listPopover.UpdateValuesAccordingToLanguage();
        }

        private static void OnIconColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var listPopover = bindable as ListPopover;

            if (listPopover.stackLayout is null)
                return;

            foreach (var item in listPopover.allItemViews)
            {
                var grid = item.Children[0] as Grid;
                var image = grid.Children[0] as Icon;

                if (newValue is not null)
                    image.TintColor = newValue as Color;
            }
        }

        private static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var listPopover = bindable as ListPopover;

            if (listPopover.stackLayout is null)
                return;

            foreach (var item in listPopover.allItemViews)
            {
                var grid = item.Children[0] as Grid;
                var label = grid.Children[1] as Label;

                label.TextColor = newValue as Color;
            }
        }

        private static void OnBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var listPopover = bindable as ListPopover;
            listPopover.rootBorder.Background = newValue as Brush;
            listPopover.InvalidateGraphicsView();
        }

        private static void OnSelectionBrushChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var listPopover = bindable as ListPopover;
            listPopover.InvalidateGraphicsView();
        }

        private static void OnMinimumWidthRequestChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var listPopover = bindable as ListPopover;
            listPopover.scrollView.MinimumWidthRequest = (double)newValue;
            listPopover.stackLayout.MinimumWidthRequest = (double)newValue;
            listPopover.InvalidateGraphicsView();
        }

        private static void OnMaximumWidthRequestChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var listPopover = bindable as ListPopover;
            listPopover.scrollView.MaximumWidthRequest = (double)newValue;
            listPopover.stackLayout.MaximumWidthRequest = (double)newValue;
            listPopover.InvalidateGraphicsView();
        }

        private record ListItem(string Title, ImageSource Icon, object Item);
    }
}
