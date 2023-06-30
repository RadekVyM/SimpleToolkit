using SimpleToolkit.Core;
using SimpleToolkit.SimpleShell.Extensions;
using SimpleToolkit.SimpleShell.Transitions;

namespace SimpleToolkit.SimpleShell.Playground.Views.Pages;

public partial class ImageDetailPage : ContentPage, IQueryAttributable
{
    private Rect imageBounds;
    private Rect transitionImageBounds;
    private Func<double, double> setOriginImageOpacity;

    public Rect InitialImageTransitionRect { get; private set; }


    public ImageDetailPage()
	{
		InitializeComponent();

        this.SetTransition(new SimpleShellTransition(
            callback: static args => {
                if (args.DestinationPage is not ImageDetailPage page)
                    return;

                page.imageBounds = page.image.GetBounds(page.transitionLayer);

                var rect = new Rect(
                    page.transitionImageBounds.X + (page.imageBounds.X - page.transitionImageBounds.X) * args.Progress,
                    page.transitionImageBounds.Y + (page.imageBounds.Y - page.transitionImageBounds.Y) * args.Progress,
                    page.transitionImageBounds.Width + (page.imageBounds.Width - page.transitionImageBounds.Width) * args.Progress,
                    page.transitionImageBounds.Height + (page.imageBounds.Height - page.transitionImageBounds.Height) * args.Progress);

                System.Diagnostics.Debug.WriteLine(rect);

                page.rootContainer.BackgroundColor = Color.FromRgba(0, 0, 0, args.Progress);
                AbsoluteLayout.SetLayoutBounds(page.transitionImage, rect);
            },
            starting: static args =>
            {
                if (args.DestinationPage is not ImageDetailPage page)
                    return;

                page.rootContainer.BackgroundColor = Color.FromRgba(0, 0, 0, 0);
                page.image.Opacity = 0.01;
                page.transitionImage.Opacity = 1;
                page.SetInitialTransitionImagePosition();
                page.setOriginImageOpacity?.Invoke(0.01);
            },
            finished: static args => {
                if (args.DestinationPage is not ImageDetailPage page)
                    return;

                page.image.Opacity = 1;
                page.transitionImage.Opacity = 0.01;
                page.setOriginImageOpacity?.Invoke(1);
            },
            duration: static args => 300u,
            easing: static args => Easing.SpringOut)
            .CombinedWith(
                transition: SimpleShell.Current.GetTransition(),
                when: static args => args.TransitionType != SimpleShellTransitionType.Pushing));
    }


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
		var imageSource = query["ImageSource"] as ImageSource;
        InitialImageTransitionRect = (Rect)query["ImageRect"];
        setOriginImageOpacity = query["SetImageOpacity"] as Func<double, double>;

        image.Source = imageSource;
		transitionImage.Source = imageSource;
    }

	private void SetInitialTransitionImagePosition()
    {
        var layoutBounds = transitionLayer.GetBounds();
        transitionImageBounds = new Rect(
            InitialImageTransitionRect.X - layoutBounds.X,
            InitialImageTransitionRect.Y - layoutBounds.Y,
            InitialImageTransitionRect.Width,
            InitialImageTransitionRect.Height);

        AbsoluteLayout.SetLayoutBounds(
            transitionImage,
            transitionImageBounds);
    }
}
