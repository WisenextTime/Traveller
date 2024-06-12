using ColdMint.scripts.utils;
using Godot;

namespace ColdMint.scripts.damage;

/// <summary>
/// <para>Node representing the damage number</para>
/// <para>表示伤害数字的节点</para>
/// </summary>
public partial class DamageNumberNodeSpawn : Marker2D
{
    private PackedScene? _damageNumberPackedScene;
    private Node2D? _rootNode;

    /// <summary>
    /// <para>The vector in the negative direction</para>
    /// <para>负方向的向量</para>
    /// </summary>
    private Vector2 _negativeVector;

    /// <summary>
    /// <para>Vector in the positive direction</para>
    /// <para>正方向的向量</para>
    /// </summary>
    private Vector2 _positiveVector;

    /// <summary>
    /// <para>物理渐变色</para>
    /// <para>physical Gradient</para>
    /// </summary>
    private Gradient? _physicalGradient;

    /// <summary>
    /// <para>魔法渐变色</para>
    /// <para>magic Gradient</para>
    /// </summary>
    private Gradient? _magicGradient;

    /// <summary>
    /// <para>默认渐变色</para>
    /// <para>default Gradient</para>
    /// </summary>
    private Gradient? _defaultGradient;


    public override void _Ready()
    {
        base._Ready();
        _damageNumberPackedScene = GD.Load("res://prefab/ui/DamageNumber.tscn") as PackedScene;
        _rootNode = GetNode<Node2D>("/root/Game/DamageNumberContainer");
        //The horizontal velocity is in the X positive direction
        //水平速度的X正方向
        var horizontalVelocityPositiveDirection = Config.CellSize * Config.HorizontalSpeedOfDamageNumbers;
        //Horizontal velocity in the negative X direction
        //水平速度的X负方向
        var horizontalVelocityNegativeDirection = -horizontalVelocityPositiveDirection;
        //vertical height
        //垂直高度
        var verticalHeight = -Config.CellSize * Config.VerticalVelocityOfDamageNumbers;
        //Compute left and right vectors
        //计算左右向量
        _negativeVector = new Vector2(horizontalVelocityNegativeDirection, verticalHeight);
        _positiveVector = new Vector2(horizontalVelocityPositiveDirection, verticalHeight);
        _physicalGradient = new Gradient();
        //Physical color from OpenColor2 to OpenColor6 (red)
        //物理色 从OpenColor2 到 OpenColor6（红色）
        _physicalGradient.SetColor(0, new Color("#ffc9c9"));
        _physicalGradient.SetColor(1, new Color("#fa5252"));
        _magicGradient = new Gradient();
        //Magic Color from OpenColor2 to OpenColor6(Purple)
        //魔法色 从OpenColor2 到 OpenColor6(紫色)
        _magicGradient.SetColor(0, new Color("#d0bfff"));
        _magicGradient.SetColor(1, new Color("#7950f2"));
        _defaultGradient = new Gradient();
        //default behavior
        //默认行为
        _defaultGradient.SetColor(0, new Color("#ff8787"));
        _defaultGradient.SetColor(1, new Color("#fa5252"));
    }

    /// <summary>
    /// <para>Added a damage digital node</para>
    /// <para>添加伤害数字节点</para>
    /// </summary>
    /// <param name="damageNumber"></param>
    private void AddDamageNumberNode(Node2D damageNumber)
    {
        _rootNode?.AddChild(damageNumber);
    }

    /// <summary>
    /// <para>Show damage</para>
    /// <para>显示伤害</para>
    /// </summary>
    /// <param name="damageTemplate"></param>
    public void Display(DamageTemplate damageTemplate)
    {
        if (_rootNode == null || _damageNumberPackedScene == null)
        {
            return;
        }

        var damageNumber = NodeUtils.InstantiatePackedScene<DamageNumber>(_damageNumberPackedScene);
        if (damageNumber == null)
        {
            return;
        }

        CallDeferred("AddDamageNumberNode", damageNumber);
        damageNumber.Position = GlobalPosition;
        if (damageTemplate.MoveLeft)
        {
            damageNumber.SetVelocity(_negativeVector);
        }
        else
        {
            damageNumber.SetVelocity(_positiveVector);
        }

        var damageLabel = damageNumber.GetNode<Label>("Label");
        if (damageLabel == null)
        {
            return;
        }

        damageLabel.Text = damageTemplate.Damage.ToString();
        var labelSettings = new LabelSettings();
        var offset = damageTemplate.Damage / (float)damageTemplate.MaxDamage;
        var gradient = GetDamageColorByType(damageTemplate.Type);
        if (gradient != null)
        {
            labelSettings.FontColor = gradient.Sample(offset);
        }

        if (damageTemplate.IsCriticalStrike)
        {
            labelSettings.FontSize = Config.CritDamageTextSize;
        }
        else
        {
            labelSettings.FontSize = Config.NormalDamageTextSize;
        }

        damageLabel.LabelSettings = labelSettings;
        damageLabel.Position = Vector2.Zero;
    }


    /// <summary>
    /// <para>Gets text color based on damage type</para>
    /// <para>根据伤害类型获取文本颜色</para>
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private Gradient? GetDamageColorByType(int type)
    {
        return type switch
        {
            Config.DamageType.Physical => _physicalGradient,
            Config.DamageType.Magic => _magicGradient,
            _ => _defaultGradient
        };
    }
}