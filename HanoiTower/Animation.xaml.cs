using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace HanoiTower;

public partial class Animation 
{
    private const double speed= 2*0.35;
    readonly int _countOfRings;
    readonly List<Tuple<int, int>> _motion = new();
    
    public Animation(Helper help)
    {
        InitializeComponent();
        _countOfRings = help.RingsCount;
        Start();
    }
    private async void Start()
    {
        CreateArea();
        HanoiTower(_countOfRings);
        foreach(var tuple in _motion)
        {
            await Move(tuple.Item1, tuple.Item2);
        }
    }
    private void HanoiTower(int n, int from=0, int to=1, int dest=2)
    {
        if (n <= 0) return;
        
        HanoiTower(n - 1, from, dest, to);
        
        _motion.Add(new Tuple<int, int>(from, to));
        
        HanoiTower(n - 1, dest, to, from);
    }
    private void Anima(Rectangle rec, int to, DoubleAnimation leftAnimation, DoubleAnimation bottomAnimation)
    { 
        leftAnimation.From = Canvas.GetLeft(rec);
        bottomAnimation.From = Canvas.GetBottom(rec);

        switch (to)
        {
            case 0:
                leftAnimation.To = Canvas.GetLeft(Column1) + ((Column1.Width / 2) - (rec.Width / 2));
                bottomAnimation.To = Canvas.GetBottom(Column1) + (Column1.Children.Count * Helper.RingHeight);
                break;
            case 1:
                leftAnimation.To = Canvas.GetLeft(Column2) + ((Column2.Width / 2) - rec.Width / 2);
                bottomAnimation.To = Canvas.GetBottom(Column1) + (Column2.Children.Count * Helper.RingHeight);
                break;
            case 2:
                leftAnimation.To = Canvas.GetLeft(Column3) + (Column3.Width / 2 - rec.Width / 2);
                bottomAnimation.To = Canvas.GetBottom(Column1) + (Column3.Children.Count * Helper.RingHeight);
                break;
        }
        leftAnimation.Duration = TimeSpan.FromSeconds(speed);
        bottomAnimation.Duration = TimeSpan.FromSeconds(speed);
    }

    private void RectangleCopy(Rectangle source, Rectangle copy, int sourceCol)
    {
        copy.Fill = source.Fill;
        copy.Width = source.Width;
        copy.Height = source.Height;
        
        switch (sourceCol)
        {
            case 0:
                Canvas.SetLeft(copy, Canvas.GetLeft(source) + Canvas.GetLeft(Column1));
                Canvas.SetBottom(copy, Canvas.GetBottom(source) + Canvas.GetBottom(Column1));
                break;
            case 1:
                Canvas.SetLeft(copy, Canvas.GetLeft(source) + Canvas.GetLeft(Column2));
                Canvas.SetBottom(copy, Canvas.GetBottom(source) + Canvas.GetBottom(Column2));
                break;
            case 2:
                Canvas.SetLeft(copy, Canvas.GetLeft(source) + Canvas.GetLeft(Column3));
                Canvas.SetBottom(copy, Canvas.GetBottom(source) + Canvas.GetBottom(Column3));
                break;
        }
    }
    
    private async Task Move(int from,int to)
    {
        Canvas fromCol = from switch
        {
            0 => Column1,
            1 => Column2,
            2 => Column3,
            _ => Column1
        };
        Canvas toCol = to switch
        {
            0 => Column1,
            1 => Column2,
            2 => Column3,
            _ => Column1
        };
        DoubleAnimation leftAnimation = new DoubleAnimation();
        DoubleAnimation bottomAnimation = new DoubleAnimation();

        Rectangle copy = new Rectangle();
        Rectangle r = (Rectangle)fromCol.Children[^1];
            
        RectangleCopy(r, copy, from);
        Anima(copy, to, leftAnimation, bottomAnimation);
        fromCol.Children.Remove(r);
        MainCanvas.Children.Add(copy);
        copy.BeginAnimation(Canvas.LeftProperty,leftAnimation);
        copy.BeginAnimation(Canvas.BottomProperty, bottomAnimation);
        Canvas.SetBottom(r, toCol.Children.Count * Helper.RingHeight);
        await Task.Delay((int)(speed*1000));
        toCol.Children.Add(r);
        MainCanvas.Children.Remove(copy);
    }
    
    private void CreateArea()
    {
        Column1.Children.Clear();
        Column2.Children.Clear();
        Column3.Children.Clear();

        int ringWidth = Helper.RingMinWidth;
        for (int i = 0; i < _countOfRings; i++)
        {
            Rectangle r = new Rectangle
            {
                Width = ringWidth - i * (Helper.Difference),
                Height = Helper.RingHeight,
                Fill = Helper.ColorBrash(Helper.Colors.ColorsList[i])
            };
            Canvas.SetLeft(r, 120 - r.Width / 2);
            Canvas.SetBottom(r, r.Height *i);
            Column1.Children.Add(r);
        }
    }
    
}