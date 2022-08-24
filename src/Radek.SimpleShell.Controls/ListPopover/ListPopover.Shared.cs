using Microsoft.Maui.Controls;

namespace Radek.SimpleShell.Controls
{
    public partial class ListPopover : Popover
    {
        private Border rootBorder;
        private VerticalStackLayout stackLayout;
        private IEnumerable<ListItem> listItems = new List<ListItem>();
        private IList<Grid> allItemViews = new List<Grid>();
        private double fontSize;
        private Size iconSize;
        private Thickness iconMargin;
        private Thickness labelMargin;
        private bool iconsAreVisible = true;

        public static readonly BindableProperty ItemsProperty = BindableProperty.Create(nameof(Items), typeof(IEnumerable<object>), typeof(ListPopover), propertyChanged: OnItemsChanged);
        public static readonly BindableProperty DesignLanguageProperty = BindableProperty.Create(nameof(DesignLanguage), typeof(DesignLanguage), typeof(ListPopover), propertyChanged: OnDesignLanguageChanged, defaultValue: DesignLanguage.Material3);
        public static readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(ListPopover), propertyChanged: OnIconColorChanged, defaultValue: Colors.Black);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ListPopover), propertyChanged: OnTextColorChanged, defaultValue: Colors.Black);
        public static readonly BindableProperty BackgroundProperty = BindableProperty.Create(nameof(Background), typeof(Brush), typeof(ListPopover), propertyChanged: OnBackgroundChanged, defaultValue: null);

        public event TabItemSelectedEventHandler ItemSelected;

        public virtual IEnumerable<object> Items
        {
            get => (IEnumerable<object>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
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


        public ListPopover() : base()
        {
            SetUpBaseStructure();
            HandlerChanged += ListPopoverHandlerChanged;
        }


        private void SetUpBaseStructure()
        {
            rootBorder = new Border
            {
                StrokeThickness = 0,
                Stroke = Colors.Transparent,
                Background = Background,
                Shadow = null,
                Style = new Style(typeof(Border))
            };
            stackLayout = new VerticalStackLayout
            {
                Style = new Style(typeof(VerticalStackLayout))
            };

            rootBorder.Content = stackLayout;

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
            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection(
                    new ColumnDefinition(new GridLength(0)),
                    new ColumnDefinition(GridLength.Auto)),
                Style = new Style(typeof(Grid)),
                BindingContext = item,
            };
            var button = new Button
            {
                Padding = new Thickness(0),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BackgroundColor = Colors.Transparent,
                BorderWidth = 0,
                CornerRadius = 0,
                Style = new Style(typeof(Button)),
                BindingContext = item
            };

            var label = new Label
            {
                Text = item.Title,
                TextColor = TextColor,
                FontSize = fontSize,
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
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TintColor = IconColor,
                Style = new Style(typeof(Icon)),
                BindingContext = item
            };

            grid.Children.Add(image);
            grid.Children.Add(label);

            grid.Children.Add(button);

            grid.SizeChanged += GridSizeChanged;

            Grid.SetColumn(image, 0);
            Grid.SetColumn(label, 1);
            Grid.SetColumnSpan(button, 3);

            static void GridSizeChanged(object sender, EventArgs e)
            {
                var grid = sender as Grid;
                var button = grid.Children[2] as Button;

                button.WidthRequest = grid.Width;
            }

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

            foreach (var item in allItemViews)
            {
                UpdateButton(item);
            }
        }

        private void UpdateButton(Grid item)
        {
            item.ColumnDefinitions = new ColumnDefinitionCollection(
                new ColumnDefinition(iconsAreVisible ? new GridLength(iconSize.Width + iconMargin.HorizontalThickness) : new GridLength(0)),
                new ColumnDefinition(GridLength.Auto));

            var image = item.Children[0] as Icon;
            var label = item.Children[1] as Label;

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
                var button = view.Children[2] as Button;

                button.Clicked -= ItemButtonClicked;
            }

            stackLayout.Children.Clear();
            allItemViews = itemViews.ToList();

            foreach (var view in allItemViews)
            {
                var button = view.Children[2] as Button;

                button.Clicked += ItemButtonClicked;

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

        private void ItemButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            ItemSelected?.Invoke(sender, new TabItemSelectedEventArgs
            {
                ShellItem = button.BindingContext as BaseShellItem
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
                var image = item.Children[0] as Icon;
                var label = item.Children[1] as Label;

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
                var image = item.Children[0] as Icon;

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
                var label = item.Children[1] as Label;

                label.TextColor = newValue as Color;
            }
        }

        private static void OnBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var listPopover = bindable as ListPopover;
            listPopover.rootBorder.Background = newValue as Brush;
        }

        private record ListItem(string Title, ImageSource Icon, object Item);
    }
}
