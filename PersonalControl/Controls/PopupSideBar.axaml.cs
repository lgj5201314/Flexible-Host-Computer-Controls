using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Layout;

namespace PersonalControl.Controls;

public partial class PopupSideBar : UserControl
{
    /// <summary>
    /// 定义一个依赖属性，可供PopupSideBar选择布局样式
    /// </summary>
    public static readonly StyledProperty<Orientation> OrientationProperty =
        AvaloniaProperty.Register<PopupSideBar, Orientation>(nameof(Orientation), Orientation.Horizontal);

    /// <summary>
    /// 布局方向：Horizontal 底部横向，Vertical 侧边竖向
    /// </summary>
    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    // ✅ 新增：定义两个事件，通知外部显示/隐藏面板
    public event EventHandler? PanelShowRequested;
    public event EventHandler? PanelHideRequested;

    public PopupSideBar()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        // 初始化时根据当前 Orientation 应用对应样式
        UpdateOrientation();

        // 按钮事件只抛自定义事件，不直接操作 AutoHidePanel
        PopupButton.PointerEntered += (s, e) => PanelShowRequested?.Invoke(this, EventArgs.Empty);
        PopupButton.PointerExited += (s, e) => PanelHideRequested?.Invoke(this, EventArgs.Empty);
        PopupButton.PointerPressed += (s, e) => PanelHideRequested?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == OrientationProperty)
        {
            UpdateOrientation();
        }
    }

    /// <summary>
    /// 根据 Orientation 值切换 Button 的样式 Class
    /// </summary>
    private void UpdateOrientation()
    {
        if (PopupButton == null) return;

        var isHorizontal = Orientation == Orientation.Horizontal;
        PopupButton.Classes.Set("horizontal", isHorizontal);
        PopupButton.Classes.Set("vertical", !isHorizontal);
    }

    public void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}