using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Controls;

namespace DirN.UI.NodeExpander
{
    public class NodeExpander:System.Windows.Controls.HeaderedContentControl
    {
        /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon),
            typeof(IconElement),
            typeof(NodeExpander),
            new PropertyMetadata(null, null, IconElement.Coerce)
        );

        /// <summary>Identifies the <see cref="CornerRadius"/> dependency property.</summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(NodeExpander),
            new PropertyMetadata(new CornerRadius(4))
        );

        /// <summary>Identifies the <see cref="ContentPadding"/> dependency property.</summary>
        public static readonly DependencyProperty ContentPaddingProperty = DependencyProperty.Register(
            nameof(ContentPadding),
            typeof(Thickness),
            typeof(NodeExpander),
            new FrameworkPropertyMetadata(
                default(Thickness),
                FrameworkPropertyMetadataOptions.AffectsParentMeasure
            )
        );

        /// <summary>
        /// Gets or sets displayed <see cref="IconElement"/>.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        public IconElement? Icon
        {
            get => (IconElement?)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }


        [Bindable(true)]
        [Category("Appearance")]
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// Gets or sets content padding Property
        /// </summary>
        [Bindable(true)]
        [Category("Layout")]
        public Thickness ContentPadding
        {
            get { return (Thickness)GetValue(ContentPaddingProperty); }
            set { SetValue(ContentPaddingProperty, value); }
        }
    }
}
